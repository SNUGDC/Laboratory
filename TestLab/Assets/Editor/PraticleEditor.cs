using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DotParticle)), CanEditMultipleObjects]
public class PraticleEditor : Editor {
	public SerializedProperty 
		minimumLife,
		fadeRate,
		angle,
		colorRate;
	public List<SerializedProperty> others;
	public List<bool> othersBool;
	
	void OnEnable () {
		minimumLife = serializedObject.FindProperty("minimumLifetime");
		fadeRate = serializedObject.FindProperty("fadeRate");
		angle = serializedObject.FindProperty("angle");
		colorRate = serializedObject.FindProperty("colorChangeRate");

		others = new List<SerializedProperty>();
		others.Add(serializedObject.FindProperty ("duration"));
		others.Add(serializedObject.FindProperty ("lifetime"));
		others.Add(serializedObject.FindProperty ("looping"));
		others.Add(serializedObject.FindProperty ("moveable"));
		others.Add(serializedObject.FindProperty ("colorChange"));
		others.Add(serializedObject.FindProperty ("startColor"));
		others.Add(serializedObject.FindProperty ("endColor"));
		others.Add(serializedObject.FindProperty ("xySpeed"));
		others.Add(serializedObject.FindProperty ("zSpeed"));
		others.Add(serializedObject.FindProperty ("gravity"));
		others.Add(serializedObject.FindProperty ("bounce"));
		others.Add(serializedObject.FindProperty ("size"));
		others.Add(serializedObject.FindProperty ("pixelPerUnit"));
		others.Add(serializedObject.FindProperty ("bilinearFilter"));
		others.Add(serializedObject.FindProperty ("rate"));
		others.Add(serializedObject.FindProperty ("burst"));
		others.Add(serializedObject.FindProperty ("drawGizmo"));
		others.Add(serializedObject.FindProperty ("playOnEnable"));
		others.Add(serializedObject.FindProperty ("destroyOnEnd"));
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		EditorGUILayout.PropertyField(others[16]);
		EditorGUILayout.PropertyField(others[17]);
		if(!others[17].boolValue)
			EditorGUILayout.PropertyField(others[18]);

		//Begin Lifetime setting box
		Rect rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect,"");
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[0]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[1]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.Slider(minimumLife,0f,others[1].floatValue);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[2]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.EndVertical();
		//Close Lifetime setting box

		//Begin Spawning setting box
		rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect,"");
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[14]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[15]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.EndVertical();
		//Close Spawning setting box

		//Begin Color setting box
		rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect,"");
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[4]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		bool isColorChange = others[4].boolValue;
		if(isColorChange){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.Slider(colorRate,0.5f,0.99f);
			GUILayout.Space(5f);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.Slider(fadeRate,0.5f,0.99f);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(5f);
		EditorGUILayout.PropertyField(others[5]);
		GUILayout.Space(5f);
		EditorGUILayout.EndHorizontal();
		if(isColorChange){
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(others[6]);
			GUILayout.Space(5f);
			EditorGUILayout.EndHorizontal();
		}
		GUILayout.Space(5f);
		EditorGUILayout.EndVertical();
		//close Color settong box

		EditorGUILayout.PropertyField(others[3]);
		EditorGUILayout.Slider(angle,0f,1f);
		EditorGUILayout.PropertyField(others[7]);
		EditorGUILayout.PropertyField(others[8]);
		EditorGUILayout.PropertyField(others[9]);
		EditorGUILayout.PropertyField(others[10]);
		EditorGUILayout.IntField("Texture Size",others[11].intValue);
		EditorGUILayout.PropertyField(others[12]);
		EditorGUILayout.PropertyField(others[13]);

		serializedObject.ApplyModifiedProperties ();

	}
}