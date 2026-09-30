using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Desktop (Windows x64) build for previewing the Mill Valley scene without a headset.
// Invoked via: Unity -batchmode -executeMethod HelmBuildScript.BuildWindows
// Output: Builds/Windows/MillValley.exe (Fly-through / Walk-through desktop modes).
public static class HelmBuildScript
{
    public static void BuildWindows()
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        if (scenes.Length == 0)
        {
            Debug.LogError("[HelmBuild] No enabled scenes in Build Settings.");
            EditorApplication.Exit(2);
            return;
        }

        string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "Windows");
        Directory.CreateDirectory(outDir);
        string exePath = Path.Combine(outDir, "MillValley.exe");

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = exePath,
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
            options = BuildOptions.None,
        };

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
        Debug.Log("[HelmBuild] Building Windows x64 (Mono) -> " + exePath);
        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("[HelmBuild] SUCCESS: " + summary.outputPath + " (" + summary.totalSize + " bytes, " + summary.totalTime + ")");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError("[HelmBuild] FAILED: result=" + summary.result + " errors=" + summary.totalErrors);
            EditorApplication.Exit(1);
        }
    }
}
