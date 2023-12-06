using System;
using System.Windows.Forms;


namespace yata
{
	sealed partial class Yata
	{
		#region Handlers (clipboard)
		/// <summary>
		/// Outputs the current contents of <c><see cref="_copyr"/></c> to the
		/// Windows clipboard.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ClipExport"/></c></param>
		/// <param name="e"></param>
		void clipclick_ExportCopy(object sender, EventArgs e)
		{
			string clip = String.Empty;
			for (int i = 0; i != _copyr.Count; ++i)
			{
				for (int j = 0; j != _copyr[i].Length; ++j)
				{
					if (j != 0) clip += gs.Space;
					clip += _copyr[i][j];
				}

				if (i != _copyr.Count - 1)
					clip += Environment.NewLine;
			}
			ClipboardService.SetText(clip);

			if (_fclip != null)
				_fclip.click_Get(sender, e);
		}

		/// <summary>
		/// Imports the current contents of the Windows clipboard to
		/// <c><see cref="_copyr"/></c> and enables
		/// <c><see cref="it_PasteRangeInsert"/></c> and
		/// <c><see cref="it_PasteRangeOverwrite"/></c> and
		/// <c><see cref="it_ClipExport"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ClipImport"/></c></param>
		/// <param name="e"></param>
		void clipclick_ImportCopy(object sender, EventArgs e)
		{
			_copyr.Clear();

			string clip = ClipboardService.GetText();
			if (clip.Length != 0)
			{
				string[] lines = clip.Split(new[]{ Environment.NewLine }, StringSplitOptions.None);
				string line;

				for (int i = 0; i != lines.Length; ++i)
				if ((line = lines[i].Trim()).Length != 0)
				{
					string[] fields = YataGrid.ParseTableRow(line);
					_copyr.Add(fields);
				}

				if (_fclip != null)
					_fclip.SetRowsBufferText();

				it_PasteRangeInsert   .Enabled = Table != null && !Table.Readonly;
				it_PasteRangeOverwrite.Enabled = Table != null && !Table.Readonly && Table.getSelectedRow() != -1;
				it_ClipExport         .Enabled = true;
			}
		}

		/// <summary>
		/// Displays contents of the Windows clipboard (if text) in an editable
		/// input/output dialog.
		/// </summary>
		/// <param name="sender"><c><see cref="it_OpenClipEditor"/></c></param>
		/// <param name="e"></param>
		void clipclick_ViewClipboard(object sender, EventArgs e)
		{
			if (_fclip == null)
			{
				_fclip = new ClipboardEditor(this);
				it_OpenClipEditor.Checked = true;
			}
			else
			{
				if (_fclip.WindowState == FormWindowState.Minimized)
				{
					if (_fclip.Maximized)
						_fclip.WindowState = FormWindowState.Maximized;
					else
						_fclip.WindowState = FormWindowState.Normal;
				}
				_fclip.BringToFront();
			}
		}
		#endregion Handlers (clipboard)


		#region Methods (clipboard)
		/// <summary>
		/// Clears the check on <c><see cref="it_OpenClipEditor"/></c> and nulls
		/// <c><see cref="_fclip"/></c> when the clipboard-dialog closes.
		/// </summary>
		internal void CloseClipEditor()
		{
			_fclip = null;
			it_OpenClipEditor.Checked = false;
		}
		#endregion Methods (clipboard)
	}
}
