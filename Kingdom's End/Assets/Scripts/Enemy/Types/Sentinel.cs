using UnityEngine;

public class Sentinel : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;
  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      enemy.Watch();

      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + enemy.reach);
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
