using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour {
  [NonSerialized] public bool isEnemyWall;
  void Start() {
    isEnemyWall = gameObject.name.Contains("EnemyWalls");
  }
  void Update() {}

  // returns which side of the hero the wall is on (1 = right, -1 = left)
  // , to push the hero away from the wall and block input toward it
  private int GetWallSide(Collider2D heroProbe) {
    float horizontalOffset = heroProbe.bounds.center.x - Hero.instance.heroCollider.bounds.center.x;
    if (Mathf.Abs(horizontalOffset) > 0.01f) {
      return horizontalOffset > 0f ? 1 : -1;
    }

    if (heroProbe.gameObject.name == "DirectionCheck-Front") {
      return Hero.instance.direction;
    }

    if (heroProbe.gameObject.name == "DirectionCheck-Back") {
      return -Hero.instance.direction;
    }

    return Hero.instance.direction;
  }

  private void FrontBump(int wallSide) {
    Debug.Log("bump from front");
    Hero.instance.FinishActionFromWallBump(wallSide);
  }

  private void OnCollisionEnter2D(Collision2D col) {
    if (!isEnemyWall) {
      if (col.collider.name == "ProximityCheck") {
        Physics2D.IgnoreCollision(col.collider, GetComponent<TilemapCollider2D>());
      }
      else {
        Debug.Log("colliding with " + col.collider.name);
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (!isEnemyWall) {
      GameObject objectColliding = col.gameObject;
      string colName = objectColliding.name;
      int wallSide = GetWallSide(col);

      if (colName == "DirectionCheck-Front" && !Hero.instance.isGrounded) { // implies a hero front collision with wall when active (jumping or falling)
        if (Hero.instance.airEdgeCheckScript.IntersectsWithWalls()) {
          if (Hero.instance.isJumping) {
            Hero.instance.airEdgeCheckScript.CheckStepOver(Hero.instance, -wallSide);
          } else {
            // TODO: verify if this blanket case (i.e. always bump when colliding with wall when not jumping) is always acceptable
            FrontBump(wallSide);
          }
        } else {
          if (Hero.instance.isJumping || Hero.instance.isFalling) {
            FrontBump(wallSide);
          }
        }
      } else if (colName == "DirectionCheck-Back" && !Hero.instance.isGrounded && Hero.instance.isHurt != 3) { // implies a hero back collision with wall when not slammed
        Debug.Log("bump from back");
        Hero.instance.Bump(bumpX: -wallSide * Hero.instance.heroWidth / 4, specificBlockDirection: wallSide > 0 ? "right" : "left");
      } else if (colName == "WeaponCollider" && Hero.instance.isDropKicking) {
        Hero.instance.FinishActionFromWallBump(wallSide);
      } else {
        Debug.Log("wall collided with " + colName);
      }
    }
  }
}
