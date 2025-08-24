using UnityEngine;

public class DirectionCheck : MonoBehaviour {
  [SerializeField] public string direction = "";
  void Start() {}
  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, true);

      // TODO: prepare an animation for when this happens (i.e. player gets "slammed" against the wall after a throwback)
      if (direction == "back" && col.tag == "Wall") {
        if (Hero.instance.isHurt == 3) {
          Debug.Log("throwback!");
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, false);
    }
  }
}
