/*
 * User: William Crawford
 * Date: 5/31/2005
 * Time: 7:14 PM
 * 
 */

using System;
using System.Xml.Serialization;
using System.Xml;

namespace TrainProcess
{
	/// <summary>
	/// Information for a single hack, including
	/// addresses, byte, name, key, etc.
	/// </summary>
	public class TFHack
	{
		public String Name;
		public int Key;
		public TFHackSet[] HackSets;
		private int currentset = 0;
		
		public TFHackSet CurrentSet
		{
			get
			{
				return HackSets[currentset];
			}
		}
		
		public int CurrentSetNumber
		{
			get
			{
				return currentset;
			}
		}
		
		public TFHack()
		{
			
		}
		
		public TFHack(String Name, int Key, TFHackSet[] HackSets)
		{
			this.Name = Name;
			this.Key = Key;
			this.HackSets = HackSets;
		}
		
		public void IncrementSet()
		{
			currentset++;
			if (currentset >= HackSets.Length)
			{
				currentset = 0;
			}
		}
		
		public String ToXML()
		{
			System.IO.StringWriter XMLsw = new System.IO.StringWriter();
			XmlTextWriter XMLtw = new XmlTextWriter(XMLsw);

			XMLtw.Formatting = Formatting.Indented;
			XMLtw.WriteStartElement("hack");
			XMLtw.WriteElementString("name", Name);
			XMLtw.WriteElementString("key", Key.ToString());
			XMLtw.WriteStartElement("hacksets");
			for (int x=0; x < HackSets.Length; x++)
			{
				XMLtw.WriteRaw(HackSets[x].ToXML());
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
					case "name":
						Name = node.InnerText;
						break;
					case "key":
						Key = XmlConvert.ToInt32(node.InnerText);
						break;
					case "hacksets":
						this.HackSets = new TFHackSet[node.ChildNodes.Count];
						for (int x=0; x < node.ChildNodes.Count; x++)
						{
							HackSets[x] = new TFHackSet();
							HackSets[x].FromXML(node.ChildNodes[x].OuterXml);
						}
						break;
				}
			}
		}
	}
}
