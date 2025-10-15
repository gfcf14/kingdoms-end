using UnityEngine;
using UnityEngine.Rendering;

public class Pillar : MonoBehaviour {
  [SerializeField] string rune;
  void Start() {
    Instantiate(Helpers.GetOrException(Objects.prefabs, $"pillar-{GameData.area}"), transform.position, Quaternion.identity, transform);

    // TODO: refine image and outline dimensions based on rune
    RuneSpecs runeSpecs = Helpers.GetOrException(Objects.runeSpecs, rune);

    // assign rune image, scale and position
    GameObject runeObject = transform.Find("Rune").gameObject;
    SpriteRenderer runeRenderer = runeObject.GetComponent<SpriteRenderer>();

    runeRenderer.sprite = Helpers.GetOrException(Sprites.runeImages, rune);
    runeObject.transform.localScale = new Vector2(runeSpecs.scale.x, runeSpecs.scale.y);
    runeObject.transform.localPosition = new Vector2(runeSpecs.position.x, runeSpecs.position.y);

    // assign outline image, color, scale and position
    GameObject outlineObject = transform.Find("Outline").gameObject;
    SpriteRenderer outlineRenderer = outlineObject.GetComponent<SpriteRenderer>();

    outlineRenderer.sprite = Helpers.GetOrException(Sprites.runeOutlines, rune);
    outlineRenderer.color = Helpers.GetOrException(Colors.runeOutlineColors, rune);
    outlineRenderer.transform.localScale = new Vector2(runeSpecs.scale.x, runeSpecs.scale.y);
    outlineRenderer.transform.localPosition = new Vector2(runeSpecs.position.x, runeSpecs.position.y);

    // Destroys placeholder once loaded
    Destroy(transform.Find("Placeholder").gameObject);
  }

  void Update() {}

  void Trigger(Collider2D col) {
    if (col.gameObject.tag == "Weapon") {
      if (Helpers.HasItem(Hero.instance.relicItems, $"talisman-{rune}")) {
        InGame.instance.PrepareFullRockExplosion(gameObject, "pillar", GameData.area);
      } else {
        bool isThrowable = col.gameObject.name.Contains("Throwable");
        InGame.instance.Block(col.ClosestPoint(transform.position), Hero.instance.isFacingLeft, volume: isThrowable ? 1 : 0.33f);

        // if any weapon is thrown at a currently non-destroyable pillar, it should get destroyed, not pass through
        if (isThrowable) {
          col.transform.parent.gameObject.GetComponent<Throwable>().DestroyThrowable();
        }
      }
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    Trigger(col);
  }

  void OnTriggerStay2D(Collider2D col) {
    Trigger(col);
  }
}
