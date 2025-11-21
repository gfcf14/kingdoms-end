using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UserInput : MonoBehaviour {
  // Cache for mapping results to avoid repeated lookups
  private static readonly Dictionary<(string, string), ButtonControl> _buttonCache = new();

  void Update() {
    // disables pausing/mapping when hero is autonomus or involved in a chat decision
    if (!Hero.instance.isAutonomous && !InGame.instance.IsMakingDecision()) {
      // Customize gamepad/keyboard buttons
      if (Hero.instance.isPaused && Pause.currentlyMapping != "") {
        var input = DetectInputForRemapping();
        if (input != null) {
          if (Hero.instance.canMap) {
            if (!input.isForbidden) {
              var deviceName = input.deviceName.ToLower();

              if (deviceName.Contains("keyboard")) deviceName = "keyboard";
              else if (deviceName.Contains("xbox")) deviceName = "xbox";
              else if (deviceName.Contains("playstation") || deviceName.Contains("dualshock") || deviceName.Contains("dualsense")) deviceName = "playstation";
              else if (deviceName.Contains("usb") || deviceName.Contains("joystick")) deviceName = "usb gamepad";

              var controlAction = StringToControlAction(Pause.currentlyMapping);

              UpdateMapping(Helpers.GetOrException(Controls.currentControlMappings, deviceName), controlAction, input.keyCode);
              Hero.instance.canMap = false;
            }
          } else {
            Hero.instance.canMap = true;
          }
        }
      }

      // Resume game
      if (IsPauseKeyUp() && Hero.instance.pauseCase == "" && Pause.currentlyMapping == "") {
        Hero.instance.ResumeGame();
      }

      // Perform back on pause UI
      if (IsBackKeyDown() && Hero.instance.isPaused) {
        // do not perform "BACK" if a key is being mapped
        if (Pause.canvasStatus != "mapping") {
          if (Pause.canvasStatus == "main") {
            Hero.instance.ResumeGame();
          } else {
            InGame.instance.pauseCanvas.GetComponent<Pause>().PerformBack();
          }
        }
      }

      // Perform back on shop UI
      if (IsBackKeyDown() && Hero.instance.pauseCase == "shopping") {
        if (Shop.canvasStatus == "action" && InGame.instance.IsShopReady()) {
          InGame.instance.CloseShop();
        } else {
          InGame.instance.shopCanvas.GetComponent<Shop>().PerformBack();
        }
      }
    }
  }

  // Gets the mapping to which the device corresponds per key press
  private static Dictionary<ControlActions, string> GetDeviceMapping(string deviceKey) {
    if (Controls.currentControlMappings.TryGetValue(deviceKey, out var mapping)) return mapping;

    return null;
  }

  public static string GetActiveGamepadKey() {
    if (Gamepad.all.Count > 0) {
      foreach (var gamepad in Gamepad.all) {
        string name = gamepad.displayName.ToLower();

        if (name.Contains("xbox")) return "xbox";
        if (name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense")) name = "playstation";
        if (name.Contains("usb") || name.Contains("joystick")) return "usb gamepad";
      }
    }

    if (Joystick.all.Count > 0) return "usb gamepad";

    return null;
  }

  // checks if an action has occurred
  public static bool IsAction(ControlActions action, KeyState state) {
    // Check keyboard mapping first
    var keyboardMapping = GetDeviceMapping("keyboard");
    if (keyboardMapping != null && keyboardMapping.TryGetValue(action, out var keyboardKey)) {
      if (CheckKeyState(keyboardKey, state)) return true;
    }

    // Check gamepad mapping
    var gamepadKey = GetActiveGamepadKey();
    if (!string.IsNullOrEmpty(gamepadKey)) {
      var gamepadMapping = GetDeviceMapping(gamepadKey);
      if (gamepadMapping != null && gamepadMapping.TryGetValue(action, out var mappedKey)) {
        if (CheckKeyState(mappedKey, state)) return true;
      }
    }

    return false;
  }

  private static bool IsGamepadControlName(string key) {
    return key switch {
      // New Input System "proper" gamepad names
      "buttonSouth" or "buttonEast" or "buttonWest" or "buttonNorth" or
      "leftShoulder" or "rightShoulder" or "leftTrigger" or "rightTrigger" or
      "startButton" or "selectButton" or "leftStick" or "rightStick" => true,

      // Legacy USB joystick-style names
      "button0" or "button1" or "button2" or "button3" or "button4" or
      "button5" or "button6" or "button7" or "button8" or "button9" or
      "trigger" or "Button 1" or "Button 2" or "Button 3" or "Button 4" or
      "Button 5" or "Button 6" or "Button 7" or "Button 8" or "Button 9" or
      "Trigger" => true,

      _ => false
    };
  }


  private static bool CheckKeyState(string key, KeyState state) {
    if ((Gamepad.all.Count > 0 || Joystick.all.Count > 0) && IsGamepadControlName(key)) {
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

    return false;
  }

  private static bool CheckButtonState(ButtonControl button, KeyState state) {
    return state switch {
      KeyState.Down => button.wasPressedThisFrame,
      KeyState.Held => button.isPressed,
      KeyState.Up => button.wasReleasedThisFrame,
      _ => false
    };
  }

  private static bool CheckGamepadButton(string key, KeyState state) {
    // First: modern Gamepad
    foreach (var gamepad in Gamepad.all) {
      var button = MapGamepadButton(key, gamepad);
      if (button != null && CheckButtonState(button, state)) {
        return true;
      }
    }

    // Second: legacy Joystick
    foreach (var joystick in Joystick.all) {
      var button = key switch {
          "button0" => joystick["button0"],
          "button1" => joystick["button1"],
          "button2" => joystick["button2"],
          "button3" => joystick["button3"],
          "button4" => joystick["button4"],
          "button5" => joystick["button5"],
          "button6" => joystick["button6"],
          "button7" => joystick["button7"],
          "button8" => joystick["button8"],
          "button9" => joystick["button9"],
          "trigger" => joystick.trigger,
          "Button 1" => joystick["Button 1"],
          "Button 2" => joystick["Button 2"],
          "Button 3" => joystick["Button 3"],
          "Button 4" => joystick["Button 4"],
          "Button 5" => joystick["Button 5"],
          "Button 6" => joystick["Button 6"],
          "Button 7" => joystick["Button 7"],
          "Button 8" => joystick["Button 8"],
          "Button 9" => joystick["Button 9"],
          "Trigger" => joystick.trigger,
          _ => null
      };

      if (button != null && CheckButtonState(button as ButtonControl, state)) {
        return true;
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

    if (name.Contains("xbox") || name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense")) {
      button = key switch {
        "buttonSouth"   => gamepad.buttonSouth,
        "buttonEast"    => gamepad.buttonEast,
        "buttonWest"    => gamepad.buttonWest,
        "buttonNorth"   => gamepad.buttonNorth,
        "leftShoulder"  => gamepad.leftShoulder,
        "rightShoulder" => gamepad.rightShoulder,
        "leftTrigger"   => gamepad.leftTrigger,
        "rightTrigger"  => gamepad.rightTrigger,
        "startButton"   => gamepad.startButton,
        "selectButton"  => gamepad.selectButton,
        "leftStick"     => gamepad.leftStickButton,
        "rightStick"    => gamepad.rightStickButton,
        _ => null
      };
    }

    _buttonCache[cacheKey] = button;
    return button;
  }

  public static bool IsPauseKeyUp() {
    // Keyboard first
    if (Keyboard.current != null && (Keyboard.current.escapeKey.wasReleasedThisFrame || Keyboard.current.pKey.wasReleasedThisFrame)) {
      return true;
    }

    // Gamepad check
    foreach (var gamepad in Gamepad.all) {
      string name = gamepad.displayName.ToLower();

      if (name.Contains("xbox") && gamepad.startButton.wasReleasedThisFrame) return true;
      if ((name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense")) && gamepad.startButton.wasReleasedThisFrame) return true;
      if (name.Contains("usb gamepad") && gamepad.startButton.wasReleasedThisFrame) return true;
    }

    // Joystick check
    foreach (var joystick in Joystick.all) {
      var startButton = joystick.TryGetChildControl<ButtonControl>(Controls.GAMEPAD_START_BUTTON);

      if (startButton != null && startButton.wasReleasedThisFrame) return true;
    }

    return false;
  }

  public static bool IsBackKeyDown() {
    if (Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame) return true;

    // Gamepad check
    foreach (var gamepad in Gamepad.all) {
      string name = gamepad.displayName.ToLower();

      if (name.Contains("xbox") && gamepad.buttonNorth.wasPressedThisFrame) return true;
      if ((name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense")) && gamepad.buttonNorth.wasPressedThisFrame) return true;
      if (name.Contains("usb gamepad") && gamepad.buttonNorth.wasPressedThisFrame) return true;
    }

    // Joystick check
    foreach (var joystick in Joystick.all) {
      foreach (var control in joystick.allControls) {
        if (control is ButtonControl button && button.wasPressedThisFrame) {
          if (button.name.Equals(Controls.GAMEPAD_BACK_BUTTON, StringComparison.OrdinalIgnoreCase)) {
            // Debug.Log($"[BackCheck] MATCH: {button.name} == {Controls.GAMEPAD_BACK_BUTTON}");
            return true; // treat as Back
          // } else {
            // Debug.Log($"[BackCheck] Other button pressed: {button.name}, Path: {button.path}");
          }
        }
      }
    }

    return false;
  }

  public static bool IsStartKeyDown() {
    // Keyboard first
    if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame) return true;

    // Gamepad check
    foreach (var gamepad in Gamepad.all) {
      string name = gamepad.displayName.ToLower();

      if (name.Contains("xbox") && gamepad.startButton.wasPressedThisFrame) return true;
      if ((name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense")) && gamepad.startButton.wasPressedThisFrame) return true;
      if (name.Contains("usb gamepad") && gamepad.startButton.wasPressedThisFrame) return true;
    }

    // Joystick check
    foreach (var joystick in Joystick.all) {
      var startButton = joystick.TryGetChildControl<ButtonControl>(Controls.GAMEPAD_START_BUTTON);

      if (startButton != null && startButton.wasPressedThisFrame) return true;
    }

    return false;
  }

  public static bool IsForbiddenToRemap(string keyCode, string deviceName) {
    if (keyCode.Contains("mouse", StringComparison.OrdinalIgnoreCase)) return true;

    var deviceKey = Controls.forbiddenKeys.Keys.FirstOrDefault(k => deviceName.ToLower().Contains(k));

    if (!string.IsNullOrEmpty(deviceKey)) {
      return Controls.forbiddenKeys[deviceKey].Any(fk => keyCode.Equals(fk, StringComparison.OrdinalIgnoreCase));
    }

    return false;
  }

  private static RemapInput BuildRemapInput(InputControl control, string deviceName) {
    string keyCode = control.name;
    bool isGamepad = deviceName.ToLower().Contains("xbox") || deviceName.ToLower().Contains("playstation") || deviceName.ToLower().Contains("gamepad");
    bool isForbidden = IsForbiddenToRemap(keyCode, deviceName);

    return new RemapInput {
      keyCode = keyCode,
      deviceName = deviceName,
      isGamepad = isGamepad,
      isForbidden = isForbidden
    };
  }

  public static RemapInput DetectInputForRemapping() {
    // Check gamepads
    foreach (var gamepad in Gamepad.all) {
      foreach (var control in gamepad.allControls) {
        if (control is ButtonControl button && button.wasPressedThisFrame)
          return BuildRemapInput(control, gamepad.displayName);
      }
    }

    // Check joysticks (legacy USB gamepads)
    foreach (var joystick in Joystick.all) {
      foreach (var control in joystick.allControls) {
        if (control is ButtonControl button && button.wasPressedThisFrame)
          return BuildRemapInput(control, joystick.displayName);
      }
    }

    // Check keyboard
    if (Keyboard.current != null) {
      foreach (var key in Keyboard.current.allKeys) {
        if (key.wasPressedThisFrame)
          return BuildRemapInput(key, "Keyboard");
      }
    }

    // Check mouse
    if (Mouse.current != null) {
      if (Mouse.current.leftButton.wasPressedThisFrame)
        return BuildRemapInput(Mouse.current.leftButton, "Mouse");
      if (Mouse.current.rightButton.wasPressedThisFrame)
        return BuildRemapInput(Mouse.current.rightButton, "Mouse");
    }

    return null; // No input detected
  }

  public static void UpdateMapping(Dictionary<ControlActions, string> mapping, ControlActions action, string newKey) {
    // Check if newKey is already mapped to another action
    var existingAction = mapping.FirstOrDefault(kv => kv.Value == newKey).Key;

    if (!existingAction.Equals(action)) { // swap
      mapping[existingAction] = mapping[action];
    }

    mapping[action] = newKey;
    InGame.instance.pauseCanvas.GetComponent<Pause>().FinishMapping();
  }

  public static ControlActions StringToControlAction(string actionName) {
    return actionName.ToLower() switch {
      "jump" => ControlActions.Jump,
      "atk1" => ControlActions.Attack1,
      "atk2" => ControlActions.Attack2,
      "action" => ControlActions.Action,
      _ => throw new ArgumentException($"Unknown action name: {actionName}")
    };
  }
}
