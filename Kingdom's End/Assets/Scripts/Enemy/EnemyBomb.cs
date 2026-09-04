using System;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {
  [SerializeField] public int damage;

  // TODO: this variable causes a bug where if the bomb was kicked back while the bomb explodes, the bouncer would still stop dropping them. Modify logic to either:
  // A) add isExploding and avoid the bounce logic
  // B) modify attack frequency of bouncers by checking how many bombs are on screen

  [SerializeField] public bool wasKickedBack = false;
  [SerializeField] public Enemy dropper;
  [NonSerialized] private Rigidbody2D body;
  void Start() {
    body = GetComponent<Rigidbody2D>();
  }
  void Update() {}

  void BounceLogic(Collider2D col) {
    if (col.gameObject.tag == "Hero") {
      Explode(col.ClosestPoint(transform.position));
    } else if (Helpers.IsValueInArray(Constants.enemyBombBounceTags, col.gameObject.tag)) {
      if (Hero.instance.isKicking && Hero.instance.projectileCheckScript.ProjectileNearby()) {
        Bounce();
      } else {
        Explode(col.ClosestPoint(transform.position));
      }
    } else {
      Debug.Log($"colliding with {col.gameObject.name}({col.gameObject.tag})");
    }
  }

  void OnCollisionEnter2D(Collision2D col) {
    if (Helpers.IsValueInArray(Constants.enemyBombTriggerTags, col.gameObject.tag)) {
      Explode(col.collider.ClosestPoint(transform.position));
    } else {
      BounceLogic(col.collider);
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (wasKickedBack && col.gameObject.CompareTag("Enemy")) {
      Explode(col.ClosestPoint(transform.position));
    } else {
      BounceLogic(col);
    }
  }

  void Explode(Vector2 position) {
    GameObject damageExplosion = Instantiate(Helpers.GetOrException(Objects.prefabs, "explosion"), position, Quaternion.identity);
    Explosion explosionScript = damageExplosion.GetComponent<Explosion>();

    explosionScript.type = "damage";
    explosionScript.damage = damage;

    Destroy(gameObject);
  }

  void Bounce() {
   InGame.instance.PlaySound(Helpers.GetOrException(Helpers.GetOrException(Sounds.impactSounds, "kick"), "normal"), transform.position);

    body.linearVelocity = new Vector2(0, 8f); // Mathf.Abs(body.velocity.y));
    body.gravityScale = 0;
    wasKickedBack = true;
    dropper.bombReturned = true;
  }
}
