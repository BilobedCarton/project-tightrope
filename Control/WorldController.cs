﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

// Controls the world, acts as the medium between Unity and the Model.
public class WorldController : MonoBehaviour
{
	// For display purposes.
	public enum MapMode
	{
		BIOME,
		ELEVATION,
		TEMPERATURE,
		RESOURCE
	}

	public static WorldController Instance;

	public Cell SelectedCell { get; private set; }

	public MapBuilder.MapType mapType;

	private World world;

	private MapMode mapMode;

	private Dictionary<string, Sprite> importedTerrainSprites;

	private Dictionary<Color, Sprite> createdTerrainSprites;

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
		this.mapType = mapType;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	// Deletes anything involved in rendering the previous world and generates new instances of involved model objects and game objects.
	public void GenerateWorld (string seed, MapBuilder.MapType type)
	{
		this.DestroyAllCellGameObjects ();
		this.world = new World (65, 65, BiomeImporter.Import (), ResourceImporter.Import (), BuildingImporter.Import (), seed, type);
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
		case "RESOURCE":
			mapMode = MapMode.RESOURCE;
			break;
		}

		UpdateAllCellGameObjects ();
	}

	// Update the cell's sprites based on map mode.
	private void UpdateAllCellGameObjects ()
	{
		foreach (Cell cell_data in cellGameObjectMap.Keys) {
			GameObject cell_go = cellGameObjectMap [cell_data];
			cell_go.GetComponent <SpriteRenderer> ().sprite = cell_data.GetCellSprite (this.mapMode);
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

	// Creates a 1x1 pixel sprite with the given color.
	private Sprite CreateCellSprite (Color c)
	{
		Texture2D t = new Texture2D (1, 1);
		t.SetPixels (new Color[] { c });
		t.wrapMode = TextureWrapMode.Repeat;
		t.Apply ();
		Sprite s = Sprite.Create (t, new Rect (0f, 0f, 1f, 1f), new Vector2 (0.5f, 0.5f), 1f);
		return s;
	}

	// Load the sprites for procedural objects.
	private void LoadSprites ()
	{
		importedTerrainSprites = new Dictionary<string, Sprite> ();
		Sprite[] terrains = Resources.LoadAll<Sprite> ("Sprites/Terrain/");
		foreach (Sprite s in terrains) {
			importedTerrainSprites [s.name] = s;
		}

		createdTerrainSprites = new Dictionary<Color, Sprite> ();
		Color e;
		Color t;
		for (int i = -50; i < 50; i++) {
			e = new Color (0.5f + i * 0.01f, 0.5f + i * 0.01f, 0.5f + i * 0.01f);
			t = new Color (0.5f + i * 0.01f, 0.0f, 0.5f - i * 0.01f);
			createdTerrainSprites [e] = CreateCellSprite (e);
			createdTerrainSprites [t] = CreateCellSprite (t);
		}
	}

	// Returns the imported sprite with the given name.
	public Sprite GetImportedTerrainSprite (string name)
	{
		return importedTerrainSprites [name];
	}

	// Returns the generated sprite with the given color.
	public Sprite GetCreatedTerrainSprite (Color c)
	{
		return createdTerrainSprites [c];
	}

	// Selects the cell at the given world coordinate.
	public void SelectCellDataAtWorldCoord (Vector3 coord)
	{
		int x = Mathf.FloorToInt (coord.x + 0.5f);
		int y = Mathf.FloorToInt (coord.y + 0.5f);

		this.SelectedCell = this.world.GetCellDataAt (x, y);
	}

	// Exits the game.
	public void Exit ()
	{
		this.DestroyAllCellGameObjects ();
		Application.Quit ();
	}

	///////////////////////////////////////////////////////////////
	/// SAVING AND LOADING
	///////////////////////////////////////////////////////////////

	public void Save (string fileName)
	{
		XmlWriter writer = new XmlTextWriter ("Assets/Resources/Data/Saves/" + fileName + ".xml", System.Text.ASCIIEncoding.ASCII);
		writer.WriteRaw ("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
		this.world.Save (writer);
		writer.Close ();
	}

	public void Load (string fileName)
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/Saves/" + fileName + ".xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);
		XmlElement worldElement = (XmlElement)doc.GetElementsByTagName ("World") [0];
		this.world.Load (worldElement);
		this.DestroyAllCellGameObjects ();
		this.CreateAllCellGameObjects (this.world);
		this.UpdateAllCellGameObjects ();
		fs.Close ();
	}
}
