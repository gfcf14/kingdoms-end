using UnityEngine;

public class PauseEffect : MonoBehaviour {
  private AudioSource audioSource;
  void Start() {
    audioSource = GetComponent<AudioSource>();
  }
  void Update() {}

  public void Disable() {
    gameObject.SetActive(false);
  }

  public void PlaySound() {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.menuSounds, "use"));
    }
  }
}
