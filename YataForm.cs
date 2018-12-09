using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Yata ....
	/// </summary>
	public partial class YataForm
		:
			Form
	{
		#region Fields & Properties
		internal static string pfe_load; // cl arg

		static YataGrid Table // there can be only 1 Table.
		{ get; set; }

		List<string[]> _copy = new List<string[]>();
		string _copycell = String.Empty;

		string _preset = String.Empty;

		internal TabControl Tabs
		{ get { return tabControl; } }

		Font FontDefault
		{ get; set; }

		internal Font FontAccent;

		internal bool _search;

		static Graphics graphics;
		#endregion Fields & Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal YataForm()
		{
			InitializeComponent();

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

			FontDefault = Font;

			Settings.ScanSettings(); // load an Optional manual settings file

			if (Settings._font != null)
			{
				Font = Settings._font;
			}

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

				contextEditor.Font.Dispose();
				contextEditor.Font = Settings._font2;

				statusbar.Font.Dispose();
				statusbar.Font = Settings._font2;

				statusbar_label_Cords.Font.Dispose();
				statusbar_label_Cords.Font = new Font(Settings._font2.FontFamily,
													  Settings._font2.SizeInPoints - 0.5f);

				statusbar_label_Info.Font.Dispose();
				statusbar_label_Info.Font = new Font(Settings._font2.FontFamily,
													 Settings._font2.SizeInPoints + 1.0f);

				int hBar = YataGraphics.MeasureHeight(YataGraphics.HEIGHT_TEST, statusbar_label_Info.Font) + 2;

				statusbar            .Height = (hBar + 5 < 22) ? 22 : hBar + 5;
				statusbar_label_Cords.Height =
				statusbar_label_Info .Height = (hBar     < 17) ? 17 : hBar;

				int wCords0 = statusbar_label_Cords.Width;
				int wCords = YataGraphics.MeasureWidth(YataGraphics.WIDTH_CORDS, statusbar_label_Info.Font) + 10;
				statusbar_label_Cords.Width = (wCords < wCords0) ? wCords0 : wCords;


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

			btn_PropertyPanel.Top = -1; // NOTE: This won't work in PP button's cTor. So do it here.

			if (!String.IsNullOrEmpty(pfe_load)
				&& File.Exists(pfe_load))
			{
				CreateTabPage(pfe_load);
			}
			//else // DEBUG ->
			//	CreateTabPage(@"C:\Users\User\Documents\Neverwinter Nights 2\override\2da\spells.2da");
		}
		#endregion cTor


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
				int error = Marshal.GetLastWin32Error();
				MessageBox.Show(String.Format("An error occurred: {0}", error));
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
				//logfile.Log("MESSAGE RECEIVED");

				// extract the file-string from COPYDATASTRUCT
				var copyData = (Crap.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Crap.COPYDATASTRUCT));
				int dataType = (int)copyData.dwData;
				if (dataType == Crap.CopyDataStructType)
				{
					pfe_load = Marshal.PtrToStringAnsi(copyData.lpData);
					if (!String.IsNullOrEmpty(pfe_load)
						&& File.Exists(pfe_load))
					{
						CreateTabPage(pfe_load);
					}
				}
//				else
//					MessageBox.Show(String.Format("Unrecognized data type: {0}.", dataType),
//									"Yata",
//									MessageBoxButtons.OK,
//									MessageBoxIcon.Error);
			}
			else
				base.WndProc(ref m);
		}
		#endregion Receive Message (pfe)


		#region Methods (static)
		static FontStyle getStyleAccented(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			if (!ff.IsStyleAvailable(style = FontStyle.Underline))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (ff.IsStyleAvailable(styleTest))
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}

		static FontStyle getStyleStandard(FontFamily ff)
		{
			FontStyle style;
			if (!ff.IsStyleAvailable(style = FontStyle.Regular))
			if (!ff.IsStyleAvailable(style = FontStyle.Italic))
			if (!ff.IsStyleAvailable(style = FontStyle.Bold))
			foreach (FontStyle styleTest in Enum.GetValues(typeof(FontStyle))) // determine first available style of Family ->
			{
				if (ff.IsStyleAvailable(styleTest))
				{
					style = styleTest;
					break;
				}
			}
			return style;
		}
		#endregion Methods (static)


		internal void ShowColorPanel(bool vis = true)
		{
			if (vis) panel_ColorFill.BringToFront();
			else     panel_ColorFill.SendToBack();
		}

		void dropdownopening(object sender, EventArgs e)
		{
			if (Table != null && Table._editor.Visible)
			{
				Table._editor.Visible = false;
				Table.Refresh();
			}
		}


		#region File menu
		/// <summary>
		/// Handles opening the FileMenu, FolderPresets item and its sub-items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void file_dropdownopening(object sender, EventArgs e)
		{
			it_Save    .Enabled = (Table != null && !Table.Readonly);
			it_Reload  .Enabled =
			it_SaveAs  .Enabled =
			it_Close   .Enabled =
			it_CloseAll.Enabled = (Table != null);

			if (Table != null && Table._editor.Visible)
			{
				Table._editor.Visible = false;
				Table.Refresh();
			}

			_preset = String.Empty;
			it_OpenFolder.DropDownItems.Clear();
			foreach (var dir in Settings._dirpreset)
			{
				if (Directory.Exists(dir))
				{
					var preset = it_OpenFolder.DropDownItems.Add(dir);
					preset.Click += fileclick_OpenFolder;
				}
			}
			it_OpenFolder.Visible = (it_OpenFolder.DropDownItems.Count != 0);
		}

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
						CreateTabPage(pfe, read);
				}
			}
		}

		void fileclick_OpenFolder(object sender, EventArgs e)
		{
			var it = sender as ToolStripMenuItem;
			_preset = it.Text;

			fileclick_Open(null, EventArgs.Empty);
		}

		/// <summary>
		/// Creates a tab-page and instantiates a table-grid for it.
		/// @note The filename w/out extension must not be blank since
		/// YataGrid.Init() is going to use blank as a fallthrough while
		/// determining the grid's 'InfoType' to call GropeLabels().
		/// </summary>
		/// <param name="pfe">path_file_extension</param>
		/// <param name="read">readonly (default false)</param>
		void CreateTabPage(string pfe, bool read = false)
		{
			if (File.Exists(pfe) && !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(pfe)))
			{
				ShowColorPanel();
				Refresh();	// NOTE: If a table is already loaded the color-panel doesn't show
							// but a refresh turns the client area gray at least instead of glitchy.

				var table = new YataGrid(this, pfe, read);

				int result = table.Load2da();
				if (result != YataGrid.LOADRESULT_FALSE)
				{
					Table = table; // NOTE: Is done also in tab_SelectedIndexChanged()

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
						WindowState = FormWindowState.Normal;

					TopMost = true;
					TopMost = false;

					DrawingControl.ResumeDrawing(Table);
				}

				tab_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}


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

				ShowColorPanel(false);

				it_MenuPaths.Visible = (Table.Info != YataGrid.InfoType.INFO_NONE);

				it_freeze1.Checked = (Table.FrozenCount == YataGrid.FreezeFirst);
				it_freeze2.Checked = (Table.FrozenCount == YataGrid.FreezeSecond);

				btn_PropertyPanel.Visible = true;
			}
			else
			{
				ShowColorPanel();

				it_MenuPaths.Visible = false;

				it_freeze1.Checked =
				it_freeze2.Checked = false;

				btn_PropertyPanel.Visible = false;
			}

			SetTitlebarText();
		}

		/// <summary>
		/// Sets the width of the tabs on the TabControl.
		/// </summary>
		void SetTabSize()
		{
			if (Tabs != null && Tabs.TabCount != 0)
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
				Tabs.Refresh(); // prevent text-drawing glitches ...
			}
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
		}

		void fileclick_CloseTab(object sender, EventArgs e)
		{
			if (Table != null
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				Tabs.TabPages.Remove(Tabs.SelectedTab);

				if (Tabs.TabCount == 0)
					Table = null;

				SetTabSize();
				SetTitlebarText();
			}
		}

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
										"warning",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Warning,
										MessageBoxDefaultButton.Button2) == DialogResult.Yes;
			}

			if (close)
			{
				Table = null;

				for (int tab = Tabs.TabCount - 1; tab != -1; --tab)
					Tabs.TabPages.Remove(Tabs.TabPages[tab]);

				SetTitlebarText();
			}
		}

		void fileclick_Quit(object sender, EventArgs e)
		{
			Close(); // let yata_Closing() handle it ...
		}

		void yata_Closing(object sender, CancelEventArgs e)
		{
			if (Tabs.TabPages.Count == 1)
			{
				e.Cancel = Table.Changed
						&& MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
										   "warning",
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
											   "warning",
											   MessageBoxButtons.YesNo,
											   MessageBoxIcon.Warning,
											   MessageBoxDefaultButton.Button2) == DialogResult.No;
				}
			}
		}

		/// <summary>
		/// Returns a list of currently loaded tables that have been modified.
		/// </summary>
		/// <param name="excludecurrent">true to exclude the current Table</param>
		/// <returns></returns>
		List<string> GetChangedTables(bool excludecurrent = false)
		{
			var tables = new List<string>();

			foreach (TabPage page in Tabs.TabPages)
			{
				var table = page.Tag as YataGrid;
				if (table.Changed && (!excludecurrent || table != Table))
				{
					tables.Add(Path.GetFileNameWithoutExtension(table.Fullpath).ToUpperInvariant());
				}
			}
			return tables;
		}


		void fileclick_Reload(object sender, EventArgs e)
		{
			if (Table != null && File.Exists(Table.Fullpath)
				&& (!Table.Changed
					|| MessageBox.Show("Data has changed." + Environment.NewLine + "Okay to exit ...",
									   "warning",
									   MessageBoxButtons.YesNo,
									   MessageBoxIcon.Warning,
									   MessageBoxDefaultButton.Button2) == DialogResult.Yes))
			{
				ShowColorPanel();
				DrawingControl.SuspendDrawing(Table);

				int result = Table.Load2da();
				if (result != YataGrid.LOADRESULT_FALSE)
				{
					it_freeze1.Checked =
					it_freeze2.Checked = false;

					Table.Init(result == YataGrid.LOADRESULT_CHANGED, true);

					if (Table._propanel != null)
					{
						Table.Controls.Remove(Table._propanel);
						Table._propanel = null;
					}
				}
				else
				{
					Table.Changed = false; // bypass the close-tab warning.
					fileclick_CloseTab(sender, e);
				}

				if (Table != null)
				{
					ShowColorPanel(false);
					DrawingControl.ResumeDrawing(Table);
				}
			}
			// TODO: Show an error if file no longer exists.
		}


		/// <summary>
		/// Sets the titlebar text to something reasonable.
		/// </summary>
		void SetTitlebarText()
		{
			string text = "Yata";

			if (Tabs.SelectedIndex != -1)
			{
				var table = Tabs.SelectedTab.Tag as YataGrid;
				if (table != null)
				{
					string pfe = table.Fullpath;
					text += " - " + Path.GetFileName(pfe);

					string path = Path.GetDirectoryName(pfe);
					if (!String.IsNullOrEmpty(path))
					{
						text += " - " + path;
					}
				}
			}
			Text = text;
		}


		/// <summary>
		/// Caches a fullpath when doing SaveAs.
		/// So that the Table's new path-variables don't get assigned unless the
		/// save is successful - ie. verify several conditions first.
		/// </summary>
		string _pfeT = String.Empty;

		/// <summary>
		/// A flag that prevents a Readonly warning/error from showing twice.
		/// </summary>
		bool _warned;

		void fileclick_Save(object sender, EventArgs e)
		{
			if (Table != null)
			{
				bool force;
				if (String.IsNullOrEmpty(_pfeT))
				{
					_pfeT = Table.Fullpath;
					force = false;
				}
				else
					force = (_pfeT == Table.Fullpath);

				if (!String.IsNullOrEmpty(_pfeT))
				{
					_warned = false;

					if (!Table.Readonly || (force && SaveWarning("The 2da-file is opened as readonly.") == DialogResult.Yes))
					{
						if ((Table._sortcol == 0 && Table._sortdir == YataGrid.SORT_ASC)
							|| SaveWarning("The 2da is not sorted by ascending ID.") == DialogResult.Yes)
						{
							if (CheckRowOrder()
								|| SaveWarning("Faulty row IDs are detected.") == DialogResult.Yes)
							{
								Table.Fullpath = _pfeT;

								Tabs.TabPages[Tabs.SelectedIndex].Text = Path.GetFileNameWithoutExtension(Table.Fullpath);
								SetTitlebarText();

								Table.Readonly = false;

								int rows = Table.RowCount;
								if (rows != 0)
								{
									Table.Changed = false;
									foreach (var row in Table.Rows)
									{
										for (int c = 0; c != Table.ColCount; ++c)
											row.cells[c].loadchanged = false;
									}
									Table.Refresh();

									using (var sw = new StreamWriter(Table.Fullpath))
									{
										sw.WriteLine("2DA V2.0");
										sw.WriteLine("");

										string line = String.Empty;
										foreach (string field in Table.Fields)
										{
											line += " " + field;
										}
										sw.WriteLine(line);


										string val;

										int cols = Table.ColCount;

										for (int r = 0; r != rows; ++r)
										{
											line = String.Empty;

											for (int c = 0; c != cols; ++c)
											{
												if (c != 0)
													line += " ";

												if (!String.IsNullOrEmpty(val = Table[r,c].text))
													line += val;
												else
													line += Constants.Stars;
											}

											sw.WriteLine(line);
										}
									}
								}
							}
						}
					}
					else if (!_warned)
						ReadonlyError();
				}
			}
		}

		void fileclick_SaveAs(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
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
						_pfeT = String.Empty;
					}
				}
			}
		}

		DialogResult SaveWarning(string info)
		{
			_warned = true;
			return MessageBox.Show(info
								   + Environment.NewLine + Environment.NewLine
								   + "Save anyway ...",
								   "burp",
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
		#endregion File menu


		internal void ReadonlyError()
		{
			MessageBox.Show("The 2da-file is opened as readonly.",
							"burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Hand,
							MessageBoxDefaultButton.Button1);
		}


		#region Context menu
		int _r;

		internal void context_(int r)
		{
			_r = r;

			Table._editor.Visible = false;

			Table.ClearSelects();

			Row row = Table.Rows[_r];
			row.selected = true;
			for (int c = 0; c != Table.ColCount; ++c)
				row.cells[c].selected = true;

			Table.EnsureDisplayedRow(_r);
			Table.Refresh();

			context_it_Header.Text = "_row @ id " + _r;

			context_it_PasteAbove.Enabled =
			context_it_Paste     .Enabled =
			context_it_PasteBelow.Enabled = (_copy.Count != 0);

			Point loc;
			if (Settings._context)							// static location
			{
				loc = new Point(YataGrid.WidthRowhead,
								YataGrid.HeightColhead);
			}
			else											// vanilla location
				loc = Table.PointToClient(Cursor.Position);

			contextEditor.Show(Table, loc);
		}

		void contextclick_Header(object sender, EventArgs e)
		{
			contextEditor.Hide();
		}

		void contextclick_EditCopy(object sender, EventArgs e)
		{
			_copy.Clear();

			var fields = new string[Table.ColCount];
			for (int c = 0; c != Table.ColCount; ++c)
				fields[c] = Table[_r,c].text;

			_copy.Add(fields);
		}

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

		void contextclick_EditPasteAbove(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				Table.Insert(_r, _copy[0]);

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditPaste(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				Row row = Table.Rows[_r];
				string field;
				for (int c = 0; c != Table.ColCount; ++c)
				{
					if (c < _copy[0].Length)
						field = _copy[0][c];
					else
						field = Constants.Stars;

					row.cells[c].text = field;
				}
				row._brush = Brushes.Created;
	
				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditPasteBelow(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				Table.Insert(_r + 1, _copy[0]);

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditCreateAbove(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				var fields = new string[Table.ColCount];
				fields[0] = _r.ToString();
				for (int c = 1; c != Table.ColCount; ++c)
				{
					fields[c] = Constants.Stars;
				}
				Table.Insert(_r, fields);

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditClear(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				for (int c = 1; c != Table.ColCount; ++c)
				{
					Table[_r,c].text = Constants.Stars;
				}
				Table.Rows[_r]._brush = Brushes.Created;

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditCreateBelow(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				var fields = new string[Table.ColCount];
				fields[0] = (_r + 1).ToString();
				for (int c = 1; c != Table.ColCount; ++c)
				{
					fields[c] = Constants.Stars;
				}
				Table.Insert(_r + 1, fields);

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}

		void contextclick_EditDelete(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Table.SetProHori();

				Table.Changed = true;
				Table.Insert(_r, null);

				Table.Refresh();
				Table._proHori = 0;
			}
			else
				ReadonlyError();
		}
		#endregion Context menu


		#region Edit menu
		/// <summary>
		/// Handles opening the EditMenu, determines if various items ought be
		/// enabled.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void edit_dropdownopening(object sender, EventArgs e)
		{
			it_Findnext.Enabled = (Table != null && !String.IsNullOrEmpty(tb_Search.Text));

			it_GotoLoadchanged.Enabled = false;

			if (Table != null && Table.RowCount != 0)
			{
				if (Table._editor.Visible)
				{
					Table._editor.Visible = false;
					Table.Refresh();
				}

				foreach (var row in Table.Rows)
				{
					for (int c = 0; c != Table.ColCount; ++c)
					{
						if (row.cells[c].loadchanged)
						{
							it_GotoLoadchanged.Enabled = true;
							break;
						}
					}
				}

				it_CopyRange .Enabled = (Table.getSelectedRow() != -1);
				it_PasteRange.Enabled = (_copy.Count != 0);

//				it_CopyCell .Enabled = (Table.GetSelectedCell() != null);
//				it_PasteCell.Enabled = !String.IsNullOrEmpty(_copycell);
			}
			else
			{
				it_CopyRange .Enabled =
				it_PasteRange.Enabled = false;
			}

			it_CopyToClipboard  .Enabled = (_copy.Count != 0);
			it_CopyFromClipboard.Enabled = Clipboard.ContainsText(TextDataFormat.Text);
		}

		void textchanged_Search(object sender, EventArgs e)
		{
			it_Findnext.Enabled = !String.IsNullOrEmpty(tb_Search.Text);
		}

		void editclick_Search(object sender, EventArgs e)
		{
			tb_Search.Focus();
			tb_Search.SelectAll();
		}

		void editclick_SearchNext(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
				Table.Select(); // F3 shall focus the table, Enter shall keep focus on the tb/cbx.
				Search();
			}
		}

		/// <summary>
		/// Performs a search when the Enter-key is pressed and focus is on
		/// either the search-box or the search-option dropdown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SearchKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter
				&& Table != null && Table.RowCount != 0)
			{
				_search = true; // Enter shall keep focus on the tb/cbx, F3 shall focus the table.
				Search();
				_search = false;
			}
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
				Table.Refresh();
			}

			// TODO: Allow frozen col(s) to be searched through also.
			// TODO: option to invert the search direction (or at least back to
			//       previous find)

			string search = tb_Search.Text;
			if (!String.IsNullOrEmpty(search))
			{
				search = search.ToLower();

				int row, r,c;

				Cell sel = Table.GetSelectedCell();
				if (sel != null)
				{
					c   = sel.x;
					row = sel.y;
				}
				else
				{
					c   = -1;
					row =  0;
				}
				Table.ClearSelects();


				string val;

				bool start = true;
				bool substring = (cb_SearchOption.SelectedIndex == 0); // else is wholestring search.

				string field;

				for (r = row; r != Table.RowCount; ++r)
				{
					if (start)
					{
						start = false;
						if (c == -1)
							c = 0;
						else
							++c;

						if (c == Table.ColCount)		// if starting on the last cell of a row
						{
							c = 0;

							if (r < Table.RowCount - 1)	// jump to the first cell of the next row
							{
								++r;
							}
							else						// or to the top of the table if on the last row(s)
								r = 0;
						}
					}
					else
						c = 0;

					for (; c != Table.ColCount; ++c)
					{
						if (c >= Table.FrozenCount && !String.IsNullOrEmpty(val = Table[r,c].text))
						{
							field = val.ToLower();
							if (field == search
								|| (substring && field.Contains(search)))
							{
//								if (sel != null)
//									sel.selected = false;

								Table[r,c].selected = true;
								Table.EnsureDisplayed(Table[r,c]);
								Table.Refresh();

								return;
							}
						}
					}
				}

				// TODO: tighten exact start/end-cells
				for (r = 0; r != row + 1; ++r) // quick and dirty wrap ->
				{
					for (c = 0; c != Table.ColCount; ++c)
					{
						if (c >= Table.FrozenCount && !String.IsNullOrEmpty(val = Table[r,c].text))
						{
							field = val.ToLower();
							if (field == search
								|| (substring && field.Contains(search)))
							{
//								if (sel != null)
//									sel.selected = false;

								Table[r,c].selected = true;
								Table.EnsureDisplayed(Table[r,c]);
								Table.Refresh();

								return;
							}
						}
					}
				}
			}
		}


		void editclick_Goto(object sender, EventArgs e)
		{
			tb_Goto.Focus();
		}

		/// <summary>
		/// Performs a goto when the Enter-key is pressed and focus is on the
		/// goto-box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void GotoKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter
				&& Table != null && Table.RowCount != 0)
			{
				int r;
				if (Int32.TryParse(tb_Goto.Text, out r)
					&& r > -1 && r < Table.RowCount)
				{
					Table._editor.Visible = false;
					Table.ClearSelects();

					Table.Select();

					Row row = Table.Rows[r];
					row.selected = true;
					for (int c = 0; c != Table.ColCount; ++c)
						row.cells[c].selected = true;

					Table.EnsureDisplayedRow(r);
					Table.Refresh();
				}
			}
		}

		/// <summary>
		/// Selects the next LoadChanged cell.
		/// @note This is fired only from the EditMenu and its item is enabled
		/// only if there actually IS a load-changed cell available. TODO: If
		/// this is given a hotkey then the item would have to be enabled when a
		/// load-changed flag(s) is set and disabled/validity-checks need to
		/// happen here or so.
		/// @note Assumes Table is valid and that there are loadchanged cells.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_GotoLoadchanged(object sender, EventArgs e)
		{
			if (Table._editor.Visible)
			{
				Table._editor.Visible = false;
				Table.Refresh();
			}

			Table.Select();

			Cell sel = Table.GetSelectedCell();
			Table.ClearSelects();

			int row, r,c;
			if (sel != null)
			{
				c   = sel.x;
				row = sel.y;
			}
			else
			{
				c   = -1;
				row =  0;
			}

			bool start = true;

			for (r = row; r != Table.RowCount; ++r)
			{
				if (start)
				{
					start = false;
					if (c == -1)
						c = 0;
					else
						++c;

					if (c == Table.ColCount)		// if starting on the last cell of a row
					{
						c = 0;

						if (r < Table.RowCount - 1)	// jump to the first cell of the next row
						{
							++r;
						}
						else						// or to the top of the table if on the last row(s)
							r = 0;
					}
				}
				else
					c = 0;

				for (; c != Table.ColCount; ++c)
				{
					if (c >= Table.FrozenCount && (sel = Table[r,c]).loadchanged)
					{
						sel.selected = true;
						Table.EnsureDisplayed(sel);
						Table.Refresh();

						return;
					}
				}
			}

			// TODO: tighten exact start/end-cells
			for (r = 0; r != row + 1; ++r) // quick and dirty wrap ->
			{
				for (c = 0; c != Table.ColCount; ++c)
				{
					if (c >= Table.FrozenCount && (sel = Table[r,c]).loadchanged)
					{
						sel.selected = true;
						Table.EnsureDisplayed(sel);
						Table.Refresh();

						return;
					}
				}
			}
		}


		void editclick_CopyCell(object sender, EventArgs e)
		{
			if (Table != null)
			{
				Cell cell = Table.GetSelectedCell();
				if (cell != null)
				{
					_copycell = cell.text;
				}
				else
					CopyPasteCellError();
			}
		}

		void editclick_PasteCell(object sender, EventArgs e)
		{
			if (Table != null)
			{
				if (!Table.Readonly)
				{
					Cell cell = Table.GetSelectedCell();
					if (cell != null)
					{
						if (cell.text != _copycell)
						{
							cell.text = _copycell;

							Table.Changed = true;
							cell.loadchanged = false;

							Table.colRewidth(cell.x, cell.y);
							Table.UpdateFrozenControls(cell.x);
						}
					}
					else
						CopyPasteCellError();
				}
				else
					ReadonlyError();
			}
		}

		void CopyPasteCellError()
		{
			MessageBox.Show("Select one (1) cell.",
							"burp",
							MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation,
							MessageBoxDefaultButton.Button1);
		}


		void editclick_CopyRange(object sender, EventArgs e)
		{
			_copy.Clear();

			int selr = Table.getSelectedRow();

			int top,bot;
			if (Table.RangeSelect > 0)
			{
				top = selr;
				bot = selr + Table.RangeSelect;
			}
			else
			{
				top = selr + Table.RangeSelect;
				bot = selr;
			}

			string[] fields;
			while (top <= bot)
			{
				fields = new string[Table.ColCount];
				for (int c = 0; c != Table.ColCount; ++c)
					fields[c] = Table[top,c].text;

				_copy.Add(fields);
				++top;
			}
			it_PasteRange.Enabled = true;
		}

		internal void EnableCopyRange(bool enabled)
		{
			it_CopyRange.Enabled = enabled;
		}

		void editclick_PasteRange(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				ShowColorPanel();
				DrawingControl.SuspendDrawing(Table);
	
				Table.Changed = true;
	
				int selr = Table.getSelectedRow();
				if (selr == -1)
					selr = Table.RowCount;
	
				int r = selr;
				for (int i = 0; i != _copy.Count; ++i)
					Table.Insert(r++, _copy[i], false);
	
				Table.Calibrate(selr, _copy.Count - 1); // paste range
				Table.EnsureDisplayedRow(selr);
	
				ShowColorPanel(false);
				DrawingControl.ResumeDrawing(Table);
			}
			else
				ReadonlyError();
		}

		/// <summary>
		/// Outputs the current contents of '_copy' to the Windows clipboard.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_ExportCopy(object sender, EventArgs e)
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
			ClipboardHelper.SetText(clip);
		}

		/// <summary>
		/// Imports the current contents of the Windows clipboard to '_copy'.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void editclick_ImportCopy(object sender, EventArgs e)
		{
			_copy.Clear();

			string clip = Clipboard.GetText(TextDataFormat.Text);
			if (!String.IsNullOrEmpty(clip))
			{
				string[] lines = clip.Split(new[]{ Environment.NewLine }, StringSplitOptions.None);
				for (int i = 0; i != lines.Length; ++i)
				{
					string[] fields = YataGrid.Parse2daRow(lines[i]);
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
		void editclick_ViewClipboard(object sender, EventArgs e)
		{
			var f = Application.OpenForms["ClipboardEditor"];
			if (f == null)
			{
				f = new ClipboardEditor(this);
				f.Show();
			}
			else
				f.BringToFront();
		}
		#endregion Edit menu


		#region 2da Ops menu
		void ops_dropdownopening(object sender, EventArgs e)
		{
			it_RenumberRows.Enabled =
			it_CheckRows   .Enabled =
			it_RecolorRows .Enabled =
			it_AutoCols    .Enabled = (Table != null);
			it_freeze1     .Enabled = (Table != null && Table.Cols.Count > 1);
			it_freeze2     .Enabled = (Table != null && Table.Cols.Count > 2);

			if (Table != null)
			{
				Table._editor.Visible = false;
				Table.Refresh();
			}
		}

		void opsclick_Reorder(object sender, EventArgs e)
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

					Table.Changed |= changed;

					if (changed)
					{
						Table.colRewidth(0, 0, Table.RowCount - 1);
						Table.UpdateFrozenControls(0);

						Table.InitScrollers();
						Table.Refresh();
					}
				}
				else
					ReadonlyError();
			}
		}

		void opsclick_CheckRowOrder(object sender, EventArgs e)
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
											"burp",
											MessageBoxButtons.YesNo,
											MessageBoxIcon.Exclamation,
											MessageBoxDefaultButton.Button1) == DialogResult.Yes)
						{
							opsclick_Reorder(null, EventArgs.Empty);
						}
					}
					else
						MessageBox.Show(info,
										"burp",
										MessageBoxButtons.OK,
										MessageBoxIcon.Exclamation,
										MessageBoxDefaultButton.Button1);
				}
				else
					MessageBox.Show("Row order is Okay.",
									"burp",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
			}
		}

		void opsclick_Recolor(object sender, EventArgs e)
		{
			if (Table != null && Table.RowCount != 0)
			{
				Row row;
				Brush brush;

				for (int id = 0; id != Table.RowCount; ++id)
				{
					brush = (id % 2 == 0) ? Brushes.Alice
										  : Brushes.Blanche;

					(row = Table.Rows[id])._brush = brush;

					for (int c = 0; c != Table.ColCount; ++c)
						row.cells[c].loadchanged = false;
				}
				Table.Refresh();
			}
		}

		internal void opsclick_AutosizeCols(object sender, EventArgs e)
		{
			if (Table != null)
			{
				ShowColorPanel();
				DrawingControl.SuspendDrawing(Table);

				foreach (var c in Table.Cols)
					c.UserSized = false;

				Table.Calibrate(0, Table.RowCount - 1); // autosize

				ShowColorPanel(false);
				DrawingControl.ResumeDrawing(Table);
			}
		}


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
						Table.colRewidth(1);
					}
					Table.FrozenCount = YataGrid.FreezeFirst;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}

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
						Table.colRewidth(1);
					}

					col = Table.Cols[2];
					if (col.UserSized)
					{
						col.UserSized = false;
						Table.colRewidth(2);
					}

					Table.FrozenCount = YataGrid.FreezeSecond;
				}
				else
					Table.FrozenCount = YataGrid.FreezeId;
			}
		}
		#endregion 2da Ops menu


		#region Font menu
		/// <summary>
		/// Opens the FontPicker form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Font(object sender, EventArgs e)
		{
			var f = Application.OpenForms["FontPickerForm"];
			if (f == null)
			{
				f = new FontPickerForm(this);
				f.Show();
			}
			else
				f.BringToFront();
		}

		/// <summary>
		/// Opens an output-box with the current table-font as a string for
		/// copying to Settings.Cfg if desired.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_CurrentFont(object sender, EventArgs e)
		{
			var f = new FontCopyForm(this);

			TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
			f.SetText("font=" + tc.ConvertToString(Font));

			f.ShowDialog();
		}

		/// <summary>
		/// Sets the form's font to the default Font.
		/// See also: FontPickerForm.doFont()
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void fontclick_Default(object sender, EventArgs e)
		{
			if (!Font.Equals(FontDefault))
				doFont(FontDefault);
		}

		internal void ToggleFontDefaultEnabled()
		{
			it_FontDefault.Enabled = !it_FontDefault.Enabled;
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

			Font = font; // rely on GC here
			FontAccent = new Font(Font, getStyleAccented(Font.FontFamily));

			YataGrid.SetStaticMetrics(this);

			if (Table != null)
			{
				ShowColorPanel();
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

					if (table.EnsureDisplayedCellOrRow())
						table.Refresh(); // for big tables ...
				}

				ShowColorPanel(false);
				DrawingControl.ResumeDrawing(Table);
			}
		}
		#endregion Font menu


		#region Help menu
		void helpclick_Help(object sender, EventArgs e)
		{
			string path = Path.Combine(Application.StartupPath, "ReadMe.txt");
			if (File.Exists(path))
				Process.Start(path);
			else
				MessageBox.Show("ReadMe.txt was not found in the application directory.",
								"burp",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
		}

		void helpclick_About(object sender, EventArgs e)
		{
			var an = Assembly.GetExecutingAssembly().GetName();
			string ver = "Ver "
					   + an.Version.Major + "."
					   + an.Version.Minor + "."
					   + an.Version.Build + "."
					   + an.Version.Revision;
#if DEBUG
			ver += " - debug";
#else
			ver += " - release";
#endif
			MessageBox.Show(ver,
							"Version info",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information,
							MessageBoxDefaultButton.Button1);
		}
		#endregion Help menu


		#region Tabmenu
		/// <summary>
		/// Sets the selected tab when a right-click on a tab is about to open
		/// a context.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabMenu_Opening(object sender, CancelEventArgs e)
		{
			Table._editor.Visible = false;
			Table.Refresh();

			var pt = Tabs.PointToClient(Cursor.Position);
			for (int tab = 0; tab != Tabs.TabCount; ++tab)
			{
				if (Tabs.GetTabRect(tab).Contains(pt))
				{
					Tabs.SelectedIndex = tab;
					return;
				}
			}
			e.Cancel = true;
		}

		/// <summary>
		/// Closes a table when a tab's context-close item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tabclick_Close(object sender, EventArgs e)
		{
			fileclick_CloseTab(null, EventArgs.Empty);
		}

		void tabclick_CloseAll(object sender, EventArgs e)
		{
			fileclick_CloseAllTabs(null, EventArgs.Empty);
		}

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
										"warning",
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
						Tabs.TabPages.Remove(Tabs.TabPages[tab]);
				}

				SetTabSize();
				SetTitlebarText();

				DrawingControl.ResumeDrawing(this);
			}
		}

		void tabclick_Reload(object sender, EventArgs e)
		{
			fileclick_Reload(null, EventArgs.Empty);
		}

		// TODO: FreezeFirst/Second
		#endregion Tabmenu


		#region Statusbar
		/// <summary>
		/// Mouseover datacells prints table-cords plus info to the statusbar if
		/// a relevant 2da (eg. Crafting, Spells) is loaded.
		/// </summary>
		/// <param name="cords">null to clear statusbar-cords and -info</param>
		internal void PrintInfo(Point? cords = null)
		{
			string st = String.Empty;

			if (cords != null && Table != null) // else CloseAll can throw on invalid object.
			{
				var pt = (Point)cords;
				int id  = pt.Y;
				int col = pt.X;

				if (id < Table.RowCount && col < Table.ColCount) // NOTE: mouseover pos can register in the scrollbars
				{
					statusbar_label_Cords.Text = "id= " + id + " col= " + col;

					switch (Table.Info)
					{
						case YataGrid.InfoType.INFO_CRAFT:
							statusbar_label_Info.Text = getCraftInfo(id, col);
							break;
						case YataGrid.InfoType.INFO_SPELL:
							statusbar_label_Info.Text = getSpellInfo(id, col);
							break;

						default:
							statusbar_label_Info.Text = st;
							break;
					}
				}
				else
				{
					statusbar_label_Cords.Text =
					statusbar_label_Info .Text = st;
				}
			}
			else
			{
				statusbar_label_Cords.Text =
				statusbar_label_Info .Text = st;
			}
		}
		#endregion Statusbar


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
		#endregion Events (override)


		#region DragDrop file(s)
		internal void yata_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
		}

		internal void yata_DragDrop(object sender, DragEventArgs e)
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string file in files)
				CreateTabPage(file);
		}
		#endregion DragDrop file(s)


		internal void TableChanged(bool changed)
		{
			DrawingControl.SuspendDrawing(this); // stops tab-flickering on Sort

			string asterisk = changed ? " *"
									  : "";
			Tabs.TabPages[Tabs.SelectedIndex].Text = Path.GetFileNameWithoutExtension(Table.Fullpath) + asterisk;
			SetTabSize();

			DrawingControl.ResumeDrawing(this);
		}


		#region PropertyPanel
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
						&& Table._propanel != null && Table._propanel.Visible))
				{
					btn_PropertyPanel.Depressed = true;
					btn_PropertyPanel.Refresh();
				}
			}
		}

		/// <summary>
		/// Handler for MouseUp on the PropertyPanel button.
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
					btn_PropertyPanel.Depressed = false;
					btn_PropertyPanel.Refresh();

					if (Table._propanel == null
						|| (Table._propanel.Visible = !Table._propanel.Visible))
					{
						if (Table._propanel == null)
							Table._propanel = new PropertyPanel(Table);
						else
						{
							Table._propanel.calcValueWidth();
							Table._propanel.setLeftHeight();
							Table._propanel.InitScroll();
						}

						Table._propanel.Show();
						Table._propanel.BringToFront();
					}
					else
						Table._propanel.Hide();
				}
				else if (e.Button == MouseButtons.Right
					&& Table._propanel != null && Table._propanel.Visible)
				{
					btn_PropertyPanel.Depressed = false;
					btn_PropertyPanel.Refresh();

					Table._propanel.DockBot = !Table._propanel.DockBot;
				}
			}
		}
		#endregion PropertyPanel


		#region Cell menu
		internal void ShowCellMenu()
		{
			Point loc = Table.PointToClient(Cursor.Position);
			cellMenu.Show(Table, loc);
		}

		void cellclick_Stars(object sender, EventArgs e)
		{
			if (!Table.Readonly)
			{
				Cell cell = Table.GetSelectedCell();
				if (cell != null)
				{
					if (cell.text != Constants.Stars)
					{
						cell.text = Constants.Stars;

						Table.Changed = true;
						cell.loadchanged = false;

						Table.colRewidth(cell.x, cell.y);
						Table.UpdateFrozenControls(cell.x);
					}
				}
				else
					CopyPasteCellError();
			}
			else
				ReadonlyError();
		}
		#endregion Cell menu
	}


	static class Constants
	{
		internal const string Stars = "****";
	}
}
