using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
	private string name;
	private readonly float baseValue;

	public float Price { get; private set; }

	public Resource (string name, float baseValue)
	{
		this.name = name;
		this.baseValue = baseValue;
	}

	public void calculatePrice (World world)
	{
		// Calculate based on supply, demand, and baseValue
		float worldStockpile = world.getWorldStockpile (this.name);
		float changeInStockpile = world.getNextStockpileChange (this.name);

		if (changeInStockpile / worldStockpile < 0.95f) {
			this.Price = baseValue - ((changeInStockpile / worldStockpile) * baseValue);
		} else {
			this.Price = 0.05f * baseValue;
		}
	}
}
