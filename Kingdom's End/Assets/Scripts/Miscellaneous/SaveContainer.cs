using UnityEngine;

public class SaveContainer : MonoBehaviour {
  [SerializeField] string type = "";
  GameObject grid;
  void Start() {
    grid = transform.Find("Grid").gameObject;
    Vector2 savePointPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);

    GameObject savePoint = Instantiate(Helpers.GetOrException(Objects.prefabs, "savepoint-" + type), savePointPosition, Quaternion.identity);
    savePoint.transform.SetParent(grid.transform);
    Object.Destroy(GetComponent<SpriteRenderer>());
  }

  void Update() {}
}
