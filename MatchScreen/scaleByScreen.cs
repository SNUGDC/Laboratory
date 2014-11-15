using UnityEngine;
using System.Collections;

public class scaleByScreen : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		float widthByPixel = Screen.width;
		float fixedWidth = (float)Screen.height/9*16;
		float scale = widthByPixel/fixedWidth;
		transform.localScale = Vector3.one * scale;
	}
}
