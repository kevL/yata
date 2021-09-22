using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class RowCreatorDialog
		: YataDialog
	{
		#region Enumerators
		enum StrtType
		{ Add, Insert }

		enum StopType
		{ non, Finish, Count }
		#endregion Enumerators


		#region Fields (static)
		static StrtType _strt;
		static StopType _stop = StopType.non;

		static string _count = "1";

		const string ADD    = "Add";
		const string INSERT = "Insert";

		readonly static Random _rand = new Random();
		#endregion Fields (static)


		#region Fields
		bool _init, _cancel;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="f">the <c><see cref="YataForm"/></c></param>
		/// <param name="r">the currently selected row-id</param>
		/// <param name="copyfillenabled"><c>true</c> if at least one
		/// <c><see cref="Row"/></c> has been copied into
		/// <c><see cref="YataForm"/>._copyr</c></param>
		internal RowCreatorDialog(YataForm f, int r, bool copyfillenabled)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_LOC, true);

			if (r != -1)
			{
				_strt = StrtType.Insert;
				_stop = StopType .Count;
			}
			else
			{
				_strt = StrtType.Add;

				rb_FillSelected.Enabled = false;
				la_FillSelected.ForeColor = SystemColors.GrayText;
			}

			if (!copyfillenabled)
			{
				rb_FillCopied.Enabled = false;
				la_FillCopied.ForeColor = SystemColors.GrayText;
			}


			_init = true;

			switch (_strt)
			{
				case StrtType.Add:
					rb_StartAdd   .Checked =
					tb_StartAdd   .Enabled = true;

					rb_StartInsert.Checked =
					tb_StartInsert.Enabled = false;
					break;

				case StrtType.Insert:
					rb_StartAdd   .Checked =
					tb_StartAdd   .Enabled = false;

					rb_StartInsert.Checked =
					tb_StartInsert.Enabled = true;
					break;
			}

			switch (_stop)
			{
				case StopType.non:
					rb_StopFinish.Checked =
					tb_StopFinish.Enabled = _strt == StrtType.Add;

					rb_StopCount .Checked =
					tb_StopCount .Enabled = _strt == StrtType.Insert;
					break;

				case StopType.Finish:
					rb_StopFinish.Checked =
					tb_StopFinish.Enabled = true;

					rb_StopCount .Checked =
					tb_StopCount .Enabled = false;
					break;

				case StopType.Count:
					rb_StopFinish.Checked =
					tb_StopFinish.Enabled = false;

					rb_StopCount .Checked =
					tb_StopCount .Enabled = true;
					break;
			}

			tb_StartAdd   .Text = YataForm.Table.Rows.Count.ToString();
			tb_StartInsert.Text = (r + 1).ToString();

			int result = Int32.Parse(_count);	// shall be valid and greater than 0.
			Control tb;
			if (rb_StartAdd.Checked)			// readonly - shall be valid.
			{
				tb = tb_StartAdd;
				btn_Accept.Text = ADD;
			}
			else // rb_StartInsert.Checked		// shall be valid.
			{
				tb = tb_StartInsert;
				btn_Accept.Text = INSERT;
			}

			tb_StopFinish.Text = (Int32.Parse(tb.Text) + result - 1).ToString();
			tb_StopCount .Text = _count;

			_init = false;

			tb_StartAdd.BackColor = Colors.TextboxReadonly;

			if (rb_StartInsert.Checked)
				tb_StartInsert.BackColor = Colors.TextboxSelected;
			else
				tb_StartInsert.BackColor = Colors.TextboxBackground;

			if (rb_StopFinish.Checked)
			{
				tb_StopFinish.BackColor = Colors.TextboxSelected;
				tb_StopCount .BackColor = Colors.TextboxBackground;
			}
			else // rb_StopCount.Checked
			{
				tb_StopFinish.BackColor = Colors.TextboxBackground;
				tb_StopCount .BackColor = Colors.TextboxSelected;
			}

			btn_Accept.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!(e.Cancel = _cancel))
			{
				if (rb_StartAdd.Checked)
					_strt = StrtType.Add;
				else // rb_StartInsert.Checked
					_strt = StrtType.Insert;

				if (rb_StopCount.Checked)
					_stop = StopType.Count;
				else // rb_StopFinish.Checked
					_stop = StopType.Finish;

				int result;
				if (Int32.TryParse(tb_StopCount.Text, out result)
					&& result > 0)
				{
					_count = tb_StopCount.Text;
				}
				else
					_count = "1";

				base.OnFormClosing(e);
			}
			else
				_cancel = false;
		}


		/// <summary>
		/// Overrides the <c>KeyDown</c> handler. <c>[F2]</c> closes this
		/// <c>RowCreatorDialog</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.F2)
				Close();

			base.OnKeyDown(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles <c>CheckChanged</c> for the <c>RadioButtons</c>. Changes
		/// <c>TextBox</c> values to reflect the current <c>RadioButton</c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rb_StartAdd"/></c></item>
		/// <item><c><see cref="rb_StopFinish"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void checkedchanged(object sender, EventArgs e)
		{
			if (!_init)
			{
				var rb = sender as RadioButton;

				if (rb == rb_StartAdd)
				{
					Control tb;

					if (tb_StartAdd.Enabled = rb.Checked)
					{
						btn_Accept.Text = ADD;

						tb = tb_StartAdd;

						tb_StartInsert.BackColor = Colors.TextboxBackground;
						tb_StartInsert.Enabled   = false;
					}
					else
					{
						btn_Accept.Text = INSERT;

						(tb = tb_StartInsert).BackColor = Colors.TextboxSelected;
						 tb                  .Enabled   = true;
					}

					int result2;
					if (Int32.TryParse(tb_StopCount.Text, out result2)
						&& result2 > 0)
					{
						int result;
						if (Int32.TryParse(tb.Text, out result)
							&& result > -1 && result <= YataForm.Table.RowCount)
						{
							tb_StopFinish.Text = (result + result2 - 1).ToString();
						}
					}
				}
				else // rb == rb_StopFinish
				{
					if (tb_StopFinish.Enabled = rb.Checked)
					{
						tb_StopFinish.BackColor = Colors.TextboxSelected;
						tb_StopFinish.Enabled   = true;

						tb_StopCount .BackColor = Colors.TextboxBackground;
						tb_StopCount .Enabled   = false;
					}
					else
					{
						tb_StopFinish.BackColor = Colors.TextboxBackground;
						tb_StopFinish.Enabled   = false;

						tb_StopCount .BackColor = Colors.TextboxSelected;
						tb_StopCount .Enabled   = true;
					}
				}
			}
		}

		/// <summary>
		/// Handles <c>TextChanged</c> for the <c>TextBoxes</c>. Changes other
		/// <c>TextBox's</c> values to reflect the current text.
		/// </summary>
		/// <param name="sender"></param>
		/// <list type="bullet">
		/// <item>tb_StartInsert</item>
		/// <item>tb_StopCount</item>
		/// <item>tb_StopFinish</item>
		/// </list>
		/// <param name="e"></param>
		void textchanged(object sender, EventArgs e)
		{
			if (!_init)
			{
				var tb = sender as TextBox;

				int result;
				if (Int32.TryParse(tb.Text, out result)
					&& (result > 0 || (result > -1 && tb != tb_StopCount)))
				{
					int result2;
					if (tb == tb_StartInsert)
					{
						if (result > YataForm.Table.Rows.Count)
						{
							tb_StartInsert.Text = YataForm.Table.Rows.Count.ToString(); // -> recurse
							tb_StartInsert.SelectionStart = tb_StartInsert.Text.Length;
						}
						else
						{
							tb_StartInsert.ForeColor = SystemColors.WindowText;
							la_StartInsert.ForeColor = SystemColors.ControlText;

							if (Int32.TryParse(tb_StopCount.Text, out result2)
								&& result2 > 0)
							{
								tb_StopFinish.Text = (result + result2 - 1).ToString();
							}
						}
					}
					else if (tb == tb_StopFinish)
					{
						if (rb_StartAdd.Checked)
						{
							result2 = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.
							if (result < result2)
							{
								tb_StopFinish.ForeColor =
								la_StopFinish.ForeColor = Color.Firebrick;
							}
							else
							{
								tb_StopFinish.ForeColor = SystemColors.WindowText;
								la_StopFinish.ForeColor = SystemColors.ControlText;

								tb_StopCount.Text = (result - result2 + 1).ToString();
							}
						}
						else // rb_StartInsert.Checked
						{
							if (!Int32.TryParse(tb_StartInsert.Text, out result2)
								|| result < result2)
							{
								tb_StopFinish.ForeColor =
								la_StopFinish.ForeColor = Color.Firebrick;
							}
							else
							{
								tb_StopFinish.ForeColor = SystemColors.WindowText;
								la_StopFinish.ForeColor = SystemColors.ControlText;

								tb_StopCount.Text = (result - result2 + 1).ToString();
							}
						}
					}
					else // tb == tb_StopCount
					{
						tb_StopCount.ForeColor = SystemColors.WindowText;
						la_StopCount.ForeColor = SystemColors.ControlText;

						if (rb_StartAdd.Checked)
						{
							result2 = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.
							tb_StopFinish.Text = (result + result2 - 1).ToString();
						}
						else // rb_StartInsert.Checked
						{
							if (Int32.TryParse(tb_StartInsert.Text, out result2))
								tb_StopFinish.Text = (result + result2 - 1).ToString();
						}
					}
				}
				else
				{
					if (tb == tb_StartInsert)
					{
						tb_StartInsert.ForeColor =
						la_StartInsert.ForeColor = Color.Firebrick;
					}
					else if (tb == tb_StopFinish)
					{
						tb_StopFinish.ForeColor =
						la_StopFinish.ForeColor = Color.Firebrick;
					}
					else // tb == tb_StopCount
					{
						tb_StopCount.ForeColor =
						la_StopCount.ForeColor = Color.Firebrick;
					}
				}
			}
		}


		/// <summary>
		/// Fires when user clicks Ok. Let the chips fly.
		/// </summary>
		/// <param name="sender"><c><see cref="btn_Accept"/></c></param>
		/// <param name="e"></param>
		void click_Ok(object sender, EventArgs e)
		{
			_cancel = true;

			var f = _f as YataForm;

			int result;
			if (rb_StartAdd.Checked)
			{
				f._startCr = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.

				if (rb_StopFinish.Checked)
				{
					if (Int32.TryParse(tb_StopFinish.Text, out result)
						&& (f._lengthCr = result - f._startCr + 1) > 0)
					{
						_cancel = false;
					}
				}
				else // rb_StopCount.Checked
				{
					if (Int32.TryParse(tb_StopCount.Text, out result)
						&& (f._lengthCr = result) > 0)
					{
						_cancel = false;
					}
				}
			}
			else // rb_StartInsert.Checked
			{
				if (Int32.TryParse(tb_StartInsert.Text, out result)
					&& (f._startCr = result) > -1
					&&  f._startCr <= YataForm.Table.RowCount)
				{
					if (rb_StopFinish.Checked)
					{
						if (Int32.TryParse(tb_StopFinish.Text, out result)
							&& (f._lengthCr = result - f._startCr + 1) > 0)
						{
							_cancel = false;
						}
					}
					else // rb_StopCount.Checked
					{
						if (Int32.TryParse(tb_StopCount.Text, out result)
							&& (f._lengthCr = result) > 0)
						{
							_cancel = false;
						}
					}
				}
			}

			if (_cancel)
			{
				using (var ib = new Infobox(gs.InfoboxTitle_error,
											GetDarthQuote(),
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
			else if (rb_FillCopied  .Checked) f._fillCr = YataForm.CrFillType.Copied;
			else if (rb_FillSelected.Checked) f._fillCr = YataForm.CrFillType.Selected;
			else                              f._fillCr = YataForm.CrFillType.Stars; // rb_FillStars.Checked
		}


		/// <summary>
		/// Enables a fillstyle <c>RadioButton</c> when its corresponding
		/// <c>Label</c> is clicked.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="la_FillCopied"/></c></item>
		/// <item><c><see cref="la_FillSelected"/></c></item>
		/// <item><c><see cref="la_FillStars"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void click_Fill(object sender, EventArgs e)
		{
			var la = sender as Label;

			if (la == la_FillCopied)
			{
				if (rb_FillCopied.Enabled && !rb_FillCopied.Checked)
					rb_FillCopied.Checked = true;
			}
			else if (la == la_FillSelected)
			{
				if (rb_FillSelected.Enabled && !rb_FillSelected.Checked)
					rb_FillSelected.Checked = true;
			}
			else // la == la_FillStars
			{
				if (!rb_FillStars.Checked)
					 rb_FillStars.Checked = true;
			}
		}
		#endregion Handlers


		#region Methods (static)
		/// <summary>
		/// Gets a Darth Vader quote.
		/// </summary>
		/// <returns>a Darth Vader quote</returns>
		static string GetDarthQuote()
		{
			string quote = String.Empty;

			switch (_rand.Next(38))
			{
				case  0: quote = "I find your lack of faith disturbing."; break;
				case  1: quote = "When I left you I was but the learner."; break;
				case  2: quote = "The Force is strong with this one."; break;
				case  3: quote = "I am altering the deal."; break;
				case  4: quote = "Obi-Wan has taught you well."; break;
				case  5: quote = "Only your hatred can destroy me."; break;
				case  6: quote = "I am your father!"; break;
				case  7: quote = "Give yourself to the Dark Side."; break;
				case  8: quote = "I see through the lies of the Jedi."; break;
				case  9: quote = "I do not fear the dark side as you do."; break;
				case 10: quote = "I have brought peace, freedom, justice, and security to my new empire."; break;
				case 11: quote = "Just for once let me look on you with my own eyes."; break;
				case 12: quote = "Don't be too proud of this technological terror you've constructed."; break;
				case 13: quote = "The ability to destroy a planet is insignificant next to the power of the Force."; break;
				case 14: quote = "Yes, your thoughts betray you."; break;
				case 15: quote = "You are a part of the Rebel Alliance and a traitor!"; break;
				case 16: quote = "From my point of view, the Jedi are evil."; break;
				case 17: quote = "Asteroids do not concern me, Admiral."; break;
				case 18: quote = "He will join us or die, Master."; break;
				case 19: quote = "You don't know the power of the dark side."; break;
				case 20: quote = "You have failed me for the last time."; break;
				case 21: quote = "What is thy bidding, my master."; break;
				case 22: quote = "Apology excepted."; break;
				case 23: quote = "Don't make me destroy you."; break;
				case 24: quote = "If you only knew the power of the dark side."; break;
				case 25: quote = "There is no escape."; break;
				case 26: quote = "All I am surrounded by is fear!"; break;
				case 27: quote = "Perhaps I can find new ways to motivate them."; break;
				case 28: quote = "Search your feelings, you know it to be true!"; break;
				case 29: quote = "I do not want the Emperor's prize damaged."; break;
				case 30: quote = "If he could be turned, he would be a powerful ally."; break;
				case 31: quote = "You are unwise to lower your defenses!"; break;
				case 32: quote = "This will be a day long remembered."; break;
				case 33: quote = "I have you now."; break;
				case 34: quote = "I have felt it."; break;
				case 35: quote = "The Emperor will show you the true nature of the Force."; break;
				case 36: quote = "Now I am the master."; break;
				case 37: quote = "You can't make this stuff up."; break; // that is not a DarthVader quote.
			}
			return quote + Environment.NewLine + Environment.NewLine
				 + "- Darth Vader";
		}
		#endregion Methods (static)
	}
}
