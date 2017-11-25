using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapGenerator
{
	private static System.Random picker = new System.Random ();

	private string seed;
	private float[,] heightMap;

	private HeightMapGenerator (string seed, int width, int length)
	{
		this.seed = seed;
		heightMap = new float[width, length];
		heightMap [0, 0] = seed.ToCharArray () [0] % 26;
		heightMap [0, width - 1] = seed.ToCharArray () [1] % 26;
		heightMap [length - 1, 0] = seed.ToCharArray () [2] % 26;
		heightMap [length - 1, width - 1] = seed.ToCharArray () [3] % 26;
	}

	// Generate the heights using an implementation of diamond square height map generation.
	// Assume width / length are powers of 2 plus 1
	public static float[,] buildHeightMap (string seed, int width, int length)
	{
		HeightMapGenerator generator = new HeightMapGenerator (seed, width, length);
		generator.runDiamondSquareStep (0, 0, width - 1, length - 1);
		return generator.heightMap;
	}

	private void runDiamondSquareStep (int x0, int y0, int x1, int y1)
	{
		Debug.Log ("tl: (" + x0 + "," + y0 + "), br: (" + x1 + "," + y1 + ")");

		// diamond step
		heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)] = 
			((heightMap [x0, y0] + heightMap [x1, y0] + heightMap [x1, y1] + heightMap [x0, y1]) / 4)
		+ picker.Next (-2, 4);

		// square step
		if (x0 == 0) {
			heightMap [0, getMidpoint (y0, y1)] = ((heightMap [0, y0]
			+ heightMap [x1 / 2, getMidpoint (y0, y1)]
			+ heightMap [0, y1]
			+ heightMap [(heightMap.GetLength (0) - 1) - (x1 / 2), getMidpoint (y0, y1)]) / 4)
			+ picker.Next (-2, 4);
		} else {
			heightMap [x0, getMidpoint (y0, y1)] = ((heightMap [x0, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x0, y1]
			+ heightMap [getMidpoint (x0 - x1, x0), (y1 - y0) / 2]) / 4)
			+ picker.Next (-2, 4);
		}

		if (y0 == 0) {
			heightMap [getMidpoint (x0, x1), 0] = ((heightMap [x0, 0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, 0]
			+ heightMap [getMidpoint (x0, x1), (heightMap.GetLength (1) - 1) - (y1 / 2)]) / 4)
			+ picker.Next (-2, 4);
		} else {
			heightMap [getMidpoint (x0, x1), y0] = ((heightMap [x0, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0 - y1, y0)]) / 4)
			+ picker.Next (-2, 4);
		}

		if (x1 == heightMap.GetLength (0) - 1) {
			heightMap [x1, getMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [x1 - x0 / 2, getMidpoint (y0, y1)]) / 4)
			+ picker.Next (-2, 4);
		} else {
			heightMap [x1, getMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x1, (2 * x1) - x0), getMidpoint (y0, y1)]) / 4)
			+ picker.Next (-2, 4);
		}

		if (y1 == heightMap.GetLength (1) - 1) {
			heightMap [getMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x0, x1), (y1 - y0) / 2]) / 4)
			+ picker.Next (-2, 4);
		} else {
			heightMap [getMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y1, (2 * y1) - y0)]) / 4)
			+ picker.Next (-2, 4);
		}
		if (x1 - x0 == 2 || y1 - y0 == 2) {
			return;
		} 
		// Run the steps on the subdivided squares
		runDiamondSquareStep (x0, y0, (x1 - x0) / 2, (y1 - y0) / 2);
		runDiamondSquareStep ((x1 - x0) / 2, y0, x1, (y1 - y0) / 2);
		runDiamondSquareStep ((x1 - x0) / 2, (y1 - y0) / 2, x1, y1);
		runDiamondSquareStep (x0, (y1 - y0) / 2, (x1 - x0) / 2, y1);
	}

	private int getMidpoint (int p1, int p2)
	{
		if (p1 < p2) {
			return p1 + ((p2 - p1) / 2);
		} else if (p1 > p2) {
			return p2 + ((p1 - p2) / 2);
		}
		return p1;
	}
}
