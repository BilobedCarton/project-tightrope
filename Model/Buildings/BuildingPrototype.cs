using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a type of building buildable by an IEntity.
public class BuildingPrototype
{
	public string Name { get; private set; }

	public Dictionary<string, int> ResourcesRequired { get; private set; }

	public Dictionary<string, int> ChangeInResources { get; private set; }

	// Creates a new BuildingPrototype with the given values.
	public static BuildingPrototype CreateBuildingPrototype (
		string name, Dictionary<string, int> resourcesRequired, Dictionary<string, int> resourcesProduced)
	{
		BuildingPrototype proto = new BuildingPrototype {
			Name = name,
			ResourcesRequired = resourcesRequired,
			ChangeInResources = resourcesProduced
		};
		return proto;
	}

	// Creates a new instance of this type of Building at the given Cell location to be owned by the given IEntity.
	public BuildingInstance BuildInstance (Cell location, IEntity owner)
	{
		return new BuildingInstance (location, this, owner);
	}
}
