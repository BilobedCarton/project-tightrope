using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a resource existing in the world somewhere.
public class Resource
{
	public string Id { get; private set; }

	public  string Name { get; private set; }

	private readonly float baseValue;

	public float Price { get; private set; }

	// Creates a new type of resource with the given name and base value.
	public Resource (string id, string name, float baseValue)
	{
		this.Id = id;
		this.Name = name;
		this.baseValue = baseValue;
	}

	// Calculates the value of this resource in the given world.
	// Based somewhat upon supply and demand in the world.
	public void CalculatePrice (World world)
	{
		// Calculate based on supply, demand, and baseValue
		float worldStockpile = world.GetWorldStockpile (this.Name);
		float changeInStockpile = world.GetNextStockpileChange (this.Name);

		if (changeInStockpile / worldStockpile < 0.95f) {
			this.Price = baseValue - ((changeInStockpile / worldStockpile) * baseValue);
		} else {
			this.Price = 0.05f * baseValue;
		}
	}
}
