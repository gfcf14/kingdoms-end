using UnityEngine;

public class RoomTraverser : MonoBehaviour {
  Hero hero;
  void Start() {
    hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
  }

  void Update() {}

  public void OnTriggerEnter2D(Collider2D col) {
    if (col.tag == "Room") {
      // if entering room, move based on speed to scape the previous room's bounds. Avoid displacements if on inclines
      float xDisp = ((hero.heroWidth / 4) * hero.body.velocity.x) * (hero.groundType == "level" ? 1 : 0);
      // TODO: consider if there would be any cases where it could be necessary to switch vertical positions.
      // Currently this is "off" as it gets messed up when transitioning due to a jump or falling
      float yDisp = 0; // hero.isDropKicking ? 0 : ((hero.heroHeight / 4) * hero.body.velocity.y) * (hero.groundType == "level" ? 1 : 0);

      hero.transform.position = new Vector2(hero.transform.position.x + xDisp, hero.transform.position.y + yDisp);

      RoomTrigger newRoom = col.gameObject.GetComponent<RoomTrigger>();
      string location = newRoom.location;

      if (GameData.area != location && location != "intersection") {
        InGame.instance.ResetTilemaps();

        if (location == "indoors") {
          InGame.instance.globalGradients.isIndoors = true;
        } else {
          GameData.area = location;
          InGame.instance.globalGradients.isIndoors = false;
          InGame.instance.globalGradients.area = location;
        }
      }
    }
  }
}
