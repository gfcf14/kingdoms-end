using UnityEngine;

public class DirectionCheck : MonoBehaviour {
  [SerializeField] public string direction = "";
  void Start() {}
  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, true);
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, false);
    }
  }
}
