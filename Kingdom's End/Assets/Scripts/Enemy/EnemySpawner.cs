using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
  [SerializeField] public string enemyType = "";
  [SerializeField] public string enemyKey = "";
  [SerializeField] public string gameCondition = "";
  [SerializeField] public List<string> dropConditions = new List<string>();
  [SerializeField] public string specificDrop = "";
  [SerializeField] public bool isMiniBoss = false;

  void Start() {}
  void Update() {}

  public void Spawn() {
    if (enemyType == "ambusher") {
      Vector2 ambushFloorPosition = new Vector2(transform.position.x - 0.5f, transform.position.y);

      GameObject ambushFloor = Instantiate(Helpers.GetOrException(Objects.prefabs, "ambush-floor"), ambushFloorPosition, Quaternion.identity, transform);
      AmbushFloor ambushFloorScript = ambushFloor.GetComponent<AmbushFloor>();

      ambushFloorScript.area = GameData.area;
      ambushFloorScript.enemyType = enemyType;
      ambushFloorScript.enemyKey = enemyKey;
      ambushFloorScript.gameCondition = gameCondition;
      ambushFloorScript.dropConditions = dropConditions;
      ambushFloorScript.specificDrop = specificDrop;
      ambushFloorScript.isMiniBoss = false; // ambushers should NEVER be minibosses
    } else {
      InGame.instance.SpawnEnemy(transform.position, enemyKey, enemyType, gameCondition, dropConditions, specificDrop, isMiniBoss, transform);
    }
  }

  // Destroys all enemies/droppables originated
  public void Cleanse() {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
  }
}
