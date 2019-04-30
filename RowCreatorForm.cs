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
			_error = false;

			int result;

			if (cb_Pad.Checked)
			{
				if (Int32.TryParse(tb_Pad.Text, out result))
				{
					_f._start  = YataForm.Table.Rows.Count;
					_f._length = result - _f._start + 1;
				}
				else
					_error = true;
			}
			else
			{
				if (Int32.TryParse(tb_Start.Text, out result))
				{
					_f._start = result;
				}
				else
					_error = true;

				if (rb_Length.Checked)
				{
					if (Int32.TryParse(tb_Length.Text, out result))
					{
						_f._length = result;
					}
					else
						_error = true;
				}
				else
				{
					if (Int32.TryParse(tb_Stop.Text, out result))
					{
						_f._length = result - _f._start + 1;
					}
					else
						_error = true;
				}
			}

			if (_error
				|| _f._start < 0
				|| _f._start > YataForm.Table.Rows.Count
				|| _f._length < 1)
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
						"you done wrong",
						" Error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error,
						MessageBoxDefaultButton.Button1,
						0);
		}
		#endregion Methods
	}
}
