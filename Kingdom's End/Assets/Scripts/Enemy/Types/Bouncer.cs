using UnityEngine;

public class Bouncer : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;

  void Start() {
    enemy = GetComponent<Enemy>();
  }


  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
      // BOUNCER MOVEMENT
      if (!enemy.isAttacking) {
        enemy.Bounce();
      }

      if (!enemy.needsCoolDown) {
        // FOUND PLAYER
        if (!enemy.playerFound) {
          enemy.CheckForPlayer(forwardCastLength);
        } else {
          enemy.AttackLogic(0, (enemy.enemyWidth / 2) + (enemy.reach * 1.5f));
        }
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
