using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	private Cell[,] cells;
	private List<BuildingInstance> buildings;
	private List<District> districts;

	private int width;
	private int length;

	private Dictionary<string, BuildingPrototype> buildingPrototypes;
	private List<string> resources;

	private Player player;
	private List<Corporation> corporations;
	private List<Nation> nations;

	public World (int width, int length)
	{
		this.cells = new Cell[width, length];
		this.buildings = new List<BuildingInstance> ();
		this.districts = new List<District> ();

		this.width = width;
		this.length = length;

		this.buildingPrototypes = new Dictionary<string, BuildingPrototype> ();
		this.resources = new List<string> ();

		this.player = new Player (this, this.resources);
		this.corporations = new List<Corporation> ();
		this.nations = new List<Nation> ();
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

	public void runTick ()
	{
		foreach (var b in buildings) {
			b.runTick ();
		}
	}
}
