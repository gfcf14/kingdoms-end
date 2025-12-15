using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThisOtherThing.UI.Shapes;
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
  [SerializeField] GameObject comparisonContainer;
  [SerializeField] GameObject[] comparisonGroups;
  [SerializeField] GameObject saleRectangle;
  [SerializeField] GameObject priceLabel;
  [SerializeField] GameObject itemPrice;
  [SerializeField] GameObject proceedPrompt;
  [SerializeField] GameObject buttonUseYes;
  [SerializeField] GameObject buttonUseNo;

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
  private GameObject previouslyFocusedItem = null;
  private List<GameObject> itemCategories = new();
  private List<GameObject> shopItemButtons = new();
  private List<Item> activeShopList = new();
  private List<Item> shopList = new();
  private int previousItemIndex = -1;
  private bool transactionHappened = false;
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
        if (isReady && currentSelectedItem != "" && Helpers.IsValueInArray(Constants.ShopActionButtonNames, currentActionSelected)) {
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

  void SetComparisonInfo(RegularItem currentItem, int categoryIndex) {
    string[] comparisonElements = Hero.instance.shopComparisonArray[categoryIndex];

    if (comparisonElements.Length == 0) {
      comparisonContainer.SetActive(false);
      return;
    }

    if (comparisonElements.Length == 1) {
      comparisonGroups[1].SetActive(false);
    }

    int i = 0;
    foreach(string equippedItem in comparisonElements) {
      bool isUnequipped = equippedItem == "";
      GameObject currentComparisonGroup = comparisonGroups[i];
      RegularItem currentEquippedItem = isUnequipped ? null : Helpers.GetOrException(Objects.regularItems, equippedItem);

      currentComparisonGroup.transform.Find("CurrentItem").GetComponent<Image>().sprite = isUnequipped ? Sprites.unequippedIcons[categoryIndex][i] : currentEquippedItem.thumbnail;
      currentComparisonGroup.transform.Find("NewItem").GetComponent<Image>().sprite = currentItem.thumbnail;

      // intends to get all children and use them in an array
      GameObject[] singleEffects = currentComparisonGroup.transform.Find("EffectsContainer").GetComponentsInChildren<Transform>(includeInactive: true).Where(t => t.name == "SingleEffect").Select(t => t.gameObject).ToArray();
      int usedEffectCounter = 0;

      // loops through each comparison check. If found on both, get the difference and modify corresponding single effect to display
      for (int j = 0; j < Constants.comparisonChecks.Length; j++) {
        string currentCheck = Constants.comparisonChecks[j];

        float? currentComparisonValue = isUnequipped ? 0 : currentEquippedItem.GetEffectValue(key: currentCheck);
        float? newComparisonValue = currentItem.GetEffectValue(key: currentCheck);

        // if both properties are null, then there isn't anything to compare, so this check can be skipped
        if ((isUnequipped && newComparisonValue == null) || (currentComparisonValue == null && newComparisonValue == null)) {
          continue;
        } else {
          GameObject currentEffect = singleEffects[usedEffectCounter];
          float difference = (newComparisonValue == null ? 0 : newComparisonValue.Value) - (currentComparisonValue == null ? 0 : currentComparisonValue.Value);
          string text = $"{(difference > 0 ? "+" : "")}{(Helpers.IsValueInArray(Constants.decimalComparisons, currentCheck) ? Helpers.TwoDecimalPlaces(difference * 100, ignoreWhenWhole: true) + " %" : difference.ToString())}";

          Color color = Helpers.GetOrException(Colors.uiColors, "white");
          if (difference != 0) {
            color = Helpers.GetOrException(Colors.uiColors, difference < 0 ? "red" : "green");
          }

          currentEffect.transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.comparisonStatIcons[j];
          Text effectText = currentEffect.transform.Find("EffectText").GetComponent<Text>();
          effectText.text = text;
          effectText.color = color;

          usedEffectCounter++;
        }
      }

      // if usedEffectCounter remained at 0, then there weren't any effects to compare, so everything should hide
      if (usedEffectCounter > 0) {
        for (int j = 0; j < singleEffects.Length; j++) {
          // display only the used effects
          singleEffects[j].SetActive(j < usedEffectCounter);
        }
      }

      currentComparisonGroup.SetActive(true);
      i++;
    }

    comparisonContainer.SetActive(true);
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

  void ToggleSaleRectangle(bool canAfford) {
    Color displayColor = canAfford ? Color.white : new Color(1, 1, 1, 0.5f);
    var rect = saleRectangle.GetComponent<Rectangle>();
    var shape = rect.ShapeProperties;

    shape.OutlineColor = displayColor;
    rect.ShapeProperties = shape;
    rect.SetAllDirty();

    priceLabel.GetComponent<Text>().color = displayColor;
    itemPrice.GetComponent<Text>().color = displayColor;
    proceedPrompt.GetComponent<Text>().color = displayColor;
    buttonUseYes.transform.Find("Text").GetComponent<Text>().color = displayColor;
    buttonUseNo.transform.Find("Text").GetComponent<Text>().color = displayColor;
  }

  void SetItemInfo(int itemIndex) {
    RegularItem currentItem = Helpers.GetOrException(Objects.regularItems, shopList.ElementAt(itemIndex).key);
    itemName.GetComponent<Text>().text = currentItem.name;
    itemPrice.GetComponent<Text>().text = currentItem.price.ToString();
    itemDescription.GetComponent<Text>().text = currentItem.description;
    itemImage.GetComponent<Image>().sprite = currentItem.image;

    SetEffectsInfo(currentItem);

    // only show comparison info when buying, no use showing it when selling since player can compare items in pause menu
    if (canvasStatus == "buy") {
      ToggleSaleRectangle(canAfford: currentItem.price < moneyValue);
      SetComparisonInfo(currentItem, categoryIndex);
    }
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
            if (!transactionHappened) {
              PlayMenuSound("move");
            } else {
              transactionHappened = false;
            }
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
      case "buy_proceed":
      case "sell_proceed":
        GoBackToItemSelect();
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
    isReady = false;
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

    // sell
    } else {
      activeShopList = Hero.instance.items.Select(item => new Item(item.key, item.amount)).ToList();

      // this allows to skip the bodyEquipment as body equipment is not sellable and the arm2 equipment if equipping a two-handed weapon
      string[] removeArray = Hero.instance.equipmentArray.Skip(Hero.arm1Equipment == Hero.arm2Equipment ? 2 : 1).ToArray();

      foreach(string removeItem in removeArray) {
        // if item is not equipped, skip
        if (removeItem != "") {
          Item itemToRemove = activeShopList.FirstOrDefault(i => i.key == removeItem);

          // removes item corresponding to what the player has equipped
          if (itemToRemove.amount > 1) {
            itemToRemove.amount -= 1;
          } else {
            activeShopList.Remove(itemToRemove);
          }
        }
      }
    }
  }

  public void ClearActiveShopList() {
    activeShopList = new List<Item>();
  }

  // sets navigation for shop list items when switching categories
  private IEnumerator SetupNavigationNextFrame(int specificItemIndex = 0) {
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

    eventSystem.SetSelectedGameObject(containerChildren[specificItemIndex]);
    // sets the item info as soon as category is changed
    SetItemInfo(specificItemIndex);
  }

  void UpdateDisplays(bool removalHappened = false) {
    moneyValue = Hero.instance.gold;
    money.GetComponent<Text>().text = moneyValue.ToString();

    ClearItemsContainer();
    ClearShopList();
    PopulateShopList();
    PopulateItemsContainer(removalHappened ? 0 : previousItemIndex);
  }

  public void ProceedToPrompt() {
    PlayMenuSound("select");
    canvasStatus = $"{canvasStatus}_proceed";
    previouslyFocusedItem = eventSystem.currentSelectedGameObject;
    eventSystem.SetSelectedGameObject(buttonUseYes);
  }

  public void GoBackToItemSelect() {
    eventSystem.SetSelectedGameObject(previouslyFocusedItem);
    previouslyFocusedItem = null;
    canvasStatus = canvasStatus.Replace("_proceed", "");
    if (transactionHappened) {
      InGame.instance.PlaySound(Helpers.GetOrException(Sounds.itemPickSounds, "money"), transform.position);
    } else {
      PlayMenuSound("back");
    }
  }

  public void ProceedWithTransaction() {
    bool removalHappened = false;
    string currentItemKey = previouslyFocusedItem.GetComponent<ItemButton>().key;
    RegularItem itemToProceedWith = Helpers.GetOrException(Objects.regularItems, currentItemKey);
    // TODO: implement a function to add/subtract gold! This prop should probably not be public
    Hero.instance.gold += itemToProceedWith.price * (canvasStatus == "buy_proceed" ? -1 : 1);

    Item transactionItem = activeShopList.FirstOrDefault(i => i.key == currentItemKey);
    List<Item> receivingList = canvasStatus == "buy_proceed" ? Hero.instance.items : Helpers.GetOrException(GameData.vendorItems, vendor);

    // updates the active shop list item amount, or removes it if only one left
    if (transactionItem.amount > 1) {
      transactionItem.amount -= 1;
    } else {
      activeShopList.Remove(transactionItem);
      removalHappened = true;
    }

    // when selling, the item should also be removed from the hero's items
    if (canvasStatus == "sell_proceed") {
      Item itemToRemove = Hero.instance.items.FirstOrDefault(i => i.key == currentItemKey);

      if (itemToRemove.amount > 1) {
        itemToRemove.amount -= 1;
      } else {
        Hero.instance.items.Remove(itemToRemove);
      }
    }

    // adds the item to the receiving list or increments its amount if present
    Item newItem = receivingList.FirstOrDefault(i => i.key == currentItemKey);
    if (newItem != null) {
      newItem.amount += 1;
    } else {
      receivingList.Add(new Item(currentItemKey, 1));
    }

    transactionHappened = true;

    UpdateDisplays(removalHappened);
    GoBackToItemSelect();
  }

  public void CancelTransaction() {
    GoBackToItemSelect();
  }

  public void CheckProceed() {
    if (canvasStatus == "sell") {
      ProceedToPrompt();

    // for buy, first the player has to have enough money
    } else {
      RegularItem itemToProceedWith = Helpers.GetOrException(Objects.regularItems, eventSystem.currentSelectedGameObject.GetComponent<ItemButton>().key);

      if (moneyValue >= itemToProceedWith.price) {
        ProceedToPrompt();
      }
    }
  }

  public void PopulateItemsContainer(int specificItemIndex = 0) {
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

      // sets current button action on press
        shopItem.GetComponent<Button>().onClick.AddListener(CheckProceed);

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

    StartCoroutine(SetupNavigationNextFrame(specificItemIndex));
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
    itemCategories[categoryIndex].GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "unselected");
  }

  public void ShowComparisonInfo() {
    proceedPrompt.GetComponent<Text>().text = $"{canvasStatus.ToUpper()}?";
    comparisonContainer.SetActive(canvasStatus == "buy");
  }

  public void HideComparisonInfo() {
    proceedPrompt.GetComponent<Text>().text = "";
  }

  // TODO: consider a similar process for the other images used in this canvas. Maybe consider an hourglass image to symbolize loading?
  void HideEffectContainers() {
    foreach (GameObject compGroup in comparisonGroups) {
      foreach (Transform child in compGroup.transform.Find("EffectsContainer")) {
        child.gameObject.SetActive(false);
      }
    }
  }

  public void ShowBodySections(string action) {
    AssignActiveShopList(action);
    SelectCategory();

    sectionCategories.SetActive(true);
    sectionItemList.SetActive(true);
    sectionDescription.SetActive(true);

    HideEffectContainers();

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

  public void HighlightActionButton(GameObject buttonPressed) {
    buttonPressed.GetComponent<Button>().enabled = false;
    buttonPressed.transform.Find("BackgroundImage").GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "highlighted");
  }

  public void BlurActionButton(GameObject buttonReturningTo) {
    buttonReturningTo.transform.Find("BackgroundImage").GetComponent<Image>().color = Helpers.GetOrException(Colors.shopButtonColors, "unselected");
    buttonReturningTo.GetComponent<Button>().enabled = true;
  }

  public void PopulateShopLists(bool isVendor) {
    PlayMenuSound("select");
    canvasStatus = isVendor ? "buy" : "sell";
    ShowComparisonInfo();
    mainPrompt.SetActive(false);
    previouslyFocusedButton = eventSystem.currentSelectedGameObject;
    HighlightActionButton(previouslyFocusedButton);
    ShowBodySections(action: canvasStatus);
  }

  public void GoBackToActionSelect() {
    HideBodySections();
    BlurActionButton(previouslyFocusedButton);
    eventSystem.SetSelectedGameObject(previouslyFocusedButton);
    mainPrompt.SetActive(true);
    HideComparisonInfo();
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
