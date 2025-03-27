using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnergy : MonoBehaviour {
  [System.NonSerialized] Animator anim;
  [System.NonSerialized] public GameObject parent;
  [SerializeField] public string summonKey;

  [SerializeField] public string enemyType;
  [SerializeField] public int enemyLevel;
  void Start() {
    anim = GetComponent<Animator>();
    Vector2 summonDimensions = Helpers.GetOrException(Objects.enemyDimensions, summonKey);
    GetComponent<SpriteRenderer>().size = new Vector2(summonDimensions.x * 1.5f, summonDimensions.y * 1.5f);
  }
  void Update() {}

  public void SummonCurtain() {
    anim.Play("summon-curtain");
  }

  public void SummonEnemy() {
    GameObject.Find("InGame").gameObject.GetComponent<InGame>().SpawnEnemy(transform.position, summonKey, enemyType, null, null, null, false, parent.transform, enemyLevel);
  }

  public void DestroySummon() {
    Destroy(gameObject);
  }
}
