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
            if (col.gameObject.name.Contains("EnemyFloors")) {
              enemy.yDirection *= -1;
            } else {
              Bounds enemyBounds = GetComponent<Collider2D>().bounds;
              Vector2 enemyCenter = new Vector2(enemyBounds.center.x, enemyBounds.min.y + enemyBounds.size.y / 2);
              Vector2 otherPos = col.bounds.center;

              float verticalOffset = otherPos.y - enemyCenter.y;

              if (Mathf.Abs(verticalOffset) > Mathf.Abs(col.bounds.extents.x)) {
                  if (verticalOffset > 0) {
                    enemy.yDirection *= -1;
                  } else {
                    enemy.yDirection *= 1;
                  }
              }
            }
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
          // flying enemies who need to stay off the ground once landing after an ambush should float
          if (enemy.type == "ambusher" && enemy.isFlyingEnemy && col.gameObject.CompareTag("Floor"))
          {
            enemy.FloatEnemy();
          }

          enemy.Trigger(col);
        }
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
