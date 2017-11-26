using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a factory to create a height map based upon Diamond Square noise generation.
public class HeightMapGenerator
{
	private static System.Random picker = new System.Random ();

	private string seed;
	private int width;
	private int length;
	private float[,] heightMap;
	private int randRange;

	// Creates a new HeightMapGenerator with the given seed, width, and length.
	private HeightMapGenerator (string seed, int width, int length)
	{
		if (seed.Length < 5) {
			seed += "44444";
		}

		this.seed = seed;
		this.width = width;
		this.length = length;
		heightMap = new float[width, length];
		heightMap [0, 0] = seed.ToCharArray () [0] % 20;
		heightMap [0, width - 1] = seed.ToCharArray () [1] % 20;
		heightMap [length - 1, 0] = seed.ToCharArray () [2] % 20;
		heightMap [length - 1, width - 1] = seed.ToCharArray () [3] % 20;
		randRange = seed.ToCharArray () [4] % 40;
	}

	// Generate the heights using an implementation of diamond square height map generation.
	// Assume width / length are powers of 2 plus 1
	public static float[,] BuildHeightMap (string seed, int width, int length)
	{
		HeightMapGenerator generator = new HeightMapGenerator (seed, width, length);
		generator.RunDiamondSquareStep (0, 0, width - 1, length - 1);
		MapBuilder.AverageValues (generator.heightMap);
		return generator.heightMap;
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
