using UnityEngine;

public class TeleportContainer : MonoBehaviour {
  [SerializeField] string type = "";
  GameObject grid;
  void Start() {
    grid = transform.Find("Grid").gameObject;
    Vector2 teleportPointPosition = new Vector2(transform.position.x, transform.position.y);

    GameObject teleportPoint = Instantiate(Helpers.GetOrException(Objects.prefabs, "teleportpoint-" + type), teleportPointPosition, Quaternion.identity);
    teleportPoint.transform.SetParent(grid.transform);
    Object.Destroy(GetComponent<SpriteRenderer>());
  }

  void Update() {}
}