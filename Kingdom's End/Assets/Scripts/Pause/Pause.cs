using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class Pause : MonoBehaviour {
  // Canvases
  [Header("Canvases")]
  [SerializeField] GameObject mainCanvas;
  [SerializeField] GameObject itemsCanvas;
  [SerializeField] GameObject equipmentCanvas;
  [SerializeField] GameObject relicsCanvas;
  [SerializeField] GameObject projectileCanvas;
  [SerializeField] GameObject mapCanvas;
  [SerializeField] GameObject optionsCanvas;
  [SerializeField] GameObject controlsCanvas;
  [SerializeField] GameObject preferredInputCanvas;
  [SerializeField] GameObject inGameElementsCanvas;
  [SerializeField] GameObject soundsCanvas;
  [SerializeField] GameObject quitCanvas;
  [SerializeField] GameObject titleCanvas;
  [Space(10)]

  // Outer Canvases
  [Header("Outer Canvases")]
  [SerializeField] GameObject standardStatsCanvas;
  [SerializeField] GameObject minifiedStatsCanvas;
  [Space(10)]

  // Objects to select first upon reaching a canvas
  [Header("First Selections")]
  [SerializeField] GameObject pauseFirstSelected;
  [SerializeField] GameObject itemsButton;
  [SerializeField] GameObject equipmentButton;
  [SerializeField] GameObject equipmentFirstSelected;
  [SerializeField] GameObject relicsButton;
  [SerializeField] GameObject mapButton;
  [SerializeField] GameObject optionsButton;
  [SerializeField] GameObject optionsFirstSelected;
  [SerializeField] GameObject controlsButton;
  [SerializeField] GameObject controlsFirstSelected;
  [SerializeField] GameObject preferredInputButton;
  [SerializeField] GameObject preferredInputFirstSelected;
  [SerializeField] GameObject inGameElementsButton;
  [SerializeField] GameObject inGameElementsFirstSelected;

  [SerializeField] GameObject soundsButton;
  [SerializeField] GameObject soundsFirstSelected;
  [SerializeField] GameObject quitButton;
  [SerializeField] GameObject quitFirstSelected;
  [Space(10)]

  // Effects Objects
  [Header("Effects")]
  [SerializeField] GameObject effectsCurrentHP;
  [SerializeField] GameObject effectsTotalHP;
  [SerializeField] GameObject effectsCurrentMP;
  [SerializeField] GameObject effectsTotalMP;
  [SerializeField] GameObject effectSTR;
  [SerializeField] GameObject effectSTA;
  [SerializeField] GameObject effectCRIT;
  [SerializeField] GameObject effectLCK;

  // Item Objects
  [Header("Item Objects")]
  [SerializeField] GameObject itemsContainer;
  [SerializeField] GameObject itemImage;
  [SerializeField] GameObject itemName;
  [SerializeField] GameObject itemDescription;
  [SerializeField] GameObject itemEffectsPanel;
  [SerializeField] GameObject itemEffectsGroupPanel;
  [SerializeField] GameObject itemEffectsStatusHealLabel;
  [SerializeField] GameObject itemEffectsAddsPanel;
  [SerializeField] GameObject itemEffectsRemovesPanel;
  [SerializeField] GameObject itemEffectsTimeLabel;
  [SerializeField] GameObject itemUseRectangle;
  [SerializeField] GameObject itemUseYes;
  [Space(10)]

  // Equipment Objects
  [Header("Equipment Objects")]
  [SerializeField] GameObject equipmentContainer;
  [SerializeField] GameObject bodyButton;
  [SerializeField] GameObject arm1Button;
  [SerializeField] GameObject arm2Button;
  [SerializeField] GameObject neckButton;
  [SerializeField] GameObject armwear1Button;
  [SerializeField] GameObject armwear2Button;
  [SerializeField] GameObject ring1Button;
  [SerializeField] GameObject ring2Button;
  [SerializeField] GameObject EquippedSTRLabel;
  [SerializeField] GameObject EquippedSTALabel;
  [SerializeField] GameObject EquippedATK1Label;
  [SerializeField] GameObject EquippedATK2Label;
  [SerializeField] GameObject EquippedDEF1Label;
  [SerializeField] GameObject EquippedDEF2Label;
  [SerializeField] GameObject EquippedCRITLabel;
  [SerializeField] GameObject EquippedLUCKLabel;
  [SerializeField] GameObject EquippedResistancesContainer;
  [SerializeField] GameObject ProjectilesContainer;
  [Space(10)]

  // Relics Objects
  [Header("Relic Objects")]
  [SerializeField] GameObject relicsContainer;
  [SerializeField] GameObject relicImage;
  [SerializeField] GameObject relicName;
  [SerializeField] GameObject relicDescription;
  [Space(10)]

  // Control buttons
  [Header("Jump Controls")]
  [SerializeField] GameObject jumpButton;
  [SerializeField] GameObject jumpKeyboardImage;
  [SerializeField] GameObject jumpGamepadImage;
  [Space(10)]

  [Header("ATK1 Controls")]
  [SerializeField] GameObject atk1Button;
  [SerializeField] GameObject atk1KeyboardImage;
  [SerializeField] GameObject atk1GamepadImage;
  [Space(10)]

  [Header("ATK2 Controls")]
  [SerializeField] GameObject atk2Button;
  [SerializeField] GameObject atk2KeyboardImage;
  [SerializeField] GameObject atk2GamepadImage;
  [Space(10)]

  [Header("ACTION Controls")]
  [SerializeField] GameObject actionButton;
  [SerializeField] GameObject actionKeyboardImage;
  [SerializeField] GameObject actionGamepadImage;
  [Space(10)]

  // Awaiting Input Objects
  [Header("Awaiting Input")]
  [SerializeField] GameObject jumpAwaitLabel;
  [SerializeField] GameObject atk1AwaitLabel;
  [SerializeField] GameObject atk2AwaitLabel;
  [SerializeField] GameObject actionAwaitLabel;
  [SerializeField] GameObject resetButton;
  [SerializeField] GameObject resetRectangle;
  [SerializeField] GameObject resetYes;
  [Space(10)]

  // main footer legends
  [Header("Footer Legends")]
  [SerializeField] GameObject mainButtonPanel;
  [SerializeField] GameObject mainKeysPanel;
  [Space(10)]

  // dynamic hero objects
  [Header("Hero Objects")]
  [SerializeField] GameObject hero;
  [SerializeField] GameObject playerAvatar;
  [SerializeField] GameObject level;
  [SerializeField] GameObject currentHPObject;
  [SerializeField] GameObject totalHP;
  [SerializeField] GameObject currentMPObject;
  [SerializeField] GameObject totalMP;
  [SerializeField] GameObject statusObject;
  [SerializeField] GameObject expObject;
  [SerializeField] GameObject nextObject;
  [SerializeField] GameObject goldObject;
  [SerializeField] GameObject strObject;
  [SerializeField] GameObject staObject;
  [SerializeField] GameObject atk1Object;
  [SerializeField] GameObject atk2Object;
  [SerializeField] GameObject def1Object;
  [SerializeField] GameObject def2Object;
  [SerializeField] GameObject luck;
  [SerializeField] GameObject critical;
  [SerializeField] GameObject locationObject;
  [Space(10)]

  // magic resistances
  [Header("Magic Resistances")]
  [SerializeField] GameObject resistance1Object;
  [SerializeField] GameObject resistance2Object;
  [SerializeField] GameObject resistance3Object;
  [SerializeField] GameObject resistance4Object;
  [SerializeField] GameObject resistance5Object;
  [SerializeField] GameObject resistance6Object;
  [SerializeField] GameObject resistance7Object;
  [SerializeField] GameObject resistance8Object;
  [SerializeField] GameObject magicEmptyObject;
  [Space(10)]

  // UI
  [Header("UI Elements")]
  [SerializeField] GameObject radioDontShowStatuses;
  [SerializeField] GameObject radioShowStatuses;
  [SerializeField] GameObject radioStatusStandard;
  [SerializeField] GameObject radioStatusMinified;
  [SerializeField] GameObject checkboxShowDamage;
  [SerializeField] GameObject checkboxShowItemInfo;
  [Space(10)]

  // Sounds
  [Header("Sounds")]
  [SerializeField] GameObject checkboxPlaySFX;
  [SerializeField] GameObject checkboxPlaySoundtrack;

  // miscellaneous
  [Header("Miscellaneous")]
  [SerializeField] GameObject preferredInputObject;
  [SerializeField] GameObject timeObject;
  [SerializeField] public static EventSystem eventSystem;
  [Space(10)]

  [System.NonSerialized] bool hasGamepad = false;

  // variables to keep track of stats
  [System.NonSerialized] Hero heroScript;
  [System.NonSerialized] string bodyEquipment = "";
  [System.NonSerialized] int playerLevel = -1;
  [System.NonSerialized] int currentHP = -1;
  [System.NonSerialized] int maxHP = -1;
  [System.NonSerialized] int currentMP = -1;
  [System.NonSerialized] int maxMP = -1;
  [System.NonSerialized] string status = "";
  [System.NonSerialized] int exp = -1;
  [System.NonSerialized] int next = -1;
  [System.NonSerialized] int gold = -1;
  [System.NonSerialized] int strength = -1;
  [System.NonSerialized] int stamina = -1;
  [System.NonSerialized] int atk1 = -1;
  [System.NonSerialized] int atk2 = -1;
  [System.NonSerialized] int def1 = -1;
  [System.NonSerialized] int def2 = -1;
  [System.NonSerialized] float luckPercentage = -0.1f;
  [System.NonSerialized] float criticalPercentage = -0.1f;

  [System.NonSerialized] string magicResistances = " ";

  // variables to keep track of preferences
  [System.NonSerialized] string preferredInputString = "";
  [System.NonSerialized] string jumpKeyboardString = "";
  [System.NonSerialized] string jumpGamepadString = "";
  [System.NonSerialized] string atk1KeyboardString = "";
  [System.NonSerialized] string atk1GamepadString = "";
  [System.NonSerialized] string atk2KeyboardString = "";
  [System.NonSerialized] string atk2GamepadString = "";
  [System.NonSerialized] string actionKeyboardString = "";
  [System.NonSerialized] string actionGamepadString = "";

  // variables to keep track of equipment
  [System.NonSerialized] string bodyEquipmentKey = "";
  [System.NonSerialized] string arm1EquipmentKey = "";
  [System.NonSerialized] string arm2EquipmentKey = "";
  [System.NonSerialized] string neckEquipmentKey = "";
  [System.NonSerialized] string armwear1EquipmentKey = "";
  [System.NonSerialized] string armwear2EquipmentKey = "";
  [System.NonSerialized] string ring1EquipmentKey = "";
  [System.NonSerialized] string ring2EquipmentKey = "";

  // current mapping button
  [System.NonSerialized] public static string currentlyMapping = "";
  // current pause status (to match with canvas display)
  [System.NonSerialized] public static string canvasStatus = "main";
  // items list for added item buttons
  [System.NonSerialized] public List<GameObject> itemButtons = new List<GameObject>();
  // current index to display info
  [System.NonSerialized] public int currentItemButtonIndex = -1;
  // tracks the previously selected game button to return to after equipping or cancelling
  [System.NonSerialized] GameObject previouslySelectedEquipmentButton;
  // tracks which item list is currently visible
  [System.NonSerialized] List<Item> currentEquipmentItems;
  // tracks which equipment should be compared to a new equipment
  [System.NonSerialized] int currentlyEquippedIndex;
  // tracks each effect in the effect panel
  [System.NonSerialized] List<GameObject> effectsList = new List<GameObject>();
  // tracks each magic resistance in the adds list
  [System.NonSerialized] List<GameObject> addsList = new List<GameObject>();
  // tracks each magic resistance in the removes list
  [System.NonSerialized] List<GameObject> removesList = new List<GameObject>();
  // tracks projectile buttons in projectile canvas
  [System.NonSerialized] List<GameObject> projectileButtonList = new List<GameObject>();
  // tracks available projectile keys
  [System.NonSerialized] List<string> availableProjectileKeys = new List<string>();
  // tracks if in-game statuses should show
  [System.NonSerialized] bool showInGameStatuses = false;
  // tracks how in-game statuses should show
  [System.NonSerialized] string statusType = "";
  // tracks if damage should show
  [System.NonSerialized] bool showDamage = false;
  // tracks if item info should show
  [System.NonSerialized] bool showItemInfo = false;

  // tracks if sfx should play
  [System.NonSerialized] bool playSFX = false;
  // tracks if soundtrack should play
  [System.NonSerialized] bool playSoundtrack = false;

  // plays sounds
  private AudioSource audioSource;

  private bool canPlayDeselect = false;
  private GameObject previouslyFocusedButton = null;

  [SerializeField] int currentGameTime = 0;

  void Start() {
    heroScript = hero.GetComponent<Hero>();
    audioSource = GetComponent<AudioSource>();
    eventSystem = EventSystem.current;

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

    // adds all projectile buttons to the list
    foreach (Transform currentChild in ProjectilesContainer.transform) {
      projectileButtonList.Add(currentChild.gameObject);
    }

    // sets the area image for the first time
    ChangeArea();
  }

  void Update() {
    CheckEventChange();
    CheckIfGamepad();
    UpdateControls();
    UpdatePreferredInput();
    UpdatePlayerStats();
    UpdateMagicResistances();
    UpdateItemView();
    UpdateEquipmentPromptButtons();
    UpdateInGameElementSettings();
  }

  public void HideCanvases() {
    itemsCanvas.SetActive(false);
    itemsContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    equipmentCanvas.SetActive(false);
    equipmentContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    relicsCanvas.SetActive(false);
    relicsContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    projectileCanvas.SetActive(false);
    mapCanvas.SetActive(false);
    optionsCanvas.SetActive(false);
    controlsCanvas.SetActive(false);
    preferredInputCanvas.SetActive(false);
    inGameElementsCanvas.SetActive(false);
    soundsCanvas.SetActive(false);
    quitCanvas.SetActive(false);
    mainCanvas.SetActive(true);
    gameObject.SetActive(false);
  }

  public void FadeOut() {
    if (currentEquipmentItems != null && currentEquipmentItems.Count > 1) {
      currentEquipmentItems.Clear();
    }
    canvasStatus = "";
    canPlayDeselect = false;
    HideEquipmentLabels();
    HideCanvases();
    Helpers.FocusUIElement(null);
    Time.timeScale = 1;
  }

  // uses the main overlay to cover the game on game over
  public void Cover() {
    GameObject.Find("InGame").gameObject.GetComponent<InGame>().Cover();
  }

  public void ReturnToTitle() {
    StartCoroutine(WaitToTransition());
  }

  // waits in real time (since this would be done while timescale is 0 (paused) for a bit then transitions to title)
  IEnumerator WaitToTransition() {
    float start = Time.realtimeSinceStartup;
    while (Time.realtimeSinceStartup < start + 3f) {
      yield return null;
    }

    SceneManager.LoadScene("Title");
  }

  public void GetTime() {
    // TODO: add saved time in place of initial game time when data is loaded
    currentGameTime = (int) ((Time.time + GameData.initialGameTime) % Constants.maxDayLength);

    // TODO: with currentGameTime now tracking the time, come up with a function to convert time to XX:XX AM/PM
    string currentTimeText = Helpers.GetGameTime(currentGameTime);

    timeObject.GetComponent<Text>().text = currentTimeText;
  }

  void SelectItemsButton() {
    canvasStatus = "main";
    Helpers.FocusUIElement(pauseFirstSelected);
    previouslyFocusedButton  = pauseFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowItemsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "items";
    mainCanvas.SetActive(false);
    itemsCanvas.SetActive(true);

    ClearItems(itemsContainer);
    PopulateItemsContainer(heroScript.items, itemsContainer);
    Helpers.FocusUIElement(itemButtons.ElementAt(0));
    previouslyFocusedButton = itemButtons.ElementAt(0);
    canPlayDeselect = true;
    SetItemInfo(0);
  }

  // destroys all current children of the items container to avoid duplicating
  void ClearItems(GameObject container) {
    foreach (Transform child in container.transform) {
      GameObject.Destroy(child.gameObject);
    }
    itemButtons.Clear();
  }

  // adds all items in the hero item list or equipment list
  void PopulateItemsContainer(List<Item> itemsList, GameObject parentContainer, bool isRelics = false) {
    List<string> itemsToRemove = new List<string>();
    List<string> itemTypes = new List<string>();
    foreach (Item currentItem in itemsList) {
      string currentKey = currentItem.key;

      // TODO: consider shortening this by grabbing the currentItemButton code in the ELSE
      // controls if the items container for equipment should have an UNEQUIP button based on currently equipped items from the currently equipped index
      if (currentKey.Contains("unequip")) {
        String[] unequipData = currentKey.Split('|');
        GameObject currentItemButton = Instantiate(Helpers.GetOrException(Objects.prefabs, "item-button"), Vector2.zero, Quaternion.identity);

        currentItemButton.transform.SetParent(parentContainer.transform);
        currentItemButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Constants.startItemY + (itemButtons.Count * Constants.itemIncrementY * -1));
        currentItemButton.transform.localScale = Vector3.one;
        currentItemButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, unequipData[1]);
        currentItemButton.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.Capitalize(unequipData[0]);
        currentItemButton.transform.Find("Amount").gameObject.GetComponent<Text>().text = "";

        EventTrigger eventTrigger = currentItemButton.GetComponent<EventTrigger>();
        EventTrigger.Entry submitEntry = new EventTrigger.Entry();
        submitEntry.eventID = EventTriggerType.Submit;
        submitEntry.callback.AddListener((data) => {
          PlayMenuSound("select");
        });

        eventTrigger.triggers.Add(submitEntry);

        itemButtons.Add(currentItemButton);
      } else {
        int currentAmount = currentItem.amount;
        PauseItem currentRegularItem = isRelics ? (PauseItem)Helpers.GetOrException(Objects.relicItems, currentKey) : Helpers.GetOrException(Objects.regularItems, currentKey);

        int itemUsageFrequency = Helpers.ValueFrequencyInArray(heroScript.equipmentArray, currentKey);

        if (canvasStatus == "relics" || canvasStatus == "items" || (canvasStatus == "equipment" && (!Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, currentKey) || (Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, currentKey) && Helpers.HasProjectilesForWeapon(currentKey, heroScript.items))) && (itemUsageFrequency < currentAmount || Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, currentKey)))) {
          GameObject currentItemButton = Instantiate(Helpers.GetOrException(Objects.prefabs, "item-button"), Vector2.zero, Quaternion.identity);

          currentItemButton.transform.SetParent(parentContainer.transform);
          currentItemButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Constants.startItemY + (itemButtons.Count * Constants.itemIncrementY * -1));
          currentItemButton.transform.localScale = Vector3.one;
          currentItemButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = currentRegularItem.thumbnail;
          currentItemButton.transform.Find("Text").gameObject.GetComponent<Text>().text = currentRegularItem.name;

          currentItemButton.transform.Find("Amount").gameObject.GetComponent<Text>().text = canvasStatus != "relics" ? (canvasStatus == "equipment" ? currentAmount - (Helpers.IsValueInArray(Constants.projectileHoldingWeaponTypes, currentKey) ? 0 : itemUsageFrequency) : currentAmount).ToString() : "";

          // set the Event Trigger Submit objects and functions
            EventTrigger eventTrigger = currentItemButton.GetComponent<EventTrigger>();

            // submit - but only for items
            if (canvasStatus == "equipment" && currentRegularItem is RegularItem regularSubmitItem) {
              EventTrigger.Entry submitEntry = new EventTrigger.Entry();
              submitEntry.eventID = EventTriggerType.Submit;
              submitEntry.callback.AddListener((data) => {
                if (!Helpers.RequiresProjectile(regularSubmitItem.type)) {
                  PlayMenuSound("select");
                }
              });

              eventTrigger.triggers.Add(submitEntry);
            }
          // end of Event Trigger settings


          itemButtons.Add(currentItemButton);

          if (currentRegularItem is RegularItem regularItem)
          itemTypes.Add(regularItem.type);
        } else {
          if (itemUsageFrequency >= currentAmount) {
            itemsToRemove.Add(currentKey);
          }
        }
      }
    }

    if (canvasStatus == "equipment") {
      for (int j = 0; j < itemsToRemove.Count; j++) {
        currentEquipmentItems.RemoveAll(currItem => currItem.key == itemsToRemove[j]);
      }
    }

    int k = 0;
    foreach (GameObject currentItemButton in itemButtons) {
      if (k > 1 && k < itemButtons.Count()) {
        Navigation buttonNavigation = new Navigation();
        buttonNavigation.mode = Navigation.Mode.Explicit;
        buttonNavigation.selectOnDown = currentItemButton.GetComponent<Button>();
        buttonNavigation.selectOnUp = itemButtons.ElementAt(k - 2).GetComponent<Button>();

        itemButtons.ElementAt(k - 1).GetComponent<Button>().navigation = buttonNavigation;
      }

      if (k == itemButtons.Count() - 1 && itemButtons.Count() > 1) {
        Navigation lastButtonNavigation = new Navigation();
        lastButtonNavigation.mode = Navigation.Mode.Explicit;
        lastButtonNavigation.selectOnDown = itemButtons.ElementAt(0).GetComponent<Button>();
        lastButtonNavigation.selectOnUp = itemButtons.ElementAt(k - 1).GetComponent<Button>();

        itemButtons.ElementAt(k).GetComponent<Button>().navigation = lastButtonNavigation;

        Navigation firstButtonNavigation = new Navigation();
        firstButtonNavigation.mode = Navigation.Mode.Explicit;
        firstButtonNavigation.selectOnDown = itemButtons.ElementAt(1).GetComponent<Button>();
        firstButtonNavigation.selectOnUp = itemButtons.ElementAt(k).GetComponent<Button>();

        itemButtons.ElementAt(0).GetComponent<Button>().navigation = firstButtonNavigation;
      }

      if (canvasStatus == "items" && Helpers.IsUsableItem(itemTypes[k])) {
        currentItemButton.GetComponent<Button>().onClick.AddListener(ProceedToUse);
      }

      if (canvasStatus == "equipment") {
        currentItemButton.GetComponent<Button>().onClick.AddListener(Equip);
      }

      k++;
    }
  }

  public void ProceedToUse() {
    canPlayDeselect = false;
    PlayMenuSound("attention");

    canvasStatus = "items_use";
    Helpers.FocusUIElement(itemUseYes);
    previouslyFocusedButton = itemUseYes;
    canPlayDeselect = true;
  }

  public void Equip() {
    string newItemKey = currentEquipmentItems.ElementAt(currentItemButtonIndex).key;

    if (newItemKey.Contains("unequip")) {
      Hero.projectileEquipment = "";
      heroScript.ChangeArrowContainerSprite(clear: true);
      heroScript.Unequip(currentlyEquippedIndex);
      CancelEquipmentSelection();
    } else {
      string newItemType = Helpers.GetOrException(Objects.regularItems, newItemKey).type;

      // if item to equip is among those that require a projectile to use (i.e. bow that requires arrows), then open a canvas that would allow equipment for this projectile
      if (Helpers.RequiresProjectile(newItemType)) {
        SelectProjectileOnNewItemKey(newItemKey);
      } else { // if not, item would be immediately equipped
        Hero.projectileEquipment = "";
        heroScript.ChangeArrowContainerSprite(clear: true);
        heroScript.EquipItem(newItemKey, currentlyEquippedIndex);
        CancelEquipmentSelection();
      }
    }
  }

  public void SelectProjectileOnNewItemKey(string itemKey) {
    canPlayDeselect = false;
    string[] itemProjectiles = Helpers.GetOrException(Objects.itemProjectiles, itemKey);
    string[] heroItemsList = heroScript.items.Select(item => item.key).ToArray();
    int currProjectileButtonIndex = 0;
    availableProjectileKeys.Clear();

    ClearProjectileButtons();

    foreach(string currentPossibleProjectile in itemProjectiles) {
      if (Helpers.IsValueInArray(heroItemsList, currentPossibleProjectile)) {
        RegularItem availableProjectileItem = Helpers.GetOrException(Objects.regularItems, currentPossibleProjectile);
        Transform projectileButton = projectileButtonList[currProjectileButtonIndex].transform;

        projectileButton.Find("ProjectileImage").GetComponent<Image>().sprite = availableProjectileItem.thumbnail;
        projectileButton.Find("ProjectileName").GetComponent<Text>().text = availableProjectileItem.name;
        projectileButton.Find("ProjectileAmount").GetComponent<Text>().text = "(" + Helpers.GetItemFromList(heroScript.items, currentPossibleProjectile).amount + ")";
        projectileButton.Find("Attack").GetComponent<Text>().text = "+" + availableProjectileItem.effects.atk;
        availableProjectileKeys.Add(currentPossibleProjectile);

        projectileButtonList[currProjectileButtonIndex].SetActive(true);
        currProjectileButtonIndex++;
      }
    }

    if (currProjectileButtonIndex > 1) { // means that there is more than one projectile
      PlayMenuSound("attention");

      canvasStatus = "equipment_select_projectile";
      projectileCanvas.SetActive(true);

      for (int i = 0; i < currProjectileButtonIndex; i++) {
        Navigation buttonNavigation = new Navigation();
        buttonNavigation.mode = Navigation.Mode.Explicit;

        if (i == 0) {
          buttonNavigation.selectOnDown = projectileButtonList[i + 1].GetComponent<Button>();
          buttonNavigation.selectOnUp = projectileButtonList[currProjectileButtonIndex - 1].GetComponent<Button>();
        } else if (i == currProjectileButtonIndex - 1) {
          buttonNavigation.selectOnDown = projectileButtonList[0].GetComponent<Button>();
          buttonNavigation.selectOnUp = projectileButtonList[i - 1].GetComponent<Button>();
        } else {
          buttonNavigation.selectOnDown = projectileButtonList[i + 1].GetComponent<Button>();
          buttonNavigation.selectOnUp = projectileButtonList[i - 1].GetComponent<Button>();
        }

        projectileButtonList[i].GetComponent<Button>().navigation = buttonNavigation;
      }

      Helpers.FocusUIElement(projectileButtonList[0]);
      previouslyFocusedButton = projectileButtonList[0];
      canPlayDeselect = true;

    } else { // if not more than one, there can only be 1 type, so use that type
      EquipProjectile(0);
    }
  }

  public void EquipProjectile(int projectileIndex) {
    Hero.projectileEquipment = availableProjectileKeys[projectileIndex];
    heroScript.ChangeArrowContainerSprite();
    string newItemKey = currentEquipmentItems.ElementAt(currentItemButtonIndex).key;
    heroScript.EquipItem(newItemKey, currentlyEquippedIndex);
    arm1EquipmentKey = "";
    arm2EquipmentKey = "";
    CancelProjectileSelection();
    CancelEquipmentSelection();
  }

  public void ClearProjectileButtons() {
    foreach(GameObject currButton in projectileButtonList) {
      currButton.SetActive(false);
    }
  }

  public void UseItem() {
    canPlayDeselect = false;
    Item heroItem = heroScript.items.ElementAt(currentItemButtonIndex);
    RegularItem currentRegularItem = Helpers.GetOrException(Objects.regularItems, heroItem.key);

    canvasStatus = "items";

    if (currentRegularItem.effects != null) {
      Effects itemEffects = currentRegularItem.effects;

      if (itemEffects.duration == null) {
        if (itemEffects.hp != null) {
          effectsCurrentHP.transform.Find("Text").gameObject.GetComponent<Text>().text = (itemEffects.hp >= 0 ? "+" : "") + itemEffects.hp;
          effectsCurrentHP.SetActive(true);

          // TODO: ensure that this can be either temporary (for potions of limited time use) or permanent (for single use items)
          heroScript.UpdateStats("hp", itemEffects.hp);
        }

        if (itemEffects.hpPercentage != null) {
          int hpTotal = (int)(itemEffects.hpPercentage * heroScript.maxHP);
          effectsCurrentHP.transform.Find("Text").gameObject.GetComponent<Text>().text = (hpTotal >= 0 ? "+" : "") + hpTotal;
          effectsCurrentHP.SetActive(true);

          // TODO: ensure that this can be either temporary (for potions of limited time use) or permanent (for single use items)
          heroScript.UpdateStats("hp", hpTotal);
        }

        // TODO: build the others as more items are created!
        if (itemEffects.mp != null) {
          effectsCurrentMP.transform.Find("Text").gameObject.GetComponent<Text>().text = (itemEffects.mp >= 0 ? "+" : "") + itemEffects.mp;
          effectsCurrentMP.SetActive(true);

          // TODO: ensure that this can be either temporary (for potions of limited time use) or permanent (for single use items)
          heroScript.UpdateStats("mp", itemEffects.mp);
        }

        if (itemEffects.mpPercentage != null) {
          int mpTotal = (int)(itemEffects.mpPercentage * heroScript.maxMP);
          effectsCurrentMP.transform.Find("Text").gameObject.GetComponent<Text>().text = (mpTotal >= 0 ? "+" : "") + mpTotal;
          effectsCurrentMP.SetActive(true);

          // TODO: ensure that this can be either temporary (for potions of limited time use) or permanent (for single use items)
          heroScript.UpdateStats("mp", mpTotal);
        }
      } else {
        heroScript.AddConsumable(new Consumable(){key=heroItem.key, duration=(float)itemEffects.duration, useTime=Time.time * 1000});
      }

      // determines what to do with the item and its amount
      if (heroItem.amount > 1) {
        heroScript.ConsumeItem(heroItem.key);
        itemButtons.ElementAt(0).transform.Find("Amount").gameObject.GetComponent<Text>().text = (heroItem.amount).ToString();
        Helpers.FocusUIElement(itemButtons.ElementAt(currentItemButtonIndex));
        previouslyFocusedButton = itemButtons.ElementAt(currentItemButtonIndex);
      } else {
        heroScript.RemoveItem(currentItemButtonIndex);
        ClearItems(itemsContainer);
        PopulateItemsContainer(heroScript.items, itemsContainer);
        Helpers.FocusUIElement(itemButtons.ElementAt(0));
        previouslyFocusedButton = itemButtons.ElementAt(0);
        SetItemInfo(0);
      }
    }
    canPlayDeselect = true;
  }

  public void CancelItemUse() {
    canPlayDeselect = false;
    canvasStatus = "items";
    GameObject itemToUse = itemButtons.ElementAt(currentItemButtonIndex);
    Helpers.FocusUIElement(itemToUse);
    previouslyFocusedButton = itemToUse;
    canPlayDeselect = true;
  }

  public void CancelEquipmentSelection() {
    equipmentContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    canPlayDeselect = false;
    HideEquipmentLabels();
    canvasStatus = "equipment";
    Helpers.FocusUIElement(previouslySelectedEquipmentButton);
    previouslyFocusedButton = previouslySelectedEquipmentButton;
    previouslySelectedEquipmentButton = null;
    canPlayDeselect = true;
  }

  public void CancelProjectileSelection() {
    canPlayDeselect = false;
    canvasStatus = "equipment_select";
    projectileCanvas.SetActive(false);
    ClearProjectileButtons();
    Helpers.FocusUIElement(itemButtons[currentItemButtonIndex]);
    previouslyFocusedButton = itemButtons[currentItemButtonIndex];
    canPlayDeselect = true;
  }

  void UpdateItemView() {
    if (Helpers.IsOnItemContainerState(Constants.itemContainerStates, canvasStatus) && itemButtons.Count > 0) {
      int i = 0;
      GameObject currentContainer = canvasStatus == "items" ? itemsContainer : canvasStatus == "equipment_select" ? equipmentContainer : canvasStatus == "relics" ? relicsContainer : null;

      foreach (GameObject currentItemButton in itemButtons) {
        if (eventSystem.currentSelectedGameObject == currentItemButton) {
          // do not change item view info if indices match; this means the item is already displaying
          if (currentItemButtonIndex != i) {
            int indexDifference = i - currentItemButtonIndex;
            // going down
            if (indexDifference == 1) {
              if (i > Constants.maxItemContainerHeight - 1) {
                int movingItemLocation = (int)(Constants.startItemY - (Constants.itemIncrementY * (Constants.maxItemContainerHeight - 1)));
                int selectedItemLocation = (int)(itemButtons.ElementAt(i - 1).GetComponent<RectTransform>().anchoredPosition.y + currentContainer.GetComponent<RectTransform>().anchoredPosition.y);

                // to avoid moving the container up if the selected button is not at the bottom
                if (selectedItemLocation == movingItemLocation) {
                  currentContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentContainer.GetComponent<RectTransform>().anchoredPosition.x, currentContainer.GetComponent<RectTransform>().anchoredPosition.y + Constants.itemIncrementY);
                }
              }
            } else if (indexDifference == -1) { //going up
              if (i <= (itemButtons.Count - Constants.maxItemContainerHeight - 1)) {
                int movingItemLocation = (int)(Constants.startItemY - (Constants.itemIncrementY * (itemButtons.Count - Constants.maxItemContainerHeight)));
                int selectedItemLocation = (int)(itemButtons.ElementAt(i).GetComponent<RectTransform>().anchoredPosition.y + (currentContainer.GetComponent<RectTransform>().anchoredPosition.y - ((itemButtons.Count + 1 - Constants.maxItemContainerHeight) * Constants.itemIncrementY)));

                // to avoid moving the container down if the selected button is not at the top
                if (selectedItemLocation == movingItemLocation) {
                  currentContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentContainer.GetComponent<RectTransform>().anchoredPosition.x, currentContainer.GetComponent<RectTransform>().anchoredPosition.y - Constants.itemIncrementY);
                }
              }
            } else if (indexDifference == itemButtons.Count - 1 && itemButtons.Count > Constants.maxItemContainerHeight) { // from first item to last
              currentContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentContainer.GetComponent<RectTransform>().anchoredPosition.x, (Constants.itemIncrementY * (itemButtons.Count - Constants.maxItemContainerHeight)));
            } else if (indexDifference == -(itemButtons.Count - 1)) { // from last item to first
              currentContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }

            if (canvasStatus == "items") {
              SetItemInfo(i);
            } else if (canvasStatus == "equipment_select") {
              SetEquipmentProspect(i);
            } else if(canvasStatus == "relics") {
              SetRelicInfo(i);
            }
          }
          break;
        }

        i++;
      }
    }
  }

  void HideEquippedResistances() {
    foreach (Transform magicResistanceChild in EquippedResistancesContainer.transform) {
      Image currentImage = magicResistanceChild.gameObject.GetComponent<Image>();
      currentImage.sprite = null;
      currentImage.color = Colors.pauseBackground;
    }

    EquippedResistancesContainer.SetActive(false);
  }

  void HideEquipmentLabels() {
    EquippedSTRLabel.SetActive(false);
    EquippedSTALabel.SetActive(false);
    EquippedATK1Label.SetActive(false);
    EquippedATK2Label.SetActive(false);
    EquippedDEF1Label.SetActive(false);
    EquippedDEF2Label.SetActive(false);
    EquippedCRITLabel.SetActive(false);
    EquippedLUCKLabel.SetActive(false);
    HideEquippedResistances();
  }

  void SetEquipmentProspect(int index) {
    HideEquipmentLabels();

    currentItemButtonIndex = index;

    // grab current equipped
    RegularItem currentEquipment = heroScript.equipmentArray[currentlyEquippedIndex] != "" ? Helpers.GetOrException(Objects.regularItems, heroScript.equipmentArray[currentlyEquippedIndex]) : null;

    string currentItemKey = currentEquipmentItems.ElementAt(currentItemButtonIndex).key;

    // grab current selected based on index
    RegularItem selectedEquipment = currentItemKey.Contains("unequip") ? new RegularItem(null, null, "", "", currentEquipment.type, new Effects(){}) : Helpers.GetOrException(Objects.regularItems, currentItemKey);

    // check if the selected equipment is a double handed item
    bool isEquippingDouble = Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, selectedEquipment.type);

    // check if the current equipment is a double handed item
    bool equippedIsDouble = currentEquipment != null ? Helpers.IsValueInArray(Constants.doubleHandedWeaponTypes, currentEquipment.type) : false;

    // Check ATK1
    if (currentlyEquippedIndex == 1 || (currentlyEquippedIndex == 2 && (isEquippingDouble || equippedIsDouble))) {
      RegularItem otherArmEquipment = heroScript.equipmentArray[1] != "" ? Helpers.GetOrException(Objects.regularItems, heroScript.equipmentArray[1]) : null;
      RegularItem equippedSelected = (currentlyEquippedIndex == 2 && (isEquippingDouble || equippedIsDouble)) ? otherArmEquipment : currentEquipment;

      // gets the attack of the selected equipment (unless user is equipping the other hand with a single and currently carries a double),
      int newEquippedATK1 = (currentlyEquippedIndex == 2 && !isEquippingDouble && equippedIsDouble ? 0 : (selectedEquipment.effects.atk ?? 0)) +
        (Hero.equippedATK1 - ( // plus whatever is currently equipped
          (equippedSelected != null ? equippedSelected.effects.atk ?? 0 : 0) + // minus the item that is currently equipped
            (Hero.projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, Hero.projectileEquipment).effects.atk ?? 0 : 0) + // including if there is a projectile equipped
            (currentlyEquippedIndex == 2 && equippedIsDouble && !isEquippingDouble ? equippedSelected.effects.atk ?? 0 : 0) // and the selected equipment if equipping the other hand with a single while currently carrying a double
          )
        );

      if (newEquippedATK1 != Hero.equippedATK1) {
        Color equippedLabelColor = newEquippedATK1 > Hero.equippedATK1 ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedATK1Label.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedATK1 > 0 ? newEquippedATK1.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedATK1Label.SetActive(true);
      }
    }

    // Check ATK2
    if (currentlyEquippedIndex == 2 || (currentlyEquippedIndex == 1 && (isEquippingDouble || equippedIsDouble))) {
      RegularItem otherArmEquipment = heroScript.equipmentArray[2] != "" ? Helpers.GetOrException(Objects.regularItems, heroScript.equipmentArray[2]) : null;
      RegularItem equippedSelected = (currentlyEquippedIndex == 1 && (isEquippingDouble || equippedIsDouble)) ? otherArmEquipment : currentEquipment;

      // gets the attack of the selected equipment (unless user is equipping the other hand with a single and currently carries a double),
      int newEquippedATK2 = (currentlyEquippedIndex == 1 && !isEquippingDouble && equippedIsDouble ? 0 : (selectedEquipment.effects.atk ?? 0)) +
        (Hero.equippedATK2 - ( // plus whatever is currently equipped
          (equippedSelected != null ? equippedSelected.effects.atk ?? 0 : 0) + // minus the item that is currently equipped
            (Hero.projectileEquipment != "" ? Helpers.GetOrException(Objects.regularItems, Hero.projectileEquipment).effects.atk ?? 0 : 0) + // including if there is a projectile equipped
            (currentlyEquippedIndex == 1 && equippedIsDouble && !isEquippingDouble ? equippedSelected.effects.atk ?? 0 : 0) // and the selected equipment if equipping the other hand with a single while currently carrying a double
          )
        );

      if (newEquippedATK2 != Hero.equippedATK2) {
        Color equippedLabelColor = newEquippedATK2 > Hero.equippedATK2 ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedATK2Label.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedATK2 > 0 ? newEquippedATK2.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedATK2Label.SetActive(true);
      }
    }

    // Check DEF1
    if (currentlyEquippedIndex == 1 || (currentlyEquippedIndex == 2 && (isEquippingDouble))) {
      RegularItem otherArmEquipment = heroScript.equipmentArray[1] != "" ? Helpers.GetOrException(Objects.regularItems, heroScript.equipmentArray[1]) : null;
      RegularItem equippedSelected = (currentlyEquippedIndex == 2 && (isEquippingDouble || equippedIsDouble)) ? otherArmEquipment : currentEquipment;

      int newEquippedDEF1 = (selectedEquipment.effects.def ?? 0) + (heroScript.equippedDEF1 - (equippedSelected != null ? equippedSelected.effects.def ?? 0 : 0));
      if (newEquippedDEF1 != heroScript.equippedDEF1) {
        Color equippedLabelColor = newEquippedDEF1 > heroScript.equippedDEF1 ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedDEF1Label.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedDEF1 > 0 ? newEquippedDEF1.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedDEF1Label.SetActive(true);
      }
    }

    // Check DEF2
    if (currentlyEquippedIndex == 2 || (currentlyEquippedIndex == 1 && isEquippingDouble)) {
      RegularItem otherArmEquipment = heroScript.equipmentArray[2] != "" ? Helpers.GetOrException(Objects.regularItems, heroScript.equipmentArray[2]) : null;
      RegularItem equippedSelected = (currentlyEquippedIndex == 1 && (isEquippingDouble || equippedIsDouble)) ? otherArmEquipment : currentEquipment;

      int newEquippedDEF2 = (selectedEquipment.effects.def ?? 0) + (heroScript.equippedDEF2 - (equippedSelected != null ? equippedSelected.effects.def ?? 0 : 0));
      if (newEquippedDEF2 != heroScript.equippedDEF2) {
        Color equippedLabelColor = newEquippedDEF2 > heroScript.equippedDEF2 ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedDEF2Label.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedDEF2 > 0 ? newEquippedDEF2.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedDEF2Label.SetActive(true);
      }
    }

    // Check STR
    if (currentlyEquippedIndex > 2) {
      int totalEquippedSTR = heroScript.strength + (int)heroScript.equippedSTR + (int)heroScript.effectSTR;
      float newEquippedSTR = (selectedEquipment.effects.atk ?? 0) + (totalEquippedSTR - (currentEquipment != null ? currentEquipment.effects.atk ?? 0 : 0));
      if (newEquippedSTR != totalEquippedSTR) {
        Color equippedLabelColor = newEquippedSTR > totalEquippedSTR ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedSTRLabel.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedSTR > 0 ? newEquippedSTR.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedSTRLabel.SetActive(true);
      }
    }

    // Check STA
    if (currentlyEquippedIndex > 2) {
      int totalEquippedSTA = heroScript.stamina + (int)heroScript.equippedSTA + (int)heroScript.effectSTA;
      float newEquippedSTA = (selectedEquipment.effects.def ?? 0) + (totalEquippedSTA - (currentEquipment != null ? currentEquipment.effects.def ?? 0 : 0));
      if (newEquippedSTA != totalEquippedSTA) {
        Color equippedLabelColor = newEquippedSTA > totalEquippedSTA ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

        Text equipmentText = EquippedSTALabel.transform.Find("Text").gameObject.GetComponent<Text>();
        equipmentText.text = newEquippedSTA > 0 ? newEquippedSTA.ToString() : "0";
        equipmentText.color = equippedLabelColor;

        EquippedSTALabel.SetActive(true);
      }
    }

    // Check CRIT
    float totalEquippedCRIT = heroScript.criticalPercentage + heroScript.equippedCRIT + heroScript.effectCRIT;
    float newEquippedCRIT = (selectedEquipment.effects.crit ?? 0) + (totalEquippedCRIT - (currentEquipment != null ? currentEquipment.effects.crit ?? 0 : 0));
    if (newEquippedCRIT != totalEquippedCRIT) {
      Color equippedLabelColor = newEquippedCRIT > totalEquippedCRIT ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

      Text equipmentText = EquippedCRITLabel.transform.Find("Text").gameObject.GetComponent<Text>();
      equipmentText.text = newEquippedCRIT > 0 ? Helpers.TwoDecimalPlaces(newEquippedCRIT * 100) + " %" : "0 %";
      equipmentText.color = equippedLabelColor;

      EquippedCRITLabel.SetActive(true);
    }

    // Check LUCK
    float totalEquippedLUCK = heroScript.luckPercentage + heroScript.equippedLUCK + heroScript.effectLCK;
    float newEquippedLUCK = (selectedEquipment.effects.luck ?? 0) + (totalEquippedLUCK - (currentEquipment != null ? currentEquipment.effects.luck ?? 0 : 0));
    if (newEquippedLUCK != totalEquippedLUCK) {
      Color equippedLabelColor = newEquippedLUCK > totalEquippedLUCK ? Helpers.GetOrException(Colors.pauseStatsColors, "higher") : Helpers.GetOrException(Colors.pauseStatsColors, "lower");

      Text equipmentText = EquippedLUCKLabel.transform.Find("Text").gameObject.GetComponent<Text>();
      equipmentText.text = newEquippedLUCK > 0 ? Helpers.TwoDecimalPlaces(newEquippedLUCK * 100) + " %" : "0 %";
      equipmentText.color = equippedLabelColor;

      EquippedLUCKLabel.SetActive(true);
    }

    // Check magic resistances
    MagicResistance[] possibleNewMagicResistances = selectedEquipment.effects.magicResistances ?? null;
    // clones the heroScript.magicResistances array to a new instance to compare
    HeroMagicResistance[] newMagicResistances = Array.ConvertAll(heroScript.magicResistances, currMR => new HeroMagicResistance() {name=(string)currMR.name.Clone(),  frequency=(int)currMR.frequency});

    // adds the effect magic resistances to make the comparison
    for (int i = 0; i < heroScript.effectMagicResistances.Length; i++) {
      HeroMagicResistance currentEffectMagicResistance = heroScript.effectMagicResistances[i];
      if (currentEffectMagicResistance.frequency == 1) {
        newMagicResistances[i].frequency = 1;
      }
    }

    // removes the current equipment's magic resistances (if any)
    if (currentEquipment != null && currentEquipment.effects.magicResistances != null) {
      foreach (MagicResistance currMagicResistance in currentEquipment.effects.magicResistances) {
        newMagicResistances[heroScript.magicResistanceTypeIndex[currMagicResistance.name]].frequency += currMagicResistance.type == "add" ? -1 : 1;
      }
    }

    if (possibleNewMagicResistances != null) {
      foreach (MagicResistance currentMagicResistance in possibleNewMagicResistances) {
        newMagicResistances[heroScript.magicResistanceTypeIndex[currentMagicResistance.name]].frequency += currentMagicResistance.type == "add" ? 1 : -1;
      }
    }

    int equippedResistanceIndex = 0;
    foreach (HeroMagicResistance currentMagicResistance in newMagicResistances) {
      // if frequency is 0, then it shouldn't be added
      if (currentMagicResistance.frequency >= 1) {
        Image currentResistanceImage = EquippedResistancesContainer.transform.GetChild(equippedResistanceIndex).gameObject.GetComponent<Image>();
        currentResistanceImage.sprite = Helpers.GetOrException(Sprites.magicResistances, currentMagicResistance.name.ToLower());
        currentResistanceImage.color = Color.white;

        equippedResistanceIndex++;
      }
    }

    EquippedResistancesContainer.SetActive(true);
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

  void SetRelicInfo(int index) {
    currentItemButtonIndex = index;

    RelicItem currentRelicItem = Helpers.GetOrException(Objects.relicItems, heroScript.relicItems[index].key);
    relicName.GetComponent<Text>().text = currentRelicItem.name.ToUpper();
    relicImage.GetComponent<Image>().sprite = currentRelicItem.image;
    relicDescription.GetComponent<Text>().text = currentRelicItem.description;
  }

  void SetItemInfo(int index) {
    HideEffectsObjects();
    currentItemButtonIndex = index;

    RegularItem currentRegularItem = Helpers.GetOrException(Objects.regularItems, heroScript.items[index].key);
    itemName.GetComponent<Text>().text = currentRegularItem.name.ToUpper();
    itemImage.GetComponent<Image>().sprite = currentRegularItem.image;
    itemDescription.GetComponent<Text>().text = currentRegularItem.description;
    itemUseRectangle.SetActive(Helpers.IsUsableItem(currentRegularItem.type));

    if (currentRegularItem.effects != null) {
      Effects itemEffects = currentRegularItem.effects;

      int effectsCounter = 0;

      if (itemEffects.hp != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[0];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.hp >= 0 ? "+" : "") + itemEffects.hp;
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.hpPercentage != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[0];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.hpPercentage >= 0 ? "+" : "") + Helpers.TwoDecimalPlaces(itemEffects.hpPercentage * 100, ignoreWhenWhole: true) + "%";
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.mp != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[1];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.mp >= 0 ? "+" : "") + itemEffects.mp;
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.mpPercentage != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[1];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.mpPercentage >= 0 ? "+" : "") + Helpers.TwoDecimalPlaces(itemEffects.mpPercentage * 100, ignoreWhenWhole: true) + "%";
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.atk != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[6];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.atk >= 0 ? "+" : "") + itemEffects.atk;
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.def != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[7];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.def >= 0 ? "+" : "") + itemEffects.def;
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.crit != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[10];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.crit >= 0 ? "+" : "") + Helpers.TwoDecimalPlaces(itemEffects.crit * 100, ignoreWhenWhole: true) + "%";
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      if (itemEffects.luck != null) {
        effectsList.ElementAt(effectsCounter).transform.Find("EffectIcon").GetComponent<Image>().sprite = Sprites.statsIcons[11];
        effectsList.ElementAt(effectsCounter).transform.Find("EffectText").GetComponent<Text>().text = (itemEffects.luck >= 0 ? "+" : "") + Helpers.TwoDecimalPlaces(itemEffects.luck * 100, ignoreWhenWhole: true) + "%";
        effectsList.ElementAt(effectsCounter).SetActive(true);
        effectsCounter++;
      }

      // if effectsCounter is 0, no single effects have been added, so the container should hide
      if (effectsCounter == 0) {
        itemEffectsGroupPanel.SetActive(false);
      } else {
        if (effectsCounter < 3) { // if the effectsCounter is less than 3, we only have 3 single effects thus the container should halve its height
          itemEffectsGroupPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(335.66f, 31);
        } else { // normal height for when an item has more than 3 single effects
          itemEffectsGroupPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(335.66f, 62);
        }

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
        itemEffectsTimeLabel.GetComponent<Text>().text = itemEffects.duration + " " + (itemEffects.duration == 1 ? "sec" : "secs");
        itemEffectsTimeLabel.SetActive(true);
      }

      itemEffectsPanel.SetActive(true);
    } else {
      itemEffectsPanel.SetActive(false);
    }
  }

  public void ShowEquipmentCanvas() {
    canPlayDeselect = false;
    canvasStatus = "equipment";
    mainCanvas.SetActive(false);
    equipmentCanvas.SetActive(true);

    Helpers.FocusUIElement(equipmentFirstSelected);
    previouslyFocusedButton = equipmentFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowRelicsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "relics";
    mainCanvas.SetActive(false);
    relicsCanvas.SetActive(true);

    ClearItems(relicsContainer);
    PopulateItemsContainer(heroScript.relicItems, relicsContainer, isRelics: true);
    Helpers.FocusUIElement(itemButtons.ElementAt(0));
    previouslyFocusedButton = itemButtons.ElementAt(0);
    canPlayDeselect = true;
    SetRelicInfo(0);
  }

  public void ShowMapCanvas() {
    canPlayDeselect = false;
    canvasStatus = "map";
    mainCanvas.SetActive(false);
    mapCanvas.SetActive(true);

    // TODO: when implementing, ensure to show buttons equivalent to each area, and activate the one currently in
    Helpers.FocusUIElement(optionsFirstSelected);
    previouslyFocusedButton = optionsFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowOptionsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "options";
    mainCanvas.SetActive(false);
    optionsCanvas.SetActive(true);

    Helpers.FocusUIElement(optionsFirstSelected);
    previouslyFocusedButton = optionsFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowControlsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "options_controls";
    optionsCanvas.SetActive(false);
    controlsCanvas.SetActive(true);

    Helpers.FocusUIElement(controlsFirstSelected);
    previouslyFocusedButton = controlsFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowPreferredInputCanvas() {
    canPlayDeselect = false;
    canvasStatus = "options_preferred-input";
    optionsCanvas.SetActive(false);
    preferredInputCanvas.SetActive(true);

    Helpers.FocusUIElement(preferredInputFirstSelected);
    previouslyFocusedButton = preferredInputFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowInGameElementsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "options_show-in-game-elements";
    optionsCanvas.SetActive(false);
    inGameElementsCanvas.SetActive(true);

    Helpers.FocusUIElement(inGameElementsFirstSelected);
    previouslyFocusedButton = inGameElementsFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowSoundsCanvas() {
    canPlayDeselect = false;
    canvasStatus = "options_sounds";
    optionsCanvas.SetActive(false);
    soundsCanvas.SetActive(true);

    Helpers.FocusUIElement(soundsFirstSelected);
    previouslyFocusedButton = soundsFirstSelected;
    canPlayDeselect = true;
  }

  public void ShowQuitCanvas() {
    canPlayDeselect = false;
    canvasStatus = "quit";
    mainCanvas.SetActive(false);
    quitCanvas.SetActive(true);

    Helpers.FocusUIElement(quitFirstSelected);
    previouslyFocusedButton = quitFirstSelected;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromItems() {
    itemsContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    canPlayDeselect = false;
    canvasStatus = "main";
    itemUseRectangle.SetActive(false);
    itemsCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(itemsButton);
    previouslyFocusedButton = itemsButton;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromEquipment() {
    canPlayDeselect = false;
    currentEquipmentItems.Clear();
    canvasStatus = "main";
    equipmentCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(equipmentButton);
    previouslyFocusedButton = equipmentButton;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromRelics() {
    canPlayDeselect = false;
    canvasStatus = "main";
    relicsCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(relicsButton);
    previouslyFocusedButton = relicsButton;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromMap() {
    canPlayDeselect = false;
    canvasStatus = "main";
    mapCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(mapButton);
    previouslyFocusedButton = mapButton;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromOptions() {
    canPlayDeselect = false;
    canvasStatus = "main";
    optionsCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(optionsButton);
    previouslyFocusedButton = optionsButton;
    canPlayDeselect = true;
  }

  public void GoBackToOptionsFromControls() {
    canPlayDeselect = false;
    canvasStatus = "options";
    controlsCanvas.SetActive(false);
    optionsCanvas.SetActive(true);

    Helpers.FocusUIElement(controlsButton);
    previouslyFocusedButton = controlsButton;
    canPlayDeselect = true;
  }

  public void GoBackToOptionsFromPreferredInput() {
    canPlayDeselect = false;
    canvasStatus = "options";
    preferredInputCanvas.SetActive(false);
    optionsCanvas.SetActive(true);

    Helpers.FocusUIElement(preferredInputButton);
    previouslyFocusedButton = preferredInputButton;
    canPlayDeselect = true;
  }

  public void GoBackToOptionsFromShowInGameElements() {
    canPlayDeselect = false;
    canvasStatus = "options";
    inGameElementsCanvas.SetActive(false);
    optionsCanvas.SetActive(true);

    Helpers.FocusUIElement(inGameElementsButton);
    previouslyFocusedButton = inGameElementsButton;
    canPlayDeselect = true;
  }

  public void GoBackToOptionsFromSounds() {
    canPlayDeselect = false;
    canvasStatus = "options";
    soundsCanvas.SetActive(false);
    optionsCanvas.SetActive(true);

    Helpers.FocusUIElement(soundsButton);
    previouslyFocusedButton = soundsButton;
    canPlayDeselect = true;
  }

  public void GoBackToMainFromQuit() {
    canPlayDeselect = false;
    canvasStatus = "main";
    quitCanvas.SetActive(false);
    mainCanvas.SetActive(true);

    Helpers.FocusUIElement(quitButton);
    previouslyFocusedButton = quitButton;
    canPlayDeselect = true;
  }

  public void PerformBack() {
    PlayMenuSound("back");

    switch (canvasStatus) {
      case "items":
        GoBackToMainFromItems();
      break;
      case "items_use":
        CancelItemUse();
      break;
      case "equipment":
        GoBackToMainFromEquipment();
      break;
      case "equipment_select":
        CancelEquipmentSelection();
      break;
      case "equipment_select_projectile":
        CancelProjectileSelection();
      break;
      case "relics":
        GoBackToMainFromRelics();
      break;
      case "map":
        GoBackToMainFromMap();
      break;
      case "options":
        GoBackToMainFromOptions();
      break;
      case "options_controls":
        GoBackToOptionsFromControls();
      break;
      case "options_controls_reset":
        CancelReset();
      break;
      case "options_preferred-input":
        GoBackToOptionsFromPreferredInput();
      break;
      case "options_show-in-game-elements":
        GoBackToOptionsFromShowInGameElements();
      break;
      case "options_sounds":
        GoBackToOptionsFromSounds();
      break;
      case "quit":
        GoBackToMainFromQuit();
      break;
      default:
        Debug.Log("unknown canvas status: " + canvasStatus);
      break;
    }
  }

  public void QuitGame() {
    GetComponent<Animator>().Play("pause-return-title");
  }

  void CheckEventChange() {
    if (canPlayDeselect) {
      GameObject currentlyFocusedButton = eventSystem.currentSelectedGameObject;

      if (currentlyFocusedButton != previouslyFocusedButton) {
        PlayMenuSound("move");
        previouslyFocusedButton = currentlyFocusedButton;
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

    if (hasGamepad && Constants.preferredInput == "gamepad" && mainButtonPanel.activeInHierarchy == false ) {
      ShowGamePadOptions();
    } else if ((!hasGamepad || Constants.preferredInput == "keyboard") && mainKeysPanel.activeInHierarchy == false) {
      ShowKeyboardOptions();
    }
  }

  void UpdateControls() {
    if (jumpKeyboardString != Controls.currentKeyboardJump) {
      jumpKeyboardString = Controls.currentKeyboardJump;
      jumpKeyboardImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, jumpKeyboardString);
    }

    if (jumpGamepadString != Controls.currentGamepadJump) {
      jumpGamepadString = Controls.currentGamepadJump;
      jumpGamepadImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, jumpGamepadString);
    }

    if (atk1KeyboardString != Controls.currentKeyboardAttack1) {
      atk1KeyboardString = Controls.currentKeyboardAttack1;
      atk1KeyboardImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, atk1KeyboardString);
    }

    if (atk1GamepadString != Controls.currentGamepadAttack1) {
      atk1GamepadString = Controls.currentGamepadAttack1;
      atk1GamepadImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, atk1GamepadString);
    }

    if (atk2KeyboardString != Controls.currentKeyboardAttack2) {
      atk2KeyboardString = Controls.currentKeyboardAttack2;
      atk2KeyboardImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, atk2KeyboardString);
    }

    if (atk2GamepadString != Controls.currentGamepadAttack2) {
      atk2GamepadString = Controls.currentGamepadAttack2;
      atk2GamepadImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, atk2GamepadString);
    }

    if (actionKeyboardString != Controls.currentKeyboardAction) {
      actionKeyboardString = Controls.currentKeyboardAction;
      actionKeyboardImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, actionKeyboardString);
    }

    if (actionGamepadString != Controls.currentGamepadAction) {
      actionGamepadString = Controls.currentGamepadAction;
      actionGamepadImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, actionGamepadString);
    }
  }

  void UpdatePreferredInput() {
    if (preferredInputString != Constants.preferredInput) {
      preferredInputString = Constants.preferredInput;
      preferredInputObject.GetComponent<Text>().text = preferredInputString.ToUpper();
    }
  }

  void UpdatePlayerStats() {
    if (bodyEquipment != Hero.bodyEquipment) {
      bodyEquipment = Hero.bodyEquipment;
      playerAvatar.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.pauseAvatars, bodyEquipment);
    }

    if (playerLevel != heroScript.playerLevel) {
      playerLevel = heroScript.playerLevel;
      level.GetComponent<Text>().text = (playerLevel).ToString();
    }

    if (currentHP != heroScript.currentHP) {
      currentHP = heroScript.currentHP;
      currentHPObject.GetComponent<Text>().text = (currentHP).ToString();
    }

    if (maxHP != heroScript.maxHP) {
      maxHP = heroScript.maxHP;
      totalHP.GetComponent<Text>().text = (maxHP).ToString();
    }

    if (currentMP != heroScript.currentMP) {
      currentMP = heroScript.currentMP;
      currentMPObject.GetComponent<Text>().text = (currentMP).ToString();
    }

    if (maxMP != heroScript.maxMP) {
      maxMP = heroScript.maxMP;
      totalMP.GetComponent<Text>().text = (maxMP).ToString();
    }

    if (status != heroScript.status) {
      status = heroScript.status;
      statusObject.GetComponent<Text>().text = char.ToUpper(status[0]) + status.Substring(1);
    }

    if (exp != heroScript.exp) {
      exp = heroScript.exp;
      expObject.GetComponent<Text>().text = (exp).ToString();
    }

    if (next != heroScript.next) {
      next = heroScript.next;
      nextObject.GetComponent<Text>().text = (next).ToString();
    }

    if (gold != heroScript.gold) {
      gold = heroScript.gold;
      goldObject.GetComponent<Text>().text = (gold).ToString();
    }

    if (strength != (heroScript.strength + (int)heroScript.equippedSTR + (int)heroScript.effectSTR)) {
      strength = heroScript.strength + (int)heroScript.equippedSTR + (int)heroScript.effectSTR;
      strObject.GetComponent<Text>().text = (strength).ToString();
      if (heroScript.effectSTR > 0) {
        strObject.GetComponent<Text>().color = Colors.effect;
      } else {
        strObject.GetComponent<Text>().color = Color.white;
      }
    }

    if (stamina != (heroScript.stamina + (int)heroScript.equippedSTA + (int)heroScript.effectSTA)) {
      stamina = heroScript.stamina + (int)heroScript.equippedSTA + (int)heroScript.effectSTA;
      staObject.GetComponent<Text>().text = (stamina).ToString();
      if (heroScript.effectSTA > 0) {
        staObject.GetComponent<Text>().color = Colors.effect;
      } else {
        staObject.GetComponent<Text>().color = Color.white;
      }
    }

    if (atk1 != Hero.equippedATK1) {
      atk1 = Hero.equippedATK1;
      atk1Object.GetComponent<Text>().text = (atk1).ToString();
    }

    if (atk2 != Hero.equippedATK2) {
      atk2 = Hero.equippedATK2;
      atk2Object.GetComponent<Text>().text = (atk2).ToString();
    }

    if (def1 != heroScript.equippedDEF1) {
      def1 = heroScript.equippedDEF1;
      def1Object.GetComponent<Text>().text = (def1).ToString();
    }

    if (def2 != heroScript.equippedDEF2) {
      def2 = heroScript.equippedDEF2;
      def2Object.GetComponent<Text>().text = (def2).ToString();
    }

    if (criticalPercentage != (heroScript.criticalPercentage + heroScript.equippedCRIT + heroScript.effectCRIT)) {
      criticalPercentage = heroScript.criticalPercentage + heroScript.equippedCRIT + heroScript.effectCRIT;
      critical.GetComponent<Text>().text = Helpers.TwoDecimalPlaces(criticalPercentage * 100) + " %";
      if (heroScript.effectCRIT > 0) {
        critical.GetComponent<Text>().color = Colors.effect;
      } else {
        critical.GetComponent<Text>().color = Color.white;
      }
    }

    if (luckPercentage != heroScript.luckPercentage + heroScript.effectLCK) {
      luckPercentage = heroScript.luckPercentage + heroScript.equippedLUCK + heroScript.effectLCK;
      luck.GetComponent<Text>().text = Helpers.TwoDecimalPlaces(luckPercentage * 100) + " %";
      if (heroScript.effectLCK > 0) {
        luck.GetComponent<Text>().color = Colors.effect;
      } else {
        luck.GetComponent<Text>().color = Color.white;
      }
    }
  }

  public void ChangeArea() {
    locationObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.areaImages, GameData.area);
  }

  void UpdateEquipmentPromptButtons() {
    if (bodyEquipmentKey != Hero.bodyEquipment) {
      bodyEquipmentKey = Hero.bodyEquipment;

      if (bodyEquipmentKey == "") {
        bodyButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "body");
        bodyButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        bodyButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, bodyEquipmentKey).thumbnail;
        bodyButton.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, bodyEquipmentKey).name;
      }
    }

    if (arm1EquipmentKey != Hero.arm1Equipment) {
      arm1EquipmentKey = Hero.arm1Equipment;

      if (arm1EquipmentKey == "") {
        arm1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "arm1");
        arm1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        arm1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Hero.projectileEquipment == "" ? Helpers.GetOrException(Objects.regularItems, arm1EquipmentKey).thumbnail : Helpers.GetOrException(Objects.compositePauseImages, arm1EquipmentKey + "-with-" + Hero.projectileEquipment).thumbnail;
        arm1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Hero.projectileEquipment == "" ? Helpers.GetOrException(Objects.regularItems, arm1EquipmentKey).name : Helpers.GetOrException(Objects.compositePauseImages, arm1EquipmentKey + "-with-" + Hero.projectileEquipment).name;
      }
    }

    if (arm2EquipmentKey != Hero.arm2Equipment) {
      arm2EquipmentKey = Hero.arm2Equipment;

      if (arm2EquipmentKey == "") {
        arm2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "arm2");
        arm2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        arm2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Hero.projectileEquipment == "" ? Helpers.GetOrException(Objects.regularItems, arm2EquipmentKey).thumbnail : Helpers.GetOrException(Objects.compositePauseImages, arm2EquipmentKey + "-with-" + Hero.projectileEquipment).thumbnail;
        arm2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Hero.projectileEquipment == "" ? Helpers.GetOrException(Objects.regularItems, arm2EquipmentKey).name : Helpers.GetOrException(Objects.compositePauseImages, arm2EquipmentKey + "-with-" + Hero.projectileEquipment).name;
      }
    }

    if (neckEquipmentKey != Hero.neckEquipment) {
      neckEquipmentKey = Hero.neckEquipment;

      if (neckEquipmentKey == "") {
        neckButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "neck");
        neckButton.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        neckButton.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, neckEquipmentKey).thumbnail;
        neckButton.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, neckEquipmentKey).name;
      }
    }

    if (armwear1EquipmentKey != Hero.armwear1Equipment) {
      armwear1EquipmentKey = Hero.armwear1Equipment;

      if (armwear1EquipmentKey == "") {
        armwear1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "armwear1");
        armwear1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        armwear1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, armwear1EquipmentKey).thumbnail;
        armwear1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, armwear1EquipmentKey).name;
      }
    }

    if (armwear2EquipmentKey != Hero.armwear2Equipment) {
      armwear2EquipmentKey = Hero.armwear2Equipment;

      if (armwear2EquipmentKey == "") {
        armwear2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "armwear2");
        armwear2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        armwear2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, armwear2EquipmentKey).thumbnail;
        armwear2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, armwear2EquipmentKey).name;
      }
    }

    if (ring1EquipmentKey != Hero.ring1Equipment) {
      ring1EquipmentKey = Hero.ring1Equipment;

      if (ring1EquipmentKey == "") {
        ring1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "ring1");
        ring1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        ring1Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, ring1EquipmentKey).thumbnail;
        ring1Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, ring1EquipmentKey).name;
      }
    }

    if (ring2EquipmentKey != Hero.ring2Equipment) {
      ring2EquipmentKey = Hero.ring2Equipment;

      if (ring2EquipmentKey == "") {
        ring2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.equipmentIcons, "ring2");
        ring2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = "None";
      } else {
        ring2Button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Objects.regularItems, ring2EquipmentKey).thumbnail;
        ring2Button.transform.Find("Text").gameObject.GetComponent<Text>().text = Helpers.GetOrException(Objects.regularItems, ring2EquipmentKey).name;
      }
    }
  }

  void UpdateInGameElementSettings() {
    if (showInGameStatuses != Settings.showInGameStatuses) {
      showInGameStatuses = Settings.showInGameStatuses;

      if (!showInGameStatuses) {
        standardStatsCanvas.SetActive(false);
        minifiedStatsCanvas.SetActive(false);

        radioDontShowStatuses.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-checked");
        radioShowStatuses.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-unchecked");
      } else {
        radioDontShowStatuses.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-unchecked");
        radioShowStatuses.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-checked");
      }
    }

    if (statusType != Settings.statusType) {
      statusType = Settings.statusType;

      if (statusType == "standard") {
        standardStatsCanvas.SetActive(true);
        minifiedStatsCanvas.SetActive(false);

        radioStatusStandard.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-checked");
        radioStatusMinified.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-unchecked");
      } else if (statusType == "minified") {
        standardStatsCanvas.SetActive(false);
        minifiedStatsCanvas.SetActive(true);

        radioStatusMinified.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-checked");
        radioStatusStandard.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "radio-unchecked");
      }
    }

    if (showDamage != Settings.showDamage) {
      showDamage = Settings.showDamage;

      if (showDamage) {
        checkboxShowDamage.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-checked");
      } else {
        checkboxShowDamage.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-unchecked");
      }
    }

    if (showItemInfo != Settings.showItemInfo) {
      showItemInfo = Settings.showItemInfo;

      if (showItemInfo) {
        checkboxShowItemInfo.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-checked");
      } else {
        checkboxShowItemInfo.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-unchecked");
      }
    }

    if (playSFX != Settings.playSFX) {
      playSFX = Settings.playSFX;

      if (playSFX) {
        checkboxPlaySFX.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-checked");
      } else {
        checkboxPlaySFX.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-unchecked");
      }
    }

    if (playSoundtrack != Settings.playSoundtrack) {
      playSoundtrack = Settings.playSoundtrack;

      if (playSoundtrack) {
        checkboxPlaySoundtrack.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-checked");
      } else {
        checkboxPlaySoundtrack.transform.Find("Checkmark").gameObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.uiElements, "checkbox-unchecked");
      }
    }
  }

  void UpdateMagicResistances() {
    string currentMagicResistances = "";

    for (int i = 0; i < heroScript.magicResistances.Length; i++) {
      HeroMagicResistance currentMagicResistance = heroScript.magicResistances[i];
      HeroMagicResistance effectMagicResistance = heroScript.effectMagicResistances[i];

      if (currentMagicResistance.frequency >= 1 || effectMagicResistance.frequency >= 1) {
        currentMagicResistances += currentMagicResistance.name + ",";
      }
    }

    if (currentMagicResistances.EndsWith(",")) {
      currentMagicResistances = currentMagicResistances.Remove(currentMagicResistances.Length - 1, 1);
    }

    if (magicResistances != currentMagicResistances) {
      GameObject[] magicResistanceObjects = new GameObject[] {resistance1Object, resistance2Object, resistance3Object, resistance4Object, resistance5Object, resistance6Object, resistance7Object, resistance8Object};
      string[] heroMagicResistances = currentMagicResistances.Split(',');

      if (heroMagicResistances.Length > 0) {
        magicEmptyObject.SetActive(false);
      } else {
        magicEmptyObject.SetActive(true);
      }

      int i = 0;
      foreach (GameObject currMagicResistanceObject in magicResistanceObjects) {
        if (i >= heroMagicResistances.Length || currentMagicResistances == "") {
          currMagicResistanceObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        } else {
          currMagicResistanceObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
          currMagicResistanceObject.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.magicResistances, heroMagicResistances[i]);
        }
        i++;
      }

      magicResistances = currentMagicResistances;
    }
  }

  void ShowGamePadOptions() {
    mainKeysPanel.SetActive(false);
    mainButtonPanel.SetActive(true);
  }

  void ShowKeyboardOptions() {
    mainButtonPanel.SetActive(false);
    mainKeysPanel.SetActive(true);
  }

  public void SetKeyboardAsPreferredInput() {
    Constants.preferredInput = "keyboard";
  }

  public void SetGamepadAsPreferredInput() {
    Constants.preferredInput = "gamepad";
  }

  public void AwaitInput(String key) {
    canPlayDeselect = false;
    canvasStatus = "mapping";
    Helpers.FocusUIElement(null);

    switch(key) {
      case "jump":
        jumpAwaitLabel.SetActive(true);
        break;
      case "atk1":
        atk1AwaitLabel.SetActive(true);
        break;
      case "atk2":
        atk2AwaitLabel.SetActive(true);
        break;
      case "action":
        actionAwaitLabel.SetActive(true);
        break;
      default:
        Debug.Log("Unknown await case: " + key);
        break;
    }

    currentlyMapping = key;
  }

  public void FinishMapping(String keyCode) {
    switch(currentlyMapping) {
      case "jump":
        if (Helpers.IsGamepadKey(keyCode)) {
          if (keyCode == Controls.currentGamepadAttack1) {
            Controls.currentGamepadAttack1 = Controls.currentGamepadJump;
          } else if (keyCode == Controls.currentGamepadAttack2) {
            Controls.currentGamepadAttack2 = Controls.currentGamepadJump;
          } else if (keyCode == Controls.currentGamepadAction) {
            Controls.currentGamepadAction = Controls.currentGamepadJump;
          }

          Controls.currentGamepadJump = keyCode;
        } else {
          if (keyCode == Controls.currentKeyboardAttack1) {
            Controls.currentKeyboardAttack1 = Controls.currentKeyboardJump;
          } else if (keyCode == Controls.currentKeyboardAttack2) {
            Controls.currentKeyboardAttack2 = Controls.currentKeyboardJump;
          } else if (keyCode == Controls.currentKeyboardAction) {
            Controls.currentKeyboardAction = Controls.currentKeyboardJump;
          }

          Controls.currentKeyboardJump = keyCode;
        }

        jumpAwaitLabel.SetActive(false);
        Helpers.FocusUIElement(jumpButton);
        previouslyFocusedButton = jumpButton;
        break;
      case "atk1":
        if (Helpers.IsGamepadKey(keyCode)) {
          if (keyCode == Controls.currentGamepadJump) {
            Controls.currentGamepadJump = Controls.currentGamepadAttack1;
          } else if (keyCode == Controls.currentGamepadAttack2) {
            Controls.currentGamepadAttack2 = Controls.currentGamepadAttack1;
          } else if (keyCode == Controls.currentGamepadAction) {
            Controls.currentGamepadAction = Controls.currentGamepadAttack1;
          }

          Controls.currentGamepadAttack1 = keyCode;
        } else {
          if (keyCode == Controls.currentKeyboardJump) {
            Controls.currentKeyboardJump = Controls.currentKeyboardAttack1;
          } else if (keyCode == Controls.currentKeyboardAttack2) {
            Controls.currentKeyboardAttack2 = Controls.currentKeyboardAttack1;
          } else if (keyCode == Controls.currentKeyboardAction) {
            Controls.currentKeyboardAction = Controls.currentKeyboardAttack1;
          }

          Controls.currentKeyboardAttack1 = keyCode;
        }

        atk1AwaitLabel.SetActive(false);
        Helpers.FocusUIElement(atk1Button);
        previouslyFocusedButton = atk1Button;
        break;
      case "atk2":
        if (Helpers.IsGamepadKey(keyCode)) {
          if (keyCode == Controls.currentGamepadJump) {
            Controls.currentGamepadJump = Controls.currentGamepadAttack2;
          } else if (keyCode == Controls.currentGamepadAttack1) {
            Controls.currentGamepadAttack1 = Controls.currentGamepadAttack2;
          } else if (keyCode == Controls.currentGamepadAction) {
            Controls.currentGamepadAction = Controls.currentGamepadAttack2;
          }

          Controls.currentGamepadAttack2 = keyCode;
        } else {
          if (keyCode == Controls.currentKeyboardJump) {
            Controls.currentKeyboardJump = Controls.currentKeyboardAttack2;
          } else if (keyCode == Controls.currentKeyboardAttack1) {
            Controls.currentKeyboardAttack1 = Controls.currentKeyboardAttack2;
          } else if (keyCode == Controls.currentKeyboardAction) {
            Controls.currentKeyboardAction = Controls.currentKeyboardAttack2;
          }

          Controls.currentKeyboardAttack2 = keyCode;
        }

        atk2AwaitLabel.SetActive(false);
        Helpers.FocusUIElement(atk2Button);
        previouslyFocusedButton = atk2Button;
        break;
      case "action":
        if (Helpers.IsGamepadKey(keyCode)) {
          if (keyCode == Controls.currentGamepadJump) {
            Controls.currentGamepadJump = Controls.currentGamepadAction;
          } else if (keyCode == Controls.currentGamepadAttack1) {
            Controls.currentGamepadAttack1 = Controls.currentGamepadAction;
          } else if (keyCode == Controls.currentGamepadAttack2) {
            Controls.currentGamepadAttack2 = Controls.currentGamepadAction;
          }

          Controls.currentGamepadAction = keyCode;
        } else {
          if (keyCode == Controls.currentKeyboardJump) {
            Controls.currentKeyboardJump = Controls.currentKeyboardAction;
          } else if (keyCode == Controls.currentKeyboardAttack1) {
            Controls.currentKeyboardAttack1 = Controls.currentKeyboardAction;
          } else if (keyCode == Controls.currentKeyboardAttack2) {
            Controls.currentKeyboardAttack2 = Controls.currentKeyboardAction;
          }

          Controls.currentKeyboardAction = keyCode;
        }

        actionAwaitLabel.SetActive(false);
        Helpers.FocusUIElement(actionButton);
        previouslyFocusedButton = actionButton;
        break;
      default:
        Debug.Log("unknown map case: " + currentlyMapping);
        break;
    }

    currentlyMapping = "";
    canvasStatus = "options_controls";
    canPlayDeselect = true;
  }

  public void ProceedToReset() {
    canPlayDeselect = false;
    PlayMenuSound("attention");

    canvasStatus = "options_controls_reset";
    resetRectangle.SetActive(true);
    Helpers.FocusUIElement(resetYes);
    previouslyFocusedButton = resetYes;
    canPlayDeselect = true;
  }

  public void ResetMapping() {
    Controls.currentKeyboardJump = Controls.DEFAULT_KEYBOARD_JUMP;
    Controls.currentKeyboardAttack1 = Controls.DEFAULT_KEYBOARD_ATTACK_1;
    Controls.currentKeyboardAttack2 = Controls.DEFAULT_KEYBOARD_ATTACK_2;

    Controls.currentGamepadJump = Controls.DEFAULT_GAMEPAD_JUMP;
    Controls.currentGamepadAttack1 = Controls.DEFAULT_GAMEPAD_ATTACK_1;
    Controls.currentGamepadAttack2 = Controls.DEFAULT_GAMEPAD_ATTACK_2;

    canvasStatus = "options_controls";
    resetRectangle.SetActive(false);
    Helpers.FocusUIElement(resetButton);
  }

  public void CancelReset() {
    canPlayDeselect = false;
    canvasStatus = "options_controls";
    resetRectangle.SetActive(false);
    Helpers.FocusUIElement(resetButton);
    previouslyFocusedButton = resetButton;
    canPlayDeselect = true;
  }

  public void PopulateEquipmentContainer(string equipmentType) {
    ClearItems(equipmentContainer);
    bool canUnequip =  HasEquipment(equipmentType);

    switch(equipmentType) {
      case "body":
        currentEquipmentItems = Helpers.GetSpecificItemList(Constants.bodyEquipmentTypes, heroScript.items);
      break;
      case "arm1":
      case "arm2":
        currentEquipmentItems = Helpers.GetSpecificItemList(Constants.armEquipmentTypes, heroScript.items, canUnequip, equipmentType);
      break;
      case "neck":
        currentEquipmentItems = Helpers.GetSpecificItemList(Constants.neckEquipmentTypes, heroScript.items, canUnequip, equipmentType);
      break;
      case "armwear1":
      case "armwear2":
        currentEquipmentItems = Helpers.GetSpecificItemList(Constants.armwearEquipmentTypes, heroScript.items, canUnequip, equipmentType);
      break;
      case "ring1":
      case "ring2":
        currentEquipmentItems = Helpers.GetSpecificItemList(Constants.ringEquipmentTypes, heroScript.items, canUnequip, equipmentType);
      break;
      default:
        Debug.Log("Unknown equipmentType: " + equipmentType);
      break;
    }

    if (currentEquipmentItems.Count > 0) {
      PopulateItemsContainer(currentEquipmentItems, equipmentContainer);
    }
  }

  public void SelectEquipment(int selectedIndex) {
    if (currentEquipmentItems.Count > 0) {
      canPlayDeselect = false;
      canvasStatus = "equipment_select";
      currentlyEquippedIndex = selectedIndex;
      previouslySelectedEquipmentButton = eventSystem.currentSelectedGameObject;
      Helpers.FocusUIElement(itemButtons.ElementAt(0));
      previouslyFocusedButton = itemButtons.ElementAt(0);
      canPlayDeselect = true;
      SetEquipmentProspect(0);
    }
  }

  public void HideStatuses() {
    Settings.showInGameStatuses = false;
  }

  public void ShowStatuses() {
    canPlayDeselect = false;
    Settings.showInGameStatuses = true;
    Helpers.FocusUIElement(radioStatusStandard);
    previouslyFocusedButton = radioStatusStandard;
    canPlayDeselect = true;
  }

  public void SetStandardStatus() {
    canPlayDeselect = false;
    Settings.statusType = "standard";
    Helpers.FocusUIElement(radioShowStatuses);
    previouslyFocusedButton = radioShowStatuses;
    canPlayDeselect = true;
  }

  public void SetMinifiedStatus() {
    canPlayDeselect = false;
    Settings.statusType = "minified";
    Helpers.FocusUIElement(radioShowStatuses);
    previouslyFocusedButton = radioShowStatuses;
    canPlayDeselect = true;
  }

  public void ToggleDamage() {
    Settings.showDamage = !showDamage;
  }

  public void ToggleItemInfo() {
    Settings.showItemInfo = !showItemInfo;
  }

  public void PlayMenuSound(string sound) {
    if (Settings.playSFX) {
      audioSource.PlayOneShot(Helpers.GetOrException(Sounds.menuSounds, sound));
    }
  }

  public void ToggleSFXPlay() {
    Settings.playSFX = !playSFX;
  }

  public void ToggleSoundtrackPlay() {
    Settings.playSoundtrack = !playSoundtrack;
  }

  public bool HasEquipment(string equipmentType) {
    int equipmentValue;

    switch (equipmentType) {
      case "body":
        equipmentValue = 0;
      break;
      case "arm1":
        equipmentValue = 1;
      break;
      case "arm2":
        equipmentValue = 2;
      break;
      case "neck":
        equipmentValue = 3;
      break;
      case "armwear1":
        equipmentValue = 4;
      break;
      case "armwear2":
        equipmentValue = 5;
      break;
      case "ring1":
        equipmentValue = 6;
      break;
      case "ring2":
        equipmentValue = 7;
      break;
      default:
        Debug.LogFormat("Unrecognized value for equipment type: {0}. Switching to default", equipmentType);
        equipmentValue = 0;
      break;
    }
    return heroScript.equipmentArray[equipmentValue] != "";
  }
}
