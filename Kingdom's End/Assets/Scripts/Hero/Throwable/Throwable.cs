using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Throwable : MonoBehaviour {
  private SpriteRenderer objectRenderer;
  private CapsuleCollider2D hitBounds;

  [NonSerialized] public GameObject extraSprite;
  [NonSerialized] public GameObject throwableCollider;
  [NonSerialized] public Sprite bounceSprite;

  [NonSerialized] public Rigidbody2D body;

  [NonSerialized] public bool isFacingLeft;
  [NonSerialized] public bool hasCollided = false;
  [NonSerialized] public bool mustBounce = false;
  [NonSerialized] public bool isExploding = false;

  [NonSerialized] public int transitionIncrement = 0;
  [NonSerialized] public float criticalRate = 0;

  [NonSerialized] public float bounceRotationMultiplier = 3;
  [NonSerialized] public float bounceX;
  [NonSerialized] public float bounceY;
  [NonSerialized] public float newAngle;
  [NonSerialized] public float collideTime;
  [NonSerialized] public float maxEllapsedCollideTime = 1500f;

  [NonSerialized] public string type;

  [NonSerialized] private Vector2 initialPosition;
  [NonSerialized] public float speed = 0f;
  [NonSerialized] public float steepness = 0f;
  [NonSerialized] public float hDisplacement = 0f;
  [NonSerialized] public float maxHeight = 0f;
  [NonSerialized] public int directionFactor = 0;
  [NonSerialized] public float rotationSpeed = 360f;
  [NonSerialized] public int rotationFactor = 1; // dictates how fast throwable rotation would go
  [NonSerialized] public bool freezeRotation = false;

  void Start() {
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

    objectRenderer.sprite = Helpers.IsValueInArray(Constants.enemyProjectiles, type) ? Helpers.GetOrException(Sprites.projectileSprites, type) : Helpers.GetOrException(Sprites.throwableSprites, type);


    // if the instantiated throwable must bounce, use different values than those for a regular throw
    Dictionary<string, ThrowableSpecs> throwableAndProjectileSpecs = Objects.throwableSpecs.Concat(Objects.projectileSpecs).ToDictionary(x => x.Key, x => x.Value);
    ThrowableSpecs throwableSpecs = mustBounce ? Constants.bounceSpecs : Helpers.GetOrException(throwableAndProjectileSpecs, type);

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
    body.linearVelocity = new Vector2(horizontalSpeed, verticalSpeed);

    // Rotates the throwable to conform to parabola
    if (!freezeRotation) {
      if (mustBounce || Helpers.IsValueInArray(Constants.rotatingThrowables.Concat(Constants.rotatingProjectiles).ToArray(), type)) {
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
      Debug.DrawRay(transform.position, body.linearVelocity, Helpers.GetOrException(Colors.raycastColors, "vxy"));
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

      if (!Helpers.IsValueInArray(Constants.explodingThrowables, type)) {
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
    body.linearVelocity = Vector2.zero;
    RemovePhysics();
  }

  public void Explode() {
    isExploding = true;
    GameObject bomb = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), transform.position, Quaternion.identity);
    Explosion bombExplosion = bomb.GetComponent<Explosion>();
    bombExplosion.type = "bomb";
    bombExplosion.damage = (int) Helpers.GetOrException(Objects.regularItems, "bomb").effects.atk;
    DestroyThrowable();
  }

  public void SetBounce(Transform t, Vector3 collisionPoint) {
    if (!Helpers.IsValueInArray(Constants.explodingThrowables, type)) {
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
    } else {
      body.linearVelocity = Vector2.zero;
      Destroy(objectRenderer);

      GameObject projectileExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), transform.position, Quaternion.identity, transform);
      projectileExplosion.GetComponent<Explosion>().type = "projectile";
    }
  }
}
