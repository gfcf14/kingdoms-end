using System;
using UnityEngine;

public class Charger : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float forwardCastLength = 10f;
    [NonSerialized] public float chargerReach;
  void Start() {
    enemy = GetComponent<Enemy>();
    chargerReach = enemy.reach * 10;
  }

  void Update() {
    if (!Hero.instance.isAutonomous) {
      if (Hero.instance != null && Hero.instance.pauseCase == "") {
        // CHARGER MOVEMENT
        if ((enemy.isWalking || enemy.isCharging) && !enemy.isAttacking) {
          enemy.DecideMovement();
          enemy.CheckEdge();
        }

        // CHARGER ATTACK
        if (!enemy.needsCoolDown) {
          if (!enemy.playerFound) {
            enemy.CheckForPlayer(forwardCastLength);
          } else {
            enemy.AttackLogic(0, (enemy.enemyWidth / 2) + enemy.reach);
          }
        } else {
          enemy.CheckCoolDown();
        }
      }
    }
  }
}
