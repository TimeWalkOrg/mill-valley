using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class CC_Base : MonoBehaviour
{
	public Shader shader;
	protected Material _material;

	public static bool IsLinear { get { return QualitySettings.activeColorSpace == ColorSpace.Linear; } }

	protected virtual void Start()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
		
		// Disable the image effect if the shader can't
		// run on the users graphics card
		if (!shader || !shader.isSupported)
			enabled = false;
	}

	protected Material material
	{
		get
		{
			if (_material == null)
			{
				_material = new Material(shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}

			return _material;
		} 
	}
	
	protected virtual void OnDisable()
	{
		if (_material)
			DestroyImmediate(_material);
	}
}
