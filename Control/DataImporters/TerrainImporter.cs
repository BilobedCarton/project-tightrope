using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

public class TerrainImporter
{
	public static List<TerrainType> importTerrain ()
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/TerrainTypes.xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);

		XmlNodeList xmlNodes = doc.GetElementsByTagName ("Terrain");
		List<TerrainType> terrainTypes = new List<TerrainType> ();

		foreach (XmlNode n in xmlNodes) {
			terrainTypes.Add (TerrainType.createTerrainType (int.Parse (n.Attributes [0].Value),
				n.ChildNodes [0].InnerText, float.Parse (n.ChildNodes [1].InnerText), float.Parse (n.ChildNodes [2].InnerText),
				n.ChildNodes [3].InnerText, float.Parse (n.ChildNodes [4].InnerText), float.Parse (n.ChildNodes [5].InnerText),
				int.Parse (n.ChildNodes [6].InnerText), int.Parse (n.ChildNodes [7].InnerText)));
		}

		return terrainTypes;
	}
}
