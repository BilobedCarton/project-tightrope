using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenPanelController : MonoBehaviour
{
	public string seed = null;
	public MapBuilder.MapType type = MapBuilder.MapType.ALPINE;

	public UnityEngine.UI.InputField SeedInputField;
	public UnityEngine.UI.Dropdown MapTypeDropdown;

	private bool isActive;

	// Use this for initialization
	void Start ()
	{
		this.isActive = true;
		this.ToggleModal ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ToggleModal ()
	{
		this.isActive = !this.isActive;
		this.gameObject.SetActive (isActive);
	}

	public void GenerateMap ()
	{
		WorldController.Instance.GenerateWorld (seed, type);
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
