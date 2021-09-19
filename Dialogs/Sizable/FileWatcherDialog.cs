using System;
using System.Drawing;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A dialog that asks user what to do when a loaded 2da-file is changed or
	/// deleted from the hardrive.
	/// </summary>
	sealed partial class FileWatcherDialog
		: YataDialog
	{
		#region Fields (static)
		internal const int FILE_DEL = 0; // file's not there, Jim.
		internal const int FILE_WSC = 1; // file's writestamp changed

		const int WIDTH_Min = 363;
		#endregion Fields (static)


		#region Fields
		YataGrid _grid;

		int _fwdType;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Instantiates this <c>FileWatcherDialog</c>.
		/// </summary>
		/// <param name="grid">a <c><see cref="YataGrid"/></c></param>
		/// <param name="fwdType">the <c>FileWatcherDialog</c> type:
		/// <c><see cref="FILE_DEL"/></c> or <c><see cref="FILE_WSC"/></c></param>
		internal FileWatcherDialog(
				YataGrid grid,
				int fwdType)
		{
			_grid = grid;
			_f = _grid._f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_NON);

			YataTabs tabs = _grid._f.Tabs;
			for (int i = 0; i != tabs.TabCount; ++i)
			{
				if (tabs.TabPages[i].Tag as YataGrid == _grid)
				{
					tabs.SelectedIndex = i;
					break;
				}
			}

			// TODO: what if user changes 2+ files on the hardrive ...


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
			int wInfo = YataGraphics.MeasureWidth(lbl_Info.Text, lbl_Info.Font);

			tb_Pfe.Text = _grid.Fullpath;
			int wPfe = YataGraphics.MeasureWidth(tb_Pfe.Text, tb_Pfe.Font);

			ClientSize = new Size(Math.Max(wInfo, wPfe) + 20, ClientSize.Height);

			MaximumSize = new Size(Int32.MaxValue,             Height);
			MinimumSize = new Size(Math.Max(WIDTH_Min, Width), Height);


			btn_Action.Select();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Resize</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			tb_Pfe.SelectionStart = 0;
			tb_Pfe.SelectionStart = tb_Pfe.Text.Length;
		}

		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			switch (DialogResult)
			{
				default:
//				case DialogResult.Cancel:	// btn_Cancel
					_grid.Readonly = false;
					_grid.Changed  = true;
					break;

				case DialogResult.Abort:	// btn_Close2da
					_grid.Changed = false;
					_grid._f.fileclick_ClosePage(null, EventArgs.Empty);
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
		#endregion Handlers (override)
	}
}
