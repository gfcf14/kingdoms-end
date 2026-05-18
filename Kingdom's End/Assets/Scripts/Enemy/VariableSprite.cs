using System;
using UnityEngine;

public class VariableSprite : MonoBehaviour {
  [NonSerialized] SpriteRenderer variableRenderer;
  [NonSerialized] private SimpleFlash variableFlash;
  Color variableColor;
  void Start() {
    variableRenderer = GetComponent<SpriteRenderer>();
    variableFlash = GetComponent<SimpleFlash>();

    variableColor = Helpers.GetOrException(Colors.variableColors, GameData.area);
    variableRenderer.color = variableColor;    
    variableFlash.repaintColor = variableColor;
  }

  void Update() {}

  public void Flash() {
    variableFlash.Flash();
  }

  public void SetFrozenVariable(GameObject variableObject) {
    SpriteRenderer variableSprite = variableObject.GetComponent<SpriteRenderer>();
    variableSprite.sprite = variableRenderer.sprite;
    variableSprite.color = variableColor;
    variableObject.transform.localPosition = transform.localPosition;
  }
}
