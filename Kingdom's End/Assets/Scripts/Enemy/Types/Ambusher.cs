using UnityEngine;

public class Ambusher : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;
    public float jumpHeight = 8f;

  void Emerge() {
    enemy.body.velocity = new Vector2(0, jumpHeight);
  }

  void Start() {
    enemy = GetComponent<Enemy>();

    if (Helpers.IsValueInArray(Constants.lowReachingEnemies, enemy.key)) {
      jumpHeight = 4f;
    }

    Emerge();
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + (enemy.reach * (enemy.canLand ? enemy.longReach : 1.5f)));
      } else {
        enemy.CheckCoolDown();
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
