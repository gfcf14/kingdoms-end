using System;
using System.Collections.Generic;
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
  [Space(10)]

  [Header("Footer Legends")]
  [SerializeField] GameObject mainGamepadPanel;
  [SerializeField] GameObject mainKeysPanel;
  [SerializeField] GameObject mainPlaystationPanel;
  [SerializeField] GameObject mainXboxPanel;
  [Space(10)]

  [Header("Properties")]
  [SerializeField] public string vendor;
  [SerializeField] public string closingChat;
  [NonSerialized] public static string canvasStatus = "action";
  [NonSerialized] public bool isReady = false;
  [NonSerialized] bool hasGamepad = false;
  [NonSerialized] private int moneyValue = 0;
  private AudioSource audioSource;
  private GameObject previouslyFocusedButton = null;
  void Start() {
    audioSource = GetComponent<AudioSource>();
    eventSystem = EventSystem.current;
  }

  void Update() {
    CheckIfGamepad();
  }

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

  void ShowXboxOptions() {
    mainKeysPanel.SetActive(false);
    mainGamepadPanel.SetActive(false);
    mainPlaystationPanel.SetActive(false);
    mainXboxPanel.SetActive(true);
  }

  void ShowPlaystationOptions() {
    mainKeysPanel.SetActive(false);
    mainGamepadPanel.SetActive(false);
    mainXboxPanel.SetActive(false);
    mainPlaystationPanel.SetActive(true);
  }

  void ShowGamePadOptions() {
    mainKeysPanel.SetActive(false);
    mainXboxPanel.SetActive(false);
    mainPlaystationPanel.SetActive(false);
    mainGamepadPanel.SetActive(true);
  }

  void ShowKeyboardOptions() {
    mainGamepadPanel.SetActive(false);
    mainXboxPanel.SetActive(false);
    mainPlaystationPanel.SetActive(false);
    mainKeysPanel.SetActive(true);
  }

  void CheckIfGamepad() {
    List<string> validGamepads = new List<String>();
    foreach (string s in Input.GetJoystickNames()) {
      if (s != "") {
        validGamepads.Add(s);
      }
    }
    hasGamepad = validGamepads.Count > 0;

    if (hasGamepad && Constants.preferredInput == "gamepad") {
      var currentGamepad = UserInput.GetActiveGamepadKey();
      if (currentGamepad == null) {
        currentGamepad = "usb gamepad";
      }

      if (currentGamepad == "xbox" && !mainXboxPanel.activeInHierarchy) {
        ShowXboxOptions();
      } else if (currentGamepad == "playstation" && !mainPlaystationPanel.activeInHierarchy) {
        ShowPlaystationOptions();
      } else if (currentGamepad == "usb gamepad" && !mainGamepadPanel.activeInHierarchy) {
        ShowGamePadOptions();
      }
    } else if ((!hasGamepad || Constants.preferredInput == "keyboard") && !mainKeysPanel.activeInHierarchy) {
      ShowKeyboardOptions();
    }
  }
}
