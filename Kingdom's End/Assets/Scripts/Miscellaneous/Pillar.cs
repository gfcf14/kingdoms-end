using UnityEngine;

public class Pillar : MonoBehaviour {
  [SerializeField] string rune;
  void Start() {
    Instantiate(Helpers.GetOrException(Objects.prefabs, $"pillar-{GameData.area}"), transform.position, Quaternion.identity, transform);

    // TODO: refine image and outline dimensions based on rune

    // assign rune image
    SpriteRenderer runeRenderer = transform.Find("Rune").GetComponent<SpriteRenderer>();
    runeRenderer.sprite = Helpers.GetOrException(Sprites.runeImages, rune);

    SpriteRenderer outline = transform.Find("Outline").GetComponent<SpriteRenderer>();

    // assign outline image and color
    outline.sprite = Helpers.GetOrException(Sprites.runeOutlines, rune);
    outline.color = Helpers.GetOrException(Colors.runeOutlineColors, rune);

    // Destroys placeholder once loaded
    Destroy(transform.Find("Placeholder").gameObject);
  }

  void Update() {}
  
  // TODO: define OnCollisionEnter2D to determine actions when attacking with and without talisman relic
}
