using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (font)
		/// <summary>
		/// Handles opening the FontMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"><c><see cref="it_MenuFont"/></c></param>
		/// <param name="e"></param>
		void font_dropdownopening(object sender, EventArgs e)
		{
			it_Font       .Enabled = _diff1 == null || _diff2 == null;
			it_FontDefault.Enabled = !it_Font.Checked
								  && !Font.Equals(FontDefault);
		}

		/// <summary>
		/// Opens the font-picker dialog - <c><see cref="FontDialog"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Font"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Font|Font...be patient</item>
		/// </list></remarks>
		void fontclick_Font(object sender, EventArgs e)
		{
			if (_ffont == null)
			{
				_ffont = new FontDialog(this);
				it_Font.Checked = true;
			}
			else
			{
				if (_ffont.WindowState == FormWindowState.Minimized)
				{
					if (_ffont.Maximized)
						_ffont.WindowState = FormWindowState.Maximized;
					else
						_ffont.WindowState = FormWindowState.Normal;
				}
				_ffont.BringToFront();
			}
		}

		/// <summary>
		/// Sets Yata's <c>Font</c> to <c><see cref="FontDefault"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_FontDefault"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Font|Load default font</item>
		/// </list>
		/// <br/><br/>
		/// The item will be disabled if <c><see cref="FontDialog"/></c> is open
		/// or if Yata's current <c>Font</c> is
		/// <c><see cref="FontDefault"/></c>.</remarks>
		void fontclick_Default(object sender, EventArgs e)
		{
			doFont(FontDefault.Clone() as Font);
		}
		#endregion Handlers (font)


		#region Methods (font)
		/// <summary>
		/// Dechecks the "Font ... be patient" it when
		/// <c><see cref="FontDialog"/></c> closes.
		/// </summary>
		internal void CloseFontDialog()
		{
			_ffont = null;
			it_Font.Checked = false;
		}

		/// <summary>
		/// Applies a specified <c>Font</c> to Yata.
		/// </summary>
		/// <param name="font"></param>
		internal void doFont(Font font)
		{
			// NOTE: Cf f.AutoScaleMode (None,Font,DPI,Inherit)
			// Since I'm doing all the necessary scaling due to font-changes
			// w/ code the AutoScaleMode should not be set to default "Font".
			// It might better be set to "DPI" for those weirdos and I don't
			// know what "Inherit" means (other than the obvious).
			// AutoScaleMode is currently set to "None".
			//
			// See also SetProcessDPIAware()
			// NOTE: Apparently setting GraphicsUnit.Pixel when creating new
			// Font-objects effectively bypasses the OS's DPI user-setting.

			Font.Dispose();
			Font = font;

			Options._fontdialog.Dispose();
			Options._fontdialog = Options.CreateDialogFont(Font);

			FontAccent.Dispose();
			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));


			YataGrid.SetStaticMetrics(this);

			if (Table != null)
			{
				Obfuscate();
//				DrawRegulator.SuspendDrawing(Table);

				SetTabSize();

				YataGrid table;
				for (int tab = 0; tab != Tabs.TabCount; ++tab)
				{
					table = Tabs.TabPages[tab].Tag as YataGrid;
					table.CreateCols(true);

					for (int c = 0; c != table.ColCount; ++c)
					{
						table.colTextwidth(c);
					}

					table.Calibrate(0, table.RowCount - 1); // font

					// TODO: This is effed because the Height (at least) of each
					// table is not well-defined by .NET - OnResize() for the
					// tables gets called multiples times per table and the
					// value of Height changes arbitrarily. Since an accurate
					// Height is required by InitScrollers() a glitch occurs
					// when the height of Font increases. That is if a cell or
					// a row is currently selected it will NOT be fully
					// displayed if it is NOT on the currently displayed page
					// and it is near the bottom of the tab-control's page-area;
					// several pixels of the selected cell or row will still be
					// covered by the horizontal scroller. Given the arbitrary
					// Height changes that occur throughout this function's
					// sequence in fact it's surprising/remarkable that things
					// turn out even almost correct.
					// NOTE: Height of any table should NOT be changing at all.

					if (table == Table)
						table.Invalidator(table.EnsureDisplayed());
				}

//				DrawRegulator.ResumeDrawing(Table);
				Obfuscate(false);

				if (_ffont != null)			// layout for big tables will send the Font dialog below the form ->
					_ffont.BringToFront();	// (although it should never be behind the form because its owner IS the form)
			}
		}
		#endregion Methods (font)
	}
}
