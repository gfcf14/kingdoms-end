using System.Collections.Generic;

public class Chat {
  public static Condition blankCondition = new Condition() {
    conditionCheck = "",
    conditionValue = ""
  };

  public static Outcome blankOutcome = new Outcome() {
    outcomeCase = "",
    outcomeValue = ""
  };

  public static Dictionary<string, Dictionary<string, ChatNode>> chatNodes = new() {
    {"peasant-girl", new() {
      {"", new ChatNode() {
        nodeCondition = blankCondition,
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "default",
            line: "Hi! Nice to meet ya! I'm a peasant girl! Here! Have a Pineapple and a Watermelon!",
            outcome: new Outcome() {
              outcomeCase = "give",
              outcomeValue = "pineapple,watermelon"
            }
          )
        },
        // nextNode = "emotion-dialogue",
        // nextNode = "pay-dialogue",
        nextNode = "fruit-salad-dialogue",
        fallbackNode = ""
      }},
      {"fruit-salad-dialogue", new ChatNode() {
        nodeCondition = new Condition() {
          conditionCheck = "resources",
          conditionValue = "apple@3,strawberry@8,banana@2,pineapple,mango@2,honeydew,money-50"
        },
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "happy",
            line: "Thanks! I'll save you a helping once it's ready!",
            outcome: new Outcome() {
              outcomeCase = "trade",
              outcomeValue = "apple@3,strawberry@8,banana@2,pineapple,mango@2,honeydew,money-50|"
            }
          )
        },
        nextNode = "pay-dialogue",
        fallbackNode = "no-fruit-salad-dialogue"
      }},
      {"pay-dialogue", new ChatNode() {
        nodeCondition = new Condition() {
          conditionCheck = "money",
          conditionValue = "1000"
        },
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "happy",
            line: "Thank you! Here's an elixir",
            outcome: new Outcome() {
              outcomeCase = "trade",
              outcomeValue = "money-1000|elixir"
            }
          )
        },
        nextNode = "emotion-dialogue",
        fallbackNode = "no-pay-dialogue"
      }},
      {"no-pay-dialogue", new ChatNode() {
        nodeCondition = blankCondition,
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "default",
            line: "If you bring me $1000 I'll give you something special",
            outcome: blankOutcome
          )
        },
        nextNode = "pay-dialogue",
        fallbackNode = ""
      }},
      {"no-fruit-salad-dialogue", new ChatNode() {
        nodeCondition = blankCondition,
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "default",
            line: "I need to make a fruit salad. Do you think you could bring me 3 apples, 8 strawberries, 2 bananas, one pineapple, 2 mangoes, and 1 honeydew? Oh, and I'd need $50 to buy some honey for it.",
            outcome: blankOutcome
          )
        },
        nextNode = "fruit-salad-dialogue",
        fallbackNode = ""
      }},
      {"emotion-dialogue", new ChatNode() {
        nodeCondition = new Condition() {
          conditionCheck = "items",
          conditionValue = "draco-shield"
        },
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "happy",
            line: "Hi again! I'm a happy peasant girl!!",
            outcome: new Outcome() {
              outcomeCase = "trade",
              outcomeValue = "draco-shield|money-5000"
            }
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "confused",
            line: "I'm not sure if I'm ever going to be given a name...",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "sad",
            line: "It would make me sad if I become just another random NPC,",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "lonely",
            line: "Heck, in fact, it would make me feel very lonely...",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "ashamed",
            line: "That's not anachronical, is it? I'd be embarrassed if it were!",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "afraid",
            line: "I fear that if I fall out of line, I'd be deleted...",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "angry",
            line: "Why should I care? The game will miss out if they don't have me!!!",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "default",
            line: "But enough about that. What should I tell you about myself?",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "surprised",
            line: "I think there's plans to make me a bigger character! Will it be moral support? Comic relief?",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "affective",
            line: "I would looove to be involved in some form of romance, not sure if with the main character...",
            outcome: blankOutcome
          ),
          new ChatLine(
            character: "peasant-girl",
            emotion: "jealous",
            line: "I just hope with all my heart that I'm not part of some form of love triangle!",
            outcome: new Outcome() {
              outcomeCase = "give",
              outcomeValue = "apple"
            }
          )
        },
        nextNode = "emotion-dialogue",
        fallbackNode = "ask-draco-shield"
      }},
      {"ask-draco-shield", new ChatNode() {
        nodeCondition = blankCondition,
        nodeLines = new ChatLine[] {
          new ChatLine(
            character: "peasant-girl",
            emotion: "default",
            line: "If you bring me the Draco Shield I'll show you my array of emotions...",
            outcome: blankOutcome
          )
        },
        nextNode = "emotion-dialogue",
        fallbackNode = ""
      }},
    }}
  };
}
