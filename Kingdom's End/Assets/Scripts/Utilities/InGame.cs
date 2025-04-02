using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InGame : MonoBehaviour {
  private Tilemap groundTiles;
  private Tilemap detailTiles;
  private AudioSource soundtrack;
  private float fadeDuration = 0.25f;

  [SerializeField] float soundtrackPausedTime = 0f; // Stores the soundtrack paused time position
  [SerializeField] float miniBossTrackPausedTime = 0f; // Stores the min boss track paused time position

  public Hero hero;
  public GameObject mainOverlay;
  public GameObject bossStatusCanvas;

  [SerializeField] public GameObject pauseCanvas;
  public GlobalGradients globalGradients;

  void Start() {
    SetComponents();

    if (DataManager.instance.playerPosition.HasValue) {
      hero.ModifyPosition(DataManager.instance.playerPosition.Value);
    }

    if (DataManager.instance.playerFalling.HasValue) {
      hero.isFalling = DataManager.instance.playerFalling.Value;
    }
  }

  public void SetComponents() {
    groundTiles = GameObject.Find("Floors").GetComponent<Tilemap>();
    detailTiles = GameObject.Find("Detail").GetComponent<Tilemap>();
    mainOverlay = GameObject.Find("MainOverlay");
    soundtrack = GetComponent<AudioSource>();
    soundtrack.volume = Settings.maxSoundtrackVolume;
    soundtrack.loop = true;

    hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
    globalGradients = GameObject.Find("Global Gradients").GetComponent<GlobalGradients>();
  }
  void Update() {}

  public void PlaySoundtrack(string key) {
    soundtrack.clip = Helpers.GetOrException(Sounds.soundtracks, key);

    if (Settings.playSoundtrack) {
      soundtrack.Play();
    }
  }

  public void StopSoundtrack() {
    soundtrack.Stop();
  }

  public void SwitchFromMiniBossTrack(string key, bool bossCausingLevelup) {
    StopSoundtrack();
    soundtrack.clip = Helpers.GetOrException(Sounds.soundtracks, key);
    if (!bossCausingLevelup) {
      StartCoroutine(FadeIn(wait: true));
    }
    miniBossTrackPausedTime = 0;
  }

  public void ClearPauseTimes() {
    miniBossTrackPausedTime = 0;
    soundtrackPausedTime = 0;
    soundtrack.time = 0;
  }

  public void ToggleSoundtrack(bool isPaused, bool restart = false, bool wait = false) {
    if (isPaused) {
      StartFadeIn(wait);
    } else {
      if (soundtrack.isPlaying) { // must check if soundtrack is playing; when not playing, time is 0 thus choosing to mute backgrounds would "reset" soundtrack position
        if (hero.isFightingBoss) {
          miniBossTrackPausedTime = soundtrack.time;
        } else {
          soundtrackPausedTime = soundtrack.time;
        }
      }

      // ensures that, when switching soundtracks, the new soundtrack starts from the beginning
      if (restart) {
        soundtrack.time = 0f;
      }

      StartFadeOutAndPause();
    }
  }

  public void StartFadeOutAndPause() {
    StartCoroutine(FadeOutAndPause());
  }

  public void StartFadeIn(bool wait = false) {
    StartCoroutine(FadeIn(wait));
  }

  public void FlashFadeOut() {
    mainOverlay.GetComponent<MainOverlay>().FlashFadeOut();
  }

  public void FlashFinish() {
    hero.PerformPortalTransport();
  }

  public void FlashFadeIn() {
    mainOverlay.GetComponent<MainOverlay>().FlashFadeIn();
  }

  public void ChangeArea() {
    pauseCanvas.GetComponent<Pause>().ChangeArea();
    mainOverlay.GetComponent<MainOverlay>().PrepareAreaTransition();
  }

  private IEnumerator FadeOutAndPause() {
    float startVolume = soundtrack.volume;

    while (soundtrack.volume > 0f) {
      soundtrack.volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
      yield return null;
    }

    StopSoundtrack();
    soundtrack.volume = startVolume;
  }

  private IEnumerator FadeIn(bool wait = false) {
    if (wait) {
      yield return new WaitForSeconds(1);
    }

    soundtrack.time = hero.isFightingBoss ? miniBossTrackPausedTime : soundtrackPausedTime;
    soundtrack.volume = 0;
    if (Settings.playSoundtrack) {
      soundtrack.Play();
    }

    while (soundtrack.volume < Settings.maxSoundtrackVolume) {
      soundtrack.volume += Time.unscaledDeltaTime / fadeDuration;
      yield return null;
    }

    soundtrack.volume = Settings.maxSoundtrackVolume;
  }

  public void SetPauseCase(string pauseCase) {
    hero.SetPauseCase(pauseCase);
  }

  public void ClearPauseCase() {
    hero.ClearPauseCase();
  }

  public void Cover() {
    mainOverlay.GetComponent<Image>().color = new Color(0, 0, 0, 1);
  }

  public void InstantiatePrefab(string prefab, string key, string rarity, GameObject room, Vector2 position, SpriteRenderer spr, bool shouldRotate = false, string rotateDirection = "", GameObject spawnedFrom = null) {
    // mainly so items instantiated from stacked breakables do not overlap fully
    float randomOffset = UnityEngine.Random.Range(-0.2f, 0.2f);

    GameObject droppedItem = Instantiate(Helpers.GetOrException(Objects.prefabs, prefab), position, Quaternion.identity, room.transform);
    Droppable droppableScript = droppedItem.transform.GetComponent<Droppable>();
    droppableScript.key = key;
    droppableScript.rarity = rarity;
    droppableScript.isDropped = true;
    droppableScript.room = room;

    if (shouldRotate) {
      droppableScript.shouldRotate = shouldRotate;
      droppableScript.rotateDirection = rotateDirection;
    }
    // provides the game object from which the enemy that instantiated the item spawned from, to manipulate it as needed after picking the item
    if (spawnedFrom) {
      droppableScript.spawnedFrom = spawnedFrom;
    }

    // adds flicker effect
    droppableScript.gameObject.AddComponent<Flicker>().enabled = false;
  }

  public string GetGroundMaterial(string tileName) {
    if (tileName == "" || tileName == null) {
      return null;
    }

    // gets the material given that tilebase name convension is "tiles-material_number"
    string material = tileName.Split('_')[0].Split('-')[1];

    return Helpers.GetMaterial(material, tileName);
  }

  public string GetTileName(Vector3 objectPosition) {
    Vector3Int groundTileCoordinates = groundTiles.WorldToCell(objectPosition);
    Vector3 tileCenter = groundTiles.GetCellCenterWorld(groundTileCoordinates);
    Vector3 tileHalfSize = groundTiles.cellSize / 2;
    Vector3Int groundTileBelowCoordinates = groundTileCoordinates;
    TileBase groundTileBelowPlayer = groundTiles.GetTile(groundTileBelowCoordinates);

    // if not found, get the tile below
    if (groundTileBelowPlayer == null) {
      groundTileCoordinates = groundTileCoordinates + new Vector3Int(0, -1, 0);
      tileCenter = groundTiles.GetCellCenterWorld(groundTileCoordinates);
      groundTileBelowCoordinates = groundTileCoordinates;
      groundTileBelowPlayer = groundTiles.GetTile(groundTileBelowCoordinates);
    }

    // Draw lines around the boundaries of the selected tile
    Debug.DrawLine(tileCenter + new Vector3(-tileHalfSize.x, -tileHalfSize.y), tileCenter + new Vector3(tileHalfSize.x, -tileHalfSize.y), Color.red);
    Debug.DrawLine(tileCenter + new Vector3(tileHalfSize.x, -tileHalfSize.y), tileCenter + new Vector3(tileHalfSize.x, tileHalfSize.y), Color.red);
    Debug.DrawLine(tileCenter + new Vector3(tileHalfSize.x, tileHalfSize.y), tileCenter + new Vector3(-tileHalfSize.x, tileHalfSize.y), Color.red);
    Debug.DrawLine(tileCenter + new Vector3(-tileHalfSize.x, tileHalfSize.y), tileCenter + new Vector3(-tileHalfSize.x, -tileHalfSize.y), Color.red);

    return groundTileBelowPlayer ? groundTileBelowPlayer.name : "";
  }

  // TODO: refactor to use the location, or a specific grid in the room, ensuring that this grid allows a material, to use this for sounds
  public string GetTileMaterial(Vector3 objectPosition) {
    Vector3Int groundTileCoordinates = groundTiles.WorldToCell(objectPosition);
    Vector3Int groundTileBelowCoordinates = groundTileCoordinates + new Vector3Int(0, -1, 0);

    Vector3Int detailTileCoordinates = detailTiles.WorldToCell(objectPosition);
    Vector3Int detailTileBelowCoordinates = detailTileCoordinates + new Vector3Int(0, -1, 0);

    TileBase groundTileBelowPlayer = groundTiles.GetTile(groundTileBelowCoordinates);
    TileBase detailTileBelowPlayer = detailTiles.GetTile(detailTileBelowCoordinates);

    if (detailTileBelowPlayer != null) {
      return "grass";
      // int detailTileIndex = int.Parse(detailTileBelowPlayer.name.Replace("tiles-details_", ""));

      // if (Helpers.IsValueInArray(Constants.detailDirt, detailTileIndex)) {
      //   return "dirt";
      // } else {
      //   return GetGroundMaterial(groundTileBelowPlayer == null ? "" : groundTileBelowPlayer.name);
      // }
    } else {
      return GetGroundMaterial(groundTileBelowPlayer == null ? "" : groundTileBelowPlayer.name);
    }
  }

  public bool IsInRoom(string roomName) {
    return roomName == hero.currentRoom.name;
  }

  // Checks the name of the provided parent if it's a room. If not a room, get its parent and recheck. If null, return blank
  public string FindRoom(Transform currentParentCheck) {
    if (currentParentCheck == null) {
      return "";
    }

    if (currentParentCheck.gameObject.name.Contains("Room")) {
      return currentParentCheck.gameObject.name;
    }

    return FindRoom(currentParentCheck.parent);
  }

  // draws a rectangle based on parameter given
  public void DrawRectangle(Vector2 center, Vector2 size) {
    Vector2 halfSize = size / 2f;

    Vector3 topLeft = new Vector3(center.x - halfSize.x, center.y + halfSize.y, 0f);
    Vector3 topRight = new Vector3(center.x + halfSize.x, center.y + halfSize.y, 0f);
    Vector3 bottomRight = new Vector3(center.x + halfSize.x, center.y - halfSize.y, 0f);
    Vector3 bottomLeft = new Vector3(center.x - halfSize.x, center.y - halfSize.y, 0f);

    Debug.DrawLine(topLeft, topRight, Color.red);
    Debug.DrawLine(topRight, bottomRight, Color.red);
    Debug.DrawLine(bottomRight, bottomLeft, Color.red);
    Debug.DrawLine(bottomLeft, topLeft, Color.red);
  }

  public void DrawDamage(Vector2 position, int damage, bool? isCritical, string soundType = "") {
    GameObject damageObject = Instantiate(Helpers.GetOrException(Objects.prefabs, "damage-container"), position, Quaternion.identity);
    damageObject.transform.SetParent(null);
    DamageContainer damageScript = damageObject.GetComponent<DamageContainer>();
    damageScript.damage = damage;
    damageScript.isCritical = isCritical ?? false;

    if (soundType != "") {
      damageScript.soundType = soundType;
    }
  }

  // Plays a sound by creating a sound prefab that lives only until it is done playing
  public void PlaySound(AudioClip clip, Vector3 position) {
    GameObject sound = Instantiate(Helpers.GetOrException(Objects.prefabs, "sound"), position, Quaternion.identity);
    Sound soundInstance = sound.GetComponent<Sound>();
    soundInstance.PlaySound(clip);
  }

  // instantiates a defense/block sprite on a contact point
  public void Block(Vector2 position, bool isFacingLeft) {
    GameObject defenseEffect = Instantiate(Helpers.GetOrException(Objects.prefabs, "defense"), position, Quaternion.identity);
    defenseEffect.GetComponent<Defense>().isFacingLeft = isFacingLeft;
  }

  public bool CheckDropCondition(string condition) {
    string[] conditionEntry = condition.Split(':');
    string conditionKey = conditionEntry[0];
    string conditionValue = conditionEntry[1];

    switch (conditionKey) {
      case "item": // Check if player doesn't have an item. Condition is of the form item:<key>=<amount>
        string[] itemAndAmount = conditionValue.Split('=');

        Debug.Log(condition + ": " + Helpers.HasAmount(hero.items, itemAndAmount[0], int.Parse(itemAndAmount[1])));

        return !Helpers.HasAmount(hero.items, itemAndAmount[0], int.Parse(itemAndAmount[1]));
      case "hp": // Check if player's HP is less than or equal to amount. CSondition is of the form hp:<amount>

        Debug.Log(condition + ": " + (hero.currentHP <= int.Parse(conditionValue)));

        return hero.currentHP <= int.Parse(conditionValue);
      default:
        return false;
    }
  }

  public void SpawnEnemy(Vector2 position, string enemyKey, string enemyType, string condition, List<string> dropConditions, string specificDrop, bool isMiniBoss, Transform parent, int level = 1) {
    GameObject enemySpawned = Instantiate(Helpers.GetOrException(Objects.prefabs, "enemy"), new Vector3(position.x, position.y, 0), Quaternion.identity, parent);
    Enemy enemyScript = enemySpawned.GetComponent<Enemy>();
    enemyScript.key = enemyKey != "" ? enemyKey : Constants.meadowEnemies[UnityEngine.Random.Range(0, Constants.meadowEnemies.Length)];
    enemyScript.isMiniBoss = isMiniBoss;

    if (isMiniBoss) {
      enemyScript.isOnCamera = true;
    }

    // Specifies a type for the enemy. If none, default to patroller
    enemyScript.type = enemyType ?? "patroller";

    // if there is a game condition, apply drop conditions check
    if (condition != "") {
      FieldInfo stateField = typeof(GameData).GetField(condition);

      if (stateField != null) {
        // NOTE: game data variables should initially be FALSE until a fulfilled event turns them to TRUE
        // Example: hasMetPrincess = false. Once the player speaks to the princess for the first time, this becomes true
        if (!(bool)stateField.GetValue(null)) {

          // since the game condition is not met, check each drop condition. If at least one is not true, assign the specific drop
          bool anyConditionFalse = dropConditions.All(condition => CheckDropCondition(condition));

          if (anyConditionFalse) {
            // if all drop conditions are true, do not assign the specific drop
            enemyScript.specificDrop = specificDrop;
          }
        } else { // clears the game, drop condition and specific drop if it passed
          condition = "";
          dropConditions.Clear();
          specificDrop = "";
        }

        enemyScript.spawnedFrom = gameObject;
      } else {
        Debug.Log("Invalid GameData lookup: The property " + condition + " doesn't appear to exist...");
      }
    } else if (specificDrop != "") { // if no condition, only check if there is a specific drop
      enemyScript.specificDrop = specificDrop;
      enemyScript.spawnedFrom = gameObject;
    }

    // TODO: implement a better way to assign level values
    enemyScript.level = level;
  }
}
