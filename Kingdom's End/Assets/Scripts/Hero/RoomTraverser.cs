using UnityEngine;

public class RoomTraverser : MonoBehaviour {
  void Start() {}

  void Update() {}

  public void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Room") {
      // if entering room, move based on speed to scape the previous room's bounds. Avoid displacements if on inclines
      float xDisp = ((Hero.instance.heroWidth / 4) * Hero.instance.body.velocity.x) * (Hero.instance.groundType == "level" ? 1 : 0);
      // TODO: consider if there would be any cases where it could be necessary to switch vertical positions.
      // Currently this is "off" as it gets messed up when transitioning due to a jump or falling
      float yDisp = 0; // Hero.instance.isDropKicking ? 0 : ((Hero.instance.heroHeight / 4) * Hero.instance.body.velocity.y) * (Hero.instance.groundType == "level" ? 1 : 0);

      Hero.instance.transform.position = new Vector2(Hero.instance.transform.position.x + xDisp, Hero.instance.transform.position.y + yDisp);

      RoomTrigger newRoom = col.gameObject.GetComponent<RoomTrigger>();
      string location = newRoom.location;

      if (GameData.area != location && location != "intersection") {
        if (InGame.instance != null) {
          InGame.instance.ResetTilemaps();
        }

        if (location == "indoors") {
          InGame.instance.globalGradients.isIndoors = true;
        } else {
          GameData.area = location;
          InGame.instance.globalGradients.isIndoors = false;
          InGame.instance.globalGradients.area = location;

          Helpers.SetZoneByArea(location);
        }
      }

      // only instantiate enemies as soon as the area is updated if at all
      foreach (Transform child in col.gameObject.transform) {
        // TODO: when implementing common enemies (nomad, wanderess, or luxhusk) ensure only one of each can implement.
        if (child.tag == "EnemySpawner") {
          child.gameObject.GetComponent<EnemySpawner>().Spawn();
          // } else if (CanSpawnMiniBoss(child)) {
          //   child.gameObject.GetComponent<Enemy>().isOnCamera = true;
        }
      }
    }
  }
}
