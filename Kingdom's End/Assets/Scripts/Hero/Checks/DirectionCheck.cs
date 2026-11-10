using UnityEngine;

public class DirectionCheck : MonoBehaviour {
  [SerializeField] public string direction = "";
  void Start() {}
  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    if (direction == "front" && col.CompareTag("Incline")) {
      Hero.instance.EnterIncline(col);
    }

    // Floor edges can be detected before their paired Wall collider.
    if (direction == "front" &&
        col.CompareTag("Floor") &&
        !col.isTrigger &&
        !Hero.instance.isGrounded &&
        (Hero.instance.isJumping || Hero.instance.isFalling) &&
        Hero.instance.blockedDirection == "" &&
        Hero.instance.horizontalInput * Hero.instance.direction > Constants.inputThreshold) {
      if (Hero.instance.airEdgeCheckScript.CanAttemptStepOver(Hero.instance)) {
        Hero.instance.airEdgeCheckScript.CheckStepOver(Hero.instance, -Hero.instance.direction);
      } else {
        Hero.instance.FinishActionFromWallBump(Hero.instance.direction);
      }
    }

    // wall contact at the feet is not ground support; floor contact still is.
    bool isLandingObject = Helpers.IsValueInArray(Constants.landingObjects, col.tag);
    if ((isLandingObject || col.CompareTag("Wall")) && !(direction == "bottom" && col.CompareTag("Wall"))) {
      if (direction == "bottom") {
        Hero.instance.SetGroundContact(col, true);
      } else if (direction == "top") {
        Hero.instance.SetCeilingContact(col, true);
      } else {
        Hero.instance.SetCollisionDirection(direction, true);
        if (direction == "front" || direction == "back") {
          Hero.instance.SetHorizontalContact(col, direction, true);
        }
      }

      if (direction == "back" && col.tag == "Wall") { // starts the slam sequence
        if (Hero.instance.isHurt == 3) {
          Hero.instance.body.linearVelocity = Vector2.zero;
          Hero.instance.isSlammed = true;
          Hero.instance.ModifyPosition(new Vector2(transform.position.x + ((Hero.instance.heroWidth * Hero.instance.direction) / 3), transform.position.y));
        }
      } else if (direction == "bottom" && col.tag == "Floor") {
        if (Hero.instance.isFallingSlammed == true) { // transitions slam sequence from falling to recover
          Hero.instance.isFallingSlammed = false;
        } else if (Hero.instance.currentHP == 0) { // starts the dead throwback animation
          Hero.instance.ClearInvulnerability();
          Hero.instance.isDead = 2;
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (direction == "front" && col.CompareTag("Incline")) {
      Hero.instance.ExitIncline(col);
    }

    bool isLandingObject = Helpers.IsValueInArray(Constants.landingObjects, col.tag);
    if ((isLandingObject || col.CompareTag("Wall")) && !(direction == "bottom" && col.CompareTag("Wall"))) {
      if (direction == "bottom") {
        Hero.instance.SetGroundContact(col, false);
      } else if (direction == "top") {
        Hero.instance.SetCeilingContact(col, false);
      } else {
        Hero.instance.SetCollisionDirection(direction, false);
        if (direction == "front" || direction == "back") {
          Hero.instance.SetHorizontalContact(col, direction, false);
        }
      }
    }
  }
}
