# TimeWalk — Mill Valley 1920

A Unity recreation of downtown Mill Valley, California, circa 1920, built by
[TimeWalk](https://timewalk.org). Runs as a standalone **Meta Quest 2/3** VR app
and as a Windows desktop walk-through / fly-through.

- **Unity:** 2020.3.48f1 (LTS), built-in render pipeline
- **XR:** Unity XR Plug-in Management + Oculus XR Plugin 1.13.1 (Android + desktop Oculus)
- **Android target:** ARM64, IL2CPP, OpenGL ES 3, minSdk 29 / targetSdk 32, Quest 2 (Quest 1 dropped)
- **Latest Quest APK (Eric's June 2026 build):** <https://github.com/arnaurodondev/mill-valley/releases/tag/quest-june-2026>

## Opening the project

1. Install Unity **2020.3.48f1** via Unity Hub, with the *Android Build Support*
   module (SDK/NDK/OpenJDK) if you intend to build for Quest.
2. Clone this repo (≈5 GB of assets — see `docs/asset-audit.md`) and open the
   folder in Unity Hub. First import takes a while.
3. Enabled scenes (Build Settings, in order): `LoadingScene` → `MainScene`,
   `OldMillScene`, `tamHigh`. Press Play in `Assets/Scenes/LoadingScene.unity`.

At startup `LoadingManager` checks for a headset. If one is present the app goes
straight into VR; otherwise the desktop control-selection menu is shown
(Walk-through or Fly-through).

## Desktop controls

| Key | Action |
|---|---|
| **W A S D** / arrow keys + mouse | Move / look (walk-through: on foot; fly-through: free camera) |
| **Y** | Next year (cycles the era models) |
| **N** | Toggle day / night |
| **C** | Show / hide credits |
| **H** | Show / hide help |
| **R** | Restart — back to the loading / mode-selection screen |
| **Esc** or **Q** | Quit |

## VR controls (Quest / Touch controllers)

| Input | Action |
|---|---|
| **Left thumbstick** | Walk (head-relative, horizontal, 2.5 m/s) |
| **Left thumbstick click** | Reset height back to the starting floor level |
| **Right thumbstick left / right** | Snap turn 45° |
| **Right thumbstick up / down** | Vertical flight — **off by default** (`VRLocomotion.allowVerticalFlight`) |
| **A** | Credits |
| **B** | Day / night |
| **X** / **Y** | Previous / next year |

A comfort vignette (`VRLocomotion.vignetteOnMove`) darkens the edges of view
while walking or turning. VR head tracking, locomotion and the vignette are
attached at runtime by `ControlManager.ConfigureVRCamera()`; the rig prefab is
untouched. Locomotion defaults live in `Assets/Prefabs/Managers/ControlManager.prefab`
and `Assets/Scripts/Managers/VRLocomotion.cs`.

## Building

Both build scripts live in `Assets/Editor/` and use the enabled scenes from
Build Settings. Run them headlessly (no editor GUI open on the project):

**Quest APK** → `Builds/Android/MillValley.apk`

```powershell
& "C:\Program Files\Unity\Hub\Editor\2020.3.48f1\Editor\Unity.exe" -batchmode -nographics -quit `
  -projectPath <path-to-repo> `
  -executeMethod MillValleyBuildScript.BuildAndroid `
  -logFile build-android.log
```

Uses the Unity-bundled Android SDK unless `ANDROID_SDK_ROOT` / `ANDROID_HOME` is
set. Install with `adb install -r Builds/Android/MillValley.apk` or Meta Quest
Developer Hub. (`MillValleyBuildScript.BuildAndRunAndroid` is kept as an alias.)

**Windows x64 desktop** → `Builds/Windows/MillValley.exe`

```powershell
& "C:\Program Files\Unity\Hub\Editor\2020.3.48f1\Editor\Unity.exe" -batchmode -nographics -quit `
  -projectPath <path-to-repo> `
  -executeMethod HelmBuildScript.BuildWindows `
  -logFile build-windows.log
```

Check the log for `error CS` / `Build succeeded` afterwards.

## Repository layout

| Path | What |
|---|---|
| `Assets/Scripts/Managers/` | `LoadingManager` (scene flow, VR auto-start), `ControlManager` (modes, hotkeys, VR camera patch-up), `VRLocomotion`, `XRBootstrap` |
| `Assets/Scripts/Components/` | Scene behaviours (signage, portals, fades, …) |
| `Assets/Editor/` | Headless build scripts |
| `Assets/XR/Settings/Oculus Settings.asset` | Oculus plugin settings (multiview, Quest 2 target) |
| `Assets/Prefabs/Managers/` | Manager prefabs incl. `ControlManager.prefab` (locomotion tuning) |
| `docs/asset-audit.md` | Asset size / duplicate / oversized-texture audit |

## Branches

- **`main`** — stable, what Eric's Quest release was cut from.
- **`helm`** — Helm's (Ted's AI assistant) integration branch; PRs into `main`.
- **`improvements`** — current working branch: Quest 2/3 targeting, VR comfort
  pass, dead-code removal, asset audit, this README. PR into `main` when tested
  on-device.
- Older branches (`2019.1`, `Cinemachine`, `Valantia`, `TNTestingSmallFixes`,
  `quest-vr-support`) are historical.

## History

Originally built 2017–2019 with UC Berkeley "Building History in 3D" student
models (Semesters 01–03 folders) on the legacy Oculus OVR SDK and Unity's
built-in VR. Migrated in 2026 to Unity 2020.3 LTS + XR Plug-in Management, with
Quest standalone support restored in summer 2026.

*Last updated by Helm — September 30, 2026*
