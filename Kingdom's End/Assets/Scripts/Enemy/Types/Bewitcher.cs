using UnityEngine;

public class Bewitcher : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 0.5f;

  void Start() {
    enemy = GetComponent<Enemy>();

    if (Helpers.IsValueInArray(Constants.shortCastEnemies, enemy.key)) {
      forwardCastLength = enemy.reach * 1.25f;
    }
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
      if (!enemy.needsCoolDown) {
        // BEWITCHER MOVEMENT
        if (enemy.isWalking && !enemy.isAttacking) {
          enemy.DecideMovement();
          enemy.CheckEdge();
        }

        // BEWITCHER PLAYER CHECK
        if (!enemy.playerFound) {
          enemy.CheckForPlayer(forwardCastLength);
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
