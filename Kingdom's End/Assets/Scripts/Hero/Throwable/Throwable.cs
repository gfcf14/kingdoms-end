using UnityEditor;
using UnityEngine;

public class Throwable : MonoBehaviour {
  private SpriteRenderer objectRenderer;
  private CapsuleCollider2D hitBounds;

  [System.NonSerialized] public GameObject extraSprite;
  [System.NonSerialized] public GameObject throwableCollider;
  [System.NonSerialized] public Sprite bounceSprite;

  [System.NonSerialized] public Rigidbody2D body;

  [System.NonSerialized] public bool isFacingLeft;
  [System.NonSerialized] public bool hasCollided = false;
  [System.NonSerialized] public bool mustBounce = false;
  [System.NonSerialized] public bool isExploding = false;

  [System.NonSerialized] public int transitionIncrement = 0;
  [System.NonSerialized] public float criticalRate = 0;

  [System.NonSerialized] public float bounceRotationMultiplier = 3;
  [System.NonSerialized] public float bounceX;
  [System.NonSerialized] public float bounceY;
  [System.NonSerialized] public float newAngle;
  [System.NonSerialized] public float collideTime;
  [System.NonSerialized] public float maxEllapsedCollideTime = 1500f;

  [System.NonSerialized] public string type;

  [System.NonSerialized] public Hero hero;

  [System.NonSerialized] private Vector2 initialPosition;
  [System.NonSerialized] public float speed = 0f;
  [System.NonSerialized] public float steepness = 0f;
  [System.NonSerialized] public float hDisplacement = 0f;
  [System.NonSerialized] public float maxHeight = 0f;
  [System.NonSerialized] public int directionFactor = 0;
  [System.NonSerialized] public float rotationSpeed = 360f;
  [System.NonSerialized] public int rotationFactor = 1; // dictates how fast throwable rotation would go
  [System.NonSerialized] public bool freezeRotation = false;

  void Start() {
    hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
    objectRenderer = GetComponent<SpriteRenderer>();
    throwableCollider = transform.Find("ThrowableCollider").gameObject;

    body = GetComponent<Rigidbody2D>();

    if (Helpers.IsValueInArray(Constants.nonSymmetricalThrowables, type) && isFacingLeft) {
      transform.localScale = new Vector3(1, -1, 1);
    }

    // TODO: remove extra component once bomb sprites are modified to include spark in them
      extraSprite = transform.Find("Extra").gameObject;
      DestroyExtra();

    initialPosition = body.position;
    body.gravityScale = 0;

    directionFactor = isFacingLeft ? -1 : 1;

    objectRenderer.sprite = Helpers.GetOrException(Sprites.throwableSprites, type);


    // if the instantiated throwable must bounce, use different values than those for a regular throw
    ThrowableSpecs throwableSpecs = mustBounce ? Constants.bounceSpecs : Helpers.GetOrException(Objects.throwableSpecs, type);

    // edit throwable specs for the collider based on type
      throwableCollider.transform.rotation = Quaternion.Euler(throwableSpecs.initialRotationValues.x, throwableSpecs.initialRotationValues.y, 0);

      CapsuleCollider2D throwableCapsuleCollider = throwableCollider.GetComponent<CapsuleCollider2D>();

      throwableCapsuleCollider.offset = mustBounce ? Vector2.zero : new Vector2(throwableSpecs.colliderOffset.x, throwableSpecs.colliderOffset.y);
      throwableCapsuleCollider.size = mustBounce ? Vector2.zero : new Vector2(throwableSpecs.colliderSize.x, throwableSpecs.colliderSize.y);

    // edit throwable specs for parabola function
      hDisplacement = throwableSpecs.hDisplacement;
      maxHeight = throwableSpecs.maxHeight;
      speed = throwableSpecs.speed;
      steepness = throwableSpecs.steepness;

    // edit throwable specs for rotation
      rotationFactor = throwableSpecs.rotationFactor ?? 1;
      freezeRotation = throwableSpecs.freezeRotation;
  }

  public void ParabolaMovement() {
    Vector2 currentPosition = body.position;

    // Horizontal velocity
    float horizontalSpeed = speed * directionFactor;

    // Calculate the time based on horizontal distance traveled
    float time = (currentPosition.x - initialPosition.x) * directionFactor;

    // Move the throwable following the parabola function f(x) = -(1 / steepness) * (x - hDisplacement)^2 + maxHeight
    // for which we need the derivative: f'(x) = -2 * steepness * (x - 20)
    // This gives us the slope of the parabola at the current point
    float parabolaSlope = (-2 * directionFactor) * (1 / steepness) * (time - hDisplacement);

    // Vertical velocity: derive it based on the parabola's slope
    float verticalSpeed = horizontalSpeed * parabolaSlope;

    // Set the Rigidbody velocity
    body.velocity = new Vector2(horizontalSpeed, verticalSpeed);

    // Rotates the throwable to conform to parabola
    if (!freezeRotation) {
      if (mustBounce || Helpers.IsValueInArray(Constants.rotatingThrowables, type)) {
        transform.Rotate(0f, 0f, rotationFactor * rotationSpeed * Time.deltaTime * -directionFactor);
      } else {
        // gets an angle for the throwable (in radians first then converted to degrees) based on the speed
        float angle = Mathf.Atan2(verticalSpeed, horizontalSpeed) * Mathf.Rad2Deg;

        // TODO: ensure to add some form of rotation speed for each rotating throwable
        // Apply the rotation to the transform
        transform.rotation = Quaternion.Euler(0, 0, angle);
      }
    }

    // DEBUG for VELOCITY: draws the speeds used by the player to attempt to understand the direction taken on movement
      // x velocity
      Debug.DrawRay(currentPosition, new Vector2(horizontalSpeed, 0), Helpers.GetOrException(Colors.raycastColors, "vx"));

      // y velocity
      Debug.DrawRay(currentPosition, new Vector2(0, verticalSpeed), Helpers.GetOrException(Colors.raycastColors, "vy"));

      // overall speed direction
      Debug.DrawRay(transform.position, body.velocity, Helpers.GetOrException(Colors.raycastColors, "vxy"));
  }

  void Update() {
    // TODO: This code pauses the game as soon as the throwable renderer has a sprite;
    //       useful for when the start position of the throwable is tested.
    //       remove this once the game is complete
    // if (objectRenderer.sprite != null) {
    //   EditorApplication.isPaused = true;
    // }

    // TODO: consider updating this to keep object "alive" for some time/length after off camera
    // if (!objectRenderer.isVisible) {
    //   DestroyThrowable();
    // }

    if (!hasCollided) {
      ParabolaMovement();
    } else {
      if (mustBounce) {
        ParabolaMovement();
      }

      float ellapsedCollideTime = (Time.time * 1000) - collideTime;

      if (ellapsedCollideTime < maxEllapsedCollideTime) {
        if (Helpers.IsNonBouncingThrowable(type)) {
          objectRenderer.color = new Color(1, 1, 1, 1 - (ellapsedCollideTime / maxEllapsedCollideTime));
        }
      } else {
        if (Helpers.IsNonBouncingThrowable(type)) {
          DestroyThrowable();
        }
      }
    }
  }

  public void DestroyExtra() {
    if (extraSprite != null) {
      Destroy(extraSprite);
    }
  }

  public void DestroyThrowable() {
    Destroy(gameObject);
  }

  public void RemovePhysics() {
    Destroy(GetComponent<Rigidbody2D>());
    Destroy(throwableCollider.GetComponent<CapsuleCollider2D>());
  }

  public void StopAndFade() {
    body.velocity = Vector2.zero;
    RemovePhysics();
  }

  public void Explode() {
    // TODO: remove usage of animation for explosions once explosion with particle effects are implemented
    Animator anim = gameObject.AddComponent<Animator>();
    anim.runtimeAnimatorController = Helpers.GetOrException(Objects.animationControllers, "throwable");
    anim.Play("exploding");
    isExploding = true;

    // TODO: consider a better way to play a sound other than using the in game helper
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.explosionSounds, "basic"), transform.position);
  }

  public void SetBounce(Transform t, Vector3 collisionPoint) {
    // TODO: this simple fix instantiates a new throwable that is meant to bounce and fade out, while the one that collided is
    // destroyed. I.e. one collision = two objects. While both eventually get destroyed, consider if it's worth changing the
    // function values of the originally colliding throwable instead of creating a new one
    GameObject throwableWeapon = Instantiate(Helpers.GetOrException(Objects.prefabs, "throwable"), collisionPoint, Quaternion.identity);
    Throwable throwableInstance = throwableWeapon.GetComponent<Throwable>();

    throwableInstance.collideTime = Time.time * 1000;
    throwableInstance.hasCollided = true;
    throwableInstance.isFacingLeft = !isFacingLeft;
    throwableInstance.mustBounce = true;
    throwableInstance.type = type;

    DestroyThrowable();
  }
}
