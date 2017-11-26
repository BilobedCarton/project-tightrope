using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : MonoBehaviour
{
	public enum MapMode
	{
		BIOME,
		ELEVATION,
		TEMPERATURE
	}

	private World world;

	private MapMode mapMode;

	private Dictionary<string, Sprite> terrainSprites;

	private Dictionary<Cell, GameObject> cellGameObjectMap;

	// Use this for initialization
	void OnEnable ()
	{
		loadSprites ();

		cellGameObjectMap = new Dictionary<Cell, GameObject> ();
		mapMode = MapMode.ELEVATION;

		generateWorld ();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public void generateWorld ()
	{
		this.destroyAllCellGameObjects ();
		this.world = new World (65, 65, TerrainImporter.importTerrain ());
		this.createAllCellGameObjects (this.world);
		updateAllCellGameObjects ();
	}

	public void setMapMode (string mode)
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

		updateAllCellGameObjects ();
	}

	private void updateAllCellGameObjects ()
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
			cell_go.GetComponent <SpriteRenderer> ().sprite = terrainSprites [idPrefix + cell_data.getSpriteId (mapMode)];
		}
	}

	private void destroyAllCellGameObjects ()
	{
		while (cellGameObjectMap.Count > 0) {
			Cell cell_data = cellGameObjectMap.Keys.First ();
			GameObject cell_go = cellGameObjectMap [cell_data];

			cellGameObjectMap.Remove (cell_data);
			Destroy (cell_go);
		}
	}

	private void createAllCellGameObjects (World world)
	{
		// Build game objects for the world.
		for (int i = 0; i < world.Width; i++) {
			for (int j = 0; j < world.Length; j++) {
				Cell cell_data = world.getCellDataAt (i, j);
				GameObject cell_go = new GameObject ();

				cellGameObjectMap.Add (cell_data, cell_go);

				cell_go.name = cell_data.toString ();
				cell_go.transform.position = new Vector3 (cell_data.X, cell_data.Y, 1);
				cell_go.transform.SetParent (this.transform, true);

				cell_go.AddComponent <SpriteRenderer> ();
			}
		}
	}

	private void loadSprites ()
	{
		terrainSprites = new Dictionary<string, Sprite> ();
		Sprite[] terrains = Resources.LoadAll<Sprite> ("Sprites/Terrain/");
		foreach (Sprite s in terrains) {
			terrainSprites [s.name] = s;
		}
	}

	private Sprite getSpriteForTerrainType (Biome t)
	{
		return terrainSprites ["Terrain_" + t.Id];
	}
}
