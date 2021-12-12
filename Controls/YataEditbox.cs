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
		/// Disallows <c>[Up]</c> and <c>[Down]</c> for keyboard input on this
		/// <c>YataEditbox</c>.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		/// <remarks><c>[Up]</c> and <c>[Down]</c> shall be used for
		/// Tabfastedit by
		/// <c><see cref="YataGrid._editor">YataGrid._editor</see></c> and
		/// <c><see cref="Propanel._editor">Propanel._editor</see></c>.</remarks>
		protected override bool IsInputKey(Keys keyData)
		{
			//logfile.Log("YataEditbox.IsInputKey() keyData= " + keyData);

			bool ret;
			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
					ret = false;
					break;

				default:
					ret = base.IsInputKey(keyData);
					break;
			}

			//logfile.Log(". " + ret);
			return ret;
		}

//		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
//		{
//			logfile.Log("YataEditbox.ProcessCmdKey() keyData= " + keyData);
//			bool ret = base.ProcessCmdKey(ref msg, keyData);
//			logfile.Log(". " + ret);
//			return ret;
//		}
//
//		protected override bool IsInputChar(char charCode)
//		{
//			logfile.Log("YataEditbox.IsInputChar() charCode= " + charCode);
//			bool ret = base.IsInputChar(charCode);
//			logfile.Log(". " + ret);
//			return ret;
//		}
//
//		protected override bool ProcessDialogChar(char charCode)
//		{
//			logfile.Log("YataEditbox.ProcessDialogChar() charCode= " + charCode);
//			bool ret = base.ProcessDialogChar(charCode);
//			logfile.Log(". " + ret);
//			return ret;
//		}
//
//		protected override bool ProcessDialogKey(Keys keyData)
//		{
//			logfile.Log("YataEditbox.ProcessDialogKey() keyData= " + keyData);
//			bool ret = base.ProcessDialogKey(keyData);
//			logfile.Log(". " + ret);
//			return ret;
//		}
		#endregion Methods (override)
	}
}
