using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfoPanelController : MonoBehaviour
{
	public static TileInfoPanelController Instance;

	public UnityEngine.UI.Text InfoText;

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

	public void Toggle ()
	{
		if (WorldController.Instance.SelectedCell != null) {
			this.gameObject.SetActive (true);
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
