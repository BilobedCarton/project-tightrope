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

	public Resource NaturalResource { get; private set; }

	public readonly float elevation;
	public readonly int temperature;

	private Sprite temperatureSprite;
	private Sprite elevationSprite;

	// TODO add terrain functionality

	// Creates a new Cell object at the given coords with the given data.
	public Cell (int x, int y, float elevation, int temperature, Biome t, Resource r = null)
	{
		this.X = x;
		this.Y = y;
		this.elevation = elevation;
		this.temperature = temperature;
		this.Terrain = t;
		this.NaturalResource = r;

		this.temperatureSprite = GetTemperatureSprite ();
		this.elevationSprite = GetHeightSprite ();
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

	public Sprite GetCellSprite (WorldController.MapMode mapMode)
	{
		switch (mapMode) {
		case WorldController.MapMode.BIOME:
			return WorldController.Instance.GetTerrainSprite (this.GetSpriteId (WorldController.MapMode.BIOME));
		case WorldController.MapMode.ELEVATION:
			return this.elevationSprite;
		case WorldController.MapMode.TEMPERATURE:
			return this.temperatureSprite;
		case WorldController.MapMode.RESOURCE:
			return WorldController.Instance.GetTerrainSprite (this.GetSpriteId (WorldController.MapMode.RESOURCE));
		default:
			Debug.LogError ("Cell.GetCellSprite(..) -- unrecognizable map mode");
			return null;
		}
	}

	private Sprite GetHeightSprite ()
	{
		Color c = new Color (0.5f + elevation * 0.01f, 0.5f + elevation * 0.01f, 0.5f + elevation * 0.01f);
		return CreateSprite (c);
	}

	private Sprite GetTemperatureSprite ()
	{
		Color c = new Color (0.5f + temperature * 0.01f, 0.0f, 0.5f - temperature * 0.01f);
		return CreateSprite (c);
	}

	private Sprite CreateSprite (Color c)
	{
		Texture2D t = new Texture2D (1, 1);
		t.SetPixels (new Color[] { c });
		t.wrapMode = TextureWrapMode.Repeat;
		t.Apply ();
		Sprite s = Sprite.Create (t, new Rect (0f, 0f, 1f, 1f), new Vector2 (0.5f, 0.5f), 1f);
		return s;
	}

	// Gets the id of the sprite for the given mapmode that correlates with this cell's data.
	private string GetSpriteId (WorldController.MapMode mapMode)
	{
		switch (mapMode) {
		case WorldController.MapMode.BIOME:
			return "Terrain_Biome_" + this.Terrain.Id;
		case WorldController.MapMode.RESOURCE:
			string naturalResourceId = this.NaturalResource != null ? this.NaturalResource.Id : "Empty";
			return "Terrain_Resource_" + naturalResourceId;
		default:
			Debug.LogError ("Cell.getSpriteId(..) -- invalid map mode");
			return "";
		}
	}
}
