using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This respresents the controller for the new map generation menu.
public class MapGenPanelController : MonoBehaviour
{
	// These are the pieces of data used to generate the new map.
	public string seed = null;
	public MapBuilder.MapType type = 0;

	// These are the instances of the input sections of this menu.
	public UnityEngine.UI.InputField SeedInputField;
	public UnityEngine.UI.Dropdown MapTypeDropdown;

	// Shows whether or the panel is active and viewable.
	public bool isActive;

	// Use this for initialization
	void Start ()
	{
		this.isActive = false;
		ToggleModal ();
	}

	// Toggles the menu modal.
	public void ToggleModal ()
	{
		this.isActive = !this.isActive;
		this.gameObject.SetActive (isActive);
	}

	// Generates a new map with the current data.
	public void GenerateMap ()
	{
		this.SetSeed ();
		this.SetMapType ();
		WorldController.Instance.GenerateWorld (seed, type);
		this.ToggleModal ();
	}

	// Sets this controller's seed to the current text in the input field.
	public void SetSeed ()
	{
		this.seed = SeedInputField.text;
	}

	// Sets this controller's mapType to the current value in the dropdown input field.
	public void SetMapType ()
	{
		this.type = (MapBuilder.MapType)MapTypeDropdown.value;
	}
}
