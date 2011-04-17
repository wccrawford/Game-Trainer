/*
 * User: William
 * Date: 4/16/2011
 * Time: 7:46 PM
 */
using System;
using TrainProcess;

namespace GTTest
{
	class GTTest
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("GTTest!");
			
			if(args.Length < 1) {
				Console.WriteLine("No process name specified.");
				return;
			}
			
			GTTest test = new GTTest();
			
			test.Run(args);
			
			Console.WriteLine("Done.");
		}
		
		public void Run(string[] args) {
			Process targetProcess = OpenProcess(args[0]);
			
			Console.WriteLine(args[0]);

			if(targetProcess == null) {
				Console.WriteLine("Failed to open target process.");
				return;
			}
			
			Console.WriteLine("Opened successfully!");
			
			if(args.Length == 3) {
				if(args[1] == "find") {
					byte[] memory = Conversion.HexToBytes(args[2]);
					IntPtr[] Locations = targetProcess.FindInMemory(memory, 0, long.MaxValue);
					
					foreach(IntPtr Location in Locations) {
						Console.WriteLine(Location);
					}
				}
				
				if(args[1] == "read") {
					byte[] memory = targetProcess.ReadMemory((IntPtr)long.Parse(args[2]), 4);
					
					foreach(byte byt in memory) {
						Console.WriteLine(byt);
					}
				}
			}
			
			if (args.Length == 4) {
				if(args[1] == "replace") {
					byte[] oldBytes = Conversion.HexToBytes(args[2]);
					byte[] newBytes = Conversion.HexToBytes(args[3]);
					
					IntPtr[] Locations = targetProcess.FindInMemory(oldBytes);
					
					foreach(IntPtr Location in Locations) {
						Console.WriteLine(Location);
						targetProcess.WriteMemory(Location, newBytes);
					}
				}
			}
			
		}
		
		private Process OpenProcess(string ProcessName)
		{
			Process TargetProcess = new Process();
			
			if (!TargetProcess.ChooseProcess(ProcessName))
			{
				Console.WriteLine(ProcessName + " not found!");
				return null;
			}
			
			if (!TargetProcess.OpenProcess())
			{
				Console.WriteLine("Can't open process!");
				return null;
			}
			
			return TargetProcess;
		}
	}
}