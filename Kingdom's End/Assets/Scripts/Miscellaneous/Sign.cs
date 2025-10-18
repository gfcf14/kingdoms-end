using UnityEngine;

public class Sign : MonoBehaviour {
  [SerializeField] public string id = "";
  // TODO: come up with a strategy to display based on a condition
  [SerializeField] public string condition = "";
  void Start() {
    Instantiate(Helpers.GetOrException(Objects.prefabs, $"sign-{GameData.area}"), transform.position, Quaternion.identity, transform);

    if (condition != "") {
      // TODO: write display strategy here
    }

    // Destroys placeholder once loaded
    Destroy(transform.Find("Placeholder").gameObject);
  }

  void Update() {}
}
