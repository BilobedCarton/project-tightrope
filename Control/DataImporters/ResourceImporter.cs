using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

// Imports all the biomes described in the Biomes.xml file.
public static class ResourceImporter
{
	// This is the action method of this importer.
	public static List<Resource> Import ()
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/Terrain/Resources.xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);

		// Select all the Biome elements of the xml file.
		XmlNodeList xmlNodes = doc.GetElementsByTagName ("Resource");
		List<Resource> resourceTypes = new List<Resource> ();

		// Add their data.
		foreach (XmlNode n in xmlNodes) {
			resourceTypes.Add (new Resource (
				n.Attributes [0].Value, n.ChildNodes [0].InnerText, float.Parse (n.ChildNodes [1].InnerText)));
		}

		return resourceTypes;
	}
}