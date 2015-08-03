using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomTexture : MonoBehaviour {
	public const int HEIGHT = 180, WIDTH = 320, HALF_HEIGHT = 90, HALF_WIDTH = 160;
	public List<PixelDisplayed> rendees;

	private SpriteRenderer myRenderer;
	private Texture2D myTexture;
	// Use this for initialization
	void Awake () {
		myRenderer = GetComponent<SpriteRenderer>();
		myTexture = new Texture2D(WIDTH,HEIGHT,TextureFormat.ARGB32,false,false);
		myRenderer.sprite = Sprite.Create(
			myTexture,
			new Rect(0f,0f,(float)WIDTH,(float)HEIGHT),
			new Vector2(0.5f,0.5f),HEIGHT/10f);
		myTexture.filterMode = FilterMode.Point;


		Color[] colors = myTexture.GetPixels();
		for(int i=0; i<colors.Length; i++){
			colors[i] = Color.black;
		}
		myTexture.SetPixels(colors);
		myTexture.Apply();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Color[] colors = myTexture.GetPixels();
		for(int i=0; i<colors.Length; i++){
			colors[i].r *= 0.8f;
			colors[i].g *= 0.8f;
			colors[i].b *= 0.8f;
		}
		foreach(PixelDisplayed rendee in rendees){
			if(rendee.isBar){
				float scale = rendee.gameObject.transform.localScale.y;
				for(int x = rendee.x-HEIGHT/64; x<rendee.x+HEIGHT/64; x++){
					for(int y = rendee.y-(int)(HEIGHT*0.2*scale); y<rendee.y+(int)(HEIGHT*0.2*scale); y++){
						SetPixel(x,y,colors);
					}
				}
			}else{
				int minx = rendee.prevX > rendee.x ? rendee.x : rendee.prevX;
				int maxx = rendee.prevX < rendee.x ? rendee.x : rendee.prevX;
				int miny = rendee.prevY > rendee.y ? rendee.y : rendee.prevY;
				int maxy = rendee.prevY < rendee.y ? rendee.y : rendee.prevY;
				if(minx != maxx){
					for(int x = minx; x <= maxx; x++){
						SetPixel(x,Mathf.RoundToInt(Mathf.Lerp(
							rendee.prevY,
							rendee.y,
							(float)(x-rendee.prevX)/(rendee.x-rendee.prevX))),colors);
					}
				}
				if(miny != maxy){
					for(int y = miny; y <= maxy; y++){
						SetPixel(Mathf.RoundToInt(Mathf.Lerp(
							rendee.prevX,
							rendee.x,
							(float)(y-rendee.prevY)/(rendee.y-rendee.prevY))),y,colors);
					}
				}
			}
		}

		myTexture.SetPixels(colors);
		myTexture.Apply();
	}

	void SetPixel(int x, int y, Color[] colors){
		if(x>=0 && x<WIDTH && y>=0 && y<HEIGHT)
			colors[WIDTH*y+x] = Color.white;
	}
}
