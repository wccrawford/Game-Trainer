/*
 * User: William Crawford
 * Date: 5/11/2005
 * Time: 10:52 AM
 * 
 */
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;

namespace TrainProcess
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MEMORY_BASIC_INFORMATION
	{
		public IntPtr BaseAddress;
		public IntPtr AllocationBase;
		public uint AllocationProtect;
		public IntPtr RegionSize;
		public uint State;
		public uint Protect;
		public uint Type;
	}
    
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Process
	{
		public Hashtable KeyNames =
			new Hashtable();
		
		public Process()
		{
			KeyNames.Add((int)Keys.F1, "F1");
			KeyNames.Add((int)Keys.F2, "F2");
			KeyNames.Add((int)Keys.F3, "F3");
			KeyNames.Add((int)Keys.F4, "F4");
			KeyNames.Add((int)Keys.F5, "F5");
			KeyNames.Add((int)Keys.F6, "F6");
			KeyNames.Add((int)Keys.F7, "F7");
			KeyNames.Add((int)Keys.F8, "F8");
			KeyNames.Add((int)Keys.F9, "F9");
			KeyNames.Add((int)Keys.F10, "F10");
			KeyNames.Add((int)Keys.F11, "F11");
			KeyNames.Add((int)Keys.F12, "F12");
		}
		
		public enum Access
		{
			TERMINATE			= 0x0001,
			CREATE_THREAD		= 0x0002, 
			SET_SESSIONID		= 0x0004, 
			VM_OPERATION		= 0x0008, 
			VM_READ				= 0x0010, 
			VM_WRITE			= 0x0020, 
			DUP_HANDLE			= 0x0040, 
			CREATE_PROCESS		= 0x0080, 
			SET_QUOTA			= 0x0100, 
			SET_INFORMATION		= 0x0200, 
			QUERY_INFORMATION	= 0x0400,
			ALL					= 0x07FF
		}
		
		public enum Keys
		{
			F1 = 0x70,
			F2 = 0x71,
			F3 = 0x72,
			F4 = 0x73,
			F5 = 0x74,
			F6 = 0x75,
			F7 = 0x76,
			F8 = 0x77,
			F9 = 0x78,
			F10 = 0x79,
			F11 = 0x7A,
			F12 = 0x7B
		}
		
		public String GetKeyName(int Key)
		{
			return KeyNames[Key].ToString();
		}
		
		[DllImport("kernel32")]
		private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, 
		int dwProcessId);

		[DllImport("kernel32.dll")]
		private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
		[In, Out] byte[] lpBuffer, UInt32 nSize, out IntPtr lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
		[In, Out] byte[] lpBuffer, UInt32 nSize, out IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		private static extern UInt32 VirtualQueryEx(IntPtr hProcess, IntPtr lpBaseAddress,
		out MEMORY_BASIC_INFORMATION lpBuffer, UInt32 dwLength);

		[DllImport("kernel32")]
		private static extern bool CloseHandle( IntPtr hObject );
		
		[DllImport("User32.dll")]
		private static extern short GetAsyncKeyState(int vKey);
		
		System.Diagnostics.Process TargetProcess = null;
		
		IntPtr TargetHandle;
		
		/// <summary>
		/// Choose Process in Memory
		/// </summary>
		/// <param name="ProcessName">Process Name to find</param>
		/// <returns>True if successful</returns>
		public bool ChooseProcess(String ProcessName)
		{
			System.Diagnostics.Process[] Processes = System.Diagnostics.Process.GetProcessesByName(ProcessName);
			if (Processes.Length < 1)
				return false;

			TargetProcess = Processes[0];
			return true;			
		}
		
		/// <summary>
		/// Open the Process to Read and Write Data using name
		/// from ChooseProcess
		/// </summary>
		/// <returns>True if successful</returns>
		public bool OpenProcess()
		{
			try
			{
				TargetHandle = OpenProcess((uint)(Access.ALL), true, TargetProcess.Id); // Read and Write access Access.VM_READ | Access.VM_WRITE | Access.VM_OPERATION
				return true;
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine("Exception: " + exc.ToString());
				return false;
			}
		}
		
		/// <summary>
		/// Choose Process in Memory and Open for Read and Write
		/// </summary>
		/// <param name="ProcessName">Process Name to find</param>
		/// <returns>True if successful</returns>
		public bool OpenProcess(String ProcessName)
		{
			if (this.ChooseProcess(ProcessName))
				return this.OpenProcess();
			return false;
		}
		
		/// <summary>
		/// Close Process
		/// </summary>
		/// <returns>true if successful</returns>
		public bool CloseProcess()
		{
			try
			{
				CloseHandle(TargetHandle);
				return true;
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine("Exception: " + exc.ToString());
				return false;
			}
		}
		
		public byte[] ReadMemory(IntPtr Address, UInt32 nSize) {
			byte[] lpBuffer = new Byte[nSize];
			IntPtr numberOfBytesRead = (IntPtr)0;
			
			bool result = ReadProcessMemory(TargetHandle, Address,
				lpBuffer, nSize, out numberOfBytesRead);
			
			return lpBuffer;
		}
		
		/// <summary>
		/// Write bytes to the Process' Memory
		/// </summary>
		/// <param name="Address">Address in memory to write to</param>
		/// <param name="buffer">Bytes to write</param>
		/// <returns>true if successful</returns>
		public bool WriteMemory(IntPtr Address, [In, Out] byte[] buffer)
		{
			IntPtr BytesWritten;
			
			//try
			//{
			//System.Windows.Forms.MessageBox.Show("Buffer Length: " + buffer.Length.ToString());
				WriteProcessMemory(TargetHandle, Address, buffer, (uint)buffer.Length, out BytesWritten);
				//System.Windows.Forms.MessageBox.Show("Bytes Written: " + BytesWritten.ToString());
				if (BytesWritten == (IntPtr)buffer.Length)
					return true;
				else
					return false;
			//}
			//catch (Exception exc)
			//{
			//	System.Diagnostics.Debug.WriteLine("Exception: " + exc.ToString());
			//	return false;
			//}
		}
		
		/// <summary>
		/// Write an entire HackSet to the Process
		/// </summary>
		/// <param name="HackSet">HackSet to write</param>
		/// <returns>true if successful</returns>
		public bool WriteHackSet(TFHackSet HackSet)
		{
			
			return WriteHackValues(HackSet.HackValues);
		}

		/// <summary>
		/// Write a set of HackValues to the Process
		/// </summary>
		/// <param name="HackValues">HackValue to write</param>
		/// <returns>true if successful</returns>
		public bool WriteHackValues(TFHackValue[] HackValues)
		{
			bool Success = true;
			
			for (int x=0; x < HackValues.Length; x++)
			{
				bool HackSuccess = WriteHackValue(HackValues[x]);
				Success = Success & HackSuccess;
			}
			return Success;
		}
		
		/// <summary>
		/// Write a single hack value. Tries up to 30 times, 1/10th
		/// of a second apart.
		/// </summary>
		/// <param name="HackValue">hack value to write</param>
		/// <returns>True if successful</returns>
		public bool WriteHackValue(TFHackValue HackValue)
		{
			return WriteMemory(HackValue.Address, HackValue.NewBytes);
		}
		
		/// <summary>
		/// Check whether a key was pressed or not
		/// </summary>
		/// <param name="Key">Key value to check for</param>
		/// <returns></returns>
		public static short CheckKey(int Key)
		{
			return GetAsyncKeyState(Key);
		}
		
		public MEMORY_BASIC_INFORMATION[] GetMemoryRegions() {
			ArrayList locations = new ArrayList();
			
			long address = 0;
			long lastAddress = 0x7fffffff;
			
			while(address < lastAddress) {
				MEMORY_BASIC_INFORMATION meminfo = new MEMORY_BASIC_INFORMATION();
				uint result = VirtualQueryEx(TargetHandle, (IntPtr)address, out meminfo, (uint)Marshal.SizeOf(meminfo));
				//if((uint)meminfo.RegionSize < 1) {
				if(result == 0) {
					break;
				}
				address = (long)((long)meminfo.BaseAddress + (long)meminfo.RegionSize);
				
				locations.Add(meminfo);
			}
			
			return (MEMORY_BASIC_INFORMATION[])locations.ToArray(typeof(MEMORY_BASIC_INFORMATION));
		}
		
		public IntPtr[] FindInMemory(byte[] Needle, long MinimumAddress=0, long MaximumAddress=0x7fffffff) {
			ArrayList Locations = new ArrayList();
			
			MEMORY_BASIC_INFORMATION[] MemoryRegions = GetMemoryRegions();
			
			foreach(MEMORY_BASIC_INFORMATION MemoryRegion in MemoryRegions) {
				if((long)MemoryRegion.BaseAddress+(long)MemoryRegion.RegionSize < MinimumAddress) {
					continue;
				}
				if((long)MemoryRegion.BaseAddress > MaximumAddress) {
					continue;
				}
				Locations.AddRange(FindInMemoryRegion(Needle, MemoryRegion));
			}
			
			return (IntPtr[])Locations.ToArray(typeof(IntPtr));
		}
		
		public IntPtr[] FindInMemoryRegion(byte[] Needle, MEMORY_BASIC_INFORMATION MemoryRegion) {
			ArrayList Locations = new ArrayList();
			
			byte[] Memory = ReadMemoryRegion(MemoryRegion);
			
			// Loop through each byte in the memory region and start searching,
			// But don't bother with the bit at the end that's too short.
			for(long indexMemory = 0; indexMemory < (Memory.Length-(Needle.Length-1)); indexMemory++) {
				// Check that each byte matches
				bool Match = true;
				for(long indexNeedle = 0; indexNeedle < Needle.Length; indexNeedle++) {
					if(Memory[indexMemory+indexNeedle] != Needle[indexNeedle]) {
						Match = false;
						break;
					}
				}
				
				if(Match) {
					Locations.Add((IntPtr)((long)MemoryRegion.BaseAddress+indexMemory));
				}
			}
			
			return (IntPtr[])Locations.ToArray(typeof(IntPtr));
		}
		
		public byte[] ReadMemoryRegion(MEMORY_BASIC_INFORMATION MemoryRegion) {
			return ReadMemory(MemoryRegion.BaseAddress, (uint)MemoryRegion.RegionSize);
		}
	}
}
