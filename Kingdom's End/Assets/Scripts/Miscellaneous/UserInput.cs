using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UserInput : MonoBehaviour {
  private const float stickDeadzone = 0.1f;

  // Cache for mapping results to avoid repeated lookups
  private static readonly Dictionary<(string, string), ButtonControl> _buttonCache = new();

  void Update() {}

  // Gets the mapping to which the gamepad corresponds per key press
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

  // checks if an action has occurred
  public static bool IsAction(ControlActions action, KeyState state) {
    var mapping = GetCurrentGamepadMapping();

    // Check keyboard first
    string keyboardKey = action switch {
      ControlActions.Jump => Controls.currentKeyboardJump,
      ControlActions.Attack1 => Controls.currentKeyboardAttack1,
      ControlActions.Attack2 => Controls.currentKeyboardAttack2,
      ControlActions.Action => Controls.currentKeyboardAction,
      _ => null
    };

    //  if a keyboard key was pressed
    if (keyboardKey != null && CheckKeyState(keyboardKey, state)) return true;

    // If a gamepad/joystick key was pressed and is within the respective dictionary
    if (mapping != null && mapping.TryGetValue(action, out var gamepadKey)) {
      if (CheckKeyState(gamepadKey, state)) return true;
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
      // checks a key pressed using the new Input System
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

      // fallback to the old input system, i.e. loop all keys and see which matches
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

    // last fallback using Input.GetKey
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

      // checks if a key is pressed based on what was found from the mapping in the cache
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

    if (gamepad.buttonSouth.isPressed) pressedButtons.Add("JoystickButton0 (buttonSouth / A / X)");
    if (gamepad.buttonEast.isPressed) pressedButtons.Add("JoystickButton1 (buttonEast / B / Circle)");
    if (gamepad.buttonWest.isPressed) pressedButtons.Add("JoystickButton2 (buttonWest / X / Square)");
    if (gamepad.buttonNorth.isPressed) pressedButtons.Add("JoystickButton3 (buttonNorth / Y / Triangle)");
    if (gamepad.leftShoulder.isPressed) pressedButtons.Add("JoystickButton4 (leftShoulder / L1)");
    if (gamepad.rightShoulder.isPressed) pressedButtons.Add("JoystickButton5 (rightShoulder / R1)");
    if (gamepad.leftTrigger.isPressed) pressedButtons.Add("JoystickButton6 (leftTrigger / L2)");
    if (gamepad.rightTrigger.isPressed) pressedButtons.Add("JoystickButton7 (rightTrigger / R2)");
    if (gamepad.startButton.isPressed) pressedButtons.Add("JoystickButton8 (start / Options)");
    if (gamepad.selectButton.isPressed) pressedButtons.Add("JoystickButton9 (select / Back)");

    if (pressedButtons.Count > 0) {
      Debug.Log($"[Debug] Device: {gamepad.displayName} | Currently Pressed: {string.Join(", ", pressedButtons)}");
    }
}

    return false;
  }

  // Maps legacy "JoystickButtonX" names to InputSystem Gamepad buttons and uses cache to avoid repeated string comparisons.
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
        "JoystickButton4" => gamepad.leftShoulder,  // Left Shoulder/Bumper
        "JoystickButton5" => gamepad.rightShoulder, // Right Shoulder/Bumper
        "JoystickButton6" => gamepad.selectButton,
        "JoystickButton7" => gamepad.startButton,
        _ => null
      };
    } else if (name.Contains("usb gamepad")) {
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
    } else {
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
