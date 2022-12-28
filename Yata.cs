using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
	public sealed partial class Yata
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
		internal static Yata that;

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
		readonly PropanelButton bu_Propanel = new PropanelButton();

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


		internal int _startCr, _lengthCr;
		internal CrFillType _fillCr;

		/// <summary>
		/// The <c><see cref="FontDialog"/></c> font-picker.
		/// </summary>
		FontDialog _ffont;

		Font FontDefault;
		internal Font FontAccent;


		internal DifferDialog _fdiffer;
		internal YataGrid _diff1, _diff2;

		internal ReplaceTextDialog _replacer;

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
		/// String-input for InfoInputSpells or InfoInputFeat or
		/// InfoInputClasses (re PathInfo). <c>str0</c> is the current value;
		/// <c>str1</c> will be the user-chosen value that's assigned on Accept.
		/// </summary>
		internal string str0, str1;

		/// <summary>
		/// Int-input for InfoInputSpells or InfoInputFeat or InfoInputClasses
		/// (re PathInfo). <c>int0</c> is the current value; <c>int1</c> will be
		/// the user-chosen value that's assigned on Accept.
		/// </summary>
		internal int int0, int1;

		// NOTE: These are to initialize 'int0' and 'int1' and need to be
		// different to recognize that an invalid current value should be
		// changed to stars (iff the user accepts the dialog).
		internal const int Info_INIT_INVALID = -2; // for 'int0' only
		internal const int Info_ASSIGN_STARS = -1; // for 'int1' or 'int0'


		/// <summary>
		/// Works in conjunction w/
		/// <c><see cref="YataGrid"></see>.OnResize()</c>.
		/// </summary>
		internal bool IsMin;

		internal int _track_x = -1; // tracks last mouseover coords ->
		internal int _track_y = -1;

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


		/// <summary>
		/// Stores the previously focused <c>TabPage</c>.
		/// </summary>
		TabPage _lastpage;
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
		internal Yata()
		{
//			Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

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


			cb_SearchOption.Items.AddRange(new []
			{
				"subfield",
				"wholefield"
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
			TalkReader.Load(Settings._dialogalt, it_PathTalkC);

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
			if (Table != null && !_isCreate)
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
			//logfile.Log("Yata.VerifyCurrentFileState()");

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
		#endregion Handlers (override)


		#region Handlers (override - Receive Message - PfeLoad arg)
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
		#endregion Handlers (override - Receive Message - PfeLoad arg)


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

		/// <summary>
		/// Caches the previously focused <c>TabPage</c>.
		/// </summary>
		/// <param name="page"></param>
		internal void SetLastPage(TabPage page)
		{
			_lastpage = page;
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
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>This should be bypassed when a page other than tabid #0 is
		/// active and user closes all other tabs - if tabid #0 is already
		/// active the selected-id does not change (but the event fires anyway).
		/// 
		/// 
		/// Invoked by
		/// <list type="bullet">
		/// <item>tabpage change</item>
		/// <item>close table</item>
		/// <item><c><see cref="CreatePage()">CreatePage()</see></c></item>
		/// <item><c><see cref="fileclick_Create()">fileclick_Create()</see></c></item>
		/// </list></remarks>
		void tab_SelectedIndexChanged(object sender, EventArgs e)
		{
			//logfile.Log("Yata.tab_SelectedIndexChanged() id= " + Tabs.SelectedIndex);

			bool replaced;

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

				visPath_its();

				bu_Propanel           .Visible = true;

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
				it_GotoReplaced       .Enabled =
				it_GotoReplaced_pre   .Enabled =
				it_ClearReplaced      .Enabled = (replaced = Table.anyReplaced());
				it_GotoLoadchanged    .Enabled =
				it_GotoLoadchanged_pre.Enabled =
				it_ClearLoadchanged   .Enabled = Table.anyLoadchanged();
				it_Defaultval         .Enabled = true;
				it_Defaultclear       .Enabled = Table._defaultval.Length != 0;

				EnableCelleditOperations();
				EnableRoweditOperations();
				Enable2daOperations();


				if (Table.Propanel != null && Table.Propanel.Visible)
				{
					Table.Propanel.telemetric();

					Cell sel = Table.getSelectedCell();
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
				it_GotoReplaced       .Enabled =
				it_GotoReplaced_pre   .Enabled =
				it_ClearReplaced      .Enabled =
				it_GotoLoadchanged    .Enabled =
				it_GotoLoadchanged_pre.Enabled =
				it_ClearLoadchanged   .Enabled =
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

				replaced = false;
			}

			SetTitlebarText();

			if (_replacer != null)
				_replacer.EnableOps(replaced);
		}


		ToolStripSeparator _tsep = new ToolStripSeparator(); // TODO: dispose <-
		ToolStripSeparator _tsepcraft;

		/// <summary>
		/// Deters what its on the Paths menu should show and hide.
		/// </summary>
		void visPath_its()
		{
			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_NONE:
					if (_tsepcraft != null) _tsepcraft.Dispose();

					it_MenuPaths.Visible = false;
					break;

				case YataGrid.InfoType.INFO_CRAFT:
					if (_tsepcraft == null || _tsepcraft.IsDisposed)
						_tsepcraft = new ToolStripSeparator(); // TODO: dispose <-

					it_MenuPaths.DropDownItems.Clear();

					it_MenuPaths.DropDownItems.Add(it_PathAll);
					it_MenuPaths.DropDownItems.Add(_tsep);
					it_MenuPaths.DropDownItems.Add(it_PathBaseItems2da);
					it_MenuPaths.DropDownItems.Add(it_PathFeat2da);
					it_MenuPaths.DropDownItems.Add(it_PathItemPropDef2da);
					it_MenuPaths.DropDownItems.Add(it_PathSkills2da);
					it_MenuPaths.DropDownItems.Add(it_PathSpells2da);
					it_MenuPaths.DropDownItems.Add(_tsepcraft);
					it_MenuPaths.DropDownItems.Add(it_PathClasses2da);
					it_MenuPaths.DropDownItems.Add(it_PathDisease2da);
					it_MenuPaths.DropDownItems.Add(it_PathIprpAmmoCost2da);
					it_MenuPaths.DropDownItems.Add(it_PathIprpFeats2da);
					it_MenuPaths.DropDownItems.Add(it_PathIprpOnHitSpell2da);
					it_MenuPaths.DropDownItems.Add(it_PathIprpSpells2da);
					it_MenuPaths.DropDownItems.Add(it_PathRaces2da);

					it_MenuPaths.Visible = true;
					break;

				case YataGrid.InfoType.INFO_SPELL:
					if (_tsepcraft != null) _tsepcraft.Dispose();

					it_MenuPaths.DropDownItems.Clear();

					it_MenuPaths.DropDownItems.Add(it_PathAll);
					it_MenuPaths.DropDownItems.Add(_tsep);
					it_MenuPaths.DropDownItems.Add(it_PathCategories2da);
					it_MenuPaths.DropDownItems.Add(it_PathClasses2da);
					it_MenuPaths.DropDownItems.Add(it_PathFeat2da);
					it_MenuPaths.DropDownItems.Add(it_PathRanges2da);
					it_MenuPaths.DropDownItems.Add(it_PathSpells2da);
					it_MenuPaths.DropDownItems.Add(it_PathSpellTarget2da);

					it_MenuPaths.Visible = true;
					break;

				case YataGrid.InfoType.INFO_FEAT:
					if (_tsepcraft != null) _tsepcraft.Dispose();

					it_MenuPaths.DropDownItems.Clear();

					it_MenuPaths.DropDownItems.Add(it_PathAll);
					it_MenuPaths.DropDownItems.Add(_tsep);
					it_MenuPaths.DropDownItems.Add(it_PathCategories2da);
					it_MenuPaths.DropDownItems.Add(it_PathClasses2da);
					it_MenuPaths.DropDownItems.Add(it_PathCombatModes2da);
					it_MenuPaths.DropDownItems.Add(it_PathFeat2da);
					it_MenuPaths.DropDownItems.Add(it_PathMasterFeats2da);
					it_MenuPaths.DropDownItems.Add(it_PathSkills2da);
					it_MenuPaths.DropDownItems.Add(it_PathSpells2da);

					it_MenuPaths.Visible = true;
					break;

				case YataGrid.InfoType.INFO_CLASS:
					if (_tsepcraft != null) _tsepcraft.Dispose();

					it_MenuPaths.DropDownItems.Clear();

					it_MenuPaths.DropDownItems.Add(it_PathAll);
					it_MenuPaths.DropDownItems.Add(_tsep);
					it_MenuPaths.DropDownItems.Add(it_PathFeat2da);
					it_MenuPaths.DropDownItems.Add(it_PathPackages2da);

					it_MenuPaths.Visible = true;
					break;

				case YataGrid.InfoType.INFO_ITEM:
					if (_tsepcraft != null) _tsepcraft.Dispose();

					it_MenuPaths.DropDownItems.Clear();

					it_MenuPaths.DropDownItems.Add(it_PathAll);
					it_MenuPaths.DropDownItems.Add(_tsep);
					it_MenuPaths.DropDownItems.Add(it_PathBaseItems2da);
					it_MenuPaths.DropDownItems.Add(it_PathFeat2da);
					it_MenuPaths.DropDownItems.Add(it_PathInventorySnds2da);
					it_MenuPaths.DropDownItems.Add(it_PathItemProps2da);
					it_MenuPaths.DropDownItems.Add(it_PathWeaponSounds2da);
					it_MenuPaths.DropDownItems.Add(it_PathAmmunitionTypes2da);

					it_MenuPaths.Visible = true;
					break;
			}
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

			YataGrid.MetricStaticHeads(this);
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


		#region Methods (statusbar)
		/// <summary>
		/// Mouseover cells prints table-cords plus PathInfo to the statusbar if
		/// a relevant 2da (ie. Crafting, Spells, Feat) is loaded.
		/// </summary>
		/// <param name="cords"><c>null</c> to clear statusbar-cords and
		/// -pathinfo</param>
		/// <param name="force"><c>true</c> to force reprint even if
		/// <c><see cref="_track_x"/></c> and/or <c><see cref="_track_y"/></c>
		/// have not changed</param>
		internal void PrintInfo(Point? cords = null, bool force = false)
		{
			if (cords != null && Table != null) // else CloseAll can throw on invalid object.
			{
				var cord = (Point)cords;
				int c = cord.X;
				int r = cord.Y;

				if (r < Table.RowCount && c < Table.ColCount) // NOTE: mouseover pos can register in the scrollbars
				{
					if (c != _track_x || r != _track_y || force)
					{
						_track_x = c; _track_y = r;

						statbar_lblCords.Text = " id= " + r + "  col= " + c;

						if (c > 0 && Strrefheads.Contains(Table.Fields[c - 1])) // NOTE: 'c' can be -1
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
								case YataGrid.InfoType.INFO_CLASS:
									statbar_lblInfo.Text = getClassInfo(r,c);
									break;
								case YataGrid.InfoType.INFO_ITEM:
									statbar_lblInfo.Text = getBaseitemInfo(r,c);
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
		/// Handler for <c>MouseDown</c> on the
		/// <c><see cref="PropanelButton"/></c>.
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
		/// Handler for <c>MouseUp</c> on the
		/// <c><see cref="PropanelButton"/></c>.
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
