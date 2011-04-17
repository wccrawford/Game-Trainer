/*
 * User: William
 * Date: 4/17/2011
 * Time: 7:20 AM
 */
using System;
using System.Xml.Serialization;
using System.Xml;

namespace TrainProcess
{
	/// <summary>
	/// Total info for hacks for a game.
	/// </summary>
	[Serializable]
	public class TFHackInfo
	{
		public String GameName = "";
		public String ProcessName = "";
		public TFHack[] Hacks;
		public int MajorVersion = 0;
		public int MinorVersion = 0;
		
		public TFHackInfo()
		{
			
		}
		
		public TFHackInfo(String GameName, String ProcessName, TFHack[] Hacks, int MajorVersion, int MinorVersion)
		{
			this.GameName = GameName;
			this.ProcessName = ProcessName;
			this.Hacks = Hacks;
			this.MajorVersion = MajorVersion;
			this.MinorVersion = MinorVersion;
		}
		
		public String XMLString
		{
			get
			{
				return ToXML();
			}
			set
			{
				FromXML(value);
			}
		}
		
		public void ToXMLFile(String Filename)
		{
			System.IO.StreamWriter sw = new System.IO.StreamWriter(Filename);
			sw.Write(this.ToXML());
			sw.Flush();
			sw.Close();
		}
		
		public String ToXML()
		{
			System.IO.StringWriter XMLsw = new System.IO.StringWriter();
			XmlTextWriter XMLtw = new XmlTextWriter(XMLsw);
			
			XMLtw.Formatting = Formatting.Indented;
			XMLtw.WriteStartDocument();
			XMLtw.WriteStartElement("hackinfo");
			XMLtw.WriteElementString("gamename", GameName);
			XMLtw.WriteElementString("processname", ProcessName);
			XMLtw.WriteElementString("version", MajorVersion.ToString() + "." + MinorVersion.ToString());
			XMLtw.WriteStartElement("hacks");
			for (int x=0; x < Hacks.Length; x++)
			{
				XMLtw.WriteRaw(Hacks[x].ToXML());
			}
			XMLtw.WriteEndElement();
			XMLtw.WriteEndElement();
			return XMLsw.ToString();
		}
		
		public void FromXMLFile(String Filename)
		{
			System.IO.StreamReader sr = new System.IO.StreamReader(Filename);
			this.FromXML(sr.ReadToEnd());
			sr.Close();
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
					case "gamename":
						GameName = node.InnerText;
						break;
					case "processname":
						ProcessName = node.InnerText;
						break;
					case "version":
						String[] version = node.InnerText.Split('.');
						MajorVersion = Convert.ToInt32(version[0]);
						MinorVersion = Convert.ToInt32(version[1]);
						break;
					case "hacks":
						this.Hacks = new TFHack[node.ChildNodes.Count];
						for (int x=0; x < node.ChildNodes.Count; x++)
						{
							Hacks[x] = new TFHack();
							Hacks[x].FromXML(node.ChildNodes[x].OuterXml);
						}
						break;
				}
			}
		}
	}
}
