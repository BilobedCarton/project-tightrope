using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(AutomaticSize))]
public class AutomaticSizeEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		if (GUILayout.Button ("Recalc Size")) {
			((AutomaticSize)target).AdjustSize ();
		}
	}
}
