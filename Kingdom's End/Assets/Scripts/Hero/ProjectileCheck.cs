using UnityEngine;

public class ProjectileCheck : MonoBehaviour {
  public Vector2 boxSize = new (5f, 3.5f);
  void Start() {}
  void Update() {}

  public bool ProjectileNearby() {
    Collider2D[] projectileCheckColliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);

    foreach (Collider2D collider in projectileCheckColliders) {
      bool isBomb = collider.gameObject.name.Contains("EnemyBomb");
      bool isProjectile = collider.gameObject.name.Contains("Projectile");

      if (isBomb || isProjectile) return true;
    }

    return false;
  }

  void OnDrawGizmos() {
    Gizmos.color = Helpers.GetOrException(Colors.raycastColors, "search");
    Gizmos.DrawWireCube(transform.position, boxSize);
  }
}
