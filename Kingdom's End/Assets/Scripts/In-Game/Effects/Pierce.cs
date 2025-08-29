using System;
using UnityEngine;

public class Pierce : MonoBehaviour {
  [NonSerialized] SpriteRenderer objectRenderer;
  [NonSerialized] public Color color;
  [NonSerialized] public bool isFacingLeft;

  void Start() {
    objectRenderer = GetComponent<SpriteRenderer>();
    transform.localScale = new Vector3(isFacingLeft ? -1 : 1, 1, 1);
  }
  void Update() {
    if (color != null) {
      objectRenderer.color = color;
    }
  }

  public void DestroyPierce() {
    Destroy(gameObject);
  }
}
