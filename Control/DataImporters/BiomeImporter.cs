using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

// Imports all the biomes described in the Biomes.xml file.
public static class BiomeImporter
{
	// This is the action method of this importer.
	public static List<Biome> Import ()
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/Biomes.xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);

		// Select all the Biome elements of the xml file.
		XmlNodeList xmlNodes = doc.GetElementsByTagName ("Biome");
		List<Biome> terrainTypes = new List<Biome> ();

		// Add their data.
		foreach (XmlNode n in xmlNodes) {
			List<string> potentialResources = new List<string> ();
			foreach (XmlNode r in n.ChildNodes [3].ChildNodes) {
				potentialResources.Add (r.Attributes [0].Value);
			}
			terrainTypes.Add (Biome.CreateTerrainType (n.Attributes [0].Value,
				n.ChildNodes [0].InnerText, float.Parse (n.ChildNodes [1].InnerText), float.Parse (n.ChildNodes [2].InnerText),
				potentialResources, float.Parse (n.ChildNodes [4].InnerText), float.Parse (n.ChildNodes [5].InnerText),
				int.Parse (n.ChildNodes [6].InnerText), int.Parse (n.ChildNodes [7].InnerText)));
		}

		fs.Close ();
		return terrainTypes;
	}
}
