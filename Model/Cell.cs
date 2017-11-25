using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	private int x;
	private int y;

	public BuildingInstance Building { get; private set; }

	public TerrainType Terrain { get; private set; }

	// TODO add terrain functionality

	public Cell (int x, int y, TerrainType t)
	{
		this.x = x;
		this.y = y;
		this.Terrain = t;
	}

	public string toString ()
	{
		return "Cell_" + x + "_" + y;
	}

	public void placeBuildingInstance (BuildingPrototype proto, IEntity owner)
	{
		this.Building = proto.buildInstance (this, owner);
	}
}
