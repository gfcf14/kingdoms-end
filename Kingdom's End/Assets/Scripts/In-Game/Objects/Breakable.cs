// TIPS FOR BREAKABLES:
// - When stacking, the rows at the bottom must have one less in the order in layer. E.g. if there are six boxes stacked in a triangle, the 3 at the bottom should have order -1, the 2 above them 0, and the one at the top 1

using System;
using System.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour {
  [SerializeField] public string type;
  [SerializeField] public string item;
  [SerializeField] public string itemRarity;
  [SerializeField] public string material;

  [SerializeField] public bool isGrounded;
  [SerializeField] public bool isFalling = false;
  [NonSerialized] public bool isBreaking = false;

  private float soundLength = 0;
  private Animator anim;
  private SpriteRenderer spriteRenderer;
  private Rigidbody2D body;
  private BoxCollider2D breakableCollider;
  private AudioSource audioSource;
  private bool inGameReady = false;
  private string fallingOn = "";

  IEnumerator Start() {
    while (InGame.instance == null) yield return null;

    inGameReady = true;
    SetupBreakable();
  }

  void SetupBreakable() {
    body = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    breakableCollider = GetComponent<BoxCollider2D>();
    audioSource = GetComponent<AudioSource>();
    spriteRenderer.sprite = Helpers.GetOrException(Sprites.breakableSprites, type);

    if (type == "vase") {
      spriteRenderer.color = Helpers.GetOrException(Colors.vaseColors, material == "" ? "brass" : material);
    }

    breakableCollider.offset = Helpers.GetOrException(Objects.breakableSizes, type).offset;
    breakableCollider.size = Helpers.GetOrException(Objects.breakableSizes, type).size;

    gameObject.AddComponent<Animator>();
    anim = GetComponent<Animator>();
    anim.runtimeAnimatorController = Helpers.GetOrException(Objects.animationControllers, "breakable");

    if (item == "") {
      throw new Exception("No item declared for breakable in " + transform.parent.gameObject.name + ". Please declare an item for proper use.");
    }

    // breakables who would not be stackable shouldn't be pushed or player should not stand on them
    if (Helpers.IsValueInArray(Constants.nonStackableBreakables, type)) {
      Destroy(body);
      breakableCollider.isTrigger = true;
    }

    fallingOn = Helpers.GetOrException(Objects.materialsPerArea, GameData.area);
  }

  void Update() {}

  bool CheckIfGrounded() {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y + breakableCollider.offset.y), breakableCollider.size, 0f);

    // if (gameObject.name == "Breakable (3)") {
    //   DrawRectangle(new Vector2(transform.position.x, transform.position.y + breakableCollider.offset.y), breakableCollider.size);
    // }

    foreach(Collider2D currentCollider in colliders) {
      if (currentCollider != breakableCollider) {
        if (currentCollider.gameObject.name == "Floor" || transform.position.y > currentCollider.gameObject.transform.position.y) {
          return true;
        }
      }
    }

    return false;
  }

  private void OnCollisionEnter2D(Collision2D col) {
    string colTag = col.gameObject.tag;

    if (colTag == "Breakable" && Helpers.IsValueInArray(Constants.stackableBreakables, col.gameObject.GetComponent<Breakable>().type) && !Helpers.IsValueInArray(Constants.stackableBreakables, type)) {
      throw new Exception("Breakable objects that are not Barrels or Boxes should not stack with anything");
    }

    if (col.gameObject.name == "ProximityCheck") {
      Physics2D.IgnoreCollision(col.gameObject.GetComponent<PolygonCollider2D>(), breakableCollider);
    }

    if (inGameReady && InGame.instance.IsInRoom(InGame.instance.FindRoom(transform.parent))) {
      isFalling = true;
      StartCoroutine(PrepareFalling(col.gameObject));
    }
  }

  IEnumerator PrepareFalling(GameObject gameObjectUnder) {
    yield return new WaitForSeconds(0.01f);
    PlayFalling(gameObjectUnder);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    string colTag = col.gameObject.tag;

    // since these objects are to go on top of other same objects, change the transform.position z value to render on top
    if (colTag == "Breakable" && !Helpers.IsValueInArray(Constants.stackableBreakables, type)) {
      Destroy(body);
      GetComponent<BoxCollider2D>().isTrigger = true;
    }

    if (Helpers.IsValueInArray(Constants.canBreakTags, colTag) && !isBreaking) {
      isBreaking = true;
      Destroy(body);
      GetComponent<BoxCollider2D>().isTrigger = true;

      string rarity = itemRarity != "" ? itemRarity : (Helpers.IsValueInArray(Constants.moneyItemKeys, item) ? "money" : "normal");

      // ensure the droppable to instantiate appears at the middle of the breakable (i.e. a bit off the breakable's ground) to avoid issues with ground collision at the beginning
      InGame.instance.InstantiatePrefab("droppable", item, rarity, GetItemSpawnedParent(), new Vector2(transform.position.x, transform.position.y + spriteRenderer.size.y / 2), spriteRenderer);

      GameObject parentObject = col.transform.parent != null ? col.transform.parent.gameObject : null;
      if (parentObject != null && parentObject.name.Contains("Throwable")) {
        Throwable parentThrowable = parentObject.GetComponent<Throwable>();
        string weaponWielded = parentThrowable.type;
        Transform parentTransform = parentObject.GetComponent<Transform>();

        if(Helpers.IsNonBouncingThrowable(weaponWielded)) {
          parentThrowable.SetBounce(parentTransform, col.ClosestPoint(transform.position));

          // ensures that the throwables start their bounce back at the point of contact
          col.transform.parent.parent.position = parentTransform.position;
        }

        parentThrowable.collideTime = Time.time * 1000;
        parentThrowable.hasCollided = true;
        parentThrowable.maxEllapsedCollideTime = 1000f;
      }

      anim.Play("breakable-" + type);
    }

    if (colTag == "Zone") {
      ZoneSpecs currZoneSpecs = Helpers.GetOrException(Objects.zoneSpecs, col.gameObject.GetComponent<Zone>().type);
      fallingOn = currZoneSpecs.groundMaterial;
    }
  }

  public void DestroyBreakable() {
    Destroy(gameObject);
  }

  public void PlayBreaking() {
    AudioClip[] breakableClips = Helpers.GetOrException(Sounds.breakableSounds, type);
    PlaySound(Helpers.GetRandomClipFromGroup(breakableClips));
  }

  public void PlayFalling(GameObject objectUnder) {
    switch (objectUnder.tag) {
      case "Breakable":
        AudioClip breakableClip = Helpers.GetOrException(Helpers.GetOrException(Sounds.fallingSounds, type), objectUnder.GetComponent<Breakable>().type);
        PlaySound(breakableClip);
      break;
      case "Floor":
        AudioClip groundClip = Helpers.GetOrException(Helpers.GetOrException(Sounds.fallingSounds, type), fallingOn);
        PlaySound(groundClip);
      break;
      case "Item":
        AudioClip itemClip = Helpers.GetOrException(Helpers.GetOrException(Sounds.fallingSounds, type), objectUnder.tag.ToLower());
        PlaySound(itemClip);
      break;
      default:
        Debug.Log("Consider adding sound for when falling on " + objectUnder.name + "(tagged " + objectUnder.tag + ")");
      break;
    }
  }

  public void PlaySound(AudioClip breakableSound) {
    if (Helpers.IsPastPlayElapsedTime() && Settings.playSFX) {
      // lower volume to aggregate to 1 depending on the breakable siblings
      float audioVolume = 1 / BreakableCount();
      audioSource.volume = audioVolume;

      soundLength = breakableSound.length;

      // TODO: since the ingame object is used for sound play, consider if the AudioSource component for breakables should be removed
      InGame.instance.PlaySound(breakableSound, transform.position, audioVolume);
      StartCoroutine(ActionAfterSound());
    }
  }

  IEnumerator ActionAfterSound() {
    yield return new WaitForSeconds(soundLength);
    audioSource.volume = 1;
    isFalling = false;
  }

  private float BreakableCount() {
    if (transform.parent.gameObject.name != "AudioGroup") {
      return 1;
    }

    return transform.parent.childCount;
  }

  private GameObject GetItemSpawnedParent() {
    Transform immediateParent = transform.parent;
    if (immediateParent.gameObject.name == "AudioGroup") {
      return immediateParent.parent.gameObject;
    }

    return immediateParent.gameObject;
  }
}