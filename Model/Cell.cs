using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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
	}

	// Converts this cell to a string.
	public override string ToString ()
	{
		return "Cell_" + X + "_" + Y;
	}

	// Places an instance of the given building on this cell with the given owner.
	public void PlaceBuildingInstance (BuildingPrototype proto, IEntity owner)
	{
		this.Building = proto.BuildInstance (this, owner);
	}

	// Returns the relevant sprite for this cell object.
	public Sprite GetCellSprite (WorldController.MapMode mapMode)
	{
		switch (mapMode) {
		case WorldController.MapMode.BIOME:
			return WorldController.Instance.GetImportedTerrainSprite (this.GetSpriteId (WorldController.MapMode.BIOME));
		case WorldController.MapMode.ELEVATION:
			return WorldController.Instance.GetCreatedTerrainSprite (this.GetHeightColor ());
		case WorldController.MapMode.TEMPERATURE:
			return WorldController.Instance.GetCreatedTerrainSprite (this.GetTemperatureColor ());
		case WorldController.MapMode.RESOURCE:
			return WorldController.Instance.GetImportedTerrainSprite (this.GetSpriteId (WorldController.MapMode.RESOURCE));
		default:
			Debug.LogError ("Cell.GetCellSprite(..) -- unrecognizable map mode");
			return null;
		}
	}

	// Returns the color that reflects the height of this cell.
	private Color GetHeightColor ()
	{
		int elevation = (int)Mathf.Floor (this.elevation);
		return new Color (0.5f + elevation * 0.01f, 0.5f + elevation * 0.01f, 0.5f + elevation * 0.01f);
	}

	// Returns the color that reflects the temperature of this cell.
	private Color GetTemperatureColor ()
	{
		int temperature = (int)Mathf.Floor (this.temperature);
		return new Color (0.5f + temperature * 0.01f, 0.0f, 0.5f - temperature * 0.01f);
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

	///////////////////////////////////////////////////////////////
	/// SAVING AND LOADING
	///////////////////////////////////////////////////////////////

	public void Save (XmlWriter writer)
	{
		writer.WriteStartElement ("Cell");
		writer.WriteAttributeString ("x", this.X.ToString ());
		writer.WriteAttributeString ("y", this.Y.ToString ());
		writer.WriteAttributeString ("elevation", this.elevation.ToString ());
		writer.WriteAttributeString ("temperature", this.temperature.ToString ());
		writer.WriteElementString ("TerrainId", this.Terrain != null ? this.Terrain.Id : "");
		writer.WriteElementString ("ResourceId", this.NaturalResource != null ? this.NaturalResource.Id : "");
		writer.WriteElementString ("Building", this.Building != null ? this.Building.GetName () : "");
		writer.WriteEndElement ();
	}

	public static Cell Load (XmlElement cellElement, Dictionary<string, Biome> biomes, Dictionary<string, Resource> resources)
	{
		Cell c = new Cell (
			         int.Parse (cellElement.GetAttribute ("x")), 
			         int.Parse (cellElement.GetAttribute ("y")),
			         float.Parse (cellElement.GetAttribute ("elevation")),
			         int.Parse (cellElement.GetAttribute ("temperature")),
			         biomes [cellElement.FirstChild.InnerText]);
		if (cellElement.ChildNodes [1].InnerXml.Equals ("") == false) {
			c.NaturalResource = resources [cellElement.ChildNodes [1].InnerXml];
		}
		return c;
	}
}
