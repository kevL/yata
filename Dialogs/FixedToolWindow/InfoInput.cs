using System;
using System.Windows.Forms;


namespace yata
{
	class InfoInput
		: Form
	{
		#region Fields
		protected Yata _f;

		internal Cell _cell; // can't be protected unless Cell is public

		/// <summary>
		/// <c>true</c> bypasses the <c>CheckedChanged</c> handler for
		/// <c>CheckBoxes</c> and the <c>SelectedIndexChanged</c> handler for
		/// the <c>ComboBox</c>.
		/// </summary>
		/// <remarks>Initialization will configure this dialog without invoking
		/// the handlers.</remarks>
		protected bool _init;

		protected CheckBox _cb;
		#endregion Fields


		#region Methods
		/// <summary>
		/// Selects an entry in the <c>ComboBox</c> and preps the int-vals in
		/// Yata to deal with user-input.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="co_Val"></param>
		/// <param name="bu_Clear"></param>
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

				if (val == gs.Stars) _f.int0 = Yata.II_ASSIGN_STARS;
				else                 _f.int0 = Yata.II_INIT_INVALID;

				_f.int1 = Yata.II_ASSIGN_STARS;

				co_Val.SelectedIndex = co_Val.Items.Count - 1;
			}
		}


		/// <summary>
		/// Clears all <c>CheckBoxes</c> except the current <c>CheckBox</c>
		/// <c><see cref="_cb"/></c> (if valid).
		/// </summary>
		/// <remarks>Set <c>(_cb = null)</c> to clear all <c>Checkboxes</c>.</remarks>
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
		/// Closes the dialog on [Esc].
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
