using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog for the 2da-differ.
	/// </summary>
	sealed partial class DifferDialog
		: YataDialog
	{
		#region Fields (static)
		const int WIDTH_Min = 325;
		#endregion Fields (static)


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="title">a caption on the titlebar</param>
		/// <param name="label">info to be displayed with a proportional font</param>
		/// <param name="copyable">info to be displayed with a fixed font in a
		/// RichTextBox so it can be copied</param>
		/// <param name="f">parent <c><see cref="Yata"/></c></param>
		/// <param name="color"></param>
		/// <param name="goto"></param>
		/// <param name="reset"></param>
		internal DifferDialog(
				string title,
				string label,
				string copyable,
				Yata f,
				Color color,
				bool @goto,
				bool reset)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_LOC, true);

			Text = title;

			btn_Goto .Visible = @goto;
			btn_Reset.Visible = reset;

			lbl_Info.ForeColor = color;
			lbl_Info.Height = YataGraphics.MeasureHeight(label, Font) + 15; // +15 = label's pad top+bot +5
			lbl_Info.Text = label;

			int w;
			if (!String.IsNullOrEmpty(copyable))
			{
				copyable += Environment.NewLine; // add a blank line to bot of the copyable text.

				w = GetWidth(copyable) + 30;					// +30 = parent panel's pad left+right +5
				pnl_Copyable.Height = GetHeight(copyable) + 20;	// +20 = parent panel's pad top+bot +5

				rtb_Copyable.Text = copyable;
			}
			else
			{
				pnl_Copyable.Visible = false;
				pnl_Copyable.Height = w = 0;
			}

			if (w < WIDTH_Min) w = WIDTH_Min;

			ClientSize = new Size(w + 20, // +20 = pad real and imagined.
								  lbl_Info.Height + pnl_Copyable.Height + btn_Okay.Height);

			MinimumSize = new Size(Width, Height);

			Show(_f); // Yata is owner.
		}
		#endregion


		#region Handlers (override)
		/// <summary>
		/// Overrides <c><see cref="YataDialog"/>.OnLoad()</c>. Niceties ...
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (_x == -1)
			{
				int fborder = (_f.Width - _f.ClientSize.Width) / 2;
				Point loc = PointToScreen(new Point(_f.Left + _f.ClientSize.Width + fborder,
													_f.Top  + 20));

				Screen screen = Screen.FromControl(_f);
				if      (screen.Bounds.Contains(new Point( loc.X  + Width, loc.Y))) _x =  loc.X;
				else if (screen.Bounds.Contains(new Point(_f.Left - Width, loc.Y))) _x = _f.Left - Width;
				else                                                                _x =  loc.X  - Width;

				// NOTE: '_f.Top' does not include Yata's '_bar' but does include its titlebar.
				_y = loc.Y;
			}
			Left = _x;
			Top  = _y;

			rtb_Copyable.AutoWordSelection = false; // <- needs to be here not in the designer to work right.

			if      (btn_Goto .Visible) btn_Goto .Select();
			else if (btn_Reset.Visible) btn_Reset.Select();
			else                        btn_Okay .Select();
		}

		/// <summary>
		/// Overrides this dialog's <c>FormClosing</c> handler. Sets the static
		/// location and nulls the differ in <c><see cref="Yata"/></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as Yata)._fdiffer = null;
			base.OnFormClosing(e);
		}


		/// <summary>
		/// Passes <c>[Enter]</c> etc to
		/// <c><see cref="click_btnGoto()">click_btnGoto()</see></c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Enter:
				case Keys.Enter | Keys.Control:
				case Keys.Enter | Keys.Control | Keys.Shift:
					e.SuppressKeyPress = true;
					click_btnGoto(null, EventArgs.Empty);
					break;
			}
			base.OnKeyDown(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles a click on the Reset button. Clears and desyncs the diff'd
		/// tables. Closes this dialog via <c><see cref="Yata"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_btnReset(object sender, EventArgs e)
		{
			(_f as Yata).tabclick_DiffReset(sender, e);
		}

		/// <summary>
		/// Handles a click on the Goto button. Goes to the next diff'd cell or
		/// the previous diff'd cell if <c>[Shift]</c> is depressed.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Goto"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Do not focus <c><see cref="YataGrid"/></c> if <c>[Ctrl]</c>
		/// is depressed.</remarks>
		void click_btnGoto(object sender, EventArgs e)
		{
			(_f as Yata).GotoDiffCell();
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Deters width based on longest copyable line.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		int GetWidth(string text)
		{
			string[] lines = text.Split(gs.CRandorLF, StringSplitOptions.RemoveEmptyEntries);

			int width = 0, test;
			foreach (var line in lines)
			{
				if ((test = YataGraphics.MeasureWidth(line, rtb_Copyable.Font)) > width)
					width = test;
			}
			return width;
		}

		/// <summary>
		/// Deters height based on line-height * lines.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		int GetHeight(string text)
		{
			string[] lines = text.Split(gs.CRandorLF, StringSplitOptions.None);

			return YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, rtb_Copyable.Font)
				 * lines.Length;
		}


		/// <summary>
		/// Enables/disables the goto button.
		/// </summary>
		/// <param name="enabled">true to enable</param>
		internal void EnableGotoButton(bool enabled)
		{
			btn_Goto.Enabled = enabled;
		}
		#endregion Methods
	}
}
