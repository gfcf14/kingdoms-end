using UnityEngine;

public class Shooter : MonoBehaviour {
  // Components
    Enemy enemy;

  // Raycast properties
    public float searchCastLength = 20f;

  void Start() {
    enemy = GetComponent<Enemy>();
  }

  void Update() {
    if (enemy.hero != null && enemy.hero.pauseCase == "") {
      if (!enemy.isThrowingWeapon) {
        enemy.SearchPlayer(searchCastLength);
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
