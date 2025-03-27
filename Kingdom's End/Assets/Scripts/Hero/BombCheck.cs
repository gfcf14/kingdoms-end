using UnityEngine;

public class BombCheck : MonoBehaviour {
  void Start() {}
  void Update() {}

  public bool BombNearby() {
    Collider2D[] bombCheckColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(2.5f, 3.5f), 0f);

    foreach (Collider2D collider in bombCheckColliders) {
      if (collider.gameObject.name.Contains("EnemyBomb")) {
        return true;
      }
    }

    return false;
  }
}
