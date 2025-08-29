using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour {
  [NonSerialized] public bool isEnemyWall;
  void Start() {
    isEnemyWall = gameObject.name.Contains("EnemyWalls");
  }
  void Update() {}

  private void FrontBump() {
    Debug.Log("bump from front");
    // when bumping, finish the jump animation to ensure the player doesn't bump upward
    Hero.instance.FinishActionFromWallBump();
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

      if (colName == "DirectionCheck-Front" && !Hero.instance.isGrounded) { // implies a hero front collision with wall when active (jumping or falling)
        if (Hero.instance.airEdgeCheckScript.IntersectsWithWalls()) {
          if (Hero.instance.isJumping) {
            Hero.instance.airEdgeCheckScript.CheckStepOver(Hero.instance, Hero.instance.direction * -1);
          } else {
            // TODO: verify if this blanket case (i.e. always bump when colliding with wall when not jumping) is always acceptable
            FrontBump();
          }
        } else {
          if (Hero.instance.isJumping || Hero.instance.isFalling) {
            FrontBump();
          }
        }
      } else if (colName == "DirectionCheck-Back" && !Hero.instance.isGrounded && Hero.instance.isHurt != 3) { // implies a hero back collision with wall when not slammed
        Debug.Log("bump from back");
        Hero.instance.Bump(bumpX: (-Hero.instance.heroWidth * Hero.instance.direction) / 4, 0, specificBlockDirection: Hero.instance.isFacingLeft ? "right" : "left");
      } else if (colName == "WeaponCollider" && Hero.instance.isDropKicking) {
        Hero.instance.FinishActionFromWallBump();
      } else {
        Debug.Log("wall collided with " + colName);
      }
    }
  }
}
