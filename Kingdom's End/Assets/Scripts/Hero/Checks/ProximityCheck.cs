using UnityEngine;

public class ProximityCheck : MonoBehaviour {
  [SerializeField] GameObject hero;
  void Start() {}
  void Update() {}

  private void OnCollisionEnter2D(Collision2D col) {
    if (Helpers.IsValueInArray(Constants.proximityCheckNonColliderTags, col.collider.tag) || col.collider.name == "Grounder") {
      Physics2D.IgnoreCollision(col.collider, GetComponent<BoxCollider2D>());
    } else {
      Debug.Log("colliding with " + col.collider.name);
    }
  }

  private void ActivateActionCanvas() {
    InGame.instance.actionCanvas.SetActive(true);

    if (InGame.instance.infoCanvas.activeSelf) {
      InGame.instance.infoCanvas.GetComponent<InfoCanvas>().AlignRight();
    }
  }

  private void SetNPCAction(NPC npc) {
    if (npc.actionAvailable != "") {
      if (!Hero.instance.isOnChat) {
        ActivateActionCanvas();
      }
      Hero.instance.SetAction(npc.actionAvailable);
      Hero.instance.NPCnearbyAction = npc.actionAvailable;
    }
  }

  private void SetObjectAction(string action) {
    ActivateActionCanvas();
    Hero.instance.SetAction(action);
  }

  // clears the current action when moving away from an interactable object
  void ClearActionOnExit() {
    InGame.instance.actionCanvas.SetActive(false);

    // if the info canvas is active, ensure it shows on the left of the screen
    if (InGame.instance.infoCanvas.activeSelf) {
      InGame.instance.infoCanvas.GetComponent<InfoCanvas>().AlignLeft();
    }

    InGame.instance.actionCanvas.GetComponent<ActionCanvas>().ClearAction();
  }

  private void OnTriggerEnter2D(Collider2D col) {
    string colTag = col.gameObject.tag;

    if (colTag == "NPC") {
      Hero.instance.NPCnearby = col.gameObject.name;
      SetNPCAction(col.gameObject.GetComponent<NPC>());
    } else if (colTag == "Portal") {
      if (!Hero.instance.isFightingBoss) {
        Hero.instance.nearbyInteractableObject = col.gameObject;

        string portalType = col.gameObject.GetComponent<Portal>().portalType;

        string action = portalType == "entrance" ? "enter" : portalType;

        if (portalType == "cave") {
          action = GameData.area == "underground" ? "exit" : "enter";
        }

        SetObjectAction(action);
      }
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    string colTag = col.gameObject.tag;

    if (colTag == "NPC") {
      ClearActionOnExit();
      Hero.instance.NPCnearby = "";
      Hero.instance.NPCnearbyAction = "";
    } else if (colTag == "Portal") {
      // when entering a building, action should not hide immediately as it should change to
      // reflect the action from the portal the player appears in, hence the action canvas
      // should only hide on a trigger exit as the player moves away from the trigger, not
      // when transported by it

      if (Hero.instance.nearbyInteractableObject != null && Hero.instance.nearbyInteractableObject.name == col.gameObject.name) {
        Hero.instance.nearbyInteractableObject = null;
        ClearActionOnExit();
      }
    }
  }

  // gets the first gameobject the proximity check overlaps with
  public GameObject OverlapsWith(string tag) {
    BoxCollider2D proximityCollider = GetComponent<BoxCollider2D>();

    Bounds bounds = proximityCollider.bounds;

    Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);
    foreach (Collider2D col in colliders) {
      if (col.CompareTag(tag)) {
        return col.gameObject;
      }
    }

    return null;
  }

  public void DecideActionShow() {
    GameObject overlappedObject = OverlapsWith("NPC");

    if (overlappedObject != null) {
      Hero.instance.NPCnearby = overlappedObject.name;
      SetNPCAction(overlappedObject.GetComponent<NPC>());
    }
  }
}
