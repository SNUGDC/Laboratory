using UnityEngine;
using System.Collections;

public class MoveByPlayer : MonoBehaviour {
	public float speed = 2;
	[System.Serializable]
	public enum whichPlayer {Player1,Player2};
	public whichPlayer playerNum;
	void FixedUpdate () {
		if(playerNum == whichPlayer.Player1)
			GetComponent<Rigidbody2D>().velocity = Input.GetAxis("Stick1") * speed * Vector2.up;
		else
			GetComponent<Rigidbody2D>().velocity = Input.GetAxis("Stick2") * speed * Vector2.up;
	}
}
