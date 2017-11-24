using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
	private World world;

	// Use this for initialization
	void Start ()
	{
		this.world = new World (5, 5, TerrainImporter.importTerrain ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
