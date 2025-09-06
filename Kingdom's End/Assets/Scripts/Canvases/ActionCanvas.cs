using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionCanvas : MonoBehaviour {
  [SerializeField] GameObject buttonImage;
  [SerializeField] GameObject actionText;
  [SerializeField] GameObject actionTextContainer;
  [SerializeField] public string text = "";
  [SerializeField] public string icon = "";

  private Animator anim;
  private RectTransform actionTextRect;
  private RectTransform actionTextContainerRect;
  private Text textComponent;

  private string currentPreferredInput = "";

  void Start() {
    SetComponents();
    SetIcon();
  }

  void Update() {
    if (currentPreferredInput != Constants.preferredInput) {
      SetIcon();
    } else if (currentPreferredInput == Constants.preferredInput) {
      if (currentPreferredInput == "keyboard" && icon != Helpers.GetOrException(Helpers.GetOrException(Controls.currentControlMappings, "keyboard"), ControlActions.Action)) {
        SetIcon();
      } else { // current preferred input is gamepad
        var currentGamepad = GetCurrentGamepad();

        if (icon != Helpers.GetOrException(Helpers.GetOrException(Controls.currentControlMappings, currentGamepad), ControlActions.Action)) {
          SetIcon(currentGamepad);
        }
      }
    }
  }

  public void SetAction(string action) {
    if (!actionTextRect || !actionTextContainerRect || !textComponent) {
      SetComponents();
    }

    textComponent.text = action.ToUpper();
    anim.Play(action);
  }

  public void ClearAction() {
    textComponent.text = "";
    anim.Play("idle");
  }

  public void SetComponents() {
    anim = GetComponent<Animator>();
    actionTextRect = actionText.GetComponent<RectTransform>();
    actionTextContainerRect = actionTextContainer.GetComponent<RectTransform>();
    textComponent = actionText.GetComponent<Text>();
  }

  public string GetCurrentGamepad() {
    var currentGamepad = UserInput.GetActiveGamepadKey();
    if (currentGamepad == null) {
      currentGamepad = "usb gamepad";
    }

    return currentGamepad;
  }

  public void SetIcon(string gamepad = "usb gamepad") {
    currentPreferredInput = Constants.preferredInput;

    if (currentPreferredInput == "keyboard") {
      icon = Helpers.GetOrException(Helpers.GetOrException(Controls.currentControlMappings, "keyboard"), ControlActions.Action);
      buttonImage.GetComponent<Image>().sprite = Helpers.GetOrException(Sprites.keycodeSprites, icon);
    } else { // gamepads
      icon = Helpers.GetOrException(Helpers.GetOrException(Controls.currentControlMappings, gamepad), ControlActions.Action);
      buttonImage.GetComponent<Image>().sprite = Helpers.GetOrException(Helpers.GetOrException(Sprites.gamepadSprites, gamepad), icon);
    }
  }
}
