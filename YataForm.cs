using System;
//using System.Collections;
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
	partial class YataForm
		:
			Form
	{
		#region Enumerators
		/// <summary>
		/// Defines field fill-types used by the CreateRows dialog.
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
		const string TITLE    = " Yata";
		const string ASTERICS = " *";

		internal static string pfe_load; // cl arg

		internal static bool IsSaveAll;

		static Graphics graphics;
		#endregion Fields (static)


		#region Fields
		readonly YataTabs tabControl = new YataTabs();
		readonly PropertyPanelButton btn_ProPanel = new PropertyPanelButton();

		/// <summary>
		/// A list used for copy/paste row(s).
		/// </summary>
		List<string[]> _copy = new List<string[]>();

		/// <summary>
		/// A string used for Copy/PasteCell.
		/// @note A cell's text shall never be null or blank, therefore
		/// '_copytext' shall never be null or blank.
		/// </summary>
		string _copytext = gs.Stars;

		string _preset = String.Empty;

		internal int _startCr, _lengthCr;
		internal CrFillType _fillCr;

		internal Font FontAccent;

		internal bool isSearch;
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
		/// A pointer to a 'YataGrid' table that will be used during the save-
		/// routine (and other places where it's convenient). Is required
		/// because it can't be assumed that the current 'Table' is the table
		/// being saved; that is, the SaveAll operation needs to cycle through
		/// all tables: See fileclick_SaveAll().
		/// </summary>
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
		/// Int for InfoInputDialog (PathInfo).
		/// </summary>
		internal int intOriginal, intInput;

		/// <summary>
		/// String for InfoInputDialog (PathInfo).
		/// </summary>
		internal string stOriginal, stInput;

		internal bool IsMin; // works in conjunction w/ YataGrid.OnResize()

		List<string> Strrefheads = new List<string>();
		internal string _strref; // the strref set by 'TalkDialog'

		int _track_x = -1; // tracks last mouseover coords ->
		int _track_y = -1;
		#endregion Fields


		#region Properties (static)
		internal static YataGrid Table // there can be only 1 Table.
		{ get; private set; }
		#endregion Properties (static)


		#region Properties
		internal TabControl Tabs
		{ get { return tabControl; } }

		Font FontDefault
		{ get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataForm()
		{
			InitializeTabControl();
			InitializePropertyPanelButton();

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


			YataGraphics.graphics = CreateGraphics(); //Graphics.FromHwnd(IntPtr.Zero))
			YataGraphics.hFontDefault = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, Font);

			FontDefault = Font;

			Settings.ScanSettings(); // load the Optional settings file Settings.Cfg

			if (Settings._font != null)
				Font = Settings._font;
			else
				Settings._fontdialog = new Font(Font.Name, Font.Size, Font.Style, Font.Unit);

			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));

			if (Settings._font2 != null)
			{
				// Relative Font-sizes (as defined in the Designer):
				//
				// menubar, contextEditor, statusbar, tabMenu, cellMenu = unity.
				// context_it_Header     = +0.5
				// statusbar_label_Cords = -0.5
				// statusbar_label_Info  = +1.0

				menubar.Font.Dispose();
				menubar.Font = Settings._font2;

				ContextEditor.Font.Dispose();
				ContextEditor.Font = Settings._font2;

				statusbar.Font.Dispose();
				statusbar.Font = Settings._font2;

				statbar_lblCords.Font.Dispose();
				statbar_lblCords.Font = new Font(Settings._font2.FontFamily,
												 Settings._font2.SizeInPoints - 0.5f);

				statbar_lblInfo.Font.Dispose();
				statbar_lblInfo.Font = new Font(Settings._font2.FontFamily,
												Settings._font2.SizeInPoints + 1.0f);

				int hBar = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, statbar_lblInfo.Font) + 2;

				statusbar       .Height = (hBar + 5 < 22) ? 22 : hBar + 5;
				statbar_lblCords.Height =
				statbar_lblInfo .Height = (hBar     < 17) ? 17 : hBar;

//				int wCords0 = statbar_lblCords.Width;
//				int wCords = YataGraphics.MeasureWidth(YataGraphics.WIDTH_CORDS, statbar_lblInfo.Font) + 10;
//				statbar_lblCords.Width = (wCords < wCords0) ? wCords0 : wCords;
				statbar_lblCords.Width = YataGraphics.MeasureWidth(YataGraphics.WIDTH_CORDS, statbar_lblCords.Font) + 20;


				context_it_Header.Font.Dispose();
				context_it_Header.Font = new Font(Settings._font2.FontFamily,
												  Settings._font2.SizeInPoints + 0.5f,
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
			ClientSize = new Size(w,h);


			cb_SearchOption.Items.AddRange(new object[]
			{
				"find substring",
				"find wholeword"
			});
			cb_SearchOption.SelectedIndex = 0;


			YataGrid.SetStaticMetrics(this);

			btn_ProPanel.Top = -1; // NOTE: This won't work in PP button's cTor. So do it here.


			if (File.Exists(pfe_load)) // load file from FileExplorer ...
				CreatePage(pfe_load);
			else
				Obfuscate();

			DontBeepEvent += HandleDontBeepEvent;

			if (Settings._recent != 0)
				InitializeRecentFiles();

			if (  TalkReader.Load(Settings._dialog,    it_PathTalkD)
				| TalkReader.Load(Settings._dialogalt, it_PathTalkC, true))
			{
				TalkReader.LoadTalkingHeads(Strrefheads);
			}
		}


		/// <summary>
		/// Initializes the TabControl.
		/// </summary>
		void InitializeTabControl()
		{
			Tabs.Name          = "Tabs";
			Tabs.TabIndex      = 3;
			Tabs.SelectedIndex = 0;

			Tabs.Dock      = DockStyle.Fill;
			Tabs.Multiline = true;
			Tabs.AllowDrop = true;
			Tabs.DrawMode  = TabDrawMode.OwnerDrawFixed;
			Tabs.SizeMode  = TabSizeMode.Fixed;

			Tabs.Location = new Point(0,24);
			Tabs.Size     = new Size(842,408);
			Tabs.Padding  = new Point(0,0); // Padding uses Point and Margin uses Padding
			Tabs.Margin   = new Padding(0); // right got it.

			Tabs.DrawItem             += tab_DrawItem;
			Tabs.SelectedIndexChanged += tab_SelectedIndexChanged;

			Controls.Add(Tabs);
		}

		/// <summary>
		/// Initializes the PropertyPanelButton.
		/// </summary>
		void InitializePropertyPanelButton()
		{
			btn_ProPanel.Name     = "btn_PropertyPanel";
			btn_ProPanel.TabIndex = 4;
			btn_ProPanel.TabStop  = false;

			btn_ProPanel.Visible = false;

			btn_ProPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btn_ProPanel.UseVisualStyleBackColor = true;

			btn_ProPanel.Location = new Point(823,0);
			btn_ProPanel.Size     = new Size(20,20);
			btn_ProPanel.Margin   = new Padding(0);

			btn_ProPanel.MouseDown += mousedown_btnPropertyPanel;
			btn_ProPanel.MouseUp   += mouseup_btnPropertyPanel;

			Controls.Add(btn_ProPanel);
		}

		void InitializeRecentFiles()
		{
			string dir = Application.StartupPath;
			string pfe = Path.Combine(dir, "recent.cfg");
			if (File.Exists(pfe))
			{
				ToolStripItemCollection recents = it_Recent.DropDownItems;

				string[] lines = File.ReadAllLines(pfe);
				foreach (string line in lines)
				{
					if (File.Exists(line))
					{
						var it = new ToolStripMenuItem(line);
						it.Click += fileclick_Recent;
						recents.Add(it);

						if (recents.Count == Settings._recent)
							break;
					}
				}
			}
		}
		#endregion cTor


		#region Events (override)
		/// <summary>
		/// Sends (unhandled) mousewheel events on the Form to the table.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (Table != null)
				Table.Scroll(e);

//			base.OnMouseWheel(e);
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
				case Keys.Control | Keys.C:					// bypass Ctrl+c and Ctrl+v if the editor is visible.
				case Keys.Control | Keys.V:					// this bypasses the Edit menuitems and lets the editbox
					if (Table != null						// take the message if/when the editbox is visible.
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
		/// Handles the KeyDown event on the form.
		/// @note Requires the form's KeyPreview property flagged true in order
		/// to handle the event if a control is focused.
		/// @note Fires repeatedly if a key is held depressed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			//logfile.Log("YataForm.OnKeyDown() e.KeyData= " + e.KeyData);

			switch (e.KeyCode)
			{
				case Keys.Enter: // do this here to get rid of the beep.
					if (Table != null && Table.RowCount != 0) // rowcount should never be "0"
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
			}
			base.OnKeyDown(e);
		}

		/// <summary>
		/// Forwards a keydown [Enter] event to an appropriate funct.
		/// @note Is basically just a convoluted handler for the OnKeyDown()
		/// handler to stop the *beep* if [Enter] is keyed when a textbox is
		/// focused.
		/// </summary>
		void HandleDontBeepEvent()
		{
			switch (_dontbeep)
			{
				case DONTBEEP_SEARCH:
					doSearch();
					break;

				case DONTBEEP_GOTO:
					Table.doGoto(tb_Goto.Text);
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
					{
						CreatePage(pfe_load);
					}
				}
			}
			else
				base.WndProc(ref m);
		}
		#endregion Receive Message (pfe)


		#region Methods (static)
		internal static FontStyle getStyleStandard(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Regular))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(styleTest)) // determine first available style (any) of Family ->
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}

		static FontStyle getStyleAccented(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			if (!ff.IsStyleAvailable(style = FontStyle.Underline))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle)))
			{
				if (ff.IsStyleAvailable(styleTest)) // determine first available style (any) of Family ->
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}
		#endregion Methods (static)


		#region Methods
		/// <summary>
		/// Obscures or unobscures the table behind a dedicated color-panel.
		/// Can be called before and after calibrating and drawing the table in
		/// order to hide unsightly .NET spaz-attacks (despite double-buffering
		/// etc).
		/// </summary>
		/// <param name="obscure">true to bring the color-panel to front, false
		/// to send the color-panel to back</param>
		internal void Obfuscate(bool obscure = true)
		{
			if (obscure) panel_ColorFill.BringToFront();
			else         panel_ColorFill.SendToBack();
		}

		/// <summary>
		/// TODO: This funct is obsolete; tables that are Readonly shall not
		/// fire events that can change the table at all.
		/// </summary>
		internal void ReadonlyError()
		{
			MessageBox.Show("The 2da-file is opened as readonly.",
							" burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Hand,
							MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Checks if there is a non-readonly table open and optionally calls
		/// all tables' Leave event handler.
		/// </summary>
		/// <param name="fireLeave">true to fire the LeaveEvent on all tables</param>
		/// <returns></returns>
		bool allowSaveAll(bool fireLeave = false)
		{
			bool allow = false;
			for (int i = 0; i != Tabs.TabCount; ++i)
			{
				_table = Tabs.TabPages[i].Tag as YataGrid;
				if (!_table.Readonly)
				{
					allow = true;
					if (!fireLeave) break;
				}

				if (fireLeave)
					_table.leave_Grid(null, EventArgs.Empty);
			}
			_table = null;
			return allow;
		}
		#endregion Methods


		#region Events
		/// <summary>
		/// Hides the editor. This is a generic handler that should be invoked
		/// when opening a menu on the menubar (iff the menu doesn't have its
		/// own dropdown-handler).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// @note The filename w/out extension must not be blank since
		/// YataGrid.Init() is going to use blank as a fallthrough while
		/// determining the grid's 'InfoType' to call GropeLabels().
		/// </summary>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read">readonly (default false)</param>
		void CreatePage(string pfe, bool read = false)
		{
			if (File.Exists(pfe)													// safety (probably).
				&& !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(pfe)))	// what idjut would ... oh, wait.
			{
				if (Settings._recent != 0)
				{
					ToolStripItemCollection recents = it_Recent.DropDownItems;
					ToolStripItem it;

					byte bits = 0;
					for (int i = 0; i != recents.Count; ++i)
					{
						it = recents[i];
						if (it.Text == pfe)
						{
							if (i == 0) bits = 1;
							else
							{
								bits = 2;
								recents.Remove(it);
							}
							break;
						}
					}

					if ((bits & 1) == 0)
					{
						it = new ToolStripMenuItem(pfe);
						it.Click += fileclick_Recent;
						recents.Insert(0, it);

						if (bits == 0)
						{
							int count = recents.Count;
							if (count > Settings._recent)
								recents.Remove(recents[count - 1]);
						}
					}
				}

				Obfuscate();
//				Refresh();	// NOTE: If a table is already loaded the color-panel doesn't show
							// but a refresh turns the client area gray at least instead of glitchy.
							// NOTE: It went away.

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
					SetTabSize();

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
			if (Tabs.SelectedIndex != -1)
			{
				Table = Tabs.SelectedTab.Tag as YataGrid; // <- very Important <--||

				Obfuscate(false);

				Cell sel = Table.getSelectedCell();

				btn_ProPanel .Visible = true;

				it_MenuPaths .Visible = Table.Info != YataGrid.InfoType.INFO_NONE;

				it_freeze1   .Checked = Table.FrozenCount == YataGrid.FreezeFirst;
				it_freeze2   .Checked = Table.FrozenCount == YataGrid.FreezeSecond;

				it_Readonly  .Checked = Table.Readonly;

				it_Undo      .Enabled = Table._ur.CanUndo;
				it_Redo      .Enabled = Table._ur.CanRedo;

				it_Reload    .Enabled = File.Exists(Table.Fullpath);
				it_Save      .Enabled = !Table.Readonly;
				it_SaveAll   .Enabled = allowSaveAll(true);
				it_SaveAs    .Enabled =
				it_Readonly  .Enabled =
				it_Close     .Enabled =
				it_CloseAll  .Enabled = true;

				it_CopyCell  .Enabled = sel != null;
				it_PasteCell .Enabled = !Table.Readonly && it_CopyCell.Enabled;
				it_CopyRange .Enabled = Table.getSelectedRow() != -1;
				it_PasteRange.Enabled = !Table.Readonly && _copy.Count != 0;
				it_CreateRows.Enabled = !Table.Readonly;

				it_OrderRows .Enabled = !Table.Readonly;
				it_CheckRows .Enabled =
				it_ColorRows .Enabled =
				it_AutoCols  .Enabled =
				it_ppOnOff   .Enabled = true;
				it_ppLocation.Enabled = Table.Propanel != null && Table.Propanel.Visible;
				it_ExternDiff.Enabled = File.Exists(Settings._diff);

				it_freeze1   .Enabled = Table.Cols.Count > 1;
				it_freeze2   .Enabled = Table.Cols.Count > 2;


				if (Table.Propanel != null && Table.Propanel.Visible)
				{
					Table.Propanel.telemetric();
					if (sel != null)
						Table.Propanel.EnsureDisplayed(sel.x);
				}

				if (_fdiffer != null)
					_fdiffer.EnableGotoButton(true);
			}
			else
			{
				Obfuscate();

				btn_ProPanel .Visible =
				it_MenuPaths .Visible =

				it_freeze1   .Checked =
				it_freeze2   .Checked =

				it_Readonly  .Checked =

				it_Undo      .Enabled =
				it_Redo      .Enabled =

				it_Reload    .Enabled =
				it_Save      .Enabled =
				it_SaveAll   .Enabled =
				it_SaveAs    .Enabled =
				it_Readonly  .Enabled =
				it_Close     .Enabled =
				it_CloseAll  .Enabled =

				it_CopyCell  .Enabled = 
				it_PasteCell .Enabled =
				it_CopyRange .Enabled =
				it_PasteRange.Enabled =
				it_CreateRows.Enabled =

				it_OrderRows .Enabled =
				it_CheckRows .Enabled =
				it_ColorRows .Enabled =
				it_AutoCols  .Enabled =
				it_ppOnOff   .Enabled =
				it_ppLocation.Enabled =
				it_ExternDiff.Enabled =

				it_freeze1   .Enabled =
				it_freeze2   .Enabled = false;

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

			int y;

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

			var font = new Font(Font.Name, Font.SizeInPoints - 0.5f, style);

			// NOTE: MS doc for DrawText() says that using a Point doesn't work on Win2000 machines.
			int w = YataGraphics.MeasureWidth(tab.Text, font);
			var rect = e.Bounds;
			rect.X   = e.Bounds.X + (e.Bounds.Width - w) / 2;
			rect.Y   = e.Bounds.Y + y; // NOTE: 'y' is a padding tweak.

			graphics = e.Graphics;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			Color color;
			if (((YataGrid)tab.Tag).Readonly)
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
		#endregion Events (tabs)


		#region Methods (tabs)
		/// <summary>
		/// Sets the width of the tabs on the TabControl.
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
		/// Disposes a tab's table's 'FileWatcher' before a specified tab-page
		/// is removed from the tab-collection.
		/// </summary>
		/// <param name="tab">the tab-page with which to deal</param>
		void CloseTabpage(TabPage tab)
		{
			_table = tab.Tag as YataGrid;
			_table.Watcher.Dispose();

			if      (_table == _diff1) _diff1 = null;
			else if (_table == _diff2) _diff2 = null;

			if (_diff1 == null && _diff2 == null && _fdiffer != null)
				_fdiffer.Close();

			Tabs.TabPages.Remove(tab);
			_table = null;
		}

		/// <summary>
		/// Sets the tabtext of a specified table.
		/// @note Is called by YataGrid.Changed flag.
		/// </summary>
		/// <param name="table"></param>
		internal void SetTabText(YataGrid table)
		{
			DrawingControl.SuspendDrawing(this); // stop tab-flicker on Sort etc.

			string asterics = table.Changed ? ASTERICS : String.Empty;
			foreach (TabPage tab in Tabs.TabPages)
			{
				if ((YataGrid)tab.Tag == table)
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
		/// @note Is called by fileclick_SaveAll().
		/// </summary>
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
		/// <returns>true if row-order is okay</returns>
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void yata_Closing(object sender, CancelEventArgs e)
		{
			if (Tabs.TabPages.Count == 1)
			{
				e.Cancel = Table.Changed
						&& MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
										   " warning",
										   MessageBoxButtons.YesNo,
										   MessageBoxIcon.Warning,
										   MessageBoxDefaultButton.Button2) == DialogResult.No;
			}
			else if (Tabs.TabPages.Count > 1)
			{
				var tables = GetChangedTables();
				if (tables.Count != 0)
				{
					string info = String.Empty;
					foreach (string table in tables)
					{
						info += table + Environment.NewLine;
					}

					e.Cancel = MessageBox.Show("Data has changed."
											   + Environment.NewLine + Environment.NewLine
											   + info
											   + Environment.NewLine
											   + "Okay to exit ...",
											   " warning",
											   MessageBoxButtons.YesNo,
											   MessageBoxIcon.Warning,
											   MessageBoxDefaultButton.Button2) == DialogResult.No;
				}
			}

			if (!e.Cancel && Settings._recent != 0)
			{
				int i = -1;
				var recents = new string[it_Recent.DropDownItems.Count];
				foreach (ToolStripMenuItem recent in it_Recent.DropDownItems)
					recents[++i] = recent.Text;

				string dir = Application.StartupPath;
				string pfe = Path.Combine(dir, "recent.cfg");
				try
				{
					File.WriteAllLines(pfe, recents);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		#endregion Events (close)


		#region Methods (close)
		/// <summary>
		/// Returns a list of currently loaded tables that have been modified.
		/// </summary>
		/// <param name="excludecurrent">true to exclude the current Table</param>
		/// <returns></returns>
		List<string> GetChangedTables(bool excludecurrent = false)
		{
			var tables = new List<string>();

			YataGrid table;
			foreach (TabPage page in Tabs.TabPages)
			{
				table = page.Tag as YataGrid;
				if (table.Changed && (!excludecurrent || table != Table))
				{
					tables.Add(Path.GetFileNameWithoutExtension(table.Fullpath).ToUpperInvariant());
				}
			}
			return tables;
		}
		#endregion Methods (close)


		#region Events (file)
		/// <summary>
		/// Handles opening the FileMenu, FolderPresets item and its sub-items.
		/// </summary>
		/// <param name="sender"></param>
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

				it_Reload  .Enabled = File.Exists(Table.Fullpath);
				it_SaveAll .Enabled = allowSaveAll();
				it_Save    .Enabled = !Table.Readonly;
			}
			else
			{
				it_Reload  .Enabled =
				it_SaveAll .Enabled =
				it_Save    .Enabled = false;
			}


			_preset = String.Empty;

			ToolStripItemCollection presets = it_OpenFolder.DropDownItems;
			presets.Clear();
			foreach (var dir in Settings._dirpreset)
			{
				if (Directory.Exists(dir))
				{
					var preset = presets.Add(dir);
					preset.Click += fileclick_OpenFolder;
				}
			}
			it_OpenFolder.Visible = (presets.Count != 0);

			it_Recent.Visible = (it_Recent.DropDownItems.Count != 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_OpenFolder(object sender, EventArgs e)
		{
			var it = sender as ToolStripMenuItem;
			_preset = it.Text;

			fileclick_Open(null, EventArgs.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_Open(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.InitialDirectory = _preset;

				ofd.Title  = "Select a 2da file";
				ofd.Filter = "2da files (*.2da)|*.2da|All files (*.*)|*.*";

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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void fileclick_Reload(object sender, EventArgs e)
		{
			if (Table != null && File.Exists(Table.Fullpath)
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
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
					fileclick_CloseTab(null, EventArgs.Empty);
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
		/// Opens a 2da-file from the Recent-files list. Removes the item from
		/// the list if it was not found on disk.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_Recent(object sender, EventArgs e)
		{
			var it = sender as ToolStripMenuItem;
			string pfe = it.Text;
			if (File.Exists(pfe))
				CreatePage(pfe);
			else
				it_Recent.DropDownItems.Remove(it);
		}

		/// <summary>
		/// Allows user to set the current table's readonly flag after it has
		/// been opened. (The readonly setter appears otherwise only on the
		/// file-open dialog.)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_Readonly(object sender, EventArgs e)
		{
			Table.Readonly = it_Readonly.Checked;
			Tabs.Invalidate();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void fileclick_Save(object sender, EventArgs e)
		{
			if (Table != null) // safety I believe.
			{
				bool force; // force a Readonly file to overwrite itself (only if invoked by SaveAs)
				bool bypassReadonly;

				var it = sender as ToolStripMenuItem;

				if (it == it_SaveAs)
				{
					_table = Table;
					// '_pfeT' is set by caller
					force = (_pfeT == _table.Fullpath);
					bypassReadonly = false;
				}
				else if (it == it_SaveAll)
				{
					// '_pfeT' and '_table' are set by caller
					force = false;
					bypassReadonly = false;
				}
				else // is rego-save or tab-save or 'FileWatcherDialog' save
				{
					_table = Table;
					_pfeT = _table.Fullpath;
					force = false;

					if (it == it_Save || it == it_tabSave)
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

							int rows = _table.RowCount;
							if (rows != 0) // rowcount should never be zero ...
							{
								_table.Changed = false;
								_table._ur.ResetSaved();

								foreach (var row in _table.Rows)
								{
									for (int c = 0; c != _table.ColCount; ++c)
										row[c].loadchanged = false;
								}

								if (_table == Table)
									_table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);

								using (var sw = new StreamWriter(_table.Fullpath))
								{
									sw.WriteLine("2DA V2.0");					// header ->
									sw.WriteLine("");

									string line = String.Empty;
									foreach (string field in _table.Fields)		// col-fields ->
									{
										line += " " + field;
									}
									sw.WriteLine(line);


									string val;

									int cols = _table.ColCount;

									for (int r = 0; r != rows; ++r)				// row-cells ->
									{
										line = String.Empty;

										for (int c = 0; c != cols; ++c)
										{
											if (c != 0)
												line += " ";

											if (!String.IsNullOrEmpty(val = _table[r,c].text))
												line += val;
											else
												line += gs.Stars;
										}

										sw.WriteLine(line);
									}

									_table.Watcher.Pfe = _table.Fullpath;
									_table.Watcher.BypassFileDeleted = false;
									_table.Watcher.BypassFileChanged = true;
								}
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_SaveAs(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // rowcount should never be zero ...
			{
				using (var sfd = new SaveFileDialog())
				{
					sfd.Title    = "Save as ...";
					sfd.Filter   = "2da files (*.2da)|*.2da|All files (*.*)|*.*";
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_SaveAll(object sender, EventArgs e)
		{
			if (allowSaveAll())
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void fileclick_CloseTab(object sender, EventArgs e)
		{
			if (Table != null
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   " warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				CloseTabpage(Tabs.SelectedTab);

				if (Tabs.TabCount == 0)
					Table = null;

				SetTabSize();
				SetTitlebarText();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fileclick_CloseAllTabs(object sender, EventArgs e)
		{
			bool close = true;

			var tables = GetChangedTables();
			if (tables.Count != 0)
			{
				string info = String.Empty;
				foreach (string table in tables)
				{
					info += table + Environment.NewLine;
				}

				close = MessageBox.Show("Data has changed."
										+ Environment.NewLine + Environment.NewLine
										+ info
										+ Environment.NewLine
										+ "Okay to exit ...",
										" warning",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Warning,
										MessageBoxDefaultButton.Button2) == DialogResult.Yes;
			}

			if (close)
			{
				Table = null;

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
					CloseTabpage(Tabs.TabPages[tab]);

				SetTitlebarText();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
			context_it_PasteBelow .Enabled = !Table.Readonly && _copy.Count != 0;

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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_Header(object sender, EventArgs e)
		{
			ContextEditor.Hide();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditCopy(object sender, EventArgs e)
		{
			_copy.Clear();

			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = Table[_r,c].text;

			_copy.Add(fields);
		}

		/// <summary>
		/// 
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditPasteAbove(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.Insert(_r, _copy[0]);

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */


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
		/// 
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
					if (c < _copy[0].Length)
						field = _copy[0][c];
					else
						field = gs.Stars;

					row[c].text = field;
					row[c].diff =
					row[c].loadchanged = false;
				}
				row._brush = Brushes.Created;

				Table.Calibrate(_r);

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditPasteBelow(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.Insert(_r + 1, _copy[0]);

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */


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
		/// 
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

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */


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
		/// 
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

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */
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
		/// 
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

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */


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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void contextclick_EditDelete(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Restorable rest = UndoRedo.createRow(Table.Rows[_r], UndoRedo.UrType.rt_Insert);


				Table.Insert(_r);

/*				int invalid = (YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ | YataGrid.INVALID_ROWS);
				if (Table.Propanel != null && Table.Propanel.Visible)
					invalid |= YataGrid.INVALID_PROP;

				Table.Invalidator(invalid); */


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
		/// Handles opening the EditMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void edit_dropdownopening(object sender, EventArgs e)
		{
			it_Searchnext     .Enabled = (Table != null && !String.IsNullOrEmpty(tb_Search.Text));
			it_GotoLoadchanged.Enabled = false;

			if (Table != null && Table.RowCount != 0) // rowcount should never be "0"
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Invalidator(YataGrid.INVALID_GRID);
				}

				foreach (var row in Table.Rows)
				{
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if (row[c].loadchanged)
						{
							it_GotoLoadchanged.Enabled = true;
							break;
						}
					}
				}

				it_CopyCell  .Enabled = (Table.getSelectedCell() != null);
				it_PasteCell .Enabled = (it_CopyCell.Enabled && !Table.Readonly);

				it_CopyRange .Enabled = (Table.getSelectedRow() != -1);
				it_PasteRange.Enabled = (_copy.Count != 0 && !Table.Readonly);
			}
			else
			{
				it_CopyCell  .Enabled = 
				it_PasteCell .Enabled =

				it_CopyRange .Enabled =
				it_PasteRange.Enabled = false;
			}

			it_CreateRows.Enabled = (Table != null && !Table.Readonly);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void textchanged_Search(object sender, EventArgs e)
		{
			it_Searchnext.Enabled = !String.IsNullOrEmpty(tb_Search.Text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_Search(object sender, EventArgs e)
		{
			tb_Search.Focus();
			tb_Search.SelectAll();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void enter_Search(object sender, EventArgs e)
		{
			_firstclick = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_SearchNext(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0) // rowcount should never be "0"
			{
				Table.Select(); // [F3] shall focus the table, [Enter] shall keep focus on the tb/cbx.
				Search();
			}
		}

		/// <summary>
		/// Performs a search when the Enter-key is pressed and focus is on
		/// either the search-box or the search-option dropdown.
		/// </summary>
		void doSearch()
		{
			isSearch = true; // [Enter] shall keep focus on the tb/cbx, [F3] shall focus the table.
			Search();
			isSearch = false;
		}

		/// <summary>
		/// Searches the current table for the string in the search-box.
		/// NOTE: Ensure that 'Table' is valid before call.
		/// </summary>
		void Search()
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

				if ((ModifierKeys & Keys.Shift) != Keys.Shift) // forward search ->
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
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void textchanged_Goto(object sender, EventArgs e)
		{
			int result;
			if (!Int32.TryParse(tb_Goto.Text, out result)
				|| result < 0)
			{
				tb_Goto.Text = "0";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_Goto(object sender, EventArgs e)
		{
			tb_Goto.Focus();
			tb_Goto.SelectAll();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void enter_Goto(object sender, EventArgs e)
		{
			_firstclick = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
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
		/// @note This is fired only from the EditMenu (click/Ctrl+N) and its
		/// item is enabled by default. The item/shortcut will be set disabled
		/// either when the EditMenu opens or when Ctrl+N is keyed iff there are
		/// no 'loadchanged' cells.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_GotoLoadchanged(object sender, EventArgs e)
		{
			bool found = false;
			if (Table != null)
			{
				for (int r = 0; r != Table.RowCount && !found; ++r)
				for (int c = 0; c != Table.ColCount && !found; ++c)
				{
					if (Table[r,c].loadchanged)
						found = true;
				}
			}

			if (it_GotoLoadchanged.Enabled = found)
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

				if ((ModifierKeys & Keys.Shift) != Keys.Shift) // forward goto ->
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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_CopyCell(object sender, EventArgs e)
		{
			if (Table != null)
			{
				Cell sel = Table.getSelectedCell();
				if (sel != null) // safety (believe it or not).
				{
					_copytext = sel.text;
				}
				else
					CopyPasteCellError();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_PasteCell(object sender, EventArgs e)
		{
			if (tb_Goto.Focused)
			{
				SetTextboxText(tb_Goto);
			}
			else if (tb_Search.Focused)
			{
				SetTextboxText(tb_Search);
			}
			else if (Table != null)
			{
				if (!Table.Readonly)
				{
					Cell sel = Table.getSelectedCell();
					if (sel != null)
					{
						if (sel.text != _copytext)
							Table.ChangeCellText(sel, _copytext); // does not do a text-check
					}
					else
						CopyPasteCellError();
				}
				else
					ReadonlyError();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_CopyRange(object sender, EventArgs e)
		{
			_copy.Clear();

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

				_copy.Add(fields);
				++rFirst;
			}

			if (!Table.Readonly)
				it_PasteRange.Enabled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_PasteRange(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Obfuscate();
				DrawingControl.SuspendDrawing(Table);


				Restorable rest = UndoRedo.createArray(_copy.Count, UndoRedo.UrType.rt_ArrayDelete);

				int selr = Table.getSelectedRow();
				if (selr == -1)
					selr = Table.RowCount;

				int r = selr;
				for (int i = 0; i != _copy.Count; ++i, ++r)
				{
					Table.Insert(r, _copy[i], false);
					rest.array[i] = Table.Rows[r].Clone() as Row;
				}

				Table.Calibrate(selr, _copy.Count - 1); // paste range

				Table.ClearSelects();
				Table.Rows[selr].selected = true;
				Table.RangeSelect = _copy.Count - 1;
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
		/// Instantiates 'RowCreatorDialog' for inserting/creating multiple
		/// blank rows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_CreateRows(object sender, EventArgs e)
		{
			if (Table != null)
			{
				if (!Table.Readonly)
				{
					int selr = Table.getSelectedRow();

					using (var f = new RowCreatorDialog(this, selr + 1, _copy.Count != 0))
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
										if (i < _copy[0].Length)
											cells[i] = _copy[0][i];
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
		#endregion Events (edit)


		#region Methods (edit)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableUndo(bool enable)
		{
			it_Undo.Enabled = enable;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enable"></param>
		internal void EnableRedo(bool enable)
		{
			it_Redo.Enabled = enable;
		}

		/// <summary>
		/// @note Is called by Row.selected.
		/// </summary>
		/// <param name="enabled"></param>
		internal void EnableCopyRange(bool enabled)
		{
			it_CopyRange.Enabled = enabled;
		}


		/// <summary>
		/// Helper for editclick_PasteCell().
		/// </summary>
		/// <param name="tb"></param>
		void SetTextboxText(ToolStripItem tb)
		{
			if (Clipboard.ContainsText(TextDataFormat.Text))
			{
				tb.Text = Clipboard.GetText(TextDataFormat.Text);
			}
			else if (_copytext != gs.Stars || tb == tb_Search)
			{
				tb.Text = _copytext;
			}
			else
				tb.Text = String.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		void CopyPasteCellError()
		{
			MessageBox.Show("Select one (1) cell.",
							" burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation,
							MessageBoxDefaultButton.Button1);
		}
		#endregion Methods (edit)


		#region Events (clipboard)
		/// <summary>
		/// Outputs the current contents of '_copy' to the Windows clipboard.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void clipclick_ExportCopy(object sender, EventArgs e)
		{
			string clip = String.Empty;
			for (int i = 0; i != _copy.Count; ++i)
			{
				for (int j = 0; j != _copy[i].Length; ++j)
				{
					if (j != 0) clip += " ";
					clip += _copy[i][j];
				}

				if (i != _copy.Count - 1)
					clip += Environment.NewLine;
			}
			ClipboardAssistant.SetText(clip);
		}

		/// <summary>
		/// Imports the current contents of the Windows clipboard to '_copy'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void clipclick_ImportCopy(object sender, EventArgs e)
		{
			_copy.Clear();

			string clip = Clipboard.GetText(TextDataFormat.Text);
			if (!String.IsNullOrEmpty(clip))
			{
				string[] lines = clip.Split(new[]{ Environment.NewLine }, StringSplitOptions.None);
				for (int i = 0; i != lines.Length; ++i)
				{
					string[] fields = YataGrid.ParseTableRow(lines[i]);
					_copy.Add(fields);
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
		/// 
		/// </summary>
		internal void Clip_uncheck()
		{
			it_OpenClipEditor.Checked = false;
		}
		#endregion Methods (clipboard)


		#region Events (2daOps)
		void ops_dropdownopening(object sender, EventArgs e)
		{
			bool valid = (Table != null);

			it_OrderRows .Enabled = valid && !Table.Readonly;
			it_CheckRows .Enabled =
			it_ColorRows .Enabled =
			it_AutoCols  .Enabled =
			it_ppOnOff   .Enabled = valid;
			it_ppLocation.Enabled = valid && Table.Propanel != null && Table.Propanel.Visible;

			it_freeze1   .Enabled = valid && Table.Cols.Count > 1;
			it_freeze2   .Enabled = valid && Table.Cols.Count > 2;

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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Order(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
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

						Table.DeterColwidth(0, 0, Table.RowCount - 1);
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
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_TestOrder(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
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
		/// 
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
		/// @note Is also called by DiffReset().
		/// </summary>
		/// <param name="table"></param>
		void AutosizeCols(YataGrid table)
		{
			foreach (var c in table.Cols)
				c.UserSized = false;

			table.Calibrate(0, table.RowCount - 1); // autosize
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void opsclick_Recolor(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
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
		/// 
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
						Table.DeterColwidth(1);
					}
					Table.FrozenCount = YataGrid.FreezeFirst;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}

		/// <summary>
		/// 
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
						Table.DeterColwidth(1);
					}

					col = Table.Cols[2];
					if (col.UserSized)
					{
						col.UserSized = false;
						Table.DeterColwidth(2);
					}

					Table.FrozenCount = YataGrid.FreezeSecond;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}


		/// <summary>
		/// Handler for the PropertyPanel's visibility.
		/// Cf mouseup_btnPropertyPanel()
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// Cf mouseup_btnPropertyPanel()
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// Opens the FontPicker form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Font(object sender, EventArgs e)
		{
			var f = Application.OpenForms["FontF"];
			if (f == null)
			{
				it_FontDefault.Enabled = false;

				it_Font.Checked = true;
				f = new FontF(this);
				f.Show(this);
			}
			else
				f.BringToFront();
		}

		/// <summary>
		/// Sets the form's font to the default Font.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Default(object sender, EventArgs e)
		{
			if (!Font.Equals(FontDefault))
				doFont(FontDefault);
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
			it_Font.Checked = false;
			it_FontDefault.Enabled = true;
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

			Font = font; // rely on GC here
			FontAccent.Dispose();
			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));
			Settings._fontdialog.Dispose();
			Settings._fontdialog = Settings.CreateDialogFont(Font);


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

					int invalid = table.EnsureDisplayed();
					if (table == Table)
						table.Invalidator(invalid);
				}

				Obfuscate(false);
				DrawingControl.ResumeDrawing(Table);
			}
		}
		#endregion Methods (font)


		#region Events (help)
		/// <summary>
		/// 
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
		/// 
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
			bool close = true;

			var tables = GetChangedTables(true);
			if (tables.Count != 0)
			{
				string info = String.Empty;
				foreach (string table in tables)
				{
					info += table + Environment.NewLine;
				}

				close = MessageBox.Show("Data has changed."
										+ Environment.NewLine + Environment.NewLine
										+ info
										+ Environment.NewLine
										+ "Okay to exit ...",
										" warning",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Warning,
										MessageBoxDefaultButton.Button2) == DialogResult.Yes;
			}

			if (close)
			{
				DrawingControl.SuspendDrawing(this); // stops tab-flickering on Remove tab

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
				{
					if (tab != Tabs.SelectedIndex)
						CloseTabpage(Tabs.TabPages[tab]);
				}

				SetTitlebarText();
				SetTabSize();

				DrawingControl.ResumeDrawing(this);
			}
		}


		// TODO: FreezeFirst/Second, gotoloadchanged, etc.


		/// <summary>
		/// Selects '_diff1'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_Diff1(object sender, EventArgs e)
		{
			tabclick_DiffReset(null, EventArgs.Empty);
			_diff1 = Table;
		}

		/// <summary>
		/// Selects '_diff2'.
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
		/// Helper for tabclick_DiffReset().
		/// @note Check that 'table' is not null before call.
		/// </summary>
		/// <param name="table"></param>
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
		/// <returns>true if differences are found</returns>
		bool doDiff()
		{
			_diff1.ClearSelects();
			_diff2.ClearSelects();


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
		/// @note Frozen cells will be selected but they don't respect
		/// EnsureDisplayed(). They get no respect ...
		/// </summary>
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
					table.ClearSelects();

				int r,c;

				bool start = true;

				if ((ModifierKeys & Keys.Shift) != Keys.Shift) // forward goto ->
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
		/// Helper for GotoDiffCell().
		/// </summary>
		/// <param name="sel">cell in the current table</param>
		/// <param name="table">the other table</param>
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
		/// Syncs two diffed tables when a cell or row gets selected.
		/// </summary>
		/// <param name="sel"></param>
		/// <param name="r"></param>
		/// <returns>true if diff-tables are valid</returns>
		internal bool SyncSelect(Cell sel = null, int r = -1)
		{
			if (_diff1 != null && _diff2 != null)
			{
				if      (Table == _diff1) _table = _diff2;
				else if (Table == _diff2) _table = _diff1;
				else return false;

				_table.ClearSelects();
				if (sel != null)
				{
					if (sel.y < _table.RowCount && sel.x < _table.ColCount)
						_table[sel.y, sel.x].selected = true;
				}
				else if (r != -1)
				{
					if (r < _table.RowCount)
					{
						// Do not call _table.SelectRow() since that's a recursion.
						Row row = _table.Rows[r];
						row.selected = true;
						for (int c = 0; c != _table.ColCount; ++c)
							row[c].selected = true;
					}
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
		/// a relevant 2da (eg. Crafting, Spells) is loaded.
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
								&& result > -1 && result < (TalkReader.bitCusto | TalkReader.strref))
							{
								bool alt = ((result & TalkReader.bitCusto) != 0);
								result &= TalkReader.strref;

								if (!alt && TalkReader.DictDialo.ContainsKey(result))
								{
									text = TalkReader.DictDialo[result];
								}
								else if (alt && TalkReader.DictCusto.ContainsKey(result))
								{
									text = TalkReader.DictCusto[result];
								}
							}

							if (!String.IsNullOrEmpty(text))
							{
								string[] array = text.Split(gs.SEPARATORS, StringSplitOptions.None);

								text = array[0];
								if (text.Length > 99)
									text = text.Substring(0, 99) + " ...";
								else if (array.Length > 1)
									text += " ...";

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
		internal void yata_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		internal void yata_DragDrop(object sender, DragEventArgs e)
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string file in files)
				CreatePage(file);
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
				if (e.Button == MouseButtons.Left
					|| (e.Button == MouseButtons.Right
						&& Table.Propanel != null && Table.Propanel.Visible))
				{
					btn_ProPanel.Depressed = true;
					btn_ProPanel.Invalidate();
				}
			}
		}

		/// <summary>
		/// Handler for MouseUp on the PropertyPanel button.
		/// Cf opsclick_PropertyPanelOnOff()
		/// Cf opsclick_PropertyPanelTopBot()
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// 
		/// </summary>
		internal void ShowCellMenu()
		{
			Cell sel = Table.getSelectedCell();

			it_cellEdit   .Enabled =
			it_cellPaste  .Enabled = !Table.Readonly;
			it_cellStars  .Enabled = !Table.Readonly && (sel.text != gs.Stars || sel.loadchanged);
			it_cellMergeCe.Enabled = 
			it_cellMergeRo.Enabled = isMergeEnabled(sel);
			it_cellInput  .Enabled = Table.Info == YataGrid.InfoType.INFO_SPELL && isInfoInputCol(sel.x);
			it_cellStrref .Enabled = isStrrefEnabled(sel);

			Point loc = Table.PointToClient(Cursor.Position);
			cellMenu.Show(Table, loc);
		}

		/// <summary>
		/// Helper for ShowCellMenu().
		/// </summary>
		/// <param name="sel"></param>
		/// <returns>true if Merge (cell or row) will be enabled</returns>
		bool isMergeEnabled(Cell sel)
		{
			if (sel.diff && _diff1 != null && _diff2 != null)
			{
				YataGrid destTable = null;
				if      (Table == _diff1) destTable = _diff2;
				else if (Table == _diff2) destTable = _diff1;

				return (destTable != null && !destTable.Readonly
					 && destTable.ColCount > sel.x
					 && destTable.RowCount > sel.y);
			}
			return false;
		}

		/// <summary>
		/// Helper for ShowCellMenu().
		/// </summary>
		/// <param name="c">the col of the selected cell</param>
		/// <returns></returns>
		bool isInfoInputCol(int c)
		{
			switch (c)
			{
				case InfoInputDialog.School:
				case InfoInputDialog.Range:
				case InfoInputDialog.MetaMagic:
				case InfoInputDialog.TargetType:
				case InfoInputDialog.ImmunityType:
				case InfoInputDialog.UserType:
				case InfoInputDialog.AsMetaMagic:
					return true;

				case InfoInputDialog.Category:
					if (Info.categoryLabels.Count != 0)
						return true;
					break;

				case InfoInputDialog.TargetingUI:
					if (Info.targetLabels.Count != 0)
						return true;
					break;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sel"></param>
		/// <returns>true if TalkEntry dialog will be enabled</returns>
		bool isStrrefEnabled(Cell sel)
		{
			if (sel.x != 0 && Strrefheads.Contains(Table.Fields[sel.x - 1]))
			{
				string strref = sel.text;
				if (strref == gs.Stars) strref = "0";

				int result;
				return Int32.TryParse(strref, out result)
					&& result > -1 && result < (TalkReader.bitCusto | TalkReader.strref);
			}
			return false;
		}
		#endregion Methods (cell)


		#region Events (cell)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_EditCell(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.startCelledit();
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Stars(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Cell sel = Table.getSelectedCell();
				if (sel != null)
				{
					Table.ChangeCellText(sel, gs.Stars); // does not do a text-check
				}
				else
					CopyPasteCellError();
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Handles a single-cell merge operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_MergeCe(object sender, EventArgs e)
		{
			YataGrid destTable = null;
			if      (Table == _diff1) destTable = _diff2;
			else if (Table == _diff2) destTable = _diff1;

			if (destTable != null) // safety.
			{
				Cell src = Table.getSelectedCell();
				int r = src.y;
				int c = src.x;

				Cell dst = destTable[r,c];
				destTable.ChangeCellText(dst, src.text); // does not do a text-check

				_diff1[r,c].diff =
				_diff2[r,c].diff = false;
			}
		}

		/// <summary>
		/// Handles a single-row merge operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_MergeRo(object sender, EventArgs e)
		{
			YataGrid destTable = null;
			if      (Table == _diff1) destTable = _diff2;
			else if (Table == _diff2) destTable = _diff1;

			if (destTable != null) // safety.
			{
				int r = Table.getSelectedCell().y;

				// - store the row's current state to 'rPre' in the Restorable
				Restorable rest = UndoRedo.createRow(destTable.Rows[r]);

				int c = 0;
				for (; c != destTable.ColCount && c != Table.ColCount; ++c)
				{
					destTable[r,c].text = Table[r,c].text; // NOTE: Strings are immutable so no need for copy/clone - is done auto.
					destTable[r,c].diff = false;

					Table[r,c].diff = false;
				}

				if (destTable.ColCount > Table.ColCount)
				{
					for (; c != destTable.ColCount; ++c)
					{
						destTable[r,c].text = gs.Stars;
						destTable[r,c].diff = false;
					}
				}
				else if (destTable.ColCount < Table.ColCount)
				{
					for (; c != Table.ColCount; ++c)
						Table[r,c].diff = false;
				}

				Table.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_FROZ);

				if (!destTable.Changed)
				{
					destTable.Changed = true;
					rest.isSaved = UndoRedo.IsSavedType.is_Undo;
				}

				// - store the row's changed state to 'rPos' in the Restorable
				rest.rPos = destTable.Rows[r].Clone() as Row;
				destTable._ur.Push(rest);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Input(object sender, EventArgs e)
		{
			Cell cell = Table.getSelectedCell();
			Form f;

			switch (cell.x)
			{
				case InfoInputDialog.School: // STRING Input ->
				case InfoInputDialog.Range:
				case InfoInputDialog.ImmunityType:
				case InfoInputDialog.UserType:
				case InfoInputDialog.TargetingUI:
					using (f = new InfoInputDialog(Table, cell))
					{
						if (f.ShowDialog(this) == DialogResult.OK
							&& stInput != stOriginal)
						{
							Table.ChangeCellText(cell, stInput); // does not do a text-check
						}
					}
					break;

				case InfoInputDialog.MetaMagic: // HEX Input ->
				case InfoInputDialog.TargetType:
				case InfoInputDialog.AsMetaMagic:
					using (f = new InfoInputDialog(Table, cell))
					{
						if (f.ShowDialog(this) == DialogResult.OK
							&& intInput != intOriginal)
						{
							string format;
							if (intInput <= 0xFF) format = "X2";
							else                  format = "X6";

							Table.ChangeCellText(cell, "0x" + intInput.ToString(format)); // does not do a text-check
						}
					}
					break;

				case InfoInputDialog.Category: // INT Input ->
					using (f = new InfoInputDialog(Table, cell))
					{
						if (f.ShowDialog(this) == DialogResult.OK
							&& intInput != intOriginal)
						{
							Table.ChangeCellText(cell, intInput.ToString()); // does not do a text-check
						}
					}
					break;
			}
		}

		/// <summary>
		/// Handler for cell-context "strref" - opens a 'TalkDialog' that
		/// displays the text's corresponding Dialog.Tlk entry in a readonly
		/// RichTextBox for the user's investigation and/or copying.
		/// @note Check that the cell's text parses to a valid non-negative
		/// integer, or blank "****", before allowing the event to trigger (ie,
		/// else disable the context it - see ShowCellMenu()).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void cellclick_Strref(object sender, EventArgs e)
		{
			Cell sel = Table.getSelectedCell();
			_strref = sel.text;

			using (var f = new TalkDialog(sel, this))
			{
				if (f.ShowDialog(this) == DialogResult.OK
					&& _strref != sel.text)
				{
					Table.ChangeCellText(sel, _strref);
					Invalidate();	// lolziMScopter - else the titlebar and borders can arbitrarily disappear.
				}					// nobody knows why ... q TwilightZone
			}
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
