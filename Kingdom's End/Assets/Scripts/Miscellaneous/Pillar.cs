using UnityEngine;

public class Pillar : MonoBehaviour {
  [SerializeField] string rune;
  [SerializeField] string area;
  void Start() {
    // TODO: use GameData.area once fully implemented

    // assign rune image


    SpriteRenderer outline = transform.Find("Outline").GetComponent<SpriteRenderer>();

    // assign outline color
    outline.color = Helpers.GetOrException(Colors.runeOutlineColors, rune);

    // Destroys placeholder once loaded
    Destroy(transform.Find("Placeholder").gameObject);
  }

  void Update() {}
}
