using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTrigger : MonoBehaviour {
  [SerializeField] GameObject virtualCam;
  [SerializeField] public string location;


  void Start() {}

  public bool CanSpawnMiniBoss(Transform child) {
    return child.gameObject.tag == "EnemySpawner" && child.gameObject.GetComponent<EnemySpawner>().isMiniBoss;
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("RoomTraverser")) {
      virtualCam.SetActive(true);

      // whenever the hero moves scenes, it gets removed from each VCam object. This line ensures cameras follow the hero singleton
      virtualCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Hero.instance.transform;

      // sets the appropriate tilemaps for save/teleport points if any in the room
        Transform currentSavePoint = gameObject.transform.Find("SaveContainer");

        if (currentSavePoint != null) {
          InGame.instance.globalGradients.savePointTilemap = currentSavePoint.transform.Find("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        }

        Transform currentTeleportPoint = gameObject.transform.Find("TeleportContainer");

        if (currentTeleportPoint != null) {
          InGame.instance.globalGradients.teleportPointTilemap = currentTeleportPoint.transform.Find("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        }

      Hero.instance.currentRoom = gameObject;

      // if the chat canvas is active, close it
      if (InGame.instance.chatCanvas != null && InGame.instance.chatCanvas.activeSelf) {
        InGame.instance.chatCanvas.GetComponent<ChatCanvas>().FinishChat(playerLeft: true);
      }

    }

    // if the player entes a room with a boss
    if (col.gameObject.name == "ProximityCheck") {
      foreach(Transform child in gameObject.transform) {
        if (CanSpawnMiniBoss(child)) {
          Rigidbody2D heroBody = Hero.instance.gameObject.GetComponent<Rigidbody2D>();
          Hero heroScript = Hero.instance.gameObject.GetComponent<Hero>();

          Hero.instance.SetPauseCase("boss-room-entry");
          Hero.instance.bossTransitionDirection = (int)(heroBody.linearVelocity.x / Math.Abs(heroBody.linearVelocity.x));
          StartCoroutine(PauseRoomWhileOnBossEntry());
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.CompareTag("RoomTraverser")) {
      virtualCam.SetActive(false);
      foreach (Transform child in gameObject.transform) {
        if (child.tag == "EnemySpawner") {
          child.gameObject.GetComponent<EnemySpawner>().Cleanse();
        // } else if (child.name == "Boss") {
        //   child.gameObject.GetComponent<Enemy>().isOnCamera = false;
        // } else if (child.name.Contains("Droppable")) {
        //   Droppable droppableInstance = child.Find("GameObject").gameObject.GetComponent<Droppable>();

        //   // only spawned items get destroyed; items that are part of the Scene will only destroy when grabbed
        //   if (droppableInstance.room != null) {
        //     GameObject.Destroy(child.gameObject);
        //   }
        } else if (child.name.Contains("ArrowBurn")) { // destroys arrow burns so they don't infinitely harm enemies when player exits and enters repeatedly
          GameObject.Destroy(child.gameObject);
        }
      }
    }

    if (col.gameObject.name == "ProximityCheck") {
      if (Hero.instance.isAutonomous && Hero.instance.mustTransitionOnAir) {
        Hero.instance.mustTransitionOnAir = false;
        Hero.instance.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
      }
    }
  }

  IEnumerator PauseRoomWhileOnBossEntry() {
    InGame.instance.ToggleSoundtrack(isPaused: false, restart: true);

    yield return new WaitForSecondsRealtime(3);

    Hero.instance.ClearPauseCase();
    Hero.instance.isFightingBoss = true;
    Hero.instance.isAutonomous = true;
    Hero.instance.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

    if (Hero.instance.isFacingLeft && Hero.instance.bossTransitionDirection == 1) {
      Hero.instance.transform.localScale = Vector3.one;
      Hero.instance.isFacingLeft = false;
    } else if (!Hero.instance.isFacingLeft && Hero.instance.bossTransitionDirection == -1) {
      Hero.instance.transform.localScale = new Vector3(-1, 1, 1);
      Hero.instance.isFacingLeft = true;
    }

    if (!Hero.instance.isGrounded) {
      Hero.instance.mustTransitionOnAir = true;
    }
  }
}
