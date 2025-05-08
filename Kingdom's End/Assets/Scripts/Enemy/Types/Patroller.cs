using UnityEngine;

public class Patroller : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;

  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      // PATROLLER MOVEMENT
      if (!enemy.needsCoolDown) {
        if (enemy.isWalking && !enemy.isAttacking) {
          enemy.DecideMovement();
          // There's floor forward
          enemy.CheckEdge();

          // FOUND PLAYER
          if (!enemy.playerFound) {
            enemy.CheckForPlayer(forwardCastLength);
          } else {
            enemy.AttackLogic(0, (enemy.enemyWidth / 2) + enemy.reach);
          }
          // }
        }
      } else {
        enemy.CheckCoolDown();
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
