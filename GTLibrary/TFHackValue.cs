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
		public IntPtr Address = (IntPtr)0;
		public byte[] NewBytes;
		public byte[] OldBytes;
		public long MinimumAddress = 0;
		public long MaximumAddress = 0x7ffffff;
		public bool Active = false;
		
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
				return Conversion.BytesToHex(NewBytes);
			}
			set
			{
				NewBytes = Conversion.HexToBytes(value);
			}
		}
		
		public String OldBytesString
		{
			get
			{
				return Conversion.BytesToHex(OldBytes);
			}
			set
			{
				OldBytes = Conversion.HexToBytes(value);
			}
		}
		
		public String MinimumAddressString
		{
			get
			{
				return String.Format("{0:x8}",(int)MinimumAddress);
			}
			set
			{
				MinimumAddress = (long)Convert.ToInt32(value, 16);
			}
		}
		
		public String MaximumAddressString
		{
			get
			{
				return String.Format("{0:x8}",(int)MaximumAddress);
			}
			set
			{
				MaximumAddress = (long)Convert.ToInt32(value, 16);
			}
		}
		
		public String ToXML()
		{
			System.IO.StringWriter XMLsw = new System.IO.StringWriter();
			XmlTextWriter XMLtw = new XmlTextWriter(XMLsw);

			XMLtw.Formatting = Formatting.Indented;
			XMLtw.WriteStartElement("hackvalue");
			if(AddressString != "0") {
				XMLtw.WriteElementString("address", AddressString);
			}
			if(NewBytesString != "") {
				XMLtw.WriteElementString("bytes", NewBytesString);
			}
			if(OldBytesString != "") {
				XMLtw.WriteElementString("oldbytes", OldBytesString);
			}
			if(MinimumAddressString != "0") {
				XMLtw.WriteElementString("minimumaddress", MinimumAddressString);
			}
			if(MaximumAddressString != "0") {
				XMLtw.WriteElementString("maximumaddress", MaximumAddressString);
			}
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
					case "bytes":
						NewBytesString = node.InnerText;
						break;
					case "oldbytes":
						OldBytesString = node.InnerText;
						break;
					case "minimumaddress":
						MinimumAddressString = node.InnerText;
						break;
					case "maximumaddress":
						MaximumAddressString = node.InnerText;
						break;
				}
			}
		}
	}
}
