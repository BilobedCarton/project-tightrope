using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticSize : MonoBehaviour
{
	public float ChildHeight = 25f;
	public float ChildWidth = 50f;

	// Use this for initialization
	void Start ()
	{
		AdjustSize ();
	}

	public void AdjustSize ()
	{
		Vector2 size = this.GetComponent<RectTransform> ().sizeDelta;
		size.x = this.transform.childCount * ChildWidth;
		size.y = this.transform.childCount * ChildHeight;
		this.GetComponent<RectTransform> ().sizeDelta = size;
	}
}
