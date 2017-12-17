using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a factory to create a height map based upon Diamond Square noise generation.
public class HeightMapGenerator
{
	private static System.Random picker = new System.Random ();

	private string seed;
	private float[,] heightMap;
	private int randRange;

	// Creates a new HeightMapGenerator with the given seed, width, and length.
	private HeightMapGenerator (string seed, int width, int length)
	{
		if (seed.Length < 5) {
			seed += "44444";
		}

		int hdWidth = (width - 1) * 4 + 1;
		int hdLength = (width - 1) * 4 + 1;

		this.seed = seed;
		heightMap = new float[hdWidth, hdLength];
		heightMap [0, 0] = seed.ToCharArray () [0] % 20;
		heightMap [0, hdWidth - 1] = seed.ToCharArray () [1] % 20;
		heightMap [hdLength - 1, 0] = seed.ToCharArray () [2] % 20;
		heightMap [hdLength - 1, hdWidth - 1] = seed.ToCharArray () [3] % 20;
		randRange = seed.ToCharArray () [4] % 40;
		this.RunDiamondSquareStep (0, 0, hdWidth - 1, hdLength - 1);
	}

	// Generate the heights using an implementation of diamond square height map generation.
	// Assume width / length are powers of 2 plus 1
	public static float[,] BuildHeightMap (string seed, int width, int length)
	{
		HeightMapGenerator generator = new HeightMapGenerator (seed, width, length);
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
		float[,] hdSection = new float[4, 4];
		int dX = 0;
		int dY = 0;
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				dX = i;
				dY = j;
				if (x == (heightMap.GetLength (0) - 1) / 4) {
					dX = 0;
				}
				if (y == (heightMap.GetLength (1) - 1) / 4) {
					dY = 0;
				}
				hdSection [i, j] = heightMap [x + dX, x + dY];
			}
		}

		float value = 0;
		foreach (var f in hdSection) {
			value += f;
		}
		return value / 16;
	}

	private int GetRandomBump ()
	{
		return picker.Next (-randRange, randRange);
	}

	private void ReduceRandRange ()
	{
		randRange /= 2;
	}

	private void RunDiamondSquareStep (int x0, int y0, int x1, int y1)
	{
		//Debug.Log ("tl: (" + x0 + "," + y0 + "), br: (" + x1 + "," + y1 + ")");

		// diamond step
		heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)] = 
			((heightMap [x0, y0] + heightMap [x1, y0] + heightMap [x1, y1] + heightMap [x0, y1]) / 4)
		+ GetRandomBump ();

		// square step
		if (x0 == 0) {
			heightMap [0, GetMidpoint (y0, y1)] = ((heightMap [0, y0]
			+ heightMap [x1 / 2, GetMidpoint (y0, y1)]
			+ heightMap [0, y1]
			+ heightMap [(heightMap.GetLength (0) - 1) - (x1 / 2), GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump ();
		} else {
			heightMap [x0, GetMidpoint (y0, y1)] = ((heightMap [x0, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0 - x1, x0), (y1 - y0) / 2]) / 4)
			+ GetRandomBump ();
		}

		if (y0 == 0) {
			heightMap [GetMidpoint (x0, x1), 0] = ((heightMap [x0, 0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, 0]
			+ heightMap [GetMidpoint (x0, x1), (heightMap.GetLength (1) - 1) - (y1 / 2)]) / 4)
			+ GetRandomBump ();
		} else {
			heightMap [GetMidpoint (x0, x1), y0] = ((heightMap [x0, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0 - y1, y0)]) / 4)
			+ GetRandomBump ();
		}

		if (x1 == heightMap.GetLength (0) - 1) {
			heightMap [x1, GetMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [x1 - x0 / 2, GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump ();
		} else {
			heightMap [x1, GetMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x1, (2 * x1) - x0), GetMidpoint (y0, y1)]) / 4)
			+ GetRandomBump ();
		}

		if (y1 == heightMap.GetLength (1) - 1) {
			heightMap [GetMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x0, x1), (y1 - y0) / 2]) / 4)
			+ GetRandomBump ();
		} else {
			heightMap [GetMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [GetMidpoint (x0, x1), GetMidpoint (y1, (2 * y1) - y0)]) / 4)
			+ GetRandomBump ();
		}
		if (x1 - x0 == 2 || y1 - y0 == 2) {
			return;
		} 

		ReduceRandRange ();
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
