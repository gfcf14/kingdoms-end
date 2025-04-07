using UnityEngine;

public class DataManager : MonoBehaviour {
  public static DataManager instance;

  public Vector2? playerPosition = null;
  public Vector2? newCameraPosition = null;
  private void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }
}
