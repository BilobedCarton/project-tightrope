using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a controller for the tile info panel.
public class TileInfoPanelController : MonoBehaviour
{
	// This is the current instance of the controller.
	public static TileInfoPanelController Instance;

	// This is the text element used to display the info.
	public UnityEngine.UI.Text InfoText;

	// Use this for initialization
	void Start ()
	{
		if (Instance != null) {
			Debug.LogError ("TileInfoPanelController.OnEnable() -- Instance should be null but isn't.");
		}

		Instance = this;
		this.Toggle ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (WorldController.Instance.SelectedCell != null) {
			Cell selectedCell = WorldController.Instance.SelectedCell;
			string resourceName = selectedCell.NaturalResource == null ? "None" : selectedCell.NaturalResource.Name;
			string buildingName = selectedCell.Building == null ? "None" : selectedCell.Building.GetName ();
			this.InfoText.text = "Coords: " + selectedCell.X + ", " + selectedCell.Y + "\n"
			+ "Biome: " + selectedCell.Terrain.Name + "\n"
			+ "Elevation: " + selectedCell.elevation + "\n"
			+ "Temperature: " + selectedCell.temperature + "\n"
			+ "Resource: " + resourceName + "\n"
			+ "Building: " + buildingName;
		}
	}

	// Toggles the visibility and activity of this panel depending on if a cell is currently selected.
	public void Toggle ()
	{
		if (WorldController.Instance.SelectedCell != null) {
			this.gameObject.SetActive (true);
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
