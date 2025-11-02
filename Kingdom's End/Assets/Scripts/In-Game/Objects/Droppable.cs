using System;
using System.Collections;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class Droppable : MonoBehaviour {
  [SerializeField] public bool isIndependent;
  [SerializeField] public string key;
  [SerializeField] public string rarity;
  [SerializeField] public GameObject room;
  [SerializeField] public bool shouldRotate;
  [SerializeField] public string rotateDirection;
  [SerializeField] public string fallingOn;
  [SerializeField] public bool canBePicked = false;
  [SerializeField] public bool isDropping = false;
  [SerializeField] public bool isDropped = false;
  [SerializeField] public bool isIdle = false;
  [SerializeField] public bool isFlickering = false;
  [SerializeField] public int collisionCounter = 0;
  [NonSerialized] MoneyItem moneyItem;
  [NonSerialized] public float timer = 0;
  [NonSerialized] public float maxIdleTime = 10000;
  [NonSerialized] public float maxFlickerTime = 5000;
  [NonSerialized] public Flicker flickerEffect;
  [NonSerialized] SpriteRenderer droppableSprite;
  [NonSerialized] Rigidbody2D body;
  [NonSerialized] Sprite spriteHolder;
  [NonSerialized] CircleCollider2D droppableCollider;
  [SerializeField] public GameObject spawnedFrom;

  private AudioSource audioSource;

  public float parabolaConstant = 0f;
  private Vector2 initialPosition;
  public float speed = 5f;
  public float rotationSpeed = 200f;
  public float bumpDistance = 0.25f;

  public int directionFactor = 0;

  [NonSerialized] public float riseVelocity = 2f;

  public bool isRising = true;

  void Start() {
    flickerEffect = GetComponent<Flicker>();
    droppableSprite = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();

    // check to ensure an independent item is given an image on editor (to ensure it's at ground level)
    if (isIndependent && droppableSprite.sprite.name.Contains("item-placeholder")) {
      throw new Exception($"Independent item {gameObject.name}(parent: {(transform.parent == null ? "Scene" : transform.parent.name)}) should have a predefined image in editor");
    }

    body = gameObject.AddComponent<Rigidbody2D>();
    body.gravityScale = 0;

    directionFactor = rotateDirection == "east" ? 1 : -1;
    parabolaConstant = UnityRandom.Range(0, 3) + 2; // gets a number between 2 and 4, inclusively

    if (rotateDirection != "") {
      isRising = false;
    }

    if (key.Contains("money")) {
      moneyItem = Helpers.GetOrException(Objects.moneyItems, key);

      droppableSprite.sprite = moneyItem.image;
      spriteHolder = moneyItem.image;
    } else {
     droppableSprite.sprite = Helpers.GetOrException(Sprites.droppableSprites, key);
     spriteHolder = Helpers.GetOrException(Sprites.droppableSprites, key);
    }

    droppableCollider = gameObject.AddComponent<CircleCollider2D>();

    // Calculate the bounds of the visible sprite in pixels
    Rect textureRect = droppableSprite.sprite.textureRect;
    float pixelsPerUnit = droppableSprite.sprite.pixelsPerUnit;

    // Determine the actual size in Unity units
    float widthInUnits = textureRect.width / pixelsPerUnit;
    float heightInUnits = textureRect.height / pixelsPerUnit;

    // Find the larger dimension
    float largestDimension = Mathf.Max(widthInUnits, heightInUnits);
    float expectedRadius = largestDimension / 2f;

    // Set the CircleCollider2D's radius (minimum value should be the minimunDroppableColliderRadius)
    droppableCollider.radius = Mathf.Max(expectedRadius, Constants.minimunDroppableColliderRadius);
    droppableCollider.offset = new Vector2(0, Mathf.Max(Constants.minimunDroppableColliderRadius - expectedRadius, 0));

    if (isIndependent) {
      body.gravityScale = 1;
      gameObject.layer = LayerMask.NameToLayer("Dropped");
    } else {
      if (shouldRotate) {
        initialPosition = body.position;
      } else {
        body.freezeRotation = true;
        droppableCollider.isTrigger = false;

        StartCoroutine(RiseAndFall());
      }
    }
  }

  void Update() {
    if (!canBePicked) {
      if (shouldRotate) {
        float horizontalSpeed = speed * directionFactor;
        float time = body.position.x - initialPosition.x;

        // Calculate vertical velocity based on parabolic function f(x) = -a(x - h)^2 + k
        // then multiply by the direction factor
        float verticalSpeed = directionFactor * (-1 * parabolaConstant * Mathf.Pow(2,(time - 1)) + parabolaConstant);

        body.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        float rotationAmount = rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, 0f, rotationAmount * -1 * directionFactor);
      }
    }

    if (flickerEffect != null) {
      if (isIdle) {
        if (Helpers.ExceedsTime(timer, maxIdleTime)) {
          flickerEffect.enabled = true;
          timer = Time.time * 1000;
          isFlickering = true;
          isIdle = false;
        }
      } else if (isFlickering) {
        if (Helpers.ExceedsTime(timer, maxFlickerTime)) {
          // just in case, destroy immediately and don't call the DestroyDroppable function if it triggers any extra actions
          Destroy(gameObject);
        }
      }
    }
  }

  void LateUpdate() {
    if (droppableSprite.sprite == null) {
      droppableSprite.sprite = spriteHolder;
    }
  }

  public void PlaySound(AudioClip droppableSound) {
    if (Hero.instance.pauseCase == "" && Settings.playSFX) {
      audioSource.PlayOneShot(droppableSound);
    }
  }

  private void OnCollisionEnter2D(Collision2D col) {
    string gameObjectTag = col.gameObject.tag;

    if (gameObjectTag == "Floor" || (shouldRotate && gameObjectTag == "Wall") || gameObjectTag == "Interactable") {
      if (shouldRotate) {
        body.gravityScale = 1;
        body.velocity = Vector2.zero;
        body.freezeRotation = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        canBePicked = true;

        // checks if droppables that rotate have crashed against a Wall, and if so bounce a bit away from their direction
        if (gameObjectTag == "Wall") {
          transform.position = new Vector2(transform.position.x + (bumpDistance * -1 * directionFactor), transform.position.y);
        }
      }

      if (!isRising && InGame.instance.IsInRoom(InGame.instance.FindRoom(transform.parent))) {
        PlaySound(Helpers.GetOrException(Helpers.GetOrException(Sounds.fallingSounds, "item"), gameObjectTag == "Interactable" ? "interactable" : fallingOn));
      }

      // destroys the rigid body and makes the collider a trigger so that
      // if the player is overlapping no movement is caused (usually pushing the player up)
      if (gameObjectTag == "Floor" || (gameObjectTag == "Interactable" && !isRising)) {

        // checks if collision is from the bottom, and if so, proceed with logic
        Vector2 normal = col.GetContact(0).normal;

        if (normal.y > 0.5f) {
          if (isRising) {
            // when rising, if the droppable collides with a "ceiling" (floor from bottom), bump it down to prepare it to fall
            transform.position = new Vector2(transform.position.x, transform.position.y - bumpDistance);
            isRising = false;
            canBePicked = true;
            body.gravityScale = 1;
            body.velocity = Vector2.zero;
          } else {
            if (!isIndependent) {
              droppableCollider.isTrigger = true;
              gameObject.layer = LayerMask.NameToLayer("Dropped");
            }
          }
        }
      }

      if (GetComponent<Flicker>() != null) {
        timer = Time.time * 1000;
        isIdle = true;
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    string gameObjectTag = col.gameObject.tag;
    if (gameObjectTag == "Hero" && canBePicked) {
      string itemPickSoundIndex = rarity == "" ? (Helpers.IsValueInArray(Constants.moneyItemKeys, key) ? "money" : "normal") : rarity;

      InGame.instance.PlaySound(Helpers.GetOrException(Sounds.itemPickSounds, itemPickSoundIndex), transform.position);
      DestroyDroppable();
    } else if (gameObjectTag == "Zone") {
      ZoneSpecs currZoneSpecs = Helpers.GetOrException(Objects.zoneSpecs, col.gameObject.GetComponent<Zone>().type);
      fallingOn = currZoneSpecs.groundMaterial;
    }
  }

  public void DestroyDroppable() {
    if (key.Contains("money")) {
      Hero.instance.gold += moneyItem.increment;
    } else {
      Item currItem = Helpers.GetItemFromList(Hero.instance.items, key);

      if (currItem == null) { // if not found, the item must be added
        Hero.instance.items.Add(new Item(key, 1));
      } else { // if found, the item is incremented
        currItem.amount++;
      }
    }

    if (Settings.showItemInfo) {
      bool displayMoney = key.Contains("money");
      InGame.instance.infoCanvas.GetComponent<InfoCanvas>().Display(displayMoney ? moneyItem.text : Helpers.GetOrException(Objects.regularItems, key).name);
    }

    // interaction with enemy spawner happens here if provided
    if (spawnedFrom) {
      //  TODO: consider in what cases it'd be necessary to drop items only once and clear them
      // spawnedFrom.GetComponent<EnemySpawner>().specificDrop = "";
    }

    Destroy(gameObject);
  }

  private IEnumerator RiseAndFall() {
    float duration = 0.5f;
    float elapsedTime = 0f;

    while (elapsedTime < duration) {
      if (!isRising) {
        yield break;
      }

      elapsedTime += Time.deltaTime;
      body.velocity = new Vector2(0, riseVelocity);

      yield return null;
    }

    isRising = false;
    canBePicked = true;
    body.gravityScale = 1;
    body.velocity = Vector2.zero;
  }
}
