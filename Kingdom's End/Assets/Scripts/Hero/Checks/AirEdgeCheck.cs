using UnityEngine;

public class AirEdgeCheck : MonoBehaviour {
  BoxCollider2D airEdgeCheckCollider;
  // using a single dimension since the collider is a square
  float colliderDimension = 0;
  float rayLength = 1;
  void Start() {
    airEdgeCheckCollider = GetComponent<BoxCollider2D>();
    colliderDimension = airEdgeCheckCollider.size.x;
    rayLength = colliderDimension - 0.01f;
  }
  void Update() {}

  public bool IntersectsWithWalls() {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(airEdgeCheckCollider.bounds.center, airEdgeCheckCollider.bounds.size, 0f);

    foreach (Collider2D col in colliders) {
      if (col.CompareTag("Wall")) {
        return true;
      }
    }

    return false;
  }

  public void CheckStepOver(Hero hero, int direction) {
    Vector2 rayOrigin = new Vector2(transform.position.x + (colliderDimension * 1.5f * direction) + (hero.heroWidth * direction * -1), transform.position.y + (colliderDimension / 2));
    Vector2 rayDirection = Vector2.down;

    RaycastHit2D differenceCast = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
    Debug.DrawRay(rayOrigin, rayDirection.normalized * rayLength, Helpers.GetOrException(Colors.raycastColors, "jump"));

    // step-over requires a nearby floor hit; all other probe results should resolve as a bump.
    if (differenceCast.collider == null || !differenceCast.collider.CompareTag("Floor")) {
      hero.Bump(bumpX: (hero.heroWidth * direction) / 4);
      return;
    }

    float yDistance = Mathf.Abs(differenceCast.point.y - rayOrigin.y);
    float stepOverHeight = colliderDimension - yDistance;

    // reject zero-height and out-of-range results so the Hero cannot settle against the wall.
    if (differenceCast.distance > 0 && stepOverHeight > 0.01f && stepOverHeight <= colliderDimension) {
      hero.StepOver(stepOverHeight);
    } else {
      hero.Bump(bumpX: (hero.heroWidth * direction) / 4);
    }
  }
}
