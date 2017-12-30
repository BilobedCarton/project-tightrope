using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents an instance of a building.
public class BuildingInstance
{
	private Cell location;
	private BuildingPrototype proto;

	public IEntity Owner { get; private set; }

	// Creates a new BuildInstance with the given Cell as its location, based upon the given BuildingPrototype, and owned by the given IEntity.
	public BuildingInstance (Cell location, BuildingPrototype proto, IEntity owner)
	{
		this.location = location;
		this.proto = proto;
		this.Owner = owner;
	}

	// Runs a tick, changing resource values based on this building's input and output.
	public void RunTick (World world)
	{
		foreach (var resource in this.proto.ChangeInResources) {
			if (Owner.GetResourceAmount (resource.Key) < -resource.Value
			    && Owner.GetMoneyBalance () < world.GetResourceCost (resource.Key) * (-resource.Value - Owner.GetResourceAmount (resource.Key))) {
				return;
			}
		}
		foreach (var resource in this.proto.ChangeInResources) {
			if (Owner.GetResourceAmount (resource.Key) < -resource.Value) {
				Owner.PurchaseResource (resource.Key, -resource.Value - Owner.GetResourceAmount (resource.Key));
			}
			Owner.ChangeResourceAmount (resource.Key, resource.Value);
		}
	}

	// Gets the amount this structure will change a stockpile of the given resource.
	public int GetChangeInResource (string name)
	{
		return this.proto.ChangeInResources [name];
	}

	// Gets the name of this type of building.
	public string GetName ()
	{
		return proto.Name;
	}
}
