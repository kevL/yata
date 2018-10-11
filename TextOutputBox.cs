﻿using System;
using System.Windows.Forms;


namespace yata
{
	partial class TextOutputBox
		:
			Form
	{
		/// <summary>
		/// cTor.
		/// </summary>
		internal TextOutputBox()
		{
			InitializeComponent();
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
