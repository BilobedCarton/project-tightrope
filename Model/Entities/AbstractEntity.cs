using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEntity : IEntity
{
	private World world;

	private Dictionary<string, int> currentResources;

	private string name;
	private float moneyBalance;

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

	public void changeResourceAmount (string name, int change)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			this.currentResources.Add (name, change);
			return;
		}
		this.currentResources.Add (name, this.currentResources [name] + change);
	}

	public int getResourceAmount (string name)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			return 0;
		}
		return this.currentResources [name];
	}

	public void purchaseResource (string name, int amount)
	{
		if (world.getResourceCost (name) * amount > this.moneyBalance) {
			Debug.LogError ("AbstractEntity.purchaseResource(...) -- trying to purchase more of given resource than able.");
			return;
		}
		this.moneyBalance -= amount * world.getResourceCost (name);
		this.changeResourceAmount (name, amount);
	}

	public void sellResource (string name, int amount)
	{
		if (this.getResourceAmount (name) < amount) {
			Debug.LogError ("AbstractEntity.sellResource(...) -- trying to sell more of a given resource than able.");
			return;
		}
		this.changeResourceAmount (name, -amount);
		this.moneyBalance += amount * world.getResourceCost (name);
	}

	public void placeBuildingInstance (Cell location, string name)
	{
		if (world.placeBuildingInstance (location, name, this) == false) {
			Debug.Log ("Failed to place structure of type: " + name + " at " + location.toString ());
		}
	}

	public float getMoneyBalance ()
	{
		return this.moneyBalance;
	}
}
