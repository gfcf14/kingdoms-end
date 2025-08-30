using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UserInput : MonoBehaviour {
  private enum KeyState { Down, Held, Up }
  private const float stickDeadzone = 0.1f;

  void Update() {
    // Handle Gamepads (Xbox, DualShock, etc.)
    foreach (var gamepad in Gamepad.all) {
      CheckGamepad(gamepad);
    }

    // Handle generic Joysticks (old USB adapters, flight sticks, etc.)
    foreach (var joystick in Joystick.all) {
      CheckJoystick(joystick);
    }

    // Handle Keyboard (optional)
    if (Keyboard.current != null) {
      if (Keyboard.current.anyKey.wasPressedThisFrame) {
        foreach (var key in Keyboard.current.allKeys) {
          if (key.wasPressedThisFrame) {
            Debug.Log($"[Keyboard] Key Pressed: {key.displayName}");
          }
        }
      }
    }
  }

  void CheckGamepad(Gamepad gamepad) {
    // Left stick
    Vector2 leftStick = gamepad.leftStick.ReadValue();
    if (leftStick.magnitude > stickDeadzone) {
      Debug.Log($"[{gamepad.displayName}] Left Stick: {leftStick}");
    }

    // Right stick
    Vector2 rightStick = gamepad.rightStick.ReadValue();
    if (rightStick.magnitude > stickDeadzone) {
      Debug.Log($"[{gamepad.displayName}] Right Stick: {rightStick}");
    }

    // Buttons
    foreach (var control in gamepad.allControls) {
      if (control is ButtonControl button && button.wasPressedThisFrame) {
        Debug.Log($"[{gamepad.displayName}] Button Pressed: {button.displayName}");
      }
    }
  }

  void CheckJoystick(Joystick joystick) {
    foreach (var control in joystick.allControls) {
      // Axis or button detection
      if (control is AxisControl axis) {
        float value = axis.ReadValue();

        if (Mathf.Abs(value) > stickDeadzone) {
          Debug.Log($"[{joystick.displayName}] Axis: {axis.displayName}, Value: {value}");
        }
      }
      else if (control is ButtonControl button && button.wasPressedThisFrame) {
        Debug.Log($"[{joystick.displayName}] Button Pressed: {button.displayName}");
      }
    }
  }
  
  
  
  public static bool IsKeyHeld(string key) {
    return CheckKeyState(key, KeyState.Held);
  }

  public static bool IsKeyDown(string key) {
    return CheckKeyState(key, KeyState.Down);
  }

  public static bool IsKeyUp(string key) {
    return CheckKeyState(key, KeyState.Up);
  }

  private static bool CheckKeyState(string key, KeyState state) {
    // Check if it's a gamepad key or keyboard key
    if (key.Contains("Joystick") && Gamepad.all.Count > 0) {
      return CheckGamepadButton(key, state);
    } else {
      return CheckKeyboardKey(key, state);
    }
  }

  private static bool CheckKeyboardKey(string key, KeyState state) {
    // 1) If new Input System keyboard exists, try to resolve a KeyControl first
    if (Keyboard.current != null) {
      // Try parse the string directly into the new InputSystem.Key enum
      if (Enum.TryParse<UnityEngine.InputSystem.Key>(key, true, out var parsedKey)) {
        var kc = Keyboard.current[parsedKey];
        if (kc != null) {
          switch (state) {
            case KeyState.Down: return kc.wasPressedThisFrame;
            case KeyState.Held: return kc.isPressed;
            case KeyState.Up:   return kc.wasReleasedThisFrame;
          }
        }
      }

      // If direct parse failed, try to match by control name or displayName (covers alternate names)
      foreach (var control in Keyboard.current.allKeys) {
        // control.name is internal name (e.g. "space"), displayName is user-friendly (e.g. "Space")
        if (string.Equals(control.name, key, StringComparison.OrdinalIgnoreCase)
        || string.Equals(control.displayName, key, StringComparison.OrdinalIgnoreCase)) {
          switch (state) {
            case KeyState.Down: return control.wasPressedThisFrame;
            case KeyState.Held: return control.isPressed;
            case KeyState.Up:   return control.wasReleasedThisFrame;
          }
        }
      }
    }

    // 2) Legacy fallback using KeyCode + Input.GetKey* (keeps backwards compatibility with your strings)
    try {
      var kc = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);
      switch (state) {
        case KeyState.Down: return Input.GetKeyDown(kc);
        case KeyState.Held: return Input.GetKey(kc);
        case KeyState.Up:   return Input.GetKeyUp(kc);
      }
    } catch (Exception) {
      // Parsing failed (unknown key string) — return false instead of throwing
      return false;
    }

    return false;
  }

  private static bool CheckGamepadButton(string key, KeyState state) {
    foreach (var gamepad in Gamepad.all) {
      // Example: key = "JoystickButton2" → map to gamepad.buttonSouth
      var button = MapGamepadButton(key, gamepad);
      if (button == null) continue;

      switch (state) {
        case KeyState.Down: if (button.wasPressedThisFrame) return true; break;
        case KeyState.Held: if (button.isPressed) return true; break;
        case KeyState.Up: if (button.wasReleasedThisFrame) return true; break;
      }
    }

    return false;
  }

  private static ButtonControl MapGamepadButton(string key, Gamepad gamepad) {
    // Basic Xbox mapping
    return key switch {
      "JoystickButton0" => gamepad.buttonSouth,
      "JoystickButton1" => gamepad.buttonEast,
      "JoystickButton2" => gamepad.buttonWest,
      "JoystickButton3" => gamepad.buttonNorth,
      "JoystickButton6" => gamepad.startButton,
      "JoystickButton7" => gamepad.selectButton,
      _ => null
    };
  }
}
