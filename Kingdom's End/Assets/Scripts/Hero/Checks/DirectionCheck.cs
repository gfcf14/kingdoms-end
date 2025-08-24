using UnityEngine;

public class DirectionCheck : MonoBehaviour {
  [SerializeField] public string direction = "";
  void Start() {}
  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, true);

      if (direction == "back" && col.tag == "Wall") { // starts the slam sequence
        if (Hero.instance.isHurt == 3) {
          Hero.instance.body.velocity = Vector2.zero;
          Hero.instance.isSlammed = true;
          Hero.instance.ModifyPosition(new Vector2(transform.position.x + ((Hero.instance.heroWidth * Hero.instance.direction) / 3), transform.position.y));
        }
      } else if (direction == "bottom" && col.tag == "Floor" && Hero.instance.isFallingSlammed == true) { // transitions slam sequence from falling to recover
        Hero.instance.isFallingSlammed = false;
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.tag == "Floor" || col.tag == "Wall") {
      Hero.instance.SetCollisionDirection(direction, false);
    }
  }
}
