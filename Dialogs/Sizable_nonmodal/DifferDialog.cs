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
			Initialize(METRIC_LOC, true);

			Text = title;

			bu_Goto .Visible = @goto;
			bu_Reset.Visible = reset;

			la_Info.ForeColor = color;
			la_Info.Height = YataGraphics.MeasureHeight(label, Font) + 15; // +15 = label's pad top+bot +5
			la_Info.Text = label;

			int w;
			if (!String.IsNullOrEmpty(copyable))
			{
				copyable += Environment.NewLine; // add a blank line to bot of the copyable text.

				w = GetWidth(copyable) + 30;					// +30 = parent panel's pad left+right +5
				pa_Copyable.Height = GetHeight(copyable) + 20;	// +20 = parent panel's pad top+bot +5

				rt_Copyable.Text = copyable;
			}
			else
			{
				pa_Copyable.Visible = false;
				pa_Copyable.Height = w = 0;
			}

			if (w < WIDTH_Min) w = WIDTH_Min;

			ClientSize = new Size(w + 20, // +20 = pad real and imagined.
								  la_Info.Height + pa_Copyable.Height + bu_Okay.Height);

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

			rt_Copyable.AutoWordSelection = false; // <- needs to be here not in the designer to work right.

			if      (bu_Goto .Visible) bu_Goto .Select();
			else if (bu_Reset.Visible) bu_Reset.Select();
			else                       bu_Okay .Select();
		}

		/// <summary>
		/// Overrides this dialog's <c>FormClosing</c> handler. Sets the static
		/// location and nulls
		/// <c><see cref="Yata._fdiffer">Yata._fdiffer</see></c>.
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
		/// <param name="sender"><c><see cref="bu_Goto"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Do not focus <c><see cref="YataGrid"/></c> if <c>[Ctrl]</c>
		/// is depressed.</remarks>
		void click_btnGoto(object sender, EventArgs e)
		{
			(_f as Yata).GotoDiff();
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
				if ((test = YataGraphics.MeasureWidth(line, rt_Copyable.Font)) > width)
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

			return YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, rt_Copyable.Font)
				 * lines.Length;
		}


		/// <summary>
		/// Enables/disables the goto button.
		/// </summary>
		/// <param name="enabled">true to enable</param>
		internal void EnableGotoButton(bool enabled)
		{
			bu_Goto.Enabled = enabled;
		}
		#endregion Methods
	}
}
