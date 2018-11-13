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
		internal FontCopyForm()
		{
			InitializeComponent();

			const string text = "This string can be used in Settings.Cfg to override Yata's default font.";

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
