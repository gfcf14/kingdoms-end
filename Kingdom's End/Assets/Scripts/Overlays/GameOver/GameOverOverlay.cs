using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverOverlay : MonoBehaviour {
  [SerializeField] GameObject titleCanvas;
  void Start() {
    Destroy(Hero.instance);
    Destroy(InGame.instance);
  }
  void Update() {}

  public void TransitionToTitle() {
    // titleCanvas.transform.Find("Prompt").gameObject.SetActive(true);
    // titleCanvas.transform.Find("ButtonPanel").gameObject.SetActive(false);
    // titleCanvas.SetActive(true);
    SceneManager.LoadScene("Title");
  }
}
