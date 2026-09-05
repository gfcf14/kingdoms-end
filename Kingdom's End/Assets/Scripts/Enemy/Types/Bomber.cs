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
    if (Hero.instance != null && Hero.instance.pauseCase == "" && enemy.currentBomb == null) {
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
