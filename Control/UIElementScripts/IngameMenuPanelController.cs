using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuPanelController : MonoBehaviour
{
	public static IngameMenuPanelController Instance;

	public bool isActive;

	// Use this for initialization
	void Start ()
	{
		if (Instance != null) {
			Debug.LogError ("IngameMenuPanelController.Start() -- Instance should be null but isn't.");
		}

		Instance = this;

		this.isActive = false;
	}

	public void Toggle ()
	{
		this.isActive = !this.isActive;
		this.gameObject.SetActive (this.isActive);
	}
}
