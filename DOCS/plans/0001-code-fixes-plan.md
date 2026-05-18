# Plan 0001 — Code Fixes & Feature Restoration

**Created**: 2026-05-17  
**Status**: Wave 1 ✅ Wave 2 ✅ Wave 3 ✅ (code complete; com.unity.xr.oculus package install still manual)  
**Total estimated effort**: ~75 minutes (Waves 1–2 no prerequisites; Wave 3 requires VR hardware)

---

## Scope

All code-level changes identified during the Unity 2020.3.48f1 migration audit:

1. **Wave 1** — Console & deprecation fixes (BUG-002, BUG-003, BUG-004)
2. **Wave 2** — WebView → `Application.OpenURL` (FEATURE-001 partial restoration)
3. **Wave 3** — VR re-enablement code wiring (FEATURE-002, requires `com.unity.xr.oculus` installed first)

---

## Dependency Graph

```
Wave 1 (console fixes)
    │
    └──► Wave 2 (WebView → OpenURL)   ← independent of Wave 1, but do in order
              │
              └──► Wave 3 (VR wiring) ← requires com.unity.xr.oculus package installed
```

Waves 1 and 2 are independent of each other but should be done before Wave 3 to keep the codebase clean.

---

## Wave 1: Console & Deprecation Fixes

**Goal**: Eliminate all known compile warnings and console spam from deprecated API usage.  
**Depends on**: none  
**Estimated effort**: 20 minutes  

---

### T-001: Remove `Debug.Log` from `DayNightStart.Update()`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/DayNightStart.cs`

**What to build**:  
Delete the `Debug.Log(percentageOfDay)` call on line 23. This fires every frame (~60×/sec), spamming the Console and measurably increasing Editor overhead. The value it logs is already visible in the Inspector when the object is selected.

**Exact change**:
```csharp
// REMOVE this line (DayNightStart.cs:23):
Debug.Log(percentageOfDay);
```

**Acceptance criteria**:
- [ ] Console shows no `percentageOfDay` log spam during Play mode
- [ ] Script still compiles with 0 errors
- [ ] Sun rotation and day/night light cycle still work in Play mode

---

### T-002: Replace deprecated `Application.LoadLevel` in `TimeWalkControls1920.cs`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/TimeWalkControls1920.cs`

**What to build**:  
Add `using UnityEngine.SceneManagement;` and replace `Application.LoadLevel(0)` with `SceneManager.LoadScene(0, LoadSceneMode.Single)`. The `Application.LoadLevel` API was removed in Unity 2019 and generates a compiler warning in 2020.

**Exact change**:
```csharp
// ADD at top (after existing using statements):
using UnityEngine.SceneManagement;

// REPLACE (line 43):
Application.LoadLevel(0);
// WITH:
SceneManager.LoadScene(0, LoadSceneMode.Single);
```

**Note**: `TimeWalkControls1920.cs` appears unused in the current scene setup (no GameObject references it in LoadingScene or MainScene). The fix is still worth making to keep the codebase warning-free.

**Acceptance criteria**:
- [ ] No `CS0618 Application.LoadLevel` warning in Console
- [ ] Script compiles with 0 errors
- [ ] Pressing R in Play mode still triggers a scene reload (if the script is active)

---

### T-003: Replace deprecated `OnLevelWasLoaded` in `Fading.cs`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/Fading.cs`

**What to build**:  
`OnLevelWasLoaded()` is never called in Unity 2020+ (the Unity warning visible at bottom of screen confirms this). Replace it with a `SceneManager.activeSceneChanged` event subscription so the fade-in triggers correctly after scene transitions. Add `using UnityEngine.SceneManagement;`.

**Exact change**:
```csharp
// ADD at top:
using UnityEngine.SceneManagement;

// ADD these two methods (replace the old OnLevelWasLoaded method):
void OnEnable()
{
    SceneManager.activeSceneChanged += OnSceneChanged;
}

void OnDisable()
{
    SceneManager.activeSceneChanged -= OnSceneChanged;
}

void OnSceneChanged(Scene oldScene, Scene newScene)
{
    BeginFade(-1);
}

// REMOVE:
void OnLevelWasLoaded()
{
    BeginFade(-1);
}
```

**Acceptance criteria**:
- [ ] The `OnLevelWasLoaded was found on Fading` warning no longer appears in Console
- [ ] Script compiles with 0 errors
- [ ] Fade-in effect still triggers when transitioning between scenes

---

### Wave 1 Validation Gate

Run in Unity Editor:

1. Open **Window → General → Console**, clear it
2. Enter Play mode (LoadingScene)
3. Verify:
   - [ ] No `percentageOfDay` log spam
   - [ ] No `CS0618 Application.LoadLevel` warning
   - [ ] No `OnLevelWasLoaded was found on Fading` warning
   - [ ] Fade-in still plays when the main scene loads
   - [ ] Zero compile errors

---

## Wave 2: WebView → `Application.OpenURL`

**Goal**: Restore building info panel functionality using the system browser instead of the missing ZenFulcrum plugin.  
**Depends on**: Wave 1 (clean codebase — recommended but not strictly required)  
**Estimated effort**: 20 minutes  

**Background**: When the player walks into a building trigger zone, `WebViewTriggerComponent.OnTriggerEnter()` fires `SendWebViewMissive(thisURL)`. `FirstPersonUIComponent.OnWebView()` receives this missive and currently hides the game UI and shows an empty `webViewUIGO` panel (because `browser.Url = missive.url` is commented out). The fix: call `Application.OpenURL(missive.url)` and keep the game UI visible — the player stays in-game while the historical page opens in their system browser.

---

### T-004: Open system browser in `FirstPersonUIComponent.OnWebView()`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/Components/FirstPersonUIComponent.cs`

**What to build**:  
In `OnWebView()`, when `missive.url != ""`, call `Application.OpenURL(missive.url)` and do **not** switch the UI to `webViewUIGO` (which is blank). The player remains in the game world with the game HUD visible. The browser opens in the background on their desktop.

**Exact change**:
```csharp
private void OnWebView(WebViewMissive missive)
{
    if (missive.url != "")
    {
        Application.OpenURL(missive.url);
        // Game UI stays active — player remains in world while browser opens
    }
    // Empty url = close signal; no action needed (game UI already showing)
}
```

Remove the `gameUIGO.SetActive(false)`, `webViewUIGO.SetActive(true)`, and `ToggleFPSControl(false)` lines from the `if` branch. Remove the `else` branch that re-shows gameUI (it's always showing now).

**Acceptance criteria**:
- [ ] Walking into a building trigger zone opens the timewalk.org URL in the system browser
- [ ] Game HUD remains visible; player is not locked out of movement
- [ ] No blank panel appears
- [ ] Exiting the trigger zone causes no errors

---

### T-005: Open system browser for VR path in `WebViewTriggerComponent`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/Components/WebViewTriggerComponent.cs`

**What to build**:  
In `OnTriggerEnter()`, the VRPlayer branch currently only enables `vRWebViewGO` (a 3D in-world panel) and would set the browser URL (commented out). Since the VR browser is unavailable, add `Application.OpenURL(thisURL)` so VR players also get the content. The `vRWebViewGO.SetActive` call can be removed since there's nothing to display there.

**Exact change** (inside `OnTriggerEnter`, VRPlayer branch):
```csharp
if (other.tag == "VRPlayer")
{
    if (!string.IsNullOrEmpty(thisURL))
        Application.OpenURL(thisURL);
}
```

Remove the `EnableWebView()`, `LookAt`, and `Quaternion.Euler` lines — those positioned a now-nonexistent in-world browser panel.

**Acceptance criteria**:
- [ ] VRPlayer trigger opens system browser (testable by temporarily tagging the FPS controller as "VRPlayer")
- [ ] No null-ref errors if `thisURL` is empty
- [ ] No compile errors

---

### Wave 2 Validation Gate

1. Enter Play mode, walk into a building trigger zone
2. Verify:
   - [ ] System browser opens with a timewalk.org URL
   - [ ] Game HUD stays visible — no blank panel replaces it
   - [ ] Player movement not locked
   - [ ] Console shows no new errors
3. Walk away from the trigger zone:
   - [ ] No errors on exit

---

## Wave 3: VR Re-enablement Code Wiring

**Goal**: Replace all hardcoded `false`/`if(false)` VR guards with live `XRDeviceUtil.isPresent()` checks so VR activates automatically when a headset is detected.  
**Depends on**: Wave 1 + Wave 2 complete; **com.unity.xr.oculus package installed** (manual step required before this wave)  
**Estimated effort**: 20 minutes of code + manual Package Manager step  

### Prerequisite (manual — not a code change)

Before starting this wave:
1. In Unity: **Window → Package Manager → + → Add package by name**
2. Enter `com.unity.xr.oculus` and install
3. Then: **Edit → Project Settings → XR Plug-in Management**
4. Check **Oculus** under both the PC and Android tabs

Without this package, `XRDeviceUtil.isPresent()` will always return `false` (no XR subsystem registered), so the code changes are safe to make beforehand but won't activate VR until the package is installed.

---

### T-006: Wire `XRDeviceUtil` into `ControlManager.cs`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/Managers/ControlManager.cs`

**Change 1** — Replace hardcoded `IsVR` property (line 127):
```csharp
// BEFORE:
public bool IsVR { get { return (false); } }

// AFTER:
public bool IsVR { get { return XRDeviceUtil.isPresent(); } }
```

**Change 2** — Replace hardcoded FPS in `EnableTestingControlType()` (line 423):
```csharp
// BEFORE:
currentControlType = ControlType.FPS;

// AFTER:
if (currentControlType == ControlType.VR && !XRDeviceUtil.isPresent())
    currentControlType = ControlType.FPS;
```

**Acceptance criteria**:
- [ ] `IsVR` returns `false` when no headset connected (FPS mode activates as normal)
- [ ] `IsVR` returns `true` when Oculus headset connected and `com.unity.xr.oculus` installed
- [ ] Zero compile errors

---

### T-007: Wire `XRDeviceUtil` into `LoadingManager.cs`

**Type**: impl  
**depends_on**: none  
**blocks**: none  
**Target file**: `Assets/Scripts/Managers/LoadingManager.cs`

**Change 1** — VR button visibility (line 87):
```csharp
// BEFORE:
controllerVRButtonUIGO.SetActive(false);

// AFTER:
controllerVRButtonUIGO.SetActive(XRDeviceUtil.isPresent());
```

**Change 2** — VR auto-launch guard (line 146):
```csharp
// BEFORE:
if (false)

// AFTER:
if (XRDeviceUtil.isPresent())
```

**Acceptance criteria**:
- [ ] VR button hidden on loading screen when no headset present
- [ ] VR button shown when headset detected
- [ ] `EnableVR()` called automatically when headset present
- [ ] FPS selection screen shown when no headset present (existing behaviour unchanged)
- [ ] Zero compile errors

---

### Wave 3 Validation Gate

**Without headset (regression test)**:
1. Enter Play mode
2. Verify loading screen shows FPS button (not VR)
3. Verify FPS mode launches normally

**With Meta Quest 2 connected** (when available):
1. Connect headset, enable Developer Mode
2. Enter Play mode
3. Verify VR button appears on loading screen
4. Verify VR mode activates automatically
5. Verify `ControlManager.IsVR` returns `true` (log it temporarily if needed)

---

## Cross-Cutting Concerns

### Build Settings (manual — not a code change)
Before any standalone build, add missing scenes:
**File → Build Settings → Add Open Scenes** for each of:
- `Assets/Scenes/OldMillScene.unity`
- `Assets/Scenes/tamHigh.unity`
- `Assets/Scenes/ParkSchool.unity`

Portal transitions will fail silently in a build if these are missing.

### Documentation Updates
After completing all waves, update:
- `DOCS/BUGS_AND_ISSUES.md` — mark BUG-002, BUG-003, BUG-004 as Fixed
- `DOCS/BUGS_AND_ISSUES.md` — mark FEATURE-001 as Partially Restored (system browser)
- `DOCS/VR_MIGRATION_GUIDE.md` — mark Wave 3 tasks as complete when done

---

## Risk Assessment

| Risk | Likelihood | Mitigation |
|------|-----------|------------|
| `Fading.cs` scene change event fires at wrong time | Low | Test fade-in on scene transition; revert to original if off |
| `Application.OpenURL` blocked by macOS sandbox in build | Low | Test in standalone build; add entitlement if needed |
| `XRDeviceUtil.isPresent()` returns true unexpectedly | Very low | Only happens if XR subsystem running; easy to revert |
| `WebViewTriggerComponent` VRPlayer branch untestable without headset | Medium | Tag FPS controller as VRPlayer temporarily to test |

**Critical path**: Wave 1 → Wave 2 → Wave 3 (in order)  
**Rollback**: Every change is a small, isolated edit. `git checkout <file>` reverts any single task instantly.

---

## Summary

| Wave | Tasks | Files Changed | Effort | Prerequisite |
|------|-------|--------------|--------|--------------|
| Wave 1 | T-001, T-002, T-003 | `DayNightStart.cs`, `TimeWalkControls1920.cs`, `Fading.cs` | 20 min | None |
| Wave 2 | T-004, T-005 | `FirstPersonUIComponent.cs`, `WebViewTriggerComponent.cs` | 20 min | Wave 1 |
| Wave 3 | T-006, T-007 | `ControlManager.cs`, `LoadingManager.cs` | 20 min | com.unity.xr.oculus package |
| **Total** | **7 tasks** | **7 files** | **~75 min** | |
