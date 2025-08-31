using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UserInput : MonoBehaviour {
  private enum KeyState { Down, Held, Up }
  private const float stickDeadzone = 0.1f;

  // Cache for mapping results to avoid repeated lookups
  private static readonly Dictionary<(string, string), ButtonControl> _buttonCache = new();

  void Update() {}

  private static Dictionary<ControlActions, string> GetCurrentGamepadMapping() {
    // First, check if any Gamepads exist
    if (Gamepad.all.Count > 0) {
      foreach (var gamepad in Gamepad.all) {
        string name = gamepad.displayName.ToLower();

        if (name.Contains("xbox")) return Controls.currentXboxMapping;
        if (name.Contains("usb gamepad")) return Controls.currentGamepadMapping;
      }
    }

    // If no Gamepad found, check for Joysticks
    if (Joystick.all.Count > 0) {
      foreach (var joystick in Joystick.all) {
        return Controls.currentGamepadMapping;
      }
    }

    return null;
  }

  public static bool IsActionUp(ControlActions action) {
    var mapping = GetCurrentGamepadMapping();

    // Check keyboard first
    string keyboardKey = action switch {
      ControlActions.Jump => Controls.currentKeyboardJump,
      ControlActions.Attack1 => Controls.currentKeyboardAttack1,
      ControlActions.Attack2 => Controls.currentKeyboardAttack2,
      ControlActions.Action => Controls.currentKeyboardAction,
      _ => null
    };
    if (keyboardKey != null && IsKeyUp(keyboardKey)) return true;

    // Check gamepad mapping
    if (mapping != null && mapping.TryGetValue(action, out var gamepadKey)) {
      if (IsKeyUp(gamepadKey)) return true;
    }
    return false;
  }

  public static bool IsKeyHeld(string key) => CheckKeyState(key, KeyState.Held);
  public static bool IsKeyDown(string key) => CheckKeyState(key, KeyState.Down);
  public static bool IsKeyUp(string key) => CheckKeyState(key, KeyState.Up);

  private static bool CheckKeyState(string key, KeyState state) {
    if (key.Contains("Joystick") && Gamepad.all.Count > 0) {
      return CheckGamepadButton(key, state);
    } else {
      return CheckKeyboardKey(key, state);
    }
  }

  private static bool CheckKeyboardKey(string key, KeyState state) {
    if (Keyboard.current != null) {
      if (Enum.TryParse<UnityEngine.InputSystem.Key>(key, true, out var parsedKey)) {
        var kc = Keyboard.current[parsedKey];
        if (kc != null) {
          return state switch {
            KeyState.Down => kc.wasPressedThisFrame,
            KeyState.Held => kc.isPressed,
            KeyState.Up => kc.wasReleasedThisFrame,
            _ => false
          };
        }
      }
      foreach (var control in Keyboard.current.allKeys) {
        if (string.Equals(control.name, key, StringComparison.OrdinalIgnoreCase)
          || string.Equals(control.displayName, key, StringComparison.OrdinalIgnoreCase)) {
          return state switch {
            KeyState.Down => control.wasPressedThisFrame,
            KeyState.Held => control.isPressed,
            KeyState.Up => control.wasReleasedThisFrame,
            _ => false
          };
        }
      }
    }
    try {
      var kc = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);
      return state switch {
        KeyState.Down => Input.GetKeyDown(kc),
        KeyState.Held => Input.GetKey(kc),
        KeyState.Up => Input.GetKeyUp(kc),
        _ => false
      };
    } catch (Exception) {
      return false;
    }
  }

  private static bool CheckGamepadButton(string key, KeyState state) {
    foreach (var gamepad in Gamepad.all) {
      var button = MapGamepadButton(key, gamepad);
      if (button == null) continue;

      bool result = state switch {
        KeyState.Down => button.wasPressedThisFrame,
        KeyState.Held => button.isPressed,
        KeyState.Up => button.wasReleasedThisFrame,
        _ => false
      };

      if (result) {
        Debug.Log($"[Match] Device: {gamepad.displayName}, Expected Key: {key}, State: {state}");
        return true;
      }
    }

    // ** If we didn't find the expected key, log what buttons are currently pressed **
    foreach (var gamepad in Gamepad.all) {
      List<string> pressedButtons = new List<string>();
      if (gamepad.buttonSouth.isPressed) pressedButtons.Add("buttonSouth (A/X)");
      if (gamepad.buttonNorth.isPressed) pressedButtons.Add("buttonNorth (Y/Triangle)");
      if (gamepad.buttonWest.isPressed) pressedButtons.Add("buttonWest (X/Square)");
      if (gamepad.buttonEast.isPressed) pressedButtons.Add("buttonEast (B/Circle)");
      if (gamepad.leftShoulder.isPressed) pressedButtons.Add("leftShoulder (L1)");
      if (gamepad.rightShoulder.isPressed) pressedButtons.Add("rightShoulder (R1)");
      if (gamepad.leftTrigger.isPressed) pressedButtons.Add("leftTrigger (L2)");
      if (gamepad.rightTrigger.isPressed) pressedButtons.Add("rightTrigger (R2)");
      if (gamepad.startButton.isPressed) pressedButtons.Add("start");
      if (gamepad.selectButton.isPressed) pressedButtons.Add("select");

      if (pressedButtons.Count > 0) {
        Debug.Log($"[Debug] Device: {gamepad.displayName} | Currently Pressed: {string.Join(", ", pressedButtons)}");
      }
    }

    return false;
  }

  private static ButtonControl MapGamepadButton(string key, Gamepad gamepad) {
    var cacheKey = (key, gamepad.displayName);
    if (_buttonCache.TryGetValue(cacheKey, out var cached)) return cached;

    string name = gamepad.displayName.ToLower();
    ButtonControl button = null;

    if (name.Contains("xbox")) {
      button = key switch {
        "JoystickButton0" => gamepad.buttonSouth,   // A
        "JoystickButton1" => gamepad.buttonEast,    // B
        "JoystickButton2" => gamepad.buttonWest,    // X
        "JoystickButton3" => gamepad.buttonNorth,   // Y
        "JoystickButton6" => gamepad.startButton,
        "JoystickButton7" => gamepad.selectButton,
        _ => null
      };
    }
    else if (name.Contains("usb gamepad")) {
      button = key switch {
        "JoystickButton4" => gamepad.buttonWest,        // Square
        "JoystickButton2" => gamepad.buttonEast,        // Circle
        "JoystickButton3" => gamepad.buttonSouth,       // X
        "JoystickButton7" => gamepad.leftShoulder,      // L1
        "JoystickButton5" => gamepad.leftTrigger,       // L2
        "JoystickButton8" => gamepad.rightShoulder,     // R1
        "JoystickButton6" => gamepad.rightTrigger,      // R2
        _ => null
      };
    }
    else {
      button = key switch {
        "JoystickButton0" => gamepad.buttonSouth,
        "JoystickButton1" => gamepad.buttonEast,
        "JoystickButton2" => gamepad.buttonWest,
        "JoystickButton3" => gamepad.buttonNorth,
        "JoystickButton6" => gamepad.startButton,
        "JoystickButton7" => gamepad.selectButton,
        _ => null
      };
    }

    _buttonCache[cacheKey] = button;
    return button;
  }
}
