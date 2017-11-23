using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrototype
{
	private string name;

	public Dictionary<string, int> ResourcesRequired { get; private set; }

	public Dictionary<string, int> ChangeInResources { get; private set; }

	public static BuildingPrototype createBuildingPrototype (
		string name, Dictionary<string, int> resourcesRequired, Dictionary<string, int> resourcesProduced)
	{
		BuildingPrototype proto = new BuildingPrototype {
			name = name,
			ResourcesRequired = resourcesRequired,
			ChangeInResources = resourcesProduced
		};
		return proto;
	}

	public BuildingInstance buildInstance (Cell location, IEntity owner)
	{
		return new BuildingInstance (location, this, owner);
	}
}
