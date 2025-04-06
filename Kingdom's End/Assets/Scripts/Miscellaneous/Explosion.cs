using UnityEngine;

public class Explosion : MonoBehaviour {
  [SerializeField] public string type;
  [SerializeField] public int damage;

  [SerializeField] public bool hasDamaged;

  void Start() {
    if (!Helpers.IsValueInArray(Constants.explosionsWithColliders, type)) {
      Destroy(GetComponent<CapsuleCollider2D>());
    }

    if (type == "arrow") {
      gameObject.tag = "Explosion";
    } else if (type == "damage") {
      gameObject.tag = "DamageExplosion";
    }

    GetComponent<Animator>().Play("explosion-" + type);
  }

  void Update() {}

  public void PlayExplosion() {
    // TODO: consider if a explosion sound needs to be played for projectiles
    if (type != "projectile") {
      InGame.instance.PlaySound(Helpers.GetOrException(Sounds.explosionSounds, type), transform.position);
    }
  }

  public void DestroyExplosion() {
    GameObject objectToDestroy = type == "projectile" ? transform.parent.gameObject : gameObject;
    Destroy(objectToDestroy);
  }
}
