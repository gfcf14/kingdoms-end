using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)] // useful to start MainCamera later and successfully apply player position first
public class MainCamera : MonoBehaviour {
  void Awake() {
    Vector2 cameraPosition = new Vector2(GameData.mainCameraX, GameData.mainCameraY);

    if (DataManager.instance != null && DataManager.instance.newCameraPosition.HasValue) {
      cameraPosition = DataManager.instance.newCameraPosition.Value;
    }
    Camera.main.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -10);
  }

  void Start() {    
    if (Hero.instance == null) return;

    // if Hero is not null, get its currentRoom's scene's RoomTrigger
      GameObject currentRoom = Hero.instance.currentRoom;
      if (currentRoom != null && currentRoom.scene == gameObject.scene && currentRoom.TryGetComponent(out RoomTrigger currentRoomTrigger)) {
        currentRoomTrigger.ActivateVirtualCamera();
        return;
      }

    // if currentRoom is null, get its RoomTraverser's collider

      RoomTraverser roomTraverser = Hero.instance.GetComponentInChildren<RoomTraverser>();
      if (roomTraverser == null) return;

      Collider2D traverserCollider = roomTraverser.GetComponent<Collider2D>();
      if (traverserCollider == null) return;

    // update to reflect recent platform changes
    Physics2D.SyncTransforms();
    
    // use the Hero's RoomTraverser to find the room it's in, and if so, activate its VCam.
    // One time thing to position the scene's MainCamera
    var contactFilter = new ContactFilter2D();
    contactFilter.SetLayerMask(Physics2D.AllLayers);
    contactFilter.useTriggers = true;

    var overlappingColliders = new List<Collider2D>();
    traverserCollider.Overlap(contactFilter, overlappingColliders);

    foreach (Collider2D overlappingCollider in overlappingColliders) {
      if (overlappingCollider.CompareTag("Room") && overlappingCollider.TryGetComponent(out RoomTrigger roomTrigger)) {
        roomTrigger.ActivateVirtualCamera();
        return;
      }
    }
  }
}
