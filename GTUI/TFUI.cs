/*
 * User: William Crawford
 * Date: 5/16/2005
 * Time: 7:50 AM
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TFUI
{
	/// <summary>
	/// Description of TFUI.
	/// </summary>
	public class TFUI : System.Windows.Forms.Form
	{
		public TFUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// TFUI
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Text = "TFUI";
			this.Name = "TFUI";
		}
		#endregion
	}
}
