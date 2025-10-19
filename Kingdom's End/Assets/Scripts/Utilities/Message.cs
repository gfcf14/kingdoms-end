using System.Collections.Generic;

public class Message {
  public static Outcome blankOutcome = new Outcome() {
    outcomeCase = "",
    outcomeValue = ""
  };
  public static Dictionary<string, Dictionary<string, MessageLine[]>> messages = new() {
    {"ME", new() {
      {"001", new MessageLine[] {
        new MessageLine() {
          line = "Test sign please don't remove.",
          outcome = blankOutcome
        }
      }}
    }}
  };
}
