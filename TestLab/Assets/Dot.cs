using UnityEngine;
using System.Collections;

public class Dot  {
	public Vector3 position;
	public Vector3 velocity;
	public int x,y,prevX,prevY;
	public float life;

	public void Update(float gravity=0f, bool bounce=false){
		position += velocity * Time.deltaTime;
		velocity += Vector3.back * gravity * Time.deltaTime;
		life -= Time.deltaTime;
		if (position.z < 0 && bounce) {
			velocity.z *= -1;
			position.z = 0;
		}
	}
}
