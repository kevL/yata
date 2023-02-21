using System;
using System.Drawing;
using System.IO;
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
		/// <summary>
		/// Deters whether this <c>FileWatcherDialog</c> needs to deal with a
		/// 2da-file that was deleted or changed.
		/// </summary>
		internal enum Input
		{
			FileDeleted, // file's not there, Jim.
			FileChanged  // file's writestamp changed
		}

		/// <summary>
		/// The result that this <c>FileWatcherDialog</c> returns in
		/// <c><see cref="OnFormClosing()">OnFormClosing()</see></c>.
		/// </summary>
		internal enum Output
		{
			non,
			Cancel,
			Close2da,
			Resave,
			Reload
		}
		#endregion Enums


		#region Fields (static)
		const int WIDTH_Min = 363;

		const string FILE_Del = "File not found.";
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
		/// <item><c><see cref="Input.FileDeleted">Input.FileDeleted</see></c> -
		/// <c><see cref="Yata.fileclick_Save()">Yata.fileclick_Save()</see></c></item>
		/// <item><c><see cref="Input.FileChanged">Input.FileChanged</see></c> -
		/// <c><see cref="Yata.fileclick_Reload()">Yata.fileclick_Reload()</see></c></item>
		/// </list>
		/// </summary>
		Input _input;

		/// <summary>
		/// A <c>Timer</c> that enables the <c>Buttons</c>.
		/// </summary>
		/// <seealso cref="t1_OnTick()"><c>t1_OnTick()</c></seealso>
		Timer _t1 = new Timer();

		/// <summary>
		/// A <c>Timer</c> that watches for further 2da-file changes while this
		/// <c>FileWatcherDialog</c> is open.
		/// </summary>
		Timer _t2 = new Timer();
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Instantiates this <c>FileWatcherDialog</c>.
		/// </summary>
		/// <param name="grid">the <c><see cref="YataGrid"/></c> being watched</param>
		/// <param name="input"><c><see cref="Input"/></c>
		/// <list type="bullet">
		/// <item><c><see cref="Input.FileDeleted">Input.FileDeleted</see></c></item>
		/// <item><c><see cref="Input.FileChanged">Input.FileChanged</see></c></item>
		/// </list></param>
		internal FileWatcherDialog(
				YataGrid grid,
				Input input)
		{
			//logfile.Log("FileWatcherDialog.cTor()");
			_f = (_grid = grid)._f; // 'YataDialog._f' is used only to set UI parameters.

			InitializeComponent();
			Initialize(METRIC_NON);

			string text = String.Empty;
			switch (_input = input)
			{
				case Input.FileDeleted:
					text           = FILE_Del;
					bu_Action.Text = "Resave";
					break;

				case Input.FileChanged:
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


			_t1.Tick += t1_OnTick;
			_t1.Interval = 330; // buttons-enabled delay.
			_t1.Start();

			_t2.Tick += t2_OnTick;
			_t2.Interval = 300; // watch file period.
			_t2.Start();
		}

		/// <summary>
		/// .net Issue: if user happens to click the parent <c>Form</c> where
		/// one of the <c>Buttons</c> will appear this <c>FileWatcherDialog</c>
		/// will accept that as a valid click on the <c>Button</c>. Don't do
		/// that.
		/// </summary>
		/// <param name="sender"><c><see cref="_t1"/></c></param>
		/// <param name="e"></param>
		/// <remarks><c>Thread.Sleep()</c> doesn't work for this workaround.</remarks>
		void t1_OnTick(object sender, EventArgs e)
		{
			bu_Cancel  .Enabled =
			bu_Close2da.Enabled =
			bu_Action  .Enabled = true;

			_t1.Stop();
			_t1.Dispose();

			bu_Action.Select();
		}

		/// <summary>
		/// Watches the 2da-file on the hardrive for changes while this
		/// <c>FileWatcherDialog</c> is open and changes
		/// <c><see cref="_input"/></c> etc as appropriate.
		/// </summary>
		/// <param name="sender"><c><see cref="_t2"/></c></param>
		/// <param name="e"></param>
		void t2_OnTick(object sender, EventArgs e)
		{
			switch (_input)
			{
				case Input.FileChanged:
					if (!File.Exists(_grid.Fullpath))
					{
						_input = Input.FileDeleted;

						la_Info  .Text = FILE_Del;
						bu_Action.Text = "Resave";
					}
					break;

				case Input.FileDeleted:
					if (File.Exists(_grid.Fullpath))
					{
						if (File.GetLastWriteTime(_grid.Fullpath) != _grid.Lastwrite)
						{
							_input = Input.FileChanged;

							la_Info  .Text = FILE_Wsc;
							bu_Action.Text = "Reload";
						}
						else
							DialogResult = DialogResult.Ignore;
					}
					break;
			}
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Load</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			CenterToParent();
			base.OnLoad(EventArgs.Empty);
		}

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
			//logfile.Log("FileWatcherDialog.OnFormClosing()");

			_t2.Stop();
			_t2.Dispose();

			switch (DialogResult)
			{
//				case DialogResult.Ignore: break;				// t2_OnTick() - 2da-file was moved out and back with the same timestamp.

				case DialogResult.Cancel:						// bu_Cancel
					_grid._f._fileresult = Output.Cancel;
					break;

				case DialogResult.Abort:						// bu_Close2da
					_grid._f._fileresult = Output.Close2da;
					break;

				case DialogResult.Yes:							// bu_Action
					switch (_input)
					{
						case Input.FileDeleted:
							_grid._f._fileresult = Output.Resave;
							break;

						case Input.FileChanged:
							_grid._f._fileresult = Output.Reload;
							break;
					}
					break;
			}

			base.OnFormClosing(e);
		}
		#endregion Handlers (override)


/*		#region Handlers
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Do not use the <c>Button.DialogResult</c> properties of the
		/// buttons themselves to set a result and close the <c>Form</c>;
		/// closing the <c>Form</c> that way tends to leave the parent
		/// <c>Form</c> in a merely pseudo-active state: its titlebar is colored
		/// like an active <c>Form</c> should be, but none of its controls show
		/// focus when mouseovered. Such a pseudo-active <c>Form</c> needs to be
		/// clicked and user-focused before it becomes truly active.
		/// 
		/// 
		/// Unfortunately this just sloughs off the indeterminate state onto
		/// the <c><see cref="Infobox"/></c> if one appears to inform user of
		/// any positive Strict-checks ... but when I implement this pattern for
		/// the <c>Infobox</c> it doesn't work and the main <c>Form's</c>
		/// indeterminate state happens regardless.</remarks>
		void mousedown(object sender, MouseEventArgs e)
		{
			if      (sender == bu_Action)   DialogResult = DialogResult.Yes;
			else if (sender == bu_Close2da) DialogResult = DialogResult.Abort;
			else                            DialogResult = DialogResult.Cancel; // sender == bu_Cancel
		}
		#endregion Handlers */
	}
}
