using UnityEngine;

public class Arrow : MonoBehaviour {
  private SpriteRenderer objectRenderer;

  [System.NonSerialized] public Rigidbody2D body;

  [System.NonSerialized] public GameObject arrowCollider;

  Hero hero;

  [System.NonSerialized] public GameObject extraSprite;

  [System.NonSerialized] public bool isFacingLeft;
  [System.NonSerialized] public bool hasCollided = false;
  [System.NonSerialized] public bool hasFired = false;
  [System.NonSerialized] public float startX;
  [System.NonSerialized] public float startY;
  [System.NonSerialized] public float collideTime;
  [System.NonSerialized] public float maxEllapsedCollideTime = 1500f;

  [System.NonSerialized] public string type;

  [System.NonSerialized] private Vector2 initialPosition;
  [System.NonSerialized] public float speed = 50f;
  [System.NonSerialized] public float steepness = 4500f;
  [System.NonSerialized] public float hDisplacement = 1f;
  [System.NonSerialized] public float maxHeight = 5f;
  [System.NonSerialized] public int directionFactor = 0;

  private InGame inGame;

  void Start() {
    hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
    body = GetComponent<Rigidbody2D>();
    arrowCollider = transform.Find("ArrowCollider").gameObject;
    objectRenderer = GetComponent<SpriteRenderer>();
    inGame = GameObject.Find("InGame").gameObject.GetComponent<InGame>();
    extraSprite = transform.Find("Extra").gameObject;
    directionFactor = isFacingLeft ? -1 : 1;

    // TODO: consider if any other arrow types could use an extra
    if (type != "arrow-fire") {
      DestroyExtra();
    } else {
      extraSprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

      if (isFacingLeft) {
        extraSprite.transform.localScale = new Vector3(1, -1, 1);
        // TODO: try to modify the extra's position when facing left
      }
    }

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
