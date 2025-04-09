using UnityEngine;

public class SceneChanger : MonoBehaviour {
  [SerializeField] float offset;
  [SerializeField] string scene;
  [SerializeField] Vector2 newCameraPosition;
  void Start() {}

  void Update() {}

    public void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.CompareTag("Hero")) {
        Hero.instance.body.velocity = Vector2.zero;
        Helpers.ChangeScene(scene, new Vector2(Hero.instance.transform.position.x + offset, Hero.instance.transform.position.y), newCameraPosition);
      }
    }
}
