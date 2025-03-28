
using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
  [SerializeField] public Vector2 targetPoint;
  [SerializeField] public string key;
  [SerializeField] public bool fromFacingLeft;

  [NonSerialized] public int directionFactor = 0;
  // TODO: consider if this value should be modified depending on enemy level
  [NonSerialized] float speed = 10f;
  [NonSerialized] SpriteRenderer projectileSprite;
  [NonSerialized] Rigidbody2D body;
  [NonSerialized] CapsuleCollider2D projectileCollider;

  // TODO: investigate how NOT to depend on this variable for damage
  [NonSerialized] public float criticalRate = 0;

  void Start() {
    projectileSprite = GetComponent<SpriteRenderer>();
    projectileSprite.sprite = Helpers.GetOrException(Sprites.projectileSprites, key);

    projectileCollider = transform.Find("ProjectileCollider").GetComponent<CapsuleCollider2D>();
    projectileCollider.size = Helpers.GetOrException(Objects.projectileColliderSpecs, key);

    body = GetComponent<Rigidbody2D>();
    body.gravityScale = 0;

    Vector2 direction = (targetPoint - (Vector2) transform.position).normalized;
    body.velocity = direction * speed;

    directionFactor = fromFacingLeft ? 1 : -1;

    if (!Helpers.IsValueInArray(Constants.rotatingProjectiles, key)) {
      float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }

  void Update() {
    if (Helpers.IsValueInArray(Constants.rotatingProjectiles, key)) {
      transform.Rotate(0f, 0f, 1440f * Time.deltaTime * directionFactor);
    }
  }

  public void StopProjectile() {
    body.velocity = Vector2.zero;
    Destroy(projectileSprite);
  }
}
