using System;
using UnityEngine;

public class SmashWave : MonoBehaviour {
  [NonSerialized] public float width;
  [NonSerialized] public int damage;
  [NonSerialized] public bool hitPlayer = false;

  void Start() {
    GetComponent<SpriteRenderer>().size = new Vector2(width, width * 0.8f);
  }

  void Update() {}

  public void DestroySmashWave() {
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Hero" && !hitPlayer) {
      hitPlayer = true;
      Hero heroInstance = col.gameObject.GetComponent<Hero>();
      // ensures the hero isn't damaged after being damaged
      if (!heroInstance.isInvulnerable) {
        heroInstance.ReceiveSmashWave(damage, col.ClosestPoint(transform.position));
      }
    }
  }

  public void PlaySmash() {
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.meleeSounds, "smash"), transform.position);
  }
}
