using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
  [SerializeField] string scene;
  [SerializeField] Vector2 newCameraPosition;
  void Start() {}

  void Update() {}

    public void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.CompareTag("Hero")) {
        Helpers.ChangeScene(scene, new Vector2(Hero.instance.transform.position.x + Hero.instance.direction, Hero.instance.transform.position.y), newCameraPosition);
      }
    }
}
