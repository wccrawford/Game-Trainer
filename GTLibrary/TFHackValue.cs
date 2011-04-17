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
	/// Address and byte information for a single poke
	/// </summary>
	public class TFHackValue
	{
		public IntPtr Address;
		public byte[] NewBytes;
		public byte[] OldBytes;
		public long MinimumAddress = 0;
		public long MaximumAddress = 0x7ffffff;
		
		public TFHackValue()
		{
			
		}
		
		public TFHackValue(IntPtr Address, byte[] NewBytes)
		{
			this.Address = Address;
			this.NewBytes = NewBytes;
		}
		
		public String AddressString
		{
			get
			{
				return String.Format("{0:x8}",(int)Address);
			}
			set
			{
				Address = (IntPtr)Convert.ToInt32(value, 16);
			}
		}
		
		public String NewBytesString
		{
			get
			{
				String NewBytes = "";
				for (int x=0; x < NewBytes.Length; x++)
				{
					if (x > 0)
					{
						NewBytes += " ";
					}
					NewBytes += String.Format("{0:x2}",NewBytes[x]);
				}
				return NewBytes;
			}
			set
			{
				String[] bytes = value.Split(' ');
				NewBytes = new byte[bytes.Length];
				for (int x=0; x < bytes.Length; x++)
				{
					NewBytes[x] = Convert.ToByte(bytes[x], 16);
				}
			}
		}
		
		public String ToXML()
		{
			System.IO.StringWriter XMLsw = new System.IO.StringWriter();
			XmlTextWriter XMLtw = new XmlTextWriter(XMLsw);

			XMLtw.Formatting = Formatting.Indented;
			XMLtw.WriteStartElement("hackvalue");
			XMLtw.WriteElementString("address", AddressString);
			XMLtw.WriteElementString("NewBytes", NewBytesString);
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
					case "address":
						AddressString = node.InnerText;
						break;
					case "NewBytes":
						NewBytesString = node.InnerText;
						break;
				}
			}
		}
	}
}
