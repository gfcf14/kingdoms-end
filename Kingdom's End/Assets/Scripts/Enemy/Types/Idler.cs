using UnityEngine;

public class Idler : MonoBehaviour {
  // Components
    Enemy enemy;
    [System.NonSerialized] public float idleReach;

  void Start() {
    enemy = GetComponent<Enemy>();
    idleReach = enemy.reach * 3;
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + idleReach);
      } else {
        enemy.CheckCoolDown();
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
