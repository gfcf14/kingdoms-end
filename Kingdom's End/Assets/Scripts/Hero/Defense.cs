using System;
using UnityEngine;

public class Defense : MonoBehaviour {
  [NonSerialized] public bool isFacingLeft;

  void Start() {
    transform.localScale = new Vector3(isFacingLeft ? -1 : 1, 1, 1);
  }
  void Update() {}

  public void DestroyDefense() {
    Destroy(gameObject);
  }

  public void PlayBlockSound() {
    InGame.instance.PlaySound(Helpers.GetOrException(Sounds.blockSounds, "basic"), transform.position);
  }
}
