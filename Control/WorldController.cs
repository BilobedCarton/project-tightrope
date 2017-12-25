using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

// Controls the world, acts as the medium between Unity and the Model.
public class WorldController : MonoBehaviour
{
	// For display purposes.
	public enum MapMode
	{
		BIOME,
		ELEVATION,
		TEMPERATURE
	}

	public static WorldController Instance;

	public Cell SelectedCell { get; private set; }

	private World world;

	private MapMode mapMode;

	private Dictionary<string, Sprite> terrainSprites;

	private Dictionary<Cell, GameObject> cellGameObjectMap;

	// Use this for initialization
	void OnEnable ()
	{
		if (Instance != null) {
			Debug.LogError ("WorldController.OnEnable() -- _instance should be null but isn't.");
		}

		Instance = this;

		LoadSprites ();

		cellGameObjectMap = new Dictionary<Cell, GameObject> ();
		mapMode = MapMode.BIOME;

		GenerateWorld (null, MapBuilder.MapType.ALPINE);
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	// Deletes anything involved in rendering the previous world and generates new instances of involved model objects and game objects.
	public void GenerateWorld (string seed, MapBuilder.MapType type)
	{
		this.DestroyAllCellGameObjects ();
		this.world = new World (65, 65, BiomeImporter.Import (), seed, type);
		this.CreateAllCellGameObjects (this.world);
		UpdateAllCellGameObjects ();
	}

	// Sets the map mode to the given value.
	public void SetMapMode (string mode)
	{
		switch (mode) {
		case "BIOME":
			mapMode = MapMode.BIOME;
			break;
		case "ELEVATION":
			mapMode = MapMode.ELEVATION;
			break;
		case "TEMPERATURE":
			mapMode = MapMode.TEMPERATURE;
			break;
		}

		UpdateAllCellGameObjects ();
	}

	// Update the cell's sprites based on map mode.
	private void UpdateAllCellGameObjects ()
	{
		string idPrefix = "Terrain_";
		switch (mapMode) {
		case MapMode.BIOME:
			idPrefix += "Biome_";
			break;
		case MapMode.ELEVATION:
			idPrefix += "Height_";
			break;
		case MapMode.TEMPERATURE:
			idPrefix += "Temperature_";
			break;
		}

		foreach (Cell cell_data in cellGameObjectMap.Keys) {
			GameObject cell_go = cellGameObjectMap [cell_data];
			cell_go.GetComponent <SpriteRenderer> ().sprite = terrainSprites [idPrefix + cell_data.GetSpriteId (mapMode)];
		}
	}

	// Deletes and destroys all game objects representing Cells.
	private void DestroyAllCellGameObjects ()
	{
		while (cellGameObjectMap.Count > 0) {
			Cell cell_data = cellGameObjectMap.Keys.First ();
			GameObject cell_go = cellGameObjectMap [cell_data];

			cellGameObjectMap.Remove (cell_data);
			Destroy (cell_go);
		}
	}

	// Creates new GameObjects to represent cells held within the given world.
	private void CreateAllCellGameObjects (World world)
	{
		// Build game objects for the world.
		for (int i = 0; i < world.Width; i++) {
			for (int j = 0; j < world.Length; j++) {
				Cell cell_data = world.GetCellDataAt (i, j);
				GameObject cell_go = new GameObject ();

				cellGameObjectMap.Add (cell_data, cell_go);

				cell_go.name = cell_data.ToString ();
				cell_go.transform.position = new Vector3 (cell_data.X, cell_data.Y, 1);
				cell_go.transform.SetParent (this.transform, true);

				cell_go.AddComponent <SpriteRenderer> ();
			}
		}
	}

	// Load the sprites for procedural objects.
	private void LoadSprites ()
	{
		terrainSprites = new Dictionary<string, Sprite> ();
		Sprite[] terrains = Resources.LoadAll<Sprite> ("Sprites/Terrain/");
		foreach (Sprite s in terrains) {
			terrainSprites [s.name] = s;
		}
	}

	public void SelectCellDataAtWorldCoord (Vector3 coord)
	{
		int x = Mathf.FloorToInt (coord.x + 0.5f);
		int y = Mathf.FloorToInt (coord.y + 0.5f);

		this.SelectedCell = this.world.GetCellDataAt (x, y);
	}
}
