using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoss : MonoBehaviour {
  public bool alignRight = false;
  public bool isItem = false;
  public bool singleItem = true;
  public string multiplierText = "";
  public Sprite itemLossImage;
  void Start() {
    Transform wrapper = transform.Find("Wrapper");
    RectTransform wrapperRect = wrapper.GetComponent<RectTransform>();

    if (alignRight) {
      wrapper.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
      wrapperRect.anchoredPosition = new Vector2(wrapperRect.anchoredPosition.x + (isItem ? 2.2f : 1.7f), wrapperRect.anchoredPosition.y);
    }

    if (isItem) {
      wrapper.Find("Image").GetComponent<SpriteRenderer>().sprite = itemLossImage;
      wrapperRect.sizeDelta = new Vector2(singleItem ? 0.57f : 0.87f, 0.6f);
      wrapperRect.anchoredPosition = new Vector2(wrapperRect.anchoredPosition.x + (alignRight ? -0.75f : 0.75f), wrapperRect.anchoredPosition.y);
    }

    wrapper.Find("Multiplier").GetComponent<TextMeshPro>().text = multiplierText;
  }

  void Update() {}

  public void DestroyItemLoss() {
    Destroy(gameObject);
  }
}
