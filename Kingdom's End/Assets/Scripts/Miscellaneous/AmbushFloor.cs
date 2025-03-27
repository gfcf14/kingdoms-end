using System.Collections.Generic;
using UnityEngine;

public class AmbushFloor : MonoBehaviour {
  [SerializeField] public string area;
  [SerializeField] public string enemyType = "";
  [SerializeField] public string enemyKey = "";
  [SerializeField] public string gameCondition = "";
  [SerializeField] public List<string> dropConditions = new List<string>();
  [SerializeField] public string specificDrop = "";
  [SerializeField] public bool isMiniBoss = false;
  private InGame inGame;
  private Animator anim;
  private SpriteRenderer tileImage;

  void Start() {
    anim = GetComponent<Animator>();
    anim.enabled = false;
    inGame = GameObject.Find("InGame").gameObject.GetComponent<InGame>();
    tileImage = transform.Find("Tile").gameObject.GetComponent<SpriteRenderer>();
    tileImage.sprite = Helpers.GetOrException(Sprites.ambushInitialSprites, area);
  }

  void Update() {}

  void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.CompareTag("Hero")) {
      anim.enabled = true;
      anim.Play("ambush-" + area);
    }
  }

  public void SpawnEnemy() {
    inGame.PlaySound(Helpers.GetOrException(Sounds.ambushFloorSounds, area), transform.position);
    inGame.SpawnEnemy(transform.position, enemyKey, enemyType, gameCondition, dropConditions, specificDrop, isMiniBoss, transform.parent);
  }

  public void Destroy() {
    Destroy(gameObject);
  }
}
