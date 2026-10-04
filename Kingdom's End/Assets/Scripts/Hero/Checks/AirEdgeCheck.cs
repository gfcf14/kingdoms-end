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

  public void CheckStepOver(Hero hero, int awayFromWallDirection) {
    Vector2 rayOrigin = new Vector2(transform.position.x + (colliderDimension * 1.5f * awayFromWallDirection) + (hero.heroWidth * awayFromWallDirection * -1), transform.position.y + (colliderDimension / 2));
    Vector2 rayDirection = Vector2.down;

    RaycastHit2D differenceCast = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
    Debug.DrawRay(rayOrigin, rayDirection.normalized * rayLength, Helpers.GetOrException(Colors.raycastColors, "jump"));
    string blockedDirection = awayFromWallDirection > 0 ? "left" : "right";

    // step-over requires a nearby floor hit; all other probe results should resolve as a bump.
    if (differenceCast.collider == null || !differenceCast.collider.CompareTag("Floor") || differenceCast.normal.y < 0.5f) {
      hero.Bump(bumpX: (hero.heroWidth * awayFromWallDirection) / 4, specificBlockDirection: blockedDirection);
      return;
    }

    float yDistance = Mathf.Abs(differenceCast.point.y - rayOrigin.y);
    float stepOverHeight = colliderDimension - yDistance;

    // reject zero-height and out-of-range results so the Hero cannot settle against the wall.
    if (differenceCast.distance > 0 && stepOverHeight > 0.01f && stepOverHeight <= colliderDimension && HasClearStepOverSpace(hero, differenceCast.collider, stepOverHeight)) {
      hero.StepOver(stepOverHeight);
    } else {
      hero.Bump(bumpX: (hero.heroWidth * awayFromWallDirection) / 4, specificBlockDirection: blockedDirection);
    }
  }

  private bool HasClearStepOverSpace(Hero hero, Collider2D landingFloor, float stepOverHeight) {
    Bounds heroBounds = hero.heroCollider.bounds;
    Vector2 destinationOffset = new Vector2(hero.heroWidth * hero.direction, stepOverHeight);
    Vector2 destinationCenter = (Vector2)heroBounds.center + destinationOffset;
    Vector2 clearanceSize = new Vector2(heroBounds.size.x - 0.02f, heroBounds.size.y - 0.02f);
    Collider2D[] destinationOverlaps = Physics2D.OverlapBoxAll(destinationCenter, clearanceSize, 0f);

    foreach (Collider2D overlap in destinationOverlaps) {
      if (overlap.isTrigger || overlap == landingFloor || overlap == hero.heroCollider || overlap.transform.IsChildOf(hero.transform)) {
        continue;
      }

      return false;
    }

    return true;
  }
}
