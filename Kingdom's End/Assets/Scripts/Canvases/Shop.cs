using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
  [SerializeField] GameObject contentContainer;
  [SerializeField] public static EventSystem eventSystem;

  [Header("Header")]
  [SerializeField] GameObject vendorName;
  [SerializeField] GameObject money;
  [Space(10)]

  [Header("Section - Action")]
  [SerializeField] GameObject shopFirstSelected;
  [SerializeField] GameObject mainPrompt;
  [Space(10)]

  [Header("Section - Categories")]
  [Space(10)]
  [SerializeField] GameObject sectionCategories;
  [SerializeField] GameObject categoryWeapons;
  [SerializeField] GameObject categoryThrowables;
  [SerializeField] GameObject categoryNecklaces;
  [SerializeField] GameObject categoryBraces;
  [SerializeField] GameObject categoryRings;
  [SerializeField] GameObject categoryFood;
  [SerializeField] GameObject categoryPotions;
  [SerializeField] GameObject categoryMiscellaneous;

  [Header("Section - Item List")]
  [Space(10)]
  [SerializeField] GameObject sectionItemList;

  [Header("Section - Description")]
  [Space(10)]
  [SerializeField] GameObject sectionDescription;

  [Header("Section - Effects")]
  [Space(10)]
  [SerializeField] GameObject sectionEffects;

  [Header("Footer")]
  [SerializeField] GameObject mainGamepadPanel;
  [SerializeField] GameObject mainKeysPanel;
  [SerializeField] GameObject mainPlaystationPanel;
  [SerializeField] GameObject mainXboxPanel;
  [Space(10)]

  [Header("Properties")]
  [SerializeField] public string vendor;
  [SerializeField] public string closingChat;
  [SerializeField] public int categoryIndex = 0;
  [SerializeField] public static string canvasStatus = "action";
  [NonSerialized] public bool isReady = false;
  [NonSerialized] bool hasGamepad = false;
  [NonSerialized] private int moneyValue = 0;
  private AudioSource audioSource;
  private GameObject previouslyFocusedButton = null;
  private List<GameObject> itemCategories = new();
  public int totalCategories = 0;

  void Start() {
    audioSource = GetComponent<AudioSource>();
    eventSystem = EventSystem.current;
    itemCategories = new List<GameObject>() { categoryWeapons, categoryThrowables, categoryNecklaces, categoryBraces, categoryRings, categoryFood, categoryPotions, categoryMiscellaneous };
    totalCategories = itemCategories.Count;
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

  public void SelectCategory() {
    itemCategories[categoryIndex].GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "highlighted");
  }

  public void ClearCategory() {
    itemCategories[categoryIndex].GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "normal");
  }

  public void ShowBodySections(string action) {
    // TODO: populate item lists here, based on action (buy or sell)
    // TODO: populate item UI container here, given the category index and select first item
    // TODO: show description info based on first item selected
    // TODO: show effects based on first item selected
    SelectCategory();

    sectionCategories.SetActive(true);
    sectionItemList.SetActive(true);
    sectionDescription.SetActive(true);
    sectionEffects.SetActive(true);
  }

  public void HideBodySections() {
    sectionEffects.SetActive(false);
    sectionDescription.SetActive(false);
    sectionItemList.SetActive(false);
    sectionCategories.SetActive(false);

    ClearCategory();
    // TODO: clear effects
    // TODO: clear description
    // TODO: clear item UI container, set category index to 0
    // TODO: clear item lists
  }

  public void PopulateShopLists(bool isVendor) {
    canvasStatus = isVendor ? "buy" : "sell";
    mainPrompt.SetActive(false);
    previouslyFocusedButton = eventSystem.currentSelectedGameObject;
    Debug.Log($"Shop lists to use vendor items? {isVendor}");
    ShowBodySections(action: canvasStatus);
  }

  public void GoBackToActionSelect() {
    HideBodySections();
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
