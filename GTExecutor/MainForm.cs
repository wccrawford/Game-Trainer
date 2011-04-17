/*
 * User: William Crawford
 * Date: 5/11/2005
 * Time: 10:53 AM
 * 
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using TrainProcess;

namespace TrainProcess
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		[DllImport("winmm.dll", EntryPoint="PlaySound",CharSet=CharSet.Auto)]
		private static extern int PlaySound(String sound, int hmod, int flags);

		private enum SND
		{
		    SND_SYNC         = 0x0000  ,/* play synchronously (default) */
		    SND_ASYNC        = 0x0001 , /* play asynchronously */
		    SND_NODEFAULT    = 0x0002 , /* silence (!default) if sound not found */
		    SND_MEMORY       = 0x0004 , /* pszSound points to a memory file */
		    SND_LOOP         = 0x0008 , /* loop the sound until next sndPlaySound */
		    SND_NOSTOP       = 0x0010 , /* don't stop any currently playing sound */
		    SND_NOWAIT       = 0x00002000, /* don't wait if the driver is busy */
		    SND_ALIAS        = 0x00010000 ,/* name is a registry alias */
		    SND_ALIAS_ID     = 0x00110000, /* alias is a pre d ID */
		    SND_FILENAME     = 0x00020000, /* name is file name */
		    SND_RESOURCE     = 0x00040004, /* name is resource name or atom */
		    SND_PURGE        = 0x0040,  /* purge non-static events for task */
		    SND_APPLICATION  = 0x0080 /* look for application specific association */
		}

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lblInstructions;
		private System.Windows.Forms.Timer timer1;
		private TFHackInfo HackInfo = new TFHackInfo();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
						
			InitHackInfo();
		}
		
		private void InitHackInfo()
		{
			String[] Args = System.Environment.GetCommandLineArgs();
			if (Args.Length == 2)
			{
				HackInfo.FromXMLFile(Args[1]);
				InitInstructions();
				timer1.Enabled = true;
			}
		}
		
		private void InitInstructions()
		{
			lblInstructions.Text = "";
			Process p = new Process();
			for (int x=0; x < HackInfo.Hacks.Length; x++)
			{
				lblInstructions.Text += p.GetKeyName(HackInfo.Hacks[x].Key) + " - " + HackInfo.Hacks[x].Name + System.Environment.NewLine;
			}
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.lblInstructions = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
			// 
			// lblInstructions
			// 
			this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblInstructions.Location = new System.Drawing.Point(0, 0);
			this.lblInstructions.Name = "lblInstructions";
			this.lblInstructions.Size = new System.Drawing.Size(292, 266);
			this.lblInstructions.TabIndex = 0;
			this.lblInstructions.Text = "You must use a trainer file as an argument.  You can do this by double-clicking a" +
" trainer file, or dragging the trainer file onto the Trainer Executor.";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.lblInstructions);
			this.Name = "MainForm";
			this.Text = "Trainer";
			this.ResumeLayout(false);
		}
		#endregion
		
		private Process OpenProcess()
		{
			Process TargetProcess = new Process();
			
			if (!TargetProcess.ChooseProcess(HackInfo.ProcessName))
			{
				System.Windows.Forms.MessageBox.Show(HackInfo.GameName + " not found!");
				throw(new Exception("Can't find process"));
			}
			
			if (!TargetProcess.OpenProcess())
			{
				System.Windows.Forms.MessageBox.Show("Can't open process!");
				throw(new Exception("Can't open process"));
			}
			
			return TargetProcess;
		}
		
		void PlaySound(String Sound)
		{
			PlaySound(Sound,0, (int) (SND.SND_ASYNC | SND.SND_ALIAS | SND.SND_NOWAIT));
		}
		
		void Timer1Tick(object sender, System.EventArgs e)
		{
			try
			{
				foreach(TFHack hack in HackInfo.Hacks)
				{
					if (TrainProcess.Process.CheckKey(hack.Key) != 0)
					{
						bool Success = false;
						
						while(!Success)
						{
							Process Game = OpenProcess();

							Success = Game.WriteHackSet(hack.CurrentSet);
							if (Success)
							{
								hack.IncrementSet();
							}

							Game.CloseProcess();
							
							Application.DoEvents();
							System.Threading.Thread.Sleep(100);
						}
						
						if (hack.CurrentSetNumber == 0)
						{
							PlaySound("SystemHand");
						}
						else
						{
							for (int x=0; x < hack.CurrentSetNumber; x++)
							{
								PlaySound(".default");
								System.Threading.Thread.Sleep(100);
							}
						}

					}
				}
			}
			catch(Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(exc.Message);
			}
		}
		
	}
}
