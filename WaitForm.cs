using System;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// 
	/// </summary>
	partial class WaitForm
		:
			Form
	{
		internal WaitForm(YataForm f)
		{
			InitializeComponent();

			Left = f.Left + (f.Width  - Width)  / 2;
			Top  = f.Top  + (f.Height - Height) / 4;
		}
	}
}
