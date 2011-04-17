﻿/*
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
			
//			MEMORY_BASIC_INFORMATION[] locations = targetProcess.GetMemoryRegions();
			
//			foreach(MEMORY_BASIC_INFORMATION location in locations) {
//				Console.WriteLine("{0}-{1} : {2} bytes", location.BaseAddress,
//				                  (uint)location.BaseAddress + (uint)location.RegionSize, location.RegionSize);
//			}
			
			if(args.Length >= 2) {
				byte[] memory = targetProcess.ReadMemory((IntPtr)long.Parse(args[1]), 4);
				foreach(byte membyte in memory) {
					Console.WriteLine(membyte);
				}
				
				IntPtr[] Locations = targetProcess.FindInMemory(memory, 200000000, 300000000);
				
				foreach(IntPtr Location in Locations) {
					Console.WriteLine(Location);
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