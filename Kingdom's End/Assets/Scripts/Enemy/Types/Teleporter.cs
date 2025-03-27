using UnityEngine;

public class Teleporter : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;

  [System.NonSerialized] public float teleporterReach;
  void Start() {
    enemy = GetComponent<Enemy>();
    teleporterReach = enemy.reach * 3;
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
      // FOUND PLAYER
      if (!enemy.playerFound) {
        enemy.CheckForPlayer(forwardCastLength);
      } else {
        enemy.isTeleporting = true;
      }

      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + teleporterReach);
      } else {
        enemy.CheckCoolDown();
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
