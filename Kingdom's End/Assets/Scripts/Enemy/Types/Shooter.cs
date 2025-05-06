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
    if (Hero.instance != null && Hero.instance.pauseCase == "") {
      if (!enemy.isThrowingWeapon) {
        enemy.SearchPlayer(searchCastLength);
      }
    }
  }

  void FixedUpdate() {
    enemy.CheckDeath();
  }
}
