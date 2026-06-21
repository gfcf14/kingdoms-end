using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrounder : MonoBehaviour {
  Enemy enemy;

  void Start() {
    enemy = gameObject.transform.parent.gameObject.GetComponent<Enemy>();
  }
  void Update(){}

  public void OnCollisionEnter2D(Collision2D col) {
    bool isPlayerRelated = col.gameObject.name.Contains("Hero") || col.gameObject.name.Contains("DirectionCheck");
    bool isEnemyRelated = Helpers.IsValueInArray(Constants.enemyNonColliderNames, col.gameObject.name.Replace("(Clone)", ""));

    if (isPlayerRelated || isEnemyRelated) {
      Physics2D.IgnoreCollision(col.collider, GetComponent<BoxCollider2D>());
    } else {
      if (enemy.type == "ambusher" && enemy.canLand) {
        if (col.gameObject.CompareTag("Floor")) {
          enemy.isWatching = true;
        }
      }
    }
  }
}
