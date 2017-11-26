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

	private List<Biome> potentialTerrain;

	private System.Random picker;

	public static Cell[,] buildMap (
		MapType type, 
		bool isIsland, 
		bool isOcean, 
		int width, 
		int length, 
		List<Biome> potentialTerrain)
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

		float[,] heightMap = HeightMapGenerator.buildHeightMap ("food", width, length);
		int[,] temperatureMap = builder.generateTemperatures (heightMap);
		// do some stuff

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				builder.map [i, j] = new Cell (
					i, j, heightMap [i, j], temperatureMap [i, j], builder.selectTerrain (heightMap [i, j], temperatureMap [i, j]));
			}
		}

		return builder.map;
	}

	public static void averageValues (int[,] values)
	{
		int leftVal, topVal, rightVal, botVal;
		for (int i = 0; i < values.GetLength (0); i++) {
			for (int j = 0; j < values.GetLength (1); j++) {
				leftVal = i == 0 ? values [i, j] : values [i - 1, j];
				topVal = j == 0 ? values [i, j] : values [i, j - 1];
				rightVal = i == values.GetLength (0) - 1 ? values [i, j] : values [i + 1, j];
				botVal = j == values.GetLength (1) - 1 ? values [i, j] : values [i, j + 1];

				values [i, j] = (values [i, j] + leftVal + topVal + rightVal + botVal) / 5;
			}
		}
	}

	public static void averageValues (float[,] values)
	{
		float leftVal, topVal, rightVal, botVal;
		for (int i = 0; i < values.GetLength (0); i++) {
			for (int j = 0; j < values.GetLength (1); j++) {
				leftVal = i == 0 ? values [i, j] : values [i - 1, j];
				topVal = j == 0 ? values [i, j] : values [i, j - 1];
				rightVal = i == values.GetLength (0) - 1 ? values [i, j] : values [i + 1, j];
				botVal = j == values.GetLength (1) - 1 ? values [i, j] : values [i, j + 1];

				values [i, j] = (values [i, j] + leftVal + topVal + rightVal + botVal) / 5;
			}
		}
	}

	// in celcius.
	private int[,] generateTemperatures (float[,] heights)
	{
		int[,] temperatureMap = new int[width, length];
		int temperatureModifier;
		switch (type) {
		case MapType.ALPINE:
			temperatureModifier = -10;
			break;
		case MapType.GRASSLAND:
			temperatureModifier = 0;
			break;
		case MapType.HIGHLANDS:
			temperatureModifier = -15;
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
				if (heights [i, j] > 0) {
					temperatureMap [i, j] = (25 - (int)heights [i, j]) + picker.Next (-4, 2) + temperatureModifier;
				} else {
					temperatureMap [i, j] = 25 + picker.Next (-2, 2) + temperatureModifier;
				}
			}
		}
		averageValues (temperatureMap);
		return temperatureMap;
	}

	private Biome selectTerrain (float height, int temperature)
	{
		List<Biome> options = new List<Biome> ();
		foreach (Biome t in potentialTerrain) {
			if (t.isOption (height, temperature)) {
				options.Add (t);
			}
		}

		if (options.Count == 0) {
			Debug.LogError ("MapBuilder.selectTerrain(...) -- No possible options for given data. Height: "
			+ height + ", Temperature: " + temperature);
			return null;
		}

		return options [picker.Next (0, options.Count)];
	}
}
