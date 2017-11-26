using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public int X { get; private set; }

	public int Y { get; private set; }

	public BuildingInstance Building { get; private set; }

	public Biome Terrain { get; private set; }

	private float elevation;
	private int temperature;

	// TODO add terrain functionality

	public Cell (int x, int y, float elevation, int temperature, Biome t)
	{
		this.X = x;
		this.Y = y;
		this.elevation = elevation;
		this.temperature = temperature;
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

	public string getSpriteId (WorldController.MapMode mapMode)
	{
		switch (mapMode) {
		case WorldController.MapMode.BIOME:
			return this.Terrain.Id;
		case WorldController.MapMode.ELEVATION:
			return this.getElevationId ();
		case WorldController.MapMode.TEMPERATURE:
			return this.getTemperatureId ();
		default:
			Debug.LogError ("Cell.getSpriteId(..) -- unrecognizable map mode");
			return "";
		}
	}

	private string getElevationId ()
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

	private string getTemperatureId ()
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
