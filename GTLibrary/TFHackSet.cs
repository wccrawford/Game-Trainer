/*
 * User: William
 * Date: 4/17/2011
 * Time: 7:22 AM
 */
using System;
using System.Xml.Serialization;
using System.Xml;

namespace TrainProcess
{
	/// <summary>
	/// A set of hacks for a cheat
	/// </summary>
	public class TFHackSet
	{
		public TFHackValue[] HackValues;
		
		public TFHackSet()
		{
			
		}
		
		public TFHackSet(TFHackValue[] HackValues)
		{
			this.HackValues = HackValues;
		}
		
		public String ToXML()
		{
			System.IO.StringWriter XMLsw = new System.IO.StringWriter();
			XmlTextWriter XMLtw = new XmlTextWriter(XMLsw);

			XMLtw.Formatting = Formatting.Indented;
			XMLtw.WriteStartElement("hackset");
			XMLtw.WriteStartElement("hackvalues");
			for (int x=0; x < HackValues.Length; x++)
			{
				XMLtw.WriteRaw(HackValues[x].ToXML());
			}
			XMLtw.WriteEndElement();
			XMLtw.WriteEndElement();
			return XMLsw.ToString();
		}
		
		public void FromXML(String XML)
		{
			XmlDocument XMLdoc = new XmlDocument();
			XMLdoc.LoadXml(XML);
			XmlNode XMLnode = XMLdoc.DocumentElement;
			foreach (XmlNode node in XMLnode.ChildNodes)
			{
				switch(node.Name)
				{
					case "hackvalues":
						this.HackValues = new TFHackValue[node.ChildNodes.Count];
						for (int x=0; x < node.ChildNodes.Count; x++)
						{
							HackValues[x] = new TFHackValue();
							HackValues[x].FromXML(node.ChildNodes[x].OuterXml);
						}
						break;
				}
			}
		}
	}
}
