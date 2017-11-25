using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapGenerator
{
	private static System.Random picker = new System.Random ();

	private string seed;
	private int width;
	private int length;
	private float[,] heightMap;
	private int randRange;

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
	public static float[,] buildHeightMap (string seed, int width, int length)
	{
		HeightMapGenerator generator = new HeightMapGenerator (seed, width, length);
		generator.runDiamondSquareStep (0, 0, width - 1, length - 1);
		generator.runErosion ();
		return generator.heightMap;
	}

	private int getRandomBump ()
	{
		return picker.Next (-randRange, randRange);
	}

	private void reduceRandRange ()
	{
		randRange /= 2;
	}

	private void runDiamondSquareStep (int x0, int y0, int x1, int y1)
	{
		//Debug.Log ("tl: (" + x0 + "," + y0 + "), br: (" + x1 + "," + y1 + ")");

		// diamond step
		heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)] = 
			((heightMap [x0, y0] + heightMap [x1, y0] + heightMap [x1, y1] + heightMap [x0, y1]) / 4)
		+ getRandomBump ();

		// square step
		if (x0 == 0) {
			heightMap [0, getMidpoint (y0, y1)] = ((heightMap [0, y0]
			+ heightMap [x1 / 2, getMidpoint (y0, y1)]
			+ heightMap [0, y1]
			+ heightMap [(heightMap.GetLength (0) - 1) - (x1 / 2), getMidpoint (y0, y1)]) / 4)
			+ getRandomBump ();
		} else {
			heightMap [x0, getMidpoint (y0, y1)] = ((heightMap [x0, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x0, y1]
			+ heightMap [getMidpoint (x0 - x1, x0), (y1 - y0) / 2]) / 4)
			+ getRandomBump ();
		}

		if (y0 == 0) {
			heightMap [getMidpoint (x0, x1), 0] = ((heightMap [x0, 0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, 0]
			+ heightMap [getMidpoint (x0, x1), (heightMap.GetLength (1) - 1) - (y1 / 2)]) / 4)
			+ getRandomBump ();
		} else {
			heightMap [getMidpoint (x0, x1), y0] = ((heightMap [x0, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0 - y1, y0)]) / 4)
			+ getRandomBump ();
		}

		if (x1 == heightMap.GetLength (0) - 1) {
			heightMap [x1, getMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [x1 - x0 / 2, getMidpoint (y0, y1)]) / 4)
			+ getRandomBump ();
		} else {
			heightMap [x1, getMidpoint (y0, y1)] = ((heightMap [x1, y0]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x1, (2 * x1) - x0), getMidpoint (y0, y1)]) / 4)
			+ getRandomBump ();
		}

		if (y1 == heightMap.GetLength (1) - 1) {
			heightMap [getMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x0, x1), (y1 - y0) / 2]) / 4)
			+ getRandomBump ();
		} else {
			heightMap [getMidpoint (x0, x1), y1] = ((heightMap [x0, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y0, y1)]
			+ heightMap [x1, y1]
			+ heightMap [getMidpoint (x0, x1), getMidpoint (y1, (2 * y1) - y0)]) / 4)
			+ getRandomBump ();
		}
		if (x1 - x0 == 2 || y1 - y0 == 2) {
			return;
		} 

		reduceRandRange ();
		// Run the steps on the subdivided squares
		runDiamondSquareStep (x0, y0, getMidpoint (x0, x1), getMidpoint (y0, y1));
		runDiamondSquareStep (getMidpoint (x0, x1), y0, x1, getMidpoint (y0, y1));
		runDiamondSquareStep (getMidpoint (x0, x1), getMidpoint (y0, y1), x1, y1);
		runDiamondSquareStep (x0, getMidpoint (y0, y1), getMidpoint (x0, x1), y1);
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

	private void runErosion ()
	{
		float leftVal, topVal, rightVal, botVal;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < length; j++) {
				leftVal = i == 0 ? heightMap [i, j] : heightMap [i - 1, j];
				topVal = j == 0 ? heightMap [i, j] : heightMap [i, j - 1];
				rightVal = i == width - 1 ? heightMap [i, j] : heightMap [i + 1, j];
				botVal = j == length - 1 ? heightMap [i, j] : heightMap [i, j + 1];

				heightMap [i, j] = (heightMap [i, j] + leftVal + topVal + rightVal + botVal) / 5;
			}
		}
	}
}
