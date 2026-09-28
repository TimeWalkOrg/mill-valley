using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR;

// VRMenuInput drives the control-selection menu with the VR controller, since a headset
// has no mouse pointer. The menu buttons (ControlSelectComponent) already do the right
// thing on click; this component lets the user HIGHLIGHT an option with the left
// thumbstick (up/down) and CONFIRM it with the trigger (or A), then invokes that button's
// normal onClick. The highlighted option is scaled up so it is obvious which is selected.
//
// Attached at runtime by LoadingManager only when a VR headset is present.
public class VRMenuInput : MonoBehaviour
{
	[Tooltip("How much bigger the highlighted option is drawn.")]
	public float highlightScale = 1.18f;

	private readonly List<Button> _buttons = new List<Button>();
	private readonly List<Vector3> _baseScales = new List<Vector3>();
	private int _index = 0;

	private bool _selectionMade = false;
	private bool _axisLatched = false;     // edge-detect stick movement
	private bool _prevConfirm = false;     // edge-detect confirm press
	private int _frame = 0;

	private void Start()
	{
		// Collect the selectable buttons (the three mode options), top-to-bottom.
		GetComponentsInChildren<Button>(true, _buttonsScratch);
		_buttons.Clear();
		_baseScales.Clear();
		foreach (Button b in _buttonsScratch)
		{
			if (b != null)
				_buttons.Add(b);
		}
		// Sort by vertical screen position so "up" on the stick goes to the upper option.
		_buttons.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
		foreach (Button b in _buttons)
			_baseScales.Add(b.transform.localScale);

		Debug.Log("[MV] VRMenuInput.Start buttons=" + _buttons.Count + " isPresent=" + XRDeviceUtil.isPresent());
		Highlight(0);
	}

	// Reusable buffer so GetComponentsInChildren doesn't allocate each call.
	private static readonly List<Button> _buttonsScratch = new List<Button>();

	private void Update()
	{
		if (_selectionMade || !XRDeviceUtil.isPresent() || _buttons.Count == 0)
			return;

		if ((_frame++ % 90) == 0)
			Debug.Log("[MV] VRMenuInput.Update index=" + _index + " buttons=" + _buttons.Count);

		InputDevice left  = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

		// --- Navigate with the left thumbstick (Y axis), edge-detected ---
		Vector2 stick;
		left.TryGetFeatureValue(CommonUsages.primary2DAxis, out stick);
		if (Mathf.Abs(stick.y) > 0.6f)
		{
			if (!_axisLatched)
			{
				int dir = stick.y > 0f ? -1 : 1; // up = previous (lower index), down = next
				Highlight(_index + dir);
				_axisLatched = true;
			}
		}
		else if (Mathf.Abs(stick.y) < 0.3f)
		{
			_axisLatched = false;
		}

		// --- Confirm with trigger or A, edge-detected ---
		bool trig = false, a = false;
		right.TryGetFeatureValue(CommonUsages.triggerButton, out trig);
		right.TryGetFeatureValue(CommonUsages.primaryButton, out a);
		bool confirm = trig || a;
		if (confirm && !_prevConfirm)
			Confirm();
		_prevConfirm = confirm;
	}

	private void Highlight(int newIndex)
	{
		if (_buttons.Count == 0)
			return;

		// Wrap around so the stick can cycle through all options.
		_index = (newIndex % _buttons.Count + _buttons.Count) % _buttons.Count;

		for (int i = 0; i < _buttons.Count; i++)
		{
			if (_buttons[i] == null)
				continue;
			_buttons[i].transform.localScale = (i == _index)
				? _baseScales[i] * highlightScale
				: _baseScales[i];
		}

		// Also drive the UI selection state (color tint) if an EventSystem exists.
		if (EventSystem.current != null && _buttons[_index] != null)
		{
			EventSystem.current.SetSelectedGameObject(_buttons[_index].gameObject);
			_buttons[_index].Select();
		}
	}

	private void Confirm()
	{
		if (_index < 0 || _index >= _buttons.Count || _buttons[_index] == null)
			return;

		_selectionMade = true;
		Debug.Log("[MV] VRMenuInput.Confirm button=" + _buttons[_index].gameObject.name);

		// Invoke the button's normal click (ControlSelectComponent -> ControlSelectMissive),
		// so VR selection behaves exactly like a mouse click.
		_buttons[_index].onClick.Invoke();

		// The menu canvas was reparented to the VR camera, so it won't be hidden by the
		// loading scene toggling off. Hide it explicitly. (Disables this component too.)
		gameObject.SetActive(false);
	}
}
