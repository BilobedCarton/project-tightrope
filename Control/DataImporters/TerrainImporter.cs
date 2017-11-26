using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

public class TerrainImporter
{
	public static List<Biome> importTerrain ()
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/Terrain/Temperatures.xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);

		XmlNodeList xmlNodes = doc.GetElementsByTagName ("Terrain");
		List<Biome> terrainTypes = new List<Biome> ();

		foreach (XmlNode n in xmlNodes) {
			terrainTypes.Add (Biome.createTerrainType (n.Attributes [0].Value,
				n.ChildNodes [0].InnerText, float.Parse (n.ChildNodes [1].InnerText), float.Parse (n.ChildNodes [2].InnerText),
				n.ChildNodes [3].InnerText, float.Parse (n.ChildNodes [4].InnerText), float.Parse (n.ChildNodes [5].InnerText),
				int.Parse (n.ChildNodes [6].InnerText), int.Parse (n.ChildNodes [7].InnerText)));
		}

		return terrainTypes;
	}
}
