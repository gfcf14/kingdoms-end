using UnityEngine;

public class Exploder : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 1f;

  void Start() {
    enemy = GetComponent<Enemy>();

    // TODO: consider changing this for low reaching enemies
    if (enemy.key == "pixie") {
      forwardCastLength = enemy.reach;
    }
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
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

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
