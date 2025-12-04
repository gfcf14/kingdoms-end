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
  [Space(10)]
  [SerializeField] GameObject sectionItemList;
  [SerializeField] GameObject itemsContainer;

  [Header("Section - Description")]
  [Space(10)]
  [SerializeField] GameObject sectionDescription;
  [SerializeField] GameObject itemName;
  [SerializeField] GameObject itemPrice;
  [SerializeField] GameObject itemImage;
  [SerializeField] GameObject itemDescription;
  [SerializeField] GameObject itemEffectsPanel;
  [SerializeField] GameObject itemEffectsGroupPanel;
  [SerializeField] GameObject itemEffectsStatusHealLabel;
  [SerializeField] GameObject itemEffectsAddsPanel;
  [SerializeField] GameObject itemEffectsRemovesPanel;
  [SerializeField] GameObject itemEffectsTimeLabel;

  [Header("Section - Comparison")]
  [Space(10)]
  [SerializeField] GameObject sectionComparison;

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
  // tracks each effect in the effect panel
  [NonSerialized] List<GameObject> effectsList = new List<GameObject>();
  // tracks each magic resistance in the adds list
  [NonSerialized] List<GameObject> addsList = new List<GameObject>();
  // tracks each magic resistance in the removes list
  [NonSerialized] List<GameObject> removesList = new List<GameObject>();
  private AudioSource audioSource;
  private GameObject previouslyFocusedButton = null;
  private List<GameObject> itemCategories = new();
  private List<GameObject> shopItemButtons = new();
  private List<Item> activeShopList = new();
  private List<Item> shopList = new();
  private int previousItemIndex = -1;
  public int totalCategories = 0;
  public string currentActionSelected = "";
  public string currentItemKey = "";

  void Start() {
    audioSource = GetComponent<AudioSource>();
    eventSystem = EventSystem.current;
    itemCategories = new List<GameObject>() { categoryWeapons, categoryThrowables, categoryNecklaces, categoryBraces, categoryRings, categoryFood, categoryPotions, categoryMiscellaneous };
    totalCategories = itemCategories.Count;

    // adds all single effects to the list
    foreach (Transform currentChild in itemEffectsGroupPanel.transform) {
      if (currentChild.name == "SingleEffect") {
        effectsList.Add(currentChild.gameObject);
      }
    }

    // adds all magic resistances to the adds list
    foreach (Transform currentChild in itemEffectsAddsPanel.transform) {
      if (currentChild.name == "MagicResistance") {
        addsList.Add(currentChild.gameObject);
      }
    }

    // adds all magic resistances to the removes list
    foreach (Transform currentChild in itemEffectsRemovesPanel.transform) {
      if (currentChild.name == "MagicResistance") {
        addsList.Add(currentChild.gameObject);
      }
    }
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

  void CheckButtonChange() {
    if (canvasStatus == "action") {
      string currentSelectedItem = eventSystem.currentSelectedGameObject.name;

      if (currentSelectedItem != currentActionSelected) {
        // only when moving to choose a shop action will the move sound be played
        if (currentSelectedItem != "" && Helpers.IsValueInArray(Constants.ShopActionButtonNames, currentActionSelected)) {
          PlayMenuSound("move");
        }
        currentActionSelected = currentSelectedItem;
      }
    }
  }

  string GetEffectText(string key, object value) {
    bool isPercentage = Helpers.IsValueInArray(Constants.effectPercentageKeys, key);
    float numericValue = Convert.ToSingle(value);

    return $"{(numericValue >= 0 ? "+" : "")}{(isPercentage ? $"{Helpers.TwoDecimalPlaces(numericValue * 100, ignoreWhenWhole: true)}%" : numericValue.ToString())}";
  }

  void HideEffectsObjects() {
    foreach(GameObject child in effectsList) {
      child.SetActive(false);
    }

    itemEffectsStatusHealLabel.SetActive(false);

    foreach(GameObject child in addsList) {
      child.SetActive(false);
    }
    itemEffectsAddsPanel.SetActive(false);

    foreach(GameObject child in removesList) {
      child.SetActive(false);
    }
    itemEffectsRemovesPanel.SetActive(false);

    itemEffectsTimeLabel.SetActive(false);
  }

  void SetEffectsInfo(RegularItem currentItem) {
    HideEffectsObjects();

    if (currentItem.effects != null) {
      Effects itemEffects = currentItem.effects;
      List<EffectItem> activeEffects = new List<EffectItem>() {
        new EffectItem {spriteIndex = 0, key = "hp", value = itemEffects.hp},
        new EffectItem {spriteIndex = 0, key = "hpPercentage", value = itemEffects.hpPercentage},
        new EffectItem {spriteIndex = 1, key = "mp", value = itemEffects.mp},
        new EffectItem {spriteIndex = 1, key = "mpPercentage", value = itemEffects.mpPercentage},
        new EffectItem {spriteIndex = 6, key = "atk", value = itemEffects.atk},
        new EffectItem {spriteIndex = 7, key = "def", value = itemEffects.def},
        new EffectItem {spriteIndex = 10, key = "crit", value = itemEffects.crit},
        new EffectItem {spriteIndex = 11, key = "luck", value = itemEffects.luck},
      }.Where(e => e.value != null).ToList();

      for (int i = 0; i < activeEffects.Count; i++) {
        EffectItem currentEffect = activeEffects[i];

        GameObject currentEffectWidget = effectsList.ElementAt(i);
        currentEffectWidget.transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[currentEffect.spriteIndex];
        currentEffectWidget.transform.Find("EffectText").GetComponent<Text>().text = GetEffectText(currentEffect.key, currentEffect.value);
        currentEffectWidget.SetActive(true);
      }

      // if there are no effects then the container should hide
      if (activeEffects.Count == 0) {
        itemEffectsGroupPanel.SetActive(false);
      } else {
        itemEffectsGroupPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(335.66f, activeEffects.Count < 3 ? 31 : 62);
        itemEffectsGroupPanel.SetActive(true);
      }

      if (itemEffects.statusHeal != null) {
        string statusEffectsText = "Heals ";

        int i = 0;
        foreach (string currStatusHeal in itemEffects.statusHeal) {
          statusEffectsText += currStatusHeal + (i < itemEffects.statusHeal.Length - 1 ? ", " : "\n");
          i++;
        }

        itemEffectsStatusHealLabel.GetComponent<Text>().text = statusEffectsText;
        itemEffectsStatusHealLabel.SetActive(true);
      }

      if (itemEffects.magicResistances != null) {
        int addsElementCounter = 0;
        int removesElementCounter = 0;

        foreach (MagicResistance currMagicResistance in itemEffects.magicResistances) {
          if (currMagicResistance.type == "add") {
            addsList.ElementAt(addsElementCounter).GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.magicResistances, currMagicResistance.name.ToLower());
            addsList.ElementAt(addsElementCounter).SetActive(true);
            addsElementCounter++;
          } else if (currMagicResistance.type == "remove") {
            addsList.ElementAt(removesElementCounter).GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.magicResistances, currMagicResistance.name.ToLower());
            addsList.ElementAt(removesElementCounter).SetActive(true);
            removesElementCounter++;
          }
        }

        if (addsElementCounter > 0) {
          itemEffectsAddsPanel.SetActive(true);
        }

        if (removesElementCounter > 0) {
          itemEffectsRemovesPanel.SetActive(true);
        }
      }

      if (itemEffects.duration != null) {
        itemEffectsTimeLabel.GetComponent<Text>().text = $"{itemEffects.duration} {(itemEffects.duration == 1 ? "sec" : "secs")}";
        itemEffectsTimeLabel.SetActive(true);
      }

      itemEffectsPanel.SetActive(true);
    } else {
      itemEffectsPanel.SetActive(false);
    }
  }

  void SetItemInfo(int itemIndex) {
    RegularItem currentItem = Helpers.GetOrException(Objects.regularItems, shopList.ElementAt(itemIndex).key);
    itemName.GetComponent<Text>().text = currentItem.name;
    itemPrice.GetComponent<Text>().text = currentItem.price.ToString();
    itemDescription.GetComponent<Text>().text = currentItem.description;
    itemImage.GetComponent<Image>().sprite = currentItem.image;

    SetEffectsInfo(currentItem);
  }

  void UpdateItemView() {
    if ((canvasStatus == "buy" || canvasStatus == "sell") && shopList.Count > 0 && eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject?.GetComponent<ItemButton>()?.key != null) {
      GameObject selectedItemObject = eventSystem.currentSelectedGameObject;

      int currentItemIndex = shopItemButtons.IndexOf(selectedItemObject);
      if (currentItemIndex == -1) return; // to avoid the logic to occur right after action selection

      // movement sound play logic
        string currentSelectedItem = selectedItemObject.GetComponent<ItemButton>().key;

        if (currentSelectedItem != currentItemKey) {
          // if both variables have a value, then we didn't just enter the category, thus the move sound can safely be played
          if (currentSelectedItem != "" && currentItemKey != "") {
            PlayMenuSound("move");
          }
          currentItemKey = currentSelectedItem;
        }

      // container scroll logic
        int indexDifference = currentItemIndex - previousItemIndex;
        if (indexDifference == 0) return; // do nothing if the indices are the same, i.e. no movement

        RectTransform itemsContainerRect = itemsContainer.GetComponent<RectTransform>();
        int maxVisibleItems = Constants.maxShopItemContainerHeight;
        int totalShopItems = shopItemButtons.Count;
        float itemYIncrement = Constants.itemIncrementY;
        float startItemY = Constants.startShopItemY;

        // TODO: should this be limited to only when indices are not equal? (i.e. difference is 0)

        // going down
        if (indexDifference == 1) {
          if (currentItemIndex > maxVisibleItems - 1) {
            int movingItemLocation = (int)(startItemY - (itemYIncrement * (maxVisibleItems - 1)));
            int selectedItemLocation = (int)(shopItemButtons.ElementAt(currentItemIndex - 1).GetComponent<RectTransform>().anchoredPosition.y + itemsContainerRect.anchoredPosition.y);

            // to avoid moving the container up if the selected button is not at the bottom
            if (selectedItemLocation == movingItemLocation) {
              itemsContainerRect.anchoredPosition = new Vector2(itemsContainerRect.anchoredPosition.x, itemsContainerRect.anchoredPosition.y + itemYIncrement);
            }
          }

        // going up
        } else if (indexDifference == -1) {
          if (currentItemIndex <= (totalShopItems - maxVisibleItems - 1)) {
            int movingItemLocation = (int)(startItemY - (itemYIncrement * (totalShopItems - maxVisibleItems)));
            int selectedItemLocation = (int)(shopItemButtons.ElementAt(currentItemIndex).GetComponent<RectTransform>().anchoredPosition.y + (itemsContainerRect.anchoredPosition.y - ((totalShopItems + 1 - maxVisibleItems) * itemYIncrement)));

            // to avoid moving the container down if the selected button is not at the top
            if (selectedItemLocation == movingItemLocation) {
              itemsContainerRect.anchoredPosition = new Vector2(itemsContainerRect.anchoredPosition.x, itemsContainerRect.anchoredPosition.y - itemYIncrement);
            }
          }
        // for first to last
        } else if (indexDifference == totalShopItems - 1 && totalShopItems > maxVisibleItems) {
          itemsContainerRect.anchoredPosition = new Vector2(itemsContainerRect.anchoredPosition.x, (itemYIncrement * (totalShopItems - maxVisibleItems)));

        // from last to first
        } else if (indexDifference == -(totalShopItems - 1)) {
          itemsContainerRect.anchoredPosition = Vector2.zero;
        }

      previousItemIndex = currentItemIndex;
      SetItemInfo(currentItemIndex);
    }
  }

  void Update() {
    CheckIfGamepad();
    CheckButtonChange();
    UpdateItemView();
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
    // sets the item info as soon as category is changed
    SetItemInfo(0);
  }

  public void PopulateItemsContainer() {
    int itemIndex = 0;
    shopItemButtons.Clear();
    itemsContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

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
        shopItem.GetComponent<ItemButton>().key = item.key;

        // adds the current button to the list to be able to track them on movement
        shopItemButtons.Add(shopItem);

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
    SelectCategory();

    sectionCategories.SetActive(true);
    sectionItemList.SetActive(true);
    sectionDescription.SetActive(true);
    sectionComparison.SetActive(true);
  }

  public void HideBodySections() {
    sectionComparison.SetActive(false);
    sectionDescription.SetActive(false);

    // this is cleared so when we select an action we don't play the move sound
    currentItemKey = "";

    sectionItemList.SetActive(false);
    sectionCategories.SetActive(false);

    ClearCategory();
    // sets the categoryIndex to 0 so when selecting an action again, it starts from the first category
    categoryIndex = 0;
    ClearActiveShopList();
  }

  public void PopulateShopLists(bool isVendor) {
    PlayMenuSound("select");
    canvasStatus = isVendor ? "buy" : "sell";
    mainPrompt.SetActive(false);
    previouslyFocusedButton = eventSystem.currentSelectedGameObject;
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
}
