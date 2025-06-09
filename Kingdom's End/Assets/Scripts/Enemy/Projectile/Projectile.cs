
using System;
using System.Collections.Generic;
using System.Linq;
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

    Dictionary<string, Sprite> throwableAndProjectileSprites = Sprites.projectileSprites.Concat(Sprites.throwableSprites).ToDictionary(x => x.Key, x => x.Value);
    projectileSprite.sprite = Helpers.GetOrException(throwableAndProjectileSprites, key);

    projectileCollider = transform.Find("ProjectileCollider").GetComponent<CapsuleCollider2D>();

    Dictionary<string, ThrowableSpecs> throwableAndProjectileSpecs = Objects.throwableSpecs.Concat(Objects.projectileSpecs).ToDictionary(x => x.Key, x => x.Value);
    ValuePair projectileSize = Helpers.GetOrException(throwableAndProjectileSpecs, key).colliderSize;
    projectileCollider.size = new Vector2(projectileSize.x, projectileSize.y);

    body = GetComponent<Rigidbody2D>();
    body.gravityScale = 0;

    Vector2 direction = (targetPoint - (Vector2) transform.position).normalized;
    body.velocity = direction * speed;

    directionFactor = fromFacingLeft ? 1 : -1;

    if (fromFacingLeft) {
      transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * -1, transform.localScale.z);
    }

    if (!Helpers.IsValueInArray(Constants.rotatingProjectiles.Concat(Constants.rotatingThrowables).ToArray(), key)) {
      float angle = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle);
    }
  }

  void Update() {
    if (Helpers.IsValueInArray(Constants.rotatingProjectiles.Concat(Constants.rotatingThrowables).ToArray(), key)) {
      transform.Rotate(0f, 0f, 1440f * Time.deltaTime * directionFactor);
    }
  }

  public void StopProjectile() {
    body.velocity = Vector2.zero;
    Destroy(projectileSprite);
  }
}
