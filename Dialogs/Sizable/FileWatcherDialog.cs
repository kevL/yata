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
		#region Enums
		internal enum Fwd
		{
			FileDeleted,	// file's not there, Jim.
			FileChanged		// file's writestamp changed
		}
		#endregion Enums


		#region Fields (static)
		const int WIDTH_Min = 363;

		const string FILE_Del = "File deleted from hardrive.";
		const string FILE_Wsc = "Writestamp changed.";
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// A <c><see cref="YataGrid"/></c> whose 2da-file is being watched.
		/// </summary>
		YataGrid _grid;

		/// <summary>
		/// Used to deter this <c>FileWatcherDialog's</c> current
		/// <c><see cref="bu_Action"/></c> operation.
		/// <list type="bullet">
		/// <item><c><see cref="Fwd.FileDeleted">Fwd.FileDeleted</see></c> -
		/// <c><see cref="YataForm.fileclick_Save()">YataForm.fileclick_Save()</see></c></item>
		/// <item><c><see cref="Fwd.FileChanged">Fwd.FileChanged</see></c> -
		/// <c><see cref="YataForm.fileclick_Reload()">YataForm.fileclick_Reload()</see></c></item>
		/// </list>
		/// </summary>
		internal Fwd _fwdType;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Instantiates this <c>FileWatcherDialog</c>.
		/// </summary>
		/// <param name="grid">the <c><see cref="YataGrid"/></c> being watched</param>
		/// <param name="fwdType"><c><see cref="Fwd"/></c>
		/// <list type="bullet">
		/// <item><c><see cref="Fwd.FileDeleted">Fwd.FileDeleted</see></c></item>
		/// <item><c><see cref="Fwd.FileChanged">Fwd.FileChanged</see></c></item>
		/// </list></param>
		internal FileWatcherDialog(
				YataGrid grid,
				Fwd fwdType)
		{
			_f = (_grid = grid)._f; // 'YataDialog._f' is used only to set UI parameters.

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
				case Fwd.FileDeleted:
					text           = FILE_Del;
					bu_Action.Text = "Resave";
					break;

				case Fwd.FileChanged:
					text           = FILE_Wsc;
					bu_Action.Text = "Reload";
					break;
			}

			if (_grid.Readonly)
				text += " CANCEL DISABLES THE READONLY FLAG.";

			la_Info.Text = text;
			int wInfo = YataGraphics.MeasureWidth(la_Info.Text, la_Info.Font);

			tb_Pfe.Text = _grid.Fullpath;
			int wPfe = YataGraphics.MeasureWidth(tb_Pfe.Text, tb_Pfe.Font);

			ClientSize = new Size(Math.Max(wInfo, wPfe) + 20, ClientSize.Height);

			MinimumSize = new Size(Math.Max(WIDTH_Min, Width), Height);
			MaximumSize = new Size(Int32.MaxValue,             Height);


			bu_Action.Select();
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
				default: // case DialogResult.Cancel	// bu_Cancel
					_grid.Readonly = false;
					_grid.Changed  = true;
					break;

				case DialogResult.Abort:				// bu_Close2da
					_grid.Changed = false;				// <- bypass Close warn
					_grid._f.fileclick_ClosePage(null, EventArgs.Empty);
					break;

				case DialogResult.Yes:					// bu_Action
					_grid.Changed = false;				// <- bypass Close warn

					switch (_fwdType)
					{
						case Fwd.FileDeleted:
							_grid._f.fileclick_Save(null, EventArgs.Empty);
							break;

						case Fwd.FileChanged:
							_grid._f.fileclick_Reload(null, EventArgs.Empty);
							break;
					}
					break;
			}

			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


		#region Methods
		/// <summary>
		/// Sets <c><see cref="_fwdType"/></c> and <c>Text</c> for
		/// <c><see cref="la_Info"/></c> and <c><see cref="bu_Action"/></c>
		/// based on the state of the 2da-file on the hardrive.
		/// </summary>
		internal void SetAction(Fwd fwdType)
		{
			switch (_fwdType = fwdType)
			{
				case Fwd.FileDeleted:
					la_Info  .Text = FILE_Del;
					bu_Action.Text = "Resave";
					break;

				case Fwd.FileChanged:
					la_Info  .Text = FILE_Wsc;
					bu_Action.Text = "Reload";
					break;
			}
		}
		#endregion Methods
	}
}
