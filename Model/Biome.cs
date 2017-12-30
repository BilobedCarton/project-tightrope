using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a Biome for a map.
public class Biome
{
	public string Id { get; private set; }

	public string Name { get; private set; }

	private float buildCostModifier;
	private float baseMovementModifier;
	List<string> potentialResources;
	private float lowestElevation;
	private float highestElevation;
	private int lowestTemperature;
	private int highestTemperature;

	// Creates a new Biome based upon the given information.
	public static Biome CreateTerrainType (
		string id,
		string name, 
		float buildCostModifier, 
		float baseMovementModifier, 
		List<string> potentialResourcesList, 
		float lowestElevation, 
		float highestElevation,
		int lowestTemperature,
		int highestTemperature)
	{
		return new Biome {
			Id = id,
			Name = name,
			buildCostModifier = buildCostModifier,
			baseMovementModifier = baseMovementModifier,
			potentialResources = potentialResourcesList,
			lowestElevation = lowestElevation,
			highestElevation = highestElevation,
			lowestTemperature = lowestTemperature,
			highestTemperature = highestTemperature
		};
	}

	// Determines if this Biome meets the given requirements.
	public bool IsOption (float elevation, int temperature)
	{
		return elevation >= lowestElevation
		&& elevation <= highestElevation
		&& temperature >= lowestTemperature
		&& temperature <= highestTemperature;
	}

	// Picks a random resource id based upon this biomes list of potential resources.
	public string PickRandomResource (System.Random picker)
	{
		if (picker.Next (0, 25) < 1) {
			return potentialResources [picker.Next (0, potentialResources.Count)];
		}
		return "Empty";
	}
}
