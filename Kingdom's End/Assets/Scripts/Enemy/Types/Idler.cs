using System;
using UnityEngine;

public class Idler : MonoBehaviour {
  // Components
    Enemy enemy;
    [NonSerialized] public float idleReach;

  void Start() {
    enemy = GetComponent<Enemy>();
    idleReach = Helpers.IsValueInArray(Constants.lowReachingEnemies, enemy.key) ? enemy.reach * 3 : enemy.reach;
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + idleReach);
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
