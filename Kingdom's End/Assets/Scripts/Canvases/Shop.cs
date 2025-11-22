using System;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
  [Header("Objects")]
  [SerializeField] GameObject contentContainer;
  [SerializeField] GameObject vendorName;
  [SerializeField] GameObject money;

  [SerializeField] public string vendor;
  [SerializeField] public string closingChat;
  [NonSerialized] public static string canvasStatus = "action";
  [NonSerialized] public bool isReady = false;
  [NonSerialized] private int moneyValue = 0;
  private AudioSource audioSource;
  void Start() {
    audioSource = GetComponent<AudioSource>();
  }

  void Update() {}

  public void StartAfterGrow() {
    isReady = true;
    contentContainer.SetActive(true);
    PopulateTopContent();
  }

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

  public void HideContent() {
    contentContainer.SetActive(false);
  }

  public void PopulateTopContent() {
    vendorName.GetComponent<Text>().text = $"Shopping with: {Helpers.GetVendorByKey(vendor)}";
    moneyValue = Hero.instance.gold;
    money.GetComponent<Text>().text = moneyValue.ToString();
  }
}
