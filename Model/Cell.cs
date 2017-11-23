using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	private int x;
	private int y;

	public BuildingInstance Building { get; private set; }

	// TODO add terrain functionality

	public Cell (int x, int y)
	{
		this.x = x;
		this.y = y;
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
