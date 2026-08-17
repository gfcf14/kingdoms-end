using UnityEngine;

public class Constants {
  public static string preferredInput = "gamepad";
  public static string[][] categoryItemTypeArray = {
    new string[] {"defense", "double", "single"},
    new string[] {"arrow", "bow", "throwable", "throwable-double"},
    new string[] {"neck"},
    new string[] {"armwear"},
    new string[] {"ring"},
    new string[] {"food", "throwable-food"},
    new string[] {"consumable"},
    new string[] {"valuable"},
  };

  public static string[] elements = {"air", "dark", "earth", "fire", "ice", "light", "lightning", "water"};

  public static string[] statEffects = {"critical-flask", "luck-flask", "stamina-flask", "strength-flask"};
  public static string[] magicDamageEffects = {"air-1", "air-2", "air-3", "dark-1", "dark-2", "dark-3", "earth-1", "earth-2", "earth-3", "fire-1", "fire-2", "fire-3", "ice-1", "ice-2", "ice-3", "light-1", "light-2", "light-3", "lightning-1", "lightning-2", "lightning-3", "water-1", "water-2", "water-3"};
  public static string[] consumableEffects = {"air-infusion", "dark-infusion", "earth-infusion", "fire-infusion", "ice-infusion", "light-infusion", "lightning-infusion", "water-infusion"};

  public static string[] variableEnemies = { "luxhusk", "nomad", "wanderess" };
  public static string[] fragmentableThrowables = { "watermelon" };
  public static string[] fragmentableProjectiles = { "leprechaun-mushroom" };
  public static string[] nonBouncingThrowables = { "lance", "dagger", "kunai", "shuriken-4", "shuriken-6", "hatchet", "axe", "skeleton-king-giant-bone", "coconut", "honeydew", "watermelon" };
  public static string[] nonBouncingProjectiles = {
    "angel-blast",
    "archangel-blast",
    "archdemon-blast",
    "archeia-blast",
    "blob-ectoplasm",
    "botarosa-scale",
    "brazenman-dagger",
    "bulgae-fang",
    "bunyip-tooth",
    "canivernus-fang",
    "crone-dagger",
    "cusith-fang",
    "cyclops-hillstone",
    "demon-blast",
    "dryad-twig",
    "dunestiff-fang",
    "dwarf-cobble",
    "elf-dagger",
    "empusa-claw",
    "entman-twig",
    "fairy-blast",
    "faun-horn",
    "flygmy-blast",
    "frostbird-orb",
    "frostman-rock",
    "fiendlord-blast",
    "glupus-fang",
    "gnome-truffle",
    "goblin-knife",
    "golem-boulder",
    "harpy-feather",
    "hellhound-fang",
    "hippocampus-scale",
    "imp-dagger",
    "janissary-yatagan",
    "jorogumo-leg",
    "jotunn-spike",
    "kabouter-cobble",
    "kappa-carcass",
    "karasu-feather",
    "kelpie-fin",
    "kitsune-kunai",
    "knight-dagger",
    "leatherwing-fang",
    "leprechaun-mushroom",
    "luxhusk-ball",
    "mamluk-dagger",
    "menehune-shingle",
    "mermaid-scale",
    "merman-scale",
    "monsman-rock",
    "morlock-eloinoggin",
    "mosswyn-dagger",
    "mummy-rib",
    "myrsel-scale",
    "naga-scale",
    "nereid-seashell",
    "neret-orb",
    "nixie-cattail",
    "nightmare-spark",
    "nomad-dagger",
    "nymph-acorn",
    "ocugoyle-blast",
    "ogre-stump",
    "pegasus-feather",
    "petroman-rock",
    "phoenix-orb",
    "pishtaco-vertebra",
    "pixie-fireball",
    "samodiva-stalagtip",
    "sandman-sandrock",
    "saraph-scale",
    "scarabkin-horn",
    "selkie-scale",
    "seraphim-blast",
    "shangsen-dart",
    "siren-feather",
    "skeleton-bone",
    "skelewing-orb",
    "skullguard-dagger",
    "snowman-snowball",
    "succubus-blast",
    "sugecapre-fang",
    "sylphid-feather",
    "thalassoman-scale",
    "thunderbird-orb",
    "troll-boulder",
    "unicorn-shard",
    "vampire-blast",
    "wanderess-hatchet",
    "waterblade-scale",
    "werewolf-fang",
    "yanmabel-stinger",
    "yukionna-kunai",
    "zombie-bone"
  };
  public static string[] smallRotatingThrowables = { "shuriken-4", "shuriken-6", "hatchet" };
  public static string[] nonSymmetricalThrowables = { "hatchet", "dagger" };

  // TODO: consider if magic medicines, infusions, etc. should also be included here
  public static string[] itemsFromRandomFlask = {"critical-flask", "luck-flask", "stamina-flask", "strength-flask"};

  // TODO: to avoid disabling medicine use, ensure a new item type, consumable-med, is included, and that it can be used even while sealed
  public static string[] usableItemTypes = {"consumable", "food", "throwable-food"};

  public static string[] bodyEquipmentTypes = {"body"};
  public static string[] armEquipmentTypes = {"double", "single", "defense", "throwable-double", "throwable", "bow", "throwable-food"};

  public static string[] throwableTypes = {"throwable", "throwable-double", "throwable-food"};
  public static string[] neckEquipmentTypes = {"neck"};
  public static string[] armwearEquipmentTypes = {"armwear"};
  public static string[] ringEquipmentTypes = {"ring"};

  public static string[] doubleHandedWeaponTypes = {"double", "throwable-double", "bow"};
  public static string[] projectileHoldingWeaponTypes = {"bow"};
  public static string[] shields = {"basic-shield"};
  public static string[] itemContainerStates = {"items", "equipment_select", "relics"};
  public static string[] stackableBreakables = {"barrel", "box"};
  public static string[] landingObjects = {"Breakable", "Floor", "Interactable"};

  // TODO: consider why it could still be needed to keep `Breakable` as part of the nonHorizontalCollidableObjects, doing so prevents the player from pushing breakables too far
  public static string[] nonHorizontalCollidableObjects= {"Breakable", "Interactable"};
  public static string[] enemyNonColliderNames = {"Enemy", "EnemyCollider", "Grounder"};
  public static string[] enemyThrowableBouncers = {"Hero", "Shield", "Weapon"};
  public static string[] proximityCheckNonColliderTags = {"Breakable", "Enemy", "Floor", "Wall", "Interactable", "Item"};
  public static string[] droppableNonColliderNames = {"ProximityCheck", "ChestOpener", "Grounder"};
  public static string[] droppableNonColliderTags = {"Enemy", "Hero"};

  public static string[] smallThrowables = {"dagger", "kunai", "shuriken-4", "shuriken-6"};
  public static string[] angledThrowables = {"skeleton-king-giant-bone", "hatchet", "axe"};
  public static string[] rotatingThrowables = {"axe", "bluecap-rock", "hatchet", "redcap-rock", "skeleton-king-giant-bone", "shuriken-4", "shuriken-6"};
  public static string[] rotatingProjectiles = {"angel-blast", "archangel-blast", "archdemon-blast", "archeia-blast", "blob-ectoplasm", "cyclops-hillstone", "demon-blast", "dyrgja-hatchet", "fairy-blast", "fiendlord-blast", "flygmy-blast", "frostbird-orb", "golem-boulder", "luxhusk-ball", "mummy-rib", "neret-orb", "pishtaco-vertebra", "phoenix-orb", "pixie-fireball", "sandman-sandrock", "seraphim-blast", "skeleton-bone", "skelewing-orb", "thunderbird-orb", "troll-boulder", "vampire-blast"};
  public static string[] nonGroundableThrowables = {"skeleton-king-giant-bone", "axe", "bomb"};
  // refers to enemies who, when killed in normal conditions, will jump back a bit before dying
  public static string[] flyingDeathEnemies = {"botarosa", "crone", "dwarf", "dyrgja", "elf", "fairy", "flygmy", "gnome", "goblin", "imp", "kabouter", "kappa", "leprechaun", "menehune", "mosswyn", "pixie", "scarabkin", "skeleton"};
  // refers to enemies who can naturally fly
  public static string[] flyingEnemies = {"fairy", "frostbird", "leatherwing", "neret", "ocugoyle", "phoenix", "pixie", "saraph", "siren", "skelewing", "sylphid", "thunderbird", "yanmabel", "waterblade"};
  // refers to enemies who are normally on the ground but can naturally fly
  public static string[] wingedEnemies = {"angel", "archangel", "archdemon", "archeia", "demon", "fiendlord", "harpy", "karasu", "scarabkin", "seraphim", "succubus"};
  // refers to enemies who are mostly grounded but by unique means can fly
  public static string[] aerialEnemies = {"shangsen", "vampire"};
  // refers to beast like enemies whose width is a lot bigger than their height
  public static string[] longEnemies = {"bulgae", "bunyip", "canivernus", "centaur", "cusith", "dunestiff", "glupus", "hellhound", "hippocampus", "kelpie", "nightmare", "pegasus", "sugecapre", "unicorn"};
  // refers to very small enemies where regular cast lengths may not work correctly
  public static string[] smallEnemies = {"blob", "botarosa", "crone", "dwarf", "dyrgja", "elf", "fairy", "flygmy", "gnome", "imp", "kabouter", "leprechaun", "menehune", "mosswyn", "pixie", "scarabkin"};
  // large flying enemies - to avoid floating them too high given their dimensions
  public static string[] largeFlyingEnemies = {"siren", "sylphid"};

  // ITEM GROUPS
    public static string[] berries = {"blueberries", "raspberries", "strawberry"};
    public static string[] seafood = {"calamari", "ceviche", "fish", "shrimp-tail", "sushi"};
    public static string[] seaGlass = {"blue-sea-glass", "green-sea-glass", "red-sea-glass", "yellow-sea-glass"};
    public static string[] lowLevelFood = {"chicken-drumstick", "apple", "banana", "orange", "pear", "strawberry", "cherry", "grapes", "mango"};
    public static string[] lowLevelMoney = {"money-20", "money-50"};
    public static string[] midLevelFood = {"pineapple", "coconut", "honeydew", "watermelon", "wine-demijohn"};

    // TODO: wine is on high level food array as placeholder. Remove once better food items are implemented
    public static string[] highLevelFood = {"wine-demijohn"};
    public static string[] midLevelMoney = {"money-100", "money-200", "money-500"};

    // TODO: silver bracelet is on low level bracelets array as placeholder. Remove once worse bracelets are implemented
    public static string[] lowLevelBracelets = {"silver-bracelet"};
    public static string[] lowLevelPotions = {"potion", "magic-ampoule"};

    // TODO: moonlight-pendant should not have such low stats. Remove from this list when worse pendants have been implemented
    public static string[] lowLevelPendants = {"moonlight-pendant"};
    public static string[] midLevelPotions = {"mid-potion", "magic-vial"};
    public static string[] highLevelMoney = {"money-1000", "money-2000"};

  // END ITEM GROUPS
  public static string[] recalculatableItemKeys = {"berries", "high-food", "high-money", "low-bracelets", "low-food", "low-money", "low-pendants", "low-potions", "mid-food", "mid-money", "mid-potions", "seafood", "sea-glass"};
  public static string[] moneyItemKeys = {"money-20", "money-50", "money-100", "money-200", "money-500", "money-1000", "money-2000"};
  public static string[] rotateDirections = {"west", "east"};

  public static string[] nonGradientAreas = {"hellscape", "skyway", "underground"};

  // TODO: consider if this animation list would be necessary
  public static string[] heroNonFallingAnimations = {"start-running-1", "stop-running-1"};

  public static string[] wanderableEnemies = {"champion", "charger", "exploder"};

  public static string[] nonBoundariedEnemies = {"ambusher", "bomber", "bouncer", "idler", "sentinel", "shooter"};
  public static string[] flyingEnemyTypes = {"bomber", "bouncer"};

  public static string[] explosionsWithColliders = {"arrow", "bomb", "damage"};

  public static string[] enemyBombTriggerTags = {"DamageExplosion", "EnemyBomb", "Floor", "Wall"};

  public static string[] enemyBombBounceTags = {"Hero", "Weapon"};

  public static string[] enemyProjectiles = {
    "angel-blast",
    "archangel-blast",
    "archdemon-blast",
    "archeia-blast",
    "blob-ectoplasm",
    "bluecap-rock",
    "botarosa-scale",
    "brazenman-dagger",
    "bulgae-fang",
    "bunyip-tooth",
    "canivernus-fang",
    "centaur-spear",
    "crone-dagger",
    "cusith-fang",
    "cyclops-hillstone",
    "demon-blast",
    "dryad-twig",
    "dunestiff-fang",
    "dwarf-cobble",
    "dyrgja-hatchet",
    "elf-dagger",
    "empusa-claw",
    "entman-twig",
    "fairy-blast",
    "faun-horn",
    "fiendlord-blast",
    "flygmy-blast",
    "frostbird-orb",
    "frostman-rock",
    "glupus-fang",
    "gnome-truffle",
    "goblin-knife",
    "golem-boulder",
    "harpy-feather",
    "hellhound-fang",
    "hippocampus-scale",
    "imp-dagger",
    "janissary-yatagan",
    "jorogumo-leg",
    "jotunn-spike",
    "kabouter-cobble",
    "kappa-carcass",
    "karasu-feather",
    "kelpie-fin",
    "kitsune-kunai",
    "knight-dagger",
    "leatherwing-fang",
    "leprechaun-mushroom",
    "luxhusk-ball",
    "mamluk-dagger",
    "monsman-rock",
    "naga-scale",
    "menehune-shingle",
    "mermaid-scale",
    "merman-scale",
    "morlock-eloinoggin",
    "mosswyn-dagger",
    "mummy-rib",
    "myrsel-scale",
    "nereid-seashell",
    "neret-orb",
    "nightmare-spark",
    "nixie-cattail",
    "nomad-dagger",
    "nymph-acorn",
    "ocugoyle-blast",
    "ogre-stump",
    "pegasus-feather",
    "petroman-rock",
    "phoenix-orb",
    "pishtaco-vertebra",
    "pixie-fireball",
    "redcap-rock",
    "samodiva-stalagtip",
    "sandman-sandrock",
    "saraph-scale",
    "scarabkin-horn",
    "selkie-scale",
    "seraphim-blast",
    "shangsen-dart",
    "siren-feather",
    "skeleton-bone",
    "skelewing-orb",
    "skullguard-dagger",
    "snowman-snowball",
    "succubus-blast",
    "sugecapre-fang",
    "sylphid-feather",
    "thalassoman-scale",
    "thunderbird-orb",
    "troll-boulder",
    "unicorn-shard",
    "vampire-blast",
    "wanderess-hatchet",
    "waterblade-scale",
    "werewolf-fang",
    "yanmabel-stinger",
    "yukionna-kunai",
    "zombie-bone"
  };
  public static string[] explodingThrowables = {"angel-blast", "archangel-blast", "archdemon-blast", "archeia-blast", "demon-blast", "fairy-blast", "fiendlord-blast", "flygmy-blast", "frostbird-orb", "neret-orb", "ocugoyle-blast", "phoenix-orb", "pixie-fireball", "seraphim-blast", "skelewing-orb", "thunderbird-orb"};
  public static string[] shortCastEnemies = {"skeleton-king"};
  public static string[] nonStackableBreakables = {"jar", "vase"};
  public static string[] canBreakTags = {"DamageExplosion", "Explosion", "Weapon"};
  public static string[] lowReachingEnemies = {"fairy", "pixie", "kappa", "siren", "sylphid"};
  public static string[] partialLightRelics = {"dawn-gem", "royal-lamp"};
  public static string[] zonedAreas = {"glaciers"};
  public static string[] possibleEnemyDirections = {"left", "right"};
  public static string[] ShopActionButtonNames = {"ButtonBuy", "ButtonSell"};
  public static string[] effectPercentageKeys = {"hpPercentage", "mpPercentage", "crit", "luck"};
  public static string[] comparisonChecks = {"atk", "def", "crit", "luck"};
  public static string[] decimalComparisons = {"crit", "luck"};
  public static string[] colorChangingEnemyTypes = {"bewitcher", "champion", "exploder", "teleporter"};

  // simulates a wheel to ensure no magic advantage
  public static string[] elementWheel = {
    "earth",
    "air",
    "water",
    "fire",
    "ice",
    "lightning",
    "light",
    "dark"
  };

  // TODO: consider if this will be used again in the future. If not, delete
  // public static string[] patrollerStates = {"attack", "burning", "death", "death-by-burning", "death-by-poison", "stunned", "stunned-on-attack", "walk"};

  // ensure that these tiles are updated if the dirt position in the detail tileset is updated
  public static int[] detailDirt = { 150, 151, 158, 159 };
  public static int[] inclineMeadows = {163, 164, 165, 166, 168, 169, 170, 171};

  public static int arrowExplosionDamage = 50;
  public static int arrowPoisonDamage = 10;
  public static int kickDamage = 10;
  public static int punchDamage = 5;

  public static int minimumDamageDealt = 5;
  public static int maximumDamageDealt = 9999;
  public static int maxItemNameLength = 20;
  public static int maxChatLineLength = 400;
  public static int maxItemDescriptionLength = 120;
  public static int maxItemCount = 999;

  // indicates the maximum "whole" items the items container can visualize
  public static int maxItemContainerHeight = 13;
  public static int maxShopItemContainerHeight = 11;

  // Marks the default mandatory additional width for the Action Canvas text container
  public static int defaultActionTextContainerWidth = 76;
  public static int actionTextContainerHeight = 75;
  public static int actionTextHeight = 60;
  // TO TEST: change this value to 24 so each hour is a second (default value: 1440)
  public static int maxDayLength = 1440;

  public static float startItemY = 375;
  public static float startShopItemY = 320;
  public static float itemIncrementY = 60;

  public static float hpAdjustDifference = 15;
  public static float mpAdjustDifference = 9;
  public static float maxHPDisplayableLimit = 500;

  public static float maxMPDisplayableLimit = 500;

  public static float minimumSoundPlayElapsedTime = 0.5f;

  public static float defaultRoomHeight = 9;
  public static float defaultRoomWidth = 16;

  public static float sparkleDistanceRadius = 0.1f;

  public static float fallThreshold = 0.3f;
  public static float yAirVelocityThreshold = -0.01f;
  public static float yInclineVelocityThreshold = -0.1962f;

  public static float bomberReach = 5f;

  // ensures that each HP unit (until reaching 500) displays in 2 UI units
  public static int containerMultiplier = 2;

  public static float defaultDroppableItemHeight = 0.8f;
  public static float minimunDroppableColliderRadius = 0.27f;
  public static float fragmentOffset = 0.1f;

  public static float itemLossHeight = 0.6f;
  public static float itemLossWidth = 3.6f;

  public static float inputThreshold = 0.01575f;

  public static float sparkleParentOffset = 0.5f;
  public static float sparkleOffsetDistance = 0.11f;
  public static float enemyBombWidth = 0.8f;
  public static float enemyEdgeforwardOffset = 0.1f;

  public static float defaultPlayerJumpHeight = 8f;
  public static float defaultPlayerMovementFriction = 0;
  public static float defaultPlayerMovementSpeed = 1;

  public static int infoCanvasRightAlignOffset = 30;

  public static int relicSparkleLimit = 6;
  public static int sparkleRelativeMin = 500;
  public static int sparkleRelativeMax = 601;

  public static Vector2 frozenSpriteOffset = new Vector2(0, 1.17f);

  public static Vector2[] fragmentPositions = { new Vector2(-fragmentOffset, fragmentOffset), new Vector2(0, fragmentOffset), new Vector2(fragmentOffset, fragmentOffset),
                                                new Vector2(-fragmentOffset, 0),              new Vector2(0, 0),              new Vector2(fragmentOffset, 0),
                                                new Vector2(-fragmentOffset, -fragmentOffset), new Vector2(0, -fragmentOffset), new Vector2(fragmentOffset, -fragmentOffset)};

  public static ThrowableSpecs bounceSpecs = new ThrowableSpecs() {hDisplacement = 0.2f, initialRotationValues = new ValuePair(0, 45), maxHeight = 0.5f, rotationFactor = 4, speed = 4f, steepness = 1.25f};
}
