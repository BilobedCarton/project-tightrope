using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
	private string name;
	private float buildCostModifier;
	private float baseMovementModifier;
	List<string> potentialResources;
	private int lowestElevation;
	private int highestElevation;
	private int lowestTemperature;
	private int highestTemperature;

	public static Terrain createTerrainType (
		string name, 
		float buildCostModifier, 
		float baseMovementModifier, 
		List<string> potentialResources, 
		int lowestElevation, 
		int highestElevation)
	{
		return new Terrain {
			name = name,
			buildCostModifier = buildCostModifier,
			baseMovementModifier = baseMovementModifier,
			potentialResources = potentialResources,
			lowestElevation = lowestElevation,
			highestElevation = highestElevation
		};
	}

	public bool isOption (float elevation, int temperature)
	{
		return elevation >= lowestElevation
		&& elevation <= highestElevation
		&& temperature >= lowestTemperature
		&& temperature <= highestTemperature;
	}
}
