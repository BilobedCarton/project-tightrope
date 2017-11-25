using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public World (int width, int length, List<TerrainType> potentialTerrain)
	{
		this.cells = new Cell[width, length];
		this.buildings = new List<BuildingInstance> ();
		this.districts = new List<District> ();

		this.Width = width;
		this.Length = length;

		this.buildingPrototypes = new Dictionary<string, BuildingPrototype> ();
		this.resources = new Dictionary<string, Resource> ();

		List<string> resourceNames = new List<string> ();
		foreach (var resourceName in this.resources.Keys) {
			resourceNames.Add (resourceName);
		}
		this.player = new Player (this, resourceNames);
		this.corporations = new List<Corporation> ();
		this.nations = new List<Nation> ();

		this.cells = MapBuilder.buildMap (MapBuilder.MapType.GRASSLAND, false, false, width, length, potentialTerrain);
	}

	public bool placeBuildingInstance (Cell location, string name, IEntity owner)
	{
		if (buildingPrototypes.ContainsKey (name) == false) {
			Debug.LogError ("World.placeBuildingInstance(...) -- trying to build non-existent type of building.");
		}

		BuildingPrototype proto = buildingPrototypes [name];
		foreach (var resource in proto.ResourcesRequired) {
			if (owner.getResourceAmount (resource.Key) < resource.Value) {
				return false;
			}
		}

		foreach (var resource in proto.ResourcesRequired) {
			owner.changeResourceAmount (resource.Key, -resource.Value);
		}

		location.placeBuildingInstance (proto, owner);
		return true;
	}

	public int getWorldStockpile (string resourceName)
	{
		int stockpile = 0;
		stockpile += player.getResourceAmount (resourceName);
		foreach (Corporation c in this.corporations) {
			stockpile += c.getResourceAmount (resourceName);
		}
		foreach (Nation n in this.nations) {
			stockpile += n.getResourceAmount (resourceName);
		}
		return stockpile;
	}

	public int getNextStockpileChange (string resourceName)
	{
		int change = 0;
		foreach (BuildingInstance b in this.buildings) {
			change += b.getChangeInResource (resourceName);
		}
		return change;
	}

	public float getResourceCost (string name)
	{
		return this.resources [name].Price;
	}

	public Cell getCellDataAt (int x, int y)
	{
		if (x >= Width || y >= Length) {
			Debug.LogError ("World.getCellDataAt(...) -- trying to access cell at non-existent indices.");
			return null;
		} 
		return cells [x, y];
	}

	public void runTick ()
	{
		foreach (var b in buildings) {
			b.runTick (this);
		}
		foreach (var r in resources.Values) {
			r.calculatePrice (this);
		}
	}
}
