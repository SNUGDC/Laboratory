using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update(){
		transform.LookAt(target);
		transform.Translate(Vector3.right * Time.deltaTime);
	}
}
