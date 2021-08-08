using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
	sealed partial class YataForm
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

		internal static string pfe_load; // cl arg

		internal static bool IsSaveAll;

		static Graphics graphics;
		#endregion Fields (static)


		#region Fields
		readonly PropertyPanelButton btn_ProPanel = new PropertyPanelButton();

		/// <summary>
		/// A 2d-array of strings used for copy/paste cell.
		/// </summary>
		/// <remarks>A cell's text shall never be null or blank, therefore
		/// <c>_copytext</c> shall never be null or blank.</remarks>
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
		/// A list used for copy/paste row(s).
		/// </summary>
		List<string[]> _copyr = new List<string[]>();

		/// <summary>
		/// A list used for copy/paste col.
		/// </summary>
		List<string> _copyc = new List<string>();


		string _preset = String.Empty;

		internal int _startCr, _lengthCr;
		internal CrFillType _fillCr;

		internal Font FontAccent;

		internal bool _isEnterkeyedSearch;
		bool _firstclick; // preps the Search or Goto textboxes to select all text

		int _dontbeep; // directs keydown [Enter] to the appropriate funct: Goto or Search
		const int DONTBEEP_DEFAULT = 0;
		const int DONTBEEP_GOTO    = 1;
		const int DONTBEEP_SEARCH  = 2;

		internal DifferDialog _fdiffer;
		internal YataGrid _diff1, _diff2;

		/// <summary>
		/// Caches a fullpath when doing SaveAs.
		/// So that the Table's new path-variables don't get assigned unless the
		/// save is successful - ie. verify several conditions first.
		/// </summary>
		string _pfeT = String.Empty;

		/// <summary>
		/// A pointer to a <c><see cref="YataGrid"/></c> table that will be used
		/// during the save-routine (and other places where it's convenient). Is
		/// required because it can't be assumed that the current
		/// <c><see cref="Table"/></c> is the table being saved; that is, the
		/// SaveAll operation needs to cycle through all tables.
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
		/// <c>System.Threading.Timer</c>.</remarks>
		System.Windows.Forms.Timer _t1 = new System.Windows.Forms.Timer();
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

		Font FontDefault
		{ get; set; }

		FontF _fontF
		{ get; set; }
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

			// init 'btn_ProPanel' ->
			btn_ProPanel.MouseDown += mousedown_btnPropertyPanel;
			btn_ProPanel.MouseUp   += mouseup_btnPropertyPanel;

			Controls.Add(btn_ProPanel);

			InitializeComponent();

			Tabs.ContextMenuStrip = tabMenu;

//			DrawingControl.SetDoubleBuffered(this);
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
				// Relative Font-sizes (as defined in the Designer):
				//
				// menubar, ContextEditor, statusbar, tabMenu, cellMenu = unity.
				// context_it_Header     = +0.75
				// statusbar_label_Cords = -0.75
				// statusbar_label_Info  = +1.50

				menubar.Font.Dispose();
				menubar.Font = Settings._font2;

				ContextEditor.Font.Dispose();
				ContextEditor.Font = Settings._font2;

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


				context_it_Header.Font.Dispose();
				context_it_Header.Font = new Font(Settings._font2.FontFamily,
												  Settings._font2.SizeInPoints + 0.75f,
												  getStyleAccented(Settings._font2.FontFamily));

				tabMenu.Font.Dispose();
				tabMenu.Font = Settings._font2;

				cellMenu.Font.Dispose();
				cellMenu.Font = Settings._font2;
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

			btn_ProPanel.Left = ClientSize.Width - PropertyPanelButton.HEIGHT + 1;
			btn_ProPanel.Top = -1; // NOTE: This won't work in PP button's cTor. So do it here.


			if (Settings._recent != 0)
				CreateRecentsSubits(); // init recents before (potentially) loading a table from FileExplorer

			if (File.Exists(pfe_load)) // load file from FileExplorer ...
				CreatePage(pfe_load);
			else
				Obfuscate();

			_t1.Interval = 223;
			_t1.Enabled = true; // TODO: stop Timer when no table is loaded /shrug.
			_t1.Tick += t1_Tick;

			DontBeepEvent += HandleDontBeepEvent;

			TalkReader.LoadTalkingHeads(Strrefheads);
			TalkReader.Load(Settings._dialog,    it_PathTalkD);
			TalkReader.Load(Settings._dialogalt, it_PathTalkC, true);

			if (Settings._maximized)
				WindowState = FormWindowState.Maximized;
		}


		/// <summary>
		/// Initializes the recent-files list from entries in the user-file
		/// "recent.cfg".
		/// </summary>
		void CreateRecentsSubits()
		{
			string dir = Application.StartupPath;
			string pfe = Path.Combine(dir, "recent.cfg");
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
			if (Table != null && _track_x != -1 && !YataGrid._init)
				Table.MouseLeaveTicker();
		}


		#region Events (override)
		/// <summary>
		/// Sends the <c>MouseWheel</c> event to the active
		/// <c><see cref="YataGrid"/></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (Table != null)
				Table.Scroll(e);
		}


		/// <summary>
		/// Sets 'IsMin' true so that when the form is minimized then restored/
		/// maximized the ensure-displayed call(s) are bypassed by the tables'
		/// OnResize() event(s). Because if the user wants to simply minimize
		/// the window temporarily to check something out in another app you
		/// don't want the view to be changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
				IsMin = true;

			base.OnResize(e);
		}

		/// <summary>
		/// Processes so-called commandkey combinations.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			//logfile.Log("YataForm.ProcessCmdKey() keyData= " + keyData);

			switch (keyData)
			{
				case Keys.Control | Keys.X: // bypass Ctrl+x, Ctrl+c, Ctrl+v if the editor is visible.
				case Keys.Control | Keys.C: // this bypasses the Edit menuitems and lets the editbox
				case Keys.Control | Keys.V: // take the message if/when the editbox is visible.
				case Keys.Delete:
//				case Keys.Delete | Keys.Shift: // appears to not be used by textboxes.
					if (Table != null
						&& (Table._editor.Visible
							|| (Table.Propanel != null && Table.Propanel._editor.Visible)))
					{
						return false;
					}
					break;

				case Keys.Shift | Keys.F3:					// reverse search
					editclick_SearchNext(null, EventArgs.Empty);
					return true;

				case Keys.Shift | Keys.Control | Keys.N:	// reverse gotoloadchanged
					editclick_GotoLoadchanged(null, EventArgs.Empty);
					return true;

				case Keys.Shift | Keys.F8:					// reverse propanel location cycle
					opsclick_PropertyPanelLocation(null, EventArgs.Empty);
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}


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
			//logfile.Log("YataForm.OnKeyDown() e.KeyData= " + e.KeyData);

			switch (e.KeyData)
			{
				case Keys.Enter: // do this here to get rid of the beep.
					if (Table != null && Table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
					{
						if (tb_Search.Focused || cb_SearchOption.Focused)
							_dontbeep = DONTBEEP_SEARCH;
						else if (tb_Goto.Focused)
							_dontbeep = DONTBEEP_GOTO;
						else
							_dontbeep = DONTBEEP_DEFAULT;

						if (_dontbeep != DONTBEEP_DEFAULT)
						{
							e.SuppressKeyPress = true;
							BeginInvoke(DontBeepEvent);
						}
					}
					break;

				case Keys.Escape:
					if (Table != null
						&& (Tabs.Focused || btn_ProPanel.Focused)) // btn -> jic.
					{
						e.SuppressKeyPress = true;
						Table.Select();
					}
					break;

				case Keys.Space:
					if (Table != null
						&& !Table._editor.Visible
						&& (Table.Propanel == null || !Table.Propanel._editor.Visible))
					{
						e.SuppressKeyPress = true;
						Table.SelectFirstCell();
					}
					break;
			}
			base.OnKeyDown(e);
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
					EnterkeyedSearch();
					break;

				case DONTBEEP_GOTO:
					Table.doGoto(tb_Goto.Text, true);
					break;
			}
		}
		#endregion Events (override)


		#region Receive Message (pfe)
		/// <summary>
		/// Disables message-blocking in Vista+ 64-bit systems.
		/// https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void yata_Load(object sender, EventArgs e)
		{
			var filter = new Crap.CHANGEFILTERSTRUCT();
			filter.size = (uint)Marshal.SizeOf(filter);
			filter.info = 0;
			if (!Crap.ChangeWindowMessageFilterEx(Handle,
												  Crap.WM_COPYDATA,
												  Crap.ChangeWindowMessageFilterExAction.Allow,
												  ref filter))
			{
				MessageBox.Show(String.Format("An error occurred: {0}", Marshal.GetLastWin32Error()));
			}
		}

		/// <summary>
		/// Receives data via WM_COPYDATA from other applications/processes.
		/// https://www.codeproject.com/Tips/1017834/How-to-Send-Data-from-One-Process-to-Another-in-Cs
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == Crap.WM_COPYDATA)
			{
				var copyData = (Crap.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Crap.COPYDATASTRUCT));
				int dataType = (int)copyData.dwData;
				if (dataType == Crap.CopyDataStructType) // extract the file-string ->
				{
					pfe_load = Marshal.PtrToStringAnsi(copyData.lpData);
					if (File.Exists(pfe_load))
						CreatePage(pfe_load);
				}
			}
			else
				base.WndProc(ref m);
		}
		#endregion Receive Message (pfe)


		#region Methods (static)
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
		/// <param name="obscure"><c>true</c> to bring the color-panel to front
		/// or <c>false</c> to send the color-panel to back</param>
		internal void Obfuscate(bool obscure = true)
		{
			if (obscure) panel_ColorFill.BringToFront();
			else         panel_ColorFill.SendToBack();
		}

		/// <summary>
		/// TODO: This funct is obsolete; tables that are
		/// <c><see cref="YataGrid.Readonly">YataGrid.Readonly</see></c> shall
		/// not enable operations that can change the table at all.
		/// </summary>
		void ReadonlyError()
		{
			MessageBox.Show("The 2da-file is opened as readonly.",
							" burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Hand,
							MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Checks if there is a non-readonly table open and optionally calls
		/// all tables' <c>Leave</c> event handler.
		/// </summary>
		/// <returns><c>true</c> if SaveAll is allowed</returns>
		bool AllowSaveAll()
		{
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				if (!(Tabs.TabPages[i].Tag as YataGrid).Readonly)
					return true;
			}
			return false;
		}

/*		/// <summary>
		/// Hides a visible editor when the tabpage changes.
		/// </summary>
		/// <remarks>This is a redundant check jic user changes the tabpage w/
		/// <c>[Ctrl+PageUp]/[Ctrl+PageDown]</c>. Typically each
		/// <c><see cref="YataGrid">YataGrid's</see></c> <c>Leave</c> handler
		/// takes care of it.</remarks>
		void HideEditor()
		{
			YataGrid table;
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				if ((table = Tabs.TabPages[i].Tag as YataGrid)._editor.Visible)
				{
					table._editor.Visible = false;
					break;
				}
			}
		} */
		#endregion Methods


		#region Events
		/// <summary>
		/// Hides the <c><see cref="YataGrid._editor">YataGrid._editor</see></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>This is a generic handler that should be invoked when
		/// opening a menu on the menubar (iff the menu doesn't have its own
		/// dropdown-handler).
		/// 
		/// 
		/// Note that the
		/// <c><see cref="YataGrid.Propanel">YataGrid.Propanel</see></c> does
		/// not need to be hidden here since it gets hidden when the
		/// <c>Propanel</c> loses focus.</remarks>
		void dropdownopening(object sender, EventArgs e)
		{
			if (Table != null && Table._editor.Visible)
			{
				Table._editor.Visible = false;
				Table.Invalidator(YataGrid.INVALID_GRID);
			}
		}
		#endregion Events


		#region Methods (create)
		/// <summary>
		/// Creates a tab-page and instantiates a table-grid for it.
		/// </summary>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read"><c>true</c> to create table as
		/// <c><see cref="YataGrid.Readonly">YataGrid.Readonly</see></c></param>
		/// <remarks>The filename w/out extension must not be blank since
		/// <c><see cref="YataGrid.Init()">YataGrid.Init()</see></c> is going to
		/// use blank as a fallthrough while determining the grid's
		/// <c><see cref="YataGrid.InfoType">YataGrid.InfoType</see></c> to call
		/// <c><see cref="GropeLabels()">GropeLabels()</see></c>.</remarks>
		void CreatePage(string pfe, bool read = false)
		{
			if (File.Exists(pfe)													// safety (probably).
				&& !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(pfe)))	// what idjut would ... oh, wait.
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

				Obfuscate();
//				Refresh();	// NOTE: If a table is already loaded the color-panel doesn't show
							// but a refresh turns the client-area gray at least instead of glitchy.
							// NOTE: It went away; the table-area turns gray.

				var table = new YataGrid(this, pfe, read);

				int result = table.LoadTable();
				if (result != YataGrid.LOADRESULT_FALSE)
				{
					Table = table; // NOTE: Is done in tab_SelectedIndexChanged() also.

					DrawingControl.SuspendDrawing(Table);

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

					DrawingControl.ResumeDrawing(Table);
				}

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


		#region Events (tabs)
		/// <summary>
		/// Handles tab-selection/deselection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

				btn_ProPanel      .Visible = true;
				it_MenuPaths      .Visible = Table.Info != YataGrid.InfoType.INFO_NONE;


				it_freeze1        .Checked = Table.FrozenCount == YataGrid.FreezeFirst;
				it_freeze2        .Checked = Table.FrozenCount == YataGrid.FreezeSecond;

				it_Readonly       .Checked = Table.Readonly;


				it_Undo           .Enabled = Table._ur.CanUndo;
				it_Redo           .Enabled = Table._ur.CanRedo;

				it_Reload         .Enabled = File.Exists(Table.Fullpath);
				it_Save           .Enabled = !Table.Readonly;
				it_SaveAll        .Enabled = AllowSaveAll();
				it_SaveAs         .Enabled =
				it_Readonly       .Enabled =
				it_Close          .Enabled =
				it_CloseAll       .Enabled = true;

				it_Searchnext     .Enabled = tb_Search.Text.Length != 0;
				it_GotoLoadchanged.Enabled = Table.anyLoadchanged();

				EnableCelleditOperations();

				bool isrowselected = Table.getSelectedRow() != -1;
				it_CutRange       .Enabled = !Table.Readonly && isrowselected;
				it_CopyRange      .Enabled = isrowselected;
				it_PasteRange     .Enabled = !Table.Readonly && _copyr.Count != 0;
				it_DeleteRange    .Enabled = !Table.Readonly && isrowselected;
				it_CreateRows     .Enabled = !Table.Readonly;

				it_OrderRows      .Enabled = !Table.Readonly;
				it_CheckRows      .Enabled =
				it_ColorRows      .Enabled =
				it_AutoCols       .Enabled =
				it_ppOnOff        .Enabled = true;
				it_ppLocation     .Enabled = Table.Propanel != null && Table.Propanel.Visible;
				it_ExternDiff     .Enabled = File.Exists(Settings._diff);

				it_freeze1        .Enabled = Table.ColCount > 1;
				it_freeze2        .Enabled = Table.ColCount > 2;


				if (Table.Propanel != null && Table.Propanel.Visible)
				{
					Table.Propanel.telemetric();
					if (sel != null)
						Table.Propanel.EnsureDisplayed(sel.x);
				}

				if (_fdiffer != null)
					_fdiffer.EnableGotoButton(true);

				SetTabSize();

				Obfuscate(false);
			}
			else
			{
				Table = null;

				Obfuscate();

				btn_ProPanel      .Visible =
				it_MenuPaths      .Visible =


				it_freeze1        .Checked =
				it_freeze2        .Checked =

				it_Readonly       .Checked =


				it_Undo           .Enabled =
				it_Redo           .Enabled =

				it_Reload         .Enabled =
				it_Save           .Enabled =
				it_SaveAll        .Enabled =
				it_SaveAs         .Enabled =
				it_Readonly       .Enabled =
				it_Close          .Enabled =
				it_CloseAll       .Enabled =

				it_Searchnext     .Enabled =
				it_GotoLoadchanged.Enabled =

				it_CutCell        .Enabled =
				it_CopyCell       .Enabled =
				it_PasteCell      .Enabled =
				it_DeleteCell     .Enabled =
				it_Lower          .Enabled =
				it_Upper          .Enabled =
				it_Apply          .Enabled =

				it_CutRange       .Enabled =
				it_CopyRange      .Enabled =
				it_PasteRange     .Enabled =
				it_DeleteRange    .Enabled =
				it_CreateRows     .Enabled =

				it_OrderRows      .Enabled =
				it_CheckRows      .Enabled =
				it_ColorRows      .Enabled =
				it_AutoCols       .Enabled =
				it_ppOnOff        .Enabled =
				it_ppLocation     .Enabled =
				it_ExternDiff     .Enabled =

				it_freeze1        .Enabled =
				it_freeze2        .Enabled = false;

				_fdiffer = null;
			}

			SetTitlebarText();
		}

		/// <summary>
		/// Draws the tab-text in Bold iff selected.
		/// </summary>
		/// <param name="sender"></param>
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

				// NOTE: The tag (and text...) can be null when the tabcontrol needs
				// to extend down to create another row. Go figur.
				// Fortunately the text of a table that is opened as readonly will
				// still appear in the TextReadonly color because .net tends to be
				// redundant and so this draw routine gets a second call right away
				// with its Tag/table valid.
				// Note that this is one of those null-errors that the debugger will
				// slough off ....
				var table = tab.Tag as YataGrid;
				if (table.Readonly) //table != null && // 'table' better not be null.
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
		#endregion Events (tabs)


		#region Methods (tabs)
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
				Tabs.ItemSize = new Size(w + 10, h + 4); // w/ pad
//				Tabs.Refresh(); // prevent text-drawing glitches ... I can't see any glitches.
			}

			YataGrid.BypassInitScroll = false;
		}

		/// <summary>
		/// Disposes a tab's table's <c><see cref="FileWatcher"/></c> before a
		/// specified <c>TabPage</c> is removed from the
		/// <c>TabPageCollection</c>.
		/// </summary>
		/// <param name="tab">the <c>TabPage</c> with which to deal</param>
		void ClosePage(TabPage tab)
		{
			_table = tab.Tag as YataGrid;

			_table.DisposeWatcher();
			_table.Dispose();

			if      (_table == _diff1) _diff1 = null;
			else if (_table == _diff2) _diff2 = null;

			if (_diff1 == null && _diff2 == null && _fdiffer != null)
				_fdiffer.Close();

			Tabs.TabPages.Remove(tab);
			tab.Dispose();
			_table = null;

			YataGrid.metricStaticHeads(this);
		}

		/// <summary>
		/// Sets the tabtext of a specified table.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Is called by
		/// <c><see cref="YataGrid.Changed">YataGrid.Changed</see></c> property.</remarks>
		internal void SetTabText(YataGrid table)
		{
			DrawingControl.SuspendDrawing(this); // stop tab-flicker on Sort etc.

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

			DrawingControl.ResumeDrawing(this);
		}

		/// <summary>
		/// Sets the tabtexts of all tables.
		/// </summary>
		/// <remarks>Called by
		/// <c><see cref="fileclick_SaveAll()">fileclick_SaveAll()</see></c>.</remarks>
		void SetAllTabTexts()
		{
			DrawingControl.SuspendDrawing(this); // stop tab-flicker on Sort etc.

			string asterics;
			foreach (TabPage tab in Tabs.TabPages)
			{
				_table = tab.Tag as YataGrid;
				asterics = _table.Changed ? ASTERICS : String.Empty;
				tab.Text = Path.GetFileNameWithoutExtension(_table.Fullpath) + asterics;
			}
			_table = null;
			SetTabSize();

			DrawingControl.ResumeDrawing(this);
		}
		#endregion Methods (tabs)


		#region Methods (save)
		/// <summary>
		/// Requests user-confirmation when saving a file when readonly or when
		/// faulty IDs get detected.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		DialogResult SaveWarning(string info)
		{
			_warned = true;
			return MessageBox.Show(info
								   + Environment.NewLine + Environment.NewLine
								   + "Save anyway ...",
								   " burp",
								   MessageBoxButtons.YesNo,
								   MessageBoxIcon.Exclamation,
								   MessageBoxDefaultButton.Button2);
		}

		/// <summary>
		/// Checks the row-order before save.
		/// </summary>
		/// <returns><c>true</c> if row-order is okay</returns>
		bool CheckRowOrder()
		{
			string val;
			int result;

			for (int id = 0; id != Table.RowCount; ++id)
			{
				if (String.IsNullOrEmpty(val = Table[id,0].text)
					|| !Int32.TryParse(val, out result)
					|| result != id)
				{
					return false;
				}
			}
			return true;
		}
		#endregion Methods (save)


		#region Events (close)
		/// <summary>
		/// Handles Yata's <c>FormClosing</c> event. Requests user-confirmation
		/// if data has changed and writes a recent-files list if appropriate.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void yata_Closing(object sender, CancelEventArgs e)
		{
			if (Tabs.TabPages.Count != 0)
			{
				if (Tabs.TabPages.Count == 1)
				{
					e.Cancel = Table.Changed
							&& MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to quit ...",
											   " warning",
											   MessageBoxButtons.YesNo,
											   MessageBoxIcon.Warning,
											   MessageBoxDefaultButton.Button2) == DialogResult.No;
				}
				else
					e.Cancel = CheckChangedTables("quit");
			}

			if (!e.Cancel && Settings._recent != 0)
			{
				int i = -1;
				var recents = new string[it_Recent.DropDownItems.Count];
				foreach (ToolStripItem recent in it_Recent.DropDownItems)
					recents[++i] = recent.Text;

				string dir = Application.StartupPath;
				string pfe = Path.Combine(dir, "recent.cfg");
				try
				{
					File.WriteAllLines(pfe, recents);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		#endregion Events (close)


		#region Methods (close)
		/// <summary>
		/// Checks if any currently opened tables have their
		/// <c><see cref="YataGrid.Changed">YataGrid.Changed</see></c> flag set.
		/// </summary>
		/// <param name="info">"close" files or "quit" Yata</param>
		/// <param name="excludecurrent"><c>true</c> to exclude the current
		/// table - used by CloseAllOtherTables</param>
		/// <returns><c>true</c> if there are any changed tables and user
		/// chooses to cancel</returns>
		bool CheckChangedTables(string info, bool excludecurrent = false)
		{
			string tables = String.Empty;

			YataGrid table;
			foreach (TabPage page in Tabs.TabPages)
			{
				if ((table = page.Tag as YataGrid).Changed
					&& (!excludecurrent || table != Table))
				{
					tables += Path.GetFileNameWithoutExtension(table.Fullpath).ToUpperInvariant()
							+ Environment.NewLine;
				}
			}

			if (tables.Length != 0)
			{
				return MessageBox.Show("Data has changed."
									  + Environment.NewLine + Environment.NewLine
									  + tables
									  + Environment.NewLine
									  + "Okay to " + info + " ...",
									  " warning",
									  MessageBoxButtons.YesNo,
									  MessageBoxIcon.Warning,
									  MessageBoxDefaultButton.Button2) == DialogResult.No;
			}
			return false;
		}
		#endregion Methods (close)


		#region Events (file)
		/// <summary>
		/// Handles opening the File menu along with the preset-dirs and
		/// recent-files submenus.
		/// </summary>
		/// <param name="sender"><c><see cref="it_MenuFile"/></c></param>
		/// <param name="e"></param>
		void file_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null)
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				it_Reload.Enabled = File.Exists(Table.Fullpath);
			}
			else
				it_Reload.Enabled = false;


			// directory presets ->
			if (Settings._dirpreset.Count != 0)
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
				ToolStripItemCollection recents = it_Recent.DropDownItems;
				foreach (ToolStripItem it in recents)
				{
					if (!File.Exists(it.Text))
					{
						recents.Remove(it);
						it.Dispose();
					}
				}
				it_Recent.Visible = recents.Count != 0;
			}
		}


		/// <summary>
		/// Handles it-click to open a preset folder.
		/// </summary>
		/// <param name="sender"><c><see cref="it_OpenFolder"/></c> subits</param>
		/// <param name="e"></param>
		void fileclick_OpenFolder(object sender, EventArgs e)
		{
			_preset = (sender as ToolStripItem).Text;
			fileclick_Open(sender, e);
		}

		/// <summary>
		/// Handles it-click to open a folder. Shows an <c>OpenFileDialog</c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Open"/></c></item>
		/// <item><c><see cref="it_OpenFolder"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Open <c>[Ctrl+o]</c></item>
		/// <item>File|Open@Folder
		/// <c><see cref="fileclick_OpenFolder()">fileclick_OpenFolder()</see></c></item>
		/// </list></remarks>
		void fileclick_Open(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.InitialDirectory = _preset;

				ofd.Title  = "Select a 2da file";
				ofd.Filter = Get2daFilter();

				ofd.ShowReadOnly =
				ofd.Multiselect  = true;

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					bool read = ofd.ReadOnlyChecked;
					foreach (var pfe in ofd.FileNames)
						CreatePage(pfe, read);
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
		/// <item><c><see cref="it_tabReload"/></c></item>
		/// <item><c>null</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|Reload <c>[Ctrl+r]</c></item>
		/// <item>tab|Reload</item>
		/// <item><c><see cref="FileWatcherDialog"/>.OnFormClosing()</c></item>
		/// </list></remarks>
		internal void fileclick_Reload(object sender, EventArgs e)
		{
			if (Table != null && File.Exists(Table.Fullpath)
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to reload ...",
									   " warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				Obfuscate();
				DrawingControl.SuspendDrawing(Table);

				if      (Table == _diff1) _diff1 = null;
				else if (Table == _diff2) _diff2 = null;


				int result = Table.LoadTable();
				if (result != YataGrid.LOADRESULT_FALSE)
				{
					Table._ur.Clear();

					it_freeze1.Checked =
					it_freeze2.Checked = false;

					Table.Init(result == YataGrid.LOADRESULT_CHANGED, true);

					if (Table.Propanel != null)
					{
						Table.Controls.Remove(Table.Propanel);
						Table.Propanel = null;
					}
				}
				else
				{
					Table.Changed = false; // bypass the close-tab warning.
					fileclick_ClosePage(sender, e);
				}

				if (Table != null)
				{
					Obfuscate(false);
					DrawingControl.ResumeDrawing(Table);

					Table.Watcher.BypassFileChanged = true;
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
			CreatePage((sender as ToolStripItem).Text);
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

			Tabs.Invalidate();
		}


		/// <summary>
		/// Handles several it-clicks that write a <c><see cref="YataGrid"/></c>
		/// to a 2da-file.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Save"/></c></item>
		/// <item><c><see cref="it_tabSave"/></c></item>
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
		/// <item><c><see cref="FileWatcherDialog"></see>.OnFormClosing</c></item>
		/// </list></remarks>
		internal void fileclick_Save(object sender, EventArgs e)
		{
			if (Table != null) // safety.
			{
				bool force; // force a Readonly file to overwrite itself (only if invoked by SaveAs)
				bool bypassReadonly;

				if (sender == it_SaveAs)
				{
					_table = Table;
					// '_pfeT' is set by caller
					force = (_pfeT == _table.Fullpath);
					bypassReadonly = false;
				}
				else if (sender == it_SaveAll)
				{
					// '_table' and '_pfeT' are set by caller
					force = false;
					bypassReadonly = false;
				}
				else // is rego-save or tab-save or 'FileWatcherDialog' save
				{
					_table = Table;
					_pfeT = _table.Fullpath;
					force = false;

					if (sender == it_Save || sender == it_tabSave)
						bypassReadonly = false;
					else
						bypassReadonly = true; // only the 'FileWatcherDialog' gets to bypass Readonly.
				}

				if (!String.IsNullOrEmpty(_pfeT)) // safety.
				{
					_warned = false;

					if (!_table.Readonly || bypassReadonly
						|| (force && SaveWarning("The 2da-file is opened as readonly.") == DialogResult.Yes))
					{
//						if ((_table._sortcol == 0 && _table._sortdir == YataGrid.SORT_ASC)
//							|| SaveWarning("The 2da is not sorted by ascending ID.") == DialogResult.Yes)
//						{
						if (CheckRowOrder()
							|| SaveWarning("Faulty row IDs are detected.") == DialogResult.Yes)
						{
							_table.Fullpath = _pfeT;

							SetTitlebarText();

							if (force) _table.Readonly = false;	// <- IMPORTANT: If a file that was opened Readonly is saved
																//               *as itself* it loses its Readonly flag.

							if (_table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
							{
								_table.Changed = false;
								_table._ur.ResetSaved();

								foreach (var row in _table.Rows)
								for (int c = 0; c != _table.ColCount; ++c)
									row[c].loadchanged = false;

								if (_table == Table)
									_table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);

								FileOutput.Write(_table);
							}
						}
//						}
					}
					else if (!_warned)
						ReadonlyError();
				}
			}
		}

		/// <summary>
		/// Handles it-click on file SaveAs.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAs"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAs <c>[Ctrl+e]</c></item>
		/// </list></remarks>
		void fileclick_SaveAs(object sender, EventArgs e)
		{
			if (Table != null)
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.Title    = "Save as ...";
					sfd.Filter   = Get2daFilter();
					sfd.FileName = Path.GetFileName(Table.Fullpath);

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						_pfeT = sfd.FileName;
						fileclick_Save(sender, e);
					}
				}
			}
		}

		/// <summary>
		/// Handles it-click on file SaveAll.
		/// </summary>
		/// <param name="sender"><c><see cref="it_SaveAll"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|SaveAll <c>[Ctrl+a]</c></item>
		/// </list></remarks>
		void fileclick_SaveAll(object sender, EventArgs e)
		{
			if (AllowSaveAll())
			{
				IsSaveAll = true;

				bool changed = false;
				for (int i = 0; i != Tabs.TabCount; ++i)
				{
					_table = Tabs.TabPages[i].Tag as YataGrid;
					if (!_table.Readonly)
					{
						_table.Watcher.Enabled = false; // TODO. wut

						if (_table.Changed)
							changed = true;

						_pfeT = _table.Fullpath;
						fileclick_Save(sender, e);

						_table.Watcher.Enabled = true;
					}
				}
				_table = null;

				if (changed)
					SetAllTabTexts();

				IsSaveAll = false;
			}
		}


		/// <summary>
		/// Handles it-click on file Close.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Close"/></c></item>
		/// <item><c><see cref="it_tabClose"/></c></item>
		/// <item><c><see cref="it_Reload"/></c></item>
		/// <item><c><see cref="it_tabReload"/></c></item>
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
		/// <item><c><see cref="FileWatcherDialog"></see>.OnFormClosing</c></item>
		/// </list></remarks>
		internal void fileclick_ClosePage(object sender, EventArgs e)
		{
			if (Table != null
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to close ...",
									   " warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				ClosePage(Tabs.SelectedTab);
			}
		}

		/// <summary>
		/// Handles it-click on file CloseAll.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_CloseAll"/></c></item>
		/// <item><c><see cref="it_tabCloseAll"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>File|CloseAll</item>
		/// <item>tab|CloseAll</item>
		/// </list></remarks>
		void fileclick_CloseAllTabs(object sender, EventArgs e)
		{
			if (!CheckChangedTables("close"))
			{
				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
					ClosePage(Tabs.TabPages[tab]);
			}
		}


		/// <summary>
		/// Handles it-click on file Quit.
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
		#endregion Events (file)


		#region Methods (context)
		/// <summary>
		/// Opens the context for single-row editing.
		/// </summary>
		/// <param name="r"></param>
		internal void context_(int r)
		{
			_r = r;

			context_it_Header.Text = "_row @ id " + _r;

			context_it_PasteAbove .Enabled =
			context_it_Paste      .Enabled =
			context_it_PasteBelow .Enabled = !Table.Readonly && _copyr.Count != 0;

			context_it_Cut        .Enabled =
			context_it_CreateAbove.Enabled =
			context_it_ClearRow   .Enabled =
			context_it_CreateBelow.Enabled =
			context_it_DeleteRow  .Enabled = !Table.Readonly;

			Point loc;
			if (Settings._context)							// static location
			{
				loc = new Point(YataGrid.WidthRowhead,
								YataGrid.HeightColhead);
			}
			else											// vanilla location
				loc = Table.PointToClient(Cursor.Position);

			ContextEditor.Show(Table, loc);
		}
		#endregion Methods (context)


		#region Events (context)
		/// <summary>
		/// Handles context-click on the context-header.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_Header(object sender, EventArgs e)
		{
			ContextEditor.Hide();
		}

		/// <summary>
		/// Handles context-click to copy a row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditCopy(object sender, EventArgs e)
		{
			_copyr.Clear();

			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = Table[_r,c].text;

			_copyr.Add(fields);
		}

		/// <summary>
		/// Handles context-click to cut a row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditCut(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				contextclick_EditCopy(  null, EventArgs.Empty);
				contextclick_EditDelete(null, EventArgs.Empty);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to paste above the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditPasteAbove(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.Insert(_r, _copyr[0]);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);


				Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to paste into the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditPaste(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				// - store the row's current state to 'rPre' in the Restorable
				Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


				Row row = Table.Rows[_r];
				string field;
				for (int c = 0; c != Table.ColCount; ++c)
				{
					if (c < _copyr[0].Length)
						field = _copyr[0][c];
					else
						field = gs.Stars;

					row[c].text = field;
					row[c].diff =
					row[c].loadchanged = false;
				}
				row._brush = Brushes.Created;

				Table.Calibrate(_r);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);
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
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to paste below the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditPasteBelow(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.Insert(_r + 1, _copyr[0]);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);


				Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to create a row above the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditCreateAbove(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				var fields = new string[Table.ColCount];
				fields[0] = _r.ToString();
				for (int c = 1; c != Table.ColCount; ++c)
				{
					fields[c] = gs.Stars;
				}
				Table.Insert(_r, fields);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);


				Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Delete);
				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to clear the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditClear(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				// - store the row's current state to 'rPre' in the Restorable
				Restorable rest = UndoRedo.createRow(Table.Rows[_r]);


				for (int c = 1; c != Table.ColCount; ++c)
				{
					Table[_r,c].text = gs.Stars;
					Table[_r,c].diff =
					Table[_r,c].loadchanged = false;
				}
				Table.Rows[_r]._brush = Brushes.Created;

				Table.Calibrate(_r);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);
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
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to create a row below the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditCreateBelow(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				var fields = new string[Table.ColCount];
				fields[0] = (_r + 1).ToString();
				for (int c = 1; c != Table.ColCount; ++c)
				{
					fields[c] = gs.Stars;
				}
				Table.Insert(_r + 1, fields);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);


				Restorable rest = UndoRedo.createRow(Table.Rows[_r + 1], UndoRedo.UrType.rt_Delete);
				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles context-click to delete the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditDelete(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Insert);


				Table.Insert(_r);

//				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
//				if (Table.Propanel != null && Table.Propanel.Visible)
//					invalid |= YataGrid.INVALID_PROP;
//
//				Table.Invalidator(invalid);


				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);
			}
			else
				ReadonlyError();
		}
		#endregion Events (context)


		#region Events (edit)
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
			if (Table != null)
			{
				Table._ur.Undo();
				it_Undo.Enabled = Table._ur.CanUndo;
				it_Redo.Enabled = true;
			}
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
			if (Table != null)
			{
				Table._ur.Redo();
				it_Redo.Enabled = Table._ur.CanRedo;
				it_Undo.Enabled = true;
			}
		}


		/// <summary>
		/// Handles textchanged for the searchbox. Enables/disables searchnext.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void textchanged_Search(object sender, EventArgs e)
		{
			it_Searchnext.Enabled = Table != null
								 && tb_Search.Text.Length != 0;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when user clicks the searchbox.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Search"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Find <c>[Ctrl+f]</c></item>
		/// </list></remarks>
		void editclick_Search(object sender, EventArgs e)
		{
			tb_Search.Focus();
			tb_Search.SelectAll();
		}

		/// <summary>
		/// Handles selectall hocus-pocus when user clicks the searchbox.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void enter_Search(object sender, EventArgs e)
		{
			_firstclick = true;
		}

		/// <summary>
		/// Handles selectall hocus-pocus when user clicks the searchbox.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Search"/></c></param>
		/// <param name="e"></param>
		void click_Search(object sender, EventArgs e)
		{
			if (_firstclick)
			{
				_firstclick = false;
				tb_Search.SelectAll();
			}
		}

		/// <summary>
		/// Performs <c>[F3]</c> search with focus on the
		/// <c><see cref="Table"/></c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_Searchnext"/></c> <c>[F3]</c></item>
		/// <item><c>null</c> (reverse search w/ <c>[Shift]</c>)</item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks><c>[F3]</c> shall focus the table, <c>[Enter]</c> shall
		/// keep focus on the tb/cbx.
		/// 
		/// 
		/// Called by
		/// <list type="bullet">
		/// <item>Edit|Find next <c>[F3]</c></item>
		/// <item><c>[Shift+F3]</c>
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c></item>
		/// </list></remarks>
		void editclick_SearchNext(object sender, EventArgs e)
		{
			if (Table != null)
			{
				Table.Select();
				Search();
			}
		}

		/// <summary>
		/// Performs a search when <c>[Enter]</c> is pressed and focus is on
		/// either the search-box or the search-option dropdown.
		/// </summary>
		/// <remarks><c>[Enter]</c> shall keep focus on the tb/cbx, <c>[F3]</c>
		/// shall focus the table.</remarks>
		void EnterkeyedSearch()
		{
			_isEnterkeyedSearch = true;
			Search();
			_isEnterkeyedSearch = false;
		}

		/// <summary>
		/// Searches the current table for the string in the search-box.
		/// </summary>
		/// <remarks>Ensure that <c><see cref="Table"/></c> is valid before
		/// call.</remarks>
		void Search()
		{
			if ((ModifierKeys & (Keys.Control | Keys.Alt)) == 0)
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}


				string search = tb_Search.Text;
				if (!String.IsNullOrEmpty(search))
				{
					search = search.ToLower();

					Cell sel = Table.getSelectedCell();
					int rStart = Table.getSelectedRow();

					Table.ClearSelects();

					bool substring = (cb_SearchOption.SelectedIndex == 0); // else is wholestring search.
					bool start = true;

					string text;

					int r,c;

					if ((ModifierKeys & Keys.Shift) == 0) // forward search ->
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
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToLower()) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = 0; r != rStart + 1;     ++r) // quick and dirty wrap ->
						for (c = 0; c != Table.ColCount; ++c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToLower()) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return;
							}
						}
					}
					else // backward search ->
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
								if (c != 0 // don't search the id-col
									&& ((text = Table[r,c].text.ToLower()) == search
										|| (substring && text.Contains(search))))
								{
									Table.SelectCell(Table[r,c]);
									return;
								}
							}
						}

						// TODO: tighten exact start/end-cells
						for (r = Table.RowCount - 1; r != rStart - 1; --r) // quick and dirty wrap ->
						for (c = Table.ColCount - 1; c != -1;         --c)
						{
							if (c != 0 // don't search the id-col
								&& ((text = Table[r,c].text.ToLower()) == search
									|| (substring && text.Contains(search))))
							{
								Table.SelectCell(Table[r,c]);
								return;
							}
						}
					}
				}
				else // not found ->
					Table.ClearSelects(); // TODO: That should return a bool if any clears happened.

				int invalid = YataGrid.INVALID_GRID
							| YataGrid.INVALID_FROZ
							| YataGrid.INVALID_ROWS;
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid);
			}
		}


		/// <summary>
		/// Handles textchanged in the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void textchanged_Goto(object sender, EventArgs e)
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
		/// Handles it-click to focus the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="it_Goto"/></c></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Goto <c>[Ctrl+g]</c></item>
		/// </list></remarks>
		void editclick_Goto(object sender, EventArgs e)
		{
			tb_Goto.Focus();
			tb_Goto.SelectAll();
		}

		/// <summary>
		/// Handles the enter event of the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void enter_Goto(object sender, EventArgs e)
		{
			_firstclick = true;
		}

		/// <summary>
		/// Handles click on the goto box.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Goto"/></c></param>
		/// <param name="e"></param>
		void click_Goto(object sender, EventArgs e)
		{
			if (_firstclick)
			{
				_firstclick = false;
				tb_Goto.SelectAll();
			}
		}

		/// <summary>
		/// Selects the next LoadChanged cell.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="it_GotoLoadchanged"/></c> <c>[Ctrl+n]</c></item>
		/// <item><c>null</c> (reverse goto w/ <c>[Shift]</c>)</item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Called by
		/// <list type="bullet">
		/// <item>Edit|Goto loadchanged <c>[Ctrl+n]</c></item>
		/// <item><c>[Shift+Ctrl+n]</c>
		/// <c><see cref="ProcessCmdKey()">ProcessCmdKey()</see></c></item>
		/// </list></remarks>
		void editclick_GotoLoadchanged(object sender, EventArgs e)
		{
			if (Table != null && Table.anyLoadchanged()
				&& (ModifierKeys & Keys.Alt) == 0)
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				Table.Select();

				Cell sel = Table.getSelectedCell();
				int rStart = Table.getSelectedRow();

				Table.ClearSelects();

				int r,c;

				bool start = true;

				if ((ModifierKeys & Keys.Shift) == 0) // forward goto ->
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
							if ((sel = Table[r,c]).loadchanged)
							{
								Table.SelectCell(sel);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = 0; r != rStart + 1;     ++r) // quick and dirty wrap ->
					for (c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = Table[r,c]).loadchanged)
						{
							Table.SelectCell(sel);
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
							if ((sel = Table[r,c]).loadchanged)
							{
								Table.SelectCell(sel);
								return;
							}
						}
					}

					// TODO: tighten exact start/end-cells
					for (r = Table.RowCount - 1; r != rStart - 1; --r) // quick and dirty wrap ->
					for (c = Table.ColCount - 1; c != -1;         --c)
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
		#endregion Events (edit)


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
		/// Enables/disables <c><see cref="it_GotoLoadchanged"/></c>.
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableGotoLoadchanged(bool enable)
		{
			it_GotoLoadchanged.Enabled = enable;
		}
		#endregion Methods (edit)


		#region Events (editcells)
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
			if (Table != null) // safety and should be taken out
			{
				if (!Table.Readonly) // safety and should be taken out
				{
					if (Table.areSelectedCellsContiguous()) // safety and should be taken out
					{
						Cell cell;

						Cell sel = Table.getFirstSelectedCell();
						if (sel != null) // safety and should be taken out
						{
							int invalid = -1;

							_copytext = new string[_copyvert, _copyhori];

							int i = -1, j;
							for (int r = sel.y; r != sel.y + _copyvert; ++r)
							{
								++i; j = -1;
								for (int c = sel.x; c != sel.x + _copyhori; ++c)
								{
									_copytext[i, ++j] = (cell = Table[r,c]).text;

									if (cell.text != gs.Stars)
									{
										Table.ChangeCellText(cell, gs.Stars);	// does not do a text-check
										invalid = YataGrid.INVALID_NONE;		// ChangeCellText() will run the Invalidator.
									}
									else if (cell.loadchanged)
									{
										cell.loadchanged = false;

										if (invalid == -1)
											invalid = YataGrid.INVALID_GRID;
									}
								}
							}

							if (invalid == YataGrid.INVALID_GRID)
								Table.Invalidator(invalid);
						}
					}
					else
						CopyPasteCellError("Select one cell or a contiguous block of cells.");
				}
				else
					ReadonlyError();
			}
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
			if (Table != null) // safety and should be taken out
			{
				if (Table.areSelectedCellsContiguous()) // safety and should be taken out
				{
					Cell sel = Table.getFirstSelectedCell();
					if (sel != null) // safety and should be taken out
					{
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
					}
				}
				else
					CopyPasteCellError("Select one cell or a contiguous block of cells.");
			}
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
		/// <item><c><see cref="it_cellPaste"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item>Cells|Paste <c>[Ctrl+v]</c></item>
		/// <item>cell|Paste</item>
		/// </list></remarks>
		void editcellsclick_PasteCell(object sender, EventArgs e)
		{
			if (tb_Goto.Focused)
			{
				SetTextboxText(tb_Goto);
			}
			else if (tb_Search.Focused)
			{
				SetTextboxText(tb_Search);
			}
			else if (Table != null) // safety and should be taken out
			{
				if (!Table.Readonly) // safety and should be taken out
				{
					Cell sel = Table.getSelectedCell();
					if (sel != null) // safety and should be taken out
					{
						Cell cell; string text;
						int invalid = -1;

						for (int i = 0; i != _copytext.GetLength(0) && i + sel.y != Table.RowCount; ++i)
						for (int j = 0; j != _copytext.GetLength(1) && j + sel.x != Table.ColCount; ++j)
						{
							cell = Table[i + sel.y,
										 j + sel.x];

							cell.selected = true;

							if ((text = _copytext[i,j]) != cell.text)
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

						if (invalid == YataGrid.INVALID_GRID)
							Table.Invalidator(invalid);

						EnableCelleditOperations();
					}
					else
						CopyPasteCellError("Select one cell.");
				}
				else
					ReadonlyError();
			}
		}

		/// <summary>
		/// Sets the text of a given textbox.
		/// </summary>
		/// <param name="tb"><c><see cref="tb_Goto"/></c> or <c><see cref="tb_Search"/></c></param>
		/// <remarks>Helper for
		/// <c><see cref="editcellsclick_PasteCell()">editcellsclick_PasteCell()</see></c>.</remarks>
		void SetTextboxText(ToolStripItem tb)
		{
			if (Clipboard.ContainsText(TextDataFormat.Text))
			{
				tb.Text = Clipboard.GetText(TextDataFormat.Text);
			}
			else if (_copytext.Length == 1
				&& (tb == tb_Search || _copytext[0,0] != gs.Stars))
			{
				tb.Text = _copytext[0,0];
			}
			else
				tb.Text = String.Empty;
		}

		/// <summary>
		/// Shows user an error if there is not a single cell or not a
		/// contiguous block of cells selected when copying or pasting a cell.
		/// </summary>
		void CopyPasteCellError(string head) // TODO: make this obsolete
		{
			MessageBox.Show(head,
							" burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation,
							MessageBoxDefaultButton.Button1);
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
			if (Table != null) // safety and should be taken out
			{
				if (!Table.Readonly) // safety and should be taken out
				{
					Cell sel;
					int invalid = -1;

					foreach (var row in Table.Rows)
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = row[c]).selected)
						{
							if (sel.text != gs.Stars)
							{
								Table.ChangeCellText(sel, gs.Stars);	// does not do a text-check
								invalid = YataGrid.INVALID_NONE;		// ChangeCellText() will run the Invalidator.
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
				else
					ReadonlyError();
			}
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
			Cell sel;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != sel.text.ToLower())
					{
						Table.ChangeCellText(sel, sel.text.ToLower());	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;				// ChangeCellText() will run the Invalidator.
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
			Cell sel;
			int invalid = -1;

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
			{
				if ((sel = row[c]).selected)
				{
					if (sel.text != sel.text.ToUpper())
					{
						Table.ChangeCellText(sel, sel.text.ToUpper());	// does not do a text-check
						invalid = YataGrid.INVALID_NONE;				// ChangeCellText() will run the Invalidator.
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
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Fired by
		/// <list type="bullet">
		/// <item><c><see cref="it_Apply"/></c> - Menu|Cells|Apply text ...</item>
		/// </list></remarks>
		void editcellsclick_Apply(object sender, EventArgs e)
		{
			using (var f = new TextInputDialog(this))
			{
				if (f.ShowDialog(this) == DialogResult.OK)
				{
					Cell sel;
					int invalid = -1;

					foreach (var row in Table.Rows)
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if ((sel = row[c]).selected)
						{
							if (sel.text != _copytext[0,0])
							{
								Table.ChangeCellText(sel, _copytext[0,0]);	// does not do a text-check
								invalid = YataGrid.INVALID_NONE;			// ChangeCellText() will run the Invalidator.
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
		#endregion Events (editcells)


		#region Methods (editcells)
		/// <summary>
		/// Determines the dis/enabled states of cell-edit operations.
		/// </summary>
		internal void EnableCelleditOperations()
		{
			bool contiguous = Table.areSelectedCellsContiguous();
			it_CutCell   .Enabled = !Table.Readonly && contiguous;
			it_CopyCell  .Enabled = contiguous;

			bool oneSelected = Table.getSelectedCell() != null;
			it_PasteCell .Enabled = !Table.Readonly && oneSelected;

			it_DeleteCell.Enabled = // TODO: if any selected cell is not 'gs.Stars'
			it_Lower     .Enabled = // TODO: if any selected cell is not lowercase
			it_Upper     .Enabled = // TODO: if any selected cell is not uppercase
			it_Apply     .Enabled = !Table.Readonly && Table.anyCellSelected();
		}
		#endregion Methods (editcells)


		#region Events (editrows)
		/// <summary>
		/// Handles opening the EditRowsMenu, determines if various items ought
		/// be enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editrows_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null)
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				bool isrowselected = Table.getSelectedRow() != -1;
//				it_CutRange   .Enabled = !Table.Readonly && isrowselected;
				it_CopyRange  .Enabled = isrowselected;
				it_PasteRange .Enabled = !Table.Readonly && _copyr.Count != 0;
				it_DeleteRange.Enabled = !Table.Readonly && isrowselected;

				it_CreateRows .Enabled = !Table.Readonly;
			}
			else
			{
//				it_CutRange   .Enabled =
				it_CopyRange  .Enabled =
				it_PasteRange .Enabled =
				it_DeleteRange.Enabled =

				it_CreateRows .Enabled = false;
			}
		}


		/// <summary>
		/// Cuts a range of rows.
		/// </summary>
		/// <param name="sender"><c><see cref="it_CutRange"/></c></param>
		/// <param name="e"></param>
		/// <remarks>
		/// <list type="bullet">
		/// <item>Rows|Cut <c>[Ctrl+Shift+x]</c></item>
		/// </list></remarks>
		void editrowsclick_CutRange(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				editrowsclick_CopyRange(null, EventArgs.Empty);
				editrowsclick_DeleteRange(null, EventArgs.Empty);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Copies a range of rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editrowsclick_CopyRange(object sender, EventArgs e)
		{
			_copyr.Clear();

			int selr = Table.getSelectedRow();

			int rFirst, rLast;
			if (Table.RangeSelect > 0)
			{
				rFirst = selr;
				rLast  = selr + Table.RangeSelect;
			}
			else
			{
				rFirst = selr + Table.RangeSelect;
				rLast  = selr;
			}

			string[] fields;
			while (rFirst <= rLast)
			{
				fields = new string[Table.ColCount];
				for (int c = 0; c != Table.ColCount; ++c)
					fields[c] = Table[rFirst,c].text;

				_copyr.Add(fields);
				++rFirst;
			}

			if (!Table.Readonly)
				it_PasteRange.Enabled = true;
		}

		/// <summary>
		/// Pastes a range of rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editrowsclick_PasteRange(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Obfuscate();
				DrawingControl.SuspendDrawing(Table);


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

				Table.ClearSelects();
				Table.Rows[selr].selected = true;
				Table.RangeSelect = _copyr.Count - 1;
				Table.EnsureDisplayedRow(selr);


				if (!Table.Changed)
				{
					Table.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}
				Table._ur.Push(rest);


				Obfuscate(false);
				DrawingControl.ResumeDrawing(Table);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Deletes a range of rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editrowsclick_DeleteRange(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				int selr = Table.getSelectedRow();
				if (selr != -1)
					Table.DeleteRows(selr);
			}
			else
				ReadonlyError();
		}


		/// <summary>
		/// Instantiates <c><see cref="RowCreatorDialog"/></c> for
		/// inserting/creating multiple rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editrowsclick_CreateRows(object sender, EventArgs e)
		{
			if (Table != null)
			{
				if (!Table.Readonly)
				{
					int selr = Table.getSelectedRow();

					using (var f = new RowCreatorDialog(this, selr + 1, _copyr.Count != 0))
					{
						if (f.ShowDialog(this) == DialogResult.OK)
						{
							Obfuscate();
							DrawingControl.SuspendDrawing(Table);


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
								cells[0] = r.ToString();

								Table.Insert(r, cells, false);
								rest.array[i] = Table.Rows[r].Clone() as Row;
							}

							Table.Calibrate(_startCr, _lengthCr - 1); // insert range

							Table.ClearSelects();
							Table.Rows[_startCr].selected = true;
							Table.RangeSelect = _lengthCr - 1;
							Table.EnsureDisplayedRow(_startCr);


							if (!Table.Changed)
							{
								Table.Changed = true;
								rest.isSaved = UndoRedo.IsSavedType.is_Undo;
							}
							Table._ur.Push(rest);


							Obfuscate(false);
							DrawingControl.ResumeDrawing(Table);
						}
					}
				}
				else
					ReadonlyError();
			}
		}
		#endregion Events (editrows)


		#region Methods (editrows)
		/// <summary>
		/// Enables/disables the copy-rows it.
		/// </summary>
		/// <param name="enabled"></param>
		/// <remarks>Is called by <c>Row.selected</c>.</remarks>
		internal void EnableCopyRange(bool enabled)
		{
			it_CopyRange.Enabled = enabled;
		}
		#endregion Methods (editrows)


		#region Events (editcol)
		/// <summary>
		/// Handles opening the EditcolMenu, determines if various items ought
		/// be enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcol_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				bool isColSelected = (Table.getSelectedCol() > 0); // id-col is disallowed

				it_CreateHead .Enabled = !Table.Readonly;
				it_DeleteHead .Enabled =
				it_RelabelHead.Enabled = !Table.Readonly && isColSelected;

				it_CopyCol    .Enabled = isColSelected;
				it_PasteCol   .Enabled = isColSelected && !Table.Readonly && _copyc.Count != 0;
			}
			else
			{
				it_CreateHead .Enabled =
				it_DeleteHead .Enabled =
				it_RelabelHead.Enabled =

				it_CopyCol    .Enabled =
				it_PasteCol   .Enabled = false;
			}
		}

		/// <summary>
		/// Opens a text-input dialog for creating a col at a selected col-id or
		/// at the far right if no col is selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcolclick_CreateHead(object sender, EventArgs e)
		{
			if (Table != null) // safety. Is checked in editcol_dropdownopening()
			{
				if (!Table.Readonly) // safety. Is checked in editcol_dropdownopening()
				{
					using (var f = new InputDialogColhead())
					{
						if (f.ShowDialog(this) == DialogResult.OK
							&& InputDialogColhead._text.Length != 0)
						{
							Obfuscate();
							DrawingControl.SuspendDrawing(Table);

							int selc = Table.getSelectedCol();		// create at far right if no col selected or id-col is selected
							if (selc < 1) selc = Table.ColCount;	// - not sure that id-col could ever be selected

							steadystate();

							Table.CreateCol(selc);

							DrawingControl.ResumeDrawing(Table);
							Obfuscate(false);

							it_freeze1.Enabled = Table.ColCount > 1;
							it_freeze2.Enabled = Table.ColCount > 2;
						}
					}
				}
				else
					ReadonlyError();
			}
		}

		/// <summary>
		/// Deletes a selected col w/ confirmation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcolclick_DeleteHead(object sender, EventArgs e)
		{
			if (Table != null) // safety. Is checked in editcol_dropdownopening()
			{
				if (!Table.Readonly) // safety. Is checked in editcol_dropdownopening()
				{
					int selc = Table.getSelectedCol();
					if (selc > 0) // safety. Is checked in editcol_dropdownopening()
					{
						if (MessageBox.Show("Are you sure you want to delete the selected col"
												+ Environment.NewLine + Environment.NewLine
												+ "\t" + Table.Fields[selc - 1],
											" Delete colhead",
											MessageBoxButtons.YesNo,
											MessageBoxIcon.Warning,
											MessageBoxDefaultButton.Button2,
											0) == DialogResult.Yes)
						{
							Obfuscate();
							DrawingControl.SuspendDrawing(Table);

							steadystate();

							Table.DeleteCol(selc);

							DrawingControl.ResumeDrawing(Table);
							Obfuscate(false);

							it_freeze1.Enabled = Table.ColCount > 1;
							it_freeze2.Enabled = Table.ColCount > 2;
						}
					}
				}
				else
					ReadonlyError();
			}
		}

		/// <summary>
		/// Puts the table in a neutral state.
		/// </summary>
		/// <remarks>Helper for
		/// <c><see cref="editcolclick_CreateHead()">editcolclick_CreateHead()</see></c>
		/// and
		/// <c><see cref="editcolclick_DeleteHead()">editcolclick_DeleteHead()</see></c>.</remarks>
		void steadystate()
		{
			it_freeze1.Checked =
			it_freeze2.Checked = false;
			Table.FrozenCount = YataGrid.FreezeId;

			Table.ClearSelects();

			foreach (var row in Table.Rows)
			for (int c = 0; c != Table.ColCount; ++c)
				row[c].loadchanged = false;

			tabclick_DiffReset(null, EventArgs.Empty);

			if (Table.Propanel != null && Table.Propanel.Visible)
			{
				// TODO: close pp-editor if visible (check code for that generally)
				Table.Propanel.Hide();
				it_ppLocation.Enabled = false;
			}

			Table._ur.Clear(); // TODO: request confirmation
		}

		/// <summary>
		/// Relabels the colhead of a selected col.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcolclick_RelabelHead(object sender, EventArgs e)
		{
			if (Table != null) // safety. Is checked in editcol_dropdownopening()
			{
				if (!Table.Readonly) // safety. Is checked in editcol_dropdownopening()
				{
					int selc = Table.getSelectedCol();
					if (selc > 0) // safety. Is checked in editcol_dropdownopening()
					{
						string head = Table.Fields[selc - 1];
						InputDialogColhead._text = head;
						using (var f = new InputDialogColhead())
						{
							if (f.ShowDialog(this) == DialogResult.OK
								&& InputDialogColhead._text.Length != 0
								&& InputDialogColhead._text != head)
							{
								Table.RelabelCol(selc);
							}
						}
					}
				}
				else
					ReadonlyError();
			}
		}

		/// <summary>
		/// Copies all cell-fields in a selected col to
		/// <c><see cref="_copyc"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcolclick_CopyCol(object sender, EventArgs e)
		{
			if (Table != null) // safety. Is checked in editcol_dropdownopening()
			{
				int selc = Table.getSelectedCol();
				if (selc > 0) // safety. Is checked in editcol_dropdownopening()
				{
					_copyc.Clear();

					for (int r = 0; r != Table.RowCount; ++r)
						_copyc.Add(Table[r, selc].text);
				}
			}
		}

		/// <summary>
		/// Pastes <c><see cref="_copyc"/></c> to the cell-fields of a selected
		/// col.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editcolclick_PasteCol(object sender, EventArgs e)
		{
			if (Table != null && _copyc.Count != 0) // safety. Is checked in editcol_dropdownopening()
			{
				if (!Table.Readonly) // safety. Is checked in editcol_dropdownopening()
				{
					int selc = Table.getSelectedCol();
					if (selc > 0) // safety. Is checked in editcol_dropdownopening()
					{
						string sWarn = String.Empty;
						if (Table.RowCount < _copyc.Count)
						{
							int diff = _copyc.Count - Table.RowCount;
							sWarn = "The table has " + diff + " less row(s) than the copy.";
						}
						else if (Table.RowCount > _copyc.Count)
						{
							int diff = Table.RowCount - _copyc.Count;
							sWarn = "The copy has " + diff + " less row(s) than the table.";
						}

						if (sWarn.Length == 0
							|| MessageBox.Show(sWarn + " Do you want to continue",
												" Count mismatch",
												MessageBoxButtons.YesNo,
												MessageBoxIcon.Warning,
												MessageBoxDefaultButton.Button2,
												0) == DialogResult.Yes)
						{
							Obfuscate();
							DrawingControl.SuspendDrawing(Table);

							Table.PasteCol(selc, _copyc);

							DrawingControl.ResumeDrawing(Table);
							Obfuscate(false);
						}
					}
				}
				else
					ReadonlyError();
			}
		}
		#endregion Events (editcol)


		#region Events (clipboard)
		/// <summary>
		/// Outputs the current contents of '_copyr' to the Windows clipboard.
		/// </summary>
		/// <param name="sender"></param>
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
			ClipboardAssistant.SetText(clip);
		}

		/// <summary>
		/// Imports the current contents of the Windows clipboard to '_copyr'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void clipclick_ImportCopy(object sender, EventArgs e)
		{
			_copyr.Clear();

			string clip = Clipboard.GetText(TextDataFormat.Text);
			if (!String.IsNullOrEmpty(clip))
			{
				string[] lines = clip.Split(new[]{ Environment.NewLine }, StringSplitOptions.None);
				for (int i = 0; i != lines.Length; ++i)
				{
					string[] fields = YataGrid.ParseTableRow(lines[i]);
					_copyr.Add(fields);
				}
			}
		}

		/// <summary>
		/// Displays contents of the clipboard (if text) in an editable
		/// output-box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void clipclick_ViewClipboard(object sender, EventArgs e)
		{
			var f = Application.OpenForms["ClipboardF"];
			if (f == null)
			{
				it_OpenClipEditor.Checked = true;
				f = new ClipboardF(this);
				f.Show(this); // will be disposed auto.
			}
			else
				f.BringToFront();
		}
		#endregion Events (clipboard)


		#region Methods (clipboard)
		/// <summary>
		/// Clears the check on the clipboard-it when the clipboard-dialog
		/// closes.
		/// </summary>
		internal void Clip_uncheck()
		{
			it_OpenClipEditor.Checked = false;
		}
		#endregion Methods (clipboard)


		#region Events (2daOps)
		/// <summary>
		/// Handles opening the 2daOpsMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ops_dropdownopening(object sender, EventArgs e)
		{
			bool valid = (Table != null);

			it_OrderRows .Enabled = valid && !Table.Readonly;
			it_CheckRows .Enabled =
			it_ColorRows .Enabled =
			it_AutoCols  .Enabled =
			it_ppOnOff   .Enabled = valid;
			it_ppLocation.Enabled = valid && Table.Propanel != null && Table.Propanel.Visible;

			it_freeze1   .Enabled = valid && Table.ColCount > 1;
			it_freeze2   .Enabled = valid && Table.ColCount > 2;

			it_ExternDiff.Enabled = valid && File.Exists(Settings._diff);
			it_ClearUr   .Enabled = valid && (Table._ur.CanUndo || Table._ur.CanRedo);

			if (valid)
			{
				Table._editor.Visible = false;
				Table.Invalidator(YataGrid.INVALID_GRID);

				it_ppOnOff.Checked = Table.Propanel != null && Table.Propanel.Visible;
			}
			else
				it_ppOnOff.Checked = false;
		}

		/// <summary>
		/// Handles it-click to order row-ids.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Order(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
			{
				if (!Table.Readonly)
				{
					bool changed = false;

					string val;
					int result;

					for (int r = 0; r != Table.RowCount; ++r)
					{
						if (String.IsNullOrEmpty(val = Table[r,0].text)
							|| !Int32.TryParse(val, out result)
							|| result != r)
						{
							Table[r,0].text = r.ToString();
							changed = true;
						}
					}

					if (changed)
					{
						Table.Changed = true;
						Table._ur.ResetSaved(true);
					}

					if (changed)
					{
						if      (Table == _diff1) _diff1 = null;
						else if (Table == _diff2) _diff2 = null;

						Table.Colwidth(0, 0, Table.RowCount - 1);
						Table.metricFrozenControls(0);

						Table.InitScroll();

						int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);
						if (Table.Propanel != null && Table.Propanel.Visible)
							invalid |= YataGrid.INVALID_PROP;

						Table.Invalidator(invalid);
					}
				}
				else
					ReadonlyError();
			}
		}

		/// <summary>
		/// Handles it-click to test row-ids.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_TestOrder(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
			{
				var list = new List<string>();

				string val;
				int result;

				bool stop = false;

				for (int id = 0; id != Table.RowCount; ++id)
				{
					if (String.IsNullOrEmpty(val = Table[id,0].text))
					{
						if (list.Count == 16) // stop this Madness
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is not valid");
					}
					else if (!Int32.TryParse(val, out result))
					{
						if (list.Count == 16)
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is not an integer");
					}
					else if (result != id)
					{
						if (list.Count == 16)
						{
							stop = true;
							break;
						}
						list.Add("id " + id + " is out of order");
					}
				}

				if (list.Count != 0)
				{
					string info = String.Empty;
					foreach (string it in list)
					{
						info += it + Environment.NewLine;
					}

					if (stop)
					{
						info += Environment.NewLine
							  + "The check has been stopped at 16 borks.";
					}

					if (!Table.Readonly)
					{
						info += Environment.NewLine + Environment.NewLine
							  + "Do you want to auto-order the ID fields?";

						if (MessageBox.Show(info,
											" burp",
											MessageBoxButtons.YesNo,
											MessageBoxIcon.Exclamation,
											MessageBoxDefaultButton.Button1) == DialogResult.Yes)
						{
							opsclick_Order(null, EventArgs.Empty);
						}
					}
					else
						MessageBox.Show(info,
										" burp",
										MessageBoxButtons.OK,
										MessageBoxIcon.Exclamation,
										MessageBoxDefaultButton.Button1);
				}
				else
					MessageBox.Show("Row order is Okay.",
									" burp",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
			}
		}


		/// <summary>
		/// Handles it-click to autosize cols.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void opsclick_AutosizeCols(object sender, EventArgs e)
		{
			if (Table != null)
			{
				Obfuscate();
				DrawingControl.SuspendDrawing(Table);

				AutosizeCols(Table);

				Obfuscate(false);
				DrawingControl.ResumeDrawing(Table);
			}
		}

		/// <summary>
		/// Autosizes all cols of a given table.
		/// </summary>
		/// <param name="table"></param>
		/// <remarks>Helper for
		/// <c><see cref="opsclick_AutosizeCols()">opsclick_AutosizeCols()</see></c>
		/// but is also called by
		/// <c><see cref="DiffReset()">DiffReset()</see></c>.</remarks>
		void AutosizeCols(YataGrid table)
		{
			foreach (var col in table.Cols)
				col.UserSized = false;

			table.Calibrate(0, table.RowCount - 1); // autosize
		}

		/// <summary>
		/// Handles it-click to recolor rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Recolor(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // NOTE: 'RowCount' shall never be 0
			{
				Row row;
				Brush brush;

				for (int id = 0; id != Table.RowCount; ++id)
				{
					brush = (id % 2 == 0) ? Brushes.Alice
										  : Brushes.Bob;

					(row = Table.Rows[id])._brush = brush;

					for (int c = 0; c != Table.ColCount; ++c)
						row[c].loadchanged = false;
				}
				Table.Invalidator(YataGrid.INVALID_GRID);
			}
		}


		/// <summary>
		/// Handles it-click to freeze first col.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Freeze1stCol(object sender, EventArgs e)
		{
			if (Table != null && Table.ColCount > 1)
			{
				Table.Select();

				it_freeze2.Checked = false;

				if (it_freeze1.Checked = !it_freeze1.Checked)
				{
					var col = Table.Cols[1];
					if (col.UserSized)
					{
						col.UserSized = false;
						Table.Colwidth(1);
					}
					Table.FrozenCount = YataGrid.FreezeFirst;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}

		/// <summary>
		/// Handles it-click to freeze second col.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Freeze2ndCol(object sender, EventArgs e)
		{
			if (Table != null && Table.ColCount > 2)
			{
				Table.Select();

				it_freeze1.Checked = false;

				if (it_freeze2.Checked = !it_freeze2.Checked)
				{
					var col = Table.Cols[1];
					if (col.UserSized)
					{
						col.UserSized = false;
						Table.Colwidth(1);
					}

					col = Table.Cols[2];
					if (col.UserSized)
					{
						col.UserSized = false;
						Table.Colwidth(2);
					}

					Table.FrozenCount = YataGrid.FreezeSecond;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}


		/// <summary>
		/// Handler for the PropertyPanel's visibility.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <seealso cref="mouseup_btnPropertyPanel()"><c>mouseup_btnPropertyPanel()</c></seealso>
		void opsclick_PropertyPanelOnOff(object sender, EventArgs e)
		{
			if (Table != null) togglePropanel();
		}

		/// <summary>
		/// Toggles visibility of the PropertyPanel - instantiates a
		/// PropertyPanel if required.
		/// </summary>
		void togglePropanel()
		{
			if (Table.Propanel == null
				|| (Table.Propanel.Visible = !Table.Propanel.Visible))
			{
				if (Table.Propanel == null)
					Table.Propanel = new PropertyPanel(Table);
				else
					Table.Propanel.rewidthValfield();

				Table.Propanel.Show();
				Table.Propanel.BringToFront();

				Table.Propanel.Dockstate = Table.Propanel.Dockstate;

				it_ppLocation.Enabled = true;
			}
			else
			{
				Table.Propanel.Hide();
				it_ppLocation.Enabled = false;
			}
		}

		/// <summary>
		/// Handler for the PropertyPanel's position.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <seealso cref="mouseup_btnPropertyPanel()"><c>mouseup_btnPropertyPanel()</c></seealso>
		void opsclick_PropertyPanelLocation(object sender, EventArgs e)
		{
			if (Table != null && Table.Propanel != null && Table.Propanel.Visible)
				Table.Propanel.Dockstate = Table.Propanel.getNextDockstate();
		}


		/// <summary>
		/// Starts an external diff/merger program with the two diffed files
		/// opened. Usually WinMerge.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_ExternalDiff(object sender, EventArgs e)
		{
			if (File.Exists(Settings._diff))
			{
				var diff = new Process();
				diff.StartInfo.FileName = Settings._diff;

				if (_diff1 != null && _diff2 != null
					&& File.Exists(_diff1.Fullpath)
					&& File.Exists(_diff2.Fullpath))
				{
					diff.StartInfo.Arguments = " \"" + _diff1.Fullpath + "\" \"" + _diff2.Fullpath + "\"";
				}
				else
					diff.StartInfo.Arguments = " \"" + Table.Fullpath + "\"";

				diff.Start();
			}
		}


		/// <summary>
		/// Clears the Undo/Redo stacks.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_ClearUr(object sender, EventArgs e)
		{
			if (Table != null)
			{
				Table._ur.Clear();

				// force GC
				long bytes = GC.GetTotalMemory(false);
				GC.Collect();
				GC.WaitForPendingFinalizers();

				bytes -= GC.GetTotalMemory(true);

				MessageBox.Show("Estimated memory freed : " + String.Format("{0:n0}", bytes) + " bytes",
								" burp",
								MessageBoxButtons.OK,
								MessageBoxIcon.Information,
								MessageBoxDefaultButton.Button1);
			}
		}
		#endregion Events (2daOps)


		#region Events (font)
		/// <summary>
		/// Handles opening the FontMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void font_dropdownopening(object sender, EventArgs e)
		{
			if (Table != null && Table._editor.Visible)
			{
				Table._editor.Visible = false;
				Table.Invalidator(YataGrid.INVALID_GRID);
			}

			it_FontDefault.Enabled = !Font.Equals(FontDefault)
								  && !it_Font.Checked;
		}

		/// <summary>
		/// Opens the FontPicker form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Font(object sender, EventArgs e)
		{
			// var f = Application.OpenForms["FontF"];
			if (_fontF == null)
			{
				it_Font.Checked = true;

				_fontF = new FontF(this);
				_fontF.Show(this);
			}
			else
			{
				if (_fontF.WindowState == FormWindowState.Minimized)
				{
					if (_fontF.Maximized)
						_fontF.WindowState = FormWindowState.Maximized;
					else
						_fontF.WindowState = FormWindowState.Normal;
				}
				_fontF.BringToFront();
			}
		}

		/// <summary>
		/// Sets the form's font to the default Font.
		/// @note The item will be disabled if the FontPicker is open or if the
		/// form's current Font is equal to the FontDefault font.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Default(object sender, EventArgs e)
		{
			doFont(FontDefault.Clone() as Font);
		}
		#endregion Events (font)


		#region Methods (font)
		/// <summary>
		/// Dechecks the "Font ... be patient" menuitem and re-enables the "Load
		/// default font" menuitem when the font-dialog closes.
		/// @note The latter item is disabled while the font dialog is open.
		/// </summary>
		internal void FontF_closing()
		{
			_fontF = null;
			it_Font.Checked = false;
		}

		/// <summary>
		/// Applies a specified font to the Form.
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
			// Font-objects effectively bypasses the OS' DPI user-setting.

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
				DrawingControl.SuspendDrawing(Table);

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

				DrawingControl.ResumeDrawing(Table);
				Obfuscate(false);

				if (_fontF != null)			// layout for big tables will send the Font dialog below the form ->
					_fontF.BringToFront();	// (although it should never be behind the form because its owner IS the form)
			}
		}
		#endregion Methods (font)


		#region Events (help)
		/// <summary>
		/// Handles it-click to open ReadMe.txt.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void helpclick_Help(object sender, EventArgs e)
		{
			string path = Path.Combine(Application.StartupPath, "ReadMe.txt");
			if (File.Exists(path))
				Process.Start(path);
			else
				MessageBox.Show("ReadMe.txt was not found in the application directory.",
								" burp",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Handles it-click to open the About box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void helpclick_About(object sender, EventArgs e)
		{
			using (var f = new About())
				f.ShowDialog(this);
		}
		#endregion Events (help)


		#region Events (tab)
		/// <summary>
		/// Sets the selected tab when a right-click on a tab is about to open
		/// the context.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabMenu_Opening(object sender, CancelEventArgs e)
		{
			Table._editor.Visible = false;
			Table.Invalidator(YataGrid.INVALID_GRID);

			bool found = false;

			var pt = Tabs.PointToClient(Cursor.Position); // select the Tab itself ->
			for (int tab = 0; tab != Tabs.TabCount; ++tab)
			{
				if (Tabs.GetTabRect(tab).Contains(pt))
				{
					Tabs.SelectedIndex = tab;
					found = true;
					break;
				}
			}

			if (found)
			{
				it_tabCloseAll      .Enabled =
				it_tabCloseAllOthers.Enabled = (Tabs.TabCount != 1);

				it_tabSave          .Enabled = !Table.Readonly;

				it_tabReload        .Enabled = File.Exists(Table.Fullpath);

				// NOTE: 'it_tabDiff1' is always enabled.
				it_tabDiff2    .Enabled = (_diff1 != null && _diff1 != Table);
				it_tabDiffReset.Enabled = (_diff1 != null || _diff2 != null);
				it_tabDiffSync .Enabled = (_diff1 != null && _diff2 != null);

				if (_diff1 != null)
					it_tabDiff1.Text = "diff1 - " + Path.GetFileNameWithoutExtension(_diff1.Fullpath);
				else
					it_tabDiff1.Text = "Select diff1";

				if (_diff2 != null)
					it_tabDiff2.Text = "diff2 - " + Path.GetFileNameWithoutExtension(_diff2.Fullpath);
				else
					it_tabDiff2.Text = "Select diff2";
			}
			else
				e.Cancel = true;
		}

		/// <summary>
		/// Closes all other tables when a tab's context-closeall item is
		/// clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_CloseAllOtherTabs(object sender, EventArgs e)
		{
			if (!CheckChangedTables("close", true))
			{
				DrawingControl.SuspendDrawing(this); // stops tab-flickering on Remove tab

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
				{
					if (tab != Tabs.SelectedIndex)
						ClosePage(Tabs.TabPages[tab]);
				}

				SetTabSize();

				it_SaveAll.Enabled = AllowSaveAll();

				DrawingControl.ResumeDrawing(this);
			}
		}


		// TODO: FreezeFirst/Second, gotoloadchanged, etc.


		/// <summary>
		/// Selects <c><see cref="_diff1"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_Diff1(object sender, EventArgs e)
		{
			tabclick_DiffReset(null, EventArgs.Empty);
			_diff1 = Table;
		}

		/// <summary>
		/// Selects <c><see cref="_diff2"/></c>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_Diff2(object sender, EventArgs e)
		{
			if (_fdiffer != null) _fdiffer.Close();
			if (_diff2   != null) DiffReset(_diff2);

			_diff2 = Table;
			if (doDiff())
				tabclick_DiffSync(null, EventArgs.Empty);
			else
				_diff1 = _diff2 = null;
		}

		/// <summary>
		/// Clears all diffed cells and nulls any pointers to diffed tables.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		#endregion Events (tab)


		#region Methods (tab)
		/// <summary>
		/// Helper for
		/// <c><see cref="tabclick_DiffReset()">tabclick_DiffReset()</see></c>.
		/// </summary>
		/// <param name="table"><c><see cref="YataGrid"/></c></param>
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
			_diff1.ClearSelects(true);
			_diff2.ClearSelects(); // <- current Table


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
					if (!String.IsNullOrEmpty(copyable))
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
				if (!String.IsNullOrEmpty(copyable))
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
				if (!String.IsNullOrEmpty(copyable))
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
				if (!String.IsNullOrEmpty(copyable))
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
				if (!String.IsNullOrEmpty(copyable))
				{
					copyable += Environment.NewLine;
					if (!prelinedone)
						copyable += Environment.NewLine;
				}
				copyable += "Cell texts: " + celldiffs + " (exclusive)";
			}


			string label;
			Color color;
			if (String.IsNullOrEmpty(copyable))
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
										this);
			_fdiffer.SetLabelColor(color);
			if (@goto)           _fdiffer.ShowGotoButton();
			if (@goto || isDiff) _fdiffer.ShowResetButton();

			_fdiffer.Show(); // is not owned, will be disposed auto.

			return isDiff || @goto;
		}

		/// <summary>
		/// Selects the next diffed cell in the table (or both tables if both
		/// are valid).
		/// </summary>
		/// <remarks>Frozen cells will be selected but they don't respect
		/// <c><see cref="YataGrid.EnsureDisplayed()">YataGrid.EnsureDisplayed()</see></c>.
		/// They get no respect ...</remarks>
		internal void GotoDiffCell()
		{
			if (WindowState == FormWindowState.Minimized)
				WindowState  = FormWindowState.Normal;
			else
			{
				TopMost = true;
				TopMost = false;
			}

			if (Table != null
				&& (_diff1 != null  || _diff2 != null)
				&& (Table == _diff1 || Table == _diff2))
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				Table.Select();

				YataGrid table; // the other table - can be null.

				if (Table == _diff1) table = _diff2;
				else                 table = _diff1;

				Cell sel = Table.getSelectedCell();
				int rStart = Table.getSelectedRow();

				Table.ClearSelects();

				if (table != null)
					table.ClearSelects(true);

				int r,c;

				bool start = true;

				if ((ModifierKeys & Keys.Shift) == 0) // forward goto ->
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
			_fdiffer.EnableGotoButton(false);
		}

		/// <summary>
		/// Helper for <c><see cref="GotoDiffCell()">GotoDiffCell()</see></c>.
		/// </summary>
		/// <param name="sel"><c><see cref="Cell"/></c> in the current table</param>
		/// <param name="table">the other <c><see cref="YataGrid"/></c></param>
		void gotodiff(Cell sel, YataGrid table)
		{
			Table.SelectCell(sel, false);

			if (table != null
				&& sel.x < table.ColCount
				&& sel.y < table.RowCount)
			{
				table[sel.y, sel.x].selected = true;
			}
		}

		/// <summary>
		/// Syncs two diffed <c><see cref="YataGrid">YataGrids</see></c> when a
		/// <c><see cref="Cell"/></c> or <c><see cref="Row"/></c> gets selected.
		/// </summary>
		/// <param name="sel"><c><see cref="Cell"/></c> in the current table -
		/// can be <c>null</c></param>
		/// <param name="r"></param>
		/// <returns><c>true</c> if diff-tables are valid</returns>
		/// <remarks><c><see cref="_table"/></c> is the other synced
		/// <c><see cref="YataGrid"/></c></remarks>
		internal bool SyncSelect(Cell sel = null, int r = -1)
		{
			if (_diff1 != null && _diff2 != null)
			{
				if      (Table == _diff1) _table = _diff2;
				else if (Table == _diff2) _table = _diff1;
				else return false;

				_table.ClearSelects(true);
				if (sel != null)
				{
					if (sel.y < _table.RowCount && sel.x < _table.ColCount)
						_table[sel.y, sel.x].selected = true;
				}
				else if (r != -1 && r < _table.RowCount)
				{
					// Do not call _table.SelectRow() since that's a recursion.
					Row row = _table.Rows[r];
					row.selected = true;
					for (int c = 0; c != _table.ColCount; ++c)
						row[c].selected = true;
				}
				_table = null;
				return true;
			}
			return false;
		}
		#endregion Methods (tab)


		#region Methods (statusbar)
		/// <summary>
		/// Mouseover cells prints table-cords plus PathInfo to the statusbar if
		/// a relevant 2da (eg. Crafting, Spells, Feat) is loaded.
		/// </summary>
		/// <param name="cords">null to clear statusbar-cords and -pathinfo</param>
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
								if (result == TalkReader.invalid)
								{
									text = gs.Space;
								}
								else
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
							}

							if (!String.IsNullOrEmpty(text))
							{
								string[] array = text.Split(gs.SEPARATORS, StringSplitOptions.None);

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


		#region Events (dragdrop)
		/// <summary>
		/// Handles dragging a file onto Yata.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void yata_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Handles dropping a file(s) onto Yata.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void yata_DragDrop(object sender, DragEventArgs e)
		{
			var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string pfe in paths)
				CreatePage(pfe);
		}
		#endregion Events (dragdrop)


		#region Events (propanel)
		/// <summary>
		/// Handler for MouseDown on the PropertyPanel button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void mousedown_btnPropertyPanel(object sender, MouseEventArgs e)
		{
			if (Table != null)
			{
				if (    e.Button == MouseButtons.Left
					|| (e.Button == MouseButtons.Right
						&& Table.Propanel != null && Table.Propanel.Visible))
				{
					btn_ProPanel.Depressed = true;
					btn_ProPanel.Invalidate();
				}
			}
		}

		/// <summary>
		/// Handler for <c>MouseUp</c> on the PropertyPanel button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <seealso cref="opsclick_PropertyPanelOnOff()"><c>opsclick_PropertyPanelOnOff()</c></seealso>
		/// <seealso cref="opsclick_PropertyPanelLocation()"><c>opsclick_PropertyPanelLocation()</c></seealso>
		void mouseup_btnPropertyPanel(object sender, MouseEventArgs e)
		{
			if (Table != null)
			{
				Table.Select();

				if (e.Button == MouseButtons.Left)
				{
					btn_ProPanel.Depressed = false;
					btn_ProPanel.Invalidate();

					togglePropanel();
				}
				else if (e.Button == MouseButtons.Right
					&& Table.Propanel != null && Table.Propanel.Visible)
				{
					btn_ProPanel.Depressed = false;
					btn_ProPanel.Invalidate();

					Table.Propanel.Dockstate = Table.Propanel.getNextDockstate();
				}
			}
		}
		#endregion Events (propanel)


		#region Methods (cell)
		/// <summary>
		/// Shows the cell popup menu.
		/// </summary>
		internal void popupCellmenu()
		{
			_sel = Table.getSelectedCell(); // '_sel' shall be valid due to rightclick

			it_cellEdit   .Enabled = !Table.Readonly;

			it_cellCut    .Enabled = !Table.Readonly;
			it_cellPaste  .Enabled = !Table.Readonly;
			it_cellDelete .Enabled = !Table.Readonly
								  && (_sel.text != gs.Stars || _sel.loadchanged);

			it_cellLower  .Enabled = !Table.Readonly
								  && (_sel.text != _sel.text.ToLower() || _sel.loadchanged);
			it_cellUpper  .Enabled = !Table.Readonly
								  && (_sel.text != _sel.text.ToUpper() || _sel.loadchanged);

			it_cellMergeCe.Enabled =
			it_cellMergeRo.Enabled = isMergeEnabled();

			it_cellStrref .Enabled = isStrrefEnabled();

			switch (Table.Info)
			{
				case YataGrid.InfoType.INFO_NONE:
				case YataGrid.InfoType.INFO_CRAFT:
					it_cellInput.Visible =
					it_cellInput.Enabled = false;
					break;

				// TODO: If table is Readonly allow viewing the InfoInput dialog
				// but disable its controls ->

				case YataGrid.InfoType.INFO_SPELL:
					it_cellInput.Text    = "InfoInput (spells.2da)";
					it_cellInput.Visible = true;
					it_cellInput.Enabled = !Table.Readonly && isSpellsInfoInputCol();
					break;

				case YataGrid.InfoType.INFO_FEAT:
					it_cellInput.Text    = "InfoInput (feat.2da)";
					it_cellInput.Visible = true;
					it_cellInput.Enabled = !Table.Readonly && isFeatInfoInputCol();
					break;
			}

			Point loc = Table.PointToClient(Cursor.Position);
			cellMenu.Show(Table, loc);
		}

		/// <summary>
		/// Helper for <c><see cref="popupCellmenu()">popupCellmenu()</see></c>.
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

				return (table != null && !table.Readonly
					 && table.ColCount > _sel.x
					 && table.RowCount > _sel.y);
			}
			return false;
		}

		/// <summary>
		/// Helper for <c><see cref="popupCellmenu()">popupCellmenu()</see></c>.
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
		/// Helper for <c><see cref="popupCellmenu()">popupCellmenu()</see></c>.
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


		#region Events (cell)
		/// <summary>
		/// Handles cell-click edit.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Edit(object sender, EventArgs e)
		{
			Table.startCelledit(_sel);
		}

		/// <summary>
		/// Handles cell-click cut.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Cut(object sender, EventArgs e)
		{
			_copytext = new string[,] {{ _sel.text }};

			if (_sel.text != gs.Stars)
			{
				Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check, does Invalidate
			}
			else if (_sel.loadchanged)
			{
				_sel.loadchanged = false;
				Table.Invalidator(YataGrid.INVALID_GRID);
			}
		}

		/// <summary>
		/// Handles cell-click copy.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Copy(object sender, EventArgs e)
		{
			_copytext = new string[,] {{ _sel.text }};
		}

		/// <summary>
		/// Handles cell-click delete.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Delete(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, gs.Stars); // does not do a text-check, does Invalidate
		}

		/// <summary>
		/// Handles cell-click lowercase.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Lower(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, _sel.text.ToLower()); // does not do a text-check, does Invalidate
		}

		/// <summary>
		/// Handles cell-click uppercase.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Upper(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, _sel.text.ToUpper()); // does not do a text-check, does Invalidate
		}

		/// <summary>
		/// Handles a single-cell merge operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_MergeCe(object sender, EventArgs e)
		{
			YataGrid table;
			if (Table == _diff1) table = _diff2;
			else                 table = _diff1;

			int r = _sel.y;
			int c = _sel.x;

			Cell dst = table[r,c];
			table.ChangeCellText(dst, _sel.text); // does not do a text-check, does Invalidate

			_diff1[r,c].diff =
			_diff2[r,c].diff = false;
		}

		/// <summary>
		/// Handles a single-row merge operation.
		/// </summary>
		/// <param name="sender"></param>
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
				table[r,c].text = Table[r,c].text; // NOTE: Strings are immutable so no need for copy/clone - is done auto.
				table[r,c].diff = false;

				Table[r,c].diff = false;
			}

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
		/// Handles cell-click InfoInput dialog.
		/// </summary>
		/// <param name="sender"></param>
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
							using (var f = new InfoInputSpells(Table, _sel))
							{
								if (f.ShowDialog(this) == DialogResult.OK
									&& str1 != str0)
								{
									Table.ChangeCellText(_sel, str1); // does not do a text-check
								}
							}
							break;

						case InfoInputSpells.MetaMagic: // HEX Input ->
						case InfoInputSpells.TargetType:
						case InfoInputSpells.AsMetaMagic:
							using (var f = new InfoInputSpells(Table, _sel))
							{
								if (f.ShowDialog(this) == DialogResult.OK
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

										Table.ChangeCellText(_sel, "0x" + int1.ToString(q)); // does not do a text-check
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
							using (var f = new InfoInputFeat(Table, _sel))
							{
								if (f.ShowDialog(this) == DialogResult.OK
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
			using (var iis = new InfoInputSpells(Table, _sel))
				doIntInput(iis);
		}

		/// <summary>
		/// - helper for
		/// <c><see cref="cellclick_InfoInput()">cellclick_InfoInput()</see></c>
		/// </summary>
		void doIntInputFeat()
		{
			using (var iif = new InfoInputFeat(Table, _sel))
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
				else                         val = int1.ToString();

				Table.ChangeCellText(_sel, val); // does not do a text-check
			}
		}


		/// <summary>
		/// Handler for the cell-context's sub "STRREF" <c>DropDownOpening</c>
		/// event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void dropdownopening_Strref(object sender, EventArgs e)
		{
			bool invalid = (_strInt == TalkReader.invalid);

			if (invalid || (_strInt & TalkReader.bitCusto) == 0)
				it_cellStrref_custom.Text = "set Custom";
			else
				it_cellStrref_custom.Text = "clear Custom";

			it_cellStrref_custom .Enabled =
			it_cellStrref_invalid.Enabled = !Table.Readonly && !invalid;
		}

		/// <summary>
		/// Handler for cell-context "STRREF" click. Opens a 'TalkDialog' that
		/// displays the text's corresponding Dialog.Tlk or special entry in a
		/// readonly RichTextBox for the user's investigation and/or copying.
		/// @note Check that the cell's text parses to a valid value before
		/// allowing the event to trigger (ie, else disable the context it - see
		/// <see cref="popupCellmenu"/> and <see cref="dropdownopening_Strref"/>).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Strref_talktable(object sender, EventArgs e)
		{
			_strref = _sel.text;

			using (var f = new TalkDialog(_sel, this))
			{
				if (f.ShowDialog(this) == DialogResult.OK
					&& _strref != _sel.text)
				{
					Table.ChangeCellText(_sel, _strref); // does not do a text-check
					Invalidate();	// lolziMScopter - else the titlebar and borders can arbitrarily disappear.
				}					// nobody knows why ... q TwilightZone
			}
		}

		/// <summary>
		/// Handler for cell-context "set/clear Custom" click. Toggles the
		/// custom-bit flag.
		/// @note Check that the cell's text parses to a valid value before
		/// allowing the event to trigger (ie, else disable the context it - see
		/// <see cref="popupCellmenu"/> and <see cref="dropdownopening_Strref"/>).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>The invalid strref (-1) cannot be toggled.</remarks>
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

			Table.ChangeCellText(_sel, _strInt.ToString()); // does not do a text-check
		}

		/// <summary>
		/// Handler for cell-context "set Invalid (-1)" click. Sets a strref to
		/// "-1" if not already.
		/// @note Check that the cell's text parses to a valid value before
		/// allowing the event to trigger (ie, else disable the context it - see
		/// <see cref="popupCellmenu"/> and <see cref="dropdownopening_Strref"/>).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Strref_invalid(object sender, EventArgs e)
		{
			Table.ChangeCellText(_sel, gs.Invalid); // does not do a text-check
		}
		#endregion Events (cell)
	}


	#region Delegates
	/// <summary>
	/// Good fuckin Lord I just wrote a "DontBeep" delegate.
	/// </summary>
	internal delegate void DontBeepEventHandler();
	#endregion Delegates
}
