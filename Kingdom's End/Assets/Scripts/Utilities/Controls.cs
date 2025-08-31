using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controls {
  // DO NOT CHANGE OR RISK THE POSSIBLITY OF UNIVERSE IMPLOSION
  public const string DEFAULT_KEYBOARD_JUMP = "Space";
  public const string DEFAULT_KEYBOARD_ATTACK_1 = "Keypad4";
  public const string DEFAULT_KEYBOARD_ATTACK_2 = "Keypad6";
  public const string DEFAULT_KEYBOARD_ACTION = "Keypad7";

  public const string DEFAULT_GAMEPAD_JUMP = "JoystickButton2";
  public const string DEFAULT_GAMEPAD_ATTACK_1 = "JoystickButton3";
  public const string DEFAULT_GAMEPAD_ATTACK_2 = "JoystickButton1";
  public const string DEFAULT_GAMEPAD_ACTION = "JoystickButton6";

  public const string DEFAULT_XBOX_JUMP = "JoystickButton0";
  public const string DEFAULT_XBOX_ATTACK_1 = "JoystickButton2";
  public const string DEFAULT_XBOX_ATTACK_2 = "JoystickButton1";
  public const string DEFAULT_XBOX_ACTION = "Left Bumper";

  // variables that define keys to use in game. To be changed only in pause menu
  public static string currentKeyboardJump = DEFAULT_KEYBOARD_JUMP;
  public static string currentKeyboardAttack1 = DEFAULT_KEYBOARD_ATTACK_1;
  public static string currentKeyboardAttack2 = DEFAULT_KEYBOARD_ATTACK_2;
  public static string currentKeyboardAction = DEFAULT_KEYBOARD_ACTION;

  public static string currentGamepadJump = DEFAULT_GAMEPAD_JUMP;
  public static string currentGamepadAttack1 = DEFAULT_GAMEPAD_ATTACK_1;
  public static string currentGamepadAttack2 = DEFAULT_GAMEPAD_ATTACK_2;
  public static string currentGamepadAction = DEFAULT_GAMEPAD_ACTION;

  public static Dictionary<ControlActions, string> currentGamepadMapping = new Dictionary<ControlActions, string>() {
    {ControlActions.Action, DEFAULT_GAMEPAD_ACTION},
    {ControlActions.Attack1, DEFAULT_GAMEPAD_ATTACK_1},
    {ControlActions.Attack2, DEFAULT_GAMEPAD_ATTACK_2},
    {ControlActions.Jump, DEFAULT_GAMEPAD_JUMP},
  };

  public static Dictionary<ControlActions, string> currentXboxMapping = new Dictionary<ControlActions, string>() {
    {ControlActions.Action, DEFAULT_XBOX_ACTION},
    {ControlActions.Attack1, DEFAULT_XBOX_ATTACK_1},
    {ControlActions.Attack2, DEFAULT_XBOX_ATTACK_2},
    {ControlActions.Jump, DEFAULT_XBOX_JUMP},
  };
}
