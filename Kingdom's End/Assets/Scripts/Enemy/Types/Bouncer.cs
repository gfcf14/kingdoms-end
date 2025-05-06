using UnityEngine;

public class Bouncer : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 2f;

  void Start() {
    enemy = GetComponent<Enemy>();

    // TODO: consider changing this for low reaching enemies
    if (enemy.key == "pixie") {
      forwardCastLength = enemy.reach;
    }
  }


  void Update() {
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      // BOUNCER MOVEMENT
      if (!enemy.isAttacking) {
        enemy.Bounce();
      }

      if (!enemy.needsCoolDown) {
        // FOUND PLAYER
        if (!enemy.playerFound) {
          enemy.CheckForPlayer(forwardCastLength);
        } else {
          enemy.AttackLogic(0, (enemy.enemyWidth / 2) + (enemy.reach * 1.5f));
        }
      } else {
        enemy.CheckCoolDown();
      }
    }
  }
}
