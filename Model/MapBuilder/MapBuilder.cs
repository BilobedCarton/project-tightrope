using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder
{
	public enum MapType
	{
		HIGHLANDS,
		GRASSLAND,
		ISLAND,
		ALPINE
	}

	private MapType type;
	private bool isIsland;
	private bool isOcean;

	private int width;
	private int length;

	private Cell[,] map;

	private List<TerrainType> potentialTerrain;

	private System.Random picker;

	public static Cell[,] buildMap (
		MapType type, 
		bool isIsland, 
		bool isOcean, 
		int width, 
		int length, 
		List<TerrainType> potentialTerrain)
	{
		MapBuilder builder = new MapBuilder {
			type = type,
			isIsland = isIsland,
			isOcean = isOcean,
			width = width,
			length = length,
			map = new Cell[width, length],
			potentialTerrain = potentialTerrain,
			picker = new System.Random ()
		};

		float[,] heightMap = HeightMapGenerator.buildHeightMap ("seed", width, length);
		int[,] temperatureMap = builder.generateTemperatures (heightMap);
		// do some stuff

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				builder.map [i, j] = new Cell (i, j, builder.selectTerrain (heightMap [i, j], temperatureMap [i, j]));
				Debug.Log (builder.map [i, j].toString () + "_height_" + heightMap [i, j] + "_temperature_" + temperatureMap [i, j]);
			}
		}

		return builder.map;
	}

	// in celcius.
	private int[,] generateTemperatures (float[,] heights)
	{
		int[,] temperatureMap = new int[width, length];
		float currHeight;
		int temperatureModifier;
		switch (type) {
		case MapType.ALPINE:
			temperatureModifier = -5;
			break;
		case MapType.GRASSLAND:
			temperatureModifier = 0;
			break;
		case MapType.HIGHLANDS:
			temperatureModifier = -10;
			break;
		case MapType.ISLAND:
			temperatureModifier = 5;
			break;
		default:
			Debug.LogError ("MapBuilder.generateTemperatures(...) -- unknown map type.");
			temperatureModifier = 0;
			break;
		}
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				currHeight = heights [i, j];
				if (currHeight > 15) {
					temperatureMap [i, j] = picker.Next (0, 10) + temperatureModifier;
				} else if (currHeight > 10) {
					temperatureMap [i, j] = picker.Next (5, 15) + temperatureModifier;
				} else if (currHeight > 5) {
					temperatureMap [i, j] = picker.Next (10, 20) + temperatureModifier;
				} else if (currHeight > 0) {
					temperatureMap [i, j] = picker.Next (15, 25) + temperatureModifier;
				} else {
					temperatureMap [i, j] = picker.Next (20, 30) + temperatureModifier;
				}
			}
		}
		return temperatureMap;
	}

	private TerrainType selectTerrain (float height, int temperature)
	{
		List<TerrainType> options = new List<TerrainType> ();
		foreach (TerrainType t in potentialTerrain) {
			if (t.isOption (height, temperature)) {
				options.Add (t);
			}
		}

		if (options.Count == 0) {
			Debug.LogError ("MapBuilder.selectTerrain(...) -- No possible options for given data.");
			return null;
		}

		return options [picker.Next (0, options.Count)];
	}
}
