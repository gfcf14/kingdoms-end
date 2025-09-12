using System;
using UnityEngine;

public class Arrow : MonoBehaviour {
  private SpriteRenderer objectRenderer;

  [NonSerialized] public Rigidbody2D body;

  [NonSerialized] public GameObject arrowCollider;

  [NonSerialized] public GameObject extraSprite;

  [NonSerialized] public bool isFacingLeft;
  [NonSerialized] public bool hasCollided = false;
  [NonSerialized] public bool hasFired = false;
  [NonSerialized] public float startX;
  [NonSerialized] public float startY;
  [NonSerialized] public float collideTime;
  [NonSerialized] public float maxEllapsedCollideTime = 1500f;

  [NonSerialized] public string type;

  [NonSerialized] private Vector2 initialPosition;
  [NonSerialized] public float speed = 50f;
  [NonSerialized] public float steepness = 4500f;
  [NonSerialized] public float hDisplacement = 1f;
  [NonSerialized] public float maxHeight = 5f;
  [NonSerialized] public int directionFactor = 0;

  void Start() {
    body = GetComponent<Rigidbody2D>();
    arrowCollider = transform.Find("ArrowCollider").gameObject;
    objectRenderer = GetComponent<SpriteRenderer>();
    extraSprite = transform.Find("Extra").gameObject;
    directionFactor = isFacingLeft ? -1 : 1;
    extraSprite.transform.localScale = new Vector3(1, directionFactor, 1);
    extraSprite.transform.localPosition = new Vector2(isFacingLeft ? 0.35f: 0.4f, 0.05f * directionFactor);

    objectRenderer.sprite = Helpers.GetOrException(Sprites.arrows, type);
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
    // gets an angle for the throwable (in radians first then converted to degrees) based on the speed
    float angle = Mathf.Atan2(verticalSpeed, horizontalSpeed) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);

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
      float ellapsedCollideTime = (Time.time * 1000) - collideTime;

      if (ellapsedCollideTime < maxEllapsedCollideTime) {
         objectRenderer.color = new Color(1, 1, 1, 1 - (ellapsedCollideTime / maxEllapsedCollideTime));
      } else {
        DestroyArrow();
      }
    }
  }

  public void RemovePhysics() {
    Destroy(GetComponent<Rigidbody2D>());
    Destroy(arrowCollider.GetComponent<CapsuleCollider2D>());
  }

  public void StopAndFade() {
    RemovePhysics();
  }

  void DestroyExtra() {
    if (extraSprite != null) {
      Destroy(extraSprite);
    }
  }

  public void DestroyArrow() {
    Destroy(gameObject);
  }
}
