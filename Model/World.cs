using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a model of the world of this game.
public class World
{
	private Cell[,] cells;
	private List<BuildingInstance> buildings;
	private List<District> districts;

	public int Width { get; private set; }

	public int Length { get; private set; }

	private Dictionary<string, BuildingPrototype> buildingPrototypes;
	private Dictionary<string, Resource> resources;

	private Player player;
	private List<Corporation> corporations;
	private List<Nation> nations;

	// Creates a new World with the given width and length, as well as the given possible Biome types.
	public World (int width, int length, List<Biome> potentialBiomes, List<Resource> resources, string seed, MapBuilder.MapType type)
	{
		this.cells = new Cell[width, length];
		this.buildings = new List<BuildingInstance> ();
		this.districts = new List<District> ();

		this.Width = width;
		this.Length = length;

		this.buildingPrototypes = new Dictionary<string, BuildingPrototype> ();
		this.resources = new Dictionary<string, Resource> ();
		foreach (Resource r in resources) {
			this.resources.Add (r.Name, r);
		}
		this.resources.Add ("Empty", null); 

		List<string> resourceNames = new List<string> ();
		foreach (var resourceName in this.resources.Keys) {
			resourceNames.Add (resourceName);
		}
		this.player = new Player (this, resourceNames);
		this.corporations = new List<Corporation> ();
		this.nations = new List<Nation> ();

		this.cells = MapBuilder.BuildMap (type, false, false, width, length, potentialBiomes, this.resources, seed);
	}

	// Places an instance of the given building in the given location with the given owner.
	public bool PlaceBuildingInstance (Cell location, string name, IEntity owner)
	{
		if (buildingPrototypes.ContainsKey (name) == false) {
			Debug.LogError ("World.placeBuildingInstance(...) -- trying to build non-existent type of building.");
		}

		BuildingPrototype proto = buildingPrototypes [name];
		foreach (var resource in proto.ResourcesRequired) {
			if (owner.GetResourceAmount (resource.Key) < resource.Value) {
				return false;
			}
		}

		foreach (var resource in proto.ResourcesRequired) {
			owner.ChangeResourceAmount (resource.Key, -resource.Value);
		}

		location.PlaceBuildingInstance (proto, owner);
		return true;
	}

	// Gets the total amount of a given resource in existence in the world.
	public int GetWorldStockpile (string resourceName)
	{
		int stockpile = 0;
		stockpile += player.GetResourceAmount (resourceName);
		foreach (Corporation c in this.corporations) {
			stockpile += c.GetResourceAmount (resourceName);
		}
		foreach (Nation n in this.nations) {
			stockpile += n.GetResourceAmount (resourceName);
		}
		return stockpile;
	}

	// Gets the next chnage in the world stockpile of a given resource.
	public int GetNextStockpileChange (string resourceName)
	{
		int change = 0;
		foreach (BuildingInstance b in this.buildings) {
			change += b.GetChangeInResource (resourceName);
		}
		return change;
	}

	// Gets the current value of a given resource.
	public float GetResourceCost (string name)
	{
		return this.resources [name].Price;
	}

	// Gets the Cell object at the given coordinates.
	public Cell GetCellDataAt (int x, int y)
	{
		if (x >= Width || x < 0 || y >= Length || y < 0) {
			// Debug.Log ("World.getCellDataAt(...) -- trying to access cell at non-existent indices: (" + x + "," + y + ").");
			return null;
		} 
		return cells [x, y];
	}

	// Runs a single tick of execution on this world.
	public void RunTick ()
	{
		foreach (var b in buildings) {
			b.RunTick (this);
		}
		foreach (var r in resources.Values) {
			r.CalculatePrice (this);
		}
	}
}
