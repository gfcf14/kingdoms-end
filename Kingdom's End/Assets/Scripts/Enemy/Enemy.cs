using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour {
  // Serialized
    [SerializeField] public string key;
    [SerializeField] public string specificDrop;
    [SerializeField] public string summonKey;
    [SerializeField] public int level;
    [SerializeField] public int currentHP;
    [SerializeField] public int maxHP;
    [SerializeField] public int atk;
    [SerializeField] public int def;
    [SerializeField] public float criticalRate;
    [SerializeField] public bool isOnCamera = false;
    [SerializeField] public bool isMiniBoss = false;

  // Components
    [NonSerialized] public Animator anim;
    [NonSerialized] private SimpleFlash flashEffect;
    [NonSerialized] public Rigidbody2D body;
    [NonSerialized] public SpriteRenderer enemyRenderer;
    [SerializeField] public GameObject variableObject;

    private AudioSource audioSource;

  // Properties
    [NonSerialized] public bool isFacingLeft = false;
    [NonSerialized] public string forceDirection = "";
    [NonSerialized] public bool needsCoolDown = false;
    [NonSerialized] public bool diesFlying = false;
    [NonSerialized] public bool isFlyingEnemy = false;
    [NonSerialized] public bool isWingedEnemy = false;
    [NonSerialized] public bool isAerialEnemy = false;

    [NonSerialized] public float arrowBurnPosition = 0;
    [NonSerialized] public float attackedStart = 0;
    [NonSerialized] public float burnTime = 0;
    [NonSerialized] public float consecutiveAttackTime = 5000;
    [NonSerialized] public float coolDownStart = 0;
    [NonSerialized] public float coolDownTime = 750;
    [NonSerialized] public float enemyHeight = 0f;
    [NonSerialized] public float enemyWidth = 0f;
    [NonSerialized] public float edgeCastLength = 0;
    [NonSerialized] public float poisonEffectTime = 0;
    [NonSerialized] public float poisonTime = 0;


    [NonSerialized] public int deadAnimationIncrement = 0;
    [NonSerialized] public int poisonAttackCounter = 1;


    [NonSerialized] string[] elementResistances;


    public Color enemyColor;


    [NonSerialized] public Vector2 deadPosition;


    [NonSerialized] public float burningDuration = 3000;
    [NonSerialized] public float distanceToPlayer;
    [NonSerialized] public float poisonAttackInterval = 600;
    [NonSerialized] public float poisonEffectDuration = 50;
    [NonSerialized] public float reach;
    [NonSerialized] public float defenseReach;
    [NonSerialized] public float longReach;
    [NonSerialized] public float speed;

    [NonSerialized] public int attacksReceived = 0;
    [NonSerialized] public int attackRetaliationCounter = 3;
    [NonSerialized] public int exp;
    [NonSerialized] public int maxPoisonAttacks = 3;
    [NonSerialized] public int maxThrows = 3;
    [NonSerialized] public int maxThrowCounter = 0;


    [NonSerialized] public string enemyName;
    [NonSerialized] public string type;
    [NonSerialized] public string form;
    [NonSerialized] public string baseMaterial;
    [NonSerialized] public string normalAttackType;

  // Game Properties
    [NonSerialized] public bool attackedFromBehind = false;
    [NonSerialized] public bool gaveExp = false;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isAttackingMelee = false;
    [SerializeField] public bool isBurning = false;
    [SerializeField] public bool isCharging = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] public bool isDeadByBurning = false;
    [SerializeField] public bool isDeadByFreezing = false;
    [SerializeField] public bool isDeadByPoison = false;
    [SerializeField] public bool isDefending = false;
    [SerializeField] public bool isDistracted;
    [SerializeField] public bool isDying = false; // should encompass all death or death-leading states (isBurning, isDeadByBurning, isDeadByPoison)
    [SerializeField] public bool isExploding = false;
    [SerializeField] public bool isHitting = false; // provisional variable to only detect the moment the enemy attacks
    [SerializeField] public bool isPoisoned = false;
    [SerializeField] public bool isStunned = false;
    [SerializeField] public bool isSummoning = false;
    [SerializeField] public bool isTeleporting = false;
    [SerializeField] public bool isThrowingWeapon = false;
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isWatching;
    [SerializeField] public bool stunOnAttack = false;

    [SerializeField] public bool bombReturned = false;
    [SerializeField] public bool canLand = false;

    public int direction = 1;
    public int yDirection = 0;

    bool mustTakeDamage = true;
    bool bossCausingLevelUp = false;

    bool isSmallEnemy = false;
    bool isVariableEnemy = false;

  // Player Related Properties
    [NonSerialized] public bool playerFound = false;
    [NonSerialized] SpriteRenderer weaponSpriteRenderer;
    [SerializeField] public GameObject spawnedFrom;

    [NonSerialized] GameObject extra;
  [NonSerialized] GameObject originator;

    Vector2 forwardCastDirection;
    public float wanderStart = 0;
    public float wanderTime = 5000;

  // TODO: ensure the watch time increases as enemy level increases, and distract time decreases as enemy level increases
    public float watchStart = 0;
    public float watchTime = 5000;
    public float distractStart = 0;
    public float distractTime = 3000;

    private float leftBound = 0;
    private float rightBound = 0;

    Vector2 searchPosition = Vector2.zero;

    private Func<bool> ShouldDefend;

    private bool ShouldDefendByLevelDifference() {
      return true;
    }

    private bool ShouldDefendAsChampion() {
      return attacksReceived >= attackRetaliationCounter;
    }

    private bool ShouldDefendAsHighLevel() {
      return attacksReceived >= attackRetaliationCounter;
    }

    private bool ShouldNeverDefend() {
      return false;
    }

  void FindBoundaries() {
    // find the parent room
    Transform parentRoom = transform.parent.parent;

    GameObject leftFlipper = null;
    GameObject rightFlipper = null;

    float flipperToEnemyYDifference = 0.5f;

    foreach (Transform child in parentRoom) {
      if (child.name.Contains("EnemyFlipper") && (child.position.y - flipperToEnemyYDifference == transform.position.y)) { // checks if flippers are at the same y position as enemy
        // checks the closest occurrence of bounds and assigns accordingly
          if (leftFlipper == null || child.position.x < leftFlipper.transform.position.x) {
            leftFlipper = child.gameObject;
          }

          if (rightFlipper == null || child.position.x > rightFlipper.transform.position.x) {
            rightFlipper = child.gameObject;
          }
      }
    }

    // prepares and sets bounds to whatever a established dimension by key is, ensuring the enemy is maintained within flipper bounds in case of position manipulation (e.g. teleportation)
      float offsetByType = Helpers.GetOrException(Objects.enemyDimensions, key).x;

      leftBound = leftFlipper.transform.position.x + offsetByType;
      rightBound = rightFlipper.transform.position.x - offsetByType;
  }

  public void FloatEnemy() {
    transform.position = new Vector2(transform.position.x, transform.position.y + (Helpers.GetOrException(Objects.enemyDimensions, key).y * (Helpers.IsValueInArray(Constants.largeFlyingEnemies, key) ? 0.5f : 1)));
    body.gravityScale = 0;
    transform.Find("Grounder").gameObject.SetActive(false);
  }

  void Start() {
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    enemyRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();

    flashEffect = GetComponent<SimpleFlash>();
    weaponSpriteRenderer = GameObject.Find("Weapon").GetComponent<SpriteRenderer>();
    extra = transform.Find("Extra").gameObject;
    originator = transform.Find("Originator").gameObject;

    isFacingLeft = forceDirection != "" ? forceDirection == "left" : Hero.instance.transform.position.x < transform.position.x;

    if (isFacingLeft) {
      Flip();
    }

    if (!Hero.instance.isAutonomous) {
      // TODO: consider if this only needs to be applied to enemies that walk
      if (type != "exploder") {
        isWalking = true;
      }

      if (type == "shooter") {
        isWatching = true;
      }
    }

    elementResistances = new string[] {};

    // TODO: consider removing the enemy color from resistances to use effects instead
    enemyColor = Helpers.GetColorFromResistances(elementResistances);
    flashEffect.repaintColor = enemyColor;
    enemyRenderer.color = enemyColor;

    EnemyStats enemyStats = Helpers.GetOrException(Objects.enemyStats, key);
    form = enemyStats.form;
    enemyName = enemyStats.name + " Lvl " + level;
    baseMaterial = enemyStats.baseMaterial;
    normalAttackType = enemyStats.normalAttackType;

    atk = Helpers.GetStatsOnEnemyLevel(enemyStats.atk, level);
    def = Helpers.GetStatsOnEnemyLevel(enemyStats.def, level);
    currentHP = Helpers.GetStatsOnEnemyLevel(enemyStats.hp, level);
    maxHP = Helpers.GetStatsOnEnemyLevel(enemyStats.hp, level);
    criticalRate = Helpers.GetStatsOnEnemyLevel(enemyStats.crit, level);

    exp = enemyStats.exp;
    speed = enemyStats.speed;
    reach = enemyStats.reach;
    longReach = enemyStats.longReach;

    edgeCastLength = enemyStats.edgeCastLength;
    arrowBurnPosition = enemyStats.arrowBurnPosition;
    body.mass = enemyStats.mass;

    diesFlying = Helpers.IsValueInArray(Constants.flyingDeathEnemies, key);
    isFlyingEnemy = Helpers.IsValueInArray(Constants.flyingEnemies, key);
    isWingedEnemy = Helpers.IsValueInArray(Constants.wingedEnemies, key);
    isAerialEnemy = Helpers.IsValueInArray(Constants.aerialEnemies, key);

    if (type == "patroller") {
      gameObject.AddComponent<Patroller>();
    } else if (type == "champion") {
      gameObject.AddComponent<Champion>();
    } else if (type == "idler") {
      gameObject.AddComponent<Idler>();
    } else if (type == "charger") {
      gameObject.AddComponent<Charger>();
    } else if (type == "sentinel") {
      gameObject.AddComponent<Sentinel>();
    } else if (type == "teleporter") {
      gameObject.AddComponent<Teleporter>();
    } else if (type == "shooter") {
      gameObject.AddComponent<Shooter>();
    } else if (type == "exploder") {
      gameObject.AddComponent<Exploder>();
    } else if (type == "bouncer") {
      gameObject.AddComponent<Bouncer>();
    } else if (type == "bomber") {
      gameObject.AddComponent<Bomber>();
    } else if (type == "ambusher") {
      gameObject.AddComponent<Ambusher>();
    } else if (type == "bewitcher") {
      gameObject.AddComponent<Bewitcher>();
    }

    // perform a check for an enemy which has the same type and key, thus it has a prepared animator with states
    bool animatorAlreadyExists = false;
    // gets a list of all Enemies usable
    GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy").Where(obj => obj.name.Contains("Enemy") && obj.name != "EnemyCollider").ToArray();

    foreach (GameObject objectWithAnimator in enemyObjects) {
      Enemy objectEnemyScript = objectWithAnimator.GetComponent<Enemy>();

      // if it is not the current gameObject and the type and key are the same and the animator and runtimeAnimatorController are not null, make a copy of that object's animator controller
      if (objectEnemyScript != gameObject && objectEnemyScript.type == type && objectEnemyScript.key == key && objectEnemyScript.GetComponent<Animator>() != null && objectEnemyScript.GetComponent<Animator>().runtimeAnimatorController != null) {
        anim.runtimeAnimatorController = objectWithAnimator.GetComponent<Animator>().runtimeAnimatorController;
        animatorAlreadyExists = true;
        break;
      }
    }

    // if no such object (same type and key) was found, instantiate a new copy and assign clips based on key to states
    if (!animatorAlreadyExists) {
      AnimatorOverrideController aoc = new AnimatorOverrideController(Instantiate(Helpers.GetOrException(Objects.animationControllers, type)));
      AnimatorOverrideController resourceAoc = new AnimatorOverrideController(InGame.instance.GetEnemyAnimatorControllerByForm(form));

      var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
      foreach (AnimationClip a in aoc.animationClips) {
        string stateName = a.name.Split('_')[1];

        // TODO: consider if this is even needed or if it's easier to replicate the nymph_death animation into a nymph_death-by-poison animation
        // Find the corresponding resource animation clip based on the enemy key and state name
        AnimationClip resourceClip = resourceAoc.animationClips.FirstOrDefault(
            // TODO: Consider if it's better to lookup the video by state name, since some enemies reuse animations (e.g. waterblade uses waterblade_idle.anim for both idle and watch)
            resourceClip => resourceClip.name == key + "_" + stateName ||
            (key == "nymph" && stateName == "death-by-poison" && resourceClip.name == "nymph_death") // nymph edge case since she uses nymph_death for regular death, and poison/burning death
        );

        // Add the original and new animation clip pair to the list
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, resourceClip));

        // anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, resourceAoc.animationClips.FirstOrDefault(
        //   resourceClip => resourceClip.name == key + "_" + stateName ||
        //   (key == "nymph" && stateName == "death-by-poison" && resourceClip.name == "nymph_death") // nymph edge case since she uses nymph_death for regular death, and poison/burning death
        // )));
      }
      aoc.ApplyOverrides(anims);

      anim.runtimeAnimatorController = aoc;
    }

    // Find existing boundaries prior to any displacement
    if (!Helpers.IsValueInArray(Constants.nonBoundariedEnemies, type)) {
      FindBoundaries();
    }

    // move enemy upward a bit from ground to account for flying behavior, except when it's an ambusher as that will only happen when they land
    if (isFlyingEnemy && type != "ambusher") {
      FloatEnemy();
    }

    // from the beginning, start the bouncer by flying up
    if (type == "bouncer") {
      transform.Find("Grounder").gameObject.SetActive(false);
      yDirection = 1;

      // TODO: the OnTriggerStay2D condition for bouncing to avoid leaving a room makes the enemy bounce intermittently in the beginning.
      // Check if there are other ways to fix besides moving the enemy slightly up at the beginning
      transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
    }

    if (type == "bomber")
    {
      // moves the bomber up initially to account for the reach specified
      transform.position = new Vector2(transform.position.x, transform.position.y + Constants.bomberReach);
      body.gravityScale = 0;
      transform.Find("Grounder").gameObject.SetActive(false);
    }

    if (!isFlyingEnemy && !isWingedEnemy && !isAerialEnemy && Helpers.IsValueInArray(Constants.flyingEnemyTypes, type)) {
      // instantiates wings only only enemies who cannot naturally fly get wings and based on a position offset ("cheaper" to store a Vector2 than a Sprite object in a Dictionary)
      Vector2 wingOffset = Helpers.GetOrException(Objects.enemyWingOffsets, key);
      GameObject wings = Instantiate(Helpers.GetOrException(Objects.prefabs, "enemy-wings"), new Vector2(transform.position.x + wingOffset.x * (isFacingLeft ? -1 : 1), transform.position.y + wingOffset.y), Quaternion.identity, transform);

      Color wingColor = Helpers.GetOrException(Colors.wingsColors, GameData.area);
      wings.GetComponent<SpriteRenderer>().color = wingColor;

      // changes the color of the fly brace (the Extra)
      extra.GetComponent<SpriteRenderer>().color = wingColor;
    }

    isSmallEnemy = Helpers.IsValueInArray(Constants.smallEnemies, key);
    isVariableEnemy = Helpers.IsValueInArray(Constants.variableEnemies, key);

    // defines conditions to defend if a weapon is incoming:
    if (level - Hero.instance.playerLevel >= 10) { //  1. If the level difference is greater than or equal to 10 (stronger enemies according to story should feel impossible to defeat!)
      ShouldDefend = ShouldDefendByLevelDifference;
    } else if (type == "champion") { //  2. If it's a champion after receiving too many attacks
      ShouldDefend = ShouldDefendAsChampion;
    } else if (level >= 30) { //  3. If it's any other enemy after level 30 and receiving too many attacks
      ShouldDefend = ShouldDefendAsHighLevel;
    } else { // do not defend otherwise
      ShouldDefend = ShouldNeverDefend;
    }
  }

  void AwardExp() {
    gaveExp = true;
    int expToAward = Helpers.GetEnemyEXP(heroLevel: Hero.instance.playerLevel, enemyLevel: level, baseExp: exp);
    Hero.instance.exp += expToAward;
    bossCausingLevelUp = Hero.instance.exp >= Hero.instance.next;
    Hero.instance.CheckLevel();
  }

  public void PlaySound(AudioClip sound) {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(sound);
    }
  }

  // TODO: consider if enemies should constantly be making sound (e.g. walking)
  public void PlayRunningSound() {
    // string materialRunningOn = InGame.instance.GetTileMaterial(transform.position);

    // if (materialRunningOn != null) {
    //   AudioClip[] materialClips = Helpers.GetOrException(Helpers.GetOrException(Sounds.runningSounds, materialRunningOn), baseMaterial);
    //   PlaySound(materialClips[UnityEngine.Random.Range(0, materialClips.Length)]);
    // }
  }

  public void PlayAttackSound() {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.attackSounds, normalAttackType));
    }
  }

  void Freeze() {
    InGame.instance.PlaySound(Sounds.iceblockSound, transform.position);

    if (!gaveExp) {
      AwardExp();
    }

    Destroy();
  }

  void Update() {
    if (enemyRenderer != null) {
      enemyHeight = enemyRenderer.bounds.size.y;
      enemyWidth = enemyRenderer.bounds.size.x;
      defenseReach = enemyWidth * (isSmallEnemy ? 4 : 2); ;
    }

    if (!isWalking && !Hero.instance.isAutonomous && type != "exploder") {
      isWalking = true;
    }

    // TODO: consider how to optimize these. Maybe set them only when isFacingLeft changes
    direction = isFacingLeft ? -1 : 1;
    forwardCastDirection = transform.TransformDirection(type == "bomber" ? new Vector2(0, -1) : new Vector2(direction, 0));

    isDying = isBurning || isDeadByBurning || isDeadByPoison;

    if (Hero.instance.isAutonomous && isMiniBoss) {
      // TODO: initially this code was meant to flip an enemy boss when the player enters on his own.
      //       But given a similar condition on start could make this invalid. Test if it's still needed.
      // enemyRenderer.sprite = Helpers.GetOrException(Sprites.firstBossSprites, key);
      // isFacingLeft = !Hero.instance.isFacingLeft;
      // if (isFacingLeft) {
      //   Flip();
      // }
    } else {
      if ((isMiniBoss && isOnCamera) || gameObject.name != "Boss") {
        // DEFENSE CAST
        Vector2 defenseCast = new Vector2(transform.position.x + (enemyWidth * direction * -(isSmallEnemy ? 2 : 1)), transform.position.y + enemyHeight / 2 + 0.05f);
        Vector2 defenseCastDirection = transform.TransformDirection(new Vector2(1 * (direction), 0));

        RaycastHit2D defenseRayCast = Physics2D.Raycast(defenseCast, defenseCastDirection, defenseReach);
        Debug.DrawRay(defenseCast, defenseCastDirection.normalized * defenseReach, Helpers.GetOrException(Colors.raycastColors, "defense"));

        if (defenseRayCast && defenseRayCast.collider.tag == "Weapon") {
          if (ShouldDefend()) {
            Defend();
          }
        }


        if (Hero.instance.pauseCase == "") {
          // ENEMY BURNING
            if (isBurning) {
              if (Helpers.ExceedsTime(burnTime, burningDuration)) {
                isBurning = false;
                isDeadByBurning = true;
                // so flying enemies drop dead
                body.gravityScale = 1;
                transform.Find("Grounder").gameObject.SetActive(true); // so flying enemies can hit the ground while dropping
              }

              // make HP 0 so it reflects upon dying by burning in the boss status canvas
              currentHP = 0;
            }

          // ENEMY BURNING
            if (!isBurning) {
              if (isFacingLeft) {
                transform.localScale = new Vector3(-1, 1, 1);
              } else {
                transform.localScale = Vector3.one;
              }
            }

          // RESET ATTACKS RECEIVED
            if (Helpers.ExceedsTime(attackedStart, consecutiveAttackTime)) {
              attacksReceived = 0;
            }

          // UPDATE ANIMATOR PARAMETERS BASED ON TYPE
            if (type == "patroller") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isStunned", isStunned);
              anim.SetBool("isStunnedOnAttack", stunOnAttack);
              anim.SetBool("isWalking", isWalking);
            } else if (type == "champion") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isAttackingMelee", isAttackingMelee);
              anim.SetBool("isStunned", isStunned);
              anim.SetBool("isStunnedOnAttack", stunOnAttack);
              anim.SetBool("isSummoning", isSummoning);
              anim.SetBool("isThrowingWeapon", isThrowingWeapon);
              anim.SetBool("isWalking", isWalking);
            } else if (type == "idler") {
              anim.SetBool("isAttacking", isAttacking);
            } else if (type == "charger") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isCharging", isCharging);
              anim.SetBool("isWalking", isWalking);
            } else if (type == "sentinel") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isDistracted", isDistracted);
              anim.SetBool("isWatching", isWatching);
            } else if (type == "teleporter") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isTeleporting", isTeleporting);
            } else if (type == "shooter") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isStunnedOnAttack", stunOnAttack);
              anim.SetBool("isThrowingWeapon", isThrowingWeapon);
              anim.SetBool("isWatching", isWatching);
            } else if (type == "exploder") {
              anim.SetBool("isExploding", isExploding);
              anim.SetBool("isWalking", isWalking);
              anim.SetBool("isWatching", isWatching);
            } else if (type == "bomber" || type == "bouncer") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isDeadFlying", isBurning || isDead || isDeadByBurning || isDeadByPoison);
            } else if (type == "ambusher") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isDeadInAir", (isBurning || isDead || isDeadByBurning || isDeadByPoison) && !isWatching);
              anim.SetBool("isThrowingWeapon", isThrowingWeapon);
              anim.SetBool("isWatching", isWatching);
            } else if (type == "bewitcher") {
              anim.SetBool("isAttacking", isAttacking);
              anim.SetBool("isWalking", isWalking);
            }

          // UPDATE ANIMATOR PARAMETERS FOR ALL TYPES
            anim.SetBool("isBurning", isBurning);
            anim.SetBool("isDead", isDead);
            anim.SetBool("isDeadByBurning", isDeadByBurning);
            anim.SetBool("isDeadByPoison", isDeadByPoison);
            anim.SetBool("needsCoolDown", needsCoolDown);
        }
      }
    }
  }

  void LateUpdate() {
    // ENEMY POISONED
    if (isPoisoned) {
      float currentTime = Time.time * 1000;
      float nextPoisonAttackTime = poisonTime + (poisonAttackInterval * poisonAttackCounter);

      if (currentTime > poisonEffectTime + poisonEffectDuration) {
        if (!isStunned) {
          // changes poison color back to original
          SetEnemyColor("normal");
        }

        if (poisonAttackCounter == maxPoisonAttacks + 1) {
          isPoisoned = false;
          poisonAttackCounter = 0;
        }
      }

      if (currentTime > nextPoisonAttackTime)  {
        InGame.instance.PlaySound(Helpers.GetOrException(Sounds.poisonSounds, "basic"), transform.position);
        TakeDamage(Constants.arrowPoisonDamage);
        poisonEffectTime = Time.time * 1000;
        // changes to poison color
        SetEnemyColor("poisoned");
        poisonAttackCounter++;

        if (currentHP <= 0) {
          isDeadByPoison = true;
          isWalking = false;
          body.linearVelocity = Vector2.zero;
          // so flying enemies drop dead
          body.gravityScale = 1;

          if (!isDead) { // avoids getting double exp if dying from poison after being attacked
            if (!gaveExp) {
              AwardExp();
            }
          }
        }
      }
    }

    if (Hero.instance.pauseCase == "") {
      if (isBurning || (!isPoisoned && !isStunned && !isExploding && (type == "bewitcher" && !isAttacking && !isPoisoned))) {
        enemyRenderer.color = enemyColor;
      }
    }
  }

  public void CheckAttackToPlayer(Collider2D col) {
    if (col.gameObject.tag == "Hero" && isAttacking) {
      // isAttacking = false;
      coolDownStart = Time.time * 1000;

      if (!needsCoolDown) {
        // ensures the hero isn't damaged after being damaged
        if (!Hero.instance.isInvulnerable) {
          Hero.instance.ReceiveEnemyAttack(gameObject, col.ClosestPoint(transform.position), bewitch: type == "bewitcher");
        }
        needsCoolDown = true;
      }
    }
  }

  public void Collision(Collision2D col) {
    CheckAttackToPlayer(col.collider);
  }

  public void DamageCalculation(Collider2D col, int specificDamage, string damageSoundType, string weaponType = "", bool isCritical = false) {
    int damage = def - ((specificDamage + Hero.instance.strength + (int)Hero.instance.equippedSTR + (int)Hero.instance.effectSTR) * (isCritical ? 2 : 1));

    if (Helpers.IsValueInArray(Constants.throwableTypes, weaponType) || !(isDefending && !attackedFromBehind)) {
      TakeDamage(damage < 0 ? Math.Abs(damage) : Constants.minimumDamageDealt, col.ClosestPoint(transform.position), isCritical, damageSoundType);
      if (!(weaponType == "throwable" || weaponType == "throwable-double")) {
        TurnWhenAttackedFromBehind();
      }
    } else {
      mustTakeDamage = false;
    }
  }

  public void Defend() {
    // TODO: ensure all enemy types have a means to return to isDefending = false
    isDefending = true;
    anim.SetTrigger("isDefending");
  }

  public void PerformFlyingDeath() {
    transform.Find("EnemyCollider").GetComponent<CapsuleCollider2D>().isTrigger = false;
    transform.Find("Grounder").gameObject.SetActive(false);
    body.linearVelocity = Vector2.zero;
    float horizontalDirection = -direction * (attackedFromBehind ? -1 : 1);

    float deathMass = body.mass / (Helpers.IsValueInArray(Constants.smallEnemies, key) ? 2 : 5);
    Vector2 deathForce = new Vector2(deathMass * horizontalDirection * 4, deathMass * 2.5f);
    body.linearVelocity = deathForce;
  }

  public void DeathSequence() {
    if (!isBurning) {
      isDead = true;
    }

    isPoisoned = false;
    isStunned = false;
    isWalking = false;

    if (isDead && (!isBurning || !isDeadByBurning || !isDeadByPoison) && diesFlying) {
      PerformFlyingDeath();
    } else {
      body.linearVelocity = Vector2.zero;
    }

    deadPosition = new Vector2(transform.position.x, transform.position.y);

    if (!isDeadByPoison) { // avoids getting double exp if attacking while dying from poison
      if (!gaveExp) {
        AwardExp();
      }
    }
  }

  public void Trigger(Collider2D col) {
    CheckAttackToPlayer(col);
    string colliderTag = col.gameObject.tag;

    if (colliderTag == "Weapon" && !Hero.instance.isParrying) {
      float currentX = transform.position.x;
      float enemyX = col.transform.position.x;
      bool willBurn = false;
      bool isCritical = Helpers.IsCritical(Hero.instance.criticalPercentage + Hero.instance.equippedCRIT + Hero.instance.effectCRIT);;
      string currentWeapon = "";
      int currentEquippedATK = 0;

      attackedFromBehind = (currentX < enemyX && isFacingLeft) || (currentX > enemyX && !isFacingLeft);

      if (Hero.instance.isKicking || Hero.instance.isDropKicking && !isDefending) {
        DamageCalculation(col, Constants.kickDamage, "kick", "", isCritical);
      } else {
        currentWeapon = Hero.instance.armUsed == 1 ? Hero.arm1Equipment : Hero.arm2Equipment;
        currentEquippedATK = Hero.instance.armUsed == 1 ? Hero.equippedATK1 : Hero.equippedATK2;

        if (currentWeapon == "" && !isDefending) {
          DamageCalculation(col, Constants.punchDamage, "punch", "", isCritical);
        } else {
          string weaponType = Helpers.GetOrException(Objects.regularItems, currentWeapon).type;

          if (weaponType == "single" || weaponType == "double" && !isDefending) {
            // TODO: might need to adjust to different types other than swords
            DamageCalculation(col, currentEquippedATK, "sword", weaponType, isCritical);
          } else if (Helpers.IsValueInArray(Constants.throwableTypes, weaponType)) {
            GameObject parentObject = col.transform.parent.gameObject;
            Throwable parentThrowable = parentObject.GetComponent<Throwable>();
            string weaponWielded = parentThrowable.type;

            mustTakeDamage = Helpers.IsNonBouncingThrowable(weaponWielded) && !parentThrowable.hasCollided;

            if (mustTakeDamage) {
              string throwableSoundType = Helpers.GetThrowableSoundType(currentWeapon);
              DamageCalculation(col, currentEquippedATK, throwableSoundType, weaponType, isCritical);
              Transform parentTransform = parentObject.GetComponent<Transform>();

              if(Helpers.IsNonBouncingThrowable(weaponWielded)) {
                parentThrowable.SetBounce(parentTransform, col.ClosestPoint(transform.position));
              }

              if (Helpers.IsValueInArray(Constants.fragmentableThrowables, weaponWielded)) {
                Vector2 fragmentOrigin = new Vector2(col.ClosestPoint(transform.position).x, col.ClosestPoint(transform.position).y + Helpers.GetItemDimensions(weaponWielded).y);

                InGame.instance.InstantiateFragments(Helpers.GetOrException(Objects.itemFragments, weaponWielded), fragmentOrigin, transform.parent.gameObject, isProjectile: false);
                parentThrowable.DestroyThrowable();
              } else {
                parentThrowable.collideTime = Time.time * 1000;
                parentThrowable.hasCollided = true;
                parentThrowable.maxEllapsedCollideTime = 1000f;
              }

              parentThrowable.transitionIncrement = 0;
            }
          } else if (Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, weaponType)) {
            GameObject parentObject = col.transform.parent.gameObject;
            Arrow parentArrow = parentObject.GetComponent<Arrow>();
            string arrowUsed = parentArrow.type;

            mustTakeDamage = !parentArrow.hasCollided;
            willBurn = parentArrow.type == "arrow-fire" && !Helpers.IsFireResistant(elementResistances) && currentHP <= Constants.arrowExplosionDamage;

            if (mustTakeDamage) {
              int damage = (def * (isDefending ? 2 : 1)) - ((Helpers.GetDamage(arrowUsed) + Hero.instance.strength + (int)Hero.instance.equippedSTR + (int)Hero.instance.effectSTR) * (isCritical ? 2 : 1));
              int expectedDamage = damage < 0 ? Math.Abs(damage) : Constants.minimumDamageDealt;

              if (expectedDamage >= currentHP && parentArrow.type == "arrow-ice" && !isDeadByFreezing) {
                isDeadByFreezing = true;
                Freeze();
              }

              // do not play standard damage sound if the arrow used is a fire arrow
              TakeDamage(expectedDamage, col.ClosestPoint(transform.position), isCritical, parentArrow.type == "arrow-fire" ? "" : "arrow");

              if (parentArrow.type == "arrow-poison" && !Helpers.IsPoisonResistant(elementResistances)) {
                isPoisoned = true;
                poisonTime = Time.time * 1000;
              }

              parentArrow.DestroyArrow();
            }
          } else if (isDefending && !Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, weaponType)) {
            InGame.instance.Block(col.ClosestPoint(transform.position), attackedFromBehind && isFacingLeft || !attackedFromBehind && !isFacingLeft);
          }
        }
      }

      if (mustTakeDamage) {
        if (currentHP > 0) {
          if (flashEffect != null && !isDefending) {
            Flash();
          }

          if (!willBurn) {
            // if enemy receives constant attacks, if past lvl 20 and it's a champion perform melee.
            // but if not past lvl 20 and it's not a bouncer stun
            if (attacksReceived >= attackRetaliationCounter) {
              if (level >= 20) {
                if (type == "champion") {
                  isAttackingMelee = true;
                  attackedStart = 0;
                }
              } else {
                if (type != "bouncer") {
                  Stun();
                }
              }
            } else {
              if (!isDefending) {
                string weaponType = Helpers.GetOrException(Objects.regularItems, currentWeapon).type;
                bool isProjectileWeapon = Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, weaponType);

                // TODO: for now, certain projectile weapons as specified by projectileHoldingWeaponTypes should not stun. Check the possibility of stunning without changing color
                if (!isProjectileWeapon && type != "bouncer" && type != "bewitcher") {
                  Stun();
                }
              }

              attacksReceived++;
              attackedStart = Time.time * 1000;
            }
          }
        } else {
          DeathSequence();
        }
      }

      DisplayEnemyInInfoCanvas();
    } else if (colliderTag == "Shield") {
      if (isAttacking) {
        // TODO: consider reusing for higher level shields
        // Stun();
      }

      DisplayEnemyInInfoCanvas();
    } else if (colliderTag == "Explosion") {
      string explosionType = col.gameObject.GetComponent<Explosion>().type;

      if (explosionType == "bomb") { // for bomb explosions
        int damageFromExplosion = (int) Helpers.GetOrException(Objects.regularItems, explosionType).effects.atk;
        int damage = def - damageFromExplosion;
        TakeDamage(damage < 0 ? Math.Abs(damage) : Constants.minimumDamageDealt, col.ClosestPoint(transform.position));

        if (flashEffect != null) {
          Flash();
          if (type != "bouncer") {
            Stun();
          }
        }

        // allows a bomb explosion to kill an enemy
        // TODO: if any other explosions are added, ensure to change the usage of arrowExplosionDamage
        if (currentHP <= 0 && damageFromExplosion != Constants.arrowExplosionDamage) {
          DeathSequence();
        }
      } else {
        string colName = col.gameObject.name.Replace("(Clone)", "");

        if (colName == "Explosion" || colName == "ArrowBurn") {
          bool willBurn = !Helpers.IsFireResistant(elementResistances) && currentHP <= Constants.arrowExplosionDamage;

          if (willBurn) {
            float currentTime = Time.time * 1000;

            // only instantiate the flame if the enemy is not set to die (hence, !isDying)
            if (!isDying) {
              GameObject arrowBurn = Instantiate(Helpers.GetOrException(Objects.prefabs, "arrow-burn"), new Vector2(transform.position.x, transform.position.y + arrowBurnPosition), Quaternion.identity);

              // sets the parent room so that the flame can be found and deleted more easily on room exit
              arrowBurn.transform.SetParent(transform.parent);

              ArrowBurn arrowBurnScript = arrowBurn.GetComponent<ArrowBurn>();
              arrowBurnScript.startTime = currentTime;
              arrowBurnScript.burnDimensions = Helpers.GetOrException(Objects.enemyDimensions, key);

              burnTime = currentTime;
              isBurning = true;
              // immediately sets the enemy color so the enemy looks burned
              SetEnemyColor("burned");
              isWalking = false;
              body.linearVelocity = Vector2.zero;
            }
          } else {
            if (!Helpers.IsFireResistant(elementResistances)) {
              int damage = def - Constants.arrowExplosionDamage;
              TakeDamage(damage < 0 ? Math.Abs(damage) : Constants.minimumDamageDealt, col.ClosestPoint(transform.position));

              if (flashEffect != null) {
                Flash();
                if (type != "bouncer") {
                  Stun();
                }
              }
            }
          }
        }
      }
    }
  }

  public void SetEnemyColor(string statusColor) {
    Color newStatusColor = Helpers.GetOrException(Colors.statusColors, statusColor);
    enemyRenderer.color = newStatusColor;
    enemyColor = newStatusColor;

    if (isVariableEnemy) {
      variableObject.GetComponent<VariableSprite>().SetColor(newStatusColor);
    }
  }

  public void Flash() {
    flashEffect.Flash();

    if (isVariableEnemy) {
      variableObject.GetComponent<VariableSprite>().Flash();
    }
  }

  public void DisplayEnemyInInfoCanvas() {
    if (!isMiniBoss) {
      InGame.instance.infoCanvas.GetComponent<InfoCanvas>().Display(enemyName, new EnemyHealth(currentHP, maxHP));
    }
  }

  public void TurnWhenAttackedFromBehind() {
    if (level >= 20 && attackedFromBehind) { // after level 20 enemy should be aware it's being hit from behind
      isFacingLeft = !isFacingLeft;
      Flip();
    }
  }

  public void TakeDamage(int damage, Vector2? damagePosition = null, bool? isCritical = false, string soundType = "") {
    int actualDamage = damage > Constants.maximumDamageDealt ? Constants.maximumDamageDealt : damage;
    currentHP -= actualDamage;

    if (Settings.showDamage) {
      Vector2 position = damagePosition ?? new Vector2(transform.position.x, transform.position.y + (enemyHeight / 2));
      InGame.instance.DrawDamage(position, actualDamage, isCritical, soundType);
    }
  }

  public void Flip() {
    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
  }

  // flips while also modifying isFacingLeft
  public void TurnAround() {
    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    isFacingLeft = !isFacingLeft;
  }

  public void Stun() {
    isStunned = true;
    enemyRenderer.color = Color.white;
    isWalking = false;

    body.linearVelocity = Vector2.zero;
  }

  void Recover() {
    isSummoning = false;
    isStunned = false;
    stunOnAttack = false;
    isWalking = true;
    isThrowingWeapon = false;
    distanceToPlayer = 0;
    maxThrowCounter = 0;
    isAttackingMelee = false;
    isDefending = false;
    playerFound = false;
  }

  public void PrepareWeaponThrow() {
    if (type != "shooter") {
      ThrowWeapon(distanceToPlayer);

      if (level >= 20 && maxThrowCounter < (maxThrows - 1)) {
        anim.Play("throw", -1, 0f);

        // if (maxThrowCounter % 2 != 0) {
        //   distanceToPlayer++;
        // } else {
        //   distanceToPlayer -= 2;
        // }

        if (maxThrowCounter % 2 == 0) {
          distanceToPlayer = 1;
        } else {
          distanceToPlayer = -1;
        }

        maxThrowCounter++;
      } else {
        if (type != "bouncer") {
          Stun();
        }
      }
    } else {
      ThrowProjectile();

      // TODO: consider if any other enemies should avoid the block behavior
      if (key != "archeia") {
        stunOnAttack = true;
        StunOnAttack();
      }
    }
  }

  public void FinishArrowShooting() {
    stunOnAttack = true;
    StunOnAttack();
  }

  public void StunOnAttack() {
    if (stunOnAttack) {
      if (type != "bouncer") {
        Stun();
      }
    }
  }

  public void StartHitting() {
    isHitting = true;
  }

  void FinishAttack() {
    playerFound = false;
    isAttacking = false;
    isHitting = false;
    maxThrowCounter = 0;

    if (type == "charger") {
      WanderBack();
    }

    if (type == "ambusher") {
      canLand = true;
      isWatching = true;
    }

    // checks if a RigidBody2D component exists. If not, add it
    Rigidbody2D currentBody = GetComponent<Rigidbody2D>();
    if (currentBody == null) {
      AddRigidBody();
    }
  }

  public void Destroy() {
    if (isMiniBoss) {
      // TODO: consider if it's necessary to use a var to store the room the enemy is in
      transform.parent.parent.Find("Bounds").gameObject.SetActive(false);

      // clears miniboss state if defeated once
      transform.parent.GetComponent<EnemySpawner>().isMiniBoss = false;
    }

    // prepares origin position given custom values if found
    Vector2 deathOrigin = new Vector2(transform.position.x, transform.position.y + (enemyHeight / 2)) +
      ((Objects.customEnemyDeathOriginModifiers.ContainsKey(key) ?
        Helpers.GetOrException(Objects.customEnemyDeathOriginModifiers, key) :
        Vector2.zero) * new Vector2(direction, 1));

    // instantiates the dropped item
    string[] droppableAndRarity = (specificDrop == "" ? Helpers.GetDroppableItem(key, level, Hero.instance.luckPercentage + Hero.instance.equippedLUCK + Hero.instance.effectLCK) : "" + specificDrop + "|rare").Split('|');
    if (isDeadByFreezing) {
      GameObject iceBlock = Instantiate(Helpers.GetOrException(Objects.prefabs, $"ice-block-{UnityEngine.Random.Range(1, 6)}"), deathOrigin, Quaternion.identity, transform.parent);
      IceBlock iceBlockScript = iceBlock.GetComponent<IceBlock>();
      GameObject iceBlockEnemy = iceBlockScript.enemy;
      GameObject iceBlockVariable = iceBlockScript.variable;

      iceBlockEnemy.GetComponent<SpriteRenderer>().sprite = enemyRenderer.sprite;

      if (isVariableEnemy) {
        variableObject.GetComponent<VariableSprite>().SetFrozenVariable(iceBlockVariable);
      }

      iceBlockScript.itemKey = droppableAndRarity[0];
      iceBlockScript.itemRarity = droppableAndRarity[1];
      iceBlockScript.isFacingLeft = isFacingLeft;
    } else {
      InGame.instance.InstantiatePrefab("droppable", droppableAndRarity[0], droppableAndRarity[1], transform.parent.gameObject, deathOrigin, shouldRotate: false, "", spawnedFrom);

      // instantiates the explosion of the enemy
      GameObject enemyExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), deathOrigin, Quaternion.identity);
      enemyExplosion.GetComponent<Explosion>().type = "enemy";
    }

    if (isMiniBoss) {
      GameObject.Find("BossStatusCanvas").SetActive(false);
      Hero.instance.isFightingBoss = false;

      InGame.instance.SwitchFromMiniBossTrack(GameData.area, bossCausingLevelUp);
    }

    Destroy(gameObject);
  }

  public void ThrowWeapon(float distance) {
    GameObject throwableWeapon = Instantiate(Helpers.GetOrException(Objects.prefabs, "throwable"), originator.transform.position, Quaternion.identity);
    Throwable throwableInstance = throwableWeapon.GetComponent<Throwable>();

    throwableInstance.isFacingLeft = isFacingLeft;
    throwableInstance.type = Helpers.GetOrException(Objects.projectileKeys, key);
    throwableInstance.criticalRate = criticalRate;

    // TODO: change when implementing other throwable types
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.attackSounds, "throwable-double-large"));
    }

    Transform throwableCollider = throwableWeapon.transform.Find("ThrowableCollider");
    throwableCollider.eulerAngles = Vector3.zero;
    throwableCollider.gameObject.tag = "EnemyWeapon";
  }

  public void ThrowProjectile() {
    GameObject projectile = Instantiate(Helpers.GetOrException(Objects.prefabs, "projectile"), originator.transform.position, Quaternion.identity, transform);
    Projectile projectileScript = projectile.GetComponent<Projectile>();

    projectileScript.fromFacingLeft = isFacingLeft;
    projectileScript.key = Helpers.GetOrException(Objects.projectileKeys, key);
    projectileScript.targetPoint = searchPosition;
    searchPosition = Vector2.zero;
  }

  public void Summon() {
    GameObject summonEnergy = Instantiate(Helpers.GetOrException(Objects.prefabs, "summon-energy"), new Vector3(transform.position.x + (isFacingLeft ? -1 : 1), transform.position.y, 0), Quaternion.identity);
    SummonEnergy summonScript = summonEnergy.GetComponent<SummonEnergy>();

    summonScript.summonKey = summonKey != "" ? summonKey : Helpers.GetRandomItemFromGroup(Helpers.GetOrException(Objects.enemyKeysByArea, GameData.area));
    // sets the enemy spawner as parent, for when the player leaves a room with summoned enemies so these can be destroyed
    summonScript.parent = transform.parent.gameObject;
    // TODO: define which enemy types can be summoned
    summonScript.enemyType = "patroller";
    // TODO: define, based on the enemy level and a threshold, up to what level can they summon
    summonScript.enemyLevel = 1;
  }

  public void Smash() {
    GameObject smashWave = Instantiate(Helpers.GetOrException(Objects.prefabs, "smash-wave"), new Vector3(transform.position.x + (isFacingLeft ? -2 : 2), transform.position.y, 0), Quaternion.identity);
    SmashWave smashWaveScript = smashWave.GetComponent<SmashWave>();
    smashWaveScript.width = enemyWidth * 2;
    smashWaveScript.damage = atk * 2;
  }

  public bool ShouldMove() {
    return isWalking && !isDead && !isDeadByBurning && !isDeadByPoison && !isBurning && (!isStunned || isStunned && type == "charger") && !isThrowingWeapon && !isAttackingMelee && !isDefending && !isSummoning;
  }

  public bool WillDie() {
    return isDead || isDeadByBurning || isDeadByPoison || isBurning;
  }

  public void OnGUI() {
    if (Hero.instance.showDebug) {
      string guiLabel = "HP: " + currentHP + "\n";
      GUI.Label(new Rect(600, 0, 200, 400), guiLabel);
    }

    // if (key == "skeleton-king") {
    //   string guiLabel = "Attacks received: " + attacksReceived + "\n";
    //   GUI.Label(new Rect(600, 0, 200, 400), guiLabel);
    // }
  }

  public void RemoveRigidBody() {
    Destroy(GetComponent<Rigidbody2D>());
  }

  public void AddRigidBody() {
    gameObject.AddComponent<Rigidbody2D>();
    body = GetComponent<Rigidbody2D>();
    body.constraints = RigidbodyConstraints2D.FreezeRotation;
  }

  public void CheckEdge() {
    float edgeCastX = transform.position.x + ((enemyWidth / 2) + Constants.enemyEdgeforwardOffset) * direction;
    Vector2 beginEdgeCast = new Vector2(edgeCastX, transform.position.y + enemyHeight);
    Vector2 edgeCastDirection = Vector2.down;
    float lengthWithHeight = enemyHeight + edgeCastLength;

    RaycastHit2D[] edgeCast = Physics2D.RaycastAll(beginEdgeCast, edgeCastDirection, lengthWithHeight);
    Debug.DrawRay(beginEdgeCast, edgeCastDirection * lengthWithHeight, Helpers.GetOrException(Colors.raycastColors, "edge"));

    foreach (RaycastHit2D hit in edgeCast) {
      if (hit.collider && hit.collider.name.Contains("EnemyFlipper")) {
        if (type == "charger" && isCharging) { // when the enemy charges, if it finds a flipper it should turn back, as the player is no longer in their area
          WanderBack();
        } else {
          isFacingLeft = !isFacingLeft;
          if (playerFound) {
            playerFound = false;
          }
        }

        break;
      }
    }
  }

  public void CheckCoolDown() {
    if (Helpers.ExceedsTime(coolDownStart, (coolDownTime * (type == "bomber" ? 2 : 1)))) {
      coolDownStart = 0;
      needsCoolDown = false;
      playerFound = false;
    }
  }

  public void AttackLogic(float xPositionOffset, float castLength) {
    Vector2 proximityVector = new Vector2(transform.position.x + (xPositionOffset * direction), transform.position.y + enemyHeight / 2);
    RaycastHit2D playerCast = Physics2D.Raycast(proximityVector, forwardCastDirection, castLength);
    Debug.DrawRay(proximityVector, forwardCastDirection.normalized * castLength, Helpers.GetOrException(Colors.raycastColors, "player"));

    if (playerCast && playerCast.collider.tag == "Hero" && !playerCast.collider.GetComponent<Hero>().isInvulnerable) {
      isAttacking = true;
      body.linearVelocity = Vector2.zero;
    }
  }

  public void CheckForPlayer(float forwardCastLength) {
    Vector2 forwardCast = new Vector2(transform.position.x + ((enemyWidth / 2) * direction), transform.position.y + enemyHeight / 2);
    RaycastHit2D searchCast = Physics2D.Raycast(forwardCast, forwardCastDirection, forwardCastLength);
    Debug.DrawRay(forwardCast, forwardCastDirection.normalized * forwardCastLength, Helpers.GetOrException(Colors.raycastColors, "search"));

    // if enemy stumbles upon the player
    if (searchCast && searchCast.collider.tag == "Hero") {
      playerFound = true;
      if (type == "charger") {
        // TODO: consider checking if the hero is hurt (currently enemy will charge and push an invincible player)
        isCharging = true;
      }

      // champions and ambushers of high enough level can make it more annoying by throwing stuff at player
      if (type == "champion" || type == "ambusher") {
        // TODO: uncomment once testing for all enemies is done, and level is better defined
        if (/*level >= 10 &&*/ !isDefending) {
          isThrowingWeapon = true;
        }
      }

      if (type == "exploder") {
        isExploding = true;
        isWalking = false;
        body.linearVelocity = Vector2.zero;
      }

      if (type == "bewitcher") {
        isAttacking = true;
        isWalking = false;
        body.linearVelocity = Vector2.zero;
      }
    } else { // if player is not found
      if (wanderStart == 0) {
        if (type == "champion") {
          if (level >= 50) {
            isSummoning = true;
          }
        }

        // only enemies who can wander will find their movement direction changing
        if (Helpers.IsValueInArray(Constants.wanderableEnemies, type)) {
          wanderStart = Time.time * 1000;
          isFacingLeft = UnityEngine.Random.Range(0, 2) != 0;

           // for exploders, decide if they should walk or not
          if (type == "exploder") {
            isWalking = UnityEngine.Random.Range(0, 2) != 0;

            if (!isWalking) {
              body.linearVelocity = Vector2.zero;
            }
          }
        }
      } else {
        // if already wandering, check for a time limit. If met, reset
        if (Helpers.IsValueInArray(Constants.wanderableEnemies, type)) {
          if (Helpers.ExceedsTime(wanderStart, wanderTime)) {
            wanderStart = 0;
          }
        }
      }
    }
  }

  public void DecideMovement() {
    if (ShouldMove()) {
      body.linearVelocity = new Vector2(direction * (speed * (type == "charger" && isCharging ? 2 : 1)), body.linearVelocity.y);
    } else {
      if (!isDead) {
        body.linearVelocity = Vector2.zero;
      }
    }
  }

  // resets the wander status as the enemy (charger) flips
  void WanderBack() {
    isCharging = false;
    isFacingLeft = !isFacingLeft;
    wanderStart = Time.time * 1000;
  }

  public void Watch() {
    if (type == "sentinel") {
      if (watchStart == 0 && distractStart == 0) { // if neither is active, start watching (should happen on the first time only)
        isWatching = true;
        watchStart = Time.time * 1000;
      } else if (watchStart > 0 && Helpers.ExceedsTime(watchStart, watchTime)) { // if watching time has exceeded, switch to distraction
        isWatching = false;
        watchStart = 0;
        isDistracted = true;
        distractStart = Time.time * 1000;
        TurnAround(); // flips the enemy to sell the concept of being distracted better
      } else if (distractStart > 0 && Helpers.ExceedsTime(distractStart, distractTime)) { // if distraction time has exceeded, switch back to watching
        isDistracted = false;
        distractStart = 0;
        isWatching = true;
        watchStart = Time.time * 1000;
        TurnAround(); // flips the enemy to show the enemy is alert and watching again
      }
    }
  }

  public void Teleport() {
    Vector2 teleportPosition = Vector2.zero;
    float newXPosition = UnityEngine.Random.Range(leftBound + enemyWidth, rightBound - enemyWidth);
    Vector2 teleportCastOrigin = new Vector2(newXPosition, transform.position.y + enemyHeight);
    float teleportGroundCastLength = enemyHeight + 2f; // 2 tiles down besides enemy height

    RaycastHit2D[] hits = Physics2D.RaycastAll(teleportCastOrigin, Vector2.down, teleportGroundCastLength);
    Debug.DrawRay(teleportCastOrigin, Vector2.down * teleportGroundCastLength, Helpers.GetOrException(Colors.raycastColors, "search"));

    bool foundFloor = false;

    foreach (RaycastHit2D hit in hits) {
      if (hit.collider != null && hit.collider.CompareTag("Floor")) {
        teleportPosition = new Vector2(newXPosition, hit.point.y);
        foundFloor = true;
        break;
      }
    }

    if (!foundFloor) {
      teleportPosition = new Vector2(newXPosition, transform.position.y);
    }

    if (isFlyingEnemy) {
      teleportPosition = new Vector2(teleportPosition.x, teleportPosition.y + Helpers.GetOrException(Objects.enemyDimensions, key).y);
    }

    transform.position = teleportPosition;

    playerFound = false;
    isTeleporting = false;
  }

  public void SearchPlayer(float searchCastLength)
  {
    float dynamicAngle = Mathf.PingPong(Time.time * 30f, 90f) - 45f; // Sweeps from -45° to 45°
    float angleInRadians = dynamicAngle * Mathf.Deg2Rad;
    Vector2 searchDirection = new Vector2(Mathf.Cos(angleInRadians) * direction, -Mathf.Sin(angleInRadians));
    Vector2 searchOrigin = new Vector2(transform.position.x, transform.position.y + enemyHeight / 2);

    RaycastHit2D searchCast = Physics2D.Raycast(searchOrigin, searchDirection, searchCastLength);
    Debug.DrawRay(searchOrigin, searchDirection * searchCastLength, Helpers.GetOrException(Colors.raycastColors, "player"));

    if (searchCast && searchCast.collider.tag == "Hero")
    {
      searchPosition = searchCast.point;
      isThrowingWeapon = true;
    }
  }

  public void Explode() {
    Vector2 explosionOrigin = new Vector2(transform.position.x, transform.position.y + enemyHeight / 2);

    GameObject damageExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), explosionOrigin, Quaternion.identity);
    Explosion explosionScript = damageExplosion.GetComponent<Explosion>();
    explosionScript.type = "damage";

    // currently damage is set to be 1/4 of the maximum enemy's HP, rounded down to a multiple of 5
    explosionScript.damage = ((int)(maxHP / 4) / 5) * 5;

    Destroy(gameObject);
  }

  public void Bounce() {
    if (ShouldMove()) {
      body.linearVelocity = new Vector2(speed * direction, speed * yDirection);
    } else {
      body.linearVelocity = Vector2.zero;
    }
  }

  public void DropBomb() {
    Vector2 bombOrigin = new Vector2(extra.transform.position.x + ((Constants.enemyBombWidth / 2) * direction), extra.transform.position.y);
    GameObject enemyBomb = Instantiate(Helpers.GetOrException(Objects.prefabs, "enemy-bomb"), bombOrigin, Quaternion.identity, transform);

    EnemyBomb bombScript = enemyBomb.GetComponent<EnemyBomb>();
    bombScript.damage = atk * 2;
    bombScript.dropper = gameObject.GetComponent<Enemy>();
    bombScript.hero = Hero.instance;
    isAttacking = false;
    coolDownStart = Time.time * 1000;
    needsCoolDown = true;
  }

  public void EnableLanding() {
    canLand = true;
  }

  public void Sparkle() {
    InGame.instance.PlaySound(Sounds.bewitchSound, extra.transform.position);
    GameObject sparkle = Instantiate(Helpers.GetOrException(Objects.prefabs, "sparkle"), extra.transform.position, Quaternion.identity, extra.transform);
    sparkle.GetComponent<Animator>().Play("sparkle");
  }
}
