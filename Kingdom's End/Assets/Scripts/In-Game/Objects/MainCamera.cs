using UnityEngine;

public class MainCamera : MonoBehaviour {
  void Awake() {
    Vector2 cameraPosition = new Vector2(GameData.mainCameraX, GameData.mainCameraY);

    if (DataManager.instance.newCameraPosition.HasValue) {
      cameraPosition = DataManager.instance.newCameraPosition.Value;
    }
    Camera.main.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -10);
  }

  void Update() {}
}
