using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public enum WeaponType{projectile,hitscan,collider};
	public WeaponType weaponType;
	public GameObject projectile;
	public GameObject attackCollider;
	public bool automatic,isInfinite;
	public int ammo,ammoMax;
	public float delayBefore,delayAfter,atkRange,stepForward,knockBack,stunTime;
	public void Trigger(Vector2 direction){
		switch(weaponType){
		case WeaponType.projectile:
			//asdf
			break;
		case WeaponType.hitscan:
			//asdf
			break;
		case WeaponType.collider:
			//asdf
			break;
		default:
			//asdf
			break;
		}
	}
}
