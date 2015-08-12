using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TiledSprite : MonoBehaviour {
	#if UNITY_EDITOR
	
	SpriteRenderer render;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		render.material.SetVector("_Tile",transform.localScale);
	}
	#endif
}
