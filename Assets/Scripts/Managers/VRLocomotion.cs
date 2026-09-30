using UnityEngine;
using UnityEngine.XR;

// Thumbstick locomotion for the VR rig. Attached at runtime by
// ControlManager.ConfigureVRCamera() to the rig root (moving the root moves the
// whole rig, camera included).
//
// Controls:
//   Left stick          walk (head-relative, horizontal plane)
//   Left stick click    reset rig height to its starting Y
//   Right stick X       snap turn (snapTurnDegrees per flick)
//   Right stick Y       vertical flight (ONLY when allowVerticalFlight is true)
//
// Comfort: a subtle tunnelling vignette fades in while walking / turning and fades
// out at rest (vignetteOnMove). Implemented with a runtime-built quad parented to
// the center-eye camera, so it needs no scene/prefab changes.
public class VRLocomotion : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.5f;            // m/s, walking pace
    public float snapTurnDegrees = 45f;

    [Header("Vertical flight (off by default for comfort)")]
    public bool allowVerticalFlight = false;
    public float verticalSpeed = 2.0f;        // m/s on the right stick Y axis when allowed

    [Header("Comfort vignette")]
    public bool vignetteOnMove = true;
    [Range(0f, 1f)] public float vignetteMaxAlpha = 0.6f;
    public float vignetteFadeSpeed = 6f;      // alpha units per second

    private bool _rightSnapped = false;
    private bool _leftClickHeld = false;
    private bool _startYCaptured = false;
    private float _startY;
    private Camera _vrCam;
    private int _logFrame = 0;

    // Vignette state
    private Material _vignetteMat;
    private GameObject _vignetteGO;
    private float _vignetteAlpha = 0f;
    private bool _movedThisFrame = false;

    // Resolve the rig's center-eye camera once. We avoid Camera.main because the VR rig
    // has multiple GameObjects tagged MainCamera, so Camera.main is ambiguous and could
    // return the wrong (or a disabled) camera. ControlManager.GetVRCamera() finds the
    // CenterEyeAnchor reliably.
    private Camera VRCam
    {
        get
        {
            if (_vrCam == null && ControlManager.instance != null)
                _vrCam = ControlManager.instance.GetVRCamera();
            return _vrCam;
        }
    }

    private void OnEnable()
    {
        if (!_startYCaptured)
        {
            _startY = transform.position.y;
            _startYCaptured = true;
        }
    }

    private void OnDisable()
    {
        SetVignetteAlpha(0f);
    }

    private void OnDestroy()
    {
        if (_vignetteGO != null) Destroy(_vignetteGO);
        if (_vignetteMat != null) Destroy(_vignetteMat);
    }

    private void Update()
    {
        if (!XRDeviceUtil.isPresent()) return;
        _movedThisFrame = false;
        HandleMovement();
        HandleHeightReset();
        HandleSnapTurn();
        UpdateVignette();
    }

    private void HandleMovement()
    {
        InputDevice left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Vector2 stick;
        if (!left.TryGetFeatureValue(CommonUsages.primary2DAxis, out stick)) return;
        if (stick.magnitude < 0.15f) return;

        Camera vrCam = VRCam;
        if (vrCam == null)
        {
            if ((_logFrame++ % 60) == 0)
                Debug.Log("[MV] VRLocomotion: stick=" + stick + " but NO vrCam (cannot move)");
            return;
        }
        Transform cam = vrCam.transform;
        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right   = cam.right;   right.y   = 0f; right.Normalize();

        transform.position += (forward * stick.y + right * stick.x) * moveSpeed * Time.deltaTime;
        _movedThisFrame = true;

        if ((_logFrame++ % 30) == 0)
            Debug.Log("[MV] VRLocomotion moving rig='" + name + "' stick=" + stick + " pos=" + transform.position);
    }

    // Left thumbstick click snaps the rig back to the height it started at. Useful
    // after vertical flight, or if the floor height ever drifts.
    private void HandleHeightReset()
    {
        InputDevice left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool click;
        if (!left.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out click)) return;

        if (click && !_leftClickHeld)
        {
            Vector3 p = transform.position;
            p.y = _startY;
            transform.position = p;
            Debug.Log("[MV] VRLocomotion: height reset to y=" + _startY);
        }
        _leftClickHeld = click;
    }

    private void HandleSnapTurn()
    {
        InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        Vector2 stick;
        if (!right.TryGetFeatureValue(CommonUsages.primary2DAxis, out stick)) return;

        // Right stick X = snap turn (left/right).
        if (Mathf.Abs(stick.x) > 0.7f && !_rightSnapped)
        {
            transform.Rotate(Vector3.up, Mathf.Sign(stick.x) * snapTurnDegrees);
            _rightSnapped = true;
            _movedThisFrame = true;
        }
        else if (Mathf.Abs(stick.x) <= 0.3f)
        {
            _rightSnapped = false;
        }

        // Right stick Y = vertical flight. Disabled by default: smooth vertical motion
        // is a common motion-sickness trigger. Enable via allowVerticalFlight.
        if (allowVerticalFlight && Mathf.Abs(stick.y) > 0.2f)
        {
            transform.position += Vector3.up * stick.y * verticalSpeed * Time.deltaTime;
            _movedThisFrame = true;
        }
    }

    // ---------------------------------------------------------------- vignette

    private void UpdateVignette()
    {
        if (!vignetteOnMove)
        {
            if (_vignetteAlpha > 0f) SetVignetteAlpha(0f);
            return;
        }

        float target = _movedThisFrame ? vignetteMaxAlpha : 0f;
        float a = Mathf.MoveTowards(_vignetteAlpha, target, vignetteFadeSpeed * Time.deltaTime);
        if (!Mathf.Approximately(a, _vignetteAlpha))
            SetVignetteAlpha(a);
    }

    private void SetVignetteAlpha(float a)
    {
        _vignetteAlpha = a;
        if (a <= 0.001f)
        {
            if (_vignetteGO != null) _vignetteGO.SetActive(false);
            return;
        }
        if (!EnsureVignette()) return;
        if (!_vignetteGO.activeSelf) _vignetteGO.SetActive(true);
        Color c = _vignetteMat.color;
        c.a = a;
        _vignetteMat.color = c;
    }

    // Builds (once) a quad just in front of the center-eye camera with a radial
    // gradient texture: transparent centre, opaque black edges. Alpha is scaled by
    // the material colour so we only touch one float per frame.
    private bool EnsureVignette()
    {
        if (_vignetteGO != null && _vignetteMat != null) return true;

        Camera cam = VRCam;
        if (cam == null) return false;

        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null) shader = Shader.Find("Unlit/Transparent");
        if (shader == null)
        {
            Debug.LogWarning("[MV] VRLocomotion: no transparent shader found, vignette disabled");
            vignetteOnMove = false;
            return false;
        }

        _vignetteMat = new Material(shader);
        _vignetteMat.mainTexture = BuildRadialTexture(128);
        _vignetteMat.color = new Color(0f, 0f, 0f, 0f);
        _vignetteMat.renderQueue = 4000; // overlay: after everything else

        _vignetteGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _vignetteGO.name = "ComfortVignette";
        Collider col = _vignetteGO.GetComponent<Collider>();
        if (col != null) Destroy(col);

        MeshRenderer mr = _vignetteGO.GetComponent<MeshRenderer>();
        mr.sharedMaterial = _vignetteMat;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

        // Sit just past the near clip plane, large enough to cover the full FOV of
        // either eye (generous scale; the gradient keeps the centre clear).
        float dist = Mathf.Max(cam.nearClipPlane + 0.05f, 0.35f);
        Transform t = _vignetteGO.transform;
        t.SetParent(cam.transform, false);
        t.localPosition = new Vector3(0f, 0f, dist);
        t.localRotation = Quaternion.identity;
        float size = dist * 3.5f;
        t.localScale = new Vector3(size, size, 1f);
        _vignetteGO.layer = cam.gameObject.layer;
        return true;
    }

    private static Texture2D BuildRadialTexture(int size)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        Color[] px = new Color[size * size];
        float half = (size - 1) * 0.5f;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = (x - half) / half;
                float dy = (y - half) / half;
                float r = Mathf.Sqrt(dx * dx + dy * dy);      // 0 centre .. ~1.41 corner
                float a = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.45f, 0.95f, r));
                px[y * size + x] = new Color(0f, 0f, 0f, a);
            }
        }
        tex.SetPixels(px);
        tex.Apply(false, true);
        return tex;
    }
}
