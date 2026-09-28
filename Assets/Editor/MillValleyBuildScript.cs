using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Build an APK without requiring a connected headset.
// Invoked via: Unity -batchmode -executeMethod MillValleyBuildScript.BuildAndroid
public static class MillValleyBuildScript
{
    // Keep the original entry point available for existing command-line workflows.
    public static void BuildAndRunAndroid()
    {
        BuildAndroid();
    }

    public static void BuildAndroid()
    {
        // Respect the SDK selected in Unity unless an environment override is supplied.
        string sdkPath = System.Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
        if (string.IsNullOrEmpty(sdkPath))
            sdkPath = System.Environment.GetEnvironmentVariable("ANDROID_HOME");

        if (!string.IsNullOrEmpty(sdkPath))
        {
            if (!Directory.Exists(sdkPath))
            {
                Debug.LogError("[MillValleyBuild] Android SDK override not found at " + sdkPath);
                EditorApplication.Exit(3);
                return;
            }
            EditorPrefs.SetString("AndroidSdkRoot", sdkPath);
            EditorPrefs.SetBool("SdkUseEmbedded", false);
            Debug.Log("[MillValleyBuild] AndroidSdkRoot set to " + sdkPath);
        }

        // 2. Collect the enabled scenes from Build Settings (LoadingScene first, etc.).
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("[MillValleyBuild] No enabled scenes in Build Settings. Aborting.");
            EditorApplication.Exit(2);
            return;
        }
        Debug.Log("[MillValleyBuild] Scenes (" + scenes.Length + "): " + string.Join(", ", scenes));

        // 3. Build the APK. Install it separately with MQDH or adb.
        string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "Android");
        Directory.CreateDirectory(outDir);
        string apkPath = Path.Combine(outDir, "MillValley.apk");

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = apkPath,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            // Build the APK only (do NOT AutoRunPlayer) so the build does not fail when the
            // Quest is disconnected from USB. The APK is installed separately via adb.
            options = BuildOptions.None,
        };

        Debug.Log("[MillValleyBuild] Starting build -> " + apkPath);
        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("[MillValleyBuild] SUCCESS: " + summary.outputPath + " (" + summary.totalSize + " bytes)");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError("[MillValleyBuild] FAILED: result=" + summary.result +
                           " errors=" + summary.totalErrors);
            EditorApplication.Exit(1);
        }
    }
}
