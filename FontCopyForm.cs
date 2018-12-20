using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FontCopyForm
		:
			Form
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal FontCopyForm(YataForm f)
		{
			InitializeComponent();

			const int padHori = 5;
			const int padVert = 6;

			const string text = "This string can be used in Settings.Cfg to override Yata's default font.";

			Font = f.Font;

			var size = YataGraphics.MeasureSize(text, Font);

			// Please NOTE: These are all just fudges since .NET would try to do
			// its own thing regardlessly. vid. AutoScaleMode=

			Width  = (size.Width  + padHori * 2) + 20;
			Height = (size.Height + padVert * 2) * 3
				   +  size.Height + padVert * 2
				   +  size.Height + padVert * 3
				   +  5;

			label1  .Height = (size.Height + padVert * 2) * 3;
			textBox1.Height =  size.Height + padVert * 2;
			panel1  .Height =  size.Height + padVert * 3;

			size = YataGraphics.MeasureSize("Cancel", Font);
			button1.Width  =
			button2.Width  = size.Width  + padHori * 6;
			button1.Height =
			button2.Height = size.Height + padVert * 2;

			label1.Text = Environment.NewLine
						+ "Current Font"
						+ Environment.NewLine + Environment.NewLine
						+ text;
		}

		internal void SetText(string text)
		{
			textBox1.Text = text;
		}

		void Button2Click(object sender, EventArgs e)
		{
			Close();
		}

		void Button1Click(object sender, EventArgs e)
		{
			Clipboard.SetText(textBox1.Text);
			Close();
		}
	}
}
