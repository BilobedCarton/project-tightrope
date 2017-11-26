using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents an absract implementation of an IEntity.
public class AbstractEntity : IEntity
{
	private World world;

	private Dictionary<string, int> currentResources;

	private string name;
	private float moneyBalance;

	// Creates a new entity with the given name, resource values, and existing in the given world.
	public AbstractEntity (World world, List<string> resources, string name)
	{
		this.world = world;
		this.currentResources = new Dictionary<string, int> ();
		foreach (var resource in resources) {
			this.currentResources.Add (resource, 0);
		}
		this.name = name;
		this.moneyBalance = 0;
	}

	public void ChangeResourceAmount (string name, int change)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			this.currentResources.Add (name, change);
			return;
		}
		this.currentResources.Add (name, this.currentResources [name] + change);
	}

	public int GetResourceAmount (string name)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			return 0;
		}
		return this.currentResources [name];
	}

	public void PurchaseResource (string name, int amount)
	{
		if (world.GetResourceCost (name) * amount > this.moneyBalance) {
			Debug.LogError ("AbstractEntity.purchaseResource(...) -- trying to purchase more of given resource than able.");
			return;
		}
		this.moneyBalance -= amount * world.GetResourceCost (name);
		this.ChangeResourceAmount (name, amount);
	}

	public void SellResource (string name, int amount)
	{
		if (this.GetResourceAmount (name) < amount) {
			Debug.LogError ("AbstractEntity.sellResource(...) -- trying to sell more of a given resource than able.");
			return;
		}
		this.ChangeResourceAmount (name, -amount);
		this.moneyBalance += amount * world.GetResourceCost (name);
	}

	public void PlaceBuildingInstance (Cell location, string name)
	{
		if (world.PlaceBuildingInstance (location, name, this) == false) {
			Debug.Log ("Failed to place structure of type: " + name + " at " + location.ToString ());
		}
	}

	public float getMoneyBalance ()
	{
		return this.moneyBalance;
	}
}
