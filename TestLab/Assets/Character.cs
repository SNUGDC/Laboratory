using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	[System.Serializable]
	public enum teams {Player, Enemy};
	public teams team;
	public Weapon weapon;

	private bool moveable = true;
	private Rigidbody2D rigid;

	void Awake(){
		rigid = GetComponent<Rigidbody2D>();
	}

	public void SetMoveless(float time){
		Invoke("SetMoveable",time);
		moveable = false;
	}
	public void SetMoveable(){
		moveable = true;
		CancelInvoke("SetMoveable");
	}
	public void SetVelocity(Vector2 vec){
		if(moveable)
			rigid.velocity = vec;
	}
	public void SetWeapon(GameObject newWeapon){
		if(weapon){
			Destroy(weapon.gameObject);
			weapon = null;
		}
		GameObject weaponObj = Instantiate (newWeapon,
		                                    transform.position,
		                                    transform.rotation)as GameObject;
		weaponObj.transform.SetParent(transform);
		weapon = weaponObj.GetComponent<Weapon>();
	}
	public void Attack(Vector2 dirc){
		if(weapon) weapon.Trigger(dirc);
	}
}
