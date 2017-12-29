using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a factory to create a height map based upon Diamond Square noise generation.
public class HeightMapGenerator
{
	public static int HD_POWER = 1;

	private int hdCoefficient { get { return (int)Math.Pow (2, HD_POWER); } }

	private System.Random picker;
	private string seed;
	private float[,] heightMap;
	private int randRange;

	// Creates a new HeightMapGenerator with the given seed, width, and length.
	private HeightMapGenerator (System.Random picker, int width, int length, float[] settings)
	{
		int hdWidth = (width - 1) * hdCoefficient + 1;
		int hdLength = (width - 1) * hdCoefficient + 1;
		this.picker = picker;

		heightMap = new float[hdWidth, hdLength];
		heightMap [0, 0] = settings [0];
		heightMap [0, hdWidth - 1] = settings [1];
		heightMap [hdLength - 1, 0] = settings [2];
		heightMap [hdLength - 1, hdWidth - 1] = settings [3];
		randRange = (int)settings [4];
		this.RunDiamondSquareStep (0, 0, hdWidth - 1, hdLength - 1);
	}

	// Generate the heights using an implementation of diamond square height map generation.
	// Assume width / length are powers of 2 plus 1
	public static float[,] BuildHeightMap (System.Random picker, int width, int length, float[] settings)
	{
		HeightMapGenerator generator = new HeightMapGenerator (picker, width, length, settings);
		MapBuilder.AverageValues (generator.heightMap);
		float[,] actualHeightMap = new float[width, length];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				actualHeightMap [i, j] = generator.getAverageOfHDSection (i, j);
			}
		}
		return generator.heightMap;
	}

	private float getAverageOfHDSection (int x, int y)
	{
		int width = hdCoefficient, length = hdCoefficient;
		float[,] hdSection = new float[width, length];
		int dX = 0;
		int dY = 0;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				dX = i;
				dY = j;
				if (x == (heightMap.GetLength (0) - 1) / width) {
					dX = 0;
				}
				if (y == (heightMap.GetLength (1) - 1) / length) {
					dY = 0;
				}
				hdSection [i, j] = heightMap [x + dX, x + dY];
			}
		}

		float value = 0;
		foreach (var f in hdSection) {
			value += f;
		}
		return value / (width * length);
	}

	private int GetRandomBump (int x0, int y0, int x1, int y1)
	{
		int range = randRange * (((x1 - x0) * (y1 - y0)) / (heightMap.GetLength (0) * heightMap.GetLength (1))) + 10;
		return picker.Next (-range, range);
	}

	private void RunDiamondSquareStep (int x0, int y0, int x1, int y1)
	{
		//Debug.Log ("tl: (" + x0 + "," + y0 + "), br: (" + x1 + "," + y1 + ")");

		// diamond step
		heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)] = 
			((heightMap [x0, y0] + heightMap [x1, y0] + heightMap [x1, y1] + heightMap [x0, y1]) / 4)
		+ GetRandomBump (x0, y0, x1, y1);

		// square step
		if (x0 == 0) {
			heightMap [0, GetMidpoint (y0, y1)] = ((heightMap [0, y0]
			+ heightMap [x1 / 2, GetMidpoint (y0, y1)]
			+ heightMap [0, y1]
			+ heightMap [(heightMap.GetLength (0) - 1) - (x1 / 2), GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		} else {
			heightMap [x0, GetMidpoint (y0, y1)] = ((heightMap [x0, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0 - x1, x0), (y1 - y0) / 2]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		}

		if (y0 == 0) {
			heightMap [GetMidpoint (x0, x1), 0] = ((heightMap [x0, 0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, 0]
			+ heightMap [GetMidpoint (x0, x1), (heightMap.GetLength (1) - 1) - (y1 / 2)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		} else {
			heightMap [GetMidpoint (x0, x1), y0] = ((heightMap [x0, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0 - y1, y0)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		}

		if (x1 == heightMap.GetLength (0) - 1) {
			heightMap [x1, GetMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [x1 - x0 / 2, GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		} else {
			heightMap [x1, GetMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x1, (2 * x1) - x0), GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		}

		if (y1 == heightMap.GetLength (1) - 1) {
			heightMap [GetMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x0, x1), (y1 - y0) / 2]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		} else {
			heightMap [GetMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y1, (2 * y1) - y0)]) / 4)
			+ GetRandomBump (x0, y0, x1, y1);
		}
		if (x1 - x0 == 2 || y1 - y0 == 2) {
			return;
		} 

		// Run the steps on the subdivided squares
		RunDiamondSquareStep (x0, y0, GetMidpoint (x0, x1), GetMidpoint (y0, y1));
		RunDiamondSquareStep (GetMidpoint (x0, x1), y0, x1, GetMidpoint (y0, y1));
		RunDiamondSquareStep (GetMidpoint (x0, x1), GetMidpoint (y0, y1), x1, y1);
		RunDiamondSquareStep (x0, GetMidpoint (y0, y1), GetMidpoint (x0, x1), y1);
	}

	private int GetMidpoint (int p1, int p2)
	{
		if (p1 < p2) {
			return p1 + ((p2 - p1) / 2);
		} else if (p1 > p2) {
			return p2 + ((p1 - p2) / 2);
		}
		return p1;
	}
}
