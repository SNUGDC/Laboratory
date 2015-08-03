using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
	public Vector2 speed;

	private Character character;

	// Use this for initialization
	void Awake () {
		character = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		character.SetVelocity(new Vector2(
			Input.GetAxis ("Horizontal")*speed.x,
			Input.GetAxis ("Vertical")*speed.y));
	}
}
