using UnityEngine;

public class Idler : MonoBehaviour {
  // Components
    Enemy enemy;
    [System.NonSerialized] public float idleReach;

  void Start() {
    enemy = GetComponent<Enemy>();
    // TODO: consider changing this for low reaching enemies
    idleReach = enemy.key != "pixie" ? enemy.reach * 3 : enemy.reach;
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

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
