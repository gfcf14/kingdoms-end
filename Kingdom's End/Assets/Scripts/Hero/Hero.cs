using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
  [System.NonSerialized] public bool showDebug = false;

  [System.NonSerialized] public string pauseCase = "intro";
  [SerializeField] public List<Consumable> consumables = new List<Consumable>();
  [SerializeField] public float speed = 5;
  [SerializeField] public string groundType = "level";

  // to hold onto the ground type value should an action modify it
  [SerializeField] public string tempGroundType = "";
  [SerializeField] public float inclineSlope = 0.125f;
  [SerializeField] public float jumpHeight = GameData.playerJumpHeight;
  [SerializeField] private float jetpackHeight;

  [SerializeField] public GameObject mpBarContainer;
  [SerializeField] public GameObject weaponCollider;
  [SerializeField] public GameObject shieldCollider;
  [SerializeField] public GameObject bow;
  [SerializeField] public GameObject airEdgeCheck;
  public AirEdgeCheck airEdgeCheckScript;
  public ProximityCheck proximityCheckScript;

  public BoxCollider2D heroCollider;
  public Rigidbody2D body;
  private Animator anim;
  private SpriteRenderer heroRenderer;
  private AudioSource audioSource;

  public float heroHeight;
  public float heroWidth;

  public bool collidingTop = false;
  public bool collidingBottom = false;
  public bool collidingFront = false;
  public bool collidingBack = false;

  // for when player must move on their own
  public bool isAutonomous = false;
  public bool mustTransitionOnAir = false;

  public bool isRunning;
  public bool isGrounded;
  public bool canFlipOnAir;
  public bool isFalling;
  public bool isJumping;

  public bool canDoubleJump = false;

  public int jumpsExecuted = 0;

  public bool canCastMagic = false;

  // public bool isJetpackUp;
  // public string jetpackHorizontal = "";
  // public float maxJetpackTime = 1500;
  // public float jetpackTime = 0;

  public float currentYPosition = 0;
  public float currentXPosition = 0;

  public float throwbackHeight = 0;

  public int isHurt = 0;
  public int hurtCounter = 0; // to increment/decrement position by mapping
  public int isDead = 0;

  public bool isDefending = false;
  public bool isParrying = false;
  public bool isClashing = false;

  // public bool isGliding;
  public bool isFacingLeft;

  public bool isAttackingSingle;
  public bool isAttackingHeavy;

  public bool isAirAttackSingle;
  public bool isAirAttackHeavy;

  // should set to true once the player learns to kick
  public bool canKick = false;
  public bool isKicking;

  // should set to true once the player learns to drop kick
  public bool canDropKick = false;
  public bool isDropKicking;

  public bool isPunching;
  public bool isAirPunching;

  // public bool isAirShooting;

  // isThrowing determines a throw in an int, so it's 0 when not throwing, but 1 or 2 depending on what arm throws
  public int isThrowing;
  public bool startThrow = false;

  // public bool isShootingSingle;
  // public bool isShootingAuto;

  public bool isShootingPull;

  public bool isInvulnerable = false;

  public bool isOnChat = false;

  public bool bombNearby = false;
  public float damageStartTime = 0;

  // TODO: develop a logic to ensure this time can be influenced by level, i.e. the higher the level, the higher the recover time (the longer invulnerability lasts)
  public float damageRecoverTime = 3000f;

  public bool horizontalCollision;

  public Vector2 heroDimensions = new Vector2(1.136886f, 2.290915f);

  public float horizontalInput = 0;
  public float verticalInput = 0;
  public int armUsed = 0;

  // public string[] weapons = new string[] {"fists", "single", "heavy", "throwables", "projectile-single", "projectile-heavy", "projectile-auto", "projectile-pull"};

  // public string jetpackUp = "🡣🡡⌴";
  // public string jetpackUp = "du$";
  // public string jetpackLeft = "🡣🡠⌴";
  // public string jetpackLeft = "dl$";
  // public string jetpackRight = "🡣🡢⌴";
  // public string jetpackRight = "dr$";
  public float timeoutDuration = 0.25f;

  private string userInput = "";
  private float timeoutTime = 0.0f;

  public int direction = 1;

  // public int weaponIndex = 0;

  // public string currentWeapon;

  public GameObject nearbyInteractableObject;

  [SerializeField] public GameObject bombCheck;
  BombCheck bombCheckScript;
  public string NPCnearby;
  public string NPCnearbyAction;
  [SerializeField] public string collisionDirection = "";
  [SerializeField] public string blockedDirection = "";
  public bool hurtFromBehind = false;
  public bool isCollidingWithCeiling = false;

  // PLAYER STATS
    [System.NonSerialized] public int playerLevel = 1;
    [System.NonSerialized] public int currentHP = GameData.baseHP;
    [System.NonSerialized] public int maxHP = GameData.baseHP;
    [System.NonSerialized] public int currentMP = GameData.baseHP;
    [System.NonSerialized] public int maxMP = GameData.baseHP;
    [System.NonSerialized] public string status = "good";
    [System.NonSerialized] public int exp = 0;
    [System.NonSerialized] public int next = 0;
    [System.NonSerialized] public int gold = 500;
    [System.NonSerialized] public int strength = GameData.baseSTR;
    [System.NonSerialized] public int stamina = GameData.baseSTA;
    [System.NonSerialized] public float luckPercentage = GameData.baseLCK;
    [System.NonSerialized] public float criticalPercentage = GameData.baseCRI;
    [System.NonSerialized] public string location = "meadows";
    [System.NonSerialized] public HeroMagicResistance[] magicResistances = new HeroMagicResistance[] {
      new HeroMagicResistance() {name = "earth", frequency = 0},
      new HeroMagicResistance() {name = "air", frequency = 0},
      new HeroMagicResistance() {name = "water", frequency = 0},
      new HeroMagicResistance() {name = "fire", frequency = 0},
      new HeroMagicResistance() {name = "lightning", frequency = 0},
      new HeroMagicResistance() {name = "ice", frequency = 0},
      new HeroMagicResistance() {name = "light", frequency = 0},
      new HeroMagicResistance() {name = "dark", frequency = 0}
    };

    [System.NonSerialized] public HeroMagicResistance[] effectMagicResistances = new HeroMagicResistance[] {
      new HeroMagicResistance() {name = "earth", frequency = 0},
      new HeroMagicResistance() {name = "air", frequency = 0},
      new HeroMagicResistance() {name = "water", frequency = 0},
      new HeroMagicResistance() {name = "fire", frequency = 0},
      new HeroMagicResistance() {name = "lightning", frequency = 0},
      new HeroMagicResistance() {name = "ice", frequency = 0},
      new HeroMagicResistance() {name = "light", frequency = 0},
      new HeroMagicResistance() {name = "dark", frequency = 0}
    };

    [System.NonSerialized] public Dictionary<string, int> magicResistanceTypeIndex = new Dictionary<string, int> {
      {"Earth", 0},
      {"Air", 1},
      {"Water", 2},
      {"Fire", 3},
      {"Lightning", 4},
      {"Ice", 5},
      {"Light", 6},
      {"Dark", 7},
    };

  // PLAYER EQUIPMENT
    [System.NonSerialized] public static string bodyEquipment = "body-1";
    [System.NonSerialized] public static string arm1Equipment = "basic-longsword";
    [System.NonSerialized] public static string arm2Equipment = "basic-longsword";
    [System.NonSerialized] public static string neckEquipment = "";
    [System.NonSerialized] public static string armwear1Equipment = "";
    [System.NonSerialized] public static string armwear2Equipment = "";
    [System.NonSerialized] public static string ring1Equipment = "";
    [System.NonSerialized] public static string ring2Equipment = "";

    [System.NonSerialized] public static string projectileEquipment = "";

    [System.NonSerialized] public string[] equipmentArray = { bodyEquipment, arm1Equipment, arm2Equipment, neckEquipment, armwear1Equipment, armwear2Equipment, ring1Equipment, ring2Equipment};

  // PLAYER EQUIPPED STATS
    [System.NonSerialized] public float equippedSTR = 0f;
    [System.NonSerialized] public float equippedSTA = 0f;
    [System.NonSerialized] public float equippedLUCK = 0f;
    [System.NonSerialized] public float equippedCRIT = 0f;
    [System.NonSerialized] public static int equippedATK1 = 0;
    [System.NonSerialized] public static int equippedATK2 = 0;
    [System.NonSerialized] public int equippedDEF1 = 0;
    [System.NonSerialized] public int equippedDEF2 = 0;

  // PLAYER EFFECT STATS
    [SerializeField] public float effectSTR = 0f;
    [SerializeField] public float effectSTA = 0f;
    [SerializeField] public float effectCRIT = 0f;
    [SerializeField] public float effectLCK = 0f;

  [System.NonSerialized] public List<Item> items = new List<Item>();
  [System.NonSerialized] public List<Item> relicItems = new List<Item>();

  public int tiredThreshold = 40;

  private int maxShieldHP = 0;
  private float currentShieldHP = 0;
  private float currentShieldRecoverTime = 0;
  private float shieldDropTime = 0;

  // TODO: remove these variables and add a new dictionary when adding another shield sprite
  public int dummyShieldHP = 5;
  public float dummyShieldRecoverTime = 2000;

  private GameObject arrowAnchor;
  private GameObject arrowMask;

  private Vector2 transportLocation;

  public bool isPaused;

  [System.NonSerialized] public GameObject currentRoom;

  [System.NonSerialized] bool canMap = false;

  [System.NonSerialized] public int bossTransitionDirection = 0;

  [System.NonSerialized] private Dictionary<string, string> npcNodes = new Dictionary<string, string> {
    {"peasant-girl", ""}
  };

  public static Hero instance;
  private void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else if (instance != this) {
      Destroy(gameObject); // Destroy any duplicates! Only ONE hero can exist!
    }
  }

  // called when script is loaded
  private void Start() {
    #if !UNITY_EDITOR
      // sets the player position when not in the editor
      transform.position = new Vector2(GameData.playerX, GameData.playerY);
    #endif

    heroCollider = GetComponent<BoxCollider2D>();
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    heroRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    airEdgeCheckScript = airEdgeCheck.GetComponent<AirEdgeCheck>();
    proximityCheckScript = transform.Find("ProximityCheck").GetComponent<ProximityCheck>();
    bombCheckScript = bombCheck.GetComponent<BombCheck>();

    // currentWeapon = weapons[weaponIndex % weapons.Length];

    heroHeight = heroRenderer.bounds.size.y;
    heroWidth = heroRenderer.bounds.size.x;

    // TODO: move this to shield equipment change once equipment options are available
    maxShieldHP = dummyShieldHP;
    currentShieldHP = maxShieldHP;
    currentShieldRecoverTime = dummyShieldRecoverTime;

    // arrowAnchor = transform.Find("ArrowAnchor").gameObject;

    //test items and equipment
    #if UNITY_EDITOR
      items.Add(new Item("watermelon", 10));
      items.Add(new Item("honeydew", 10));
      items.Add(new Item("coconut", 10));
      items.Add(new Item("luck-flask", 1));
      items.Add(new Item("lightning-med", 1));
      items.Add(new Item("strength-flask", 1));
      items.Add(new Item("stamina-flask", 1));
      items.Add(new Item("magic-vial", 1));
      items.Add(new Item("potion", 1));
      items.Add(new Item("chicken-drumstick", 5));
      items.Add(new Item("basic-shield", 2));
      items.Add(new Item("basic-sword", 1));
      items.Add(new Item("basic-longsword", 1));
      items.Add(new Item("love-necklace", 1));
      // items.Add(new Item("solomon-ring", 1));
      items.Add(new Item("ra-ring", 1));
      items.Add(new Item("skull-ring", 1));
      items.Add(new Item("gold-bracelet", 1));
      items.Add(new Item("silver-bracelet", 1));
      items.Add(new Item("rabbit-paw", 1));
      items.Add(new Item("moonlight-pendant", 1));
      items.Add(new Item("rainbow-bracer", 1));
      items.Add(new Item("body-1", 1));
      items.Add(new Item("lance", 30));
      items.Add(new Item("axe", 20));
      items.Add(new Item("hatchet", 14));
      items.Add(new Item("shuriken-6", 23));
      items.Add(new Item("shuriken-4", 50));
      items.Add(new Item("knife", 30));
      items.Add(new Item("kunai", 37));
      items.Add(new Item("basic-bow", 1));
      items.Add(new Item("arrow-standard", 21));
      items.Add(new Item("arrow-poison", 5));
      items.Add(new Item("arrow-fire", 10));
      items.Add(new Item("bomb", 99));
      items.Add(new Item("king-bone", 16));
      items.Add(new Item("elixir", 99));
      items.Add(new Item("theriac", 1));
      items.Add(new Item("strawberry", 1));
      items.Add(new Item("pineapple", 1));
      items.Add(new Item("skull", 1));
      items.Add(new Item("textiles", 1));
      items.Add(new Item("amethyst", 1));
      items.Add(new Item("ruby", 1));
      items.Add(new Item("citrine", 1));
      items.Add(new Item("emerald", 1));
      items.Add(new Item("critical-flask", 1));
      items.Add(new Item("hashish", 1));
      items.Add(new Item("pearl", 1));
      items.Add(new Item("mid-potion", 1));
      // items.Add(new Item("high-potion", 1));
      items.Add(new Item("fire-med", 1));
      items.Add(new Item("dark-med", 1));
      items.Add(new Item("watermelon-slice", 1));

      bodyEquipment = "body-1";
      canKick = true;
      canDropKick = true;

      // TODO: use this code to navigate while underwater
      // anim.speed = 0.66f;
      // jumpHeight = 10;

    #else
      items.Add(new Item("arrow-fire", 25));
      items.Add(new Item("arrow-poison", 25));
      items.Add(new Item("arrow-standard", 25));
      items.Add(new Item("axe", 25));
      items.Add(new Item("basic-shield", 1));
      items.Add(new Item("basic-sword", 1));
      items.Add(new Item("body-1", 1));
      items.Add(new Item("bomb", 25));
      items.Add(new Item("basic-bow", 1));
      items.Add(new Item("chicken-drumstick", 2));
      items.Add(new Item("hatchet", 25));
      items.Add(new Item("king-bone", 25));
      items.Add(new Item("knife", 25));
      items.Add(new Item("kunai", 25));
      items.Add(new Item("lance", 25));
      items.Add(new Item("potion", 25));
      items.Add(new Item("shuriken-4", 25));
      items.Add(new Item("shuriken-6", 25));
      items.Add(new Item("watermelon", 10));
      items.Add(new Item("honeydew", 10));
      items.Add(new Item("coconut", 10));

      items.Add(new Item("darklord-sword", 1));
    #endif

    // TODO: after implementing the load functionality, playerLevel should be updated via reading save data
    SetupStatsByLevel();
  }

  public void AddToRelics(string relicKey) {
    // For the first time, the pause buttons must be changed to allow for the Relic button to be enabled
    if (relicItems.Count == 0) {
      // fetch all buttons involved
      Transform mainCanvas = InGame.instance.pauseCanvas.transform.Find("PauseBackground").Find("Wrapper Outline").Find("Right Outline").Find("MainCanvas");
      GameObject buttonEquipment = mainCanvas.Find("ButtonEquipment").gameObject;
      GameObject buttonRelics = mainCanvas.Find("ButtonRelics").gameObject;
      GameObject buttonMap = mainCanvas.Find("ButtonMap").gameObject;

      // modify button navigation targets
        Navigation newButtonEquipmentNavigation = new Navigation();
        newButtonEquipmentNavigation.mode = Navigation.Mode.Explicit;
        newButtonEquipmentNavigation.selectOnDown = buttonRelics.GetComponent<Button>();
        newButtonEquipmentNavigation.selectOnUp = mainCanvas.Find("ButtonItems").GetComponent<Button>();
        buttonEquipment.GetComponent<Button>().navigation = newButtonEquipmentNavigation;

        Navigation newButtonMapNavigation = new Navigation();
        newButtonMapNavigation.mode = Navigation.Mode.Explicit;
        newButtonMapNavigation.selectOnDown = mainCanvas.Find("ButtonOptions").GetComponent<Button>();
        newButtonMapNavigation.selectOnUp = buttonRelics.GetComponent<Button>();
        buttonMap.GetComponent<Button>().navigation = newButtonMapNavigation;

      // modify button text opacity
      buttonRelics.transform.Find("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
    }

    relicItems.Add(new Item(relicKey, 1));

    // TODO: though this sets a hero property given the relic effect value (a string), it's necessary to create a switch block to determine how
    // this should happen given the effect type as well
    this.GetType().GetField(Helpers.GetOrException(Objects.relicItems, relicKey).effect.value).SetValue(this, true);
  }

  public void SetupStatsByLevel() {
    foreach (string currStat in GameData.playerStats) {
      switch(currStat) {
        case "HP":
          int hpByLevel = (int)Helpers.GetStatByLevel(currStat, playerLevel);
          // only modify max value (avoids full recovery after level up)
          maxHP = hpByLevel;
        break;
        case "MP":
          int mpByLevel = (int)Helpers.GetStatByLevel(currStat, playerLevel);
          // only modify max value (avoids full recovery after level up)
          maxMP = mpByLevel;
        break;
        case "STR":
          strength = (int)Helpers.GetStatByLevel(currStat, playerLevel);
        break;
        case "STA":
          stamina = (int)Helpers.GetStatByLevel(currStat, playerLevel);
        break;
        case "CRI":
          criticalPercentage = Helpers.GetStatByLevel(currStat, playerLevel);
        break;
        case "LCK":
          luckPercentage = Helpers.GetStatByLevel(currStat, playerLevel);
        break;
      }
    }

    UpdateStatsValues();

    next = Helpers.NextLevelEXP(playerLevel + 1);
  }

  public float GetGroundVerticalModifier(string groundType, float currentSpeed) {
    if (!isGrounded) {
      return body.velocity.y;
    }

    if (groundType == "level") {
      return 0;
    }

    if (groundType == "incline" && !isFacingLeft) { // going up left to right
      return currentSpeed * inclineSlope;
    } else if (groundType == "descent" && isFacingLeft) { // going up right to left
      return -currentSpeed * inclineSlope;
    } else if (groundType == "incline" && isFacingLeft) { // going down right to left
      return currentSpeed * inclineSlope * 4;
    } else if (groundType == "descent" && !isFacingLeft) { // going down left to right
      return -currentSpeed * inclineSlope * 4;
    }

    return 0;
  }

  // adds consumable only if it hasn't been consumed before
  public void AddConsumable(Consumable newConsumable) {
    bool isUsed = consumables.Any(c => c.key == newConsumable.key);

    if (!isUsed) {
      consumables.Add(newConsumable);
      UpdateEffectValues(newConsumable.key, true);
      UpdateEffectMagicResistances();
    }
  }

  public void UpdateEffectValues(string key, bool add) {
    RegularItem effectItem = Helpers.GetOrException(Objects.regularItems, key);
    int multiplier = add ? 1 : -1;

    effectSTR += (float)(effectItem.effects.atk ?? 0) * multiplier;
    effectSTA += (float)(effectItem.effects.def ?? 0) * multiplier;
    effectCRIT += (float)(effectItem.effects.crit ?? 0) * multiplier;
    effectLCK += (float)(effectItem.effects.luck ?? 0) * multiplier;
  }

  public void UpdateStatsValues() {
    equippedSTR = PrepareEquippedStat("atk");
    equippedSTA = PrepareEquippedStat("def");
    equippedLUCK = PrepareEquippedStat("luck");
    equippedCRIT = PrepareEquippedStat("crit");

    equippedATK1 = (arm1Equipment != "" ? Helpers.GetOrException(Objects.regularItems, arm1Equipment).effects.atk != null ? (int)Helpers.GetOrException(Objects.regularItems, arm1Equipment).effects.atk : 0 : 0) + (projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, projectileEquipment).effects.atk ?? 0 : 0);
    equippedATK2 = (arm2Equipment != "" ? Helpers.GetOrException(Objects.regularItems, arm2Equipment).effects.atk != null ? (int)Helpers.GetOrException(Objects.regularItems, arm2Equipment).effects.atk : 0 : 0)  + (projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, projectileEquipment).effects.atk ?? 0 : 0);
    equippedDEF1 = arm1Equipment != "" ? Helpers.GetOrException(Objects.regularItems, arm1Equipment).effects.def != null ? (int)Helpers.GetOrException(Objects.regularItems, arm1Equipment).effects.def : 0 : 0;
    equippedDEF2 = arm2Equipment != "" ? Helpers.GetOrException(Objects.regularItems, arm2Equipment).effects.def != null ? (int)Helpers.GetOrException(Objects.regularItems, arm2Equipment).effects.def : 0 : 0;

    equippedSTR = equippedSTR - (arm1Equipment == "" ? 0 : equippedATK1) - (arm2Equipment == "" ? 0 : equippedATK2) + (projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, projectileEquipment).effects.atk * 2 ?? 0 : 0);
    equippedSTA = equippedSTA - (arm1Equipment == "" ? 0 : equippedDEF1) - (arm2Equipment == "" ? 0 : equippedDEF2) + (projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, projectileEquipment).effects.atk * 2 ?? 0 : 0);

    UpdateMagicResistances();
  }

  public float PrepareEquippedStat(string effect) {
    float totalStat = 0.0f;

    int i = 0;
    foreach (string currentEquipment in equipmentArray) {
      if (currentEquipment != "") {
        Effects currentEffects = Helpers.GetOrException(Objects.regularItems, currentEquipment).effects;
        if (currentEffects.GetType().GetField(effect).GetValue(currentEffects) != null) {
          totalStat += float.Parse(currentEffects.GetType().GetField(effect).GetValue(currentEffects).ToString());
        }
      }

      i++;
    }

    return totalStat;
  }

  public void UpdateMagicResistances() {
    foreach (HeroMagicResistance currHeroMR in magicResistances) {
      // Frequencies are implied to be decided by item equipped only
      currHeroMR.frequency = 0;
    }

    foreach (string currentEquipment in equipmentArray) {
      if (currentEquipment != "") {
        RegularItem currentRegularItem = Helpers.GetOrException(Objects.regularItems, currentEquipment);

        if (currentRegularItem.effects.magicResistances != null) {
          foreach (MagicResistance currMagicResistance in currentRegularItem.effects.magicResistances) {
            magicResistances[Helpers.GetOrException(magicResistanceTypeIndex, currMagicResistance.name)].frequency += currMagicResistance.type == "add" ? 1 : -1;
          }
        }
      }
    }
  }

  public void UpdateEffectMagicResistances() {
    foreach (HeroMagicResistance currentEffectHeroMR in effectMagicResistances) {
      currentEffectHeroMR.frequency = 0;
    }

    foreach (Consumable currentConsumable in consumables) {
      RegularItem currentRegularItem = Helpers.GetOrException(Objects.regularItems, currentConsumable.key);

      if (currentRegularItem.effects.magicResistances != null) {
        foreach (MagicResistance currMagicResistance in currentRegularItem.effects.magicResistances) {
          effectMagicResistances[Helpers.GetOrException(magicResistanceTypeIndex, currMagicResistance.name)].frequency += currMagicResistance.type == "add" ? 1 : -1;
        }
      }
    }
  }

  public void EquipItem(string newItem, int itemIndex) {
    string newItemType = Helpers.GetOrException(Objects.regularItems, newItem).type;
    switch (itemIndex) {
      case 0:
        bodyEquipment = newItem;
      break;
      case 1:
        arm1Equipment = newItem;

        if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, newItemType)) {
          arm2Equipment = newItem;
        } else {
          if (arm2Equipment != "") {
            string arm2Type = Helpers.GetOrException(Objects.regularItems, arm2Equipment).type;

            if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, arm2Type)) {
              arm2Equipment = "";
            }
          }
        }
      break;
      case 2:
        arm2Equipment = newItem;

        if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, newItemType)) {
          arm1Equipment = newItem;
        } else {
          if (arm1Equipment != "") {
            string arm1Type = Helpers.GetOrException(Objects.regularItems, arm1Equipment).type;

            if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, arm1Type)) {
              arm1Equipment = "";
            }
          }
        }
      break;
      case 3:
        neckEquipment = newItem;
      break;
      case 4:
        armwear1Equipment = newItem;
      break;
      case 5:
        armwear2Equipment = newItem;
      break;
      case 6:
        ring1Equipment = newItem;
      break;
      case 7:
        ring2Equipment = newItem;
      break;
      default:
        Debug.Log("Updating item " + newItem + " at index " + itemIndex + " somehow");
      break;
    }

    equipmentArray = new string[] { bodyEquipment, arm1Equipment, arm2Equipment, neckEquipment, armwear1Equipment, armwear2Equipment, ring1Equipment, ring2Equipment};
    UpdateStatsValues();
  }

  public void Unequip(int itemIndex) {
    switch (itemIndex) {
      case 1: {
        string removedItemType = Helpers.GetOrException(Objects.regularItems, arm1Equipment).type;

        if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, removedItemType)) {
          arm2Equipment = "";
        }
        arm1Equipment = "";

        break;
      }
      case 2: {
        string removedItemType = Helpers.GetOrException(Objects.regularItems, arm2Equipment).type;

        if (Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, removedItemType)) {
          arm1Equipment = "";
        }
        arm2Equipment = "";

        break;
      }
      case 3:
        neckEquipment = "";
      break;
      case 4:
        armwear1Equipment = "";
      break;
      case 5:
        armwear2Equipment = "";
      break;
      case 6:
        ring1Equipment = "";
      break;
      case 7:
        ring2Equipment = "";
      break;
      default:
        Debug.Log("Unequipping item at index " + itemIndex + " somehow");
      break;
    }

    equipmentArray = new string[] { bodyEquipment, arm1Equipment, arm2Equipment, neckEquipment, armwear1Equipment, armwear2Equipment, ring1Equipment, ring2Equipment};
    UpdateStatsValues();
  }

  public void UpdateStats(string stat, int? aggregate) {
    switch (stat) {
      case "hp":
        int hpAggregate = aggregate ?? 0;

        // condition below prevents healing more than allowed
        if ((currentHP + hpAggregate) >= maxHP) {
          currentHP = maxHP;
        } else {
          currentHP += hpAggregate;
        }
      break;
      case "mp":
        int mpAggregate = aggregate ?? 0;

        // condition below prevents restoring more than allowed
        if ((currentMP + mpAggregate) >= maxMP) {
          currentMP = maxMP;
        } else {
          currentMP += mpAggregate;
        }
      break;
      default:
        Debug.Log("Value for stat \"" + stat + "\" doesn't correspond to this function. Either overload another or check the type");
      break;
    }

    // TODO: build the others as more items are created!
  }

  public void ConsumeItem(string key) {
    int i = 0;
    foreach (Item currItem in items) {
      if (key == currItem.key) {
        currItem.amount--;
        break;
      }
      i++;
    }
  }

  public void RemoveItem(int index) {
    items.RemoveAt(index);
  }

  public void PlaySound(AudioClip sound) {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(sound);
    }
  }

  public void PlayRunningSound() {
    string materialRunningOn = InGame.instance.GetTileMaterial(new Vector3(transform.position.x + direction, transform.position.y + 0.1f, transform.position.z));

    // if there is no tile material, falling sound will be assumed from location
    if (materialRunningOn == null) {
      materialRunningOn = Helpers.GetMaterial(location);
    }

    if (materialRunningOn != null) {
      AudioClip[] materialClips = Helpers.GetOrException(Helpers.GetOrException(Sounds.runningSounds, materialRunningOn), Helpers.GetOrException(Objects.equipmentBaseMaterial, bodyEquipment));
      PlaySound(materialClips[UnityEngine.Random.Range(0, materialClips.Length)]);
    }
  }

  public GameObject GetObjectUnder() {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f);

    // get the first one that is not Hero
    foreach (Collider2D collider in colliders) {
      if (collider.gameObject.tag != "Hero") {
        return collider.gameObject;
      }
    }

    return null;
  }

  public void PlayAttackSound() {
    RegularItem equipmentUsed = Helpers.GetOrException(Objects.regularItems, equipmentArray[armUsed]);
    weaponCollider.GetComponent<Weapon>().PlaySound(equipmentUsed.type, equipmentArray[armUsed]);
  }

  public void PunchSound() {
    weaponCollider.GetComponent<Weapon>().PlaySound("punch");
  }

  public void KickSound() {
    weaponCollider.GetComponent<Weapon>().PlaySound("kick");
  }

  public void PlayBowSound() {
    bow.GetComponent<Bow>().PlaySound(projectileEquipment);
  }

  public void PerformGroundFall() {
    // when falling, y position may need to be adjusted by 0.1f to avoid null tile recognition
    string tileMaterial = InGame.instance.GetTileMaterial(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z));

    // if there is no tile material, falling sound will be assumed from location
    if (tileMaterial == null) {
      tileMaterial = Helpers.GetMaterial(location);
    }

    PlayFallingSound(tileMaterial, Helpers.GetOrException(Objects.equipmentBaseMaterial, bodyEquipment));
  }

  public void PlayFallingSound(string fallen, string fallingOn) {
    PlaySound(Helpers.GetOrException(Helpers.GetOrException(Sounds.characterFallingSounds, fallen), fallingOn));
  }

  public void ToggleAirCheck(bool activate) {
    if (activate) {
      if (!airEdgeCheck.activeSelf) {
        airEdgeCheck.SetActive(true);
      }
    } else {
      if (airEdgeCheck.activeSelf) {
        airEdgeCheck.SetActive(false);
      }
    }
  }

  public void GroundOnIncline() {
    ToggleAirCheck(false);
    isJumping = false;
    isFalling = false;
    jumpsExecuted = 0;
    isGrounded = true;
    blockedDirection = "";
    PerformGroundFall();
  }

  public bool IsOnIncline() {
    return groundType != "level";
  }

  public void ClearInvulnerability() {
    isInvulnerable = false;
    body.mass = 1;
    // restores color so the flicker won't leave it in weird transparency
    heroRenderer.color = Color.white;
  }

  public void ResumeGame() {
    InGame.instance.ToggleSoundtrack(isPaused);
    isPaused = !isPaused;
    Helpers.TogglePause(isPaused, InGame.instance.pauseCanvas);
  }

  public void PerformPortalTransport() {
    InGame.instance.actionCanvas.SetActive(false);
    transform.position = transportLocation;
    transportLocation = Vector2.zero;
    InGame.instance.FlashFadeIn();
  }

  public void TransportViaPortal(Vector2 newLocation) {
    transportLocation = newLocation;
    InGame.instance.FlashFadeOut();
  }

  public bool IsMovingUphill() {
    return ((isFacingLeft && groundType == "descent") || (!isFacingLeft && groundType == "incline")) && isRunning && isGrounded;
  }

  // called on every frame of the game
  private void Update() {
    direction = isFacingLeft ? -1 : 1;

    // THE DEBUG OPTIONS BELOW SHOULD IDEALLY BE TESTED INDIVIDUALLY (by commenting out all others when testing one) to avoid over saturating the window with colors

    // DEBUG for VELOCITY: draws the speeds used by the player to attempt to understand the direction taken on movement
      // x velocity
      Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.01f), Vector2.right * body.velocity.x, Helpers.GetOrException(Colors.raycastColors, "vx"));

      // y velocity
      Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.01f), Vector2.up * body.velocity.y, Helpers.GetOrException(Colors.raycastColors, "vy"));

      // overall speed direction
      Debug.DrawRay(transform.position, body.velocity, Helpers.GetOrException(Colors.raycastColors, "vxy"));
    // END of DEBUG for VELOCITY

    // PLAYER FALLING ALGORITHM: checks if player collides with anything. If not, player should fall
      // draws the collider based on the pivot plus half player height up so it is a rectangle which north and south sides start at the head and end at the feet, respectively
      Vector2 playerColliderPosition = new Vector2(transform.position.x, transform.position.y + heroHeight / 2);
      Collider2D[] playerColliders = Physics2D.OverlapBoxAll(playerColliderPosition, heroDimensions, 0f);

      // DEBUG for FALL BOUNDS: draws this to be visible on Scene mode (or with gizmos) to check how it can change and affect falling strategy
        // InGame.instance.DrawRectangle(playerColliderPosition, heroDimensions);
      // END of DEBUG for FALL BOUNDS


      // gets all non-trigger collider count from the intersecting ones
      int colliderCount = playerColliders.Count(col => !col.isTrigger);

      // if only the player collider is found, nothing else was found and player should fall
      // TODO: check if other attack types cause the player to lift off the ground, even but slightly, and add them here
      // TODO: consider if at any point it'd be necessary to include some form of list of animations where falling shouldn't happen
      if (/*!Helpers.IsAnyPlaying(anim, Constants.heroNonFallingAnimations) && */!collidingBottom  && !IsOnIncline() && !IsMovingUphill() && !isAttackingHeavy && colliderCount <= 1 && body.velocity.y < 0 && GroundFallDistance() > Constants.fallThreshold) {
        Fall();
      }
    // end of PLAYER FALLING ALGORITHM

    // DEBUG FOR TILE: checks for tile name and debugs its position
        // InGame.instance.GetTileName(transform.position);
    // END of DEBUG FOR TILE


    if (!isAutonomous) {
      // TODO: remove key combinations as they will not be used to favor two keys pressed
      foreach (KeyCode currentKey in System.Enum.GetValues(typeof(KeyCode))) {
        if(Input.GetKeyUp(currentKey)) {
          if (userInput.Length == 0) {
            timeoutTime = Time.time + timeoutDuration;
          }

          // if (currentKey.ToString() == "Space") {
          //   // userInput += "⌴";
          //   userInput += "$";
          // } else
          if (currentKey.ToString() == "UpArrow") {
            // userInput += "🡡";
            userInput += "u";
          } else if (currentKey.ToString() == "DownArrow") {
            // userInput += "🡣";
            userInput += "d";
          } else if (currentKey.ToString() == "LeftArrow") {
            // userInput += "🡠";
            userInput += "l";
          } else if (currentKey.ToString() == "RightArrow") {
            // userInput += "🡢";
            userInput += "r";
          } else {
            if (currentKey.ToString() != "Space") {
              userInput += currentKey.ToString();
            }
          }

          // if (userInput.Contains(jetpackUp)) {
          //   JetpackUp();
          //   userInput = "";
          // } else if (userInput.Contains(jetpackLeft)) {
          //   Debug.Log("jetpack left");
          //   userInput = "";
          // } else if (userInput.Contains(jetpackRight)) {
          //   Debug.Log("jetpack right");
          //   userInput = "";
          // } else if (userInput == "s") { // jumping?
          //   Jump();
          // }

          // Debug.Log(currentKey.ToString());

          if (isPaused) {
            if (Pause.currentlyMapping != "") {
              if ((currentKey.ToString().Contains("JoystickButton") && Input.GetJoystickNames()[0] != "") || !currentKey.ToString().Contains("Joystick")) {
                if (canMap) {
                  if (!Helpers.IsForbiddenToRemap(currentKey.ToString())) {
                    InGame.instance.pauseCanvas.GetComponent<Pause>().FinishMapping(currentKey.ToString());
                    canMap = false;
                  }
                } else {
                  canMap = true;
                }
              }
            }
          }
        } else if (Time.time > timeoutTime && userInput.Length > 0) { // input is cleared
          userInput = "";
        }
      }

      if (!isPaused && pauseCase == "") {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (shieldDropTime != 0) {
          if (Helpers.ExceedsTime(shieldDropTime, currentShieldRecoverTime)) {
            currentShieldHP = maxShieldHP;
            shieldDropTime = 0;
          }
        }

        // restricts horizontal input based on blocked direction due to bumping
        if ((blockedDirection == "left" && horizontalInput < -Constants.inputThreshold) || (blockedDirection == "right" && horizontalInput > Constants.inputThreshold)) {
          horizontalInput = 0;
        }

        // x axis movement
        if (!horizontalCollision && isHurt < 1) {
          if (!isDefending && !isParrying && !isClashing && isThrowing == 0) {
            // movement happens on this line
            body.velocity = new Vector2(!isDropKicking ? horizontalInput * speed : 0, GetGroundVerticalModifier(groundType, horizontalInput * speed));
          }

          // flip player back when moving right
          if (horizontalInput > 0.01f && (isGrounded || canFlipOnAir) && !isAttackingSingle) {
            transform.localScale = Vector3.one;

            if (!isDropKicking) {
              isFacingLeft = false;
            }
          }
          // flip player when moving left
          else if (horizontalInput < -0.01f && (isGrounded || canFlipOnAir) && !isAttackingSingle) {
            FlipPlayer();

            if (!isDropKicking) {
              isFacingLeft = true;
            }
          }
        }

        if (isClashing) {
          // TODO: modify the 2 to make it a multiplie based on enemy strength (?)
          body.velocity = new Vector2( (isFacingLeft ? 1 : -1) * speed * 2, body.velocity.y);
        }

        if (isHurt == 1) {
          body.velocity = new Vector2(0, body.velocity.y);
        }

        // // jumping + actions (kick, drop kick)
        // if (Helpers.IsKeyHeld(Controls.currentKeyboardJump) || Helpers.IsKeyHeld(Controls.currentGamepadJump)) {
        //   if (isGrounded) {
        //     if (isHoldingDown) {
        //       if (!isRunning && !isKicking && canKick) {
        //         isKicking = true;
        //         anim.SetTrigger("isKicking");
        //         weaponCollider.SetActive(true);
        //       }
        //     }
        //   } else {
        //     if (isHoldingDown && isJumping && !isFalling && canDropKick) {
        //       DropKick();
        //     }
        //   }
        //   // userInput += "$";
        //   // if (isGrounded) {
        //   //   if (userInput.Contains(jetpackUp)) {
        //   //     JetpackUp();
        //   //     userInput = "";
        //   //   } else {
        //   //     Jump();
        //   //     userInput = "";
        //   //   }
        //   // } else {
        //   //   if (userInput.Contains(jetpackLeft)) {
        //   //     JetpackHorizontal("left");
        //   //     userInput = "";
        //   //   } else if (userInput.Contains(jetpackRight)) {
        //   //     JetpackHorizontal("right");
        //   //     userInput = "";
        //   //   }
        //   // }

        // // jumping
        // } else if ((Helpers.IsKeyUp(Controls.currentKeyboardJump) || Helpers.IsKeyUp((Controls.currentGamepadJump))) && !isHoldingDown) {
        //   if (isGrounded || (canDoubleJump && jumpsExecuted < GameData.maxJumpLimit)) {
        //     jumpsExecuted++;
        //     Jump();
        //   }
        // }

        // TODO: for some reason, a gamepad's d-pad down button doesn't trigger vertical input. Investigate a workaround
        if (verticalInput < -Constants.inputThreshold) { // if DOWN key is being held
          if (Helpers.IsKeyDown(Controls.currentKeyboardJump) || Helpers.IsKeyDown(Controls.currentGamepadJump)) { // Perform actions if JUMP key is also held
            if (isGrounded) {
              if (!isRunning && !isKicking && canKick) { // KICK
                isKicking = true;

                if (bombCheckScript.BombNearby()) {
                  anim.SetTrigger("isKickingBomb");
                } else {
                  anim.SetTrigger("isKicking");
                }

                weaponCollider.SetActive(true);
              }
            } else if (isJumping && !isFalling && canDropKick) { // DROPKICK
              DropKick();
            }
          }
        } else {
          // JUMP
          if ((Helpers.IsKeyUp(Controls.currentKeyboardJump) || Helpers.IsKeyUp((Controls.currentGamepadJump)))) {
            if (isGrounded || (canDoubleJump && jumpsExecuted < GameData.maxJumpLimit)) {
              jumpsExecuted++;
              Jump();
            }
          }
        }

        // action
        if (Helpers.IsKeyUp(Controls.currentKeyboardAction) || Helpers.IsKeyUp(Controls.currentGamepadAction)) {
          if (InGame.instance.chatCanvas.activeSelf) {
            // CloseChat();
          } else {
            if (nearbyInteractableObject) {
              if (nearbyInteractableObject.tag == "Portal") {
                Portal interactingPortal = nearbyInteractableObject.GetComponent<Portal>();

                if (interactingPortal.portalType == "cave") {
                  Helpers.ChangeScene("Underground", interactingPortal.transportLocation, interactingPortal.cameraPosition);
                } else {
                  TransportViaPortal(interactingPortal.transportLocation);
                }
              }
            } else {
              if (NPCnearbyAction == "chat") {
                OpenChat();
              }
            }
          }
        }

        // arm 1
        if (Helpers.IsKeyDown(Controls.currentKeyboardAttack1) || Helpers.IsKeyDown(Controls.currentGamepadAttack1)) {
          DecideAttackType(arm1Equipment, 1);
        }
        if (Helpers.IsKeyUp(Controls.currentKeyboardAttack1) || Helpers.IsKeyUp(Controls.currentGamepadAttack1)) {
          DecideShieldRelease(arm1Equipment);
          isParrying = false;
        }

        // arm 2
        if (Helpers.IsKeyDown(Controls.currentKeyboardAttack2) || Helpers.IsKeyDown(Controls.currentGamepadAttack2)) {
          DecideAttackType(arm2Equipment, 2);
        }
        if (Helpers.IsKeyUp(Controls.currentKeyboardAttack2) || Helpers.IsKeyUp(Controls.currentGamepadAttack2)) {
          DecideShieldRelease(arm2Equipment);
          isParrying = false;
        }

        // if (!isGrounded && verticalSpeed < -1 && jetpackHorizontal == "") {


        // if (Input.GetKey(KeyCode.Keypad4) && currentWeapon == "projectile-auto") {
        //   isShootingAuto = true;
        // }

        // if (Input.GetKeyUp(KeyCode.Keypad4) && isShootingAuto) {
        //   isShootingAuto = false;
        // }


        // if (Input.GetKeyDown(KeyCode.Keypad7)) {
        //   PlayerHurt(1);
        // }

        // if (Input.GetKeyDown(KeyCode.Keypad8) && isGrounded) {
        //   PlayerHurt(2);
        // }

        // if (Input.GetKeyDown(KeyCode.Keypad9)) {
        //   PlayerHurt(3);
        // }

        if (isDropKicking) {
          body.velocity = new Vector2(body.velocity.x + (jumpHeight * direction), -(float)(jumpHeight * 0.75));
        }

        // if (isGliding) {
        //   body.velocity = new Vector2(body.velocity.x + (jumpHeight * direction), -(float)(jumpHeight * 0.25));
        // }

        // if (jetpackHorizontal != "") {
        //   body.velocity = new Vector2(body.velocity.x + (jetpackHeight * (jetpackHorizontal == "left" ? -1 : 1)), body.velocity.y);
        //   transform.position = new Vector2(transform.position.x, currentYPosition);
        //   if ((Time.time * 1000) > jetpackTime + maxJetpackTime) {
        //     jetpackHorizontal = "";
        //     jetpackTime = 0;
        //     body.velocity = new Vector2(0, 0);
        //   }
        // }

        if (isDead == 2) {
          if (!isGrounded) {
            body.velocity = new Vector2(-body.velocity.x + (jumpHeight * (isFacingLeft ? 2 : -2)), -(float)jumpHeight);
          } else {
            body.velocity = Vector2.zero;
          }
        }

        // Continuously checks if consumed items should be in effect
        for (int i = 0; i < consumables.Count; i++) {
          Consumable currentConsumable = consumables[i];

          if (Helpers.ExceedsTime(currentConsumable.useTime, currentConsumable.duration * 1000)) {
            UpdateEffectValues(currentConsumable.key, false);
            consumables.RemoveAt(i);
            UpdateEffectMagicResistances();
          }
        }
      }

      // gliding
      // if (Input.GetKey(KeyCode.UpArrow)) {
      //   if (!isGrounded) {
      //     if (Input.GetKey(KeyCode.Space)) {
      //       Glide();
      //     } else {
      //       isGliding = false;
      //     }
      //   }
      // } else {
      //   isGliding = false;
      // }

      if (pauseCase == "") { // only update isRunning if it's not paused in any way
        isRunning = Helpers.IsBeyondOrUnderRange(horizontalInput, Constants.inputThreshold) && !isJumping && !isFalling && !isAttackingSingle; // && !isJetpackUp;
        // Debug.Log("horizontalInput: " + horizontalInput + ", isJumping: " + isJumping + ", isFalling: " + isFalling + ", isAttackingSingle: " + isAttackingSingle);
      }

      // TESTING FOR PROGRAMMATIC PLAY
      // if (isRunning) {
      //   anim.Play("running-1", -1, 0f);
      // }

      if (Helpers.IsPauseKeyUp() && pauseCase == "" && Pause.currentlyMapping == "") {
        ResumeGame();
      }

      if (Helpers.IsBackKeyDown() && isPaused) {
        // do not perform "BACK" if a key is being mapped
        if (Pause.canvasStatus != "mapping") {
          if (Pause.canvasStatus == "main") {
            ResumeGame();
          } else {
            InGame.instance.pauseCanvas.GetComponent<Pause>().PerformBack();
          }
        }
      }

      // checks for invulnerability time
      if (isInvulnerable) {
        if (Helpers.ExceedsTime(damageStartTime, damageRecoverTime)) {
          ClearInvulnerability();
        }
      }
    }

    // UNCOMMENT ALL THESE TO START TESTING FOR PROGRAMMATIC PLAY - use the Hero - Copy animator
    // set animator parameters
    anim.SetBool("isRunning", isRunning);
    anim.SetBool("isGrounded", isGrounded);
    anim.SetBool("isFalling", isFalling);
    anim.SetBool("isJumping", isJumping);
    // anim.SetBool("isJetpackUp", isJetpackUp);
    anim.SetBool("horizontalCollision", horizontalCollision);
    anim.SetBool("isDropKicking", isDropKicking);
    // anim.SetBool("isShootingSingle", isShootingSingle);
    // anim.SetBool("isShootingAuto", isShootingAuto);
    anim.SetBool("isShootingPull", isShootingPull);
    // anim.SetBool("isAirShooting", isAirShooting);
    // anim.SetBool("isJetpackHorizontal", jetpackHorizontal != "");
    // anim.SetBool("isGliding", isGliding);
    anim.SetBool("isTired", (float)currentHP / (float)maxHP <= 0.2f);
    anim.SetInteger("isHurt", isHurt);
    anim.SetInteger("isDead", isDead);
    anim.SetBool("isDefending", isDefending);
    anim.SetBool("isParrying", isParrying);
    anim.SetBool("isClashing", isClashing);
    anim.SetBool("isCollidingWithCeiling", isCollidingWithCeiling);
  }

  void FixedUpdate() {
    if (isAutonomous) {
      if (isGrounded) {
        isRunning = true;
        body.velocity = new Vector2(speed * bossTransitionDirection, GetGroundVerticalModifier(groundType, speed * bossTransitionDirection));
      } else {
        isFalling = true;
        anim.Play("falling-1", -1, normalizedTime: 0);
        if (mustTransitionOnAir) {
          body.velocity = new Vector2(speed * bossTransitionDirection, 0);
        }
      }
    } else {
      if (isHurt == 2 && isGrounded) {
        float xIncrement = hurtCounter >= Constants.HurtBTransitions.Length ? 0 : Constants.HurtBTransitions[hurtCounter];
        transform.position = new Vector2(transform.position.x + ((isFacingLeft ? 1 : -1) * xIncrement * (hurtFromBehind ? -1 : 1)), currentYPosition);
        hurtCounter++;
      }

      if (isHurt == 3 && currentHP > 0) {
        if (body.gravityScale == 1) {
          body.gravityScale = 0;
        }

        if (body.interpolation == RigidbodyInterpolation2D.Interpolate) {
          body.interpolation = RigidbodyInterpolation2D.Extrapolate;
        }

        float xIncrement =  Constants.hurtCXTransitions[hurtCounter >= Constants.hurtCXTransitions.Length ? Constants.hurtCXTransitions.Length - 1 : hurtCounter];
        float yIncrement =  Constants.hurtCYTransitions[hurtCounter >= Constants.hurtCYTransitions.Length ? Constants.hurtCYTransitions.Length - 1 : hurtCounter];

        // TODO: weird position change happens here! ensure to check what causes it
        transform.position = new Vector2(currentXPosition + (xIncrement * direction * (hurtFromBehind ? -1 : 1)), currentYPosition + yIncrement);

        hurtCounter++;
      }
    }
  }

  void DecideAttackType(string armEquipment, int armIndex) {
    armUsed = armIndex;

    if (isGrounded) {
        if (armEquipment == "") {
          isPunching = true;
          anim.SetTrigger("isPunching");
          weaponCollider.SetActive(true);
        } else {
          string weaponType = Helpers.GetOrException(Objects.regularItems, armEquipment).type;

          switch (weaponType) {
            case "single":
              isAttackingSingle = true;
              weaponCollider.SetActive(true);
              anim.SetTrigger("isAttackingSingle");
            break;
            case "double":
              // TODO: consider how this gets affected while key is released and input is decreasing to zero
              if (verticalInput < -Constants.inputThreshold) {
                isParrying = true;
              } else {
                isAttackingHeavy = true;
                // TESTING FOR PROGRAMMATIC PLAY
                // anim.Play("attack-longsword-1", -1, 0f);
                weaponCollider.SetActive(true);
                anim.SetTrigger("isAttackingHeavy");
              }
            break;
            case "throwable":
            case "throwable-double":
            case "throwable-food":
              isThrowing = armUsed;
              anim.SetTrigger("isThrowing");
            break;
            case "bow":
              isShootingPull = true;
            break;
            case "defense":
              if (currentShieldHP > 0) {
                isDefending = true;
              }
            break;
            default:
              Debug.Log("Case " + weaponType + " is not accounted for");
            break;
          }
        }
      } else if (isJumping || isFalling) {
        if (armEquipment == "") {
          isAirPunching = true;
          weaponCollider.SetActive(true);
          anim.SetTrigger("isAirPunching");
        } else {
          string weaponType = Helpers.GetOrException(Objects.regularItems, armEquipment).type;

          switch (weaponType) {
             case "single":
              isAirAttackSingle = true;
              weaponCollider.SetActive(true);
              anim.SetTrigger("isAirAttackSingle");
            break;
            case "double":
              isAirAttackHeavy = true;
              weaponCollider.SetActive(true);
              anim.SetTrigger("isAirAttackHeavy");
            break;
            default:
              Debug.Log("Case " + weaponType + " is not accounted for");
            break;
          }
        }
      }
  }

  void DecideShieldRelease(string armEquipment) {
    if (Helpers.IsValueInArray(Constants.shields, armEquipment)) {
      isDefending = false;
    }
  }

  public void FlipPlayer(bool hasBeenHurt = false) {
    if (hasBeenHurt && isFacingLeft) {
      transform.localScale = Vector3.one;
    } else {
      transform.localScale = new Vector3(-1, 1, 1);
    }
  }

  void PlayerDying(bool isGrounded) {
    isDead = isGrounded ? 1 : 2;
    ClearInvulnerability();
  }

  void PlayerHurt(int hurtLevel) {
    body.velocity = Vector2.zero;
    isHurt = hurtLevel;
    hurtCounter = 0;

    if (hurtLevel > 1) {
      currentXPosition = transform.position.x;
      currentYPosition = transform.position.y;
    }

    if (hurtLevel == 3) {
      throwbackHeight = 2f;
    }
  }

  void ConsumeProjectile(string key) {
    Item projectileUsed = items.FirstOrDefault(currItem => currItem.key == key);
    if (projectileUsed.amount == 1) {
      items.Remove(projectileUsed);

      // removes item from the equipment
      RegularItem projectileItem = Helpers.GetOrException(Objects.regularItems, key);

      // clears equipment once items run out
      switch (projectileItem.type) {
        case "throwable-double":
          arm1Equipment = "";
          arm2Equipment = "";
        break;
        case "arrow":
          arm1Equipment = "";
          arm2Equipment = "";
          projectileEquipment = "";
          ChangeArrowContainerSprite(clear: true);
        break;
        default:
          if (isThrowing == 1) {
            arm1Equipment = "";
          } else {
            arm2Equipment = "";
          }
        break;
      }

      //recalculates stats after item removal
      UpdateStatsValues();
    } else {
      projectileUsed.amount--;
    }
  }

  void StartThrow() {
    string throwableType = Helpers.GetRegularItemKeyByName(Helpers.GetOrException(Objects.regularItems, isThrowing == 1 ? arm1Equipment : arm2Equipment).name);

    float xModifier = 1;
    if (throwableType == "axe") {
      xModifier = isFacingLeft ? 2 : 0;
    } else if (throwableType == "king-bone") {
      xModifier = isFacingLeft ? 2.25f : 0.25f;
    }

    float yModifier = 0.75f;
    if (throwableType == "axe") {
      yModifier = 0.5f;
    } else if (throwableType == "king-bone") {
      yModifier = 0.625f;
    }

    float throwableX = transform.position.x + (direction * heroWidth * xModifier);
    float throwableY = transform.position.y + (heroHeight * yModifier);

    GameObject throwableWeapon = Instantiate(Helpers.GetOrException(Objects.prefabs, "throwable"), new Vector3(throwableX, throwableY, 0), Quaternion.identity);
    Throwable throwableInstance = throwableWeapon.GetComponent<Throwable>();

    throwableInstance.isFacingLeft = isFacingLeft;
    throwableInstance.type = throwableType;
    ConsumeProjectile(throwableType);
  }

  public void Recover() {
    isHurt = 0;
    hurtCounter = 0;
    body.gravityScale = 1;
    body.interpolation = RigidbodyInterpolation2D.Interpolate;
  }

  void ClearPunch() {
    isPunching = false;
    weaponCollider.SetActive(false);
  }

  void ClearAirPunch() {
    isAirPunching = false;
    weaponCollider.SetActive(false);
  }

  void ClearAttackSingle() {
    isAttackingSingle = false;
    armUsed = 0;
    weaponCollider.SetActive(false);
  }

  void ClearAirAttackSingle() {
    isAirAttackSingle = false;
    armUsed = 0;
    weaponCollider.SetActive(false);
  }

  void ClearAirAttackHeavy() {
    isAirAttackHeavy = false;
    armUsed = 0;
    weaponCollider.SetActive(false);
  }

  // void ClearAirShooting() {
  //   isAirShooting = false;
  // }

  void ClearKick() {
    isKicking = false;
    weaponCollider.SetActive(false);
  }

  void ClearThrow() {
    isThrowing = 0;
  }

  // void ClearShootingSingle() {
  //   isShootingSingle = false;
  // }

  // TODO: rather than creating and firing at different times in the animation, ensure the refactored arrow script fires immediately after creating (when the player sprite changes to release the bow)
  void CreateArrow() {
    Vector2 containerPosition = transform.Find("ArrowContainer").transform.position;
    GameObject currentArrow = Instantiate(Helpers.GetOrException(Objects.prefabs, "arrow"), new Vector2(containerPosition.x, containerPosition.y), Quaternion.identity);
    Arrow arrowScript = currentArrow.GetComponent<Arrow>();
    arrowScript.isFacingLeft = isFacingLeft;
    arrowScript.type = projectileEquipment;
    ConsumeProjectile(projectileEquipment);
  }

  void ShowCurrentArrowMask() {
    // arrowMask.GetComponent<SpriteMask>().enabled = true;
  }

  void FireArrow() {
    // Destroy(arrowMask);
    // arrowScript.hasFired = true;
  }

  void ClearShootingPull() {
    isShootingPull = false;
  }

  void AdjustGroundType() {
    if (tempGroundType != "") {
      groundType = tempGroundType;
      tempGroundType = "";
    }
  }

  void ClearAttackHeavy() {
    isAttackingHeavy = false;
    armUsed = 0;
    weaponCollider.SetActive(false);
    AdjustGroundType();
    // TESTING FOR PROGRAMMATIC PLAY
    // anim.Play("idle-1", -1, 0f);
  }

  void DropDefense() {
    isDefending = false;
  }

  public void Clash() {
    isClashing = true;
  }

  public void FinishCeilingCollision() {
    isCollidingWithCeiling = false;
    Fall();
  }

  void DropParry() {
    body.velocity = Vector2.zero;

    isClashing = false;
    isParrying = false;
  }

  public void OnGUI() {
    string guiLabel = showDebug ? "HP: " + currentHP + "\n" +
                      "Defending: " + isDefending + "\n" +
                      "Parrying: " + isParrying + "\n" +
                      "Clashing: " + isClashing + "\n" +
                      "Running: " + isRunning + "\n" +
                      "Grounded: " + isGrounded + "\n" +
                      "Falling: " + isFalling + "\n" +
                      "Jumping: " + isJumping + "\n" +
                      // "Gliding: " + isGliding + "\n" +
                      // "JetpackUp: " + isJetpackUp + "\n" +
                      // "JetpackHorizontal: " + (jetpackHorizontal != "" ? jetpackHorizontal : "none") + "\n" +
                      "horizontalCollision: " + horizontalCollision + "\n" +
                      // "Equipment: " + currentWeapon + "\n" +
                      "Attack_Single: " + isAttackingSingle + "\n" +
                      "Attack_Heavy: " + isAttackingHeavy + "\n" +
                      "Air_Attack_Single: " + isAirAttackSingle + "\n" +
                      "Air_Attack_Heavy: " + isAirAttackHeavy + "\n" +
                      // "Air_Shooting: " + isAirShooting + "\n" +
                      "Kick: " + isKicking + "\n" +
                      "Drop_Kick: " + isDropKicking + "\n" +
                      "Punching: " + isPunching + "\n" +
                      "Air_Punch: " + isAirPunching + "\n" +
                      "Throwing: " + (isThrowing > 0) + "\n" +
                      // "Shooting: " + (isShootingSingle || isShootingAuto || isShootingPull || isAirShooting) + "\n" +
                      "Shooting: " + isShootingPull + "\n" +
                      "Shield HP: " + currentShieldHP + "\n" : "";
    GUI.Label(new Rect(0, 0, 200, 400), guiLabel);
  }

  private void Fall() {
    ToggleAirCheck(true);

    isGrounded = false;
    isFalling = true;
    // TESTING FOR PROGRAMMATIC PLAY
    // anim.Play("falling-1", -1, 0f);
    DropDefense();
  }

  public void Jump(bool clearDropKick = false) {
    // resets collision with ceiling to avoid that animation upon starting jump
    isCollidingWithCeiling = false;

    ToggleAirCheck(true);

    if (clearDropKick) {
      isDropKicking = false;
      isFalling = false;
      canFlipOnAir = true;
      weaponCollider.SetActive(false);
    }

    body.velocity = new Vector2(body.velocity.x, jumpHeight);

    isJumping = true;
    // TESTING FOR PROGRAMMATIC PLAY
    // anim.Play("jumping-1", -1, 0f);
    isGrounded = false;
  }

  // private void Glide() {
  //   isGliding = true;
  // }

  // private void JetpackUp() {
  //   body.velocity = new Vector2(body.velocity.x, jetpackHeight);
  //   isJetpackUp = true;
  //   isJumping = false;
  //   isGrounded = false;
  // }

  // private void JetpackHorizontal(string direction) {
  //   jetpackHorizontal = direction;
  //   jetpackTime = Time.time * 1000;
  //   currentYPosition = transform.position.y;
  //   isJumping = false;
  //   isGrounded = false;
  // }

  private void DropKick() {
    canFlipOnAir = false;
    isDropKicking = true;
    weaponCollider.SetActive(true);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    string colTag = col.gameObject.tag;

    if (colTag == "Incline" && !collidingTop) {
      groundType = col.gameObject.GetComponent<Incline>().inclineFromRight;
    } else if (colTag == "Zone") {
      Zone zoneScript = col.gameObject.GetComponent<Zone>();

      ZoneSpecs currZoneSpecs = Helpers.GetOrException(Objects.zoneSpecs, zoneScript.type);

      if (currZoneSpecs.jumpHeight != null) {
        jumpHeight = (float) currZoneSpecs.jumpHeight;
      }

      if (currZoneSpecs.animSpeed != null) {
        anim.speed = (float) currZoneSpecs.animSpeed;
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    string colTag = col.gameObject.tag;

    if (colTag == "Incline" && !isFalling) {
      groundType = "level";
    } else if (colTag == "Zone") {
      if (!Helpers.Intersects(heroCollider, col.gameObject.GetComponent<PolygonCollider2D>())) {
        jumpHeight = GameData.playerJumpHeight;
        anim.speed = 1;
      }
    }
  }

  // gets the ground collision direction based on different states of movement:
  // - if falling, it should always be bottom collision
  // - if jumping, it should never be bottom collision
  //   - check front. If colliding, check step over. If it fails, bump back
  //   - If front is not colliding, check back. If colliding, BuildCompression forward
  // TODO: no top value is yet returned as no issues have arisen from the top. Investigate for possible scenarios
  private string GetGroundCollisionDirection() {
    if (!isFalling) {
      if (isJumping) {
        if (collidingFront) {
          return "front";
        } else if (collidingBack) {
          return "back";
        }
      }
    }

    return "bottom";
  }

  public void SetCollisionDirection(string direction, bool collidingValue) {
    switch (direction) {
      case "top":
        collidingTop = collidingValue;
      break;
      case "front":
        collidingFront = collidingValue;
      break;
      case "bottom":
        collidingBottom = collidingValue;
      break;
      case "back":
        collidingBack = collidingValue;
      break;
    }
  }

  public void ModifyPosition(Vector2 newPosition) {
    transform.position = newPosition;
  }

  public void StepOver(float stepOverHeight) {
    ModifyPosition(new Vector2(transform.position.x + (heroWidth * direction), transform.position.y + stepOverHeight));
    ToggleAirCheck(false);
    jumpsExecuted = 0;
    isGrounded = true;
    blockedDirection = "";
    isFalling = false;
    isJumping = false;
  }

  // moves the player back a bit to ensure behavior is correct
  public void Bump(float bumpX = 0, float bumpY = 0, string specificBlockDirection = "") {
    // blocks the direction bumped into to avoid continous bumping
    if (!isFalling) {
      isFalling = true;
    }
    blockedDirection = specificBlockDirection != "" ? specificBlockDirection : (isFacingLeft ? "left" : "right");
    ModifyPosition(new Vector2(transform.position.x - (bumpX * -direction) * direction, transform.position.y + bumpY));
  }

  private void MainCollisionLogic(Collider2D collider, Collider2D otherCollider, string colTag) {
    if (Helpers.IsValueInArray(Constants.landingObjects, colTag)) {
      if (otherCollider.tag == "Hero") {
        if (!isHorizontalCollision(otherCollider, collider)) {
          if (collider.tag == "Floor" && isFalling) {
            PerformGroundFall();
          } else if  (collider.tag == "Breakable") {
            // TODO: This will fail for barrels. Prepare falling sound for barrels
            PlayFallingSound(collider.gameObject.GetComponent<Breakable>().type, Helpers.GetOrException(Objects.equipmentBaseMaterial, bodyEquipment));
          } else if (collider.tag == "Interactable") {
            // TODO: for now, box sounds appear to work fine. If interactables made of non-wood material are implemented, consider changing this
            PlayFallingSound("box", "boots");
          }

          ToggleAirCheck(false);
          jumpsExecuted = 0;
          isGrounded = true;
          blockedDirection = "";
          // TODO: Consider if it's ever necessary to perform y position modification to ensure it gets rounded
          // transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y));
          isFalling = false;
          isJumping = false;
          // isJetpackUp = false;
          horizontalCollision = false;
          isDropKicking = false;

          if (isHurt == 3) {
            Recover();
          }

          // disable air attack animations if these haven't finished when player hits ground
          isAirPunching = false;
          // isAirShooting = false;
          isAirAttackSingle = false;
          isAirAttackHeavy = false;

          weaponCollider.SetActive(false);
        } else {
          horizontalCollision = Helpers.IsValueInArray(Constants.nonHorizontalCollidableObjects, colTag) ? true : false;

          if (isBottomCollision(otherCollider, collider)) {
            horizontalCollision = false;
            ClearAirAttackSingle();
          }
        }
      }
    }
  }

  public bool isFightingBoss = false;

  private void OnCollisionEnter2D(Collision2D col) {
    Collider2D collider = col.collider;
    Collider2D otherCollider = col.otherCollider;
    string colTag = col.gameObject.tag;

    if (colTag == "DamageExplosion" && !col.gameObject.GetComponent<Explosion>().hasDamaged) {
      ReceiveExplosionDamage(col.gameObject, col.collider.ClosestPoint(transform.position));
      col.gameObject.GetComponent<Explosion>().hasDamaged = true;
    } else {
      // TODO: consider the use of collisionDirection and remove it if not needed
      collisionDirection = GetGroundCollisionDirection();

      // only toggle the isCollidingWithCeiling flag if the player has collided while jumping, i.e. not falling
      if (collidingTop && !isFalling) {
        isCollidingWithCeiling = true;
      } else {
        MainCollisionLogic(collider, otherCollider, colTag);

        if (IsOnIncline() && isFalling) {
          GroundOnIncline();
        }
      }
    }
  }

  public void ReceiveThrowable(GameObject throwable, Vector2 contactPoint) {
    Throwable throwableInstance = throwable.GetComponent<Throwable>();

    float currentX = transform.position.x;
    float throwableX = throwable.transform.position.x;
    string throwableType = throwableInstance.type;
    float criticalRate = throwableInstance.criticalRate;
    bool isCritical = Helpers.IsCritical(criticalRate);

    ReceiveFlyingWeapon(contactPoint, currentX, throwableX, throwableType, criticalRate, isCritical);
  }

  public void ReceiveProjectile(GameObject projectile, Vector2 contactPoint) {
    float currentX = transform.position.x;
    float throwableX = projectile.transform.position.x;

    // TODO: ensure properties for other enemy throwables besides the king-bone are implemented, along with different sounds for impact
    ReceiveFlyingWeapon(contactPoint, currentX, throwableX, type: "king-bone", criticalRate: 0, isCritical: false);
  }

  public void ReceiveFlyingWeapon(Vector2 contactPoint, float currentX, float flyingX, string type, float criticalRate, bool isCritical) {
    hurtFromBehind = (currentX < flyingX && isFacingLeft) || (currentX > flyingX && !isFacingLeft);
    bool mustTakeDamage = (!isDefending || (isDefending && hurtFromBehind)) && (!isParrying || (isParrying && hurtFromBehind));

    if (hurtFromBehind) {
      FlipPlayer(true);
    }

    int flyingDamage = Helpers.GetDamage(type);

    if (mustTakeDamage) {
      int damage = (stamina + (int)equippedSTA + (int)effectSTA) - (flyingDamage * (isCritical ? 2 : 1));
      TakeDamage(damage < 0 ? Math.Abs(damage) : Constants.minimumDamageDealt, contactPoint, isCritical, Helpers.GetOrException(Objects.throwableImpactType, type));

      if (currentHP > 0) {
        PlayerHurt(isGrounded ? 2 : 3);
      } else {
        PlayerDying(isGrounded);
      }
    } else {
      if (isDefending) {
        int shieldDefense = armUsed == 1 ? equippedDEF1 : equippedDEF2;

        if (flyingDamage <= shieldDefense) {
          currentShieldHP--;
        } else {
          DropDefense();
          currentShieldHP--;
          int damage = (stamina + (int)equippedSTA + shieldDefense + (int)effectSTA) - (flyingDamage * (isCritical ? 2 : 1));
          TakeDamage(damage < 0 ? Math.Abs(damage) :  Constants.minimumDamageDealt, contactPoint, isCritical, Helpers.GetOrException(Objects.throwableImpactType, type));

          if (currentHP > 0) {
            PlayerHurt(isGrounded ? 2 : 3);
          } else {
            PlayerDying(isGrounded);
          }
        }
      }

      if (currentShieldHP == 0) {
        shieldDropTime = Time.time * 1000;
        DropDefense();
      }
      if (isParrying) {
        Clash();
      }
    }
  }

  public void ReceiveExplosionDamage(GameObject explosion, Vector2 contactPoint) {
    Explosion explosionScript = explosion.GetComponent<Explosion>();

    ReceiveAttack(contactPoint, xPosition: explosionScript.transform.position.x, atk: explosionScript.damage, attackType: "", criticalRate: -1);
  }

  public void ReceiveEnemyAttack(GameObject enemy, Vector2 contactPoint, bool bewitch = false) {
    Enemy enemyScript = enemy.GetComponent<Enemy>();

    ReceiveAttack(contactPoint, xPosition: enemyScript.transform.position.x, atk: enemyScript.atk, attackType: enemyScript.normalAttackType, criticalRate: enemyScript.criticalRate,  isAttacking: enemyScript.isAttacking, enemyScript, bewitch);
  }

  public void ReceiveAttack(Vector2 contactPoint, float xPosition, int atk, string attackType, float criticalRate, bool isAttacking = true, Enemy enemyScript = null, bool bewitch = false) {
    if (isAttacking) {
      float currentX = transform.position.x;
      float enemyX = xPosition;

      hurtFromBehind = (currentX < enemyX && isFacingLeft) || (currentX > enemyX && !isFacingLeft);

      bool mustTakeDamage = (!isDefending || (isDefending && hurtFromBehind)) && (!isParrying || (isParrying && hurtFromBehind));

      if (hurtFromBehind) {
        FlipPlayer(true);
      }

      if (mustTakeDamage) {
        bool isCritical = bewitch ? true : Helpers.IsCritical(criticalRate);
        int damage = bewitch ? -(currentHP - Constants.minimumDamageDealt) : (stamina + (int)equippedSTA + (int)effectSTA) - (atk * (isCritical ? 2 : 1));
        // TODO: modify first argument based on different attack type used by the enemy
        TakeDamage(damage < 0 ? Math.Abs(damage) : (damage == 0 && bewitch ? 0 : Constants.minimumDamageDealt), contactPoint, isCritical, attackType);

        if (currentHP > 0) {
          PlayerHurt(isGrounded ? 2 : 3);
        } else {
          PlayerDying(isGrounded);
        }
      } else {
        if (isDefending) {
          int shieldDefense = armUsed == 1 ? equippedDEF1 : equippedDEF2;

          if (bewitch || atk <= shieldDefense) {
            InGame.instance.Block(shieldCollider.transform.position, !isFacingLeft);
            currentShieldHP--;
          } else {
            DropDefense();
            currentShieldHP--;
            bool isCritical = Helpers.IsCritical(criticalRate);
            int damage = (stamina + (int)equippedSTA + shieldDefense + (int)effectSTA) - (atk * (isCritical ? 2 : 1));
            // TODO: modify first argument based on different attack type used by the enemy
            TakeDamage(damage < 0 ? Math.Abs(damage) :  Constants.minimumDamageDealt, contactPoint, isCritical, attackType);

            if (currentHP > 0) {
              PlayerHurt(isGrounded ? 2 : 3);
            } else {
              PlayerDying(isGrounded);
            }
          }
        }

        if (currentShieldHP == 0) {
          shieldDropTime = Time.time * 1000;
          DropDefense();
        }
        if (isParrying) {
          InGame.instance.Block(weaponCollider.transform.position, !isFacingLeft);
          Clash();
          if (enemyScript != null) {
            enemyScript.stunOnAttack = true;
          }
        }
      }
    }
  }

  public void ReceiveSmashWave(int damage, Vector2? damagePosition = null) {
    int damageReceived = (stamina + (int)equippedSTA) - damage;
    int actualDamage = damageReceived < 0 ? Math.Abs(damageReceived) : Constants.minimumDamageDealt;
    TakeDamage(actualDamage, damagePosition);
    if (currentHP > actualDamage) {
      PlayerHurt(3);
    } else {
      // TODO: change to false once the dead-2 animation is implemented
      PlayerDying(isGrounded: true);
    }
  }

  public void TakeDamage(int damage, Vector2? damagePosition = null, bool? isCritical = false, string soundType = "") {
    // if the damage received is greater or equal than the current HP, the display width must be the current HP
    int damageToDisplay = damage;
    if (damage >= currentHP) {
      damageToDisplay = currentHP;
    }

    currentHP -= damage;

    if (currentHP < 0) {
      currentHP = 0;
    }

    GameObject barDecrement = Instantiate(Helpers.GetOrException(Objects.prefabs, "bar-decrement"), Vector2.zero, Quaternion.identity);
    int damageWidth = (maxHP > Constants.maxHPDisplayableLimit ? (int)(Constants.maxHPDisplayableLimit * ((float)damageToDisplay/(float)maxHP)) : damageToDisplay) * Constants.containerMultiplier + (int)Constants.hpAdjustDifference;
    barDecrement.transform.SetParent(InGame.instance.hpBarContainer.transform, false);
    barDecrement.GetComponent<BarDecrement>().width = damageWidth;
    barDecrement.GetComponent<BarDecrement>().type = "hp";

    if (Settings.showDamage) {
      // TODO: consider changing the Hero colliders for damage so the connecting point is higher
      Vector2 position = damagePosition == null ? new Vector2(transform.position.x, transform.position.y + heroHeight / 2) : new Vector2(damagePosition.Value.x, damagePosition.Value.y + heroHeight / 2);
      InGame.instance.DrawDamage(position, damage, isCritical, soundType);
    }

    if (!isInvulnerable) {
      damageStartTime = Time.time * 1000;
      isInvulnerable = true;
      body.mass = 0;
    }

    // TODO: for testing purposes. Remove once magic can be spent by other means
    SpendMagic(damage);
  }

  public void SpendMagic(int value) {
    // if the value spent is greater than or equal to the current MP, the display width must be the current MP
    int mpSpentToDisplay = value;
    if (value >= currentMP) {
      mpSpentToDisplay = currentMP;
    }

    currentMP -= value;

    if (currentMP < 0) {
      currentMP = 0;
    }

    if (canCastMagic) {
      GameObject barDecrement = Instantiate(Helpers.GetOrException(Objects.prefabs, "bar-decrement"), Vector2.zero, Quaternion.identity);
      int mpSpentWidth = (maxMP > Constants.maxMPDisplayableLimit ? (int)(Constants.maxMPDisplayableLimit * ((float)mpSpentToDisplay/(float)maxMP)) : mpSpentToDisplay) * Constants.containerMultiplier + (int)Constants.mpAdjustDifference;
      barDecrement.transform.SetParent(mpBarContainer.transform, false);
      barDecrement.GetComponent<BarDecrement>().width = mpSpentWidth;
      barDecrement.GetComponent<BarDecrement>().type = "mp";
    }
  }

  private bool isBottomCollision(Collider2D collider1, Collider2D collider2) {
    int c1BottomEdge = (int) collider1.bounds.max.y;
    int c2TopEdge = (int) collider2.bounds.min.y;

    return c1BottomEdge == c2TopEdge;
  }

  private bool isHorizontalCollision(Collider2D collider1, Collider2D collider2) {
    int c1RightEdge = (int) collider1.bounds.max.x;
    int c1LeftEdge = (int) collider1.bounds.min.x;

    int c2RightEdge = (int) collider2.bounds.max.x;
    int c2LeftEdge = (int) collider2.bounds.min.x;

    return (c1RightEdge == c2LeftEdge) || (c1LeftEdge == c2RightEdge);
  }

  public void CheckLevel() {
    if (exp >= next) {
      InGame.instance.ToggleSoundtrack(isPaused: false);
      LevelUp();
    }
  }

  public void LevelUp() {
    playerLevel++;
    SetupStatsByLevel();
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.notificationSounds, "levelup"), transform.position);
    InGame.instance.fanfareCanvas.SetActive(true);
    SetPauseCase("level-up");
    InGame.instance.fanfareCanvas.GetComponent<FanfareCanvas>().ShowLevelUp();
  }

  public void GetRelic() {

    InGame.instance.ToggleSoundtrack(isPaused: false);
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.notificationSounds, "levelup"), transform.position);
    InGame.instance.fanfareCanvas.SetActive(true);
    SetPauseCase("got-relic");
    InGame.instance.fanfareCanvas.GetComponent<FanfareCanvas>().ShowGetRelic();
  }

  public void PlayerDeath() {
    SetPauseCase("death");
    InGame.instance.fadeOutCanvas.SetActive(true);
    InGame.instance.StartFadeOutAndPause();
  }

  public void SetPauseCase(string newPauseCase) {
    pauseCase = newPauseCase;
    Time.timeScale = 0;
  }

  public void ClearPauseCase(bool resumeSoundtrack = false, bool waitIfLevelingUp = false) {
    if (pauseCase == "got-relic") {
      InGame.instance.PlaySound(Helpers.GetOrException(Sounds.itemPickSounds, "normal"), transform.position);
    }

    pauseCase = "";
    Time.timeScale = 1;

    if (resumeSoundtrack) {
      InGame.instance.ToggleSoundtrack(isPaused: true, restart: false, wait: waitIfLevelingUp);
    }
  }

  public void SetAction(string action) {
    InGame.instance.actionCanvas.GetComponent<ActionCanvas>().SetAction(action);
  }

  public bool SatisfiesCondition(Condition nodeCondition) {
    switch (nodeCondition.conditionCheck) {
      case "items":
        string[] itemsToCheck = nodeCondition.conditionValue.Split(',');

        return Helpers.HasAll(items, itemsToCheck);
      case "money":
        int moneyValue;

        if (int.TryParse(nodeCondition.conditionValue, out moneyValue)) {
          return gold >= moneyValue;
        } else {
          return false;
        }
      default:
        Debug.Log("Returning false for unknown case: check=" + nodeCondition.conditionCheck + ", value=" + nodeCondition.conditionValue);
        return false;
    }
  }

  public ChatLine[] GetChatLines(string npcKey, string nodeKey) {
    ChatNode currentNode = Helpers.GetOrException(Helpers.GetOrException(Chat.chatNodes, npcKey), nodeKey);

    if (currentNode.nodeCondition.conditionCheck == "") {
      UpdateChatNode(npcKey, nodeKey);
      return currentNode.nodeLines;
    } else {
      if (SatisfiesCondition(currentNode.nodeCondition)) {
        UpdateChatNode(npcKey, nodeKey);
        return currentNode.nodeLines;
      } else {
        return GetChatLines(npcKey, currentNode.fallbackNode);
      }
    }
  }

  // TODO: player and/or NPC should change their current sprite to the appropriate emotion sprite
  public void OpenChat() {
    string npcKey = Helpers.PascalToKebab(NPCnearby);
    NPC currentNPC = GameObject.Find(NPCnearby).GetComponent<NPC>();
    currentNPC.DecideFlip(transform.position);
    ChatCanvas chatCanvasScript = InGame.instance.chatCanvas.GetComponent<ChatCanvas>();

    chatCanvasScript.chatLines = GetChatLines(npcKey, Helpers.GetOrException(npcNodes, npcKey));
    chatCanvasScript.startingNPC = npcKey;
    chatCanvasScript.nextNode = Helpers.GetOrException(Helpers.GetOrException(Chat.chatNodes, npcKey), Helpers.GetOrException(npcNodes, npcKey)).nextNode;

    // closes the action canvas when the chat canvas activates
    InGame.instance.actionCanvas.SetActive(false);

    // if the info canvas is active, then it should return to its left alignment
    if (InGame.instance.infoCanvas.activeSelf) {
      InGame.instance.infoCanvas.GetComponent<InfoCanvas>().AlignLeft();
    }

    // resets the action canvas so when the chat closes and it should show again, it won't show at full width
    InGame.instance.actionCanvas.GetComponent<ActionCanvas>().ClearAction();

    InGame.instance.chatCanvas.SetActive(true);
    isOnChat = true;
    chatCanvasScript.StartChat();
  }

  // TODO: other properties, such as changing the emotion sprites, should be done inside below
  public void CloseChat() {
    InGame.instance.chatCanvas.SetActive(false);
    isOnChat = false;
    proximityCheckScript.DecideActionShow();
  }

  public void UpdateChatNode(string npcKey, string newNodeKey) {
    npcNodes[npcKey] = newNodeKey;
  }

  public void InstantiateLoss(string prefabKey, bool isItem, string multiplierText, Sprite itemLossImage) {
    Vector2 lossPosition = new Vector2(transform.position.x + (((heroWidth / 2) + (Constants.itemLossWidth / 2)) * direction), transform.position.y + heroHeight);

    GameObject lossObject = Instantiate(Helpers.GetOrException(Objects.prefabs, prefabKey), lossPosition, Quaternion.identity);
    ItemLoss itemLossScript = lossObject.GetComponent<ItemLoss>();
    itemLossScript.multiplierText = multiplierText;
    itemLossScript.isItem = isItem;
    itemLossScript.alignRight = isFacingLeft;

    if (isItem) {
      itemLossScript.itemLossImage = itemLossImage;
    }
  }

  public void FinishActionFromWallBump() {
    isJumping = false;
    isDropKicking = false;
    isFalling = true;
    body.velocity = Vector2.zero;
    Bump(bumpX: (heroWidth * -direction) / 4, specificBlockDirection: isFacingLeft ? "left" : "right");
  }

  public void KeepGroundType() {
    tempGroundType = groundType == "level" ? "" : groundType;
  }

  public float GroundFallDistance() {
    Vector2 fallCast = new Vector2(transform.position.x, transform.position.y);
    // TODO: consider the tradeoffs of using an infinite downward raycast
    RaycastHit2D fallRayCast = Physics2D.Raycast(fallCast, Vector2.down, Mathf.Infinity);

    // DEBUG for FALLING: draws the cast to seek fallable objects
    Debug.DrawRay(fallCast, Vector2.down.normalized * Mathf.Infinity, Helpers.GetOrException(Colors.raycastColors, "search"));

    // TODO: Consider if at any point it could be necessary to add walls
    if (fallRayCast.collider && Helpers.IsValueInArray(Constants.landingObjects, fallRayCast.collider.tag)) {
      return fallRayCast.distance;
    }

    return 0;
  }

  public void ChangeArrowContainerSprite(bool clear = false) {
    transform.Find("ArrowContainer").GetComponent<SpriteRenderer>().sprite = clear ? null : Helpers.GetOrException(Sprites.arrows, projectileEquipment);
  }
}
