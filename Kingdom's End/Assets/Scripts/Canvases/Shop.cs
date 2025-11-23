using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
  [Header("Objects")]
  [SerializeField] GameObject contentContainer;
  [SerializeField] GameObject vendorName;
  [SerializeField] GameObject money;
  [SerializeField] GameObject shopFirstSelected;
  [SerializeField] GameObject mainPrompt;
  [SerializeField] public static EventSystem eventSystem;

  [Header("Properties")]
  [SerializeField] public string vendor;
  [SerializeField] public string closingChat;
  [NonSerialized] public static string canvasStatus = "action";
  [NonSerialized] public bool isReady = false;
  [NonSerialized] private int moneyValue = 0;
  private AudioSource audioSource;
  private GameObject previouslyFocusedButton = null;
  void Start() {
    audioSource = GetComponent<AudioSource>();
    eventSystem = EventSystem.current;
  }

  void Update() {}

  public void StartAfterGrow() {
    isReady = true;
    contentContainer.SetActive(true);
    PopulateTopContent();
    eventSystem.SetSelectedGameObject(shopFirstSelected);
  }

  public void PerformBack() {
    PlayMenuSound("back");

    switch (canvasStatus) {
      case "buy":
      case "sell":
        GoBackToActionSelect();
      break;
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

  public void PopulateShopLists(bool isVendor) {
    canvasStatus = isVendor ? "buy" : "sell";
    mainPrompt.SetActive(false);
    previouslyFocusedButton = eventSystem.currentSelectedGameObject;
    Debug.Log($"Shop lists to use vendor items? {isVendor}");
    // TODO: show the item lists
  }

  public void GoBackToActionSelect() {
    // TODO: hide the item lists
    eventSystem.SetSelectedGameObject(previouslyFocusedButton);
    mainPrompt.SetActive(true);
    canvasStatus = "action";
  }
}
