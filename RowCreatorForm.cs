using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class RowCreatorForm
		:
			Form
	{
		#region Fields (static)
		static bool _pad    = true;
		static bool _length = true;

		static Random _rand = new Random();
		#endregion Fields (static)


		#region Fields
		YataForm _f;
		bool _error;
		#endregion Fields


		#region cTor
		internal RowCreatorForm(YataForm f)
		{
			InitializeComponent();

			_f = f;

			tb_Pad.Text = YataForm.Table.Rows.Count.ToString();

			int r = YataForm.Table.getSelectedRow() + 1;

			tb_Start .Text =
			tb_Stop  .Text = r.ToString();
			tb_Length.Text = "1";

			cb_Pad   .Checked = _pad;
			rb_Length.Checked =  _length;
			rb_Stop  .Checked = !_length;
		}
		#endregion cTor


		#region Events (override)
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!(e.Cancel = _error))
			{
				_pad    = cb_Pad   .Checked;
				_length = rb_Length.Checked;
			}
			base.OnFormClosing(e);
		}
		#endregion Events (override)


		#region Events
		void click_Cancel(object sender, EventArgs e)
		{
			_error = false;
			DialogResult = DialogResult.Cancel;
		}

		void click_Ok(object sender, EventArgs e)
		{
			_error = true;

			int result;

			if (cb_Pad.Checked)
			{
				if (Int32.TryParse(tb_Pad.Text, out result))
				{
					_error = false;
					_f._createstart  = YataForm.Table.RowCount;
					_f._createlength = result - _f._createstart + 1;
				}
			}
			else
			{
				if (Int32.TryParse(tb_Start.Text, out result))
				{
					_error = false;
					_f._createstart = result;
				}

				if (rb_Length.Checked)
				{
					if (Int32.TryParse(tb_Length.Text, out result))
					{
						_error = false;
						_f._createlength = result;
					}
				}
				else if (Int32.TryParse(tb_Stop.Text, out result))
				{
					_error = false;
					_f._createlength = result - _f._createstart + 1;
				}
			}

			if (_error
				|| _f._createstart < 0
				|| _f._createstart > YataForm.Table.RowCount
				|| _f._createlength < 1)
			{
				DialogResult = DialogResult.Cancel; // safety.
				ShowError();
			}
			else
				DialogResult = DialogResult.OK;
		}


		void checkchanged_Pad(object sender, EventArgs e)
		{
			tb_Pad   .Enabled =  cb_Pad.Checked;
			gb_Insert.Enabled = !cb_Pad.Checked;
		}


		void checkchanged_Insert(object sender, EventArgs e)
		{
			tb_Stop  .Enabled =  rb_Stop.Checked;
			tb_Length.Enabled = !rb_Stop.Checked;
		}
		#endregion Events


		#region Methods
		void ShowError()
		{
			_error = true;

			MessageBox.Show(
						GetDarthQuote(),
						" Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1,
						0);
		}


		/// <summary>
		/// Gets a Darth Vader quote.
		/// </summary>
		/// <returns></returns>
		string GetDarthQuote()
		{
			string quote = String.Empty;

			switch (_rand.Next(37))
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
			}
			return quote + Environment.NewLine + Environment.NewLine
				 + "- Darth Vader";
		}
		#endregion Methods
	}
}
