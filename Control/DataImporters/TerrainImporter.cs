using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

public class TerrainImporter
{
	public static List<Terrain> importTerrain ()
	{
		List<Terrain> terrain = new List<Terrain> ();
		FileStream fs = new FileStream ("Resources/Data/TerrainTypes.xml", FileMode.Open, FileAccess.Read);

		return terrain;
	}
}
