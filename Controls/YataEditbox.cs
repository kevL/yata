using System;
using System.Windows.Forms;


namespace yata
{
	sealed class YataEditbox
		: TextBox
	{
		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataEditbox()
		{
			Visible     = false;
			BackColor   = Colors.Editor;
			BorderStyle = BorderStyle.None;
			WordWrap    = false;
			Margin      = new Padding(0);
		}
		#endregion cTor


		#region Methods (override)
		/// <summary>
		/// Disallows <c>[Up]</c>, <c>[Down]</c>, <c>[PageUp]</c>, and
		/// <c>[PageDown]</c> for keyboard input on this <c>YataEditbox</c>.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks><c>[Up]</c>, <c>[Down]</c>, <c>[PageUp]</c>, and
		/// <c>[PageDown]</c> shall be used for Tabfastedit by
		/// <c><see cref="YataGrid._editor">YataGrid._editor</see></c> and/or
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c>.</remarks>
		protected override bool IsInputKey(Keys keyData)
		{
			logfile.Log("YataEditbox.IsInputKey() keyData= " + keyData);

			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.PageUp:
				case Keys.PageDown:
					logfile.Log(". YataEditbox.IsInputKey force FALSE");
					return false;
			}

			bool ret = base.IsInputKey(keyData);
			logfile.Log(". YataEditbox.IsInputKey ret= " + ret);
			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			logfile.Log("YataEditbox.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			logfile.Log(". YataEditbox.ProcessDialogKey ret= " + ret);
			return ret;
		}
		#endregion Methods (override)
	}
}
