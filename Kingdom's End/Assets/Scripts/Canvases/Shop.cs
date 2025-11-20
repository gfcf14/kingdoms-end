using System;
using UnityEngine;

public class Shop : MonoBehaviour {
  [SerializeField] public string vendor;
  [SerializeField] public string closingChat;
  [NonSerialized] public static string canvasStatus = "action";
  private AudioSource audioSource;
  void Start() {
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {}

  public void PerformBack() {
    PlayMenuSound("back");

    switch (canvasStatus) {
      default:
        Debug.Log("unknown canvas status: " + canvasStatus);
      break;
    }
  }

  public void PlayMenuSound(string sound) {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.menuSounds, sound));
    }
  }
}
