using System;
using UnityEngine;

public class VariableSprite : MonoBehaviour {
  [NonSerialized] SpriteRenderer variableRenderer;
  [NonSerialized] private SimpleFlash variableFlash;
  [NonSerialized] private Enemy enemyScript;
  Color variableColor;

  public bool isColorChanging = false;
  void Start() {
    variableRenderer = GetComponent<SpriteRenderer>();
    variableFlash = GetComponent<SimpleFlash>();
    enemyScript = transform.parent.gameObject.GetComponent<Enemy>();

    variableColor = Helpers.GetOrException(Colors.variableColors, GameData.area);
    variableRenderer.color = variableColor;
    variableFlash.repaintColor = variableColor;
  }

  void Update() {}

  void LateUpdate() {
    if (isColorChanging) {
      if (enemyScript.isBurning) {
        SetColor(Helpers.GetOrException(Colors.statusColors, "burned"));
      } else if (enemyScript.isFlashing) {
        SetColor(Color.white);

      // TODO: somehow teleporter is a special case. Investigate why
      } else if (enemyScript.type == "teleporter") {
        bool isIdle = enemyScript.anim.GetCurrentAnimatorStateInfo(0).IsName("idle");

        if (isIdle && !enemyScript.isFlashing) {
          ResetColor();
        }

      // when enemy is exploding
      } else if (enemyScript.type == "exploder" && enemyScript.isExploding) {
        SetColor(enemyScript.enemyRenderer.color);

      // when bewitcher attacks
      } else if (enemyScript.type == "bewitcher" && enemyScript.isAttacking) {
        SetColor(enemyScript.enemyRenderer.color);
      } else {
        ResetColor();
      }
    }
  }

  public void Flash() {
    variableFlash.Flash();
  }

  public void SetFrozenVariable(GameObject variableObject) {
    SpriteRenderer variableSprite = variableObject.GetComponent<SpriteRenderer>();
    variableSprite.sprite = variableRenderer.sprite;
    variableSprite.color = variableColor;
    variableObject.transform.localPosition = transform.localPosition;
  }

  // combines incoming color with the one obtained from the variableColors dictionary
  // by multiplying the status color into the variable color
  public void SetColor(Color newColor) {
    Color baseColor = enemyScript.isFlashing ? Color.white : variableColor;
    Color combinedColor = new (baseColor.r * newColor.r, baseColor.g * newColor.g, baseColor.b * newColor.b, baseColor.a * newColor.a);

    variableRenderer.color = combinedColor;
    variableFlash.repaintColor = combinedColor;
  }

  public void ResetColor() {
    variableRenderer.color = variableColor;
    variableFlash.repaintColor = variableColor;
  }
}
