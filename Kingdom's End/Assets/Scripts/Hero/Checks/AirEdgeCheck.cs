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

  public void HandleFrontFloorContact(Hero hero, Collider2D frontCheck) {
    HandleFrontContact(hero, frontCheck, hero.direction, floorContact: true);
  }

  public void HandleFrontWallContact(Hero hero, Collider2D frontCheck, int wallSide) {
    HandleFrontContact(hero, frontCheck, wallSide, floorContact: false);
  }

  private void HandleFrontContact(Hero hero, Collider2D frontCheck, int wallSide, bool floorContact) {
    // Ignore contacts that are grounded, already bump-locked, or not part of an air action.
    if (hero.isGrounded ||
        hero.blockedDirection != "" ||
        (!hero.isJumping && !hero.isFalling)) {
      return;
    }

    bool movingForward = hero.horizontalInput * hero.direction > Constants.inputThreshold;
    if (!movingForward) {
      if (!floorContact) {
        BumpFromFront(hero, wallSide);
      }
      return;
    }

    // Look for both solid parts of the edge inside the front sensor.
    Collider2D[] edgeColliders = Physics2D.OverlapBoxAll(frontCheck.bounds.center, frontCheck.bounds.size, 0f);
    bool overlapsFloor = false;
    bool overlapsWall = false;

    foreach (Collider2D col in edgeColliders) {
      // Ignore triggers and the Hero's own colliders; only level geometry defines an edge.
      if (col.isTrigger || col.transform == hero.transform || col.transform.IsChildOf(hero.transform)) {
        continue;
      }

      overlapsFloor |= col.CompareTag("Floor");
      overlapsWall |= col.CompareTag("Wall");
    }

    if (!overlapsFloor || !overlapsWall) {
      // A Floor callback may arrive before its Wall callback. While rising, wait for that second contact.
      if (floorContact &&
          hero.isJumping &&
          !hero.isFalling &&
          hero.body.linearVelocity.y > Constants.yAirVelocityThreshold) {
        return;
      }

      BumpFromFront(hero, wallSide);
      return;
    }

    Vector2 rayOrigin = GetStepOverRayOrigin(hero, -wallSide);
    // If the cast starts inside either part of this edge, bump instead of stepping through it.
    Collider2D[] pointOverlaps = Physics2D.OverlapPointAll(rayOrigin);
    foreach (Collider2D pointOverlap in pointOverlaps) {
      if (pointOverlap.isTrigger) {
        continue;
      }

      // Only a Floor or Wall already identified as part of this edge can block the cast origin.
      foreach (Collider2D edgeCollider in edgeColliders) {
        if (pointOverlap == edgeCollider &&
            (edgeCollider.CompareTag("Floor") || edgeCollider.CompareTag("Wall"))) {
          BumpFromFront(hero, wallSide);
          return;
        }
      }
    }

    CheckStepOver(hero, -wallSide);
  }

  private void BumpFromFront(Hero hero, int wallSide) {
    hero.FinishActionFromWallBump(wallSide);
  }

  private void CheckStepOver(Hero hero, int awayFromWallDirection) {
    Vector2 rayOrigin = GetStepOverRayOrigin(hero, awayFromWallDirection);
    Vector2 rayDirection = Vector2.down;

    RaycastHit2D differenceCast = Physics2D.Raycast(rayOrigin, rayDirection, rayLength);
    Debug.DrawRay(rayOrigin, rayDirection.normalized * rayLength, Helpers.GetOrException(Colors.raycastColors, "jump"));
    string blockedDirection = awayFromWallDirection > 0 ? "left" : "right";

    // step-over requires a nearby floor hit; all other probe results should resolve as a bump.
    if (differenceCast.collider == null || !differenceCast.collider.CompareTag("Floor")) {
      hero.Bump(bumpX: (hero.heroWidth * awayFromWallDirection) / 4, specificBlockDirection: blockedDirection);
      return;
    }

    float yDistance = Mathf.Abs(differenceCast.point.y - rayOrigin.y);
    float stepOverHeight = colliderDimension - yDistance;

    // reject zero-height and out-of-range results so the Hero cannot settle against the wall.
    if (differenceCast.distance > 0 && stepOverHeight > 0.01f && stepOverHeight <= colliderDimension && HasClearStepOverSpace(hero, stepOverHeight)) {
      hero.StepOver(stepOverHeight);
    } else {
      hero.Bump(bumpX: (hero.heroWidth * awayFromWallDirection) / 4, specificBlockDirection: blockedDirection);
    }
  }

  private Vector2 GetStepOverRayOrigin(Hero hero, int awayFromWallDirection) {
    // The origin is at the AirEdgeCheck's top height, offset along awayFromWallDirection by 1.5 square widths minus Hero width.
    return new Vector2(
      transform.position.x + (colliderDimension * 1.5f * awayFromWallDirection) + (hero.heroWidth * awayFromWallDirection * -1),
      transform.position.y + (colliderDimension / 2)
    );
  }

  private bool HasClearStepOverSpace(Hero hero, float stepOverHeight) {
    Bounds heroBounds = hero.GetVisualBounds();
    Vector2 destinationOffset = new Vector2(hero.heroWidth * hero.direction, stepOverHeight);
    Vector2 destinationCenter = (Vector2)heroBounds.center + destinationOffset;
    Vector2 clearanceSize = new Vector2(heroBounds.size.x - 0.02f, heroBounds.size.y - 0.02f);
    Collider2D[] destinationOverlaps = Physics2D.OverlapBoxAll(destinationCenter, clearanceSize, 0f);

    // A solid Wall in the destination blocks the step; the landing Floor itself is not treated as an obstruction.
    foreach (Collider2D overlap in destinationOverlaps) {
      if (overlap.isTrigger ||
          !overlap.CompareTag("Wall") ||
          overlap.transform.IsChildOf(hero.transform)) {
        continue;
      }

      return false;
    }

    return true;
  }
}
