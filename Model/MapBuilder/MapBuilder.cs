﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a builder to create a new Map based upon a various settings.
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

	private List<Biome> potentialBiomes;

	private System.Random picker;

	// Builds a new map of Cell objects.
	public static Cell[,] BuildMap (
		MapType type, 
		bool isIsland, 
		bool isOcean, 
		int width, 
		int length, 
		List<Biome> potentialBiomes,
		string seed)
	{
		MapBuilder builder = new MapBuilder {
			type = type,
			isIsland = isIsland,
			isOcean = isOcean,
			width = width,
			length = length,
			map = new Cell[width, length],
			potentialBiomes = potentialBiomes,
			picker = new System.Random ()
		};

		float[] settings;
		switch (type) {
		case MapType.ALPINE:
			settings = new float[]{ 10, 10, 10, 10, 30 };
			break;
		case MapType.GRASSLAND:
			settings = new float[]{ 0, 0, 0, 0, 10 };
			break;
		case MapType.HIGHLANDS:
			settings = new float[]{ 20, 20, 20, 20, 15 };
			break;
		case MapType.ISLAND:
			settings = new float[]{ -5, -5, -5, -5, 30 };
			break;
		default:
			settings = new float[]{ 0, 0, 0, 0, 50 };
			break;
		}

		float[,] heightMap = HeightMapGenerator.BuildHeightMap (seed, width, length, settings);
		int[,] temperatureMap = builder.GenerateTemperatures (heightMap);

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				builder.map [i, j] = new Cell (
					i, j, heightMap [i, j], temperatureMap [i, j], builder.SelectTerrain (heightMap [i, j], temperatureMap [i, j]));
			}
		}

		return builder.map;
	}

	// Averages the values in a 2d array of ints to be similar to their neighbours (smooths the differences between neighbours).
	public static void AverageValues (int[,] values)
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

	// Same as above but for floats.
	public static void AverageValues (float[,] values)
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

	// Generates a 2d array of temperatures in celcius based on the given heights;.
	private int[,] GenerateTemperatures (float[,] heights)
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
		AverageValues (temperatureMap);
		return temperatureMap;
	}

	// Selects a biome for the cell based on the given data.
	private Biome SelectTerrain (float height, int temperature)
	{
		List<Biome> options = new List<Biome> ();
		foreach (Biome t in potentialBiomes) {
			if (t.IsOption (height, temperature)) {
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
