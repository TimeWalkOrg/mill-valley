# Known Bugs & Issues

Last updated: 2026-05-17 (Waves 1 & 2 implemented)

---

## Fixed

### BUG-001: SetDay() set `nowIsDay = false` (FIXED)
**File**: `Assets/Scripts/timeWalkDayNightToggle.cs`, line 78  
**Symptom**: Calling `SetDay()` applied day visuals but internally marked the scene as night. On the next `ToggleDayNight()` call, the visuals would immediately jump to night instead of toggling.  
**Fix**: Changed `nowIsDay = false` → `nowIsDay = true` in `SetDay()`.  
**Fixed**: 2026-05-17

---

## Active Bugs

### BUG-002: `Application.LoadLevel` deprecated — FIXED 2026-05-17
**File**: `Assets/Scripts/TimeWalkControls1920.cs`, line 47  
**Fix applied**: Replaced with `SceneManager.LoadScene(0, LoadSceneMode.Single)`, added `using UnityEngine.SceneManagement;`

### BUG-003: `OnLevelWasLoaded` deprecated — FIXED 2026-05-17
**File**: `Assets/Scripts/Fading.cs`  
**Fix applied**: Replaced with `OnEnable`/`OnDisable`/`OnSceneChanged` pattern using `SceneManager.activeSceneChanged` event.

### BUG-004: `Debug.Log()` in `Update()` — FIXED 2026-05-17
**File**: `Assets/Scripts/DayNightStart.cs`  
**Fix applied**: Deleted the `Debug.Log(percentageOfDay)` line.

---

## Non-Functional Features

### FEATURE-001: WebView building info panels — PARTIALLY RESTORED 2026-05-17
**Status**: Functional via system browser  
**Fix applied**: Replaced blank panel with `Application.OpenURL(url)` in both `FirstPersonUIComponent.cs` and `WebViewTriggerComponent.cs`. Entering a building trigger now opens the historical info URL in the user's system browser. The in-game panel is no longer shown.  
**Remaining option**: Purchase ZenFulcrum EmbeddedBrowser for in-game embedded browser experience.

### FEATURE-002: VR Mode
**Status**: Code wiring complete — package not yet installed  
**Remaining step**: Install `com.unity.xr.oculus` via **Window → Package Manager → Add package by name**, then enable Oculus in **Edit → Project Settings → XR Plug-in Management**.  
**What was done**: `ControlManager.IsVR` and `EnableTestingControlType()` now call `XRDeviceUtil.isPresent()` instead of hardcoded `false`. `LoadingManager` VR button and `EnableVR()` call are now live. VR will activate automatically once the package is installed and a headset is connected.  
**See**: `DOCS/VR_MIGRATION_GUIDE.md` for full re-enablement steps.

### FEATURE-003: Oculus Spatial Audio
**Status**: Non-functional  
**Reason**: OSPNative scripts reference native .so/.dylib files from removed OVR SDK.  
**User impact**: Audio plays as standard Unity 3D audio (no head-tracked spatialization).

---

## Warnings (Non-Breaking)

| Warning | File | Notes |
|---------|------|-------|
| `XRDevice.isPresent is obsolete` | ControlManager.cs:129 (commented) | Replaced by XRDeviceUtil but not yet called |
| `OCULUS_SDK scripting define set but SDK not present` | ProjectSettings | Leftover define symbol; harmless |
| MegaWireEditor.cs: `Handles.DotCap` obsolete | MegaWireEditor.cs:13 | Editor-only, no gameplay impact |

---

## Performance Notes

- `FindObjectOfType<>()` called multiple times in `ControlManager.cs:272` and `LoadingManager.cs:245`. For a scene this size it is not a problem, but caching the result would be cleaner.
- GUI-based fade (`CameraFade.cs`, `Fading.cs`) is less efficient than a dedicated `CanvasGroup` fade. Not a bottleneck at current scene complexity.
- No object pooling for WebView spawning. Since it happens infrequently on player entry, this is acceptable.
