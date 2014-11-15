using UnityEngine;
using System.Collections;

public class positionByScreen : MonoBehaviour {
	public bool positionInRight = false;
	public float distanceFromEdge = 1f;

	private float width;
	private float pixelPerUnit;
	private float widthByPixel;
	private Camera cam;

	// Use this for initialization
	void Awake () {
		if(transform.parent !=null && transform.parent.gameObject.GetComponent<Camera>() != null)
			cam = transform.parent.camera;
		else
			cam = Camera.main;

		widthByPixel = Screen.width;
		pixelPerUnit = Screen.height/(cam.orthographicSize*2);
		width = widthByPixel/pixelPerUnit;

		if(positionInRight){
			Vector3 buttonPosition = transform.localPosition;
			buttonPosition.x = width/2 - distanceFromEdge;
			transform.localPosition = buttonPosition;
		}else{
			Vector3 buttonPosition = transform.localPosition;
			buttonPosition.x = distanceFromEdge - width/2;
			transform.localPosition = buttonPosition;
		}
	}
}
