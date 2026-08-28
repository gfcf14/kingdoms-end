using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  private AudioSource audioSource;
  [NonSerialized] public List<GameObject> triggeredObjects = new List<GameObject>();

  void Start() {
    audioSource = transform.parent.gameObject.GetComponent<AudioSource>();
  }

  void Update() {}

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.gameObject.tag == "Enemy") {
      GameObject parentObject = col.gameObject.transform.parent.gameObject;
      float enemyTopBounds = col.transform.position.y + parentObject.GetComponent<SpriteRenderer>().bounds.size.y;

      if (Hero.instance.isDropKicking) {
        float heroBottomBounds = Hero.instance.transform.position.y;

        if (Mathf.Abs(heroBottomBounds - enemyTopBounds) <= 1) {
          parentObject.GetComponent<Enemy>().Trigger(GetComponent<CapsuleCollider2D>());
          Hero.instance.Jump(true);
        }
      }
    }

    if (col.gameObject.name.Contains("IceBlock")) {
      col.gameObject.GetComponent<IceBlock>().DestroyBlock();
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if (col.gameObject.tag == "Enemy") {
      triggeredObjects.Remove(col.gameObject);
    }
  }

  void PlayBlockSound() {
    // TODO: ensure this block can be different per weapon used (e.g. fist, kick, sword, rock club, etc.)
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.blockSounds, "basic"), transform.position);
  }

  public void DetermineProjectileSpawn(string projectileKey, Vector2 collisionPoint, GameObject fragmentParent) {
    if (Helpers.IsValueInArray(Constants.fragmentableProjectiles, projectileKey)) {
      // if the projectile is "grabbable":
      if (Hero.instance.isKicking) {
        PlayBlockSound();

        // if hero is kicking, he has a 33% chance of not destroying the object, but make it bounce away to pick later
        if (UnityEngine.Random.value <= 0.33f) {
          Vector2 fragmentOrigin = new (collisionPoint.x, collisionPoint.y + Helpers.GetItemDimensions(projectileKey).y);
          InGame.instance.InstantiateFragments(new FragmentOutcome() { key = projectileKey, count = 1 }, fragmentOrigin, fragmentParent, isProjectile: true);
        }
      } else if (Hero.instance.isPunching) {
        // if hero is punching, he has a 50% chance of grabbing the object
        if (UnityEngine.Random.value <= 0.5f) {
          InGame.instance.PlaySound(Helpers.GetOrException(Sounds.itemPickSounds, "rare"), transform.position);
          InGame.instance.PickItem(projectileKey);
        } else {
          PlayBlockSound();
        }
      }
    }
  }

  private string GetThrowableSound(string type, string key) {
    return type + (
      type.Contains("double") ? "-large" : (
        Helpers.IsValueInArray(Constants.smallThrowables, key) ? "-small" : "-middle"
      )
    );
  }

  public void PlaySound(string type, string key = "") {
    if (Settings.playSFX) {
      string soundKey = type.Contains("throwable") ? GetThrowableSound(type, key) : type;
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.attackSounds, soundKey));
    }
  }
}
