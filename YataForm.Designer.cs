namespace yata
{
//	private System.Windows.Forms.TabControl tabControl;
//	private yata.DraggableTabControl tabControl;

	partial class YataForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private yata.YataTabs tabControl;
		private System.Windows.Forms.MenuStrip menubar;
		private System.Windows.Forms.ToolStripMenuItem it_MenuFile;
		private System.Windows.Forms.ToolStripMenuItem it_Open;
		private System.Windows.Forms.ToolStripMenuItem it_Save;
		private System.Windows.Forms.ToolStripMenuItem it_SaveAs;
		private System.Windows.Forms.ToolStripMenuItem it_Close;
		private System.Windows.Forms.ToolStripSeparator separator_2;
		private System.Windows.Forms.ToolStripMenuItem it_Quit;
		private System.Windows.Forms.ToolStripSeparator separator_1;
		private System.Windows.Forms.ContextMenuStrip contextEditor;
		private System.Windows.Forms.ToolStripMenuItem context_it_Cut;
		private System.Windows.Forms.ToolStripMenuItem context_it_Copy;
		private System.Windows.Forms.ToolStripMenuItem context_it_Paste;
		private System.Windows.Forms.ToolStripMenuItem context_it_DeleteRow;
		private System.Windows.Forms.ToolStripMenuItem context_it_Header;
		private System.Windows.Forms.ToolStripMenuItem context_it_ClearRow;
		private System.Windows.Forms.ToolStripMenuItem context_it_CreateBelow;
		private System.Windows.Forms.ToolStripMenuItem context_it_PasteBelow;
		private System.Windows.Forms.ToolStripMenuItem it_MenuEdit;
		private System.Windows.Forms.ToolStripMenuItem it_RenumberRows;
		private System.Windows.Forms.ToolStripMenuItem context_it_CreateAbove;
		private System.Windows.Forms.ToolStripMenuItem context_it_PasteAbove;
		private System.Windows.Forms.ToolStripMenuItem it_RecolorRows;
		private System.Windows.Forms.ToolStripSeparator separator_7;
		private System.Windows.Forms.ToolStripSeparator separator_8;
		private System.Windows.Forms.ToolStripSeparator separator_9;
		private System.Windows.Forms.ToolStripSeparator separator_10;
		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.ToolStripStatusLabel statusbar_label_Coords;
		private System.Windows.Forms.ToolStripStatusLabel statusbar_label_Info;
		private System.Windows.Forms.ToolStripMenuItem it_MenuPaths;
		private System.Windows.Forms.ToolStripMenuItem it_PathSpells2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathFeat2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathItemPropDef2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathBaseItems2da;
		private System.Windows.Forms.ToolStripTextBox tb_Search;
		private System.Windows.Forms.ToolStripComboBox cb_SearchOption;
		private System.Windows.Forms.ToolStripMenuItem it_MenuOptions;
		private System.Windows.Forms.ToolStripMenuItem it_PathSkills2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathAll;
		private System.Windows.Forms.ToolStripMenuItem it_PathRaces2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathClasses2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpSpells2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathDisease2da;
		private System.Windows.Forms.ToolStripMenuItem it_freeze1;
		private System.Windows.Forms.ToolStripMenuItem it_freeze2;
		private System.Windows.Forms.ToolStripSeparator separator_5;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpOnHitSpell2da;
		private System.Windows.Forms.ToolStripMenuItem it_CurrentFont;
		private System.Windows.Forms.ToolStripMenuItem it_CheckRows;
		private System.Windows.Forms.ToolStripMenuItem it_Reload;
		private System.Windows.Forms.ToolStripMenuItem it_Folders;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpFeats2da;
		private System.Windows.Forms.ToolStripSeparator separator_6;
		private System.Windows.Forms.Panel panel_ColorFill;
		private System.Windows.Forms.ContextMenuStrip tabMenu;
		private System.Windows.Forms.ToolStripMenuItem it_tabClose;
		private System.Windows.Forms.ToolStripMenuItem it_Search;
		private System.Windows.Forms.ToolStripMenuItem it_Findnext;
		private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem it_Font;
		private System.Windows.Forms.ToolStripMenuItem it_FontDefault;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpAmmoCost2da;
		private System.Windows.Forms.ToolStripMenuItem it_tabCloseAll;
		private System.Windows.Forms.ToolStripMenuItem it_CloseAll;
		private System.Windows.Forms.ToolStripTextBox tb_Goto;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem it_Goto;
		private System.Windows.Forms.ToolStripMenuItem it_GotoLoadchanged;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem it_CopyRange;
		private System.Windows.Forms.ToolStripMenuItem it_PasteRange;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem it_CopyToClipboard;
		private System.Windows.Forms.ToolStripMenuItem it_CopyFromClipboard;
		private System.Windows.Forms.ToolStripMenuItem it_ViewClipboardContents;
		private System.Windows.Forms.ToolStripMenuItem it_tabCloseAllOthers;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

//			this.tabControl = new System.Windows.Forms.TabControl();
//			this.tabControl = new yata.DraggableTabControl();
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YataForm));
			this.tabControl = new yata.YataTabs();
			this.tabMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.it_tabClose = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menubar = new System.Windows.Forms.MenuStrip();
			this.it_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Reload = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Folders = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_1 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Save = new System.Windows.Forms.ToolStripMenuItem();
			this.it_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_2 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Quit = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Search = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Findnext = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Goto = new System.Windows.Forms.ToolStripMenuItem();
			this.it_GotoLoadchanged = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ViewClipboardContents = new System.Windows.Forms.ToolStripMenuItem();
			this.tb_Goto = new System.Windows.Forms.ToolStripTextBox();
			this.tb_Search = new System.Windows.Forms.ToolStripTextBox();
			this.cb_SearchOption = new System.Windows.Forms.ToolStripComboBox();
			this.it_MenuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.it_RenumberRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CheckRows = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.it_RecolorRows = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.it_freeze1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_freeze2 = new System.Windows.Forms.ToolStripMenuItem();
			this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Font = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CurrentFont = new System.Windows.Forms.ToolStripMenuItem();
			this.it_FontDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuPaths = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathAll = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_5 = new System.Windows.Forms.ToolStripSeparator();
			this.it_PathBaseItems2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathFeat2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathItemPropDef2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathSkills2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathSpells2da = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_6 = new System.Windows.Forms.ToolStripSeparator();
			this.it_PathClasses2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathDisease2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathIprpAmmoCost2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathIprpFeats2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathIprpOnHitSpell2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathIprpSpells2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathRaces2da = new System.Windows.Forms.ToolStripMenuItem();
			this.contextEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.context_it_Header = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_7 = new System.Windows.Forms.ToolStripSeparator();
			this.context_it_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.context_it_Cut = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_8 = new System.Windows.Forms.ToolStripSeparator();
			this.context_it_PasteAbove = new System.Windows.Forms.ToolStripMenuItem();
			this.context_it_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.context_it_PasteBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_9 = new System.Windows.Forms.ToolStripSeparator();
			this.context_it_CreateAbove = new System.Windows.Forms.ToolStripMenuItem();
			this.context_it_ClearRow = new System.Windows.Forms.ToolStripMenuItem();
			this.context_it_CreateBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_10 = new System.Windows.Forms.ToolStripSeparator();
			this.context_it_DeleteRow = new System.Windows.Forms.ToolStripMenuItem();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusbar_label_Coords = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusbar_label_Info = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel_ColorFill = new System.Windows.Forms.Panel();
			this.it_tabCloseAllOthers = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMenu.SuspendLayout();
			this.menubar.SuspendLayout();
			this.contextEditor.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.AllowDrop = true;
			this.tabControl.ContextMenuStrip = this.tabMenu;
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl.Location = new System.Drawing.Point(0, 24);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.Padding = new System.Drawing.Point(0, 0);
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(842, 408);
			this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl.TabIndex = 0;
			this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tab_DrawItem);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tab_SelectedIndexChanged);
			// 
			// tabMenu
			// 
			this.tabMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_tabClose,
			this.it_tabCloseAll,
			this.it_tabCloseAllOthers});
			this.tabMenu.Name = "tabMenu";
			this.tabMenu.Size = new System.Drawing.Size(153, 92);
			this.tabMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tabMenu_Opening);
			// 
			// it_tabClose
			// 
			this.it_tabClose.Name = "it_tabClose";
			this.it_tabClose.Size = new System.Drawing.Size(152, 22);
			this.it_tabClose.Text = "Close";
			this.it_tabClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabClose.Click += new System.EventHandler(this.tabclick_Close);
			// 
			// it_tabCloseAll
			// 
			this.it_tabCloseAll.Name = "it_tabCloseAll";
			this.it_tabCloseAll.Size = new System.Drawing.Size(152, 22);
			this.it_tabCloseAll.Text = "Close all";
			this.it_tabCloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAll.Click += new System.EventHandler(this.tabclick_CloseAll);
			// 
			// menubar
			// 
			this.menubar.Font = new System.Drawing.Font("Consolas", 6.5F);
			this.menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_MenuFile,
			this.it_MenuEdit,
			this.tb_Goto,
			this.tb_Search,
			this.cb_SearchOption,
			this.it_MenuOptions,
			this.fontToolStripMenuItem,
			this.it_MenuPaths});
			this.menubar.Location = new System.Drawing.Point(0, 0);
			this.menubar.Name = "menubar";
			this.menubar.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.menubar.Size = new System.Drawing.Size(842, 24);
			this.menubar.TabIndex = 0;
			this.menubar.Text = "menuStrip1";
			// 
			// it_MenuFile
			// 
			this.it_MenuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Open,
			this.it_Reload,
			this.it_Folders,
			this.separator_1,
			this.it_Save,
			this.it_SaveAs,
			this.it_Close,
			this.it_CloseAll,
			this.separator_2,
			this.it_Quit});
			this.it_MenuFile.Name = "it_MenuFile";
			this.it_MenuFile.Size = new System.Drawing.Size(37, 20);
			this.it_MenuFile.Text = "File";
			this.it_MenuFile.DropDownOpening += new System.EventHandler(this.file_dropdownopening_FolderPresets);
			// 
			// it_Open
			// 
			this.it_Open.Name = "it_Open";
			this.it_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.it_Open.Size = new System.Drawing.Size(145, 22);
			this.it_Open.Text = "Open ...";
			this.it_Open.Click += new System.EventHandler(this.fileclick_Open);
			// 
			// it_Reload
			// 
			this.it_Reload.Name = "it_Reload";
			this.it_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.it_Reload.Size = new System.Drawing.Size(145, 22);
			this.it_Reload.Text = "Reload";
			this.it_Reload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// it_Folders
			// 
			this.it_Folders.Name = "it_Folders";
			this.it_Folders.Size = new System.Drawing.Size(145, 22);
			this.it_Folders.Text = "Folders";
			this.it_Folders.Visible = false;
			// 
			// separator_1
			// 
			this.separator_1.Name = "separator_1";
			this.separator_1.Size = new System.Drawing.Size(142, 6);
			// 
			// it_Save
			// 
			this.it_Save.Name = "it_Save";
			this.it_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.it_Save.Size = new System.Drawing.Size(145, 22);
			this.it_Save.Text = "Save";
			this.it_Save.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// it_SaveAs
			// 
			this.it_SaveAs.Name = "it_SaveAs";
			this.it_SaveAs.Size = new System.Drawing.Size(145, 22);
			this.it_SaveAs.Text = "Save As ...";
			this.it_SaveAs.Click += new System.EventHandler(this.fileclick_SaveAs);
			// 
			// it_Close
			// 
			this.it_Close.Name = "it_Close";
			this.it_Close.Size = new System.Drawing.Size(145, 22);
			this.it_Close.Text = "Close";
			this.it_Close.Click += new System.EventHandler(this.fileclick_Close);
			// 
			// it_CloseAll
			// 
			this.it_CloseAll.Name = "it_CloseAll";
			this.it_CloseAll.Size = new System.Drawing.Size(145, 22);
			this.it_CloseAll.Text = "Close all";
			this.it_CloseAll.Click += new System.EventHandler(this.fileclick_CloseAll);
			// 
			// separator_2
			// 
			this.separator_2.Name = "separator_2";
			this.separator_2.Size = new System.Drawing.Size(142, 6);
			// 
			// it_Quit
			// 
			this.it_Quit.Name = "it_Quit";
			this.it_Quit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.it_Quit.Size = new System.Drawing.Size(145, 22);
			this.it_Quit.Text = "Quit";
			this.it_Quit.Click += new System.EventHandler(this.fileclick_Quit);
			// 
			// it_MenuEdit
			// 
			this.it_MenuEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Search,
			this.it_Findnext,
			this.toolStripSeparator3,
			this.it_Goto,
			this.it_GotoLoadchanged,
			this.toolStripSeparator4,
			this.it_CopyRange,
			this.it_PasteRange,
			this.toolStripSeparator5,
			this.it_CopyToClipboard,
			this.it_CopyFromClipboard,
			this.it_ViewClipboardContents});
			this.it_MenuEdit.Name = "it_MenuEdit";
			this.it_MenuEdit.Size = new System.Drawing.Size(37, 20);
			this.it_MenuEdit.Text = "Edit";
			this.it_MenuEdit.DropDownOpening += new System.EventHandler(this.edit_dropdownopening_EnableItems);
			// 
			// it_Search
			// 
			this.it_Search.Name = "it_Search";
			this.it_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.it_Search.Size = new System.Drawing.Size(190, 22);
			this.it_Search.Text = "Find";
			this.it_Search.Click += new System.EventHandler(this.editclick_Search);
			// 
			// it_Findnext
			// 
			this.it_Findnext.Name = "it_Findnext";
			this.it_Findnext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.it_Findnext.Size = new System.Drawing.Size(190, 22);
			this.it_Findnext.Text = "Find next";
			this.it_Findnext.Click += new System.EventHandler(this.editclick_SearchNext);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
			// 
			// it_Goto
			// 
			this.it_Goto.Name = "it_Goto";
			this.it_Goto.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.it_Goto.Size = new System.Drawing.Size(190, 22);
			this.it_Goto.Text = "Goto";
			this.it_Goto.Click += new System.EventHandler(this.editclick_Goto);
			// 
			// it_GotoLoadchanged
			// 
			this.it_GotoLoadchanged.Enabled = false;
			this.it_GotoLoadchanged.Name = "it_GotoLoadchanged";
			this.it_GotoLoadchanged.Size = new System.Drawing.Size(190, 22);
			this.it_GotoLoadchanged.Text = "Goto loadchanged";
			this.it_GotoLoadchanged.Click += new System.EventHandler(this.editclick_GotoLoadchanged);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
			// 
			// it_CopyRange
			// 
			this.it_CopyRange.Enabled = false;
			this.it_CopyRange.Name = "it_CopyRange";
			this.it_CopyRange.Size = new System.Drawing.Size(190, 22);
			this.it_CopyRange.Text = "copy range";
			this.it_CopyRange.Click += new System.EventHandler(this.editclick_CopyRange);
			// 
			// it_PasteRange
			// 
			this.it_PasteRange.Enabled = false;
			this.it_PasteRange.Name = "it_PasteRange";
			this.it_PasteRange.Size = new System.Drawing.Size(190, 22);
			this.it_PasteRange.Text = "paste range";
			this.it_PasteRange.Click += new System.EventHandler(this.editclick_PasteRange);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(187, 6);
			// 
			// it_CopyToClipboard
			// 
			this.it_CopyToClipboard.Enabled = false;
			this.it_CopyToClipboard.Name = "it_CopyToClipboard";
			this.it_CopyToClipboard.Size = new System.Drawing.Size(190, 22);
			this.it_CopyToClipboard.Text = "export copy to clipboard";
			this.it_CopyToClipboard.Click += new System.EventHandler(this.editclick_ExportCopy);
			// 
			// it_CopyFromClipboard
			// 
			this.it_CopyFromClipboard.Enabled = false;
			this.it_CopyFromClipboard.Name = "it_CopyFromClipboard";
			this.it_CopyFromClipboard.Size = new System.Drawing.Size(190, 22);
			this.it_CopyFromClipboard.Text = "import clipboard to copy";
			this.it_CopyFromClipboard.Click += new System.EventHandler(this.editclick_ImportCopy);
			// 
			// it_ViewClipboardContents
			// 
			this.it_ViewClipboardContents.Name = "it_ViewClipboardContents";
			this.it_ViewClipboardContents.Size = new System.Drawing.Size(190, 22);
			this.it_ViewClipboardContents.Text = "view clipboard contents";
			this.it_ViewClipboardContents.Click += new System.EventHandler(this.editclick_ViewClipboard);
			// 
			// tb_Goto
			// 
			this.tb_Goto.AutoSize = false;
			this.tb_Goto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Goto.Margin = new System.Windows.Forms.Padding(1, 0, 6, 0);
			this.tb_Goto.Name = "tb_Goto";
			this.tb_Goto.Size = new System.Drawing.Size(35, 18);
			this.tb_Goto.Text = "goto";
			this.tb_Goto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GotoKeyPress);
			// 
			// tb_Search
			// 
			this.tb_Search.AutoSize = false;
			this.tb_Search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Search.Name = "tb_Search";
			this.tb_Search.Size = new System.Drawing.Size(125, 18);
			this.tb_Search.Text = "search";
			this.tb_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchKeyPress);
			// 
			// cb_SearchOption
			// 
			this.cb_SearchOption.AutoSize = false;
			this.cb_SearchOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_SearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cb_SearchOption.Name = "cb_SearchOption";
			this.cb_SearchOption.Size = new System.Drawing.Size(95, 18);
			this.cb_SearchOption.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchKeyPress);
			// 
			// it_MenuOptions
			// 
			this.it_MenuOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_RenumberRows,
			this.it_CheckRows,
			this.toolStripSeparator1,
			this.it_RecolorRows,
			this.toolStripSeparator2,
			this.it_freeze1,
			this.it_freeze2});
			this.it_MenuOptions.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
			this.it_MenuOptions.Name = "it_MenuOptions";
			this.it_MenuOptions.Size = new System.Drawing.Size(52, 20);
			this.it_MenuOptions.Text = "2da Ops";
			// 
			// it_RenumberRows
			// 
			this.it_RenumberRows.Name = "it_RenumberRows";
			this.it_RenumberRows.Size = new System.Drawing.Size(155, 22);
			this.it_RenumberRows.Text = "order row ids";
			this.it_RenumberRows.Click += new System.EventHandler(this.opsclick_Reorder);
			// 
			// it_CheckRows
			// 
			this.it_CheckRows.Name = "it_CheckRows";
			this.it_CheckRows.Size = new System.Drawing.Size(155, 22);
			this.it_CheckRows.Text = "test row order";
			this.it_CheckRows.Click += new System.EventHandler(this.opsclick_CheckRowOrder);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
			// 
			// it_RecolorRows
			// 
			this.it_RecolorRows.Name = "it_RecolorRows";
			this.it_RecolorRows.Size = new System.Drawing.Size(155, 22);
			this.it_RecolorRows.Text = "recolor rows";
			this.it_RecolorRows.Click += new System.EventHandler(this.opsclick_Recolor);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
			// 
			// it_freeze1
			// 
			this.it_freeze1.Name = "it_freeze1";
			this.it_freeze1.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.it_freeze1.Size = new System.Drawing.Size(155, 22);
			this.it_freeze1.Text = "freeze 1st col";
			this.it_freeze1.Click += new System.EventHandler(this.opsclick_Freeze1stCol);
			// 
			// it_freeze2
			// 
			this.it_freeze2.Name = "it_freeze2";
			this.it_freeze2.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.it_freeze2.Size = new System.Drawing.Size(155, 22);
			this.it_freeze2.Text = "freeze 2nd col";
			this.it_freeze2.Click += new System.EventHandler(this.opsclick_Freeze2ndCol);
			// 
			// fontToolStripMenuItem
			// 
			this.fontToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.fontToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Font,
			this.it_CurrentFont,
			this.it_FontDefault});
			this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
			this.fontToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fontToolStripMenuItem.Text = "Font";
			// 
			// it_Font
			// 
			this.it_Font.Name = "it_Font";
			this.it_Font.Size = new System.Drawing.Size(165, 22);
			this.it_Font.Text = "Font ... be patient";
			this.it_Font.Click += new System.EventHandler(this.fontclick_Font);
			// 
			// it_CurrentFont
			// 
			this.it_CurrentFont.Name = "it_CurrentFont";
			this.it_CurrentFont.Size = new System.Drawing.Size(165, 22);
			this.it_CurrentFont.Text = "current font string";
			this.it_CurrentFont.Click += new System.EventHandler(this.fontclick_CurrentFont);
			// 
			// it_FontDefault
			// 
			this.it_FontDefault.Name = "it_FontDefault";
			this.it_FontDefault.Size = new System.Drawing.Size(165, 22);
			this.it_FontDefault.Text = "default";
			this.it_FontDefault.Click += new System.EventHandler(this.fontclick_Default);
			// 
			// it_MenuPaths
			// 
			this.it_MenuPaths.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuPaths.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_PathAll,
			this.separator_5,
			this.it_PathBaseItems2da,
			this.it_PathFeat2da,
			this.it_PathItemPropDef2da,
			this.it_PathSkills2da,
			this.it_PathSpells2da,
			this.separator_6,
			this.it_PathClasses2da,
			this.it_PathDisease2da,
			this.it_PathIprpAmmoCost2da,
			this.it_PathIprpFeats2da,
			this.it_PathIprpOnHitSpell2da,
			this.it_PathIprpSpells2da,
			this.it_PathRaces2da});
			this.it_MenuPaths.Name = "it_MenuPaths";
			this.it_MenuPaths.Size = new System.Drawing.Size(42, 20);
			this.it_MenuPaths.Text = "Paths";
			this.it_MenuPaths.Visible = false;
			// 
			// it_PathAll
			// 
			this.it_PathAll.Name = "it_PathAll";
			this.it_PathAll.Size = new System.Drawing.Size(190, 22);
			this.it_PathAll.Text = "Path all ...";
			this.it_PathAll.Click += new System.EventHandler(this.itclick_PathAll);
			// 
			// separator_5
			// 
			this.separator_5.Name = "separator_5";
			this.separator_5.Size = new System.Drawing.Size(187, 6);
			// 
			// it_PathBaseItems2da
			// 
			this.it_PathBaseItems2da.Name = "it_PathBaseItems2da";
			this.it_PathBaseItems2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathBaseItems2da.Text = "path BaseItems.2da";
			this.it_PathBaseItems2da.Click += new System.EventHandler(this.itclick_PathBaseItems2da);
			// 
			// it_PathFeat2da
			// 
			this.it_PathFeat2da.Name = "it_PathFeat2da";
			this.it_PathFeat2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathFeat2da.Text = "path Feat.2da";
			this.it_PathFeat2da.Click += new System.EventHandler(this.itclick_PathFeat2da);
			// 
			// it_PathItemPropDef2da
			// 
			this.it_PathItemPropDef2da.Name = "it_PathItemPropDef2da";
			this.it_PathItemPropDef2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathItemPropDef2da.Text = "path ItemPropDef.2da";
			this.it_PathItemPropDef2da.Click += new System.EventHandler(this.itclick_PathItemPropDef2da);
			// 
			// it_PathSkills2da
			// 
			this.it_PathSkills2da.Name = "it_PathSkills2da";
			this.it_PathSkills2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathSkills2da.Text = "path Skills.2da";
			this.it_PathSkills2da.Click += new System.EventHandler(this.itclick_PathSkills2da);
			// 
			// it_PathSpells2da
			// 
			this.it_PathSpells2da.Name = "it_PathSpells2da";
			this.it_PathSpells2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathSpells2da.Text = "path Spells.2da";
			this.it_PathSpells2da.Click += new System.EventHandler(this.itclick_PathSpells2da);
			// 
			// separator_6
			// 
			this.separator_6.Name = "separator_6";
			this.separator_6.Size = new System.Drawing.Size(187, 6);
			// 
			// it_PathClasses2da
			// 
			this.it_PathClasses2da.Enabled = false;
			this.it_PathClasses2da.Name = "it_PathClasses2da";
			this.it_PathClasses2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathClasses2da.Text = "path Classes.2da";
			// 
			// it_PathDisease2da
			// 
			this.it_PathDisease2da.Enabled = false;
			this.it_PathDisease2da.Name = "it_PathDisease2da";
			this.it_PathDisease2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathDisease2da.Text = "path Disease.2da";
			// 
			// it_PathIprpAmmoCost2da
			// 
			this.it_PathIprpAmmoCost2da.Enabled = false;
			this.it_PathIprpAmmoCost2da.Name = "it_PathIprpAmmoCost2da";
			this.it_PathIprpAmmoCost2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathIprpAmmoCost2da.Text = "path Iprp_AmmoCost.2da";
			// 
			// it_PathIprpFeats2da
			// 
			this.it_PathIprpFeats2da.Enabled = false;
			this.it_PathIprpFeats2da.Name = "it_PathIprpFeats2da";
			this.it_PathIprpFeats2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathIprpFeats2da.Text = "path Iprp_Feats.2da";
			// 
			// it_PathIprpOnHitSpell2da
			// 
			this.it_PathIprpOnHitSpell2da.Enabled = false;
			this.it_PathIprpOnHitSpell2da.Name = "it_PathIprpOnHitSpell2da";
			this.it_PathIprpOnHitSpell2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathIprpOnHitSpell2da.Text = "path Iprp_OnHitSpell.2da";
			// 
			// it_PathIprpSpells2da
			// 
			this.it_PathIprpSpells2da.Enabled = false;
			this.it_PathIprpSpells2da.Name = "it_PathIprpSpells2da";
			this.it_PathIprpSpells2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathIprpSpells2da.Text = "path Iprp_Spells.2da";
			// 
			// it_PathRaces2da
			// 
			this.it_PathRaces2da.Enabled = false;
			this.it_PathRaces2da.Name = "it_PathRaces2da";
			this.it_PathRaces2da.Size = new System.Drawing.Size(190, 22);
			this.it_PathRaces2da.Text = "path RacialTypes.2da";
			// 
			// contextEditor
			// 
			this.contextEditor.Font = new System.Drawing.Font("Consolas", 6.5F);
			this.contextEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.context_it_Header,
			this.separator_7,
			this.context_it_Copy,
			this.context_it_Cut,
			this.separator_8,
			this.context_it_PasteAbove,
			this.context_it_Paste,
			this.context_it_PasteBelow,
			this.separator_9,
			this.context_it_CreateAbove,
			this.context_it_ClearRow,
			this.context_it_CreateBelow,
			this.separator_10,
			this.context_it_DeleteRow});
			this.contextEditor.Name = "contextMenuStrip1";
			this.contextEditor.Size = new System.Drawing.Size(176, 248);
			// 
			// context_it_Header
			// 
			this.context_it_Header.Font = new System.Drawing.Font("Consolas", 7F, System.Drawing.FontStyle.Bold);
			this.context_it_Header.ForeColor = System.Drawing.SystemColors.ControlText;
			this.context_it_Header.Name = "context_it_Header";
			this.context_it_Header.Size = new System.Drawing.Size(175, 22);
			this.context_it_Header.Text = "_row @ 16";
			this.context_it_Header.Click += new System.EventHandler(this.contextclick_Header);
			// 
			// separator_7
			// 
			this.separator_7.Name = "separator_7";
			this.separator_7.Size = new System.Drawing.Size(172, 6);
			// 
			// context_it_Copy
			// 
			this.context_it_Copy.Name = "context_it_Copy";
			this.context_it_Copy.Size = new System.Drawing.Size(175, 22);
			this.context_it_Copy.Text = "Copy row @ id";
			this.context_it_Copy.Click += new System.EventHandler(this.contextclick_EditCopy);
			// 
			// context_it_Cut
			// 
			this.context_it_Cut.Name = "context_it_Cut";
			this.context_it_Cut.Size = new System.Drawing.Size(175, 22);
			this.context_it_Cut.Text = "Cut row @ id";
			this.context_it_Cut.Click += new System.EventHandler(this.contextclick_EditCut);
			// 
			// separator_8
			// 
			this.separator_8.Name = "separator_8";
			this.separator_8.Size = new System.Drawing.Size(172, 6);
			// 
			// context_it_PasteAbove
			// 
			this.context_it_PasteAbove.Name = "context_it_PasteAbove";
			this.context_it_PasteAbove.Size = new System.Drawing.Size(175, 22);
			this.context_it_PasteAbove.Text = "Paste clip above id";
			this.context_it_PasteAbove.Click += new System.EventHandler(this.contextclick_EditPasteAbove);
			// 
			// context_it_Paste
			// 
			this.context_it_Paste.Name = "context_it_Paste";
			this.context_it_Paste.Size = new System.Drawing.Size(175, 22);
			this.context_it_Paste.Text = "Paste clip @ id";
			this.context_it_Paste.Click += new System.EventHandler(this.contextclick_EditPaste);
			// 
			// context_it_PasteBelow
			// 
			this.context_it_PasteBelow.Name = "context_it_PasteBelow";
			this.context_it_PasteBelow.Size = new System.Drawing.Size(175, 22);
			this.context_it_PasteBelow.Text = "Paste clip below id";
			this.context_it_PasteBelow.Click += new System.EventHandler(this.contextclick_EditPasteBelow);
			// 
			// separator_9
			// 
			this.separator_9.Name = "separator_9";
			this.separator_9.Size = new System.Drawing.Size(172, 6);
			// 
			// context_it_CreateAbove
			// 
			this.context_it_CreateAbove.Name = "context_it_CreateAbove";
			this.context_it_CreateAbove.Size = new System.Drawing.Size(175, 22);
			this.context_it_CreateAbove.Text = "Create blank above id";
			this.context_it_CreateAbove.Click += new System.EventHandler(this.contextclick_EditCreateAbove);
			// 
			// context_it_ClearRow
			// 
			this.context_it_ClearRow.Name = "context_it_ClearRow";
			this.context_it_ClearRow.Size = new System.Drawing.Size(175, 22);
			this.context_it_ClearRow.Text = "Clear fields @ id";
			this.context_it_ClearRow.Click += new System.EventHandler(this.contextclick_EditClear);
			// 
			// context_it_CreateBelow
			// 
			this.context_it_CreateBelow.Name = "context_it_CreateBelow";
			this.context_it_CreateBelow.Size = new System.Drawing.Size(175, 22);
			this.context_it_CreateBelow.Text = "Create blank below id";
			this.context_it_CreateBelow.Click += new System.EventHandler(this.contextclick_EditCreateBelow);
			// 
			// separator_10
			// 
			this.separator_10.Name = "separator_10";
			this.separator_10.Size = new System.Drawing.Size(172, 6);
			// 
			// context_it_DeleteRow
			// 
			this.context_it_DeleteRow.Name = "context_it_DeleteRow";
			this.context_it_DeleteRow.Size = new System.Drawing.Size(175, 22);
			this.context_it_DeleteRow.Text = "Delete @ id";
			this.context_it_DeleteRow.Click += new System.EventHandler(this.contextclick_EditDelete);
			// 
			// statusbar
			// 
			this.statusbar.Font = new System.Drawing.Font("Consolas", 6.5F);
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.statusbar_label_Coords,
			this.statusbar_label_Info});
			this.statusbar.Location = new System.Drawing.Point(0, 432);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(842, 22);
			this.statusbar.TabIndex = 2;
			this.statusbar.Text = "statusStrip1";
			// 
			// statusbar_label_Coords
			// 
			this.statusbar_label_Coords.AutoSize = false;
			this.statusbar_label_Coords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statusbar_label_Coords.Name = "statusbar_label_Coords";
			this.statusbar_label_Coords.Size = new System.Drawing.Size(160, 17);
			this.statusbar_label_Coords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusbar_label_Info
			// 
			this.statusbar_label_Info.AutoSize = false;
			this.statusbar_label_Info.Font = new System.Drawing.Font("Consolas", 8F);
			this.statusbar_label_Info.Name = "statusbar_label_Info";
			this.statusbar_label_Info.Size = new System.Drawing.Size(667, 17);
			this.statusbar_label_Info.Spring = true;
			this.statusbar_label_Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel_ColorFill
			// 
			this.panel_ColorFill.BackColor = System.Drawing.Color.LightSeaGreen;
			this.panel_ColorFill.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_ColorFill.Location = new System.Drawing.Point(0, 24);
			this.panel_ColorFill.Name = "panel_ColorFill";
			this.panel_ColorFill.Size = new System.Drawing.Size(842, 408);
			this.panel_ColorFill.TabIndex = 3;
			// 
			// it_tabCloseAllOthers
			// 
			this.it_tabCloseAllOthers.Name = "it_tabCloseAllOthers";
			this.it_tabCloseAllOthers.Size = new System.Drawing.Size(152, 22);
			this.it_tabCloseAllOthers.Text = "Close all others";
			this.it_tabCloseAllOthers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAllOthers.Click += new System.EventHandler(this.tabclick_CloseAllOthers);
			// 
			// YataForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(842, 454);
			this.Controls.Add(this.panel_ColorFill);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.menubar);
			this.Controls.Add(this.statusbar);
			this.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menubar;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "YataForm";
			this.Text = "Yata";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.yata_Closing);
			this.tabMenu.ResumeLayout(false);
			this.menubar.ResumeLayout(false);
			this.menubar.PerformLayout();
			this.contextEditor.ResumeLayout(false);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
