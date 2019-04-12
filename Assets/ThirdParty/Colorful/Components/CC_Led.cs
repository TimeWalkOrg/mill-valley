using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/LED")]
public class CC_Led : CC_Base
{
	[Range(1f, 255f)]
	public float scale = 80.0f;

	[Range(0f, 10f)]
	public float brightness = 1.0f;

	public bool automaticRatio = false;
	public float ratio = 1.0f;
	public int mode = 0;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		switch (mode)
		{
			case 0:
				material.SetFloat("_Scale", scale);
				break;
			case 1:
			default:
				material.SetFloat("_Scale", GetComponent<Camera>().pixelWidth / scale);
				break;
		}

		material.SetFloat("_Ratio", automaticRatio ? (GetComponent<Camera>().pixelWidth / GetComponent<Camera>().pixelHeight) : ratio);
		material.SetFloat("_Brightness", brightness);
		Graphics.Blit(source, destination, material);
	}
}
