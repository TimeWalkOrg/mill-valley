using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

/// <summary>
/// Performance batch-ops for the Mill Valley Quest 2 project.
/// Disables realtime shadows on point/spot lights (the biggest GPU cost on Quest)
/// while leaving directional lights untouched.
/// </summary>
public static class PerfOps
{
    private const string LogPrefix = "[MV][PERF]";

    // Candidate scenes to process. Only those that actually exist on disk are opened.
    private static readonly string[] CandidateScenes =
    {
        "Assets/Scenes/MainScene.unity",
        "Assets/Scenes/OldMillScene.unity",
        "Assets/Scenes/tamHigh.unity",
    };

    /// <summary>
    /// Batch-mode entry point. Run with:
    ///   Unity -batchmode -quit -executeMethod PerfOps.DisableRealtimeShadows
    /// Exits the editor with code 0 on success, 1 on failure.
    /// </summary>
    public static void DisableRealtimeShadows()
    {
        try
        {
            RunDisableRealtimeShadows();
            EditorApplication.Exit(0);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{LogPrefix} FAILED: {e.GetType().Name}: {e.Message}\n{e.StackTrace}");
            EditorApplication.Exit(1);
        }
    }

    /// <summary>
    /// Editor menu wrapper. Same logic but does NOT exit the editor.
    /// </summary>
    [MenuItem("Tools/Perf/Disable Realtime Point-Spot Shadows")]
    public static void DisableRealtimeShadowsMenu()
    {
        try
        {
            RunDisableRealtimeShadows();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"{LogPrefix} FAILED: {e.GetType().Name}: {e.Message}\n{e.StackTrace}");
        }
    }

    /// <summary>
    /// Core logic shared by both the batch-mode and menu entry points.
    /// </summary>
    private static void RunDisableRealtimeShadows()
    {
        int totalChanged = 0;
        int scenesProcessed = 0;

        foreach (string scenePath in CandidateScenes)
        {
            if (!File.Exists(scenePath))
            {
                Debug.Log($"{LogPrefix} Skipping (not found): {scenePath}");
                continue;
            }

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // Include inactive objects so disabled lights are caught too.
            Light[] lights = Object.FindObjectsOfType<Light>(true);

            int changedInScene = 0;
            foreach (Light light in lights)
            {
                if (light == null)
                {
                    continue;
                }

                if (light.type == LightType.Point || light.type == LightType.Spot)
                {
                    if (light.shadows != LightShadows.None)
                    {
                        light.shadows = LightShadows.None;
                        changedInScene++;
                    }
                }
                // Directional lights are intentionally left unchanged.
            }

            if (changedInScene > 0)
            {
                EditorSceneManager.MarkSceneDirty(scene);
            }

            EditorSceneManager.SaveScene(scene);

            Debug.Log($"{LogPrefix} {scenePath}: disabled realtime shadows on {changedInScene} point/spot light(s).");

            totalChanged += changedInScene;
            scenesProcessed++;
        }

        Debug.Log($"{LogPrefix} DONE. Processed {scenesProcessed} scene(s), disabled shadows on {totalChanged} point/spot light(s) total.");
    }
}
