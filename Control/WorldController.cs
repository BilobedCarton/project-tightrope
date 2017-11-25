using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
	private World world;

	private Dictionary<string, Sprite> terrainSprites;

	private Dictionary<Cell, GameObject> cellGameObjectMap;

	// Use this for initialization
	void OnEnable ()
	{
		loadSprites ();

		this.world = new World (65, 65, TerrainImporter.importTerrain ());

		// Build game objects for the world.
		cellGameObjectMap = new Dictionary<Cell, GameObject> ();
		for (int i = 0; i < world.Width; i++) {
			for (int j = 0; j < world.Length; j++) {
				Cell cell_data = world.getCellDataAt (i, j);
				GameObject cell_go = new GameObject ();

				cellGameObjectMap.Add (cell_data, cell_go);

				cell_go.name = cell_data.toString ();
				cell_go.transform.position = new Vector3 (cell_data.X, cell_data.Y, 1);
				cell_go.transform.SetParent (this.transform, true);

				cell_go.AddComponent <SpriteRenderer> ();
				cell_go.GetComponent <SpriteRenderer> ().sprite = terrainSprites ["Terrain_" + cell_data.Terrain.Id];
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	private void loadSprites ()
	{
		terrainSprites = new Dictionary<string, Sprite> ();
		Sprite[] terrains = Resources.LoadAll<Sprite> ("Sprites/Terrain/Elevation");
		foreach (Sprite s in terrains) {
			terrainSprites [s.name] = s;
		}
	}

	private Sprite getSpriteForTerrainType (TerrainType t)
	{
		return terrainSprites ["Terrain_" + t.Id];
	}
}
