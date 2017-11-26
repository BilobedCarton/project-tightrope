using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
	public string Id { get; private set; }

	private string name;
	private float buildCostModifier;
	private float baseMovementModifier;
	List<string> potentialResources;
	private float lowestElevation;
	private float highestElevation;
	private int lowestTemperature;
	private int highestTemperature;

	public static Biome createTerrainType (
		string id,
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
		return new Biome {
			Id = id,
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
}
