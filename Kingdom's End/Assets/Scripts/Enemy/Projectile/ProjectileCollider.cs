using UnityEngine;

public class ProjectileCollider : MonoBehaviour {
  void Start() {}

  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    string colliderTag = col.gameObject.tag;
    GameObject parentObject = transform.parent.gameObject;

    if (colliderTag == "Floor" || colliderTag == "Wall") {
      Explode(parentObject);
    } else if ((gameObject.tag == "EnemyWeapon" && Helpers.IsValueInArray(Constants.enemyThrowableBouncers, colliderTag)) || (gameObject.tag == "Weapon" && colliderTag == "EnemyWeapon")) {
      if (colliderTag == "Hero") {
        Hero heroInstance = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
        // ensures the hero isn't damaged after being damaged
        if (!heroInstance.isInvulnerable) {
          heroInstance.ReceiveProjectile(parentObject, col.ClosestPoint(transform.position));
        }
      }

      Explode(parentObject);

      // ensures the animation starts at the impact point
      if (gameObject.tag == "EnemyWeapon") {
        // plays a weapon clash sound when enemy throwables collide with the player weapon
        if (colliderTag == "Weapon") {
          GameObject.Find("InGame").gameObject.GetComponent<InGame>().PlaySound(Helpers.GetOrException(Sounds.blockSounds, "basic"), transform.position);
        }
      }

      // plays a weapon clash sound when two opposing throwables collide
      if (colliderTag == "EnemyWeapon") {
        GameObject.Find("InGame").gameObject.GetComponent<InGame>().PlaySound(Helpers.GetOrException(Sounds.blockSounds, "basic"), transform.position);
      }
    }
  }

  void Explode(GameObject parentObject) {
    parentObject.GetComponent<Projectile>().StopProjectile();
    GameObject projectileExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), transform.position, Quaternion.identity, transform.parent);
    projectileExplosion.GetComponent<Explosion>().type = "projectile";
  }
}
