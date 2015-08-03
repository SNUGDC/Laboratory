using UnityEngine;
using System.Collections;

public class PixelDisplayed : MonoBehaviour {
	public int x,y,prevX,prevY;
	public bool isBar = false;
	private bool active = false;
	// Use this for initialization
	void Awake () {
		x = Mathf.RoundToInt((transform.localPosition.x)*CustomTexture.HEIGHT/10f+CustomTexture.HALF_WIDTH);
		y = Mathf.RoundToInt((transform.localPosition.y)*CustomTexture.HEIGHT/10f+CustomTexture.HALF_HEIGHT);
		prevX = x;
		prevY = y;
		Invoke ("Activate",0.1f);
	}

	void OnEnable(){
		GameObject.FindObjectOfType<CustomTexture>().rendees.Add(this);
	}

	void OnDisable(){
		GetComponentInParent<CustomTexture>().rendees.Remove(this);
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player" 
		    && GetComponentInParent<CustomTexture>().rendees.Count<150 
		    && active 
		    && !isBar){
			GameObject instance = Instantiate(gameObject) as GameObject;
			instance.transform.SetParent(transform.parent);
			instance.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * -1;
		}
	}

	void Activate(){
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		prevX = x;
		prevY = y;
		x = Mathf.RoundToInt((transform.localPosition.x)*CustomTexture.HEIGHT/10f+CustomTexture.HALF_WIDTH);
		y = Mathf.RoundToInt((transform.localPosition.y)*CustomTexture.HEIGHT/10f+CustomTexture.HALF_HEIGHT);
		if(transform.localPosition.x < -10){
			Debug.Log (ScoreCounter.score += Vector2.up);
			Destroy(gameObject);
		}else if(transform.localPosition.x > 10){
			Debug.Log (ScoreCounter.score += Vector2.right);
			Destroy(gameObject);
		}

	}
}
