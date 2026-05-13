using UnityEngine;

public class VariableSprite : MonoBehaviour {
  SpriteRenderer variableRenderer;
  void Start() {
    variableRenderer = GetComponent<SpriteRenderer>();
    variableRenderer.color = Helpers.GetOrException(Colors.variableColors, GameData.area);
  }

  void Update() {}
}
