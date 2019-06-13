using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class RowCreatorDialog
		:
			Form
	{
		#region Enums
		enum StartType
		{ non, Add, Insert }

		enum StopType
		{ non, Finish, Count }
		#endregion Enums


		#region Fields (static)
		static StartType _start = StartType.non;
		static StopType  _stop  = StopType .non;

		static string _count = "1";

		readonly static Random _rand = new Random();
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		bool _cancel, _init;
		#endregion Fields


		#region cTor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="f"></param>
		/// <param name="enablefill"></param>
		internal RowCreatorDialog(YataForm f, bool enablefill)
		{
			InitializeComponent();

			_f = f;

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			if (Settings._fontf != null)
			{
				tb_StartAdd   .Font =
				tb_StartInsert.Font =
				tb_StopFinish .Font =
				tb_StopCount  .Font = Settings._fontf;
			}
			else
			{
				tb_StartAdd   .Font =
				tb_StartInsert.Font =
				tb_StopFinish .Font =
				tb_StopCount  .Font = _f.Font;
			}


			_init = true;

			int r = YataForm.Table.getSelectedRow() + 1;
			if (r != 0)
			{
				_start = StartType.Insert;
				_stop  = StopType.Count;
			}

			switch (_start)
			{
				case StartType.non:
					rb_StartAdd   .Checked =
					tb_StartAdd   .Enabled = (r == 0);
					rb_StartInsert.Checked =
					tb_StartInsert.Enabled = (r != 0);
					break;

				case StartType.Add:
					rb_StartAdd   .Checked =
					tb_StartAdd   .Enabled = true;
					rb_StartInsert.Checked =
					tb_StartInsert.Enabled = false;
					break;

				case StartType.Insert:
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
					tb_StopFinish.Enabled =  rb_StartAdd.Checked;
					rb_StopCount .Checked =
					tb_StopCount .Enabled = !rb_StartAdd.Checked;
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
			tb_StartInsert.Text = r.ToString();

			int result = Int32.Parse(_count); // shall be valid and greater than 0.
			if (rb_StartAdd.Checked)
				tb_StopFinish.Text = (Int32.Parse(tb_StartAdd   .Text) + result - 1).ToString(); // readonly - shall be valid.
			else //if (rb_StartInsert.Checked)
				tb_StopFinish.Text = (Int32.Parse(tb_StartInsert.Text) + result - 1).ToString(); // shall be valid.

			tb_StopCount.Text = _count;

			_init = false;


			tb_StartAdd.BackColor = Color.Azure;

			if (rb_StartInsert.Checked)
				tb_StartInsert.BackColor = Color.FloralWhite;
			else
				tb_StartInsert.BackColor = Color.WhiteSmoke;

			if (rb_StopFinish.Checked)
			{
				tb_StopFinish.BackColor = Color.FloralWhite;
				tb_StopCount .BackColor = Color.WhiteSmoke;
			}
			else //if (rb_StopCount.Checked)
			{
				tb_StopFinish.BackColor = Color.WhiteSmoke;
				tb_StopCount .BackColor = Color.FloralWhite;
			}

			cb_Fill.Enabled = enablefill;
		}
		#endregion cTor


		#region Events (override)
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!(e.Cancel = _cancel))
			{
				if (rb_StartAdd.Checked)
					_start = StartType.Add;
				else //if (rb_StartInsert.Checked)
					_start = StartType.Insert;

				if (rb_StopCount.Checked)
					_stop = StopType.Count;
				else //if (rb_StopFinish.Checked)
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
		#endregion Events (override)


		#region Events
		void checkchanged(object sender, EventArgs e)
		{
			if (!_init)
			{
				var rb = sender as RadioButton;

				if (rb == rb_StartAdd)
				{
					tb_StartAdd   .Enabled =  rb.Checked;
					tb_StartInsert.Enabled = !rb.Checked;

					tb_StartInsert.BackColor = Color.WhiteSmoke;

					int result2;
					if (Int32.TryParse(tb_StopCount.Text, out result2)
						&& result2 > 0)
					{
						int result = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.
						tb_StopFinish.Text = (result + result2 - 1).ToString();
					}
				}
				else if (rb == rb_StartInsert)
				{
					tb_StartAdd   .Enabled = !rb.Checked;
					tb_StartInsert.Enabled =  rb.Checked;

					tb_StartInsert.BackColor = Color.FloralWhite;

					int result2;
					if (Int32.TryParse(tb_StopCount.Text, out result2)
						&& result2 > 0)
					{
						int result;
						if (Int32.TryParse(tb_StartInsert.Text, out result))
							tb_StopFinish.Text = (result + result2 - 1).ToString();
					}
				}
				else if (rb == rb_StopFinish)
				{
					if (tb_StopFinish.Enabled = rb.Checked)
					{
						tb_StopFinish.BackColor = Color.FloralWhite;
						tb_StopCount .BackColor = Color.WhiteSmoke;
					}
					else
					{
						tb_StopFinish.BackColor = Color.WhiteSmoke;
						tb_StopCount .BackColor = Color.FloralWhite;
					}
					tb_StopCount.Enabled = !rb.Checked;
				}
				else //if (rb == rb_StopCount)
				{
					if (tb_StopCount.Enabled = rb.Checked)
					{
						tb_StopFinish.BackColor = Color.WhiteSmoke;
						tb_StopCount .BackColor = Color.FloralWhite;
					}
					else
					{
						tb_StopFinish.BackColor = Color.FloralWhite;
						tb_StopCount .BackColor = Color.WhiteSmoke;
					}
					tb_StopFinish.Enabled = !rb.Checked;
				}
			}
		}

		void textchanged(object sender, EventArgs e)
		{
			if (!_init)
			{
				var tb = sender as TextBox;

				int result;
				if (Int32.TryParse(tb.Text, out result)
					&& (result > 0 || (tb != tb_StopCount && result > -1)))
				{
					int result2;
					if (tb == tb_StartInsert) // result
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
					else if (tb == tb_StopFinish) // result
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
						else //if (rb_StartInsert.Checked)
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
					else //if (tb == tb_StopCount) // result
					{
						tb_StopCount.ForeColor = SystemColors.WindowText;
						la_StopCount.ForeColor = SystemColors.ControlText;

						if (rb_StartAdd.Checked)
						{
							result2 = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.
							tb_StopFinish.Text = (result + result2 - 1).ToString();
						}
						else //if (rb_StartInsert.Checked)
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
					else //if (tb == tb_StopCount)
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
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void click_Ok(object sender, EventArgs e)
		{
			_cancel = false;

			int result;
			if (rb_StartAdd.Checked)
			{
				_f._startCr = Int32.Parse(tb_StartAdd.Text); // readonly - shall be valid.

				if (rb_StopFinish.Checked)
				{
					if (Int32.TryParse(tb_StopFinish.Text, out result))
					{
						_f._lengthCr = result - _f._startCr + 1;
					}
					else
						_cancel = true;
				}
				else //if (rb_StopCount.Checked)
				{
					if (Int32.TryParse(tb_StopCount.Text, out result))
					{
						_f._lengthCr = result;
					}
					else
						_cancel = true;
				}
			}
			else //if (rb_StartInsert.Checked)
			{
				if (Int32.TryParse(tb_StartInsert.Text, out result))
				{
					_f._startCr = result;

					if (rb_StopFinish.Checked)
					{
						if (Int32.TryParse(tb_StopFinish.Text, out result))
						{
							_f._lengthCr = result - _f._startCr + 1;
						}
						else
							_cancel = true;
					}
					else //if (rb_StopCount.Checked)
					{
						if (Int32.TryParse(tb_StopCount.Text, out result))
						{
							_f._lengthCr = result;
						}
						else
							_cancel = true;
					}
				}
				else
					_cancel = true;
			}

			if (_cancel
				|| _f._startCr < 0
				|| _f._startCr > YataForm.Table.RowCount
				|| _f._lengthCr < 1)
			{
				_cancel = true;
				MessageBox.Show(
							GetDarthQuote(),
							" Error",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error,
							MessageBoxDefaultButton.Button1,
							0);
			}
			else
				_f._fillCr = cb_Fill.Checked;
		}
		#endregion Events


		#region Methods (static)
		/// <summary>
		/// Gets a Darth Vader quote.
		/// </summary>
		/// <returns></returns>
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
