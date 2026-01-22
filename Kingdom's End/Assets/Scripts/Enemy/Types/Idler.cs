using System;
using UnityEngine;

public class Idler : MonoBehaviour {
  // Components
    Enemy enemy;

  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      if (!enemy.needsCoolDown) {
        enemy.AttackLogic(0, (enemy.enemyWidth / 2) + enemy.reach);
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
