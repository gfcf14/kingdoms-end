using UnityEngine;

public class RockExplosion : MonoBehaviour {
  [SerializeField] public string type;
  void Start() {
    GetComponent<Animator>().Play($"rock-explosion-{type}");
  }

  void Update() {}

  public void DestroyRockExplosion() {
    Destroy(gameObject);
  }
}
