using UnityEngine;

public class EnemyBomb : MonoBehaviour {
  [SerializeField] public int damage;
  [SerializeField] public bool wasKickedBack = false;
  [SerializeField] public Enemy dropper;
  [SerializeField] public Hero hero;
  [System.NonSerialized] private Rigidbody2D body;
  void Start() {
    body = GetComponent<Rigidbody2D>();
  }
  void Update() {}

  void OnCollisionEnter2D(Collision2D col) {
    if (Helpers.IsValueInArray(Constants.enemyBombTriggerTags, col.gameObject.tag)) {
      Explode(col.collider.ClosestPoint(transform.position));
    } else if (Helpers.IsValueInArray(Constants.enemyBombBounceTags, col.gameObject.tag)) {
      if (hero.isKicking) {
        Bounce();
      } else {
        Explode(col.collider.ClosestPoint(transform.position));
      }
    } else {
      Debug.LogFormat("colliding with {0}({1})", col.gameObject.name, col.gameObject.tag);
    }
  }

  void OnTriggerEnter2D(Collider2D col) {
    if (wasKickedBack && col.gameObject.CompareTag("Enemy")) {
      Explode(col.ClosestPoint(transform.position));
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
    GameObject.Find("InGame").gameObject.GetComponent<InGame>().PlaySound(Helpers.GetOrException(Helpers.GetOrException(Sounds.impactSounds, "kick"), "normal"), transform.position);

    body.velocity = new Vector2(0, 8f); // Mathf.Abs(body.velocity.y));
    body.gravityScale = 0;
    wasKickedBack = true;
    dropper.bombReturned = true;
  }
}
