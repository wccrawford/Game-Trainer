/*
 * User: William
 * Date: 4/17/2011
 * Time: 8:45 AM
 */
using System;

namespace TrainProcess
{
	/// <summary>
	/// Description of Conversion.
	/// </summary>
	public class Conversion
	{
		public Conversion()
		{
		}
		
		public static byte[] HexToBytes(string Hex) {
			String[] bytes = Hex.Split(' ');
			byte[] NewBytes = new byte[bytes.Length];
			for (int x=0; x < bytes.Length; x++)
			{
				NewBytes[x] = Convert.ToByte(bytes[x], 16);
			}
			return NewBytes;
		}
		
		public static string BytesToHex(byte[] Bytes) {
			String Hex = "";
			for (int x=0; x < Bytes.Length; x++)
			{
				if (x > 0)
				{
					Hex += " ";
				}
				Hex += String.Format("{0:x2}",Bytes[x]);
			}
			return Hex;
		}
	}
}
