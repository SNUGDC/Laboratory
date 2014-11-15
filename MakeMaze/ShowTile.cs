using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowTile : MonoBehaviour {
	[System.NonSerialized]
	public List<int> directions;
	public GameObject[] wayGraphics;
	private SpriteRenderer srenderer;

	public void setDirc(int newDirc){
		wayGraphics[newDirc].SetActive(true);
		if(!srenderer.enabled)
			srenderer.enabled = true;
	}
	public void setColor(Color clr){
		srenderer.color = clr;
	}
	void Awake(){
		foreach(GameObject obj in wayGraphics)
			obj.SetActive(false);
		srenderer = GetComponent<SpriteRenderer>();
		srenderer.enabled = false;
	}
}
