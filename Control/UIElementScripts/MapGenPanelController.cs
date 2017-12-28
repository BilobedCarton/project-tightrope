using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenPanelController : MonoBehaviour
{
	public string seed = null;
	public MapBuilder.MapType type = MapBuilder.MapType.ALPINE;

	public UnityEngine.UI.InputField SeedInputField;
	public UnityEngine.UI.Dropdown MapTypeDropdown;

	public bool isActive;

	// Use this for initialization
	void Start ()
	{
		this.isActive = false;
		ToggleModal ();
	}

	public void ToggleModal ()
	{
		this.isActive = !this.isActive;
		this.gameObject.SetActive (isActive);
	}

	public void GenerateMap ()
	{
		WorldController.Instance.GenerateWorld (seed, type);
		this.ToggleModal ();
	}

	public void SetSeed ()
	{
		this.seed = SeedInputField.text;
	}

	public void SetMapType ()
	{
		this.type = (MapBuilder.MapType)MapTypeDropdown.value;
	}
}
