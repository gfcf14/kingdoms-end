using System.Collections;
using UnityEngine;

public class Droppable : MonoBehaviour {
  [SerializeField] public string key;
  [SerializeField] public string rarity;
  [SerializeField] public GameObject room;
  [SerializeField] public bool shouldRotate;
  [SerializeField] public string rotateDirection;

  [SerializeField] public bool canBePicked = false;
  [SerializeField] public bool isDropping = false;
  [SerializeField] public bool isDropped = false;
  [SerializeField] public bool isIdle = false;
  [SerializeField] public bool isFlickering = false;
  [SerializeField] public int collisionCounter = 0;
  [System.NonSerialized] MoneyItem moneyItem;
  [System.NonSerialized] public float timer = 0;
  [System.NonSerialized] public float maxIdleTime = 10000;
  [System.NonSerialized] public float maxFlickerTime = 5000;
  [System.NonSerialized] public Flicker flickerEffect;
  [System.NonSerialized] SpriteRenderer droppableSprite;
  [System.NonSerialized] Rigidbody2D body;
  [System.NonSerialized] Sprite spriteHolder;
  [System.NonSerialized] CircleCollider2D droppableCollider;
  [SerializeField] public GameObject spawnedFrom;

  private AudioSource audioSource;
  private InGame inGame;

  public float parabolaConstant = 0f;
  private Vector2 initialPosition;
  public float speed = 5f;
  public float rotationSpeed = 200f;
  public float bumpDistance = 0.25f;

  public int directionFactor = 0;

  [System.NonSerialized] public float riseVelocity = 2f;

  public bool isRising = true;

  void Start() {
    flickerEffect = GetComponent<Flicker>();
    droppableSprite = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    inGame = GameObject.Find("InGame").gameObject.GetComponent<InGame>();

    body = gameObject.AddComponent<Rigidbody2D>();
    body.gravityScale = 0;

    directionFactor = rotateDirection == "east" ? 1 : -1;
    parabolaConstant = Random.Range(0, 3) + 2; // gets a number between 2 and 4, inclusively

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

    // Set the CircleCollider2D's radius (half of the largest dimension)
    droppableCollider.radius = largestDimension / 2f;

    if (shouldRotate) {
      initialPosition = body.position;
    } else {
      body.freezeRotation = true;
      droppableCollider.isTrigger = false;

      StartCoroutine(RiseAndFall());
    }

  }

  void Update() {
    if (!canBePicked) {
      if (shouldRotate) {
        float horizontalSpeed = speed * directionFactor;
        float time = (body.position.x - initialPosition.x);

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
          Destroy(transform.parent.gameObject);
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
    if (inGame.hero.pauseCase == "" && Settings.playSFX) {
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

      if (inGame.IsInRoom(inGame.FindRoom(transform.parent))) {
        string materialFallingOn = inGame.GetTileMaterial(transform.position);
        if (materialFallingOn == null) {
          // TODO: find a better way to get the location
          materialFallingOn = Helpers.GetMaterial(GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>().location);
        }
        PlaySound(Helpers.GetOrException(Sounds.droppableFallingSounds, gameObjectTag == "Interactable" ? "interactable" : materialFallingOn));
      }

      // destroys the rigid body and makes the collider a trigger so that
      // if the player is overlapping no movement is caused (usually pushing the player up)
      if (gameObjectTag == "Floor") {

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
            Destroy(body);
            droppableCollider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Dropped");
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

      inGame.PlaySound(Helpers.GetOrException(Sounds.itemPickSounds, itemPickSoundIndex), transform.position);
      DestroyDroppable(col.gameObject.GetComponent<Hero>());
    }
  }

  public void DestroyDroppable(Hero hero) {
    if (key.Contains("money")) {
      hero.gold += moneyItem.increment;
    } else {
      Item currItem = Helpers.GetItemFromList(hero.items, key);

      if (currItem == null) { // if not found, the item must be added
        hero.items.Add(new Item(key, 1));
      } else { // if found, the item is incremented
        currItem.amount++;
      }
    }

    if (Settings.showItemInfo) {
      bool displayMoney = key.Contains("money");
      hero.infoCanvas.GetComponent<InfoCanvas>().Display(displayMoney ? moneyItem.text : Helpers.GetOrException(Objects.regularItems, key).name);
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
