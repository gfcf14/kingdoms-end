using UnityEngine;

public class EnemyCollider : MonoBehaviour {
  Enemy enemy;

  void Start() {
    enemy = gameObject.transform.parent.gameObject.GetComponent<Enemy>();
  }

  void Update() {}

  private void OnCollisionEnter2D(Collision2D col) {
    if (!enemy.WillDie()) {
      if (col.gameObject.name == "EnemyCollider") {
        Physics2D.IgnoreCollision(col.gameObject.GetComponent<CapsuleCollider2D>(), GetComponent<CapsuleCollider2D>());
      // TODO: ensure the type check is better
      } else if (/*Helpers.IsValueInArray(Constants.flyingEnemyTypes, enemy.type) &&*/ col.gameObject.CompareTag("Hero") && (!enemy.isAttacking || !col.gameObject.GetComponent<Hero>().isDropKicking)) {
        Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CapsuleCollider2D>());
      } else {
        enemy.Collision(col);
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (!enemy.WillDie()) {
      if (col.gameObject.tag == "Weapon" && col.gameObject.name != "ThrowableCollider" && col.gameObject.name != "ArrowCollider") {
        if (!col.gameObject.GetComponent<Weapon>().triggeredObjects.Contains(gameObject)) {
          enemy.Trigger(col);
          col.gameObject.GetComponent<Weapon>().triggeredObjects.Add(gameObject);
        }
      } else {
        if (enemy.type == "bouncer" && (col.gameObject.CompareTag("Floor") || col.gameObject.CompareTag("Wall"))) {
          if (col.gameObject.CompareTag("Floor")) { // when hitting the floor/ceiling, bounce by flipping the y direction
            enemy.yDirection *= -1;
          } else if (col.gameObject.CompareTag("Wall")) { // when hitting walls, bounce by turning around (which switches scale and isFacingLeft, which switches direction)
            enemy.TurnAround();
          }
        } else if (col.gameObject.name.Contains("EnemyBomb")) {
          EnemyBomb collidedBomb = col.gameObject.GetComponent<EnemyBomb>();

          if (collidedBomb.wasKickedBack) {
            enemy.isDead = true;
          }
        } else if (enemy.type == "ambusher" && !enemy.isAttacking) {
          if (col.gameObject.CompareTag("Floor")) {
              if (enemy.canLand && !enemy.isWatching) {
              enemy.isWatching = true;
              enemy.body.velocity = Vector2.zero;
              enemy.transform.position = col.ClosestPoint(transform.position);
            }
          } else if (col.gameObject.CompareTag("Hero")) { // if meeting the player in air, enemy should be able to land
            enemy.canLand = true;
          }
        } else {
          enemy.Trigger(col);
        }
      }
    } else {
      if ((col.tag == "Floor" || col.tag == "Wall") && enemy.diesFlying) {
        // if already dying, destroy on ground (wall) contact to avoid droppable spawn past wall (when enemy dies flying)
        enemy.anim.speed = 0;
        // move enemy a bit to the opposite direction to ensure droppable won't stick to the wall
        enemy.transform.position = new Vector2(enemy.transform.position.x + ((enemy.enemyWidth / 8) * enemy.direction), enemy.transform.position.y);
        enemy.Destroy();
      }
    }
  }

  private void OnTriggerStay2D(Collider2D col) {
    if (enemy.type == "bouncer") {
      if (col.CompareTag("Room")) {
        Bounds roomBounds = col.bounds;
        Vector2 enemyPos = transform.position;

        float halfWidth = GetComponent<Collider2D>().bounds.extents.x;
        float halfHeight = GetComponent<Collider2D>().bounds.extents.y;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>(); // Get Rigidbody2D to check velocity

        // Check if touching the top or bottom and moving in that direction
        if ((enemyPos.y + halfHeight >= roomBounds.max.y && rb.velocity.y > 0) || (enemyPos.y - halfHeight <= roomBounds.min.y && rb.velocity.y < 0)) {
          enemy.yDirection *= -1; // Flip Y direction only if moving out
        }
        // Check if touching the left or right and moving in that direction
        else if ((enemyPos.x - halfWidth <= roomBounds.min.x && rb.velocity.x < 0) || (enemyPos.x + halfWidth >= roomBounds.max.x && rb.velocity.x > 0)) {
          enemy.TurnAround(); // Flip X direction only if moving out
        }
      }
    }
  }
}
