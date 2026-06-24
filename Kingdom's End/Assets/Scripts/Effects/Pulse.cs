using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pulse : MonoBehaviour {
  [SerializeField] private SpriteRenderer sourceRenderer;

  [Header("Shader values")]
  [SerializeField] private float outlineWidth = 2f;
  [SerializeField] private float pulseSpeed = 4f;
  [SerializeField] private float minAlpha = 0f;
  [SerializeField] private float maxAlpha = 1f;

  // TODO: set this color based on magic damage infringement
  [SerializeField] public Color outlineColor = Color.cyan;

  private SpriteRenderer outlineRenderer;
  private MaterialPropertyBlock propertyBlock;

  private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");
  private static readonly int OutlineWidthId = Shader.PropertyToID("_OutlineWidth");
  private static readonly int PulseSpeedId = Shader.PropertyToID("_PulseSpeed");
  private static readonly int MinAlphaId = Shader.PropertyToID("_MinAlpha");
  private static readonly int MaxAlphaId = Shader.PropertyToID("_MaxAlpha");

  private void Awake() {
    sourceRenderer = transform.parent.GetComponent<SpriteRenderer>();
    outlineRenderer = GetComponent<SpriteRenderer>();
    propertyBlock = new MaterialPropertyBlock();
  }

  private void LateUpdate() {
    // Animator has already chosen this frame's sprite by LateUpdate.
    outlineRenderer.sprite = sourceRenderer.sprite;
    outlineRenderer.flipX = sourceRenderer.flipX;
    outlineRenderer.flipY = sourceRenderer.flipY;

    // Optional: keeps it aligned if the source renderer changes its draw mode.
    outlineRenderer.drawMode = sourceRenderer.drawMode;
    outlineRenderer.size = sourceRenderer.size;

    ApplyShaderValues();
  }

  public void SetOutlineColor(Color newColor) {
    outlineColor = newColor;
    ApplyShaderValues();
  }

  public void SetPulse(Color newColor, float newPulseSpeed, float newMinAlpha = 0f, float newMaxAlpha = 1f) {
    outlineColor = newColor;
    pulseSpeed = newPulseSpeed;
    minAlpha = newMinAlpha;
    maxAlpha = newMaxAlpha;

    ApplyShaderValues();
  }

  private void ApplyShaderValues() {
    if (outlineRenderer == null) return;

    outlineRenderer.GetPropertyBlock(propertyBlock);

    propertyBlock.SetColor(OutlineColorId, outlineColor);
    propertyBlock.SetFloat(OutlineWidthId, outlineWidth);
    propertyBlock.SetFloat(PulseSpeedId, pulseSpeed);
    propertyBlock.SetFloat(MinAlphaId, minAlpha);
    propertyBlock.SetFloat(MaxAlphaId, maxAlpha);

    outlineRenderer.SetPropertyBlock(propertyBlock);
  }
}
