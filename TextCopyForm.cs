using System;
//using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	partial class TextCopyForm
		:
			Form
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal TextCopyForm()
		{
			InitializeComponent();

			const string text = "This string can be used in Settings.Cfg to override Yata's default font.";
//			Width = TextRenderer.MeasureText(text, Font).Width;

//			var image = new Bitmap(1,1);
//			var graphics = Graphics.FromImage(image);
//			Width = Convert.ToInt32(graphics.MeasureString(text, Font).Width);

			label1.Text = "Current Font"
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
