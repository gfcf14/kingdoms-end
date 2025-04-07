using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutOverlay : MonoBehaviour {
  void Start() {}
  void Update() {}

  // uses the main overlay to cover the game on game over
  public void Cover() {
    InGame.instance.Cover();
  }

  void DestroyGameSingletons() {
    Destroy(InGame.instance.gameObject);
    Destroy(Hero.instance.gameObject);
  }

  void ClearDataManager() {
    DataManager.instance.newCameraPosition = null;
    DataManager.instance.playerPosition = null;
  }

  public void GameOver() {
    DestroyGameSingletons();
    ClearDataManager();

    SceneManager.LoadScene("GameOver");
    Time.timeScale = 1; // starts time again so game can keep playing if player starts over
  }
}
