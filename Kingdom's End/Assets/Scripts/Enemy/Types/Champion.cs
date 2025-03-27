using UnityEngine;

public class Champion : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 5f;
    public float playerCastLength = 0.1f;

  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (!enemy.hero.isAutonomous) {
      if (enemy.hero != null && enemy.hero.pauseCase == "") {
        // CHAMPION MOVEMENT
        if (!enemy.needsCoolDown) {
          if (enemy.isWalking && !enemy.isAttacking) {
            enemy.DecideMovement();
            enemy.CheckEdge();

            // FOUND PLAYER
            if (!enemy.playerFound) {
              enemy.CheckForPlayer(forwardCastLength);
            } else {
              enemy.AttackLogic((Helpers.GetOrException(Objects.enemyDimensions, enemy.key).x * enemy.reach) * enemy.direction * 2, playerCastLength);
            }
          }
        } else {
          enemy.CheckCoolDown();
        }
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
