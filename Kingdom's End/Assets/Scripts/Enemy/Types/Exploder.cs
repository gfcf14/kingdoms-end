using UnityEngine;

public class Exploder : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 1f;

  void Start() {
    enemy = GetComponent<Enemy>();

    if (Helpers.IsValueInArray(Constants.lowReachingEnemies, enemy.key)) {
      forwardCastLength = enemy.reach;
    }
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      // EXPLODER MOVEMENT
      if (enemy.isWalking && !enemy.isAttacking && !enemy.isExploding) {
        enemy.DecideMovement();
        enemy.CheckEdge();
      }

      // EXPLODER EXPLOSION CHECK
      if (!enemy.playerFound) {
        enemy.CheckForPlayer(forwardCastLength);
      }
    }
  }
}
