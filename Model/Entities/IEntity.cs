using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
	void changeResourceAmount (string name, int change);

	int getResourceAmount (string name);
}
