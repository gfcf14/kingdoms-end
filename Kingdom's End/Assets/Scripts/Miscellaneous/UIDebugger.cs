using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIDebugger : MonoBehaviour {
  [SerializeField] GameObject eventSystemObject;
  private InputSystemUIInputModule module;
  private EventSystem eventSystem;

  void Awake() {
    if (eventSystemObject != null) {
      module = eventSystemObject.GetComponent<InputSystemUIInputModule>();
      eventSystem = eventSystemObject.GetComponent<EventSystem>();
    } else if (EventSystem.current != null) {
      module = EventSystem.current.GetComponent<InputSystemUIInputModule>();
    }
  }
  void Start() {
    // FOR DEBUG PURPOSES ONLY
    // if (module != null) {
    //   Debug.Log("[UI Debugger] Found InputSystemUIInputModule! Dumping UI action bindings...");

    //   if (module.submit?.action != null) DumpActionBindings("Submit", module.submit.action);
    //   if (module.cancel?.action != null) DumpActionBindings("Cancel", module.cancel.action);
    //   if (module.point?.action != null) DumpActionBindings("Point", module.point.action);
    //   if (module.move?.action != null) DumpActionBindings("Navigate", module.move.action);
    //   if (module.leftClick?.action != null) DumpActionBindings("Click", module.leftClick.action);
    //   if (module.rightClick?.action != null) DumpActionBindings("RightClick", module.rightClick.action);
    //   if (module.middleClick?.action != null) DumpActionBindings("MiddleClick", module.middleClick.action);
    //   if (module.scrollWheel?.action != null) DumpActionBindings("ScrollWheel", module.scrollWheel.action);
    //   if (module.trackedDevicePosition?.action != null) DumpActionBindings("TrackedDevicePosition", module.trackedDevicePosition.action);
    //   if (module.trackedDeviceOrientation?.action != null) DumpActionBindings("TrackedDeviceOrientation", module.trackedDeviceOrientation.action);
    // } else {
    //   Debug.LogWarning("[UI Debugger] No InputSystemUIInputModule found on EventSystem.");
    // }
  }

  void Update() {
    #if UNITY_WEBGL && !UNITY_EDITOR
      // 🎯 Explicit fallback check for USB gamepad WebGL Action Button in WebGL
      foreach (var joystick in Joystick.all) {
        var webglActionButton = joystick.TryGetChildControl<ButtonControl>(Controls.WEBGL_ACTION_BUTTON);
        if (webglActionButton != null && webglActionButton.wasPressedThisFrame) {
          Debug.Log("[UI] Fallback: USB Gamepad WebGL Action Button pressed — forcing Submit!");

          ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
      }
    #endif
  }

  private void DumpActionBindings(string label, InputAction action) {
    if (action == null) {
      Debug.Log($"[UI Action] {label}: <null>");
      return;
    }

    Debug.Log($"[UI Action] {label}: {action.name}, {action.bindings.Count} binding(s)");

    for (int i = 0; i < action.bindings.Count; i++) {
      var binding = action.bindings[i];
      Debug.Log($"   • Binding {i}: {binding.path}, groups={binding.groups}, override={binding.overridePath}");
    }
  }
}
