using UnityEngine;

public class ProjectileCollider : MonoBehaviour {
  void Start() {}

  void Update() {}

  void ExplosionWithBlock() {
    Explode(transform.parent.gameObject);
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
          string projectileKey = parentObject.GetComponent<Projectile>().key;

          if (Helpers.IsValueInArray(Constants.fragmentableProjectiles, projectileKey)) {
            // if the projectile is "grabbable":
            if (Hero.instance.isKicking) {
              // if hero is kicking, he has a 33% chance of not destroying the object, but make it bounce away to pick later
              if (Random.value <= 0.33f) {
                Vector2 fragmentOrigin = new Vector2(col.ClosestPoint(transform.position).x, col.ClosestPoint(transform.position).y + Helpers.GetItemDimensions(projectileKey).y);
                GameObject fragmentParent = transform.parent.parent.parent.gameObject;

                Disappear(parentObject);
                InGame.instance.InstantiateFragments(new FragmentOutcome() { key = projectileKey, count = 1 }, fragmentOrigin, fragmentParent, isProjectile: true);
              }
            } else if (Hero.instance.isPunching) {
              // if hero is punching, he has a 50% chance of grabbing the object
              if (Random.value <= 0.5f) {
                Disappear(parentObject);
                InGame.instance.PickItem(projectileKey);
              }
            } else {
              ExplosionWithBlock();
            }
          } else {
            ExplosionWithBlock();
          }
        }
      }

      // plays a weapon clash sound when two opposing throwables collide
      if (colliderTag == "EnemyWeapon") {
        ExplosionWithBlock();
      }
    }
  }

  void Disappear(GameObject parentObject) {
    parentObject.GetComponent<Projectile>().StopProjectile();
  }

  void Explode(GameObject parentObject) {
    parentObject.GetComponent<Projectile>().StopProjectile();
    GameObject projectileExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), transform.position, Quaternion.identity, transform.parent);
    projectileExplosion.GetComponent<Explosion>().type = "projectile";
  }
}
