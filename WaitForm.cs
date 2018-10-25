using System;
//using System.Drawing;
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

/*		int state;
		internal void SetLabelText()
		{
			string text = "";

			switch (state)
			{
				case 0:
					state = 1;
					text = "";
					break;
				case 1:
					state = 2;
					text = "u";
					break;
				case 2:
					state = 3;
					text = "uh";
					break;
				case 3:
					state = 0;
					text = "uhh";
					break;
			}
			label2.Text = text;
		} */
	}
}
