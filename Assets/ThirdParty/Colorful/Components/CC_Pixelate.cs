using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Pixelate")]
public class CC_Pixelate : CC_Base
{
	[Range(1f, 1024f)]
	public float scale = 80.0f;
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
		Graphics.Blit(source, destination, material);
	}
}
