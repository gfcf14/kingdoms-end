using UnityEngine;

public class ArrowCollider : MonoBehaviour {
  [System.NonSerialized] GameObject parentObject;
  [System.NonSerialized] Arrow parentArrow;

  void Start() {
    parentObject = transform.parent.gameObject;
    parentArrow = parentObject.GetComponent<Arrow>();
  }

  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    string colliderTag = col.gameObject.tag;

    if (colliderTag == "Floor" || colliderTag == "Wall") {
      if (!parentArrow.hasCollided) {
        parentArrow.hasCollided = true;
        parentArrow.collideTime = Time.time * 1000;
        parentArrow.StopAndFade();
      }
    } else if (colliderTag == "Enemy" || colliderTag == "Breakable") {
      Vector2 collisionPoint = col.ClosestPoint(transform.position);

      if (parentArrow.type != "arrow-fire") {
        GameObject pierceEffect = Instantiate(Helpers.GetOrException(Objects.prefabs, "pierce"), collisionPoint, Quaternion.identity);

        if (parentArrow.type == "arrow-poison") {
          pierceEffect.GetComponent<Pierce>().color = Helpers.GetOrException(Colors.statusColors, "poisoned");
        } else {
          pierceEffect.GetComponent<Pierce>().color = Color.white;
        }

        pierceEffect.GetComponent<Pierce>().isFacingLeft = parentArrow.isFacingLeft;
      } else {
        GameObject arrowExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), collisionPoint, Quaternion.identity);
        arrowExplosion.GetComponent<Explosion>().type = "arrow";
      }

      if (colliderTag == "Breakable") {
        parentArrow.DestroyArrow();
      }
    }
  }
}
