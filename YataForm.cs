using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Yata ....
	/// </summary>
	/// <remarks>Public access is required for the pointers in
	/// <c><see cref="YataDialog"/></c>.</remarks>
	public sealed partial class YataForm
		: Form
	{
		#region Enumerators
		/// <summary>
		/// Defines field fill-types used by
		/// <c><see cref="RowCreatorDialog"/></c>.
		/// </summary>
		internal enum CrFillType
		{ Stars, Selected, Copied }
		#endregion Enumerators


		#region Triggers
		/// <summary>
		/// whatever. Don't beep at us.
		/// </summary>
		internal event DontBeepEventHandler DontBeepEvent;
		#endregion Triggers


		#region Fields (static)
		internal static YataForm that;

		const string TITLE    = " Yata";
		const string ASTERICS = " *";

		const string RECENTCFG = "recent.cfg";

		internal static string PfeLoad; // cl arg

		static Graphics graphics;

		const int FROZEN_COL_Id     = 0;
		const int FROZEN_COL_First  = 1;
		const int FROZEN_COL_Second = 2;
		#endregion Fields (static)


		#region Fields
		readonly PropanelBu bu_Propanel = new PropanelBu();

		/// <summary>
		/// The <c><see cref="SettingsEditor"/></c> dialog/editor.
		/// </summary>
		SettingsEditor _fsettings;

		/// <summary>
		/// The <c><see cref="ClipboardEditor"/></c> dialog/editor.
		/// </summary>
		internal ClipboardEditor _fclip;

		/// <summary>
		/// A 2d-array of <c>strings</c> used for copy/paste cell.
		/// </summary>
		/// <remarks>A cell's text shall never be <c>null</c> or blank therefore
		/// <c>_copytext</c> shall never be <c>null</c> or blank.</remarks>
		internal string[,] _copytext = {{ gs.Stars }};

		/// <summary>
		/// The count of rows in <c><see cref="_copytext"/></c>.
		/// </summary>
		internal int _copyvert;

		/// <summary>
		/// The count of cols in <c><see cref="_copytext"/></c>.
		/// </summary>
		internal int _copyhori;


		/// <summary>
		/// A <c>List</c> of <c>string[]</c> arrays used for copy/paste row(s).
		/// </summary>
		internal List<string[]> _copyr = new List<string[]>();

		/// <summary>
		/// A <c>List</c> of <c>strings</c> used for copy/paste col.
		/// </summary>
		internal List<string> _copyc = new List<string>();


		string _preset = String.Empty;
		string _lastSaveasDirectory;

		internal int _startCr, _lengthCr;
		internal CrFillType _fillCr;

		/// <summary>
		/// The <c><see cref="FontDialog"/></c> font-picker.
		/// </summary>
		FontDialog _ffont;

		Font FontDefault;
		internal Font FontAccent;

		/// <summary>
		/// <c>true</c> to prevent <c><see cref="Table"/></c> from being
		/// selected when the table scrolls.
		/// </summary>
		internal bool IsSearch;


		int _dontbeep; // directs keydown [Enter] to the appropriate funct: Goto or Search
		const int DONTBEEP_DEFAULT = 0;
		const int DONTBEEP_GOTO    = 1;
		const int DONTBEEP_SEARCH  = 2;


		/// <summary>
		/// preps the Search textbox to select all text
		/// </summary>
		bool _selectall_search;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		bool _isEditclick_search;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		internal bool IsTabbed_search;

		/// <summary>
		/// preps the Goto textbox to select all text
		/// </summary>
		bool _selectall_goto;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		bool _isEditclick_goto;

		/// <summary>
		/// I don't know exactly what this is doing or why it works but it does.
		/// It appears to prevent a click on an already selected
		/// <c>ToolStripTextBox</c> from reselecting all text ...
		/// </summary>
		internal bool IsTabbed_goto;


		internal DifferDialog _fdiffer;
		internal YataGrid _diff1, _diff2;

		/// <summary>
		/// Caches a fullpath when doing SaveAs.
		/// So that the Table's new path-variables don't get assigned unless the
		/// save is successful - ie. verify several conditions first.
		/// </summary>
		string _pfeT = String.Empty;

		/// <summary>
		/// A pointer to a <c><see cref="YataGrid"/></c> that shall be used
		/// during the save-routine. Is required because it can't be assumed
		/// that the current <c><see cref="Table"/></c> is the table being
		/// saved; that is, the SaveAll operation needs to cycle through all
		/// tables.
		/// </summary>
		/// <seealso cref="fileclick_SaveAll()"><c>fileclick_SaveAll()</c></seealso>
		YataGrid _table;

		/// <summary>
		/// A flag that prevents a Readonly warning/error from showing twice.
		/// </summary>
		bool _warned;

		/// <summary>
		/// Tracks the row-id during single-row edit operations via the context.
		/// </summary>
		int _r;


		/// <summary>
		/// String-input for InfoInputSpells or InfoInputFeat (re PathInfo).
		/// <c>str0</c> is the current value; <c>str1</c> will be the
		/// user-chosen value that's assigned on Accept.
		/// </summary>
		internal string str0, str1;

		/// <summary>
		/// Int-input for InfoInputSpells or InfoInputFeat (re PathInfo).
		/// <c>int0</c> is the current value; <c>int1</c> will be the
		/// user-chosen value that's assigned on Accept.
		/// </summary>
		internal int int0, int1;

		// NOTE: These are to initialize 'int0' and 'int1' and need to be
		// different to recognize that an invalid current value should be
		// changed to stars (iff the user accepts the dialog).
		internal const int II_INIT_INVALID = -2; // for 'int0'
		internal const int II_ASSIGN_STARS = -1; // for 'int1'


		/// <summary>
		/// Works in conjunction w/
		/// <c><see cref="YataGrid"></see>.OnResize()</c>.
		/// </summary>
		internal bool IsMin;

		List<string> Strrefheads = new List<string>();
		internal string _strref; // the strref assigned by 'TalkDialog'
		int _strInt;	// cache for cell-context's dropdown functs
		Cell _sel;		// cache for cell-context's dropdown functs

		int _track_x = -1; // tracks last mouseover coords ->
		int _track_y = -1;

		/// <summary>
		/// Hides any info that's currently displayed on the statusbar when the
		/// cursor leaves the table-area.
		/// </summary>
		/// <remarks>Maintain namespace to differentiate
		/// <c>System.Threading.Timer</c>. jic.</remarks>
		System.Windows.Forms.Timer _t1 = new System.Windows.Forms.Timer();

		/// <summary>
		/// A <c>bool</c> indicating that a
		/// <c><see cref="FileWatcherDialog"/></c> is already invoked so don't
		/// try to invoke another one.
		/// </summary>
		/// <remarks>Can also be used to bypass
		/// <c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c>
		/// when loading or creating a 2da-file or closing Yata etc.
		/// 
		/// 
		/// Set <c>true</c> by
		/// <list type="bullet">
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// <item><c><see cref="OnFormClosing()">OnFormClosing()</see></c></item>
		/// <item><c><see cref="CreatePage()">CreatePage()</see></c></item>
		/// <item><c><see cref="fileclick_Create()">fileclick_Create()</see></c></item>
		/// <item><c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// </list></remarks>
		bool _bypassVerifyFile;

		/// <summary>
		/// A result returned by
		/// <c><see cref="FileWatcherDialog"/>.OnFormClosing()</c>.
		/// </summary>
		internal FileWatcherDialog.FwdResult _fileresult;
		#endregion Fields


		#region Properties (static)
		/// <summary>
		/// There can be only 1 <c>Table</c>.
		/// </summary>
		internal static YataGrid Table
		{ get; private set; }
		#endregion Properties (static)


		#region Properties
		readonly YataTabs tabControl = new YataTabs();
		internal YataTabs Tabs
		{ get { return tabControl; } }

		internal bool IsSaveAll
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor. This is Yata.
		/// </summary>
		internal YataForm()
		{
			that = this;

			// init 'Tabs' ->
			Tabs.DrawItem             += tab_DrawItem;
			Tabs.SelectedIndexChanged += tab_SelectedIndexChanged;

			Controls.Add(Tabs);

			// init 'bu_Propanel' ->
			bu_Propanel.MouseDown += mousedown_buPropanel;
			bu_Propanel.MouseUp   += mouseup_buPropanel;

			Controls.Add(bu_Propanel);

			InitializeComponent();

			_bar.setYata(this);

			tb_Search.BackColor =
			tb_Goto  .BackColor = Color.GhostWhite;

			Tabs.MouseClick += click_Tabs;

//			DrawRegulator.SetDoubleBuffered(this);
			SetStyle(ControlStyles.OptimizedDoubleBuffer
				   | ControlStyles.AllPaintingInWmPaint
				   | ControlStyles.UserPaint
				   | ControlStyles.ResizeRedraw, true);

			// IMPORTANT: The Client-area apart from the Menubar and Statusbar
			// has both a TabControl and a solid-colored Panel that *overlay*
			// each other and fill the area. The Panel is on top and is used
			// only to color the Client-area (else TabControl is pure white) -
			// it is shown when there are no TabPages and hides when there are.
			// It also appears when loading a 2da in an attempt to hide
			// unsightly graphical glitches.
			//
			// TODO: Instead of using BringToFront() and SendToBack() to show or
			// hide the panel try using its Visible bool.


			YataGraphics.graphics = CreateGraphics();

			FontDefault = new Font("Georgia", 8F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
			Font = FontDefault.Clone() as Font;

			YataGraphics.hFontDefault = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font);

			Settings.ScanSettings(); // load the Optional settings file Settings.Cfg

			if (Settings._font != null)
				Font = Settings._font;
			else
				Settings._fontdialog = Settings.CreateDialogFont(Font);

			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));

			if (Settings._font2 != null)
			{
				// Relative Font-sizes (as defined in the Designers):
				//
				// _bar, statusbar, _contextTa, _contextRo, _contextCe = all unity.
				// rowit_Header     = +0.75
				// statbar_lblCords = -0.75
				// statbar_lblInfo  = +1.50

				_bar.Font.Dispose();
				_bar.Font = Settings._font2;

				statusbar.Font.Dispose();
				statusbar.Font = Settings._font2;

				statbar_lblCords.Font.Dispose();
				statbar_lblCords.Font = new Font(Settings._font2.FontFamily,
												 Settings._font2.SizeInPoints - 0.75f);

				statbar_lblInfo.Font.Dispose();
				statbar_lblInfo.Font = new Font(Settings._font2.FontFamily,
												Settings._font2.SizeInPoints + 1.5f);

				int hBar = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, statbar_lblInfo.Font) + 2;

				statusbar       .Height = (hBar + 5 < 22) ? 22 : hBar + 5;
				statbar_lblCords.Height =
				statbar_lblInfo .Height = (hBar     < 17) ? 17 : hBar;

				statbar_lblCords.Width = YataGraphics.MeasureWidth(YataGraphics.WIDTH_CORDS, statbar_lblCords.Font) + 20;


				rowit_Header.Font.Dispose();
				rowit_Header.Font = new Font(Settings._font2.FontFamily,
											 Settings._font2.SizeInPoints + 0.75f,
											 getStyleAccented(Settings._font2.FontFamily));

				_contextTa.Font.Dispose();
				_contextTa.Font = Settings._font2;

				_contextRo.Font.Dispose();
				_contextRo.Font = Settings._font2;

				_contextCe.Font.Dispose();
				_contextCe.Font = Settings._font2;
			}

			int
				x = Settings._x,
				y = Settings._y,
				w = Settings._w,
				h = Settings._h;

			if (x != -1 || y != -1)
			{
				StartPosition = FormStartPosition.Manual;
				if (x == -1) x = Left;
				if (y == -1) y = Top;
				Location = new Point(x,y);
			}

			if (w == -1) w = Width;
			if (h == -1) h = Height;

			if (w != Width || h != Height)
				ClientSize = new Size(w,h);


			cb_SearchOption.Items.AddRange(new object[]
			{
				"find substring",
				"find wholeword"
			});
			cb_SearchOption.SelectedIndex = 0;


			YataGrid.SetStaticMetrics(this);

			bu_Propanel.Left = ClientSize.Width - bu_Propanel.Width + 1;
			bu_Propanel.Top = -1; // NOTE: This won't work in PP button's cTor. So do it here.


			if (Settings._recent != 0)
				CreateRecentsSubits(); // init recents before (potentially) loading a table from FileExplorer

			if (File.Exists(PfeLoad))
				CreatePage(PfeLoad); // start Yata and load file w/ file-association
			else
				Obfuscate();

			_t1.Interval = 223;
			_t1.Tick += t1_Tick;

			DontBeepEvent += HandleDontBeepEvent;

			TalkReader.LoadTalkingHeads(Strrefheads);
			TalkReader.Load(Settings._dialog,    it_PathTalkD);
			TalkReader.Load(Settings._dialogalt, it_PathTalkC, true);

			if (Settings._maximized)
				WindowState = FormWindowState.Maximized;

//			_bar.TabStop = true; // can be set in the designer <-
			// if focus is not forced here focus will be given to the File it.

//			_bar.Select(); // focuses the File it's container, the Menubar itself.
//			Tabs.Select(); // this happens by default if _bar's TabStop property is left False.
		}


		/// <summary>
		/// Initializes the recent-files list from entries in the user-file
		/// "recent.cfg".
		/// </summary>
		void CreateRecentsSubits()
		{
			string dir = Application.StartupPath;
			string pfe = Path.Combine(dir, RECENTCFG);
			if (File.Exists(pfe))
			{
				ToolStripItemCollection recents = it_Recent.DropDownItems;
				ToolStripItem it;

				string[] lines = File.ReadAllLines(pfe);
				foreach (string line in lines)
				{
					if (File.Exists(line))
					{
						it = new ToolStripMenuItem(line);
						it.Click += fileclick_Recent;
						recents.Add(it);

						if (recents.Count == Settings._recent)
							break;
					}
				}
			}
		}
		#endregion cTor


		/// <summary>
		/// Handles timer ticks - clears statusbar coordinates and path-info
		/// when the mouse-cursor leaves the grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void t1_Tick(object sender, EventArgs e)
		{
			if (_track_x != -1)
				Table.MouseLeaveTicker();
		}


		#region Methods (close)
		/// <summary>
		/// Checks if any currently opened tables have their
		/// <c><see cref="YataGrid.Changed">YataGrid.Changed</see></c> flag set.
		/// </summary>
		/// <param name="descriptor">"close" files or "quit" Yata</param>
		/// <param name="excludecurrent"><c>true</c> to exclude the current
		/// table - used by
		/// <c><see cref="tabclick_CloseAllOtherTabs()">tabclick_CloseAllOtherTabs()</see></c></param>
		/// <returns><c>true</c> if there are any changed tables and user
		/// chooses to cancel; <c>false</c> if there are no changed tables or
		/// user chooses to close/quit anyway</returns>
		bool CancelChangedTables(string descriptor, bool excludecurrent = false)
		{
			string tables = String.Empty;

			YataGrid table;
			foreach (TabPage page in Tabs.TabPages)
			{
				if ((table = page.Tag as YataGrid).Changed
					&& (!excludecurrent || table != Table))
				{
					if (tables.Length != 0) tables += Environment.NewLine;
					tables += Path.GetFileNameWithoutExtension(table.Fullpath).ToUpperInvariant();
				}
			}

			if (tables.Length != 0)
			{
				using (var ib = new Infobox(Infobox.Title_alert,
											"Data has changed. Okay to " + descriptor + " ...",
											tables,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					return ib.ShowDialog(this) == DialogResult.Cancel;
				}
			}
			return false;
		}
		#endregion Methods (close)


		#region Handlers (override)
		/// <summary>
		/// Overrides Yata's <c>Activated</c> eventhandler. Checks if the
		/// currently active table has been changed on the hardrive.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnActivated(EventArgs e)
		{
			logfile.Log("YataForm.OnActivated()");

			if (Table != null)
			{
				// NOTE: This could cause VerifyCurrentFileState() to run twice
				// if user activates Yata by clicking on a tab that changes the
				// currently selected tab - see: tab_SelectedIndexChanged()
				VerifyCurrentFileState();
			}
		}

		/// <summary>
		/// Overrides Yata's <c>Deactivate</c> eventhandler. Ensures that focus
		/// gets removed from the Menubar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDeactivate(EventArgs e)
		{
			logfile.Log("YataForm.OnDeactivate()");

			if (Table != null)
			{
				Table.editresultdefault();
				Table.Select();
			}
			else
				Tabs.Select();

			base.OnDeactivate(e);
		}

		/// <summary>
		/// Checks whether the 2da-file of the current
		/// <c><see cref="Table">Table's</see></c>
		/// <c><see cref="YataGrid.Fullpath">YataGrid.Fullpath</see></c> has
		/// been deleted or overwritten. Invokes a
		/// <c><see cref="FileWatcherDialog"/></c> if so.
		/// </summary>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item><c><see cref="OnActivated()">OnActivated()</see></c></item>
		/// <item><c><see cref="OnFormClosing()">OnFormClosing()</see></c> - cancelled</item>
		/// <item><c><see cref="tab_SelectedIndexChanged()">tab_SelectedIndexChanged()</see></c></item>
		/// <item><c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// </list></remarks>
		void VerifyCurrentFileState()
		{
			//logfile.Log("YataForm.VerifyCurrentFileState()");

			if (!_bypassVerifyFile)
			{
				_bypassVerifyFile = true; // bypass this funct when the FileWatcherDialog closes and this form's Activate event fires.

				_fileresult = FileWatcherDialog.FwdResult.non;

				if (!File.Exists(Table.Fullpath))
				{
					using (var fwd = new FileWatcherDialog(Table, FileWatcherDialog.FwdType.FileDeleted))
						fwd.ShowDialog(this);
				}
				else if (File.GetLastWriteTime(Table.Fullpath) != Table.Lastwrite)
				{
					using (var fwd = new FileWatcherDialog(Table, FileWatcherDialog.FwdType.FileChanged))
						fwd.ShowDialog(this);
				}

				//logfile.Log(". _fileresult= " + _fileresult);
				switch (_fileresult)
				{
//					case FileWatcherDialog.FwdResult.non: break;

					case FileWatcherDialog.FwdResult.Cancel:
						Table.Readonly = false;
						Table.Changed  = true;

						if (File.Exists(Table.Fullpath))
							Table.Lastwrite = File.GetLastWriteTime(Table.Fullpath);
						break;

					case FileWatcherDialog.FwdResult.Close2da:
						Table.Changed = false;					// <- bypass Close warn
						fileclick_ClosePage(null, EventArgs.Empty);
						break;

					case FileWatcherDialog.FwdResult.Resave:
						Table.Changed = false;					// <- bypass Close warn
						fileclick_Save(null, EventArgs.Empty);

						if (File.Exists(Table.Fullpath))
							Table.Lastwrite = File.GetLastWriteTime(Table.Fullpath);
						break;

					case FileWatcherDialog.FwdResult.Reload:
						Table.Changed = false;					// <- bypass Close warn
						fileclick_Reload(null, EventArgs.Empty);

						if (Table != null && File.Exists(Table.Fullpath)) // Table can fail on reload
							Table.Lastwrite = File.GetLastWriteTime(Table.Fullpath);
						break;
				}

				_bypassVerifyFile = false;
			}
			//else logfile.Log(". _bypassVerifyFile");
		}


		/// <summary>
		/// Overrides Yata's <c>FormClosing</c> handler. Requests
		/// user-confirmation if data has changed and writes a recent-files list
		/// if appropriate.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_bypassVerifyFile = true;

			if (Tabs.TabPages.Count != 0)
			{
				if (Tabs.TabPages.Count == 1)
				{
					if (e.Cancel = Table.Changed)
					{
						using (var ib = new Infobox(Infobox.Title_alert,
													"Data has changed. Okay to quit ...",
													null,
													InfoboxType.Warn,
													InfoboxButtons.CancelYes))
						{
							e.Cancel = ib.ShowDialog() == DialogResult.Cancel;
						}
					}
				}
				else
					e.Cancel = CancelChangedTables("quit");
			}

			if (e.Cancel)
			{
				_bypassVerifyFile = false;
				VerifyCurrentFileState();
			}
			else if (Settings._recent != 0)
			{
				int i = -1;
				var recents = new string[it_Recent.DropDownItems.Count];
				foreach (ToolStripItem recent in it_Recent.DropDownItems)
					recents[++i] = recent.Text;

				string pfe = Path.Combine(Application.StartupPath, RECENTCFG);
				try
				{
					File.WriteAllLines(pfe, recents);
				}
				catch (Exception ex)
				{
					using (var ib = new Infobox(Infobox.Title_excep,
												"Failed to write Recent.cfg to the application directory.",
												ex.ToString(),
												InfoboxType.Error))
					{
						ib.ShowDialog(this);
					}
				}
			}

			base.OnFormClosing(e);
		}


		/// <summary>
		/// Sends the <c>MouseWheel</c> event to the active
		/// <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (Table != null) Table.Scroll(e);
		}

		/// <summary>
		/// Sets <c><see cref="IsMin"/></c> true so that when the form is
		/// minimized then restored/maximized the ensure-displayed call(s) are
		/// bypassed by <c><see cref="YataGrid"/>.OnResize()</c> event(s).
		/// Because if the user wants to simply minimize the window temporarily
		/// to check something out in another app you don't want the view to be
		/// changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (Table != null)
			{
				Table.editresultdefault();
				Table.Select();
			}

			if (WindowState == FormWindowState.Minimized)
				IsMin = true;

			base.OnResize(e);
		}

#if DEBUG
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			if (gc.KeyLog && (e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataForm.OnPreviewKeyDown() e.KeyData= " + e.KeyData + " e.IsInputKey= " + e.IsInputKey);

			base.OnPreviewKeyDown(e);
		}
#endif

		/// <summary>
		/// Processes so-called command-keys.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
#if DEBUG
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataForm.ProcessCmdKey() keyData= " + keyData);
#endif
			switch (keyData)
			{
				case Keys.Menu | Keys.Alt:
					// NOTE: The Menubar container is by default TabStop=FALSE and ... (not) forced TRUE in YataForm.cTor <-
					// so it can never take focus - but its subcontrols are fucked re. "focus".
					// ... because they aren't actually 'Controls'.

					if (Table != null)
					{
#if DEBUG
						if (gc.KeyLog) logfile.Log(". select Table");
#endif
						// set '_editor.Visible' FALSE else its leave event
						// fires twice when it loses focus ->

						Table.editresultdefault();
						Table.Select();
					}
					else
					{
#if DEBUG
						if (gc.KeyLog) logfile.Log(". select Tabs");
#endif
						Tabs.Select();
					}
					return false; // do not return True. [Alt] needs to 'activate' the Menubar.
			}

			bool ret = base.ProcessCmdKey(ref msg, keyData);
#if DEBUG
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataForm.ProcessCmdKey ret= " + ret);
#endif
			return ret;
		}

#if DEBUG
		protected override bool IsInputKey(Keys keyData)
		{
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataForm.IsInputKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataForm.IsInputKey ret= " + ret);

			return ret;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log("YataForm.ProcessDialogKey() keyData= " + keyData);

			bool ret = base.ProcessDialogKey(keyData);
			if (gc.KeyLog && (keyData & ~gc.ControlShift) != 0)
				logfile.Log(". YataForm.ProcessDialogKey ret= " + ret);

			return ret;
		}
#endif

		/// <summary>
		/// Handles the <c>KeyDown</c> event at the form-level.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires the form's <c>KeyPreview</c> property flagged
		/// <c>true</c>.
		/// 
		/// 
		/// Fires repeatedly if a key is held depressed.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
#if DEBUG
			if (gc.KeyLog && (e.KeyData & ~gc.ControlShift) != 0)
				logfile.Log("YataForm.OnKeyDown() e.KeyData= " + e.KeyData);
#endif
			if (Table != null)
			{
				switch (e.KeyData)
				{
					case Keys.Enter: // do this here to get rid of the beep.
					case Keys.Shift | Keys.Enter:
#if DEBUG
						if (gc.KeyLog) logfile.Log(". Keys.Enter");
#endif
						if (tb_Search.Focused || cb_SearchOption.Focused)
						{
#if DEBUG
							if (gc.KeyLog)
							{
								if (e.KeyData == Keys.Enter) logfile.Log(". . Search forward");
								else                         logfile.Log(". . Search reverse");
							}
#endif
							_dontbeep = DONTBEEP_SEARCH;
						}
						else if (tb_Goto.Focused && e.KeyData == Keys.Enter)
						{
#if DEBUG
							if (gc.KeyLog) logfile.Log(". . Goto");
#endif
							_dontbeep = DONTBEEP_GOTO;
						}
						else
						{
#if DEBUG
							if (gc.KeyLog) logfile.Log(". . Search or Goto not focused");
#endif
							_dontbeep = DONTBEEP_DEFAULT;
						}

						if (_dontbeep != DONTBEEP_DEFAULT)
						{
							e.SuppressKeyPress = true;
							BeginInvoke(DontBeepEvent);
						}
						break;

					case Keys.Escape:
#if DEBUG
						if (gc.KeyLog) logfile.Log(". Keys.Escape");
#endif
						if (Tabs.Focused || bu_Propanel.Focused)	// btn -> jic. The Propanel button can become focused by
						{											// keyboard (I saw it happen once) but can't figure out how.
#if DEBUG															// NOTE: It wasn't actually focused, it was a graphical glitch.
							if (gc.KeyLog) logfile.Log(". . deselect Tabs -> select Grid");
#endif
							e.SuppressKeyPress = true;
							Table.Select();
						}
#if DEBUG
						else
							if (gc.KeyLog) logfile.Log(". . Tabs not focused");
#endif
						break;

					case Keys.Space:
#if DEBUG
						if (gc.KeyLog) logfile.Log(". Keys.Space");
#endif
						if (!Table._editor.Visible
							&& (Table.Propanel == null || !Table.Propanel._editor.Visible))
						{
#if DEBUG
							if (gc.KeyLog) logfile.Log(". . select first cell");
#endif
							e.SuppressKeyPress = true;
							Table.SelectFirstCell();
						}
#if DEBUG
						else
							if (gc.KeyLog) logfile.Log(". . an Editor is visible -> do not select first cell");
#endif
						break;

					case Keys.Control | Keys.Space:
#if DEBUG
						if (gc.KeyLog) logfile.Log(". Keys.Control | Keys.Space");
#endif
						if (!Table._editor.Visible
							&& (Table.Propanel == null || !Table.Propanel._editor.Visible))
						{
#if DEBUG
							if (gc.KeyLog) logfile.Log(". . select first row");
#endif
							e.SuppressKeyPress = true;
							Table.SelectFirstRow();
						}
#if DEBUG
						else
							if (gc.KeyLog) logfile.Log(". . an Editor is visible -> do not select first row");
#endif
						break;
				}


				// clear the col-width adjustor on '_panelCols' ->
				switch (e.KeyCode)
				{
					case Keys.Menu:			// Keys.Alt
					case Keys.ControlKey:	// Keys.Control
					case Keys.ShiftKey:		// Keys.Shift
						Cursor = Cursors.Default;
						Table._panelCols.IsCursorSplit = false;
						Table._panelCols.IsGrab = false;
						break;
				}
			}
			base.OnKeyDown(e);
		}

		/// <summary>
		/// Overrides the <c>KeyUp</c> eventhandler. Enables the col-width
		/// adjustor if appropriate.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (Table != null)
			{
				// enable the col-width adjustor on '_panelCols' ->
				switch (e.KeyCode)
				{
					case Keys.Menu:			// Keys.Alt
					case Keys.ControlKey:	// Keys.Control
					case Keys.ShiftKey:		// Keys.Shift
						if (ModifierKeys == Keys.None)
						{
							Point pos = Table._panelCols.PointToClient(Cursor.Position);
							if (Table._panelCols.GetSplitterCol(pos.X) != -1)
							{
								Cursor = Cursors.VSplit;
								Table._panelCols.IsCursorSplit = true;
							}
						}
						break;
				}
			}
			base.OnKeyUp(e);
		}

		/// <summary>
		/// Forwards a <c>KeyDown</c> <c>[Enter]</c> event to an appropriate
		/// funct.
		/// </summary>
		/// <remarks>Is basically just a convoluted handler for the
		/// <c><see cref="OnKeyDown()">OnKeyDown()</see></c> handler to stop the
		/// *beep* if <c>[Enter]</c> is keyed when a <c>TextBox</c> is focused.</remarks>
		void HandleDontBeepEvent()
		{
			switch (_dontbeep)
			{
				case DONTBEEP_SEARCH:
					Search_keyEnter();
					break;

				case DONTBEEP_GOTO:
					Table.doGoto(tb_Goto.Text, true);
					break;
			}
		}
		#endregion Handlers (override)


		#region Handlers (override -  Receive Message - PfeLoad arg)
		/// <summary>
		/// Disables message-blocking in Vista+ 64-bit systems.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs</remarks>
		protected override void OnLoad(EventArgs e)
		{
			GC.Collect(); // .net appears to load ~38mb of garbage at program start.
			GC.WaitForPendingFinalizers();

			var filter = new Crap.CHANGEFILTERSTRUCT();
			filter.size = (uint)Marshal.SizeOf(filter);
			filter.info = 0;
			if (!Crap.ChangeWindowMessageFilterEx(Handle,
												  Crap.WM_COPYDATA,
												  Crap.ChangeWindowMessageFilterExAction.Allow,
												  ref filter))
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"The MessageFilter could not be changed.",
											"LastWin32Error " + Marshal.GetLastWin32Error(),
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Receives data via WM_COPYDATA from other applications/processes.
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs</remarks>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == Crap.WM_COPYDATA)
			{
				var copyData = (Crap.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Crap.COPYDATASTRUCT));
				if ((int)copyData.dwData == Crap.CopyDataStructType) // extract the file-string ->
				{
					PfeLoad = Marshal.PtrToStringAnsi(copyData.lpData);
					if (File.Exists(PfeLoad))
						CreatePage(PfeLoad); // load file w/ file-association
				}
			}
			else
				base.WndProc(ref m);
		}
		#endregion Handlers (override -  Receive Message - PfeLoad arg)


		#region Methods (static)
		/// <summary>
		/// Gets a standard-ish <c>FontStyle</c> given a <c>FontFamily</c>.
		/// </summary>
		/// <param name="ff"><c>FontFamily</c></param>
		/// <returns><c>FontStyle</c></returns>
		internal static FontStyle getStyleStandard(FontFamily ff)
		{
			if (ff.IsStyleAvailable(FontStyle.Regular)) return FontStyle.Regular;
			if (ff.IsStyleAvailable(FontStyle.Italic))  return FontStyle.Italic;
			if (ff.IsStyleAvailable(FontStyle.Bold))    return FontStyle.Bold;

			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(style)) // determine first available style (any) of Family ->
					return style;
			}
			return FontStyle.Regular; // this ought never happen.
		}

		/// <summary>
		/// Gets an accented-ish <c>FontStyle</c> given a <c>FontFamily</c>.
		/// </summary>
		/// <param name="ff"><c>FontFamily</c></param>
		/// <returns><c>FontStyle</c></returns>
		static FontStyle getStyleAccented(FontFamily ff)
		{
			if (ff.IsStyleAvailable(FontStyle.Bold))      return FontStyle.Bold;
			if (ff.IsStyleAvailable(FontStyle.Underline)) return FontStyle.Underline;
			if (ff.IsStyleAvailable(FontStyle.Italic))    return FontStyle.Italic;

			foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(style)) // determine first available style (any) of Family ->
					return style;
			}
			return FontStyle.Regular; // this ought never happen.
		}
		#endregion Methods (static)


		#region Methods
		/// <summary>
		/// Obscures or unobscures the table behind a dedicated color-panel.
		/// Can be called before and after calibrating and drawing the table in
		/// order to hide unsightly .NET spaz-attacks (despite double-buffering
		/// etc).
		/// </summary>
		/// <param name="obscure"><c>true</c> to bring
		/// <c><see cref="panel_ColorFill"/></c> to front or <c>false</c> to
		/// send it to back</param>
		internal void Obfuscate(bool obscure = true)
		{
			if (obscure) panel_ColorFill.BringToFront();
			else         panel_ColorFill.SendToBack();
		}
		#endregion Methods


		#region Methods (create)
		/// <summary>
		/// Creates a tab-page and instantiates a table-grid for it.
		/// </summary>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read"><c>true</c> to create table as
		/// <c><see cref="YataGrid.Readonly">YataGrid.Readonly</see></c></param>
		void CreatePage(string pfe, bool read = false)
		{
			if (File.Exists(pfe)										// ~safety
				&& Path.GetFileNameWithoutExtension(pfe).Length != 0)	// what idjut would ... oh wait.
			{
				if (Settings._recent != 0)
				{
					ToolStripItemCollection recents = it_Recent.DropDownItems;
					ToolStripItem it;

					bool found = false;

					for (int i = 0; i != recents.Count; ++i)
					{
						if ((it = recents[i]).Text == pfe)
						{
							found = true;

							if (i != 0)
							{
								recents.Remove(it);
								recents.Insert(0, it);
							}
							break;
						}
					}

					if (!found)
					{
						it = new ToolStripMenuItem(pfe);
						it.Click += fileclick_Recent;
						recents.Insert(0, it);

						if (recents.Count > Settings._recent)
						{
							recents.Remove(it = recents[recents.Count - 1]);
							it.Dispose();
						}
					}
				}


				// check if 2da-file is already open ->
				for (int i = 0; i != Tabs.TabPages.Count; ++i)
				if ((Tabs.TabPages[i].Tag as YataGrid).Fullpath == pfe)
				{
					TopMost = true; // drag&drop from FileExplorer could leave the Infobox hidden behind other windows.
					TopMost = false;

					Tabs.SelectedIndex = i;

					if (!Settings._allowdupls) return;

					using (var ib = new Infobox(Infobox.Title_warni,
												"The 2da-file is already open. Do you want another instance ...",
												null,
												InfoboxType.Warn,
												InfoboxButtons.CancelYes))
					{
						if (ib.ShowDialog(this) == DialogResult.Cancel)
							return;
					}
					break;
				}


				Obfuscate();
//				Refresh();	// NOTE: If a table is already loaded the color-panel doesn't show
							// but a refresh turns the client-area gray at least instead of glitchy.
							// NOTE: It went away; the table-area turns gray.

				var table = new YataGrid(this, pfe, read);

				_bypassVerifyFile = true;

				int result = table.LoadTable();
				if (result != YataGrid.LOADRESULT_FALSE)
				{
					Table = table; // NOTE: Is done in tab_SelectedIndexChanged() also.

					DrawRegulator.SuspendDrawing(Table);

					var tab = new TabPage();
					Tabs.TabPages.Add(tab);

					tab.Tag = Table;

					tab.Text = Path.GetFileNameWithoutExtension(pfe);

					tab.Controls.Add(Table);
					Tabs.SelectedTab = tab;

					Table.Init(result == YataGrid.LOADRESULT_CHANGED);

					if (WindowState == FormWindowState.Minimized)
						WindowState  = FormWindowState.Normal;

					TopMost = true;
					TopMost = false;

					DrawRegulator.ResumeDrawing(Table);
				}
				else
				{
					YataGrid._init = false;
					table.Dispose();
				}

				_bypassVerifyFile = false;

				tab_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Sets the titlebar text to something reasonable.
		/// </summary>
		void SetTitlebarText()
		{
			string text = TITLE;

			if (Table != null)
			{
				string pfe = Table.Fullpath;
				text += " - " + Path.GetFileName(pfe);

				string dir = Path.GetDirectoryName(pfe);
				if (!String.IsNullOrEmpty(dir))
				{
					text += " - " + dir;
				}
			}
			Text = text;
		}
		#endregion Methods (create)


		#region Handlers (tabs)
		/// <summary>
		/// Handles tab-selection/deselection.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="Tabs"/></c></item>
		/// <item><c>null</c></item>
		/// </list>
		/// </param>
		/// <param name="e"></param>
		/// <remarks>This should be bypassed when a page other than tabid #0 is
		/// active and user closes all other tabs - if tabid #0 is already
		/// active the selected-id does not change.
		/// 
		/// 
		/// Invoked by
		/// <list type="bullet">
		/// <item>Tab click</item>
		/// <item><c><see cref="CreatePage()">CreatePage()</see></c></item>
		/// </list></remarks>
		void tab_SelectedIndexChanged(object sender, EventArgs e)
		{
			//logfile.Log("YataForm.tab_SelectedIndexChanged() id= " + Tabs.SelectedIndex);

			if (Tabs.SelectedIndex != -1)
			{
				Table = Tabs.SelectedTab.Tag as YataGrid; // <- very Important <--||

				// NOTE: Hiding a visible editor when user changes the tabpage
				// is usually handled by YataGrid's Leave event; but
				// [Ctrl+PageUp]/[Ctrl+PageDown] will change the tabpage without
				// firing the Leave event. So do this typically redundant check
				// ->
//				HideEditor();
				// NOTE: Appears to no longer be needed.

				Cell sel = Table.getSelectedCell();

				bu_Propanel           .Visible = true;
				it_MenuPaths          .Visible = Table.Info != YataGrid.InfoType.INFO_NONE;


				it_freeze1            .Checked = Table.FrozenCount == YataGrid.FreezeFirst;
				it_freeze2            .Checked = Table.FrozenCount == YataGrid.FreezeSecond;

				it_Readonly           .Checked = Table.Readonly;


				it_Reload             .Enabled = File.Exists(Table.Fullpath);
				it_Save               .Enabled = !Table.Readonly;
				it_SaveAll            .Enabled = AllowSaveAll();
				it_SaveAs             .Enabled =
				it_Readonly           .Enabled =
				it_Close              .Enabled =
				it_CloseAll           .Enabled = true;

				it_Undo               .Enabled = Table._ur.CanUndo;
				it_Redo               .Enabled = Table._ur.CanRedo;
				it_Searchnext         .Enabled =
				it_Searchprev         .Enabled = tb_Search.Text.Length != 0;
				it_GotoLoadchanged    .Enabled =
				it_GotoLoadchanged_pre.Enabled = Table.anyLoadchanged();
				it_Defaultval         .Enabled = true;
				it_Defaultclear       .Enabled = Table._defaultval.Length != 0;

				EnableCelleditOperations();
				EnableRoweditOperations();
				Enable2daOperations();


				if (Table.Propanel != null && Table.Propanel.Visible)
				{
					Table.Propanel.telemetric();
					if (sel != null)
						Table.Propanel.EnsureDisplayed(sel.x);
				}

				if (_fdiffer != null)
					_fdiffer.EnableGotoButton(true);

				SetTabSize();

				if (!_t1.Enabled) _t1.Enabled = true;

				Obfuscate(false);

				//logfile.Log(". call VerifyCurrentFileState()");
				VerifyCurrentFileState();
			}
			else
			{
				Obfuscate();

				_t1.Enabled = false;
				Table = null;

				// Visible ->

				bu_Propanel           .Visible =
				it_MenuPaths          .Visible =

				// Checked ->

				it_freeze1            .Checked =
				it_freeze2            .Checked =
				it_Propanel           .Checked =
				it_Readonly           .Checked =

				// Enabled ->

				it_Reload             .Enabled =
				it_Save               .Enabled =
				it_SaveAll            .Enabled =
				it_SaveAs             .Enabled =
				it_Readonly           .Enabled =
				it_Close              .Enabled =
				it_CloseAll           .Enabled =

				it_Undo               .Enabled =
				it_Redo               .Enabled =
				it_Searchnext         .Enabled =
				it_Searchprev         .Enabled =
				it_GotoLoadchanged    .Enabled =
				it_GotoLoadchanged_pre.Enabled =
				it_Defaultval         .Enabled =
				it_Defaultclear       .Enabled =

				it_DeselectCell       .Enabled =
				it_CutCell            .Enabled =
				it_CopyCell           .Enabled =
				it_PasteCell          .Enabled =
				it_DeleteCell         .Enabled =
				it_Lower              .Enabled =
				it_Upper              .Enabled =
				it_Apply              .Enabled =

				it_DeselectRows       .Enabled =
				it_CutRange           .Enabled =
				it_CopyRange          .Enabled =
				it_PasteRange         .Enabled =
				it_DeleteRange        .Enabled =
				it_CreateRows         .Enabled =

				it_OrderRows          .Enabled =
				it_CheckRows          .Enabled =
				it_ColorRows          .Enabled =
				it_AutoCols           .Enabled =
				it_freeze1            .Enabled =
				it_freeze2            .Enabled =
				it_Propanel           .Enabled =
				it_PropanelLoc        .Enabled =
				it_PropanelLoc_pre    .Enabled =
				it_ExternDiff         .Enabled = false;

				_fdiffer = null;
			}

			SetTitlebarText();
		}

		/// <summary>
		/// Draws the tab-text in Bold iff selected.
		/// </summary>
		/// <param name="sender"><c><see cref="Tabs"/></c></param>
		/// <param name="e"></param>
		void tab_DrawItem(object sender, DrawItemEventArgs e)
		{
			var tab = Tabs.TabPages[e.Index];

			int y; // vertical text-padding tweak

			FontStyle style;
			if (tab == Tabs.SelectedTab)
			{
				style = getStyleAccented(Font.FontFamily);
				y = 1;
			}
			else
			{
				style = getStyleStandard(Font.FontFamily);
				y = 2;
			}

			using (var font = new Font(Font.Name, Font.SizeInPoints - 0.75f, style))
			{
				// NOTE: MS doc for DrawText() says that using a Point doesn't work on Win2000 machines.
				int w = YataGraphics.MeasureWidth(tab.Text, font);
				var rect = e.Bounds;
				rect.X   = e.Bounds.X + (e.Bounds.Width - w) / 2;
				rect.Y   = e.Bounds.Y + y;

				graphics = e.Graphics;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				Color color;

				// NOTE: The Tag can be null when the tabcontrol needs to extend
				// down to create another row. Go figur.
				//
				// Fortunately the text of a table that is opened as readonly
				// will still appear in the TextReadonly color because .net
				// tends to be redundant and so this draw routine gets a second
				// call right away with its Tag/table valid.
				//
				// Note that this is one of those null-errors that the debugger
				// will slough off ....

				if (tab.Tag != null && (tab.Tag as YataGrid).Readonly)
					color = Colors.TextReadonly;
				else
					color = Colors.Text;

				TextRenderer.DrawText(graphics,
									  tab.Text,
									  font,
									  rect,
									  color,
									  YataGraphics.flags);

				if (y == 1 && Tabs.Focused)
				{
					y = rect.Y + rect.Height - 5;
					graphics.DrawLine(Pencils.LightLine,
									  rect.X,     y,
									  rect.X + w, y);
				}
			}
		}
		#endregion Handlers (tabs)


		#region Methods (tabs)
		/// <summary>
		/// Disposes a tab's <c><see cref="YataGrid"/></c> before the specified
		/// <c>TabPage</c> is removed from the <c>TabPageCollection</c>. Closes
		/// <c><see cref="_fdiffer"/></c> if it's no longer required and the
		/// <c>TabPage</c> is then <c>Disposed()</c>.
		/// </summary>
		/// <param name="tab">the <c>TabPage</c> with which to deal</param>
		void ClosePage(TabPage tab)
		{
			var table = tab.Tag as YataGrid;

			table.Dispose();

			if      (_diff1 == table) _diff1 = null;
			else if (_diff2 == table) _diff2 = null;

			if (_fdiffer != null && _diff1 == null && _diff2 == null)
				_fdiffer.Close();

			Tabs.TabPages.Remove(tab);
			tab.Dispose();

			YataGrid.metricStaticHeads(this);
		}

		/// <summary>
		/// Sets the width of the tabs on <c><see cref="YataTabs"/></c>.
		/// </summary>
		void SetTabSize()
		{
			YataGrid.BypassInitScroll = true; // ie. foff with your 50 bazillion behind-the-scene calls to OnResize().

			if (Tabs.TabCount != 0)
			{
				Size size;
				int w = 25, wT, h = 10, hT;
				for (int tab = 0; tab != Tabs.TabCount; ++tab)
				{
					size = YataGraphics.MeasureSize(Tabs.TabPages[tab].Text, FontAccent);
					if ((wT = size.Width) > w)
						w = wT;

					if ((hT = size.Height) > h)
						h = hT;
				}
				Tabs.ItemSize = new Size(w + 10, h + 4);
			}

			YataGrid.BypassInitScroll = false;
		}

		/// <summary>
		/// Sets the tabtext of a specified table.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Is called by
		/// <c><see cref="YataGrid.Changed">YataGrid.Changed</see></c> property.</remarks>
		internal void SetTabText(YataGrid table)
		{
			DrawRegulator.SuspendDrawing(this); // stop tab-flicker on Sort etc.

			string asterics = table.Changed ? ASTERICS : String.Empty;
			foreach (TabPage tab in Tabs.TabPages)
			{
				if (tab.Tag as YataGrid == table)
				{
					tab.Text = Path.GetFileNameWithoutExtension(table.Fullpath) + asterics;
					break;
				}
			}
			SetTabSize();

			DrawRegulator.ResumeDrawing(this);
		}

		/// <summary>
		/// Sets the tabtexts of all tables.
		/// </summary>
		/// <remarks>Called by
		/// <c><see cref="fileclick_SaveAll()">fileclick_SaveAll()</see></c>.</remarks>
		void SetAllTabTexts()
		{
			DrawRegulator.SuspendDrawing(this); // stop tab-flicker on Sort etc.

			YataGrid table;
			foreach (TabPage tab in Tabs.TabPages)
			{
				table = tab.Tag as YataGrid;
				tab.Text = Path.GetFileNameWithoutExtension(table.Fullpath)
						 + (table.Changed ? ASTERICS : String.Empty);
			}
			SetTabSize();

			DrawRegulator.ResumeDrawing(this);
		}
		#endregion Methods (tabs)


		#region Handlers (file)
		/// <summary>
		/// Handles opening the File menu along with the preset-dirs and
		/// recent-files subits.
		/// </summary>
		/// <param name="sender"><c><see cref="it_MenuFile"/></c></param>
		/// <param name="e"></param>
		void file_dropdownopening(object sender, EventArgs e)
		{
			it_Reload.Enabled = Table != null && File.Exists(Table.Fullpath);


			if (Settings._dirpreset.Count != 0) // directory presets ->
			{
				_preset = String.Empty;

				ToolStripItemCollection presets = it_OpenFolder.DropDownItems;
				for (int i = presets.Count - 1; i != -1; --i)
					presets[i].Dispose();

				presets.Clear();

				ToolStripItem preset;
				foreach (var dir in Settings._dirpreset)
				{
					if (Directory.Exists(dir))
					{
						preset = presets.Add(dir);
						preset.Click += fileclick_OpenFolder;
					}
				}
				it_OpenFolder.Visible = presets.Count != 0;
			}

			if (Settings._recent != 0) // recent files ->
			{
				ToolStripItem it;
				ToolStripItemCollection recents = it_Recent.DropDownItems;
				for (int i = recents.Count - 1; i != -1; --i)
				{
					if (!File.Exists((it = recents[i]).Text))
					{
						recents.Remove(it);
						it.Dispose();
					}
				}
				it_Recent.Visible = recents.Count != 0;
			}
		}


		/// <summary>
		/// Handles it-click to create a new 2da-file.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Create"/></c></param>
		/// <param name="e"></param>
		void fileclick_Create(object sender, EventArgs e)
		{
			Table = new YataGrid(this, String.Empty, false);

			Table.CreateTable(); // <- instead of LoadTable()

			_isCreate = true;
			fileclick_SaveAs(it_SaveAs, EventArgs.Empty); // shall set Fullpath (incl. tab-string).
			_isCreate = false;

			if (File.Exists(Table.Fullpath)) // instead of CreatePage() ->
			{
				DrawRegulator.SuspendDrawing(Table);

				var tab = new TabPage();
				Tabs.TabPages.Add(tab);

				tab.Tag = Table;

				tab.Text = Path.GetFileNameWithoutExtension(Table.Fullpath);

				tab.Controls.Add(Table);
				Tabs.SelectedTab = tab;

				Table.Init();

				DrawRegulator.ResumeDrawing(Table);
			}
			else
			{
				YataGrid._init = false;
				Table.Dispose();
			}

			_bypassVerifyFile = true;
			tab_SelectedIndexChanged(null, EventArgs.Empty);
			_bypassVerifyFile = false;
		}
		bool _isCreate;

		/// <summary>
		/// Handles it-click to open a 2da-file in a preset folder.
		/// </summary>
		/// <param name="sender"><c><see cref="it_OpenFolder"/></c> subits</param>
		/// <param name="e"></param>
		void fileclick_OpenFolder(object sender, EventArgs e)
		{
			_preset = (sender as ToolStripItem).Text;
			fileclick_Open(sender, e);
		}

		/// <summary>
		/// Handles it-click to open a 2da-file.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Open"/></c></item>
		/// <item><c><see cref="it_OpenFolder"/></c> subits</item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Open <c>[Ctrl+o]</c></item>
		/// <item>File|Open@Folder ...
		/// <c><see cref="fileclick_OpenFolder()">fileclick_OpenFolder()</see></c>
		/// subits</item>
		/// </list></remarks>
		void fileclick_Open(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				if (sender == it_Open)
				{
					string dir = Directory.GetCurrentDirectory();
					if (dir != Application.StartupPath)
					{
//						ofd.InitialDirectory = Directory.GetCurrentDirectory();
						ofd.FileName = Path.Combine(dir, "*.2da");
					}
					// else use the directory of the filetype stored in the Registry
					// ... if it exists.
				}
				else // invoked by fileclick_OpenFolder()
				{
					ofd.RestoreDirectory = true;

//					ofd.InitialDirectory = _preset;					// <- does not always work.
					ofd.FileName = Path.Combine(_preset, "*.2da");	// -> but that forces it to.
				}

				ofd.Title  = "Select a 2da file";
				ofd.Filter = Get2daFilter();

				ofd.ShowReadOnly =
				ofd.Multiselect  = true;


				if (ofd.ShowDialog() == DialogResult.OK)
				{
					bool read = ofd.ReadOnlyChecked;
					foreach (var pfe in ofd.FileNames)
						CreatePage(pfe, read); // load 1+ file(s) by openfiledialog
				}
			}
		}

		/// <summary>
		/// Handles it-click to reload the current table. Requests
		/// user-confirmation if data has changed etc.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Reload"/></c></item>
		/// <item><c><see cref="tabit_Reload"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Reload <c>[Ctrl+r]</c></item>
		/// <item>tab|Reload</item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_Reload(object sender, EventArgs e)
		{
			if (File.Exists(Table.Fullpath)) // check 'Table.Fullpath' in case user presses [Ctrl+r] after deleting the 2da-file on the hardrive.
			{
				bool reload = !Table.Changed;
				if (!reload)
				{
					using (var ib = new Infobox(Infobox.Title_alert,
												"Data has changed. Okay to reload ...",
												null,
												InfoboxType.Warn,
												InfoboxButtons.CancelYes))
					{
						reload = ib.ShowDialog(this) == DialogResult.OK;
					}
				}

				if (reload)
				{
					Obfuscate();

					if      (_diff1 == Table) _diff1 = null;
					else if (_diff2 == Table) _diff2 = null;


					_bypassVerifyFile = true;

					int result = Table.LoadTable();
					if (result != YataGrid.LOADRESULT_FALSE)
					{
						DrawRegulator.SuspendDrawing(Table);

						Table._ur.Clear();

						it_freeze1.Checked =
						it_freeze2.Checked = false;

						Table.Init(result == YataGrid.LOADRESULT_CHANGED, true);

						if (Table.Propanel != null)
						{
							Table.Controls.Remove(Table.Propanel);
							Table.Propanel = null;
						}

						DrawRegulator.ResumeDrawing(Table);
					}
					else
					{
						Table.Changed = false; // bypass Close warn
						fileclick_ClosePage(sender, e);
					}

					_bypassVerifyFile = false;

					if (Table != null)
					{
						Obfuscate(false);
						VerifyCurrentFileState();
					}
				}
			}
			else
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"File does not exist.",
											Table.Fullpath,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Opens a 2da-file from the recent-files list.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Recent"/></c> subits</param>
		/// <param name="e"></param>
		/// <remarks>Invalid subits get deleted when dropdown opens.</remarks>
		void fileclick_Recent(object sender, EventArgs e)
		{
			CreatePage((sender as ToolStripItem).Text); // load file from recents
		}

		/// <summary>
		/// Allows user to set the current table's readonly flag after it has
		/// been opened.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Readonly"/></c></param>
		/// <param name="e"></param>
		/// <remarks>The readonly setter appears otherwise only in the file-open
		/// dialog.
		/// 
		/// 
		/// Called by
		/// <list type="bullet">
		/// <item>File|Readonly <c>[F12]</c></item>
		/// </list></remarks>
		void fileclick_Readonly(object sender, EventArgs e)
		{
			it_Save   .Enabled = !(Table.Readonly = it_Readonly.Checked);
			it_SaveAll.Enabled = AllowSaveAll();

			EnableCelleditOperations();
			EnableRoweditOperations();

			it_OrderRows.Enabled = !Table.Readonly;

			Tabs.Invalidate();
		}


		/// <summary>
		/// Handles several it-clicks that write a <c><see cref="YataGrid"/></c>
		/// to a 2da-file.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Save"/></c></item>
		/// <item><c><see cref="tabit_Save"/></c></item>
		/// <item><c><see cref="it_SaveAs"/></c></item>
		/// <item><c><see cref="it_SaveAll"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Save <c>[Ctrl+s]</c></item>
		/// <item>tab|Save</item>
		/// <item>File|SaveAs <c>[Ctrl+e]</c> <c><see cref="fileclick_SaveAs()">fileclick_SaveAs()</see></c></item>
		/// <item>File|SaveAll <c>[Ctrl+a]</c> <c><see cref="fileclick_SaveAll()">fileclick_SaveAll()</see></c></item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_Save(object sender, EventArgs e)
		{
			bool overwrite; // force a Readonly file to overwrite itself (only if invoked by SaveAs)
			bool bypassReadonly;

			if (sender == it_SaveAs)
			{
				_table = Table;
				// '_pfeT' is set by caller
				overwrite = (_pfeT == _table.Fullpath);
				bypassReadonly = false;
			}
			else if (sender == it_SaveAll)
			{
				// '_table' and '_pfeT' are set by caller
				overwrite = false;
				bypassReadonly = false;
			}
			else // is rego-save or tab-save or 'FileWatcherDialog' save
			{
				_table = Table;
				_pfeT = _table.Fullpath;
				overwrite = false;

				if (sender == it_Save || sender == tabit_Save)
					bypassReadonly = false;
				else
					bypassReadonly = true; // only 'VerifyCurrentFileState()' gets to bypass Readonly.
			}

			_warned = false;

			if (!_table.Readonly || bypassReadonly
				|| (overwrite && SaveWarning("The 2da-file is opened as readonly.")))
			{
//				if ((_table._sortcol == 0 && _table._sortdir == YataGrid.SORT_ASC)
//					|| SaveWarning("The 2da is not sorted by ascending ID."))
//				{
				if (CheckRowOrder() || SaveWarning("Faulty row ids are detected."))
				{
					_table.Fullpath = _pfeT;

					SetTitlebarText();

					if (!_isCreate) // stuff that's unneeded and/or unwanted when creating a 2da ->
					{
						if (overwrite) _table.Readonly = false;	// <- IMPORTANT: If a file that was opened Readonly is saved
																//               *as itself* it loses its Readonly flag.

						_table.Changed = false;
						_table._ur.ResetSaved();

						_table.ClearLoadchanged(); // this toggles YataGrid._init - don't do that when creating a 2da

						if (_table == Table)
							_table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
					}

					FileOutput.Write(_table);
				}
//				}
			}
			else if (!_warned)
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"The 2da-file is opened as readonly.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Requests user-confirmation when saving a file when readonly or when
		/// a faulty row-id is detected.
		/// </summary>
		/// <param name="head"></param>
		/// <returns><c>true</c> to proceed - <c>false</c> to stop</returns>
		bool SaveWarning(string head)
		{
			_warned = true;
			using (var ib = new Infobox(Infobox.Title_alert,
										head + " Save anyway ...",
										null,
										InfoboxType.Warn,
										InfoboxButtons.CancelYes))
			{
				return ib.ShowDialog(this) == DialogResult.OK;
			}
		}

		/// <summary>
		/// Checks the row-order before save.
		/// </summary>
		/// <returns><c>true</c> if row-order is okay</returns>
		static bool CheckRowOrder()
		{
			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			if (!Int32.TryParse(Table[r,0].text, out result) || result != r)
				return false;

			return true;
		}

		/// <summary>
		/// Handles it-click on File|SaveAs.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAs"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAs <c>[Ctrl+e]</c></item>
		/// <item>File|Create ...</item>
		/// </list></remarks>
		void fileclick_SaveAs(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.Title    = "Save as ...";
				sfd.Filter   = Get2daFilter();
				sfd.FileName = Path.GetFileName(Table.Fullpath);

				if (Directory.Exists(_lastSaveasDirectory))
				{
					sfd.InitialDirectory = _lastSaveasDirectory;
				}
				else if (Table.Fullpath.Length != 0)
				{
					string dir = Path.GetDirectoryName(Table.Fullpath);
					if (Directory.Exists(dir))
						sfd.InitialDirectory = dir;
				}


				if (sfd.ShowDialog() == DialogResult.OK)
				{
					_lastSaveasDirectory = Path.GetDirectoryName(sfd.FileName);

					_pfeT = sfd.FileName;
					fileclick_Save(sender, e);
				}
			}
		}

		/// <summary>
		/// Handles it-click on File|SaveAll.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAll"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAll <c>[Ctrl+a]</c></item>
		/// </list></remarks>
		void fileclick_SaveAll(object sender, EventArgs e)
		{
			IsSaveAll = true;

			bool changed = false;
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				_table = Tabs.TabPages[i].Tag as YataGrid;
				if (!_table.Readonly)
				{
					if (_table.Changed)
						changed = true;

					_pfeT = _table.Fullpath;
					fileclick_Save(sender, e);
				}
			}

			if (changed) SetAllTabTexts();

			IsSaveAll = false;
		}


		/// <summary>
		/// Handles it-click on File|Close.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Close"/></c></item>
		/// <item><c><see cref="tabit_Close"/></c></item>
		/// <item><c><see cref="it_Reload"/></c></item>
		/// <item><c><see cref="tabit_Reload"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Close <c>[F4]</c></item>
		/// <item>tab|Close</item>
		/// <item>File|Reload <c>[Ctrl+r]</c>
		/// <c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// <item>tab|Reload 
		/// <c><see cref="fileclick_Reload()">fileclick_Reload()</see></c></item>
		/// <item><c><see cref="VerifyCurrentFileState()">VerifyCurrentFileState()</see></c></item>
		/// </list></remarks>
		internal void fileclick_ClosePage(object sender, EventArgs e)
		{
			bool close = !Table.Changed;
			if (!close)
			{
				using (var ib = new Infobox(Infobox.Title_alert,
											"Data has changed. Okay to close ...",
											null,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					close = ib.ShowDialog(this) == DialogResult.OK;
				}
			}

			if (close) ClosePage(Tabs.SelectedTab);
		}

		/// <summary>
		/// Handles it-click on File|CloseAll.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_CloseAll"/></c></item>
		/// <item><c><see cref="tabit_CloseAll"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|CloseAll <c>[Ctrl+F4]</c></item>
		/// <item>tab|CloseAll</item>
		/// </list></remarks>
		void fileclick_CloseAllTabs(object sender, EventArgs e)
		{
			if (!CancelChangedTables("close"))
			{
				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
					ClosePage(Tabs.TabPages[tab]);
			}
		}


		/// <summary>
		/// Handles it-click on File|Quit.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Quit"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Quit <c>[Ctrl+q]</c></item>
		/// </list></remarks>
		void fileclick_Quit(object sender, EventArgs e)
		{
			Close(); // let yata_Closing() handle it ...
		}
		#endregion Handlers (file)


		#region Methods (file)
		/// <summary>
		/// Checks if there is a non-readonly table open. Also checks if there
		/// are two non-readonly instances of the same 2da-file open.
		/// </summary>
		/// <returns><c>true</c> if SaveAll is allowed</returns>
		bool AllowSaveAll()
		{
			if (Settings._allowdupls)
			{
				// iterate through all tables and if a different table has the
				// same Fullpath and neither table is Readonly return false ->

				YataGrid table0, table1;
				string pfe;

				for (int i = 0; i != Tabs.TabPages.Count; ++i)
				if (!(table0 = Tabs.TabPages[i].Tag as YataGrid).Readonly)
				{
					pfe = table0.Fullpath;
					for (int j = 0; j != Tabs.TabPages.Count; ++j)
					if (j != i
						&& !(table1 = Tabs.TabPages[j].Tag as YataGrid).Readonly
						&& table1.Fullpath == pfe)
					{
						return false;
					}
				}
			}

			// next just iterate over all tables and if any is not Readonly
			// allow SaveAll ->
			for (int i = 0; i != Tabs.TabCount; ++i)
			if (!(Tabs.TabPages[i].Tag as YataGrid).Readonly)
				return true;

			return false;
		}
		#endregion Methods (file)


		#region Handlers (edit)
		/// <summary>
		/// Handles the <c>DropDownOpening</c> event for
		/// <c><see cref="it_MenuEdit"/></c>. Deters if
		/// <c><see cref="it_DeselectAll"/></c> ought be enabled.
		/// </summary>
		/// <param name="sender"><c>it_MenuEdit</c></param>
		/// <param name="e"></param>
		void edit_dropdownopening(object sender, EventArgs e)
		{
			it_DeselectAll.Enabled = Table != null && Table.anySelected();
		}


		/// <summary>
		/// Handles it-click to undo the previous operation if possible.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Undo"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Undo <c>[Ctrl+z]</c></item>
		/// </list></remarks>
		void editclick_Undo(object sender, EventArgs e)
		{
			Table._ur.Undo();
			it_Undo.Enabled = Table._ur.CanUndo;
			it_Redo.Enabled = true;
		}

		/// <summary>
		/// Handles it-click to redo the previous operation if possible.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Redo"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Redo <c>[Ctrl+y]</c></item>
		/// </list></remarks>
		void editclick_Redo(object sender, EventArgs e)
		{
			Table._ur.Redo();
			it_Redo.Enabled = Table._ur.CanRedo;
			it_Undo.Enabled = true;
		}


		/// <summary>
		/// Deselects all <c><see cref="Cell">Cells</see></c>/
		/// <c><see cref="Row">Rows</see></c>/
		/// <c><see cref="Col">Cols</see></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_DeselectAll"/></c></item>
		/// <item><c><see cref="YataGrid"/></c> - <c>RMB</c> outside the grid</item>
		/// </list></param>
		/// <param name="e"></param>
		internal void editclick_Deselect(object sender, EventArgs e)
		{
			Table.ClearSelects();
			ClearSyncSelects();

			Table.Invalidator(YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS
							| YataGrid.INVALID_COLS);
		}


		/// <summary>
		/// This is used by
		/// <c><see cref="YataStrip"></see>.WndProc()</c> to workaround .net
		/// fuckuppery that causes the <c>TextBoxes</c> on the Menubar to refire
		/// their <c>Enter</c> events even when they are already Entered and
		/// Focused, which screws up the select-all-text routine.
		/// </summary>
		/// <returns><c>true</c> if either <c><see cref="tb_Goto"/></c> or
		/// <c><see cref="tb_Search"/> has focus.</c></returns>
		internal bool isTextboxFocused()
		{
			return tb_Goto.Focused || tb_Search.Focused;
		}


		/// <summary>
		/// Handles select-all hocus-pocus when focus enters the Search
		/// <c>ToolStripTextBox</c> on the <c><see cref="YataStrip"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void enter_Searchbox(object sender, EventArgs e)
		{
			//logfile.Log("YataForm.enter_Searchbox()");
//			(sender as ToolStripTextBox).SelectAll(); haha good luck. Text cannot be selected in the Enter event.

			_selectall_search   = !_isEditclick_search && !IsTabbed_search;
			_isEditclick_search =
			IsTabbed_search     = false;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when a <c>MouseDown</c> event occurs
		/// for a <c>ToolStripTextBox</c> on the Menubar.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Goto"/></c></item>
		/// <item><c><see cref="tb_Search"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void mousedown_Searchbox(object sender, MouseEventArgs e)
		{
			//logfile.Log("YataForm.mousedown_Searchbox() _selectall= " + _selectall_search);
			if (_selectall_search)
			{
				_selectall_search = false;
				(sender as ToolStripTextBox).SelectAll();
			}
		}

		/// <summary>
		/// Handles selectall hocus-pocus when user clicks the search-box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Search"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Find <c>[Ctrl+f]</c></item>
		/// </list></remarks>
		void editclick_FocusSearch(object sender, EventArgs e)
		{
			_isEditclick_search = true;
			tb_Search.Focus();
			tb_Search.SelectAll();
		}

		/// <summary>
		/// Handles the <c>TextChanged</c> event on the search-box.
		/// Enables/disables find next/find previous.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void textchanged_Searchbox(object sender, EventArgs e)
		{
			it_Searchnext.Enabled =
			it_Searchprev.Enabled = Table != null
								 && tb_Search.Text.Length != 0;
		}

		/// <summary>
		/// Performs search without changing focus.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Searchnext"/></c> <c>[F3]</c></item>
		/// <item><c><see cref="it_Searchprev"/></c> <c>[Shift+F3]</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Find next <c>[F3]</c></item>
		/// <item>Edit|Find previous <c>[Shift+F3]</c></item>
		/// </list></remarks>
		void editclick_StartSearch(object sender, EventArgs e)
		{
			// the editor will never be visible here because the only way to
			// get here is by click on the Menubar - will close the editor
			// because the editor loses focus - or by [F3] - which will be
			// bypassed if the editor is open - see YataEditbox.OnPreviewKeyDown().

			IsSearch = true;
			Search(sender == it_Searchnext); // [F3] shall not force the Table focused.
			IsSearch = false;
		}

		/// <summary>
		/// Performs a search when <c>[Enter]</c> or [Shift+Enter] is pressed
		/// and focus is on either the search-box or the search-option dropdown.
		/// </summary>
		/// <remarks>[Enter] and [Shift+Enter] change focus to the table.</remarks>
		void Search_keyEnter()
		{
			IsSearch = true;
			if (Search((ModifierKeys & Keys.Shift) == Keys.None))
				Table.Select();

			IsSearch = false;
		}

		/// <summary>
		/// Searches the current table for the text in the search-box.
		/// </summary>
		/// <param name="forward"></param>
		/// <returns><c>true</c> if a match is found</returns>
		/// <remarks>Ensure that <c><see cref="Table"/></c> is valid before
		/// call.</remarks>
		bool Search(bool forward)
		{
			if ((ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				string search = tb_Search.Text;
				if (search.Length != 0)
				{
					search = search.ToUpper(CultureInfo.CurrentCulture);

					Cell sel = Table.getSelectedCell();
					int selr = Table.getSelectedRow();

					Table.ClearSelects();

					bool substring = (cb_SearchOption.SelectedIndex == 0); // else is wholestring search.
					bool start = true;

					string text;

					int r,c;

					if (forward) // forward search ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = -1;
							if (selr == -1) selr = 0;
						}

						for (r = selr; r != Table.RowCount; ++r)
						{
							if (start)
							{
								start = false;
								if (++c == Table.ColCount)		// if starting on the last cell of a row
								{
									c = 0;

									if (r < Table.RowCount - 1)	// jump to the first cell of the next row
										++r;
									else						// or to the top of the table if on the last row
										r = 0;
								}
							}
							else
								c = 0;

							for (; c != Table.ColCount; ++c)
							{
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != selr + 1;       ++r) // quick and dirty wrap ->
						for (c = 0; c != Table.ColCount; ++c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return true;
							}
						}
					}
					else // backward search ->
					{
						if (sel != null) { c = sel.x; selr = sel.y; }
						else
						{
							c = Table.ColCount;
							if (selr == -1) selr = Table.RowCount - 1;
						}

						for (r = selr; r != -1; --r)
						{
							if (start)
							{
								start = false;
								if (--c == -1)	// if starting on the first cell of a row
								{
									c = Table.ColCount - 1;

									if (r > 0)	// jump to the last cell of the previous row
										--r;
									else		// or to the bottom of the table if on the first row
										r = Table.RowCount - 1;
								}
							}
							else
								c = Table.ColCount - 1;

							for (; c != -1; --c)
							{
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return true;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = Table.RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
						for (c = Table.ColCount - 1; c != -1;       --c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToUpper(CultureInfo.CurrentCulture)) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return true;
							}
						}
					}
				}
				else // not found ->
				{
					Table.ClearSelects(); // TODO: That should return a bool if any clears happened.
					ClearSyncSelects();
				}

				int invalid = YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS;
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid);
			}
			return false;
		}


		/// <summary>
		/// Handles select-all hocus-pocus when focus enters the Goto
		/// <c>ToolStripTextBox</c> on the <c><see cref="YataStrip"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void enter_Gotobox(object sender, EventArgs e)
		{
			//logfile.Log("YataForm.enter_Gotobox()");
//			(sender as ToolStripTextBox).SelectAll(); haha good luck. Text cannot be selected in the Enter event.

			_selectall_goto   = !_isEditclick_goto && !IsTabbed_goto;
			_isEditclick_goto =
			IsTabbed_goto     = false;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when a <c>MouseDown</c> event occurs
		/// for a <c>ToolStripTextBox</c> on the Menubar.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tb_Goto"/></c></item>
		/// <item><c><see cref="tb_Search"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void mousedown_Gotobox(object sender, MouseEventArgs e)
		{
			//logfile.Log("YataForm.mousedown_Gotobox() _selectall= " + _selectall_search);
			if (_selectall_goto)
			{
				_selectall_goto = false;
				(sender as ToolStripTextBox).SelectAll();
			}
		}

		/// <summary>
		/// Handles the <c>Click</c> event to focus the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Goto"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Goto <c>[Ctrl+g]</c></item>
		/// </list></remarks>
		void editclick_FocusGoto(object sender, EventArgs e)
		{
			_isEditclick_goto = true;
			tb_Goto.Focus();
			tb_Goto.SelectAll();
		}

		/// <summary>
		/// Handles the <c>TextChanged</c> event on the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void textchanged_Gotobox(object sender, EventArgs e)
		{
			// TODO: allow a blank string

			int result;
			if (!Int32.TryParse(tb_Goto.Text, out result)
				|| result < 0)
			{
				tb_Goto.Text = "0"; // recurse
			}
			else if (Table != null && Settings._instantgoto)
			{
				Table.doGoto(tb_Goto.Text, false); // NOTE: Text is checked for validity in doGoto().
			}
		}


		/// <summary>
		/// Handles the <c>Click</c> event to edit the 2da-file's defaultval.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Defaultval"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Default value</item>
		/// </list></remarks>
		void editclick_Defaultval(object sender, EventArgs e)
		{
			InputDialog._defaultval = Table._defaultval;
			using (var idc = new InputDialog(this))
			{
				if (idc.ShowDialog(this) == DialogResult.OK
					&& InputDialog._defaultval != Table._defaultval)
				{
					Table._defaultval = InputDialog._defaultval;
					if (!Table.Changed) Table.Changed = true;

					it_Defaultclear.Enabled = Table._defaultval.Length != 0;
				}
			}
		}

		/// <summary>
		/// Handles the <c>Click</c> event to clear the 2da-file's defaultval.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Defaultclear"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Clear Default</item>
		/// </list></remarks>
		void editclick_Defaultclear(object sender, EventArgs e)
		{
			Table._defaultval = String.Empty;
			if (!Table.Changed) Table.Changed = true;

			it_Defaultclear.Enabled = false;
		}
		#endregion Handlers (edit)


		#region Methods (edit)
		/// <summary>
		/// Enables/disables <c><see cref="it_Undo"/></c>.
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableUndo(bool enable)
		{
			it_Undo.Enabled = enable;
		}

		/// <summary>
		/// Enables/disables <c><see cref="it_Redo"/></c>.
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableRedo(bool enable)
		{
			it_Redo.Enabled = enable;
		}

		/// <summary>
		/// Enables/disables <c><see cref="it_GotoLoadchanged"/></c> and
		/// <c><see cref="it_GotoLoadchanged_pre"/></c>.
		/// </summary>
		/// <param name="enabled"></param>
		internal void EnableGotoLoadchanged(bool enabled)
		{
			it_GotoLoadchanged    .Enabled =
			it_GotoLoadchanged_pre.Enabled = enabled;
		}
		#endregion Methods (edit)


		#region Handlers (editcells)
		/// <summary>
		/// Deselects all <c><see cref="Cell">Cells</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectCell"/></c></param>
		/// <param name="e"></param>
		void editcellsclick_Deselect(object sender, EventArgs e)
		{
			Table.ClearCellSelects();
			ClearSyncSelects();

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
			// TODO: not sure why but that deselects and invalidates a Propanel select also.
		}


		/// <summary>
		/// Cuts an only selected cell or cells in a contiguous block.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CutCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Cut <c>[Ctrl+x]</c></item>
		/// </list></remarks>
		void editcellsclick_CutCell(object sender, EventArgs e)
		{
			Cell sel = Table.getFirstSelectedCell();
			Cell cell; string text;

			int invalid = -1;

			_copytext = new string[_copyvert, _copyhori];

			int i = -1, j;
			for (int r = sel.y; r != sel.y + _copyvert; ++r)
			{
				++i; j = -1;
				for (int c = sel.x; c != sel.x + _copyhori; ++c)
				{
					_copytext[i, ++j] = (cell = Table[r,c]).text;

					if (c == 0 && Settings._autorder)
						text = r.ToString(CultureInfo.InvariantCulture);
					else
						text = gs.Stars;

					if (cell.text != text)
					{
						Table.ChangeCellText(cell, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else if (cell.loadchanged)
					{
						cell.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (_fclip != null)
				_fclip.SetCellsBufferText();

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Copies an only selected cell or cells in a contiguous block.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CopyCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Copy <c>[Ctrl+c]</c></item>
		/// </list></remarks>
		void editcellsclick_CopyCell(object sender, EventArgs e)
		{
			Cell sel = Table.getFirstSelectedCell();

			_copytext = new string[_copyvert, _copyhori];

			int i = -1, j;
			for (int r = sel.y; r != sel.y + _copyvert; ++r)
			{
				++i; j = -1;
				for (int c = sel.x; c != sel.x + _copyhori; ++c)
				{
					_copytext[i, ++j] = Table[r,c].text;
				}
			}

			if (_fclip != null)
				_fclip.SetCellsBufferText();
		}

		/// <summary>
		/// Pastes to an only selected cell. If more than one field is in
		/// <c><see cref="_copytext">_copytext[,]</see></c> then the only
		/// selected cell will be the top-left corner of the paste-block; fields
		/// that overflow the table to right or bottom shall be ignored.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_PasteCell"/></c></item>
		/// <item><c><see cref="cellit_Paste"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Paste <c>[Ctrl+v]</c></item>
		/// <item>cell|Paste <c><see cref="cellclick_Paste()">cellclick_Paste()</see></c></item>
		/// </list></remarks>
		void editcellsclick_PasteCell(object sender, EventArgs e)
		{
			Cell sel = Table.getSelectedCell();
			Cell cell; string text;

			int invalid = -1;

			if (sel.x >= Table.FrozenCount)
			{
				for (int r = 0; r != _copytext.GetLength(0) && r + sel.y != Table.RowCount; ++r)
				for (int c = 0; c != _copytext.GetLength(1) && c + sel.x != Table.ColCount; ++c)
				{
					(cell = Table[r + sel.y,
								  c + sel.x]).selected = true;

					if (cell.x == 0 && Settings._autorder)
						text = cell.y.ToString(CultureInfo.InvariantCulture);
					else
						text = _copytext[r,c];

					if (text != cell.text)
					{
						Table.ChangeCellText(cell, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else if (cell.loadchanged)
					{
						cell.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}
			else
			{
				if (sel.x == 0 && Settings._autorder)
					text = sel.y.ToString(CultureInfo.InvariantCulture);
				else
					text = _copytext[0,0];

				if (text != sel.text)
				{
					Table.ChangeCellText(sel, text);	// does not do a text-check
					invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
				}
				else if (sel.loadchanged)
				{
					sel.loadchanged = false;
					invalid = YataGrid.INVALID_GRID;
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);

			EnableCelleditOperations();
		}

		/// <summary>
		/// Pastes "****" to all selected cells.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeleteCell"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Delete <c>[Delete]</c></item>
		/// </list></remarks>
		internal void editcellsclick_Delete(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (c == 0 && Settings._autorder)
						text = sel.y.ToString(CultureInfo.InvariantCulture);
					else
						text = gs.Stars;

					if (sel.text != text)
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else if (sel.loadchanged)
					{
						sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Converts all selected cells to lowercase.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Lower"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Lowercase</item>
		/// </list></remarks>
		void editcellsclick_Lower(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != (text = sel.text.ToLower(CultureInfo.CurrentCulture)))
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else if (sel.loadchanged)
					{
						sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Converts all selected cells to uppercase.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Upper"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Uppercase</item>
		/// </list></remarks>
		void editcellsclick_Upper(object sender, EventArgs e)
		{
			Cell sel; string text;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != (text = sel.text.ToUpper(CultureInfo.CurrentCulture)))
					{
						Table.ChangeCellText(sel, text);	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
					}
					else if (sel.loadchanged)
					{
						sel.loadchanged = false;

						if (invalid == -1)
							invalid = YataGrid.INVALID_GRID;
					}
				}
			}

			if (invalid == YataGrid.INVALID_GRID)
				Table.Invalidator(invalid);
		}

		/// <summary>
		/// Opens a text-input dialog for pasting text to all selected cells.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Apply"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Apply text ...</item>
		/// </list></remarks>
		void editcellsclick_Apply(object sender, EventArgs e)
		{
			using (var tid = new InputCelltextDialog(this))
			{
				if (tid.ShowDialog(this) == DialogResult.OK)
				{
					Cell sel; string text;
					int invalid = -1;

					foreach (var row in Table.Rows)
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = row[c]).selected)
						{
							if (c == 0 && Settings._autorder)
								text = sel.y.ToString(CultureInfo.InvariantCulture);
							else
								text = _copytext[0,0];

							if (sel.text != text)
							{
								Table.ChangeCellText(sel, text);	// does not do a text-check
								invalid = YataGrid.INVALID_NONE;	// ChangeCellText() will run the Invalidator.
							}
							else if (sel.loadchanged)
							{
								sel.loadchanged = false;
		
								if (invalid == -1)
									invalid = YataGrid.INVALID_GRID;
							}
						}
					}

					if (invalid == YataGrid.INVALID_GRID)
						Table.Invalidator(invalid);
				}
			}
		}

		/// <summary>
		/// Selects the next LoadChanged cell.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_GotoLoadchanged"/></c> <c>[Ctrl+n]</c></item>
		/// <item><c><see cref="it_GotoLoadchanged_pre"/></c> <c>[Shift+Ctrl+n]</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Goto loadchanged <c>[Ctrl+n]</c></item>
		/// <item>Edit|Goto loadchanged pre <c>[Shift+Ctrl+n]</c></item>
		/// </list></remarks>
		void editcellsclick_GotoLoadchanged(object sender, EventArgs e)
		{
			if ((ModifierKeys & Keys.Alt) == Keys.None && Table.anyLoadchanged())
			{
				Table.Select();

				Cell sel = Table.getSelectedCell();
				int selr = Table.getSelectedRow();

				Table.ClearSelects();

				int r,c;

				bool start = true;

				if (sender == it_GotoLoadchanged) // forward gotolc ->
				{
					if (sel != null) { c = sel.x; selr = sel.y; }
					else
					{
						c = -1;
						if (selr == -1) selr = 0;
					}

					for (r = selr; r != Table.RowCount; ++r)
					{
						if (start)
						{
							start = false;
							if (++c == Table.ColCount)		// if starting on the last cell of a row
							{
								c = 0;

								if (r < Table.RowCount - 1)	// jump to the first cell of the next row
									++r;
								else						// or to the top of the table if on the last row
									r = 0;
							}
						}
						else
							c = 0;

						for (; c != Table.ColCount; ++c)
						{
							if ((sel = Table[r,c]).loadchanged)
							{
								Table.SelectCell(sel);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = 0; r != selr + 1;       ++r) // quick and dirty wrap ->
					for (c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = Table[r,c]).loadchanged)
						{
							Table.SelectCell(sel);
							return;
						}
					}
				}
				else // backward gotolc ->
				{
					if (sel != null) { c = sel.x; selr = sel.y; }
					else
					{
						c = Table.ColCount;
						if (selr == -1) selr = Table.RowCount - 1;
					}

					for (r = selr; r != -1; --r)
					{
						if (start)
						{
							start = false;
							if (--c == -1)	// if starting on the first cell of a row
							{
								c = Table.ColCount - 1;

								if (r > 0)	// jump to the last cell of the previous row
									--r;
								else		// or to the bottom of the table if on the first row
									r = Table.RowCount - 1;
							}
						}
						else
							c = Table.ColCount - 1;

						for (; c != -1; --c)
						{
							if ((sel = Table[r,c]).loadchanged)
							{
								Table.SelectCell(sel);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = Table.RowCount - 1; r != selr - 1; --r) // quick and dirty wrap ->
					for (c = Table.ColCount - 1; c != -1;       --c)
					{
						if ((sel = Table[r,c]).loadchanged)
						{
							Table.SelectCell(sel);
							return;
						}
					}
				}
			}
		}
		#endregion Handlers (editcells)


		#region Methods (editcells)
		/// <summary>
		/// Determines the dis/enabled states of cell-edit operations.
		/// </summary>
		internal void EnableCelleditOperations()
		{
			bool anyselected = Table.anyCellSelected();
			it_DeselectCell.Enabled = anyselected;

			bool contiguous = Table.areSelectedCellsContiguous();
			it_CutCell     .Enabled = !Table.Readonly && contiguous;
			it_CopyCell    .Enabled = contiguous;

			it_PasteCell   .Enabled = !Table.Readonly && Table.getSelectedCell() != null;

			it_DeleteCell  .Enabled = // TODO: if any selected cell is not 'gs.Stars' or loadchanged
			it_Lower       .Enabled = // TODO: if any selected cell is not lowercase  or loadchanged
			it_Upper       .Enabled = // TODO: if any selected cell is not uppercase  or loadchanged
			it_Apply       .Enabled = !Table.Readonly && anyselected;

			// NOTE: 'it_GotoLoadchanged*.Enabled' shall be detered independently
			// by EnableGotoLoadchanged()
		}
		#endregion Methods (editcells)


		#region Handlers (editrows)
		/// <summary>
		/// Deselects all <c><see cref="Row">Rows</see></c> and subrows as well
		/// as all <c><see cref="Cell">Cells</see></c> in
		/// <c><see cref="YataGrid.FrozenPanel">YataGrid.FrozenPanel</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectRows"/></c></param>
		/// <param name="e"></param>
		void editrowsclick_Deselect(object sender, EventArgs e)
		{
			foreach (var row in Table.Rows)
			{
				if (row.selected)
					row.selected = false;

				for (int c = 0; c != Table.ColCount; ++c)
				{
					if (!Table.Cols[c].selected)
						row[c].selected = false;
				}
			}

			YataGrid table; // do special sync ->

			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else                      table = null;

			if (table != null)
			{
				foreach (var row in table.Rows)
				{
					if (row.selected)
					{
						Row._bypassEnableRowedit = true;
						row.selected = false;
						Row._bypassEnableRowedit = false;
					}
	
					for (int c = 0; c != table.ColCount; ++c)
					{
						if (!table.Cols[c].selected)
							row[c].selected = false;
					}
				}
			}

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS);
		}


		/// <summary>
		/// Cuts a range of rows.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CutRange"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c></item>
		/// </list></remarks>
		void editrowsclick_CutRange(object sender, EventArgs e)
		{
			editrowsclick_CopyRange(  sender, e);
			editrowsclick_DeleteRange(sender, e);
		}

		/// <summary>
		/// Copies a range of rows and enables
		/// <c><see cref="it_PasteRange"/></c> and
		/// <c><see cref="it_ClipExport"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_CopyRange"/></c></item>
		/// <item><c><see cref="it_CutRange"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Copy <c>[Ctrl+Shift+c]</c></item>
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c>
		/// <c><see cref="editrowsclick_CutRange()">editrowsclick_CutRange()</see></c></item>
		/// </list></remarks>
		void editrowsclick_CopyRange(object sender, EventArgs e)
		{
			_copyr.Clear();

			int selr = Table.getSelectedRow();

			int strt, stop;
			if (Table.RangeSelect > 0)
			{
				strt = selr;
				stop = selr + Table.RangeSelect;
			}
			else
			{
				strt = selr + Table.RangeSelect;
				stop = selr;
			}

			string[] celltexts;
			do
			{
				celltexts = new string[Table.ColCount];
				for (int c = 0; c != Table.ColCount; ++c)
					celltexts[c] = Table[strt, c].text;

				_copyr.Add(celltexts);
			}
			while (++strt <= stop);

			if (_fclip != null)
				_fclip.SetRowsBufferText();

			it_PasteRange.Enabled = !Table.Readonly;
			it_ClipExport.Enabled = true;
		}

		/// <summary>
		/// Pastes a range of rows.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PasteRange"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Paste <c>[Ctrl+Shift+v]</c></item>
		/// </list></remarks>
		void editrowsclick_PasteRange(object sender, EventArgs e)
		{
			Obfuscate();
			DrawRegulator.SuspendDrawing(Table);


			Restorable rest = UndoRedo.createArray(_copyr.Count, UndoRedo.UrType.rt_ArrayDelete);

			int selr = Table.getSelectedRow();
			if (selr == -1)
				selr = Table.RowCount;

			int r = selr;
			for (int i = 0; i != _copyr.Count; ++i, ++r)
			{
				Table.Insert(r, _copyr[i], false);
				rest.array[i] = Table.Rows[r].Clone() as Row;
			}

			Table.Calibrate(selr, _copyr.Count - 1); // paste range

			Table.ClearSelects(false, true);
			Table.Rows[selr].selected = true;
			Table.RangeSelect = _copyr.Count - 1;
			Table.EnsureDisplayedRow(selr);


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);


			DrawRegulator.ResumeDrawing(Table);
			Obfuscate(false);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Deletes a range of rows.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_DeleteRange"/></c></item>
		/// <item><c><see cref="it_CutRange"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Delete <c>[Shift+Delete]</c></item>
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c>
		/// <c><see cref="editrowsclick_CutRange()">editrowsclick_CutRange()</see></c></item>
		/// </list></remarks>
		void editrowsclick_DeleteRange(object sender, EventArgs e)
		{
			Table.DeleteRows();

			EnableRoweditOperations();

			if (Settings._autorder && order() != 0) layout();
		}


		/// <summary>
		/// Instantiates <c><see cref="RowCreatorDialog"/></c> for
		/// inserting/creating multiple rows.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CreateRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Rows|Create ... <c>[F2]</c></item>
		/// </list></remarks>
		void editrowsclick_CreateRows(object sender, EventArgs e)
		{
			int selr = Table.getSelectedRowOrCells();

			using (var rcd = new RowCreatorDialog(this, selr, _copyr.Count != 0))
			{
				if (rcd.ShowDialog(this) == DialogResult.OK)
				{
					Obfuscate();
					DrawRegulator.SuspendDrawing(Table);


					Restorable rest = UndoRedo.createArray(_lengthCr, UndoRedo.UrType.rt_ArrayDelete);

					var cells = new string[Table.ColCount];
					switch (_fillCr)
					{
						case CrFillType.Stars:
							for (int i = 0; i != Table.ColCount; ++i)
								cells[i] = gs.Stars;
							break;

						case CrFillType.Selected:
							for (int i = 0; i != Table.ColCount; ++i)
								cells[i] = Table[selr, i].text;
							break;

						case CrFillType.Copied:
							for (int i = 0; i != Table.ColCount; ++i)
							{
								if (i < _copyr[0].Length)
									cells[i] = _copyr[0][i];
								else
									cells[i] = gs.Stars;
							}
							break;
					}

					int r = _startCr;
					for (int i = 0; i != _lengthCr; ++i, ++r)
					{
						cells[0] = r.ToString(CultureInfo.InvariantCulture);

						Table.Insert(r, cells, false);
						rest.array[i] = Table.Rows[r].Clone() as Row;
					}

					Table.Calibrate(_startCr, _lengthCr - 1); // insert range

					Table.ClearSelects(false, true);
					Table.Rows[_startCr].selected = true;
					Table.RangeSelect = _lengthCr - 1;
					Table.EnsureDisplayedRow(_startCr);


					if (!Table.Changed)
					{
						Table.Changed = true;
						rest.isSaved = UndoRedo.IsSavedType.is_Undo;
					}
					Table._ur.Push(rest);


					DrawRegulator.ResumeDrawing(Table);
					Obfuscate(false);

					if (Settings._autorder && order() != 0) layout();
				}
			}
		}
		#endregion Handlers (editrows)


		#region Methods (editrows)
		/// <summary>
		/// Determines the dis/enabled states of row-edit operations.
		/// </summary>
		internal void EnableRoweditOperations()
		{
			bool isrowselected = Table.getSelectedRow() != -1;

			it_DeselectRows.Enabled = isrowselected;

			it_CutRange    .Enabled = !Table.Readonly && isrowselected;
			it_CopyRange   .Enabled = isrowselected;
			it_PasteRange  .Enabled = !Table.Readonly && _copyr.Count != 0;
			it_DeleteRange .Enabled = !Table.Readonly && isrowselected;

			it_CreateRows  .Enabled = !Table.Readonly;
		}
		#endregion Methods (editrows)


		#region Handlers (editcol)
		/// <summary>
		/// Handles the <c>DropDownOpening</c> event for
		/// <c><see cref="it_MenuCol"/></c>. Deters if subits ought be enabled.
		/// </summary>
		/// <param name="sender"><c>it_MenuCol</c></param>
		/// <param name="e"></param>
		void col_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null)
			{
				bool isColSelected = Table.getSelectedCol() > 0; // id-col is disallowed

				it_DeselectCol.Enabled = isColSelected;

				it_CreateHead .Enabled = !Table.Readonly;
				it_DeleteHead .Enabled = !Table.Readonly && isColSelected && Table.ColCount > 2;
				it_RelabelHead.Enabled = !Table.Readonly && isColSelected;

				it_CopyCells  .Enabled = isColSelected;
				it_PasteCells .Enabled = isColSelected && !Table.Readonly && _copyc.Count != 0;
			}
			else
			{
				it_DeselectCol.Enabled =

				it_CreateHead .Enabled =
				it_DeleteHead .Enabled =
				it_RelabelHead.Enabled =

				it_CopyCells  .Enabled =
				it_PasteCells .Enabled = false;
			}
		}


		/// <summary>
		/// Deselects <c><see cref="Col">Col</see></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeselectCol"/></c></param>
		/// <param name="e"></param>
		void editcolclick_Deselect(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();
			Table.Cols[selc].selected = false;

			int selr = Table.getSelectedRow();
			for (int r = 0; r != Table.RowCount; ++r)
			{
				if (   (r > selr && r > selr + Table.RangeSelect)
					|| (r < selr && r < selr + Table.RangeSelect))
				{
					Table[r, selc].selected = false;
				}
			}

			YataGrid table; // do special sync ->

			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else                      table = null;

			if (table != null)
			{
				selc = table.getSelectedCol();
				table.Cols[selc].selected = false;

				selr = table.getSelectedRow();
				for (int r = 0; r != table.RowCount; ++r)
				{
					if (   (r > selr && r > selr + table.RangeSelect)
						|| (r < selr && r < selr + table.RangeSelect))
					{
						table[r, selc].selected = false;
					}
				}
			}

			EnableCelleditOperations();

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS); // NOTE: INVALID_COLS does not appear to be needed.
		}


		const string _warnColhead = "This operation cannot be undone. It clears the Undo/Redo stacks.";
//								  + " Tip: tidy and save the 2da first.";

		/// <summary>
		/// Opens a text-input dialog for creating a col at a selected col-id or
		/// at the far right if no col is selected.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CreateHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_CreateHead(object sender, EventArgs e)
		{
			const string head = _warnColhead + " Are you sure you want to create a col ...";

			using (var ib = new Infobox(Infobox.Title_infor,
										head,
										null,
										InfoboxType.Info,
										InfoboxButtons.CancelYes))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
				{
					int selc = Table.getSelectedCol();
					using (var idc = new InputDialog(this, selc))
					{
						if (idc.ShowDialog(this) == DialogResult.OK
							&& InputDialog._colabel.Length != 0)
						{
							Obfuscate();
							DrawRegulator.SuspendDrawing(Table);

							// create at far right if no col selected
							if (selc < Table.FrozenCount) // ~safety.
								selc = Table.ColCount;

							steadystate();

							Table.CreateCol(selc);

							it_freeze1.Enabled = Table.ColCount > 1;
							it_freeze2.Enabled = Table.ColCount > 2;

							DrawRegulator.ResumeDrawing(Table);
							Obfuscate(false);
						}
					}
				}
			}
		}

		/// <summary>
		/// Deletes a selected col w/ confirmation.
		/// </summary>
		/// <param name="sender"><c><see cref="it_DeleteHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_DeleteHead(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();

			const string head = _warnColhead + " Are you sure you want to delete the selected col ...";
			using (var ib = new Infobox(Infobox.Title_infor,
										head,
										Table.Fields[selc - 1],
										InfoboxType.Info,
										InfoboxButtons.CancelYes))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
				{
					Obfuscate();
					DrawRegulator.SuspendDrawing(Table);

					steadystate();

					Table.DeleteCol(selc);

					it_freeze1.Enabled = Table.ColCount > 1;
					it_freeze2.Enabled = Table.ColCount > 2;

					DrawRegulator.ResumeDrawing(Table);
					Obfuscate(false);
				}
			}
		}

		/// <summary>
		/// Puts the table in a neutral state.
		/// </summary>
		/// <remarks>Helper for
		/// <list type="bullet">
		/// <item><c><see cref="editcolclick_CreateHead()">editcolclick_CreateHead()</see></c></item>
		/// <item><c><see cref="editcolclick_DeleteHead()">editcolclick_DeleteHead()</see></c></item>
		/// </list></remarks>
		void steadystate()
		{
			it_freeze1.Checked =
			it_freeze2.Checked = false;
			Table.FrozenCount = YataGrid.FreezeId;

			Table.ClearSelects();
			Table.ClearLoadchanged();

			tabclick_DiffReset(null, EventArgs.Empty);

			if (Table.Propanel != null && Table.Propanel.Visible)
			{
				Table.Propanel.Hide();
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = false;
			}

			Table._ur.Clear();
		}

		/// <summary>
		/// Relabels the colhead of a selected col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_RelabelHead"/></c></param>
		/// <param name="e"></param>
		void editcolclick_RelabelHead(object sender, EventArgs e)
		{
			int selc = Table.getSelectedCol();

			string head = Table.Fields[selc - 1];
			InputDialog._colabel = head;
			using (var idc = new InputDialog(this, selc))
			{
				if (idc.ShowDialog(this) == DialogResult.OK
					&& InputDialog._colabel.Length != 0
					&& InputDialog._colabel != head)
				{
					Table.RelabelCol(selc);
				}
			}
		}

		/// <summary>
		/// Copies all cell-fields in a selected col to
		/// <c><see cref="_copyc"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CopyCell"/></c></param>
		/// <param name="e"></param>
		void editcolclick_CopyCol(object sender, EventArgs e)
		{
			_copyc.Clear();

			int selc = Table.getSelectedCol();

			for (int r = 0; r != Table.RowCount; ++r)
				_copyc.Add(Table[r, selc].text);

			if (_fclip != null)
				_fclip.SetColBufferText();
		}

		/// <summary>
		/// Pastes <c><see cref="_copyc"/></c> to the cell-fields of a selected
		/// col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_PasteCell"/></c></param>
		/// <param name="e"></param>
		void editcolclick_PasteCol(object sender, EventArgs e)
		{
			int diff; string head;
			if (Table.RowCount < _copyc.Count)
			{
				diff = _copyc.Count - Table.RowCount;
				head = "The table has " + diff + " less row" + (diff == 1 ? String.Empty : "s") + " than the copy.";
			}
			else if (Table.RowCount > _copyc.Count)
			{
				diff = Table.RowCount - _copyc.Count;
				head = "The copy has " + diff + " less row" + (diff == 1 ? String.Empty : "s") + " than the table.";
			}
			else { diff = 0; head = null; }

			if (diff != 0)
			{
				using (var ib = new Infobox(Infobox.Title_warni,
											head + " Proceed ...",
											null,
											InfoboxType.Warn,
											InfoboxButtons.CancelYes))
				{
					if (ib.ShowDialog(this) == DialogResult.OK)
						diff = 0;
				}
			}

			if (diff == 0)
			{
				Obfuscate();
				DrawRegulator.SuspendDrawing(Table);

				Table.PasteCol(_copyc);

				DrawRegulator.ResumeDrawing(Table);
				Obfuscate(false);
			}
		}
		#endregion Handlers (editcol)


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
		/// <c><see cref="it_PasteRange"/></c> and
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

				it_PasteRange.Enabled = Table != null && !Table.Readonly;
				it_ClipExport.Enabled = true;
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


		#region Handlers (2daOps)
		/// <summary>
		/// Handles opening the 2daOpsMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ops_dropdownopening(object sender, EventArgs e)
		{
			it_ClearUr.Enabled =  Table != null
							  && (Table._ur.CanUndo || Table._ur.CanRedo);
		}


		/// <summary>
		/// Handles it-click to order row-ids.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_OrderRows"/></c></item>
		/// <item><c><see cref="it_CheckRows"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Order rows <c>[Ctrl+d]</c></item>
		/// <item>2daOps|Test rows <c>[Ctrl+t]</c>
		/// <c><see cref="opsclick_TestOrder()">opsclick_TestOrder()</see></c></item>
		/// </list></remarks>
		void opsclick_Order(object sender, EventArgs e)
		{
			string title, head; InfoboxType ibt;

			int changed = order();
			if (changed != 0)
			{
				layout();

				title = Infobox.Title_warni;
				head  = changed + " id" + (changed == 1 ? String.Empty : "s") + " corrected.";
				ibt   = InfoboxType.Warn;
			}
			else
			{
				title = Infobox.Title_infor;
				head  = "Row order is Okay - no change.";
				ibt   = InfoboxType.Info;
			}

			using (var ib = new Infobox(title,
										head,
										null,
										ibt))
			{
				ib.ShowDialog(this);
			}
		}

		/// <summary>
		/// Orders row-ids.
		/// </summary>
		/// <returns>the count of changed row-ids</returns>
		static internal int order()
		{
			int changed = 0;

			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			if (!Int32.TryParse(Table[r,0].text, out result)
				|| result != r)
			{
				Table[r,0].text = r.ToString(CultureInfo.InvariantCulture);
				++changed;
			}
			return changed;
		}

		/// <summary>
		/// Lays out table after rows are ordered.
		/// </summary>
		internal void layout()
		{
			DrawRegulator.SuspendDrawing(this);


			if (!Table.Changed)
				 Table.Changed = true;

			Table._ur.ResetSaved(true);

			if      (_diff1 == Table) _diff1 = null;
			else if (_diff2 == Table) _diff2 = null;

			Table.Colwidth(0, 0, Table.RowCount - 1);
			Table.metricFrozenControls(FROZEN_COL_Id);

			Table.InitScroll();

			int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
			if (Table.Propanel != null && Table.Propanel.Visible)
				invalid |= YataGrid.INVALID_PROP;

			Table.Invalidator(invalid);


			DrawRegulator.ResumeDrawing(this);
		}

		/// <summary>
		/// Handles it-click to test row-ids.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CheckRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Test rows <c>[Ctrl+t]</c></item>
		/// </list></remarks>
		void opsclick_TestOrder(object sender, EventArgs e)
		{
			var borks = new List<string>();

			bool stop = false;

			int result;
			for (int r = 0; r != Table.RowCount; ++r)
			{
				if (!Int32.TryParse(Table[r,0].text, out result))
				{
					if (borks.Count == 16) // stop this Madness
					{
						stop = true;
						break;
					}
					borks.Add("id " + r + " is not an integer");
				}
				else if (result != r)
				{
					if (borks.Count == 16) // stop this Madness
					{
						stop = true;
						break;
					}
					borks.Add("id " + r + " is out of order");
				}
			}

			string title, head;
			string copy = String.Empty;
			InfoboxType ibt;

			if (borks.Count != 0)
			{
				foreach (string bork in borks)
				{
					if (copy.Length != 0) copy += Environment.NewLine;
					copy += bork;
				}

				title = Infobox.Title_warni;
				head  = "Row order is borked.";
				ibt   = InfoboxType.Warn;

				if (!Table.Readonly)
					head += " Do you want to auto-order the ids ...";
			}
			else
			{
				title = Infobox.Title_infor;
				head  = "Row order is Okay.";
				ibt   = InfoboxType.Info;
			}

			using (var ib = new Infobox(title,
										(stop ? "The test has been stopped at 16 borks. " : String.Empty) + head,
										(copy.Length != 0 ? copy + (stop ? Environment.NewLine + "..." : String.Empty) : null),
										ibt,
										(copy.Length != 0 && !Table.Readonly ? InfoboxButtons.CancelYes : InfoboxButtons.Okay)))
			{
				if (ib.ShowDialog(this) == DialogResult.OK)
					opsclick_Order(sender, e);
			}
		}


		/// <summary>
		/// Handles it-click to recolor rows. Also clears all
		/// <c><see cref="Cell.loadchanged">Cell.loadchanged</see></c> flags.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ColorRows"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Recolor rows <c>[Ctrl+l]</c></item>
		/// </list></remarks>
		void opsclick_Recolor(object sender, EventArgs e)
		{
			for (int r = 0; r != Table.RowCount; ++r)
			{
				Table.Rows[r]._brush = (r % 2 == 0) ? Brushes.Alice
													: Brushes.Bob;
			}
			Table.ClearLoadchanged();

			Table.Invalidator(YataGrid.INVALID_GRID);
		}

		/// <summary>
		/// Handles it-click to autosize cols.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_AutoCols"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Autosize cols <c>[Ctrl+i]</c></item>
		/// <item><c><see cref="DiffReset()">DiffReset()</see></c></item>
		/// </list></remarks>
		internal void opsclick_AutosizeCols(object sender, EventArgs e)
		{
			Obfuscate();
			DrawRegulator.SuspendDrawing(Table);

			AutosizeCols(Table);

			DrawRegulator.ResumeDrawing(Table);
			Obfuscate(false);
		}

		/// <summary>
		/// Autosizes all cols of a given <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Helper for
		/// <c><see cref="opsclick_AutosizeCols()">opsclick_AutosizeCols()</see></c>
		/// and <c><see cref="DiffReset()">DiffReset()</see></c>.</remarks>
		static void AutosizeCols(YataGrid table)
		{
			foreach (var col in table.Cols)
				col.UserSized = false;

			table.Calibrate(0, table.RowCount - 1);
		}


		/// <summary>
		/// Handles it-click to freeze first col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_freeze1"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Freeze 1st col <c>[F5]</c></item>
		/// </list></remarks>
		void opsclick_Freeze1stCol(object sender, EventArgs e)
		{
			Table.Select();

			it_freeze2.Checked = false;

			if (it_freeze1.Checked = !it_freeze1.Checked)
			{
				Col col = Table.Cols[FROZEN_COL_First];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_First);
				}
				Table.FrozenCount = YataGrid.FreezeFirst;
			}
			else
				Table.FrozenCount = YataGrid.FreezeId;
		}

		/// <summary>
		/// Handles it-click to freeze second col.
		/// </summary>
		/// <param name="sender"><c><see cref="it_freeze2"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Freeze 2nd col <c>[F6]</c></item>
		/// </list></remarks>
		void opsclick_Freeze2ndCol(object sender, EventArgs e)
		{
			Table.Select();

			it_freeze1.Checked = false;

			if (it_freeze2.Checked = !it_freeze2.Checked)
			{
				Col col = Table.Cols[FROZEN_COL_First];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_First);
				}

				col = Table.Cols[FROZEN_COL_Second];
				if (col.UserSized)
				{
					col.UserSized = false;
					Table.Colwidth(FROZEN_COL_Second);
				}

				Table.FrozenCount = YataGrid.FreezeSecond;
			}
			else
				Table.FrozenCount = YataGrid.FreezeId;
		}


		/// <summary>
		/// Toggles the <c><see cref="Propanel"/></c> of the current
		/// <c><see cref="Table"/></c> - creates a <c>Propanel</c> if
		/// required.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Propanel"/></c></item>
		/// <item><c><see cref="bu_Propanel"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Propanel <c>[F7]</c></item>
		/// <item><c><see cref="mouseup_buPropanel()">mouseup_buPropanel()</see></c></item>
		/// </list></remarks>
		void opsclick_Propanel(object sender, EventArgs e)
		{
			// TODO: hide Table._editor

			if (Table.Propanel == null
				|| (Table.Propanel.Visible = !Table.Propanel.Visible))
			{
				if (Table.Propanel == null)
					Table.Propanel = new Propanel(Table);
				else
					Table.Propanel.widthValcol();

				Table.Propanel.Show();
				Table.Propanel.BringToFront();

				Table.Propanel.Dockstate = Table.Propanel.Dockstate;

				it_Propanel       .Checked =
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = true;
			}
			else
			{
				Table.Propanel.Hide();

				it_Propanel       .Checked =
				it_PropanelLoc    .Enabled =
				it_PropanelLoc_pre.Enabled = false;
			}
		}

		/// <summary>
		/// Cycles the location of the <c><see cref="Propanel"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_PropanelLoc"/></c></item>
		/// <item><c><see cref="it_PropanelLoc_pre"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Propanel location<c>[F8]</c></item>
		/// <item>2daOps|Propanel location pre<c>[Shift+F8]</c></item>
		/// </list></remarks>
		/// <seealso cref="mouseup_buPropanel()"><c>mouseup_buPropanel()</c></seealso>
		void opsclick_PropanelLocation(object sender, EventArgs e)
		{
			// TODO: hide Table._editor

			if (Table.Propanel != null && Table.Propanel.Visible)
				Table.Propanel.Dockstate = Table.Propanel.getNextDockstate(sender == it_PropanelLoc_pre);
		}


		/// <summary>
		/// Starts an external diff/merger program with the two diffed files
		/// opened. Usually WinMerge although kdiff3 and perhaps other
		/// file-comparision utilities - their first 2 commandline arguments
		/// must be the fullpaths of the files to compare. If
		/// <c><see cref="_diff1"/></c> or <c><see cref="_diff2"/></c> doesn't
		/// exist then try to start the app with the current
		/// <c><see cref="Table"/></c> loaded.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ExternDiff"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|External diff <c>[Ctrl+m]</c></item>
		/// </list></remarks>
		void opsclick_ExternalDiff(object sender, EventArgs e)
		{
			if (File.Exists(Settings._diff))
			{
				var differ = new Process();
				differ.StartInfo.FileName = Settings._diff;

				if (_diff1 != null && _diff2 != null
					&& File.Exists(_diff1.Fullpath)
					&& File.Exists(_diff2.Fullpath))
				{
					differ.StartInfo.Arguments = " \"" + _diff1.Fullpath + "\" \"" + _diff2.Fullpath + "\"";
				}
				else
					differ.StartInfo.Arguments = " \"" + Table.Fullpath + "\"";

				differ.Start();
			}
		}


		/// <summary>
		/// Clears the Undo/Redo stacks.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ClearUr"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>2daOps|Clear undo/redo</item>
		/// </list></remarks>
		void opsclick_ClearUr(object sender, EventArgs e)
		{
			// after first run (clears ~300..500kb) this appears to clear
			// exactly 0 bytes per Clear.

			long bytes = GetUsage();

			Table._ur.Clear();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			bytes -= GetUsage();

			string head = "Estimated memory freed : " + String.Format(CultureInfo.InvariantCulture, "{0:n0}", bytes) + " bytes";
			using (var ib = new Infobox(Infobox.Title_infor, head))
				ib.ShowDialog(this);
		}
		#endregion Handlers (2daOps)


		#region Methods (2daOps)
		/// <summary>
		/// Determines the dis/enabled states of 2daOps operations.
		/// </summary>
		void Enable2daOperations()
		{
			it_OrderRows      .Enabled = !Table.Readonly;
			it_CheckRows      .Enabled =

			it_ColorRows      .Enabled =
			it_AutoCols       .Enabled = true;

			it_freeze1        .Enabled = Table.ColCount > 1;
			it_freeze2        .Enabled = Table.ColCount > 2;

			it_Propanel       .Enabled = true;
			it_PropanelLoc    .Enabled =
			it_PropanelLoc_pre.Enabled =
			it_Propanel       .Checked = Table.Propanel != null && Table.Propanel.Visible;

			it_ExternDiff     .Enabled = File.Exists(Settings._diff);
		}

		/// <summary>
		/// Gets Yata's current memory usage in bytes.
		/// </summary>
		/// <returns></returns>
		static long GetUsage()
		{
			long bytes;
			using (Process proc = Process.GetCurrentProcess())
			{
				// The proc.PrivateMemorySize64 will return the private memory usage in bytes.
				// - to convert to Megabytes divide it by 2^20
				bytes = proc.PrivateMemorySize64; // / (1024*1024);

//				using (var ib = new Infobox(" bytes used", String.Format("{0:n0}", bytes)))
//					ib.ShowDialog(this);
			}
			return bytes;
		}
		#endregion Methods (2daOps)


		#region Handlers (font)
		/// <summary>
		/// Handles opening the FontMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"><c><see cref="it_MenuFont"/></c></param>
		/// <param name="e"></param>
		void font_dropdownopening(object sender, EventArgs e)
		{
			it_FontDefault.Enabled = !it_Font.Checked
								  && !Font.Equals(FontDefault);
		}

		/// <summary>
		/// Opens the font-picker dialog - <c><see cref="FontDialog"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Font"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Font|Font...be patient</item>
		/// </list></remarks>
		void fontclick_Font(object sender, EventArgs e)
		{
			if (_ffont == null)
			{
				_ffont = new FontDialog(this);
				it_Font.Checked = true;
			}
			else
			{
				if (_ffont.WindowState == FormWindowState.Minimized)
				{
					if (_ffont.Maximized)
						_ffont.WindowState = FormWindowState.Maximized;
					else
						_ffont.WindowState = FormWindowState.Normal;
				}
				_ffont.BringToFront();
			}
		}

		/// <summary>
		/// Sets Yata's <c>Font</c> to <c><see cref="FontDefault"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_FontDefault"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Font|Load default font</item>
		/// </list>
		/// 
		/// 
		/// The item will be disabled if <c><see cref="FontDialog"/></c> is open
		/// or if Yata's current <c>Font</c> is
		/// <c><see cref="FontDefault"/></c>.</remarks>
		void fontclick_Default(object sender, EventArgs e)
		{
			doFont(FontDefault.Clone() as Font);
		}
		#endregion Handlers (font)


		#region Methods (font)
		/// <summary>
		/// Dechecks the "Font ... be patient" it when
		/// <c><see cref="FontDialog"/></c> closes.
		/// </summary>
		internal void CloseFontDialog()
		{
			_ffont = null;
			it_Font.Checked = false;
		}

		/// <summary>
		/// Applies a specified <c>Font</c> to Yata.
		/// </summary>
		/// <param name="font"></param>
		internal void doFont(Font font)
		{
			// NOTE: Cf f.AutoScaleMode (None,Font,DPI,Inherit)
			// Since I'm doing all the necessary scaling due to font-changes
			// w/ code the AutoScaleMode should not be set to default "Font".
			// It might better be set to "DPI" for those weirdos and I don't
			// know what "Inherit" means (other than the obvious).
			// AutoScaleMode is currently set to "None".
			//
			// See also SetProcessDPIAware()
			// NOTE: Apparently setting GraphicsUnit.Pixel when creating new
			// Font-objects effectively bypasses the OS's DPI user-setting.

			Font.Dispose();
			Font = font;

			Settings._fontdialog.Dispose();
			Settings._fontdialog = Settings.CreateDialogFont(Font);

			FontAccent.Dispose();
			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));


			YataGrid.SetStaticMetrics(this);

			if (Table != null)
			{
				Obfuscate();
				DrawRegulator.SuspendDrawing(Table);

				SetTabSize();

				YataGrid table;
				for (int tab = 0; tab != Tabs.TabCount; ++tab)
				{
					table = Tabs.TabPages[tab].Tag as YataGrid;
					table.CreateCols(true);
					table.Calibrate(0, table.RowCount - 1); // font

					// TODO: This is effed because the Height (at least) of each
					// table is not well-defined by .NET - OnResize() for the
					// tables gets called multiples times per table and the
					// value of Height changes arbitrarily. Since an accurate
					// Height is required by InitScrollers() a glitch occurs
					// when the height of Font increases. That is if a cell or
					// a row is currently selected it will NOT be fully
					// displayed if it is NOT on the currently displayed page
					// and it is near the bottom of the tab-control's page-area;
					// several pixels of the selected cell or row will still be
					// covered by the horizontal scroller. Given the arbitrary
					// Height changes that occur throughout this function's
					// sequence in fact it's surprising/remarkable that things
					// turn out even almost correct.
					// NOTE: Height of any table should NOT be changing at all.

					if (table == Table)
						table.Invalidator(table.EnsureDisplayed());
				}

				DrawRegulator.ResumeDrawing(Table);
				Obfuscate(false);

				if (_ffont != null)			// layout for big tables will send the Font dialog below the form ->
					_ffont.BringToFront();	// (although it should never be behind the form because its owner IS the form)
			}
		}
		#endregion Methods (font)


		#region Handlers (help)
		/// <summary>
		/// Handles it-click to open ReadMe.txt.
		/// </summary>
		/// <param name="sender"><c><see cref="it_ReadMe"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Help|ReadMe.txt <c>[F1]</c></item>
		/// </list></remarks>
		void helpclick_Help(object sender, EventArgs e)
		{
			string pfe = Path.Combine(Application.StartupPath, "ReadMe.txt");
			if (File.Exists(pfe))
				Process.Start(pfe);
			else
			{
				using (var ib = new Infobox(Infobox.Title_error,
											"ReadMe.txt was not found in the application directory.",
											null,
											InfoboxType.Error))
				{
					ib.ShowDialog(this);
				}
			}
		}

		/// <summary>
		/// Handles it-click to open the About box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_About"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Help|About</item>
		/// </list></remarks>
		void helpclick_About(object sender, EventArgs e)
		{
			using (var f = new About(this))
				f.ShowDialog(this);
		}


		/// <summary>
		/// Handles it-click to open the <c><see cref="SettingsEditor"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Settings"/></c></param>
		/// <param name="e"></param>
		void helpclick_Settings(object sender, EventArgs e)
		{
			if (_fsettings == null)
			{
				string pfe = Path.Combine(Application.StartupPath, Settings.FE);

				if (!File.Exists(pfe))
				{
					const string head = "a Settings.cfg file does not exist in the application directory. Do you want to create one ...";

					using (var ib = new Infobox(Infobox.Title_infor,
												head,
												null,
												InfoboxType.Info,
												InfoboxButtons.CancelYes))
					{
						if (ib.ShowDialog(this) == DialogResult.OK)
						{
							try
							{
								using (var sw = new StreamWriter(File.Open(pfe,
																		   FileMode.Create,
																		   FileAccess.Write,
																		   FileShare.None)))
								{
									sw.WriteLine("#Help|ReadMe.txt describes these settings.");

									if (Settings.options == null)
										Settings.CreateOptions();

									for (int i = 0; i != Settings.ids; ++i)
										sw.WriteLine(Settings.options[i]);
								}
							}
							catch (Exception ex)
							{
								using (var ibo = new Infobox(Infobox.Title_excep,
															"a Settings.cfg file could not be created in the application directory.",
															ex.ToString(),
															InfoboxType.Error))
								{
									ibo.ShowDialog(this);
								}
							}
						}
					}
				}

				if (File.Exists(pfe))
				{
					try
					{
						string[] lines = File.ReadAllLines(pfe);
						_fsettings = new SettingsEditor(this, lines);
						it_Settings.Checked = true;
					}
					catch (Exception ex)
					{
						// the stock MessageBox 'shall' be used if an exception is going to cause a CTD:
						// eg. a stock Font was disposed but the SettingsEditor needs it during its
						// initialization ... The app can't show a Yata-dialog in such a case; but the
						// stock MessageBox will pop up then ... CTD.

//						MessageBox.Show("The Settings.cfg file could not be read in the application directory."
//										+ Environment.NewLine + Environment.NewLine
//										+ ex);

						using (var ib = new Infobox(Infobox.Title_excep,
													"The Settings.cfg file could not be read in the application directory.",
													ex.ToString(),
													InfoboxType.Error))
						{
							ib.ShowDialog(this);
						}
					}
				}
			}
			else
			{
				if (_fsettings.WindowState == FormWindowState.Minimized)
				{
					if (_fsettings.Maximized)
						_fsettings.WindowState = FormWindowState.Maximized;
					else
						_fsettings.WindowState = FormWindowState.Normal;
				}
				_fsettings.BringToFront();
			}
		}
		#endregion Handlers (help)


		#region Methods (help)
		/// <summary>
		/// Clears the check on <c><see cref="it_Settings"/></c> and nulls
		/// <c><see cref="_fsettings"/></c> when the settings-editor closes.
		/// </summary>
		internal void CloseSettingsEditor()
		{
			_fsettings = null;
			it_Settings.Checked = false;
		}
		#endregion Methods (help)


		#region Methods (row)
		/// <summary>
		/// Shows the RMB-context on the rowhead for single-row edit operations.
		/// </summary>
		/// <param name="r"></param>
		/// <remarks><c><see cref="_contextRo"/></c> is not assigned to a
		/// <c><see cref="YataGrid"/>._panelRows'</c> <c>ContextMenuStrip</c>
		/// or <c>ContextMenu</c>.</remarks>
		internal void ShowRowContext(int r)
		{
			_r = r;

			rowit_Header.Text = "_row @ id " + _r;

			rowit_PasteAbove .Enabled =
			rowit_Paste      .Enabled =
			rowit_PasteBelow .Enabled = !Table.Readonly && _copyr.Count != 0;

			rowit_Cut        .Enabled =
			rowit_CreateAbove.Enabled =
			rowit_Clear      .Enabled =
			rowit_CreateBelow.Enabled =
			rowit_Delete     .Enabled = !Table.Readonly;

			Point loc;
			if (Settings._context)							// static location
			{
				loc = new Point(YataGrid.WidthRowhead,
								YataGrid.HeightColhead);
			}
			else											// cursor location
				loc = Table.PointToClient(Cursor.Position);

			_contextRo.Show(Table, loc);
		}
		#endregion Methods (row)


		#region Handlers (row)
		/// <summary>
		/// Handles context-click on the context-header.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Header"/></c></param>
		/// <param name="e"></param>
		void rowclick_Header(object sender, EventArgs e)
		{
			_contextRo.Hide();
		}

		/// <summary>
		/// Handles context-click to cut a row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Cut"/></c></param>
		/// <param name="e"></param>
		void rowclick_Cut(object sender, EventArgs e)
		{
			rowclick_Copy(  sender, e);
			rowclick_Delete(sender, e);
		}

		/// <summary>
		/// Handles context-click to copy a row and enables
		/// <c><see cref="it_PasteRange"/></c> and
		/// <c><see cref="it_ClipExport"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rowit_Copy"/></c></item>
		/// <item><c><see cref="rowit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void rowclick_Copy(object sender, EventArgs e)
		{
			_copyr.Clear();

			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = Table[_r,c].text;

			_copyr.Add(fields);

			if (_fclip != null)
				_fclip.SetRowsBufferText();

			it_PasteRange.Enabled = !Table.Readonly;
			it_ClipExport.Enabled = true;
		}

		/// <summary>
		/// Handles context-click to paste above the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_PasteAbove"/></c></param>
		/// <param name="e"></param>
		void rowclick_PasteAbove(object sender, EventArgs e)
		{
			Table.Insert(_r, _copyr[0]);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to paste into the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Paste"/></c></param>
		/// <param name="e"></param>
		void rowclick_Paste(object sender, EventArgs e)
		{
			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


			YataGrid._init = true; // bypass EnableGotoLoadchanged() in Cell.setter_loadchanged

			Row row = Table.Rows[_r];

			int c = 0;
			if (Settings._autorder)
			{
				row[c].text = _r.ToString(CultureInfo.InvariantCulture);
				row[c].diff =
				row[c].loadchanged = false;

				++c;
			}

			for (; c != Table.ColCount; ++c)
			{
				if (c < _copyr[0].Length)
					row[c].text = _copyr[0][c];
				else
					row[c].text = gs.Stars; // TODO: perhaps keep any remaining cells as they are.

				row[c].diff =
				row[c].loadchanged = false;
			}
			row._brush = Brushes.Created;

			YataGrid._init = false;
			EnableGotoLoadchanged(Table.anyLoadchanged());

			Table.Calibrate(_r);

			Table.Invalidator(YataGrid.INVALID_GRID);


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = Table.Rows[_r].Clone() as Row;
			Table._ur.Push(rest);
		}

		/// <summary>
		/// Handles context-click to paste below the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_PasteBelow"/></c></param>
		/// <param name="e"></param>
		void rowclick_PasteBelow(object sender, EventArgs e)
		{
			Table.Insert(_r + 1, _copyr[0]);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to create a row above the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_CreateAbove"/></c></param>
		/// <param name="e"></param>
		void rowclick_CreateAbove(object sender, EventArgs e)
		{
			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = gs.Stars;

			Table.Insert(_r, fields);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to clear the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_Clear"/></c></param>
		/// <param name="e"></param>
		void rowclick_Clear(object sender, EventArgs e)
		{
			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


			YataGrid._init = true; // bypass EnableGotoLoadchanged() in Cell.setter_loadchanged

			Row row = Table.Rows[_r];

			int c = 0;
			if (Settings._autorder)
			{
				row[c].text = _r.ToString(CultureInfo.InvariantCulture);
				row[c].diff =
				row[c].loadchanged = false;

				++c;
			}

			for (; c != Table.ColCount; ++c)
			{
				row[c].text = gs.Stars;
				row[c].diff =
				row[c].loadchanged = false;
			}
			row._brush = Brushes.Created;

			YataGrid._init = false;
			EnableGotoLoadchanged(Table.anyLoadchanged());

			Table.Calibrate(_r);

			Table.Invalidator(YataGrid.INVALID_GRID);


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = Table.Rows[_r].Clone() as Row;
			Table._ur.Push(rest);
		}

		/// <summary>
		/// Handles context-click to create a row below the current row.
		/// </summary>
		/// <param name="sender"><c><see cref="rowit_CreateBelow"/></c></param>
		/// <param name="e"></param>
		void rowclick_CreateBelow(object sender, EventArgs e)
		{
			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = gs.Stars;

			Table.Insert(_r + 1, fields);


			Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}

		/// <summary>
		/// Handles context-click to delete the current row.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rowit_Delete"/></c></item>
		/// <item><c><see cref="rowit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void rowclick_Delete(object sender, EventArgs e)
		{
			Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Insert);


			Table.Delete(_r);

			EnableRoweditOperations();


			if (!Table.Changed)
			{
				Table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}
			Table._ur.Push(rest);

			if (Settings._autorder && order() != 0) layout();
		}
		#endregion Handlers (row)


		#region Methods (cell)
		/// <summary>
		/// Shows the RMB-context on a cell for single-cell edit operations.
		/// </summary>
		/// <remarks><c><see cref="_contextCe"/></c> is not assigned to a
		/// <c><see cref="YataGrid">YataGrid's</see></c> <c>ContextMenuStrip</c>
		/// or <c>ContextMenu</c> since that leads to .net bullshit.</remarks>
		internal void ShowCellContext()
		{
			_sel = Table.getSelectedCell(); // '_sel' shall be valid due to rightclick

			cellit_Edit   .Enabled = !Table.Readonly;

			cellit_Cut    .Enabled = !Table.Readonly;
			cellit_Paste  .Enabled = !Table.Readonly;
			cellit_Clear  .Enabled = !Table.Readonly && (_sel.text != gs.Stars || _sel.loadchanged);

			cellit_Lower  .Enabled = !Table.Readonly && (_sel.text != _sel.text.ToLower(CultureInfo.CurrentCulture) || _sel.loadchanged);
			cellit_Upper  .Enabled = !Table.Readonly && (_sel.text != _sel.text.ToUpper(CultureInfo.CurrentCulture) || _sel.loadchanged);

			cellit_MergeCe.Enabled =
			cellit_MergeRo.Enabled = isMergeEnabled();

			cellit_Strref .Enabled = isStrrefEnabled();

			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_NONE:
				case YataGrid.InfoType.INFO_CRAFT:
					cellit_Input.Visible =
					cellit_Input.Enabled = false;
					break;

				// TODO: If table is Readonly allow viewing the InfoInput dialog
				// but disable its controls ->

				case YataGrid.InfoType.INFO_SPELL:
					cellit_Input.Text    = "InfoInput (spells.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isSpellsInfoInputCol();
					break;

				case YataGrid.InfoType.INFO_FEAT:
					cellit_Input.Text    = "InfoInput (feat.2da)";
					cellit_Input.Visible = true;
					cellit_Input.Enabled = !Table.Readonly && isFeatInfoInputCol();
					break;
			}

			_contextCe.Show(Table, Table.PointToClient(Cursor.Position));
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if merge operations (cell or row) will be
		/// enabled</returns>
		bool isMergeEnabled()
		{
			if (_sel.diff && _diff1 != null && _diff2 != null)
			{
				YataGrid table = null;
				if      (Table == _diff1) table = _diff2;
				else if (Table == _diff2) table = _diff1;

				return table != null && !table.Readonly
					&& table.ColCount > _sel.x
					&& table.RowCount > _sel.y;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// Spells.2da.</returns>
		bool isSpellsInfoInputCol()
		{
			switch (_sel.x)
			{
				case InfoInputSpells.School: // these don't rely on 2da-gropes ->
				case InfoInputSpells.Range:
				case InfoInputSpells.MetaMagic:
				case InfoInputSpells.TargetType:
				case InfoInputSpells.ImmunityType:
				case InfoInputSpells.UserType:
				case InfoInputSpells.AsMetaMagic:
					return true;

				case InfoInputSpells.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.SpontCastClass:
					if (Info.classLabels.Count != 0)
						return true;
					break;

				case InfoInputSpells.TargetingUI:
					if (Info.targetLabels.Count != 0)
						return true;
					break;
			}
			return false;
		}

		/// <summary>
		/// Helper for
		/// <c><see cref="ShowCellContext()">ShowCellContext()</see></c>.
		/// </summary>
		/// <returns><c>true</c> if the InfoInput operation will show for
		/// Feat.2da.</returns>
		bool isFeatInfoInputCol()
		{
			switch (_sel.x)
			{
				// "MINSPELLLVL"
				// "PREREQFEAT1"		info
				// "PREREQFEAT2"		info
				// "GAINMULTIPLE"
				// "EFFECTSSTACK"
				// "ALLCLASSESCANUSE"
				// "CATEGORY"			info + infoinput
				// "SPELLID"			info
				// "SUCCESSOR"			info
				// "USESMAPFEAT"
				// "MASTERFEAT"			info + infoinput
				// "TARGETSELF"
				// "OrReqFeat0"			info
				// "OrReqFeat1"			info
				// "OrReqFeat2"			info
				// "OrReqFeat3"			info
				// "OrReqFeat4"			info
				// "OrReqFeat5"			info
				// "REQSKILL"			info
				// "REQSKILL2"			info
				// "TOOLSCATEGORIES"	info + infoinput
				// "HostileFeat"
				// "MinLevelClass"
				// "PreReqEpic"
				// "FeatCategory"
				// "IsActive"
				// "IsPersistent"
				// "TogleMode"			info + infoinput
				// "DMFeat"
				// "REMOVED"
				// "AlignRestrict"
				// "ImmunityType"
				// "Instant"

				case InfoInputFeat.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.MasterFeat:
					if (Info.masterfeatLabels.Count != 0)
						return true;
					break;

				case InfoInputFeat.ToolsCategories: // this doesn't rely on 2da-gropes ->
					return true;

				case InfoInputFeat.ToggleMode:
					if (Info.combatmodeLabels.Count != 0)
						return true;
					break;
			}
			return false;
		}

		/// <summary>
		/// Checks if it's okay to print a TalkTable entry to the statusbar.
		/// </summary>
		/// <returns><c>true</c> if TalkEntry dialog will be enabled</returns>
		bool isStrrefEnabled()
		{
			if (_sel.x != 0 && Strrefheads.Contains(Table.Fields[_sel.x - 1]))
			{
				string strref = _sel.text;
				if (strref == gs.Stars) strref = "0";

				return Int32.TryParse(strref, out _strInt)
					&& _strInt >=  TalkReader.invalid
					&& _strInt <= (TalkReader.bitCusto | TalkReader.strref);
			}
			return false;
		}
		#endregion Methods (cell)


		#region Handlers (cell)
		/// <summary>
		/// Handles singlecell-click edit.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Edit"/></c></param>
		/// <param name="e"></param>
		void cellclick_Edit(object sender, EventArgs e)
		{
			Table.startCelledit(_sel);
		}

		/// <summary>
		/// Handles singlecell-click cut.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Cut"/></c></param>
		/// <param name="e"></param>
		void cellclick_Cut(object sender, EventArgs e)
		{
			cellclick_Copy(  sender, e);
			cellclick_Delete(sender, e);
		}

		/// <summary>
		/// Handles singlecell-click copy.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="cellit_Copy"/></c></item>
		/// <item><c><see cref="cellit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void cellclick_Copy(object sender, EventArgs e)
		{
			_copytext = new string[,] {{ _sel.text }};

			if (_fclip != null)
				_fclip.SetCellsBufferText();
		}

		/// <summary>
		/// Handles singlecell-click paste.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Paste"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Pass this on to the Cells|Paste handler since this shall
		/// allow pasting of a contiguous block.</remarks>
		void cellclick_Paste(object sender, EventArgs e)
		{
			if (_copytext.Length == 1)
			{
				if (_sel.text != _copytext[0,0])
					Table.ChangeCellText(_sel, _copytext[0,0]);
				else if (_sel.loadchanged)
					Table.ClearLoadchanged(_sel);
			}
			else
				editcellsclick_PasteCell(sender, e);
		}

		/// <summary>
		/// Handles singlecell-click delete.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="cellit_Clear"/></c></item>
		/// <item><c><see cref="cellit_Cut"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void cellclick_Delete(object sender, EventArgs e)
		{
			if (_sel.text != gs.Stars)
				Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check, does Invalidate
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
		}

		/// <summary>
		/// Handles singlecell-click lowercase.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Lower"/></c></param>
		/// <param name="e"></param>
		void cellclick_Lower(object sender, EventArgs e)
		{
			string text = _sel.text.ToLower(CultureInfo.CurrentCulture);
			if (_sel.text != text)
				Table.ChangeCellText(_sel, text); // does not do a text-check, does Invalidate
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
		}

		/// <summary>
		/// Handles singlecell-click uppercase.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Upper"/></c></param>
		/// <param name="e"></param>
		void cellclick_Upper(object sender, EventArgs e)
		{
			string text = _sel.text.ToUpper(CultureInfo.CurrentCulture);
			if (_sel.text != text)
				Table.ChangeCellText(_sel, text); // does not do a text-check, does Invalidate
			else if (_sel.loadchanged)
				Table.ClearLoadchanged(_sel);
		}

		/// <summary>
		/// Handles a single-cell merge operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_MergeCe"/></c></param>
		/// <param name="e"></param>
		void cellclick_MergeCe(object sender, EventArgs e)
		{
			YataGrid table;
			if (Table == _diff1) table = _diff2;
			else                 table = _diff1;

			int r = _sel.y;
			int c = _sel.x;

			Cell cell = table[r,c];
			table.ChangeCellText(cell, _sel.text); // does not do a text-check, does Invalidate

			_diff1[r,c].diff =
			_diff2[r,c].diff = false;
		}

		/// <summary>
		/// Handles a single-row merge operation.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_MergeRo"/></c></param>
		/// <param name="e"></param>
		void cellclick_MergeRo(object sender, EventArgs e)
		{
			YataGrid table;
			if (Table == _diff1) table = _diff2;
			else                 table = _diff1;

			int r = _sel.y;

			// - store the row's current state to 'rPre' in the Restorable
			Restorable rest = UndoRedo.createRow(table.Rows[r]);


			int c = 0;
			for (; c != table.ColCount && c != Table.ColCount; ++c)
			{
				table[r,c].text = Table[r,c].text;
				table[r,c].diff = false;

				Table[r,c].diff = false;
			}

			if (Settings._autorder)
				table[r,0].text = table[r,0].y.ToString(CultureInfo.InvariantCulture);	// not likely to happen. user'd have to load a table w/
																						// an out of order id then merge that row to another table.
			if (table.ColCount > Table.ColCount)
			{
				for (; c != table.ColCount; ++c)
				{
					table[r,c].text = gs.Stars;
					table[r,c].diff = false;
				}
			}
			else if (table.ColCount < Table.ColCount)
			{
				for (; c != Table.ColCount; ++c)
					Table[r,c].diff = false;
			}

			Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);

			// TODO: test if this funct needs to re-width a bunch of stuff


			if (!table.Changed)
			{
				table.Changed = true;
				rest.isSaved = UndoRedo.IsSavedType.is_Undo;
			}

			// - store the row's changed state to 'rPos' in the Restorable
			rest.rPos = table.Rows[r].Clone() as Row;
			table._ur.Push(rest);
		}


		/// <summary>
		/// Handles singlecell-click InfoInput dialog.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Input"/></c></param>
		/// <param name="e"></param>
		void cellclick_InfoInput(object sender, EventArgs e)
		{
			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_SPELL:
					switch (_sel.x)
					{
						case InfoInputSpells.School: // STRING Input ->
						case InfoInputSpells.Range:
						case InfoInputSpells.ImmunityType:
						case InfoInputSpells.UserType:
							using (var iis = new InfoInputSpells(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						case InfoInputSpells.MetaMagic: // HEX Input ->
						case InfoInputSpells.TargetType:
						case InfoInputSpells.AsMetaMagic:
							using (var iis = new InfoInputSpells(this, _sel))
							{
								if (iis.ShowDialog(this) == DialogResult.OK
									&& int1 != int0)
								{
									if (int1 == II_ASSIGN_STARS)
									{
										Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check
									}
									else
									{
										string q;
										if (int1 > 0xFF) q = "X6"; // is MetaMagic (invocation)
										else             q = "X2"; // is MetaMagic (standard) or TargetType

										Table.ChangeCellText(_sel, "0x" + int1.ToString(q, CultureInfo.InvariantCulture)); // does not do a text-check
									}
								}
							}
							break;

						case InfoInputSpells.Category: // INT Input ->
						case InfoInputSpells.SpontCastClass:
						case InfoInputSpells.TargetingUI:
							doIntInputSpells();
							break;
					}
					break;

				case YataGrid.InfoType.INFO_FEAT:
					switch (_sel.x)
					{
						case InfoInputFeat.Category: // INT Input ->
						case InfoInputFeat.MasterFeat:
						case InfoInputFeat.ToggleMode:
							doIntInputFeat();
							break;

						case InfoInputFeat.ToolsCategories:
							using (var iif = new InfoInputFeat(this, _sel))
							{
								if (iif.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;
					}
					break;
			}
		}

		/// <summary>
		/// - helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputSpells()
		{
			using (var iis = new InfoInputSpells(this, _sel))
				doIntInput(iis);
		}

		/// <summary>
		/// - helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputFeat()
		{
			using (var iif = new InfoInputFeat(this, _sel))
				doIntInput(iif);
		}

		/// <summary>
		/// Shows an InfoInput dialog and handles its return.
		/// </summary>
		/// <param name="dialog"></param>
		/// <remarks>- helper for
		/// <list type="bullet">
		/// <item><c><see cref="doIntInputSpells()">doIntInputSpells()</see></c></item>
		/// <item><c><see cref="doIntInputFeat()">doIntInputFeat()</see></c></item>
		/// </list></remarks>
		void doIntInput(Form dialog)
		{
			if (dialog.ShowDialog(this) == DialogResult.OK
				&& int1 != int0)
			{
				string val;
				if (int1 == II_ASSIGN_STARS) val = gs.Stars;
				else                         val = int1.ToString(CultureInfo.InvariantCulture);

				Table.ChangeCellText(_sel, val); // does not do a text-check
			}
		}


		/// <summary>
		/// Handler for the singlecell-context's subit "STRREF"
		/// <c>DropDownOpening</c> event.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref"/></c></param>
		/// <param name="e"></param>
		void dropdownopening_Strref(object sender, EventArgs e)
		{
			bool invalid = _strInt == TalkReader.invalid;

			if (invalid || (_strInt & TalkReader.bitCusto) == 0)
				cellit_Strref_custom.Text = "set Custom";
			else
				cellit_Strref_custom.Text = "clear Custom";

			cellit_Strref_custom .Enabled =
			cellit_Strref_invalid.Enabled = !Table.Readonly && !invalid;
		}

		/// <summary>
		/// Handler for singlecell-context "STRREF" click. Opens
		/// <c><see cref="TalkDialog"/></c> that displays the text's
		/// corresponding Dialog.Tlk or special entry in a readonly
		/// <c>RichTextBox</c> for the user's investigation and/or copying.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_talktable"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.</remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_talktable(object sender, EventArgs e)
		{
			_strref = _sel.text;

			using (var td = new TalkDialog(_sel, this))
			{
				if (td.ShowDialog(this) == DialogResult.OK
					&& _strref != _sel.text)
				{
					Table.ChangeCellText(_sel, _strref); // does not do a text-check
					Invalidate();	// lolziMScopter - else the titlebar and borders can arbitrarily disappear.
				}					// nobody knows why ... q TwilightZone
			}
		}

		/// <summary>
		/// Handler for singlecell-context "set/clear Custom" click. Toggles the
		/// custom-bit flag.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_custom"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.
		/// 
		/// 
		/// The invalid strref (-1) cannot be toggled.
		/// </remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_custom(object sender, EventArgs e)
		{
			if ((_strInt & TalkReader.bitCusto) != 0)
			{
				_strInt &= TalkReader.strref;	// knocks out 'bitCusto' and any MSB errors
			}
			else
			{
				_strInt &= TalkReader.strref;	// knocks out any MSB errors
				_strInt |= TalkReader.bitCusto;	// flags 'bitCusto'
			}

			Table.ChangeCellText(_sel, _strInt.ToString(CultureInfo.InvariantCulture)); // does not do a text-check
		}

		/// <summary>
		/// Handler for singlecell-context "set Invalid (-1)" click. Sets a
		/// strref to <c>-1</c> if not already.
		/// </summary>
		/// <param name="sender"><c><see cref="cellit_Strref_invalid"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Check that the cell's text parses to a valid value before
		/// allowing the event to trigger else disable the context it.</remarks>
		/// <seealso cref="ShowCellContext()"><c>ShowCellContext()</c></seealso>
		/// <seealso cref="dropdownopening_Strref()"><c>dropdownopening_Strref()</c></seealso>
		void cellclick_Strref_invalid(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, gs.Invalid); // does not do a text-check
		}
		#endregion Handlers (cell)


		#region Handlers (tab)
		/// <summary>
		/// Handles the <c>MouseClick</c> event on the <c>TabControl</c>. Shows
		/// <c><see cref="_contextTa"/></c> when a tab is rightclicked.
		/// </summary>
		/// <param name="sender"><c><see cref="Tabs"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Do not use the <c>TabControl's</c> <c>ContextMenuStrip</c>
		/// since a rightclick on any tabpage's <c><see cref="YataGrid"/></c>
		/// would cause
		/// <c><see cref="opening_TabContext()">opening_TabContext()</see></c>
		/// to fire. Although the context does not show rightclicks are used for
		/// other things by <c>YataGrid</c>.</remarks>
		void click_Tabs(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				_contextTa.Show(Tabs, e.Location);
		}


		/// <summary>
		/// Sets the selected tab when a right-click on a tab is about to open
		/// the context.
		/// </summary>
		/// <param name="sender"><c><see cref="_contextTa"/></c></param>
		/// <param name="e"></param>
		/// <remarks><c>_contextTa</c> is assigned to
		/// <c><see cref="Tabs"/>.ContextMenuStrip</c>.</remarks>
		void opening_TabContext(object sender, CancelEventArgs e)
		{
			bool found = false;

			Point loc = Tabs.PointToClient(Cursor.Position); // activate the Tab ->
			for (int tab = 0; tab != Tabs.TabCount; ++tab)
			{
				if (Tabs.GetTabRect(tab).Contains(loc))
				{
					Tabs.SelectedIndex = tab;
					found = true;
					break;
				}
			}

			if (found)
			{
				tabit_CloseAll      .Enabled =
				tabit_CloseAllOthers.Enabled = Tabs.TabCount != 1;

				tabit_Save          .Enabled = !Table.Readonly;

				tabit_Reload        .Enabled = File.Exists(Table.Fullpath);

				tabit_Diff2    .Enabled = _diff1 != null && _diff1 != Table;
				tabit_DiffReset.Enabled = _diff1 != null || _diff2 != null;
				tabit_DiffSync .Enabled = _diff1 != null && _diff2 != null;

				if (_diff1 != null)
					tabit_Diff1.Text = "diff1 - " + Path.GetFileNameWithoutExtension(_diff1.Fullpath);
				else
					tabit_Diff1.Text = "Select diff1";

				if (_diff2 != null)
					tabit_Diff2.Text = "diff2 - " + Path.GetFileNameWithoutExtension(_diff2.Fullpath);
				else
					tabit_Diff2.Text = "Select diff2";
			}
			else
				e.Cancel = true;
		}


		/// <summary>
		/// Closes all other tables when a tab's context-closeall item is
		/// clicked.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_CloseAllOthers"/></c></param>
		/// <param name="e"></param>
		void tabclick_CloseAllOtherTabs(object sender, EventArgs e)
		{
			if (!CancelChangedTables("close", true))
			{
				DrawRegulator.SuspendDrawing(this); // stops tab-flickering on Remove tab

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
				{
					if (tab != Tabs.SelectedIndex)
						ClosePage(Tabs.TabPages[tab]);
				}

				SetTabSize();

				it_SaveAll.Enabled = AllowSaveAll();

				DrawRegulator.ResumeDrawing(this);
			}
		}

		// TODO: FreezeFirst/Second, gotoloadchanged, etc.


		/// <summary>
		/// Selects <c><see cref="_diff1"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_Diff1"/></c></param>
		/// <param name="e"></param>
		void tabclick_Diff1(object sender, EventArgs e)
		{
			tabclick_DiffReset(sender, e);
			_diff1 = Table;
		}

		/// <summary>
		/// Selects <c><see cref="_diff2"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tabit_Diff2"/></c></param>
		/// <param name="e"></param>
		void tabclick_Diff2(object sender, EventArgs e)
		{
			if (_fdiffer != null) _fdiffer.Close();
			if (_diff2   != null) DiffReset(_diff2);

			_diff2 = Table;
			if (doDiff())
				tabclick_DiffSync(sender, e);
			else
				_diff1 = _diff2 = null;
		}

		/// <summary>
		/// Clears all diffed cells and nulls any pointers to diffed tables.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tabit_DiffReset"/></c></item>
		/// <item><c><see cref="DifferDialog"/>.btn_Reset</c></item>
		/// <item><c><see cref="tabit_Diff1"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>tab|Diff reset</item>
		/// <item><c><see cref="DifferDialog"/>.click_btnReset()</c></item>
		/// <item><c><see cref="tabclick_Diff1()">tabclick_Diff1()</see></c></item>
		/// <item><c><see cref="steadystate()">steadystate()</see></c></item>
		/// <item><c><see cref="YataGrid"/>.ColSort()</c></item>
		/// </list></remarks>
		internal void tabclick_DiffReset(object sender, EventArgs e)
		{
			if (_fdiffer != null) _fdiffer.Close();

			if (_diff1 != null)
			{
				DiffReset(_diff1);
				_diff1 = null;
			}

			if (_diff2 != null)
			{
				DiffReset(_diff2);
				_diff2 = null;
			}

			Table.Select();
		}

		/// <summary>
		/// Aligns the two diffed tables for easy switching back and forth.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="tabit_DiffSync"/></c></item>
		/// <item><c><see cref="tabit_Diff2"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>tab|Diff sync tables</item>
		/// <item><c><see cref="tabclick_Diff2()">tabclick_Diff2()</see></c></item>
		/// </list></remarks>
		void tabclick_DiffSync(object sender, EventArgs e)
		{
			int cols = Math.Min(_diff1.ColCount, _diff2.ColCount);
			int w1, w2;

			for (int c = 0; c != cols; ++c)
			{
				w1 = _diff1.Cols[c].width();
				w2 = _diff2.Cols[c].width();

				if      (w1 > w2) _diff2.Cols[c].width(w1, true);
				else if (w2 > w1) _diff1.Cols[c].width(w2, true);
			}

			cols = Math.Min(YataGrid.FreezeSecond, _diff1.ColCount);
			for (int c = 0; c != cols; ++c)
				_diff1.metricFrozenControls(c);

			cols = Math.Min(YataGrid.FreezeSecond, _diff2.ColCount);
			for (int c = 0; c != cols; ++c)
				_diff2.metricFrozenControls(c);

			_diff1._scrollVert.Value =
			_diff1._scrollHori.Value =
			_diff2._scrollVert.Value =
			_diff2._scrollHori.Value = 0; // keep it simple stupid.

			_diff1.InitScroll();
			_diff2.InitScroll();

			YataGrid table;
			if      (_diff1 == Table) table = _diff1;
			else if (_diff2 == Table) table = _diff2;
			else                      table = null;

			if (table != null)
				table.Invalidator(YataGrid.INVALID_GRID);
		}
		#endregion Handlers (tab)


		#region Methods (tab)
		/// <summary>
		/// Helper for
		/// <c><see cref="tabclick_DiffReset()">tabclick_DiffReset()</see></c>.
		/// </summary>
		/// <param name="table">a <c><see cref="YataGrid"/></c></param>
		/// <remarks>Check that <paramref name="table"/> is not null before
		/// call.</remarks>
		void DiffReset(YataGrid table)
		{
			for (int r = 0; r != table.RowCount; ++r)
			for (int c = 0; c != table.ColCount; ++c)
			{
				table[r,c].diff = false;
			}

			if (table == Table)
				opsclick_AutosizeCols(null, EventArgs.Empty);
			else
				AutosizeCols(table);
		}

		/// <summary>
		/// The yata-diff routine.
		/// </summary>
		/// <returns><c>true</c> if differences are found</returns>
		bool doDiff()
		{
			_diff1.ClearSelects(true, true);	// sync table
			_diff2.ClearSelects();				// currently active table


			bool isDiff = false;

			string copyable = String.Empty;

			int fields1 = _diff1.Fields.Length;				// check colhead count ->
			int fields2 = _diff2.Fields.Length;
			if (fields1 != fields2)
			{
				isDiff = true;
				copyable = "Head count: (a) " + fields1 + "  (b) " + fields2;
			}

			int fields = Math.Min(fields1, fields2);		// diff fields ->
			for (int f = 0; f != fields; ++f)
			{
				if (_diff1.Fields[f] != _diff2.Fields[f])
				{
					isDiff = true;
					if (copyable.Length != 0)
						copyable += Environment.NewLine;

					copyable += "Head #" + f + ": (a) " + _diff1.Fields[f] + "  (b) " + _diff2.Fields[f];
				}
			}


			bool prelinedone = false;

			int cols1 = _diff1.ColCount;					// check col count ->
			int cols2 = _diff2.ColCount;
			if (cols1 != cols2)
			{
				isDiff = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine + Environment.NewLine;
					prelinedone = true;
				}
				copyable += "Col count: (a) " + cols1 + "  (b) " + cols2;
			}

			int rows1 = _diff1.RowCount;					// check row count ->
			int rows2 = _diff2.RowCount;
			if (rows1 != rows2)
			{
				isDiff = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine;
					if (!prelinedone)
						copyable += Environment.NewLine;
				}
				copyable += "Row count: (a) " + rows1 + "  (b) " + rows2;
			}


			prelinedone = false;

			int celldiffs = 0;

			int cols = Math.Min(cols1, cols2);				// diff cells ->
			int rows = Math.Min(rows1, rows2);
			for (int r = 0; r != rows; ++r)
			for (int c = 0; c != cols; ++c)
			{
				if (_diff1[r,c].text != _diff2[r,c].text)
				{
					++celldiffs;

					_diff1[r,c].diff =
					_diff2[r,c].diff = true;
				}
			}

			bool @goto = false;
			if (celldiffs != 0)
			{
				@goto = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine + Environment.NewLine;
					prelinedone = true;
				}
				copyable += "Cell texts: " + celldiffs + " (inclusive)";
			}

			celldiffs = 0;

			if (cols1 > cols2)								// diff cols of the wider table ->
			{
				for (int c = cols2; c != cols1; ++c)
				for (int r = 0;     r != rows1; ++r)
				{
					++celldiffs;
					_diff1[r,c].diff = true;
				}
			}
			else if (cols2 > cols1)
			{
				for (int c = cols1; c != cols2; ++c)
				for (int r = 0;     r != rows2; ++r)
				{
					++celldiffs;
					_diff2[r,c].diff = true;
				}
			}

			if (rows1 > rows2)								// diff rows of the longer table ->
			{
				for (int c = 0;     c != cols;  ++c)
				for (int r = rows2; r != rows1; ++r)
				{
					++celldiffs;
					_diff1[r,c].diff = true;
				}
			}
			else if (rows2 > rows1)
			{
				for (int c = 0;     c != cols;  ++c)
				for (int r = rows1; r != rows2; ++r)
				{
					++celldiffs;
					_diff2[r,c].diff = true;
				}
			}

			if (celldiffs != 0)
			{
				@goto = true;
				if (copyable.Length != 0)
				{
					copyable += Environment.NewLine;
					if (!prelinedone)
						copyable += Environment.NewLine;
				}
				copyable += "Cell texts: " + celldiffs + " (exclusive)";
			}


			string label;
			Color color;
			if (copyable.Length == 0)
			{
				label = "Tables are identical.";
				color = Colors.Text;
			}
			else
			{
				label = "Tables are different.";
				color = Color.Firebrick;
			}

			string title = " diff (a) "
						 + Path.GetFileNameWithoutExtension(_diff1.Fullpath)
						 + " - (b) "
						 + Path.GetFileNameWithoutExtension(_diff2.Fullpath);

			_fdiffer = new DifferDialog(title,
										label,
										copyable,
										this,
										color,
										@goto,
										@goto || isDiff);
			return isDiff || @goto;
		}

		/// <summary>
		/// Selects the next diffed cell in the table (or both tables if both
		/// are valid).
		/// </summary>
		/// <remarks>Frozen cells will be selected but they don't respect
		/// <c><see cref="YataGrid.EnsureDisplayed()">YataGrid.EnsureDisplayed()</see></c>.
		/// They get no respect ...
		/// 
		/// 
		/// Do not focus <c><see cref="YataGrid"/></c> if <c>[Ctrl]</c>
		/// is depressed.</remarks>
		internal void GotoDiffCell()
		{
			if ((ModifierKeys & Keys.Alt) == Keys.None)
			{
				if (WindowState == FormWindowState.Minimized)
					WindowState  = FormWindowState.Normal;
				else
				{
					TopMost = true;
					TopMost = false;
				}

				bool bypassFocus = (ModifierKeys & Keys.Control) == Keys.Control;
				if (bypassFocus) _fdiffer.Activate();


				if (Table != null
					&& (_diff1 != null  || _diff2 != null)
					&& (_diff1 == Table || _diff2 == Table))
				{
					if (!bypassFocus) Table.Select();


					YataGrid table; // the other table - can be null.

					if (Table == _diff1) table = _diff2;
					else                 table = _diff1;

					Cell sel = Table.getSelectedCell();
					int rStart = Table.getSelectedRow();

					Table.ClearSelects();

					if (table != null)
						table.ClearSelects(true, true);

					int r,c;

					bool start = true;

					if ((ModifierKeys & Keys.Shift) == Keys.None) // forward goto ->
					{
						if (sel != null) { c = sel.x; rStart = sel.y; }
						else
						{
							c = -1;
							if (rStart == -1) rStart = 0;
						}

						for (r = rStart; r != Table.RowCount; ++r)
						{
							if (start)
							{
								start = false;
								if (++c == Table.ColCount)		// if starting on the last cell of a row
								{
									c = 0;

									if (r < Table.RowCount - 1)	// jump to the first cell of the next row
										++r;
									else						// or to the top of the table if on the last row
										r = 0;
								}
							}
							else
								c = 0;

							for (; c != Table.ColCount; ++c)
							{
								if ((sel = Table[r,c]).diff)
								{
									gotodiff(sel, table);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != rStart + 1;     ++r) // quick and dirty wrap ->
						for (c = 0; c != Table.ColCount; ++c)
						{
							if ((sel = Table[r,c]).diff)
							{
								gotodiff(sel, table);
								return;
							}
						}
					}
					else // backward goto ->
					{
						if (sel != null) { c = sel.x; rStart = sel.y; }
						else
						{
							c = Table.ColCount;
							if (rStart == -1) rStart = Table.RowCount - 1;
						}

						for (r = rStart; r != -1; --r)
						{
							if (start)
							{
								start = false;
								if (--c == -1)	// if starting on the first cell of a row
								{
									c = Table.ColCount - 1;

									if (r > 0)	// jump to the last cell of the previous row
										--r;
									else		// or to the bottom of the table if on the first row
										r = Table.RowCount - 1;
								}
							}
							else
								c = Table.ColCount - 1;

							for (; c != -1; --c)
							{
								if ((sel = Table[r,c]).diff)
								{
									gotodiff(sel, table);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = Table.RowCount - 1; r != rStart - 1; --r) // quick and dirty wrap ->
						for (c = Table.ColCount - 1; c != -1;         --c)
						{
							if ((sel = Table[r,c]).diff)
							{
								gotodiff(sel, table);
								return;
							}
						}
					}
				}
			}
			_fdiffer.EnableGotoButton(false);
		}

		/// <summary>
		/// Helper for <c><see cref="GotoDiffCell()">GotoDiffCell()</see></c>.
		/// </summary>
		/// <param name="sel">a <c><see cref="Cell"/></c> in the current table</param>
		/// <param name="table">the other <c><see cref="YataGrid"/></c></param>
		static void gotodiff(Cell sel, YataGrid table)
		{
			Table.SelectCell(sel, false);

			if (table != null
				&& sel.y < table.RowCount
				&& sel.x < table.ColCount)
			{
				table[sel.y, sel.x].selected = true;
			}
		}


		/// <summary>
		/// Clears all selects on a sync-table if that table is valid.
		/// </summary>
		/// <returns>the sync'd <c><see cref="YataGrid"/></c> if a sync-table is
		/// valid</returns>
		internal YataGrid ClearSyncSelects()
		{
			YataGrid table;
			if      (Table == _diff1) table = _diff2;
			else if (Table == _diff2) table = _diff1;
			else
				return null;

			if (table != null)
				table.ClearSelects(true, true);

			return table;
		}
		#endregion Methods (tab)


		#region Methods (statusbar)
		/// <summary>
		/// Mouseover cells prints table-cords plus PathInfo to the statusbar if
		/// a relevant 2da (ie. Crafting, Spells, Feat) is loaded.
		/// </summary>
		/// <param name="cords"><c>null</c> to clear statusbar-cords and
		/// -pathinfo</param>
		internal void PrintInfo(Point? cords = null)
		{
			if (cords != null && Table != null) // else CloseAll can throw on invalid object.
			{
				var cord = (Point)cords;
				int c = cord.X;
				int r = cord.Y;

				if (r < Table.RowCount && c < Table.ColCount) // NOTE: mouseover pos can register in the scrollbars
				{
					if (c != _track_x || r != _track_y)
					{
						_track_x = c; _track_y = r;

						statbar_lblCords.Text = " id= " + r + "  col= " + c;

						if (c != 0 && Strrefheads.Contains(Table.Fields[c - 1]))
						{
							string text = null;

							string strref = Table[r,c].text;
							if (strref == gs.Stars) strref = "0";

							int result;
							if (Int32.TryParse(strref, out result)
								&& result >=  TalkReader.invalid
								&& result <= (TalkReader.bitCusto | TalkReader.strref))
							{
								if (result != TalkReader.invalid)
								{
									bool alt = ((result & TalkReader.bitCusto) != 0);
									result &= TalkReader.strref;

									if (!alt)
									{
										if (TalkReader.DictDialo.ContainsKey(result))
											text = TalkReader.DictDialo[result];
									}
									else if (TalkReader.DictCusto.ContainsKey(result))
										text = TalkReader.DictCusto[result];
								}
								else
									text = gs.Space;
							}

							if (!String.IsNullOrEmpty(text))
							{
								string[] array = text.Split(gs.CRandorLF, StringSplitOptions.None);

								text = array[0];
								if      (text .Length > 99) text = text.Substring(0, 99) + " ...";
								else if (array.Length >  1) text += " ...";

								statbar_lblInfo.Text = text;
							}
							else
								statbar_lblInfo.Text = gs.non;
						}
						else
						{
							switch (Table.Info)
							{
								case YataGrid.InfoType.INFO_CRAFT:
									statbar_lblInfo.Text = getCraftInfo(r,c);
									break;
								case YataGrid.InfoType.INFO_SPELL:
									statbar_lblInfo.Text = getSpellInfo(r,c);
									break;
								case YataGrid.InfoType.INFO_FEAT:
									statbar_lblInfo.Text = getFeatInfo(r,c);
									break;

								default:
									statbar_lblInfo.Text = String.Empty;
									break;
							}
						}
					}
					return;
				}
			}

			_track_x = _track_y = -1;
			statbar_lblCords.Text =
			statbar_lblInfo .Text = String.Empty;
		}
		#endregion Methods (statusbar)


		#region Handlers (dragdrop)
		/// <summary>
		/// Handles dragging a file onto Yata.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="drgevent"></param>
		internal void yata_DragEnter(object sender, DragEventArgs drgevent)
		{
			if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
				drgevent.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Handles dropping a file(s) onto Yata.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="drgevent"></param>
		internal void yata_DragDrop(object sender, DragEventArgs drgevent)
		{
			var paths = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
			foreach (string pfe in paths)
				CreatePage(pfe); // load 1+ files by drag&drop
		}
		#endregion Handlers (dragdrop)


		#region Handlers (propanel)
		/// <summary>
		/// Handler for <c>MouseDown</c> on the <c><see cref="PropanelBu"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Propanel"/></c></param>
		/// <param name="e"></param>
		/// <remarks><c>bu_Propanel</c> is not visible if
		/// <c><see cref="Table"/></c> is invalid</remarks>
		void mousedown_buPropanel(object sender, MouseEventArgs e)
		{
			if (    e.Button == MouseButtons.Left
				|| (e.Button == MouseButtons.Right
					&& Table.Propanel != null && Table.Propanel.Visible))
			{
				bu_Propanel.depressed(true);
			}
		}

		/// <summary>
		/// Handler for <c>MouseUp</c> on the <c><see cref="PropanelBu"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Propanel"/></c></param>
		/// <param name="e"></param>
		/// <remarks><c>bu_Propanel</c> is not visible if
		/// <c><see cref="Table"/></c> is invalid</remarks>
		/// <seealso cref="opsclick_Propanel()"><c>opsclick_Propanel()</c></seealso>
		/// <seealso cref="opsclick_PropanelLocation()"><c>opsclick_PropanelLocation()</c></seealso>
		void mouseup_buPropanel(object sender, MouseEventArgs e)
		{
			Table.Select();

			switch (e.Button)
			{
				case MouseButtons.Left:
					bu_Propanel.depressed(false);
					opsclick_Propanel(sender, e);
					break;

				case MouseButtons.Right:
					if (Table.Propanel != null && Table.Propanel.Visible)
					{
						bu_Propanel.depressed(false);
						Table.Propanel.Dockstate = Table.Propanel.getNextDockstate((ModifierKeys & Keys.Shift) == Keys.Shift);
					}
					break;
			}
		}
		#endregion Handlers (propanel)
	}


	#region Delegates
	/// <summary>
	/// Good fuckin Lord I just wrote a "DontBeep" delegate.
	/// </summary>
	internal delegate void DontBeepEventHandler();
	#endregion Delegates
}
