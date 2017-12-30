using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a controller for the ingame main menu modal's functionality.
public class IngameMenuPanelController : MonoBehaviour
{
	// This is the current instance of the controller.
	public static IngameMenuPanelController Instance;

	// this informs us if the modal us currently active and viewable.
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

	// Toggles the modal.
	public void Toggle ()
	{
		this.isActive = !this.isActive;
		this.gameObject.SetActive (this.isActive);
	}
}
