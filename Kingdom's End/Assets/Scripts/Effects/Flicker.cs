using UnityEngine;

public class Flicker : MonoBehaviour {
  public SpriteRenderer droppableRenderer;
  public bool alpha = false;

  public float flickerInterval = 0.025f;
  private float flickerTimer = 0f;

  void Start() {
    droppableRenderer = GetComponent<SpriteRenderer>();
  }

  void Update() {
    flickerTimer += Time.deltaTime;

    if (flickerTimer >= flickerInterval) {
      flickerTimer = 0f;

      Color newColor = droppableRenderer.color;
      newColor.a = alpha ? 1 : 0;
      droppableRenderer.color = newColor;

      alpha = !alpha;
    }
  }
}
