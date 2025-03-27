using UnityEngine;

public class Bow : MonoBehaviour {
  AudioSource audioSource;
  void Start() {
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {}

  public void PlaySound(string projectileEquipment) {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.attackSounds, "bow"));
    }
  }
}
