using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// NOTE: Keep in mind the functions here correspond from a non-player point of view. As such GiveItem = give item to player, and TakeItem = take item from player
public class ChatCanvas : MonoBehaviour {
  [SerializeField] GameObject characterObject;
  [SerializeField] GameObject textObject;
  [SerializeField] GameObject continuePrompt;
  [SerializeField] GameObject decisionPrompt;
  [SerializeField] GameObject decisionFirstSelected;
  [SerializeField] public ChatLine[] chatLines;
  [SerializeField] public MessageLine[] messageLines;
  [SerializeField] public string messageOriginator;
  [SerializeField] public string startingNPC;
  [SerializeField] public string currentNode;
  [SerializeField] public string nextNode;
  [SerializeField] float textSpeed;

  private int lineIndex;
  private Text characterComponent;
  private Text textComponent;
  [SerializeField] public static EventSystem chatEventSystem;
  private bool hasDecision = false;

  void Read() {
    RunOutcome(messageLines[lineIndex].outcome);
    ClearText();
  }
  void ShowChat() {
    SetCharacter(chatLines[lineIndex].character);
    SetEmotion(chatLines[lineIndex].character, chatLines[lineIndex].emotion);
    RunOutcome(chatLines[lineIndex].outcome);
    ClearText();
  }

  void Start() {
    chatEventSystem = EventSystem.current;
    characterComponent = characterObject.GetComponent<Text>();
    textComponent = textObject.GetComponent<Text>();

    if (chatLines != null) {
      StartChat();
    } else if (messageLines != null) {
      StartMessage();
    }
  }
  void Update() {
    if (UserInput.IsAction(ControlActions.Action, KeyState.Up)) {
      if (decisionPrompt.activeSelf) {
        // selects either YES or NO
        ExecuteEvents.Execute(chatEventSystem.currentSelectedGameObject, new BaseEventData(chatEventSystem), ExecuteEvents.submitHandler);

      // if the entire text is on screen, get the next line
      } else if (chatLines != null && textComponent.text == chatLines[lineIndex].line) {
        NextChatLine();
      } else if (messageLines != null && textComponent.text == messageLines[lineIndex].line) {
        NextMessageLine();
      } else { // otherwise, show the entire line right away
        StopAllCoroutines();

        if (chatLines != null) {
          textComponent.text = chatLines[lineIndex].line;
        } else if (messageLines != null) {
          textComponent.text = messageLines[lineIndex].line;
        }

        DecidePromptDisplay();
      }
    }
  }

  void DecidePromptDisplay() {
    // if the current line has a decision field, it should be shown instead
    if (hasDecision) {
      decisionPrompt.SetActive(true);
      chatEventSystem.SetSelectedGameObject(decisionFirstSelected);
    } else {
      continuePrompt.SetActive(true);
    }
  }

  public void StartMessage() {
    if (characterComponent != null) {
      characterComponent.text = $"{messageOriginator}:";
    }

    lineIndex = 0;
    Read();
    if (textComponent != null) {
      StartCoroutine(ShowMessageLine());
    }
  }

  public void StartChat() {
    if (characterComponent != null) {
      lineIndex = 0;
      ShowChat();
      if (textComponent != null) {
        StartCoroutine(ShowChatLine());
      }
    }
  }

  // adds a line character by character based on the textSpeed
  IEnumerator ShowMessageLine() {
    foreach (char c in messageLines[lineIndex].line.ToCharArray()) {
      textComponent.text += c;
      if(textComponent.text.Length == messageLines[lineIndex].line.Length) {
        continuePrompt.SetActive(true);
      } else {
        continuePrompt.SetActive(false);
      }
      yield return new WaitForSeconds(textSpeed);
    }
  }

  // adds a line character by character based on the textSpeed
  IEnumerator ShowChatLine() {
    // prior to showing the chat line we check if there is a decision, so when it's done showing it shows that instead of the continue prompt
    hasDecision = chatLines[lineIndex].decision != null;

    foreach (char c in chatLines[lineIndex].line.ToCharArray()) {
      textComponent.text += c;
      if(textComponent.text.Length == chatLines[lineIndex].line.Length) {
        DecidePromptDisplay();
      } else {
        continuePrompt.SetActive(false);
      }
      yield return new WaitForSeconds(textSpeed);
    }
  }

  void ClearText() {
    if (textComponent != null) {
      textComponent.text = "";
    }
  }

  void SetCharacter(string character) {
    if (characterComponent != null) {
      characterComponent.text = Helpers.KebabToCharacter(character) + ":";
    }
  }

  void SetEmotion(string character, string emotion) {
    GameObject.Find(Helpers.KebabToObject(character)).GetComponent<SpriteRenderer>().sprite = Helpers.GetOrException(Helpers.GetOrException(Sprites.emotions, character), emotion);
  }

  // Gives item(s) to the player
  void GiveItem(string itemKey) {
    InGame.instance.PickItem(itemKey);
  }

  // Takes item(s) from the player
  void TakeItem(string itemKey) {
    if (itemKey.Contains("money")) { // if there is money involved, remove from the gold value
      string moneyValue = itemKey.Split('-')[1];
      Hero.instance.gold -= int.Parse(moneyValue);

      InGame.instance.InstantiateLoss("money-loss", isItem: false, moneyValue, null);
    } else { // if there is no money involved, remove from the hero item list
      string[] itemAndCount = itemKey.Split('@');
      string itemToRemove = itemAndCount[0];
      int itemToRemoveAmount = itemAndCount.Length > 1 ? int.Parse(itemAndCount[1]) : 1;

      Item currItem = Helpers.GetItemFromList(Hero.instance.items, itemToRemove);

      if (currItem.amount > itemToRemoveAmount) { // if more than the amount, simply subtract
        currItem.amount -= itemToRemoveAmount;
      } else { // otherwise, remove it from the item list
        Hero.instance.RemoveItem(Helpers.GetItemIndex(Hero.instance.items, itemToRemove));
      }

      InGame.instance.InstantiateLoss("item-loss", isItem: true, "", Helpers.GetOrException(Objects.regularItems, itemToRemove).thumbnail, itemToRemoveAmount);
    }
  }

  private IEnumerator TakeItemsCoroutine(string[] heroItems) {
    foreach (string item in heroItems) {
      TakeItem(item);
      yield return new WaitForSeconds(0.375f);
    }
  }

  public void RunOutcome(Outcome outcome) {
    switch (outcome.outcomeCase) {
      case "":
        // do nothing
      return;
      case "give":
        string[] itemKeys = outcome.outcomeValue.Split(',');

        foreach (string itemKey in itemKeys) {
          GiveItem(itemKey);
        }
      break;
      case "trade":
        string[] outcomeValues = outcome.outcomeValue.Split('|'); // splits the outcome value by | in two, where the left part is what the hero gives, and the right side is what the NPC gives
        string[] heroItems = outcomeValues[0].Split(',');
        string[] npcItems = outcomeValues[1] != "" ? outcomeValues[1].Split(',') : new string[] { };

        StartCoroutine(TakeItemsCoroutine(heroItems));

        foreach(string item in npcItems) {
          GiveItem(item);
        }
      break;
      default:
        Debug.Log("Unknown outcome case: case=" + outcome.outcomeCase + "value=" + outcome.outcomeValue);
        break;
    }
  }

  void NextMessageLine() {
    if (lineIndex < messageLines.Length - 1) {
      lineIndex++;
      Read();
      StartCoroutine(ShowMessageLine());
    } else { // if there are no more lines, hide the chat window
      FinishMessage(playerLeft: false);
    }
  }

  void NextChatLine() {
    hasDecision = false;

    if (lineIndex < chatLines.Length - 1) {
      lineIndex++;
      ShowChat();
      StartCoroutine(ShowChatLine());
    } else { // if there are no more lines, hide the chat window
      FinishChat(playerLeft: false);
    }
  }

  // Finishes the chat, but only sets the next node if the player left, otherwise a line could potentially not be read by player
  public void FinishChat(bool playerLeft = false) {
    if (!playerLeft) {
      Hero.instance.UpdateChatNode(startingNPC, nextNode);
    } else {
      // TODO: consider if it's a good idea to have a variable chat to set when the player leaves without finishing the chat. Then maybe the NPC, when talked to again, can reply like "You know, it's rude to leave when someone talks to you."
      ChatNode currentChatNode = Helpers.GetOrException(Helpers.GetOrException(Chat.chatNodes, startingNPC), currentNode);

      if (currentChatNode.continueOnLeave) {
        Hero.instance.UpdateChatNode(startingNPC, nextNode);
      }
    }

    SetEmotion(startingNPC, "default");
    chatLines = null;
    Hero.instance.CloseChat();
  }

  public void FinishMessage(bool playerLeft = false) {
    messageOriginator = "";
    messageLines = null;
    Hero.instance.CloseChat();
  }

  public void SetDecision(string decision) {
    decisionPrompt.SetActive(false);
    string decisionNode = chatLines[lineIndex].decision.Get(decision);

    // TODO: include support for actions (e.g. open shop)

    Hero.instance.UpdateChatNode(startingNPC, decisionNode);
    Hero.instance.OpenChat();
  }

  public void HideDecisionPrompt() {
    decisionPrompt.SetActive(false);
  }

  public bool IsOnDecision() {
    return decisionPrompt.activeSelf;
  }
}
