using System;
using TMPro;
using UnityEngine;

public class BarCanvasMini : MonoBehaviour {
  [SerializeField] GameObject hpObject;
  [SerializeField] GameObject mpObject;

  [NonSerialized] int hp = -1;
  [NonSerialized] int hpDifference = 0;
  [NonSerialized] int mp = -1;
  [NonSerialized] int mpDifference = 0;

  void Start() {
    hp = Hero.instance.currentHP;
    mp = Hero.instance.currentMP;

    hpObject.GetComponent<TextMeshProUGUI>().text = hp.ToString();
    mpObject.GetComponent<TextMeshProUGUI>().text = mp.ToString();

    UpdateColor(hpObject, hp, "hp");
    UpdateColor(mpObject, mp, "mp");
  }

  void Update() {
    UpdateHP();
    UpdateMP();
  }

  public void UpdateHP() {
    if (hp != Hero.instance.currentHP && hpDifference == 0) {
      hpDifference = hp - Hero.instance.currentHP;
    }

    if (hpDifference != 0) {
      int difference = hpDifference > 0 ? -1 : 1;

      hpDifference += difference;
      hp += difference;
      hpObject.GetComponent<TextMeshProUGUI>().text = hp.ToString();

      UpdateColor(hpObject, hp, "hp");
    }
  }

  public void UpdateMP() {
    if (mp != Hero.instance.currentMP && mpDifference == 0) {
      mpDifference = mp - Hero.instance.currentMP;
    }

    if (mpDifference != 0) {
      int difference = mpDifference > 0 ? -1 : 1;

      mpDifference += difference;
      mp += difference;
      mpObject.GetComponent<TextMeshProUGUI>().text = mp.ToString();

      UpdateColor(mpObject, mp, "mp");
    }
  }

  public void UpdateColor(GameObject statObject, int stat, string type) {
    float percentage = (float)stat / (type == "hp" ? Hero.instance.maxHP : Hero.instance.maxMP);

    if (percentage == 1) {
        statObject.GetComponent<TextMeshProUGUI>().color = Colors.miniHPFull;
      } else if (percentage > 0.2f) {
        statObject.GetComponent<TextMeshProUGUI>().color = Colors.miniHPNotFull;
      } else {
        statObject.GetComponent<TextMeshProUGUI>().color = Colors.miniHPBelow20;
      }
  }
}
