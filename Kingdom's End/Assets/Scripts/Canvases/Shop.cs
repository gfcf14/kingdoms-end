using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  [SerializeField] GameObject itemsContainer;
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
  private List<Item> activeShopList = new();
  private List<Item> shopList = new();
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

  public void AssignActiveShopList(string action) {
    if (action == "buy") {
      activeShopList = Helpers.GetOrException(GameData.vendorItems, vendor);
    } else { // sell
      activeShopList = Hero.instance.items;
      // TODO: use the hero's equipmentArray to subtract equipped items from the activeShopList
    }
  }

  public void ClearActiveShopList() {
    activeShopList = new List<Item>();
  }

  // sets navigation for shop list items when switching categories
  private IEnumerator SetupNavigationNextFrame() {
    yield return null; // allows to wait for shop list rebuild in UI

    GameObject[] containerChildren = itemsContainer.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
    int totalShopItems = containerChildren.Length;

    for (int i = 0; i < totalShopItems; i++) {
      Button btn = containerChildren[i].GetComponent<Button>();
      Navigation nav = new Navigation {
        mode = Navigation.Mode.Explicit,
        selectOnUp = containerChildren[i == 0 ? totalShopItems - 1 : i - 1].GetComponent<Button>(),
        selectOnDown = containerChildren[i == totalShopItems - 1 ? 0 : i + 1].GetComponent<Button>()
      };

      btn.navigation = nav;
    }

    eventSystem.SetSelectedGameObject(containerChildren[0]);
  }

  public void PopulateItemsContainer() {
    int itemIndex = 0;
    foreach (Item item in shopList) {
      RegularItem currRegItem = Helpers.GetOrException(Objects.regularItems, item.key);
      GameObject shopItem = Instantiate(Helpers.GetOrException(Objects.prefabs, "item-button"), Vector2.zero, Quaternion.identity);

      // sets current button properties
        shopItem.transform.SetParent(itemsContainer.transform);
        shopItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Constants.startShopItemY + (itemIndex * Constants.itemIncrementY * -1));
        shopItem.transform.localScale = Vector3.one;
        shopItem.transform.Find("Image").gameObject.GetComponent<Image>().sprite = currRegItem.thumbnail;
        shopItem.transform.Find("Text").gameObject.GetComponent<Text>().text = currRegItem.name;
        shopItem.transform.Find("Amount").gameObject.GetComponent<Text>().text = item.amount.ToString();

      // sets submit event trigger for current button
        EventTrigger eventTrigger = shopItem.GetComponent<EventTrigger>();
        EventTrigger.Entry submitEntry = new EventTrigger.Entry();
        submitEntry.eventID = EventTriggerType.Submit;
        submitEntry.callback.AddListener((data) => {
          PlayMenuSound("select");
        });

        eventTrigger.triggers.Add(submitEntry);

      itemIndex++;
    }

    StartCoroutine(SetupNavigationNextFrame());
  }

  public void ClearItemsContainer() {
    eventSystem.SetSelectedGameObject(null);

    foreach (Transform child in itemsContainer.transform) {
      Destroy(child.gameObject);
    }
  }

  public void PopulateShopList() {
    string[] currentCategoryTypes = Constants.categoryItemTypeArray[categoryIndex];

    foreach (Item item in activeShopList) {
      string currItemType = Helpers.GetOrException(Objects.regularItems, item.key).type;
      if (Helpers.IsValueInArray(currentCategoryTypes, currItemType)) {
        shopList.Add(item);
      }
    }
  }

  public void ClearShopList() {
    shopList.Clear();
  }

  public void SelectCategory() {
    itemCategories[categoryIndex].GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "highlighted");
    PopulateShopList();
    PopulateItemsContainer();
  }

  public void ClearCategory() {
    ClearItemsContainer();
    ClearShopList();
    itemCategories[categoryIndex].GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "normal");
  }

  public void ShowBodySections(string action) {
    AssignActiveShopList(action);
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
    // sets the categoryIndex to 0 so when selecting an action again, it starts from the first category
    categoryIndex = 0;
    // TODO: clear effects
    // TODO: clear description
    ClearActiveShopList();
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
