using UnityEngine;

public class Sign : MonoBehaviour {
  void Start() {
    Instantiate(Helpers.GetOrException(Objects.prefabs, $"sign-{GameData.area}"), transform.position, Quaternion.identity, transform);
    // Destroys placeholder once loaded
    Destroy(transform.Find("Placeholder").gameObject);
  }

  void Update() {}
}
