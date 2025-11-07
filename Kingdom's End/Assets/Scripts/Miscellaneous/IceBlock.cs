using UnityEngine;

public class IceBlock : MonoBehaviour {
  [SerializeField] public string itemKey;
  [SerializeField] public string itemRarity;
  [SerializeField] public bool isFacingLeft;
  GameObject enemy;
  void Start() {
    enemy = transform.Find("Enemy").gameObject;

    if (isFacingLeft) {
      enemy.transform.localScale = new Vector2(-1, 1);
    }

    enemy.transform.localPosition = new Vector2(0, -enemy.GetComponent<SpriteRenderer>().bounds.size.y / 2);
  }

  void Update() {}

  public void DestroyBlock() {
    InGame.instance.InstantiatePrefab("droppable", itemKey, itemRarity, transform.parent.gameObject, transform.position, shouldRotate: false, "", transform.parent.gameObject);
    InGame.instance.PrepareFullRockExplosion(gameObject, "ice", "ice");
  }
}
