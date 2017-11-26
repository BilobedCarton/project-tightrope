using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the player's domain as an entity.
public class Player : AbstractEntity
{
	public Player (World world, List<string> resources) : base (world, resources, "Player")
	{
		
	}
}
