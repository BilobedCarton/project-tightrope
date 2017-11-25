using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainType
{
	private int id;
	private string name;
	private float buildCostModifier;
	private float baseMovementModifier;
	List<string> potentialResources;
	private float lowestElevation;
	private float highestElevation;
	private int lowestTemperature;
	private int highestTemperature;

	public static TerrainType createTerrainType (
		int id,
		string name, 
		float buildCostModifier, 
		float baseMovementModifier, 
		string potentialResourcesList, 
		float lowestElevation, 
		float highestElevation,
		int lowestTemperature,
		int highestTemperature)
	{
		List<string> potentialResources = new List<string> ();
		potentialResources = new List<string> (potentialResourcesList.Split (new char[] { ',' }));
		return new TerrainType {
			id = id,
			name = name,
			buildCostModifier = buildCostModifier,
			baseMovementModifier = baseMovementModifier,
			potentialResources = potentialResources,
			lowestElevation = lowestElevation,
			highestElevation = highestElevation,
			lowestTemperature = lowestTemperature,
			highestTemperature = highestTemperature
		};
	}

	public bool isOption (float elevation, int temperature)
	{
		return elevation >= lowestElevation
		&& elevation <= highestElevation
		&& temperature >= lowestTemperature
		&& temperature <= highestTemperature;
	}

	public string toString ()
	{
		return id + "_" + name + "_" + buildCostModifier + "_" + baseMovementModifier + "_"
		+ lowestElevation + "_" + highestElevation + "_" + lowestTemperature + "_" + highestTemperature;
	}
}
