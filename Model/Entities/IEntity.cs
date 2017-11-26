using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents some form of entity: from nations to corporations to unions.
public interface IEntity
{
	// Changes the amount of the given resource this entity controls.
	void ChangeResourceAmount (string name, int change);

	// Returns the amount of a given resource that this entity controls.
	int GetResourceAmount (string name);

	// Purchases an amount of the given resource for this entity.
	void PurchaseResource (string name, int amount);

	// Sells an amount of the given resource for this entity.
	void SellResource (string name, int amount);

	// Places an instance of the given type of building in the given Cell location.
	void PlaceBuildingInstance (Cell location, string name);

	// Returns the amount of money currently available to this entity.
	float GetMoneyBalance ();
}
