using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class CRTCamera : MonoBehaviour {
	public Material effectMaterial;
	public int screenWidth;
	public int screenHeight;

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		effectMaterial.SetInt("_width",screenWidth);
		effectMaterial.SetInt("_height",screenHeight);
		source.filterMode = FilterMode.Point;
		source.useMipMap = false;
		Graphics.Blit (source, destination, effectMaterial);
	}
}
