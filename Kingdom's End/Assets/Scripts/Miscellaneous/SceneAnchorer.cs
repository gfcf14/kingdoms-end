using UnityEngine;

public class SceneAnchorer : MonoBehaviour {
  void Awake() {
    Destroy(GetComponent<SpriteRenderer>());
  }
}
