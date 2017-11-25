using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public int X { get; private set; }

	public int Y { get; private set; }

	public BuildingInstance Building { get; private set; }

	public TerrainType Terrain { get; private set; }

	// TODO add terrain functionality

	public Cell (int x, int y, TerrainType t)
	{
		this.X = x;
		this.Y = y;
		this.Terrain = t;
	}

	public string toString ()
	{
		return "Cell_" + X + "_" + Y;
	}

	public void placeBuildingInstance (BuildingPrototype proto, IEntity owner)
	{
		this.Building = proto.buildInstance (this, owner);
	}
}
