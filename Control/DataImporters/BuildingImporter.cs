using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

// Imports all the biomes described in the Buildings.xml file.
public static class BuildingImporter
{
	// This is the action method of this importer.
	public static List<BuildingPrototype> Import ()
	{
		FileStream fs = new FileStream ("Assets/Resources/Data/Buildings.xml", FileMode.Open, FileAccess.Read);
		XmlDocument doc = new XmlDocument ();
		doc.Load (fs);

		// Select all the Biome elements of the xml file.
		XmlNodeList xmlNodes = doc.GetElementsByTagName ("Building");
		List<BuildingPrototype> buildingTypes = new List<BuildingPrototype> ();

		// Add their data.
		foreach (XmlNode n in xmlNodes) {
			Dictionary<string, int> resourcesRequired = new Dictionary<string, int> ();
			foreach (XmlNode rr in n.ChildNodes[1].ChildNodes) {
				resourcesRequired.Add (rr.Attributes [0].Value, int.Parse (rr.InnerText));
			}
			Dictionary<string, int> changeInResources = new Dictionary<string, int> ();
			foreach (XmlNode dr in n.ChildNodes[2].ChildNodes) {
				changeInResources.Add (dr.Attributes [0].Value, int.Parse (dr.InnerText));
			}
			buildingTypes.Add (BuildingPrototype.CreateBuildingPrototype (
				n.Attributes [0].Value, 
				n.ChildNodes [0].InnerText, 
				resourcesRequired, 
				changeInResources,
				n.ChildNodes [3].InnerText == "" ? n.ChildNodes [3].InnerText : "none"));
		}

		fs.Close ();
		return buildingTypes;
	}
}
