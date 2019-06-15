using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	sealed partial class FileWatcherDialog
		:
			Form
	{
		#region Fields (static)
		internal const int FILE_DEL = 0; // file's not there, Jim.
		internal const int FILE_WSC = 1; // file's writestamp changed
		#endregion Fields (static)


		#region Fields
		YataGrid _grid;

		int _fwdType;
		#endregion Fields


		#region cTor
		internal FileWatcherDialog(
				YataGrid grid,
				int fwdType)
		{
			InitializeComponent();

			_grid = grid;
			Font = YataForm.CreateFont(_grid.Font);

			var f = _grid._f;
			for (int i = 0; i != f.Tabs.TabCount; ++i)
			{
				TabPage tab = f.Tabs.TabPages[i];
				if ((YataGrid)tab.Tag == _grid)
				{
					f.Tabs.SelectedIndex = i;
					break;
				}
			}

			tb_Pfe.Text = _grid.Fullpath;

			Width = TextRenderer.MeasureText(tb_Pfe.Text, Font).Width + 15;

			string text = String.Empty;
			switch (_fwdType = fwdType)
			{
				case FILE_DEL:
					text = "The file cannot be found on disk.";
					btn_Action.Text = "Resave file";
					break;

				case FILE_WSC:
					text = "The file on disk has changed.";
					btn_Action.Text = "Reload file";
					break;
			}

			if (_grid.Readonly)
				text += " CANCEL DISABLES THE READONLY FLAG.";

			lbl_Info.Text = text;

			btn_Action.Select();
		}
		#endregion cTor


		#region Events (override)
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			tb_Pfe.SelectionStart = 0;
			tb_Pfe.SelectionStart = tb_Pfe.Text.Length;
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			switch (DialogResult)
			{
				default:
				case DialogResult.Cancel:	// btn_Cancel
					_grid.Readonly = false;
					_grid.Changed = true;
					break;

				case DialogResult.Abort:	// btn_Close2da
					_grid.Changed = false;
					_grid._f.fileclick_CloseTab(null, EventArgs.Empty);
					break;

				case DialogResult.Yes:		// btn_Action
					_grid.Changed = false;

					switch (_fwdType)
					{
						case FILE_DEL:
							_grid._f.fileclick_Save(null, EventArgs.Empty);
							break;

						case FILE_WSC:
							_grid._f.fileclick_Reload(null, EventArgs.Empty);
							break;
					}
					break;
			}

			base.OnFormClosing(e);
		}
		#endregion Events (override)
	}
}
