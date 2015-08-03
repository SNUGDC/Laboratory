using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class CRTeffect : MonoBehaviour {
	public Material effectMaterial;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		source.filterMode = FilterMode.Point;
		Graphics.Blit (source, destination, effectMaterial);
	}
}
