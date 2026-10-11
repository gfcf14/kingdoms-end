using System;
using UnityEngine;

public class DataManager : MonoBehaviour {
  public static DataManager instance;

  [NonSerialized] public Vector2? playerPosition = null;
  [NonSerialized] public Vector2? newCameraPosition = null;

  [NonSerialized] public int? anchorIndex = -1;
  private void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }
}
