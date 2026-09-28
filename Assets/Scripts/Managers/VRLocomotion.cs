using UnityEngine;
using UnityEngine.XR;

public class VRLocomotion : MonoBehaviour
{
    public float moveSpeed = 40.0f;
    public float snapTurnDegrees = 45f;
    public float verticalSpeed = 20.0f;   // up/down speed on the right stick Y axis

    private bool _rightSnapped = false;
    private Camera _vrCam;
    private int _logFrame = 0;

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

    private void Update()
    {
        if (!XRDeviceUtil.isPresent()) return;
        HandleMovement();
        HandleSnapTurn();
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

        if ((_logFrame++ % 30) == 0)
            Debug.Log("[MV] VRLocomotion moving rig='" + name + "' stick=" + stick + " pos=" + transform.position);
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
        }
        else if (Mathf.Abs(stick.x) <= 0.3f)
        {
            _rightSnapped = false;
        }

        // Right stick Y = vertical movement (up/down). Lets the user rise off the floor
        // (the starting viewpoint is low) and fly to any height. Dead-zone to avoid drift.
        if (Mathf.Abs(stick.y) > 0.2f)
            transform.position += Vector3.up * stick.y * verticalSpeed * Time.deltaTime;
    }
}
