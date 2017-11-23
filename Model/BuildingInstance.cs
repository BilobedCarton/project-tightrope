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

	public void runTick ()
	{
		foreach (var resource in this.proto.ChangeInResources) {
			Owner.changeResourceAmount (resource.Key, resource.Value);
		}
	}

	public int getChangeInResource (string name)
	{
		return this.proto.ChangeInResources [name];
	}
}
