using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstance
{
	private Cell location;
	private BuildingPrototype proto;

	public IEntity Owner { get; private set; }

	public BuildingInstance (Cell location, BuildingPrototype proto, IEntity owner)
	{
		this.location = location;
		this.proto = proto;
		this.Owner = owner;
	}

	public void runTick (World world)
	{
		foreach (var resource in this.proto.ChangeInResources) {
			if (Owner.getResourceAmount (resource.Key) < -resource.Value
			    && Owner.getMoneyBalance () < world.getResourceCost (resource.Key) * (-resource.Value - Owner.getResourceAmount (resource.Key))) {
				return;
			}
		}
		foreach (var resource in this.proto.ChangeInResources) {
			if (Owner.getResourceAmount (resource.Key) < -resource.Value) {
				Owner.purchaseResource (resource.Key, -resource.Value - Owner.getResourceAmount (resource.Key));
			}
			Owner.changeResourceAmount (resource.Key, resource.Value);
		}
	}

	public int getChangeInResource (string name)
	{
		return this.proto.ChangeInResources [name];
	}
}
