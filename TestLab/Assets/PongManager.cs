using UnityEngine;
using System.Collections;

public class PongManager : MonoBehaviour {
	public GameObject ball;
	public float initialVelocity;

	// Use this for initialization
	void Start () {
		MoveByPlayer[] players = GameObject.FindObjectsOfType<MoveByPlayer>();
		ball.GetComponent<Rigidbody2D>().velocity = 
			(players[Random.Range(0,1)].transform.position - ball.transform.position).normalized * initialVelocity;
	}
}
