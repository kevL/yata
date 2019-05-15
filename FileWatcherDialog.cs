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
				int fwdType,
				string pfe)
		{
			InitializeComponent();

			_grid = grid;
			Font = _grid.Font;

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

			tb_Pfe.Text = pfe;

			ClientSize = new Size(TextRenderer.MeasureText(pfe, Font).Width + 15,
								  ClientSize.Height);

			switch (_fwdType = fwdType)
			{
				case FILE_DEL:
					lbl_Info  .Text = "The file cannot be found on disk.";
					btn_Action.Text = "Resave file";
					break;

				case FILE_WSC:
					lbl_Info  .Text = "The file on disk has changed.";
					btn_Action.Text = "Reload file";
					break;
			}
		}
		#endregion cTor


		#region Events (override)
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			tb_Pfe.SelectionStart = 0;
			tb_Pfe.SelectionStart = tb_Pfe.Text.Length;
		}
		#endregion Events (override)


		#region Events
		void click_Cancel(object sender, EventArgs e)
		{
			_grid.Changed = true;

			// DialogResult.Cancel
		}

		void click_Close2da(object sender, EventArgs e)
		{
			_grid.Changed = false;
			_grid._f.fileclick_CloseTab(sender, e);

			// DialogResult.Abort
		}

		void click_Action(object sender, EventArgs e)
		{
			_grid.Changed = false;

			switch (_fwdType)
			{
				case FILE_DEL:
					_grid._f.fileclick_Save(sender, e);
					break;

				case FILE_WSC:
					_grid._f.fileclick_Reload(sender, e);
					break;
			}

			// DialogResult.Yes
		}
		#endregion Events
	}
}
