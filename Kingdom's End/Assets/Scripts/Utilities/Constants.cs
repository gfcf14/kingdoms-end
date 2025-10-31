using UnityEngine;

public class Constants {
  public static string preferredInput = "gamepad";
  public static string[] fragmentableThrowables = { "watermelon" };
  public static string[] nonBouncingThrowables = { "lance", "knife", "kunai", "shuriken-4", "shuriken-6", "hatchet", "axe", "skeleton-king-giant-bone", "coconut", "honeydew", "watermelon" };
  public static string[] nonBouncingProjectiles = {"bunyip-tooth", "cyclops-hillstone", "dwarf-cobble", "dryad-twig", "fairy-blast", "faun-horn", "frostbird-orb", "gnome-truffle", "goblin-knife", "hippocampus-scale", "kelpie-fin", "leprechaun-mushroom", "mermaid-scale", "merman-scale", "mummy-rib", "nereid-seashell", "neret-orb", "nixie-cattail", "nymph-acorn", "ogre-stump", "phoenix-orb", "pishtaco-vertebra", "pixie-fireball", "samodiva-stalagtip", "skeleton-bone", "skelewing-orb", "thunderbird-orb", "troll-boulder", "unicorn-shard", "werewolf-fang", "yanmabel-stinger"};
  public static string[] smallRotatingThrowables = { "shuriken-4", "shuriken-6", "hatchet" };
  public static string[] nonSymmetricalThrowables = { "hatchet", "knife" };
  public static string[] forbiddenKeys = {"Escape", "KeypadEnter", "Return", "W", "A", "S", "D", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "P"};
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

  public static string[] smallThrowables = {"knife", "kunai", "shuriken-4", "shuriken-6"};
  public static string[] angledThrowables = {"skeleton-king-giant-bone", "hatchet", "axe"};
  public static string[] rotatingThrowables = {"skeleton-king-giant-bone", "shuriken-4", "shuriken-6", "hatchet", "axe"};
  public static string[] rotatingProjectiles = {"cyclops-hillstone", "fairy-blast", "frostbird-orb", "mummy-rib", "neret-orb", "pishtaco-vertebra", "phoenix-orb", "pixie-fireball", "skeleton-bone", "skelewing-orb", "thunderbird-orb", "troll-boulder"};
  public static string[] nonGroundableThrowables = {"skeleton-king-giant-bone", "axe", "bomb"};
  public static string[] flyingDeathEnemies = {"dwarf", "fairy", "gnome", "goblin", "leprechaun", "pixie", "skeleton"};
  public static string[] flyingEnemies = {"fairy", "frostbird", "neret", "phoenix", "pixie", "skelewing", "thunderbird", "yanmabel"};
  public static string[] longEnemies = {"bunyip", "centaur", "hippocampus", "kelpie", "unicorn"};
  public static string[] smallEnemies = { "dwarf", "fairy", "gnome", "leprechaun", "pixie" };

  public static string[] lowLevelFood = {"chicken-drumstick", "apple", "banana", "orange", "pear", "strawberry", "cherry", "grapes", "mango"};
  public static string[] lowLevelMoney = {"money-50", "money-100"};
  public static string[] midLevelFood = {"pineapple", "coconut", "honeydew", "watermelon", "wine"};

  // TODO: wine is on high level food array as placeholder. Remove once better food items are implemented
  public static string[] highLevelFood = {"wine"};
  public static string[] goblinKnives = {"bandit-knife", "knife"};
  public static string[] goblinMidItem4 = {"silver-bar", "bronze-ingot"};
  public static string[] midLevelMoney = {"money-200", "money-500"};
  public static string[] goblinHighItem4 = {"silver-ingot", "gold-bar"};

  // TODO: silver bracelet is on low level bracelets array as placeholder. Remove once worse bracelets are implemented
  public static string[] lowLevelBracelets = {"silver-bracelet"};
  public static string[] lowLevelPotions = {"potion", "magic-ampoule"};

  // TODO: moonlight-pendant should not have such low stats. Remove from this list when worse pendants have been implemented
  public static string[] lowLevelPendants = {"moonlight-pendant"};
  public static string[] midLevelPotions = {"mid-potion", "magic-vial"};
  public static string[] highLevelPotions = {"high-potion", "magic-bottle"};
  public static string[] recalculatableItemKeys = {"goblin-high-item4", "goblin-knives", "goblin-mid-item4", "high-food", "high-potions", "low-bracelets", "low-food", "low-money", "low-pendants", "low-potions", "mid-food", "mid-money", "mid-potions"};
  public static string[] moneyItemKeys = {"money-50", "money-100", "money-200", "money-500", "money-1000", "money-2000", "money-5000"};
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

  public static string[] enemyProjectiles = {"bunyip-tooth", "centaur-spear", "cyclops-hillstone", "dryad-twig", "dwarf-cobble", "fairy-blast", "faun-horn", "frostbird-orb", "gnome-truffle", "goblin-knife", "hippocampus-scale", "kelpie-fin", "leprechaun-mushroom", "mermaid-scale", "merman-scale", "mummy-rib", "nereid-seashell", "neret-orb", "nixie-cattail", "nymph-acorn", "ogre-stump", "phoenix-orb", "pishtaco-vertebra", "pixie-fireball", "samodiva-stalagtip", "skeleton-bone", "skelewing-orb", "thunderbird-orb", "troll-boulder", "unicorn-shard", "werewolf-fang", "yanmabel-stinger"};
  public static string[] explodingThrowables = {"fairy-blast", "frostbird-orb", "neret-orb", "phoenix-orb", "pixie-fireball", "skelewing-orb", "thunderbird-orb"};
  public static string[] shortCastEnemies = {"skeleton-king"};
  public static string[] nonStackableBreakables = {"jar", "vase"};

  public static string[] canBreakTags = {"DamageExplosion", "Explosion", "Weapon"};

  public static string[] lowReachingEnemies = {"fairy", "pixie"};

  public static string[] partialLightRelics = {"dawn-gem", "royal-lamp"};

  public static string[] zonedAreas = {"glaciers"};

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
  public static int maxItemDescriptionLength = 120;
  public static int maxItemCount = 999;

  // indicates the maximum "whole" items the items container can visualize
  public static int maxItemContainerHeight = 13;

  // Marks the default mandatory additional width for the Action Canvas text container
  public static int defaultActionTextContainerWidth = 76;
  public static int actionTextContainerHeight = 75;
  public static int actionTextHeight = 60;
  // TO TEST: change this value to 24 so each hour is a second (default value: 1440)
  public static int maxDayLength = 1440;

  public static float startItemY = 375;
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

  public static Vector2[] fragmentPositions = { new Vector2(-fragmentOffset, fragmentOffset), new Vector2(0, fragmentOffset), new Vector2(fragmentOffset, fragmentOffset),
                                                new Vector2(-fragmentOffset, 0),              new Vector2(0, 0),              new Vector2(fragmentOffset, 0),
                                                new Vector2(-fragmentOffset, -fragmentOffset), new Vector2(0, -fragmentOffset), new Vector2(fragmentOffset, -fragmentOffset)};

  public static ThrowableSpecs bounceSpecs = new ThrowableSpecs() {hDisplacement = 0.2f, initialRotationValues = new ValuePair(0, 45), maxHeight = 0.5f, rotationFactor = 4, speed = 4f, steepness = 1.25f};
}
