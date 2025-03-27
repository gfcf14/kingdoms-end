using UnityEngine;

public class Bomber : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = Constants.bomberReach;

  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "" && !enemy.bombReturned) {
      if (!enemy.needsCoolDown) {
        if (!enemy.playerFound) {
          enemy.AttackLogic(0.5f, forwardCastLength);
        }
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
