using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a square on a map, or a cell.
public class Cell
{
	public int X { get; private set; }

	public int Y { get; private set; }

	public BuildingInstance Building { get; private set; }

	public Biome Terrain { get; private set; }

	private float elevation;
	private int temperature;

	// TODO add terrain functionality

	// Creates a new Cell object at the given coords with the given data.
	public Cell (int x, int y, float elevation, int temperature, Biome t)
	{
		this.X = x;
		this.Y = y;
		this.elevation = elevation;
		this.temperature = temperature;
		this.Terrain = t;
	}

	// Converts this cell to a string.
	public string ToString ()
	{
		return "Cell_" + X + "_" + Y;
	}

	// Places an instance of the given building on this cell with the given owner.
	public void PlaceBuildingInstance (BuildingPrototype proto, IEntity owner)
	{
		this.Building = proto.BuildInstance (this, owner);
	}

	// Gets the id of the sprite for the given mapmode that correlates with this cell's data.
	public string GetSpriteId (WorldController.MapMode mapMode)
	{
		switch (mapMode) {
		case WorldController.MapMode.BIOME:
			return this.Terrain.Id;
		case WorldController.MapMode.ELEVATION:
			return this.GetElevationId ();
		case WorldController.MapMode.TEMPERATURE:
			return this.GetTemperatureId ();
		default:
			Debug.LogError ("Cell.getSpriteId(..) -- unrecognizable map mode");
			return "";
		}
	}

	// Gets the id of this cell for an Elevation map display.
	private string GetElevationId ()
	{
		if (elevation < 0) {
			return "0";
		} else if (elevation < 5) {
			return "1";
		} else if (elevation < 10) {
			return "2";
		} else if (elevation < 15) {
			return "3";
		} else if (elevation < 20) {
			return "4";
		} else if (elevation < 25) {
			return "5";
		} else if (elevation < 30) {
			return "6";
		} else {
			return "7";
		}
	}

	// Gets the id of this cell for a Temperature map display.
	private string GetTemperatureId ()
	{
		if (temperature < -10) {
			return "0";
		} else if (temperature < 0) {
			return "1";
		} else if (temperature < 6) {
			return "2";
		} else if (temperature < 14) {
			return "3";
		} else if (temperature < 22) {
			return "4";
		} else if (temperature < 30) {
			return "5";
		} else if (temperature < 35) {
			return "6";
		} else {
			return "7";
		}
	}
}
