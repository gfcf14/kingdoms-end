using System;
using UnityEngine.UI;
using UnityEngine;

public class BarsCanvas : MonoBehaviour {
  [SerializeField] GameObject hpBackground;
  [SerializeField] GameObject hpForeground;
  [SerializeField] GameObject hpBar;

  [SerializeField] GameObject mpBackground;
  [SerializeField] GameObject mpForeground;
  [SerializeField] GameObject mpBar;

  [Header("MP Display Container")]
  [SerializeField] GameObject mpPanelBackground;
  [SerializeField] GameObject mpPanelForeground;
  [SerializeField] GameObject mpBarContainer;
  [SerializeField] bool mpBarDisplaying = false;

  [NonSerialized] int currentHPWidth = -1;
  [NonSerialized] int currentMPWidth = -1;
  [NonSerialized] int maxHPWidth = -1;
  [NonSerialized] int maxMPWidth = -1;

  void Start() {}
  void Update() {
    CheckMPBarDisplay();

    UpdateHPContainer();
    UpdateHPBar();

    UpdateMPContainer();
    UpdateMPBar();
  }

  public void CheckMPBarDisplay() {
    if (Hero.instance.canCastMagic && !mpBarDisplaying) {
      DisplayMPBar();
      mpBarDisplaying = true;
    }
  }

  public void UpdateHPContainer() {
    if (maxHPWidth != Hero.instance.maxHP) {
      maxHPWidth = Hero.instance.maxHP;

      // obtains a dimension that is dependent on the hp value
      Vector2 containerDimension = new Vector2((maxHPWidth > Constants.maxHPDisplayableLimit ? Constants.maxHPDisplayableLimit : maxHPWidth) * Constants.containerMultiplier, 36);
      containerDimension.x -= Constants.hpAdjustDifference;

      hpBackground.GetComponent<RectTransform>().sizeDelta = containerDimension;
      hpForeground.GetComponent<RectTransform>().sizeDelta = containerDimension;
    }
  }

  public void UpdateHPBar() {
    if (currentHPWidth != Hero.instance.currentHP) {
      currentHPWidth = Hero.instance.currentHP;
      float calculatedHPDisplay = maxHPWidth > Constants.maxHPDisplayableLimit ? Constants.maxHPDisplayableLimit * ((float)currentHPWidth / (float)maxHPWidth) : currentHPWidth;
      Vector2 hpDisplayVector = new Vector2(calculatedHPDisplay, 27);
      hpDisplayVector.x *= Constants.containerMultiplier;
      hpBar.GetComponent<RectTransform>().sizeDelta = hpDisplayVector;

      float healthPercentage = (float)Hero.instance.currentHP / (float)Hero.instance.maxHP;

      if (healthPercentage >= 0.4f) {
        hpBar.GetComponent<Image>().color = Colors.barHPAbove40;
      } else if (healthPercentage > 0.2f) {
        hpBar.GetComponent<Image>().color = Colors.barHPAbove20;
      } else {
        hpBar.GetComponent<Image>().color = Colors.barHPBelow20;
      }
    }
  }

  public void UpdateMPContainer() {
    if (maxMPWidth != Hero.instance.maxMP) {
      maxMPWidth = Hero.instance.maxMP;

      // obtains a dimension that is dependent on the mp value
      Vector2 containerDimension = new Vector2((maxMPWidth > Constants.maxMPDisplayableLimit ? Constants.maxMPDisplayableLimit : maxMPWidth) * Constants.containerMultiplier, 16);
      containerDimension.x -= Constants.mpAdjustDifference;

      mpBackground.GetComponent<RectTransform>().sizeDelta = containerDimension;
      mpForeground.GetComponent<RectTransform>().sizeDelta = containerDimension;
    }
  }

  public void UpdateMPBar() {
    if (currentMPWidth != Hero.instance.currentMP) {
      currentMPWidth = Hero.instance.currentMP;
      float calculatedMPDisplay = maxMPWidth > Constants.maxMPDisplayableLimit ? Constants.maxMPDisplayableLimit * ((float)currentMPWidth / (float)maxMPWidth) : currentMPWidth;
      Vector2 mpDisplayVector = new Vector2(calculatedMPDisplay, 9);
      mpDisplayVector.x *= Constants.containerMultiplier;
      mpBar.GetComponent<RectTransform>().sizeDelta = mpDisplayVector;
    }
  }

  // The bar has to be displayed with alpha 1 to simulate it shows
  public void DisplayMPBar() {
    GameObject[] MPDisplayContainers = {mpPanelBackground, mpPanelForeground, mpBarContainer};
    foreach(GameObject currMPDisplayContainer in MPDisplayContainers) {
      foreach(Transform child in currMPDisplayContainer.transform) {
        Color currColor = child.GetComponent<Image>().color;
        Color newColor = new Color(currColor.r, currColor.g, currColor.b, 1);
        child.GetComponent<Image>().color = newColor;
      }
    }
  }
}
