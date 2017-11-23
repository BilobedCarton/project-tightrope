using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEntity : IEntity
{
	private World world;

	private Dictionary<string, int> currentResources;

	public AbstractEntity (World world, List<string> resources)
	{
		this.world = world;
		this.currentResources = new Dictionary<string, int> ();
		foreach (var resource in resources) {
			this.currentResources.Add (resource, 0);
		}
	}

	public void changeResourceAmount (string name, int change)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			Debug.LogError ("Player.changeResourceAmount(...) -- trying the change amount of non-existent resource.");
		}
		this.currentResources.Add (name, this.currentResources [name] + change);
	}

	public int getResourceAmount (string name)
	{
		if (this.currentResources.ContainsKey (name) == false) {
			Debug.LogError ("Player.getResourceAmount(...) -- trying the get amount of non-existent resource.");
		}
		return this.currentResources [name];
	}

	public void placeBuildingInstance (Cell location, string name)
	{
		if (world.placeBuildingInstance (location, name, this) == false) {
			Debug.Log ("Failed to place structure of type: " + name + " at " + location.toString ());
		}
	}
}
