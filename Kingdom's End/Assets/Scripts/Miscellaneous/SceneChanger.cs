using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
  [SerializeField] string scene;
  [SerializeField] Vector2 newCameraPosition;
  void Start() {}

  void Update() {}

    public void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.CompareTag("Hero")) {
        Hero heroInstance = GameObject.Find("Hero").GetComponent<Hero>();

        DataManager.instance.playerPosition = new Vector2(heroInstance.transform.position.x + heroInstance.direction, heroInstance.transform.position.y);
        DataManager.instance.newCameraPosition = newCameraPosition;
        DataManager.instance.playerFalling = heroInstance.isJumping || heroInstance.isFalling;

        SceneManager.LoadScene(scene);
      }
    }
}
