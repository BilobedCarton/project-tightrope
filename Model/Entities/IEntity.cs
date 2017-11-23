using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
	void changeResourceAmount (string name, int change);

	int getResourceAmount (string name);

	void purchaseResource (string name, int amount);

	void sellResource (string name, int amount);

	void placeBuildingInstance (Cell location, string name);

	float getMoneyBalance ();
}
