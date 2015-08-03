using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotParticle : MonoBehaviour {
	public bool drawGizmo = false;
	public bool playOnEnable = true;
	public bool destroyOnEnd = true;
	public float duration = 1f;
	public float lifetime = 0.5f;
	public float minimumLifetime = 0f;
	public float fadeRate = 0.8f;
	public bool looping = false;
	public float angle = 0.5f;
	public bool moveable = true;
	public bool colorChange = true;
	public float colorChangeRate = 0.8f;
	public Color startColor = Color.white;
	public Color endColor = Color.clear;
	public float xySpeed = 5f;
	public float zSpeed = 5f;
	public float gravity = 10f;
	public bool bounce = true;
	public int size=128,pixelPerUnit=128;
	public bool bilinearFilter = false;

	public int rate = 10;
	public int burst = 20;

	private SpriteRenderer myRenderer;
	private Texture2D myTexture;
	private List<Dot> rendees;
	private float angle_r;

	private float time;
	private int spawned;

	private Color fadeColor;
	private float fadeness;
	private Color[] workingColors;
	private Color[] previousColors;
	private Vector3 previousPosition;

	public void PlayBurst(int number = -1){
		if(number==-1) number = burst;
		for(int i=0;i<number;i++){
			Dot newDot = new Dot();
			newDot.position = Vector3.zero;
			newDot.velocity = 
				(Vector3)Random.insideUnitCircle * xySpeed
					+ Vector3.forward * zSpeed * Random.Range(0f,1f);
			newDot.prevX = size/2;
			newDot.prevY = size/2;
			newDot.life = Random.Range(0f,lifetime);
			rendees.Add(newDot);
		}
	}

	// Use this for initialization
	void Awake () {
		myRenderer = GetComponent<SpriteRenderer>();
		if(myRenderer==null)
			myRenderer = gameObject.AddComponent<SpriteRenderer>();

		myTexture = new Texture2D(size,size,TextureFormat.ARGB32,false,false);
		myRenderer.sprite = Sprite.Create(
			myTexture,
			new Rect(0f,0f,(float)size,(float)size),
			new Vector2(0.5f,0.5f),pixelPerUnit);

		if(!bilinearFilter){
			myTexture.filterMode = FilterMode.Point;
		}
		workingColors = myTexture.GetPixels();
		ClearTexture ();

		fadeColor = new Color();
		rendees = new List<Dot>();
	}

	void OnEnable(){
		ClearTexture();
		rendees.Clear();
		if(playOnEnable)
			Initiate();
	}
	
	void Initiate(){
		angle_r = Mathf.Sqrt(1-angle*angle);
		CancelInvoke("LoopEnd");
		Invoke("LoopEnd",duration);
		for(int i=0;i<burst;i++){
			Dot newDot = new Dot();
			newDot.position = Vector3.zero;
			newDot.velocity = 
				(Vector3)Random.insideUnitCircle * xySpeed
				+ Vector3.forward * zSpeed * Random.Range(0f,1f);
			newDot.prevX = size/2;
			newDot.prevY = size/2;
			newDot.life = Random.Range(0f,lifetime);
			rendees.Add(newDot);
		}
		time = 0f;
		spawned = 0;
		fadeColor.a = fadeRate;
		fadeColor.r = Mathf.Pow(endColor.r + 0.3f,1-colorChangeRate);
		fadeColor.g = Mathf.Pow(endColor.g + 0.3f,1-colorChangeRate);
		fadeColor.b = Mathf.Pow(endColor.b + 0.3f,1-colorChangeRate);
		workingColors = myTexture.GetPixels();
		if(previousColors==null)
			previousColors = (Color[])workingColors.Clone();
		previousPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if(moveable)
			MoveTexture();

		time += Time.deltaTime;
		SpawnDots ();
		FadeColors ();
		UpdateRendees ();
		for(int i=0;i<workingColors.Length;i++){
			previousColors[i] = workingColors[i];
		}
		myTexture.SetPixels(workingColors);
		myTexture.Apply();
	}

	void SpawnDots ()
	{
		int spawnGoal = (int)(rate * time);
		if (spawned < spawnGoal) {
			for (int i = 0; i < spawnGoal - spawned; i++) {
				spawned++;
				Dot newDot = new Dot ();
				newDot.position = Vector3.zero;
				newDot.velocity = (Vector3)Random.insideUnitCircle * xySpeed + Vector3.forward * zSpeed * Random.Range (0f, 1f);
				newDot.prevX = size / 2;
				newDot.prevY = size / 2;
				newDot.life = Random.Range (0f, lifetime);
				rendees.Add (newDot);
			}
		}
	}

	void FadeColors ()
	{
		if (colorChange) {
			for (int i = 0; i < workingColors.Length; i++) {
				if (workingColors [i].a != 0f) {
					workingColors [i] *= fadeColor;
					if (workingColors [i].a < 0.1f)
						workingColors [i].a = 0f;
				}
			}
		}
		else {
			for (int i = 0; i < workingColors.Length; i++) {
				workingColors [i].a *= fadeRate;
			}
		}
	}

	void UpdateRendees ()
	{
		int minx, maxx, miny, maxy;
		for (int i = 0; i < rendees.Count; i++) {
			rendees[i].Update(gravity,bounce);
			rendees[i].x = Mathf.RoundToInt (rendees[i].position.x + size / 2);
			rendees[i].y = Mathf.RoundToInt (angle_r * rendees[i].position.y + angle * rendees[i].position.z + size / 2);
			if (rendees[i].life < 0) {
				rendees.RemoveAt (i);
				continue;
			}

			if (rendees [i].prevX > rendees [i].x) {
				minx = rendees [i].x;
				maxx = rendees [i].prevX;
			}
			else {
				maxx = rendees [i].x;
				minx = rendees [i].prevX;
			}
			if (rendees [i].prevY > rendees [i].y) {
				miny = rendees [i].y;
				maxy = rendees [i].prevY;
			}
			else {
				maxy = rendees [i].y;
				miny = rendees [i].prevY;
			}
			bool[] ylist = new bool[maxy - miny + 1];
			if (minx != maxx) {
				for (int x = minx; x <= maxx; x++) {
					if(x==rendees[i].prevX) continue;
					int y = Mathf.RoundToInt (Mathf.Lerp (rendees [i].prevY, rendees [i].y, (float)(x - rendees [i].prevX) / (rendees [i].x - rendees [i].prevX)));
					SetPixel (x, y, workingColors, rendees [i].life / lifetime);
					ylist [y - miny] = true;
				}
			}
			if (miny != maxy) {
				for (int y = miny; y <= maxy; y++) {
					if(y==rendees[i].prevY) continue;
					if (!ylist [y - miny])
						SetPixel (Mathf.RoundToInt (Mathf.Lerp (rendees [i].prevX, rendees [i].x, (float)(y - rendees [i].prevY) / (rendees [i].y - rendees [i].prevY))), y, workingColors, rendees [i].life / lifetime);
				}
			}
			//drawing in texture
			rendees [i].prevX = rendees [i].x;
			rendees [i].prevY = rendees [i].y;
		}
	}

	void ClearTexture ()
	{
		for (int i = 0; i < workingColors.Length; i++) {
			workingColors [i] = Color.clear;
		}
		myTexture.SetPixels (workingColors);
		myTexture.Apply ();
	}

	void SetPixel(int x, int y, Color[] colors, float fillRate = 1f){
		if(x>=0 && x<size && y>=0 && y<size){
			Color addColor = startColor;
			addColor.a = fillRate;
			colors[size*y+x] += addColor;
		}
	}

	void LoopEnd(){
		if(looping)
			Initiate();
		else if(destroyOnEnd)
			Destroy(gameObject);
		else
			rendees.Clear();
	}

	void MoveTexture(){
		Vector3 deltaPosition = (transform.position - previousPosition) / transform.localScale.x;
		int deltaX = (int)(deltaPosition.x * pixelPerUnit);
		int deltaY = (int)(deltaPosition.y * pixelPerUnit);
		previousPosition.x += (float)deltaX*transform.localScale.x/pixelPerUnit;
		previousPosition.y += (float)deltaY*transform.localScale.y/pixelPerUnit;
		for(int i=0; i<workingColors.Length; i++){
			int x = i%size + deltaX;
			int y = i/size + deltaY;
			if(x>=0 && x<size && y>=0 && y<size){
				workingColors[i] = previousColors[size*y+x];
			}else{
				workingColors[i] = Color.clear;
			}
		}
		for(int i=0; i<rendees.Count; i++){
			rendees[i].x -= deltaX;
			rendees[i].prevX -= deltaX;
			rendees[i].position -= new Vector3(deltaX,deltaY/angle_r,0);
			rendees[i].y -= deltaY;
			rendees[i].prevY -= deltaY;
		}
	}

	void OnDrawGizmos(){
		if(drawGizmo){
			float length = size / pixelPerUnit * transform.localScale.x;
			Gizmos.DrawWireCube(transform.position,new Vector3(length,length,0f));
		}
	}
	
}
