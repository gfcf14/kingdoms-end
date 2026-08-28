using UnityEngine;

public class ProjectileCollider : MonoBehaviour {
  void Start() {}

  void Update() {}

  void ExplosionWithBlock() {
    Explode(transform.parent.gameObject);
    // TODO: ensure this block can be different per weapon used (e.g. fist, kick, sword, rock club, etc.)
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.blockSounds, "basic"), transform.position);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    string colliderTag = col.gameObject.tag;
    GameObject parentObject = transform.parent.gameObject;
    bool hit = false;

    if (colliderTag == "Floor" || colliderTag == "Wall") {
      Explode(parentObject);
    } else if ((gameObject.tag == "EnemyWeapon" && Helpers.IsValueInArray(Constants.enemyThrowableBouncers, colliderTag)) || (gameObject.tag == "Weapon" && colliderTag == "EnemyWeapon")) {
      if (colliderTag == "Hero") {
        // ensures the hero isn't damaged after being damaged
        if (!Hero.instance.isInvulnerable) {
          Hero.instance.ReceiveProjectile(parentObject.transform.position.x, col.ClosestPoint(transform.position));
          hit = true;
        }
      }

      if (hit) {
        Explode(parentObject);
      }

      // ensures the animation starts at the impact point
      if (gameObject.tag == "EnemyWeapon") {
        // plays a weapon clash sound when enemy throwables collide with the player weapon
        if (colliderTag == "Weapon") {
          Explode(parentObject);
          col.gameObject.GetComponent<Weapon>().DetermineProjectileSpawn(parentObject.GetComponent<Projectile>().key, col.ClosestPoint(transform.position), transform.parent.parent.parent.gameObject);
        }
      }

      // plays a weapon clash sound when two opposing throwables collide
      if (colliderTag == "EnemyWeapon") {
        ExplosionWithBlock();
      }
    }
  }

  // TODO: consider if this should be used for anything
  void Disappear(GameObject parentObject) {
    parentObject.GetComponent<Projectile>().StopProjectile();
  }

  void Explode(GameObject parentObject) {
    parentObject.GetComponent<Projectile>().StopProjectile();
    GameObject projectileExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), transform.position, Quaternion.identity, transform.parent);
    projectileExplosion.GetComponent<Explosion>().type = "projectile";
  }
}
