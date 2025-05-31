using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
  [SerializeField] public EnemyKeyTypeCondition enemyKeyTypeCondition;
  [SerializeField] public EnemyType enemyType;
  [SerializeField] public string enemyKey = "";
  [SerializeField] public string gameCondition = "";
  [SerializeField] public List<string> dropConditions = new List<string>();
  [SerializeField] public string specificDrop = "";
  [SerializeField] public bool isMiniBoss = false;

  void Start() {}
  void Update() {}
  public Tuple<string, EnemyType> PopulateValuesPriorToSpawn() {
    string spawnKey = "";
    EnemyType spawnType = EnemyType.unspecified;
    string keyTypeCondition = enemyKeyTypeCondition.ToString();

    if (enemyKey == "") {
      if (keyTypeCondition == "custom") {
        Debug.LogError("Enemy at " + gameObject.transform.parent.name + " needs to have a key specified as its condition is custom");
      } else {
        spawnKey = Helpers.GetRandomItemFromGroup(Helpers.GetOrException(Objects.enemyKeysByArea, GameData.area));
      }
    }

    if (enemyType.ToString() == "unspecified") {
      if (keyTypeCondition == "custom") {
        Debug.LogError("Enemy at " + gameObject.transform.parent.name + " needs to have a type specified as its condition is custom");
      } else {
        Enum.TryParse<EnemyType>(Helpers.GetRandomItemFromGroup(Helpers.GetOrException(Objects.enemyTypesByCondition, keyTypeCondition)), out spawnType);
      }
    }

    return Tuple.Create(spawnKey == "" ? enemyKey : spawnKey, spawnType == EnemyType.unspecified ? enemyType : spawnType);
  }

  public void Spawn() {
    Tuple<string, EnemyType> keyAndTypeForSpawn = PopulateValuesPriorToSpawn();
    string spawnKey = keyAndTypeForSpawn.Item1;
    string spawnType = keyAndTypeForSpawn.Item2.ToString();

    if (spawnType == "ambusher") {
      Vector2 ambushFloorPosition = new Vector2(transform.position.x - 0.5f, transform.position.y);

      GameObject ambushFloor = Instantiate(Helpers.GetOrException(Objects.prefabs, "ambush-floor"), ambushFloorPosition, Quaternion.identity, transform);
      AmbushFloor ambushFloorScript = ambushFloor.GetComponent<AmbushFloor>();

      ambushFloorScript.area = GameData.area;
      ambushFloorScript.enemyType = spawnType;
      ambushFloorScript.enemyKey = spawnKey;
      ambushFloorScript.gameCondition = gameCondition;
      ambushFloorScript.dropConditions = dropConditions;
      ambushFloorScript.specificDrop = specificDrop;
      ambushFloorScript.isMiniBoss = false; // ambushers should NEVER be minibosses
    } else {
      InGame.instance.SpawnEnemy(transform.position, spawnKey, spawnType, gameCondition, dropConditions, specificDrop, isMiniBoss, transform);
    }
  }

  // Destroys all enemies/droppables originated
  public void Cleanse() {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
  }
}
