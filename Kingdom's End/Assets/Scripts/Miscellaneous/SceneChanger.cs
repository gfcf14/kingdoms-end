using UnityEngine;

public class SceneChanger : MonoBehaviour {
  [SerializeField] int anchorIndex;
  [SerializeField] string scene;
  [SerializeField] Vector2 newCameraPosition;
  void Start() {}

  void Update() {}

    public void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.CompareTag("Hero")) {
        DataManager.instance.anchorIndex = anchorIndex;

        Hero.instance.body.velocity = Vector2.zero;
        Helpers.ChangeScene(scene, Vector2.zero, newCameraPosition);
      }
    }
}
