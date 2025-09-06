using System.Collections.Generic;
using System.Linq;

public static class Controls {
  // DO NOT CHANGE OR RISK THE POSSIBLITY OF UNIVERSE IMPLOSION
  public const string DEFAULT_KEYBOARD_JUMP = "Space";
  public const string DEFAULT_KEYBOARD_ATTACK_1 = "Keypad4";
  public const string DEFAULT_KEYBOARD_ATTACK_2 = "Keypad6";
  public const string DEFAULT_KEYBOARD_ACTION = "Keypad7";

  public const string DEFAULT_GAMEPAD_JUMP = "button3";
  public const string DEFAULT_GAMEPAD_ATTACK_1 = "button4";
  public const string DEFAULT_GAMEPAD_ATTACK_2 = "button2";
  public const string DEFAULT_GAMEPAD_ACTION = "button7";

  public const string DEFAULT_XBOX_JUMP = "buttonSouth";
  public const string DEFAULT_XBOX_ATTACK_1 = "buttonWest";
  public const string DEFAULT_XBOX_ATTACK_2 = "buttonEast";
  public const string DEFAULT_XBOX_ACTION = "leftShoulder";

  // variables that define keys to use in game. To be changed only in pause menu
  public static Dictionary<string, Dictionary<ControlActions, string>> defaultControlMappings = new() {
    {"keyboard", new() {
      {ControlActions.Action, DEFAULT_KEYBOARD_ACTION},
      {ControlActions.Attack1, DEFAULT_KEYBOARD_ATTACK_1},
      {ControlActions.Attack2, DEFAULT_KEYBOARD_ATTACK_2},
      {ControlActions.Jump, DEFAULT_KEYBOARD_JUMP},
    }},
    {"usb gamepad", new() {
      {ControlActions.Action, DEFAULT_GAMEPAD_ACTION},
      {ControlActions.Attack1, DEFAULT_GAMEPAD_ATTACK_1},
      {ControlActions.Attack2, DEFAULT_GAMEPAD_ATTACK_2},
      {ControlActions.Jump, DEFAULT_GAMEPAD_JUMP},
    }},
    {"xbox", new() {
      {ControlActions.Action, DEFAULT_XBOX_ACTION},
      {ControlActions.Attack1, DEFAULT_XBOX_ATTACK_1},
      {ControlActions.Attack2, DEFAULT_XBOX_ATTACK_2},
      {ControlActions.Jump, DEFAULT_XBOX_JUMP},
    }},
  };

  public static Dictionary<string, Dictionary<ControlActions, string>> currentControlMappings = defaultControlMappings.ToDictionary(
    outer => outer.Key,
    outer => outer.Value.ToDictionary(inner => inner.Key, inner => inner.Value)
  );

  // Forbidden keys grouped by device type
  public static Dictionary<string, string[]> forbiddenKeys = new() {
    {"keyboard", new[] {"escape", "enter", "return", "w", "a", "s", "d", "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "p"}},
    {"xbox", new[] {"start", "select", "menu"}},
    {"playstation", new[] {"options", "touchpad"}},
    {"usb gamepad", new[] {"button9"}}
  };
}
