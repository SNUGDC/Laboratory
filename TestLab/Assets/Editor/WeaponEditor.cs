using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Weapon)), CanEditMultipleObjects]
public class WeaponEditor : Editor {
	public SerializedProperty 
		weaponType_Prop,
		projectileType_Prop,
		attackCollider_Prop,
		atkRange_Prop,
		controllable_Prop,
		ammo,ammoMax,
		isInfinite;
	public List<SerializedProperty> others;
	public List<bool> othersBool;
	
	void OnEnable () {
		// Setup the SerializedProperties
		weaponType_Prop = serializedObject.FindProperty ("weaponType");
		projectileType_Prop = serializedObject.FindProperty("projectile");
		attackCollider_Prop = serializedObject.FindProperty ("attackCollider");
		atkRange_Prop = serializedObject.FindProperty ("atkRange");
		ammo = serializedObject.FindProperty ("ammo");
		ammoMax = serializedObject.FindProperty ("ammoMax");
		isInfinite = serializedObject.FindProperty ("isInfinite");

		others = new List<SerializedProperty>();
		others.Add(serializedObject.FindProperty ("delayBefore"));
		others.Add(serializedObject.FindProperty ("delayAfter"));
		others.Add(serializedObject.FindProperty ("stepForward"));
		others.Add(serializedObject.FindProperty ("knockBack"));
		others.Add(serializedObject.FindProperty ("stunTime"));
		othersBool = new List<bool>();
		for(int i=0;i<others.Count;i++)
			othersBool.Add(false);
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		EditorGUILayout.PropertyField( weaponType_Prop );
		
		Weapon.WeaponType st = (Weapon.WeaponType)weaponType_Prop.enumValueIndex;
		
		switch( st ) {
		case Weapon.WeaponType.projectile:            
			EditorGUILayout.PropertyField( projectileType_Prop ); 
			EditorGUILayout.Slider( atkRange_Prop, 0f, 1f);    
			break;
			
		case Weapon.WeaponType.hitscan:            
			EditorGUILayout.Slider( atkRange_Prop, 0f, 1f);    
			break;
			
		case Weapon.WeaponType.collider:            
			EditorGUILayout.PropertyField( attackCollider_Prop );    
			break;
		}

		EditorGUILayout.PropertyField(isInfinite);
		if(!(bool)isInfinite.boolValue){
			Rect rect = EditorGUILayout.BeginVertical();
			GUI.Box(rect,"");
			GUILayout.Space(5f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(ammoMax);
			GUILayout.Space(5f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.IntSlider(ammo,0,ammoMax.intValue);
			GUILayout.Space(5f);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.EndVertical();
		}

		for(int i=0;i<others.Count;i++){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(others[i]);
			othersBool[i] = EditorGUILayout.ToggleLeft("Show range",othersBool[i]);
			EditorGUILayout.EndHorizontal();
		}
		serializedObject.ApplyModifiedProperties ();
	}
}