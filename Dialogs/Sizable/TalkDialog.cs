using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog that displays entries of TalkTable(s).
	/// </summary>
	sealed partial class TalkDialog
		: YataDialog
	{
		#region Fields (static)
		const int WIDTH_Min = 325;
		const int WIDTH_Max = 900;
		const int HIGHT_Min = 130;
		const int HIGHT_Max = 500;

		const int pad_HORI = 20; // pad real and imagined
		const int pad_VERT = 10; // pad above Cancel button
		#endregion Fields (static)


		#region Fields
		int _eId; // talktable's EntryId
		int _eId_init;

		SortedDictionary<int, string> _dict;
		int lo, hi;

		bool _init;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="f"></param>
		/// <remarks>Check that the <c><see cref="Cell.text">Cell.text</see></c>
		/// of <paramref name="cell"/> parses to a valid value before invoking
		/// this <c>TalkDialog</c>.</remarks>
		internal TalkDialog(Cell cell, Yata f)
		{
			_f = f;

			InitializeComponent();
			Initialize(METRIC_LOC);

			string strref = cell.text;
			if (strref == gs.Stars) strref = "0";

			Text = " tlk - " + strref;

			_eId_init = Int32.Parse(strref, CultureInfo.InvariantCulture);

			if (_eId_init == TalkReader.invalid || (_eId_init & TalkReader.bitCusto) == 0)
			{
				_dict = TalkReader.DictDialo;
				lo = TalkReader.loDialo;
				hi = TalkReader.hiDialo;
			}
			else
			{
				_dict = TalkReader.DictCusto;
				lo = TalkReader.loCusto;
				hi = TalkReader.hiCusto;

				_init = true;
				cb_Custo.Checked = true;
				_init = false;
			}

			if (_eId_init != TalkReader.invalid)
				_eId_init &= TalkReader.strref;


			tb_Strref.Text = _eId_init.ToString(CultureInfo.InvariantCulture); // <- sets '_eId' and 'rt_Copyable.Text'


			if (TalkReader.AltLabel != null)
				cb_Custo.Text = TalkReader.AltLabel;


			if ((bu_Accept.Enabled = !Yata.Table.Readonly))
				bu_Accept.Select();
			else
				bu_Cancel.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler. Niceties ...
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			int w = GetWidth(rt_Copyable.Text) + 30;				// +30 = parent panel's pad left+right +5
			pa_Copyable.Height = GetHeight(rt_Copyable.Text) + 20;	// +20 = parent panel's pad top+bot +5

			if      (w < WIDTH_Min) w = WIDTH_Min;
			else if (w > WIDTH_Max) w = WIDTH_Max;

			int h = pa_Head.Height + pa_Copyable.Height + bu_Cancel.Height;
			if      (h < HIGHT_Min) h = HIGHT_Min;
			else if (h > HIGHT_Max) h = HIGHT_Max;

			ClientSize = new Size(w + pad_HORI,
								  h + pad_VERT);

			enable();


			int widthborder = (Width  - ClientSize.Width) / 2;
			int heighttitle = (Height - ClientSize.Height - 2 * widthborder);

			MinimumSize = new Size(WIDTH_Min + pad_HORI + 2 * widthborder,
								   HIGHT_Min + pad_VERT + 2 * widthborder + heighttitle);

			base.OnLoad(e);
		}

		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			pa_Copyable.Height = ClientSize.Height
							   - pa_Head   .Height
							   - bu_Cancel .Height - pad_VERT;
			base.OnResize(e);
			pa_Copyable.Invalidate();
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles the textchanged event of the strref-box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Strref"/></c></param>
		/// <param name="e"></param>
		void textchanged_Strref(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(tb_Strref.Text))
			{
				_eId = _eId_init; // revert to default.
			}
			else
			{
				int result;
				if (!Int32.TryParse(tb_Strref.Text, out result)
					|| result < TalkReader.invalid || result > TalkReader.strref)
				{
					tb_Strref.Text = _eId_init.ToString(CultureInfo.InvariantCulture);
					tb_Strref.SelectionStart = tb_Strref.Text.Length;
					return; // recurse.
				}

				// else let the Select button handle it.
				_eId = result; // This is the setter for the '_eId' val.
			}

			if (_dict.ContainsKey(_eId))
				rt_Copyable.Text = _dict[_eId];
			else
				rt_Copyable.Text = String.Empty;
		}

		/// <summary>
		/// Handles a click on the Select button. Passes the current strref to
		/// <c>Yata</c> and closes this dialog.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Accept"/></c></param>
		/// <param name="e"></param>
		void click_btnSelect(object sender, EventArgs e)
		{
			bool proceed = _eId == TalkReader.invalid
						|| _dict.Count == 0			// -> talkfile not loaded, therefore user knows what he/she
						|| _dict.ContainsKey(_eId);	// is doing (ie, red panel BG) so let it go through.

			if (!proceed)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											"Entry not found in the .tlk file. Proceed ...",
											null,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					proceed = ib.ShowDialog(this) == DialogResult.OK;
				}
			}

			if (proceed)
			{
				if (_eId != TalkReader.invalid && cb_Custo.Checked)
					_eId |= TalkReader.bitCusto;

				(_f as Yata)._strref = _eId.ToString(CultureInfo.InvariantCulture);
				Close();
			}
		}

		/// <summary>
		/// Handles a click on the Load ... button.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Load"/></c></param>
		/// <param name="e"></param>
		void click_btnLoad(object sender, EventArgs e)
		{
			if (!cb_Custo.Checked)
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select Dialog.Tlk";
					ofd.Filter = Yata.GetTlkFilter();

					ofd.FileName = "dialog.tlk";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, (_f as Yata).it_PathTalkD);
				}

				lo = TalkReader.loDialo;
				hi = TalkReader.hiDialo;
			}
			else
			{
				using (var ofd = new OpenFileDialog())
				{
					ofd.Title  = " Select a TalkTable";
					ofd.Filter = Yata.GetTlkFilter();

					ofd.FileName = "*.tlk";
					ofd.AutoUpgradeEnabled = false;

					if (ofd.ShowDialog() == DialogResult.OK)
						TalkReader.Load(ofd.FileName, (_f as Yata).it_PathTalkC, true);
				}

				lo = TalkReader.loCusto;
				hi = TalkReader.hiCusto;
			}


			enable();

			if (_dict.ContainsKey(_eId))
				rt_Copyable.Text = _dict[_eId];
			else
				rt_Copyable.Text = String.Empty;

			if (TalkReader.AltLabel != null)
				cb_Custo.Text = TalkReader.AltLabel;
			else
				cb_Custo.Text = "Custom";
		}


		/// <summary>
		/// Handles a click on the Prevert button. Reverts the strref to its
		/// initial val.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Prevert"/></c></param>
		/// <param name="e"></param>
		void click_btnPrevert(object sender, EventArgs e)
		{
			tb_Strref.Text = _eId_init.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Handles a click on the Pre button. Steps backward to the antecedent
		/// dialog-entry.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Backward"/></c></param>
		/// <param name="e"></param>
		void click_btnPre(object sender, EventArgs e)
		{
			do
			{
				if (_eId <= lo)
					_eId = hi + 1;

				if (_dict.ContainsKey(--_eId))
				{
					tb_Strref.Text = _eId.ToString(CultureInfo.InvariantCulture);
					return;
				}
			}
			while (true);
		}

		/// <summary>
		/// Handles a click on the Pos button. Steps forward to the posterior
		/// dialog-entry.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Forward"/></c></param>
		/// <param name="e"></param>
		void click_btnPos(object sender, EventArgs e)
		{
			do
			{
				if (_eId >= hi)
					_eId = lo - 1;

				if (_dict.ContainsKey(++_eId))
				{
					tb_Strref.Text = _eId.ToString(CultureInfo.InvariantCulture);
					return;
				}
			}
			while (true);
		}


		/// <summary>
		/// Does a jig.
		/// </summary>
		/// <param name="sender"><c><see cref="cb_Custo"/></c></param>
		/// <param name="e"></param>
		void checkchanged_Custo(object sender, EventArgs e)
		{
			if (!_init)
			{
				if (!cb_Custo.Checked)
				{
					_dict = TalkReader.DictDialo;
					lo = TalkReader.loDialo;
					hi = TalkReader.hiDialo;
				}
				else
				{
					_dict = TalkReader.DictCusto;
					lo = TalkReader.loCusto;
					hi = TalkReader.hiCusto;
				}


				enable();

				if (_dict.ContainsKey(_eId))
					rt_Copyable.Text = _dict[_eId];
				else
					rt_Copyable.Text = String.Empty;
			}
		}


		/// <summary>
		/// Draws a 1px border around the copyable-panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_Copyable"/></c></param>
		/// <param name="e"></param>
		void paint_CopyPanel(object sender, PaintEventArgs e)
		{
			int w = pa_Copyable.Width  - 1;
			int h = pa_Copyable.Height - 1;

			var tl = new Point(0, 0);
			var tr = new Point(w, 0);
			var br = new Point(w, h);
			var bl = new Point(0, h);

			Graphics graphics = e.Graphics;
			graphics.DrawLine(Pencils.DarkLine, tl, tr);
			graphics.DrawLine(Pencils.DarkLine, tr, br);
			graphics.DrawLine(Pencils.DarkLine, br, bl);
			graphics.DrawLine(Pencils.DarkLine, bl, tl);
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
		/// Enables <c><see cref="bu_Forward"/></c> and
		/// <c><see cref="bu_Backward"/></c> based on whether there are any
		/// entries in the current TalkTable's <c>Dictionary</c>. Also sets the
		/// background color.
		/// </summary>
		void enable()
		{
			Color color;
			if (bu_Backward.Enabled =
				bu_Forward .Enabled = _dict.Count != 0)
			{
				color = Colors.TalkfileLoaded;
			}
			else
				color = Colors.TalkfileLoaded_f;

			pa_Copyable.BackColor =
			rt_Copyable.BackColor = color;
		}
		#endregion Methods
	}
}
