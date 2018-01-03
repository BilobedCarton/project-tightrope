using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a type of building buildable by an IEntity.
public class BuildingPrototype
{
	public string Id { get; private set; }

	public string Name { get; private set; }

	public Dictionary<string, int> ResourcesRequired { get; private set; }

	public Dictionary<string, int> ChangeInResources { get; private set; }

	private string requiredNaturalResource;

	// Creates a new BuildingPrototype with the given values.
	public static BuildingPrototype CreateBuildingPrototype (
		string id, 
		string name, 
		Dictionary<string, int> resourcesRequired, 
		Dictionary<string, int> resourcesProduced, 
		string requiredNaturalResource)
	{
		BuildingPrototype proto = new BuildingPrototype {
			Id = id,
			Name = name,
			ResourcesRequired = resourcesRequired,
			ChangeInResources = resourcesProduced,
			requiredNaturalResource = requiredNaturalResource
		};
		return proto;
	}

	// Creates a new instance of this type of Building at the given Cell location to be owned by the given IEntity.
	public BuildingInstance BuildInstance (Cell location, IEntity owner)
	{
		if (this.requiredNaturalResource != "none"
		    && (location.NaturalResource.Id != this.requiredNaturalResource) || location.NaturalResource == null) {
			Debug.Log ("BuildingPrototype.BuildInstance(...) -- location does not have correct required natural resource.");
			return null;
		}
		return new BuildingInstance (location, this, owner);
	}
}
