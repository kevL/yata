using System;
using System.Drawing;
using System.Windows.Forms;

//using System.Runtime.InteropServices;


namespace yata
{
	/// <summary>
	/// An inherited class for <c>InfoInput*</c> dialogs.
	/// <list type="bullet">
	/// <item><c><see cref="InfoInputSpells"/></c></item>
	/// <item><c><see cref="InfoInputFeat"/></c></item>
	/// <item><c><see cref="InfoInputClasses"/></c></item>
	/// <item><c><see cref="InfoInputBaseitems"/></c></item>
	/// </list>
	/// </summary>
	class InfoInputDialog
		: Form
	{
		#region Fields (static)
		static int _x = Int32.MinValue;
		static int _y = Int32.MinValue;

//		static Font TitleFont = new Font("Consolas", 7); // TODO: Dispose (not req'd.)
		#endregion Fields (static)


		#region Fields
		protected Yata _f;

		internal Cell _cell; // can't be protected unless Cell is public

		/// <summary>
		/// <c>true</c> bypasses the <c>CheckedChanged</c> handler for
		/// <c>CheckBoxes</c> and the <c>SelectedIndexChanged</c> handler for
		/// the <c>ComboBox</c>.
		/// </summary>
		/// <remarks>Initialization will configure this dialog but bypasses the
		/// handlers.</remarks>
		protected bool _init;

		protected CheckBox _cb;

		/// <summary>
		/// Pad left of the title text.
		/// </summary>
		protected string pad = "  ";
		#endregion Fields


/*		#region custom titletext font
		protected string _title = "default";
		protected int _hTitle;
		protected int _width;

		[DllImport("User32.dll")]
		static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("User32.dll")]
		static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		const int WM_NCPAINT = 0x85;

		/// <summary>
		/// Prints title-text.
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>https://stackoverflow.com/questions/9566443/how-to-change-title-bar-font-in-win-apps-by-c</remarks>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg == WM_NCPAINT)
			{
				IntPtr hdc = GetWindowDC(m.HWnd);
				Graphics graphics = Graphics.FromHdc(hdc);

				if (Options._fontf_tb != null)
					TitleFont = Options._fontf_tb;

				Size size = YataGraphics.MeasureSize(_title, TitleFont);
				float x = (_width  - size.Width)  / 2.0f - 1.0f;
				float y = (_hTitle - size.Height) / 2.0f + 1.0f;

				graphics.DrawString(_title, TitleFont, SystemBrushes.ActiveCaptionText, x,y);

				ReleaseDC(m.HWnd, hdc);
			}
		}
		#endregion custom titletext font */


		#region Methods
		/// <summary>
		/// Selects an entry in the <c>ComboBox</c> and preps the int-vals in
		/// <c><see cref="Yata"/></c> to deal with user-input.
		/// <list type="bullet">
		/// <item><c><see cref="Yata.int0"></see></c></item>
		/// <item><c><see cref="Yata.int1"></see></c></item>
		/// </list>
		/// </summary>
		/// <param name="val">the current int-val as a <c>string</c></param>
		/// <param name="co_Val">the <c>ComboBox</c> to deal with</param>
		/// <param name="bu_Clear">the Clear <c>Button</c> to disable if things
		/// go south</param>
		/// <remarks>Don't try to declare <paramref name="co_Val"/> and/or
		/// <paramref name="bu_Clear"/> in this base class because the
		/// designers can't figure that out.</remarks>
		protected void initintvals(string val, ComboBox co_Val, Button bu_Clear)
		{
			int result;
			if (Int32.TryParse(val, out result)
				&& result > -1 && result < co_Val.Items.Count - 1)
			{
				co_Val.SelectedIndex = _f.int0 = _f.int1 = result;
			}
			else
			{
				bu_Clear.Enabled = false;

				if (val == gs.Stars) _f.int0 = Yata.Info_ASSIGN_STARS;
				else                 _f.int0 = Yata.Info_INIT_INVALID;

				_f.int1 = Yata.Info_ASSIGN_STARS;

				co_Val.SelectedIndex = co_Val.Items.Count - 1;
			}
		}

		/// <summary>
		/// Selects an entry in the <c>ComboBox</c> and preps the int-vals in
		/// <c><see cref="Yata"/></c> to deal with user-input when the value in
		/// the 2da field is 1 greater than the values in the
		/// <c><see cref="Info"/></c> list.
		/// <list type="bullet">
		/// <item><c><see cref="Yata.int0"></see></c></item>
		/// <item><c><see cref="Yata.int1"></see></c></item>
		/// </list>
		/// </summary>
		/// <param name="val">the current int-val as a <c>string</c></param>
		/// <param name="co_Val">the <c>ComboBox</c> to deal with</param>
		/// <param name="bu_Clear">the Clear <c>Button</c> to disable if things
		/// go south</param>
		protected void initintvals_1(string val, ComboBox co_Val, Button bu_Clear)
		{
			int result;
			if (Int32.TryParse(val, out result)
				&& result > 0 && result < co_Val.Items.Count)
			{
				co_Val.SelectedIndex = result - 1;
				_f.int0 = _f.int1 = result;
			}
			else
			{
				bu_Clear.Enabled = false;

				if (val == gs.Stars) _f.int0 = Yata.Info_ASSIGN_STARS;
				else                 _f.int0 = Yata.Info_INIT_INVALID;

				_f.int1 = Yata.Info_ASSIGN_STARS;

				co_Val.SelectedIndex = co_Val.Items.Count - 1;
			}
		}


		/// <summary>
		/// Clears all <c>CheckBoxes</c> except the current <c>CheckBox</c>
		/// <c><see cref="_cb"/></c>. Set <c>(_cb = null)</c> to clear all
		/// <c>Checkboxes</c>.
		/// </summary>
		/// <remarks>Disregards invocation MetaMagic group <c>CheckBoxes</c> in
		/// <c><see cref="InfoInputSpells"/></c>.</remarks>
		protected void clearchecks()
		{
			_init = true;

			CheckBox cb;
			foreach (var control in Controls)
			{
				if ((cb = control as CheckBox) != null
					&& cb.Checked && (_cb == null || cb != _cb))
				{
					cb.Checked = false;
				}
			}
			_init = false;
		}
		#endregion Methods


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler. Sets <c>Location</c> wrt the
		/// desktop.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (_x != Int32.MinValue) // TODO: check if isOnScreen
			{
				DesktopLocation = new Point(_x, _y);
			}
			else
				CenterToParent();

			base.OnLoad(e);
		}

		/// <summary>
		/// Overrides the <c>Closing</c> handler. Stores <c>Location</c> wrt
		/// the desktop.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			_x = Math.Max(0, DesktopLocation.X);
			_y = Math.Max(0, DesktopLocation.Y);

			base.OnClosing(e);
		}

		/// <summary>
		/// Closes this dialog on <c>[Esc]</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Handlers (override)
	}
}
