using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

// Manual XR initialization for Quest.
//
// Auto-init ("Initialize XR on Startup" / m_InitManagerOnStart) is DISABLED for Android in
// Assets/XR/XRGeneralSettings.asset. With auto-init, Unity does the Oculus loader bring-up
// synchronously on the main thread *before the splash screen*, while the Oculus runtime is
// still negotiating session focus. If focus isn't granted in time (e.g. headset not yet on
// the face), that native call blocks the main thread forever -> the intermittent startup
// hang that stopped logs at "Oculus UI thread done".
//
// Instead we initialize the loader ourselves via a COROUTINE (InitializeLoader yields, so it
// never blocks the main thread the way auto-init does), then StartSubsystems(). Game code
// that branches on XRDeviceUtil.isPresent() must wait for IsXRReady first (see LoadingManager).
public static class XRBootstrap
{
    // True once init has finished (XR running OR determined unavailable / desktop).
    public static bool IsXRReady { get; private set; }

    // True if the Oculus loader actually started a running display subsystem.
    public static bool IsXRActive { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        IsXRReady = false;
        IsXRActive = false;

        var runner = new GameObject("XRBootstrapRunner");
        Object.DontDestroyOnLoad(runner);
        runner.hideFlags = HideFlags.HideAndDontSave;
        runner.AddComponent<XRBootstrapRunner>().StartCoroutine(InitRoutine());
    }

    private static IEnumerator InitRoutine()
    {
        var mgr = XRGeneralSettings.Instance != null
            ? XRGeneralSettings.Instance.Manager
            : null;

        if (mgr == null)
        {
            Debug.LogWarning("[MV][XR] No XRManagerSettings instance; desktop mode.");
            IsXRReady = true;
            yield break;
        }

        if (mgr.activeLoader == null)
        {
            Debug.Log("[MV][XR] InitializeLoader (manual)...");
            yield return mgr.InitializeLoader(); // coroutine form: does not block the main thread
        }

        if (mgr.activeLoader != null)
        {
            Debug.Log("[MV][XR] Loader initialized (" + mgr.activeLoader.name + ") -> StartSubsystems");
            mgr.StartSubsystems();
            IsXRActive = true;
            Debug.Log("[MV][XR] StartSubsystems done. XR active.");
        }
        else
        {
            Debug.LogWarning("[MV][XR] InitializeLoader returned no active loader; desktop mode.");
        }

        IsXRReady = true;
    }

    private class XRBootstrapRunner : MonoBehaviour { }
}
