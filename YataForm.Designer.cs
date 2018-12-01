namespace yata
{
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
		private System.Windows.Forms.ToolStripStatusLabel statusbar_label_Cords;
		private System.Windows.Forms.ToolStripStatusLabel statusbar_label_Info;
		private System.Windows.Forms.ToolStripMenuItem it_MenuPaths;
		private System.Windows.Forms.ToolStripMenuItem it_PathSpells2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathFeat2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathItemPropDef2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathBaseItems2da;
		private System.Windows.Forms.ToolStripTextBox tb_Search;
		private System.Windows.Forms.ToolStripComboBox cb_SearchOption;
		private System.Windows.Forms.ToolStripMenuItem it_Menu2daOps;
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
		private System.Windows.Forms.ToolStripMenuItem it_OpenFolder;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpFeats2da;
		private System.Windows.Forms.ToolStripSeparator separator_6;
		private System.Windows.Forms.Panel panel_ColorFill;
		private System.Windows.Forms.ContextMenuStrip tabMenu;
		private System.Windows.Forms.ToolStripMenuItem it_tabClose;
		private System.Windows.Forms.ToolStripMenuItem it_Search;
		private System.Windows.Forms.ToolStripMenuItem it_Findnext;
		private System.Windows.Forms.ToolStripMenuItem it_MenuFont;
		private System.Windows.Forms.ToolStripSeparator separator_11;
		private System.Windows.Forms.ToolStripSeparator separator_12;
		private System.Windows.Forms.ToolStripMenuItem it_Font;
		private System.Windows.Forms.ToolStripMenuItem it_FontDefault;
		private System.Windows.Forms.ToolStripMenuItem it_PathIprpAmmoCost2da;
		private System.Windows.Forms.ToolStripMenuItem it_tabCloseAll;
		private System.Windows.Forms.ToolStripMenuItem it_CloseAll;
		private System.Windows.Forms.ToolStripTextBox tb_Goto;
		private System.Windows.Forms.ToolStripSeparator separator_3;
		private System.Windows.Forms.ToolStripMenuItem it_Goto;
		private System.Windows.Forms.ToolStripMenuItem it_GotoLoadchanged;
		private System.Windows.Forms.ToolStripSeparator separator_4;
		private System.Windows.Forms.ToolStripMenuItem it_CopyRange;
		private System.Windows.Forms.ToolStripMenuItem it_PasteRange;
		private System.Windows.Forms.ToolStripSeparator separator_13;
		private System.Windows.Forms.ToolStripMenuItem it_CopyToClipboard;
		private System.Windows.Forms.ToolStripMenuItem it_CopyFromClipboard;
		private System.Windows.Forms.ToolStripMenuItem it_ViewClipboardContents;
		private System.Windows.Forms.ToolStripMenuItem it_tabCloseAllOthers;
		private System.Windows.Forms.ToolStripMenuItem it_AutoCols;
		private System.Windows.Forms.ToolStripSeparator separator_14;
		private System.Windows.Forms.ToolStripMenuItem it_tabReload;
		private System.Windows.Forms.ToolStripMenuItem it_MenuHelp;
		private System.Windows.Forms.ToolStripMenuItem it_ReadMe;
		private System.Windows.Forms.ToolStripMenuItem it_About;
		private System.Windows.Forms.ToolStripSeparator separator_15;
		private System.Windows.Forms.ToolStripMenuItem it_PathRanges2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathCategories2da;
		private System.Windows.Forms.ToolStripMenuItem it_PathSpellTarget2da;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tabControl = new yata.YataTabs();
			this.tabMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.it_tabClose = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabCloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabCloseAllOthers = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_14 = new System.Windows.Forms.ToolStripSeparator();
			this.it_tabReload = new System.Windows.Forms.ToolStripMenuItem();
			this.menubar = new System.Windows.Forms.MenuStrip();
			this.it_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.it_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Reload = new System.Windows.Forms.ToolStripMenuItem();
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
			this.separator_3 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Goto = new System.Windows.Forms.ToolStripMenuItem();
			this.it_GotoLoadchanged = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_4 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_13 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ViewClipboardContents = new System.Windows.Forms.ToolStripMenuItem();
			this.tb_Goto = new System.Windows.Forms.ToolStripTextBox();
			this.tb_Search = new System.Windows.Forms.ToolStripTextBox();
			this.cb_SearchOption = new System.Windows.Forms.ToolStripComboBox();
			this.it_Menu2daOps = new System.Windows.Forms.ToolStripMenuItem();
			this.it_RenumberRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CheckRows = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_11 = new System.Windows.Forms.ToolStripSeparator();
			this.it_RecolorRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_AutoCols = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_12 = new System.Windows.Forms.ToolStripSeparator();
			this.it_freeze1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_freeze2 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuFont = new System.Windows.Forms.ToolStripMenuItem();
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
			this.separator_15 = new System.Windows.Forms.ToolStripSeparator();
			this.it_PathCategories2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathRanges2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathSpellTarget2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ReadMe = new System.Windows.Forms.ToolStripMenuItem();
			this.it_About = new System.Windows.Forms.ToolStripMenuItem();
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
			this.statusbar_label_Cords = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusbar_label_Info = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel_ColorFill = new System.Windows.Forms.Panel();
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
			this.tabMenu.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_tabClose,
			this.it_tabCloseAll,
			this.it_tabCloseAllOthers,
			this.separator_14,
			this.it_tabReload});
			this.tabMenu.Name = "tabMenu";
			this.tabMenu.Size = new System.Drawing.Size(164, 98);
			this.tabMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tabMenu_Opening);
			// 
			// it_tabClose
			// 
			this.it_tabClose.Name = "it_tabClose";
			this.it_tabClose.Size = new System.Drawing.Size(163, 22);
			this.it_tabClose.Text = "Close";
			this.it_tabClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabClose.Click += new System.EventHandler(this.tabclick_Close);
			// 
			// it_tabCloseAll
			// 
			this.it_tabCloseAll.Name = "it_tabCloseAll";
			this.it_tabCloseAll.Size = new System.Drawing.Size(163, 22);
			this.it_tabCloseAll.Text = "Close all";
			this.it_tabCloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAll.Click += new System.EventHandler(this.tabclick_CloseAll);
			// 
			// it_tabCloseAllOthers
			// 
			this.it_tabCloseAllOthers.Name = "it_tabCloseAllOthers";
			this.it_tabCloseAllOthers.Size = new System.Drawing.Size(163, 22);
			this.it_tabCloseAllOthers.Text = "Close all others";
			this.it_tabCloseAllOthers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAllOthers.Click += new System.EventHandler(this.tabclick_CloseAllOtherTabs);
			// 
			// separator_14
			// 
			this.separator_14.Name = "separator_14";
			this.separator_14.Size = new System.Drawing.Size(160, 6);
			// 
			// it_tabReload
			// 
			this.it_tabReload.Name = "it_tabReload";
			this.it_tabReload.Size = new System.Drawing.Size(163, 22);
			this.it_tabReload.Text = "Reload";
			this.it_tabReload.Click += new System.EventHandler(this.tabclick_Reload);
			// 
			// menubar
			// 
			this.menubar.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_MenuFile,
			this.it_MenuEdit,
			this.tb_Goto,
			this.tb_Search,
			this.cb_SearchOption,
			this.it_Menu2daOps,
			this.it_MenuFont,
			this.it_MenuPaths,
			this.it_MenuHelp});
			this.menubar.Location = new System.Drawing.Point(0, 0);
			this.menubar.Name = "menubar";
			this.menubar.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.menubar.Size = new System.Drawing.Size(842, 24);
			this.menubar.TabIndex = 0;
			// 
			// it_MenuFile
			// 
			this.it_MenuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_OpenFolder,
			this.it_Open,
			this.it_Reload,
			this.separator_1,
			this.it_Save,
			this.it_SaveAs,
			this.it_Close,
			this.it_CloseAll,
			this.separator_2,
			this.it_Quit});
			this.it_MenuFile.Name = "it_MenuFile";
			this.it_MenuFile.Size = new System.Drawing.Size(38, 20);
			this.it_MenuFile.Text = "File";
			this.it_MenuFile.DropDownOpening += new System.EventHandler(this.file_dropdownopening);
			// 
			// it_OpenFolder
			// 
			this.it_OpenFolder.Name = "it_OpenFolder";
			this.it_OpenFolder.Size = new System.Drawing.Size(171, 22);
			this.it_OpenFolder.Text = "Open ... @ folder";
			this.it_OpenFolder.Visible = false;
			// 
			// it_Open
			// 
			this.it_Open.Name = "it_Open";
			this.it_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.it_Open.Size = new System.Drawing.Size(171, 22);
			this.it_Open.Text = "Open ...";
			this.it_Open.Click += new System.EventHandler(this.fileclick_Open);
			// 
			// it_Reload
			// 
			this.it_Reload.Name = "it_Reload";
			this.it_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.it_Reload.Size = new System.Drawing.Size(171, 22);
			this.it_Reload.Text = "Reload";
			this.it_Reload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// separator_1
			// 
			this.separator_1.Name = "separator_1";
			this.separator_1.Size = new System.Drawing.Size(168, 6);
			// 
			// it_Save
			// 
			this.it_Save.Name = "it_Save";
			this.it_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.it_Save.Size = new System.Drawing.Size(171, 22);
			this.it_Save.Text = "Save";
			this.it_Save.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// it_SaveAs
			// 
			this.it_SaveAs.Name = "it_SaveAs";
			this.it_SaveAs.Size = new System.Drawing.Size(171, 22);
			this.it_SaveAs.Text = "Save As ...";
			this.it_SaveAs.Click += new System.EventHandler(this.fileclick_SaveAs);
			// 
			// it_Close
			// 
			this.it_Close.Name = "it_Close";
			this.it_Close.Size = new System.Drawing.Size(171, 22);
			this.it_Close.Text = "Close";
			this.it_Close.Click += new System.EventHandler(this.fileclick_CloseTab);
			// 
			// it_CloseAll
			// 
			this.it_CloseAll.Name = "it_CloseAll";
			this.it_CloseAll.Size = new System.Drawing.Size(171, 22);
			this.it_CloseAll.Text = "Close all";
			this.it_CloseAll.Click += new System.EventHandler(this.fileclick_CloseAllTabs);
			// 
			// separator_2
			// 
			this.separator_2.Name = "separator_2";
			this.separator_2.Size = new System.Drawing.Size(168, 6);
			// 
			// it_Quit
			// 
			this.it_Quit.Name = "it_Quit";
			this.it_Quit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.it_Quit.Size = new System.Drawing.Size(171, 22);
			this.it_Quit.Text = "Quit";
			this.it_Quit.Click += new System.EventHandler(this.fileclick_Quit);
			// 
			// it_MenuEdit
			// 
			this.it_MenuEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Search,
			this.it_Findnext,
			this.separator_3,
			this.it_Goto,
			this.it_GotoLoadchanged,
			this.separator_4,
			this.it_CopyRange,
			this.it_PasteRange,
			this.separator_13,
			this.it_CopyToClipboard,
			this.it_CopyFromClipboard,
			this.it_ViewClipboardContents});
			this.it_MenuEdit.Name = "it_MenuEdit";
			this.it_MenuEdit.Size = new System.Drawing.Size(40, 20);
			this.it_MenuEdit.Text = "Edit";
			this.it_MenuEdit.DropDownOpening += new System.EventHandler(this.edit_dropdownopening);
			// 
			// it_Search
			// 
			this.it_Search.Name = "it_Search";
			this.it_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.it_Search.Size = new System.Drawing.Size(216, 22);
			this.it_Search.Text = "Find";
			this.it_Search.Click += new System.EventHandler(this.editclick_Search);
			// 
			// it_Findnext
			// 
			this.it_Findnext.Name = "it_Findnext";
			this.it_Findnext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.it_Findnext.Size = new System.Drawing.Size(216, 22);
			this.it_Findnext.Text = "Find next";
			this.it_Findnext.Click += new System.EventHandler(this.editclick_SearchNext);
			// 
			// separator_3
			// 
			this.separator_3.Name = "separator_3";
			this.separator_3.Size = new System.Drawing.Size(213, 6);
			// 
			// it_Goto
			// 
			this.it_Goto.Name = "it_Goto";
			this.it_Goto.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.it_Goto.Size = new System.Drawing.Size(216, 22);
			this.it_Goto.Text = "Goto";
			this.it_Goto.Click += new System.EventHandler(this.editclick_Goto);
			// 
			// it_GotoLoadchanged
			// 
			this.it_GotoLoadchanged.Enabled = false;
			this.it_GotoLoadchanged.Name = "it_GotoLoadchanged";
			this.it_GotoLoadchanged.Size = new System.Drawing.Size(216, 22);
			this.it_GotoLoadchanged.Text = "Goto loadchanged";
			this.it_GotoLoadchanged.Click += new System.EventHandler(this.editclick_GotoLoadchanged);
			// 
			// separator_4
			// 
			this.separator_4.Name = "separator_4";
			this.separator_4.Size = new System.Drawing.Size(213, 6);
			// 
			// it_CopyRange
			// 
			this.it_CopyRange.Enabled = false;
			this.it_CopyRange.Name = "it_CopyRange";
			this.it_CopyRange.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.it_CopyRange.Size = new System.Drawing.Size(216, 22);
			this.it_CopyRange.Text = "copy range";
			this.it_CopyRange.Click += new System.EventHandler(this.editclick_CopyRange);
			// 
			// it_PasteRange
			// 
			this.it_PasteRange.Enabled = false;
			this.it_PasteRange.Name = "it_PasteRange";
			this.it_PasteRange.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.it_PasteRange.Size = new System.Drawing.Size(216, 22);
			this.it_PasteRange.Text = "paste range";
			this.it_PasteRange.Click += new System.EventHandler(this.editclick_PasteRange);
			// 
			// separator_13
			// 
			this.separator_13.Name = "separator_13";
			this.separator_13.Size = new System.Drawing.Size(213, 6);
			// 
			// it_CopyToClipboard
			// 
			this.it_CopyToClipboard.Enabled = false;
			this.it_CopyToClipboard.Name = "it_CopyToClipboard";
			this.it_CopyToClipboard.Size = new System.Drawing.Size(216, 22);
			this.it_CopyToClipboard.Text = "export copy to Clipboard";
			this.it_CopyToClipboard.Click += new System.EventHandler(this.editclick_ExportCopy);
			// 
			// it_CopyFromClipboard
			// 
			this.it_CopyFromClipboard.Enabled = false;
			this.it_CopyFromClipboard.Name = "it_CopyFromClipboard";
			this.it_CopyFromClipboard.Size = new System.Drawing.Size(216, 22);
			this.it_CopyFromClipboard.Text = "import Clipboard to copy";
			this.it_CopyFromClipboard.Click += new System.EventHandler(this.editclick_ImportCopy);
			// 
			// it_ViewClipboardContents
			// 
			this.it_ViewClipboardContents.Name = "it_ViewClipboardContents";
			this.it_ViewClipboardContents.Size = new System.Drawing.Size(216, 22);
			this.it_ViewClipboardContents.Text = "open Clipboard editor";
			this.it_ViewClipboardContents.Click += new System.EventHandler(this.editclick_ViewClipboard);
			// 
			// tb_Goto
			// 
			this.tb_Goto.AutoSize = false;
			this.tb_Goto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Goto.Margin = new System.Windows.Forms.Padding(1, 0, 5, 0);
			this.tb_Goto.Name = "tb_Goto";
			this.tb_Goto.Size = new System.Drawing.Size(36, 18);
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
			this.tb_Search.TextChanged += new System.EventHandler(this.textchanged_Search);
			// 
			// cb_SearchOption
			// 
			this.cb_SearchOption.AutoSize = false;
			this.cb_SearchOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_SearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cb_SearchOption.Name = "cb_SearchOption";
			this.cb_SearchOption.Size = new System.Drawing.Size(100, 18);
			this.cb_SearchOption.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchKeyPress);
			// 
			// it_Menu2daOps
			// 
			this.it_Menu2daOps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_Menu2daOps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_RenumberRows,
			this.it_CheckRows,
			this.separator_11,
			this.it_RecolorRows,
			this.it_AutoCols,
			this.separator_12,
			this.it_freeze1,
			this.it_freeze2});
			this.it_Menu2daOps.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.it_Menu2daOps.Name = "it_Menu2daOps";
			this.it_Menu2daOps.Size = new System.Drawing.Size(66, 20);
			this.it_Menu2daOps.Text = "2da Ops";
			this.it_Menu2daOps.DropDownOpening += new System.EventHandler(this.ops_dropdownopening);
			// 
			// it_RenumberRows
			// 
			this.it_RenumberRows.Name = "it_RenumberRows";
			this.it_RenumberRows.Size = new System.Drawing.Size(175, 22);
			this.it_RenumberRows.Text = "order row ids";
			this.it_RenumberRows.Click += new System.EventHandler(this.opsclick_Reorder);
			// 
			// it_CheckRows
			// 
			this.it_CheckRows.Name = "it_CheckRows";
			this.it_CheckRows.Size = new System.Drawing.Size(175, 22);
			this.it_CheckRows.Text = "test row order";
			this.it_CheckRows.Click += new System.EventHandler(this.opsclick_CheckRowOrder);
			// 
			// separator_11
			// 
			this.separator_11.Name = "separator_11";
			this.separator_11.Size = new System.Drawing.Size(172, 6);
			// 
			// it_RecolorRows
			// 
			this.it_RecolorRows.Name = "it_RecolorRows";
			this.it_RecolorRows.Size = new System.Drawing.Size(175, 22);
			this.it_RecolorRows.Text = "recolor rows";
			this.it_RecolorRows.Click += new System.EventHandler(this.opsclick_Recolor);
			// 
			// it_AutoCols
			// 
			this.it_AutoCols.Name = "it_AutoCols";
			this.it_AutoCols.Size = new System.Drawing.Size(175, 22);
			this.it_AutoCols.Text = "autosize cols";
			this.it_AutoCols.Click += new System.EventHandler(this.opsclick_AutosizeCols);
			// 
			// separator_12
			// 
			this.separator_12.Name = "separator_12";
			this.separator_12.Size = new System.Drawing.Size(172, 6);
			// 
			// it_freeze1
			// 
			this.it_freeze1.Name = "it_freeze1";
			this.it_freeze1.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.it_freeze1.Size = new System.Drawing.Size(175, 22);
			this.it_freeze1.Text = "freeze 1st col";
			this.it_freeze1.Click += new System.EventHandler(this.opsclick_Freeze1stCol);
			// 
			// it_freeze2
			// 
			this.it_freeze2.Name = "it_freeze2";
			this.it_freeze2.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.it_freeze2.Size = new System.Drawing.Size(175, 22);
			this.it_freeze2.Text = "freeze 2nd col";
			this.it_freeze2.Click += new System.EventHandler(this.opsclick_Freeze2ndCol);
			// 
			// it_MenuFont
			// 
			this.it_MenuFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuFont.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Font,
			this.it_CurrentFont,
			this.it_FontDefault});
			this.it_MenuFont.Name = "it_MenuFont";
			this.it_MenuFont.Size = new System.Drawing.Size(43, 20);
			this.it_MenuFont.Text = "Font";
			this.it_MenuFont.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_Font
			// 
			this.it_Font.Name = "it_Font";
			this.it_Font.Size = new System.Drawing.Size(177, 22);
			this.it_Font.Text = "Font ... be patient";
			this.it_Font.Click += new System.EventHandler(this.fontclick_Font);
			// 
			// it_CurrentFont
			// 
			this.it_CurrentFont.Name = "it_CurrentFont";
			this.it_CurrentFont.Size = new System.Drawing.Size(177, 22);
			this.it_CurrentFont.Text = "current font string";
			this.it_CurrentFont.Click += new System.EventHandler(this.fontclick_CurrentFont);
			// 
			// it_FontDefault
			// 
			this.it_FontDefault.Name = "it_FontDefault";
			this.it_FontDefault.Size = new System.Drawing.Size(177, 22);
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
			this.it_PathRaces2da,
			this.separator_15,
			this.it_PathCategories2da,
			this.it_PathRanges2da,
			this.it_PathSpellTarget2da});
			this.it_MenuPaths.Name = "it_MenuPaths";
			this.it_MenuPaths.Size = new System.Drawing.Size(50, 20);
			this.it_MenuPaths.Text = "Paths";
			this.it_MenuPaths.Visible = false;
			this.it_MenuPaths.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_PathAll
			// 
			this.it_PathAll.Name = "it_PathAll";
			this.it_PathAll.Size = new System.Drawing.Size(222, 22);
			this.it_PathAll.Text = "Path all ...";
			this.it_PathAll.Click += new System.EventHandler(this.itclick_PathAll);
			// 
			// separator_5
			// 
			this.separator_5.Name = "separator_5";
			this.separator_5.Size = new System.Drawing.Size(219, 6);
			// 
			// it_PathBaseItems2da
			// 
			this.it_PathBaseItems2da.Name = "it_PathBaseItems2da";
			this.it_PathBaseItems2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathBaseItems2da.Text = "path BaseItems.2da";
			this.it_PathBaseItems2da.Click += new System.EventHandler(this.itclick_PathBaseItems2da);
			// 
			// it_PathFeat2da
			// 
			this.it_PathFeat2da.Name = "it_PathFeat2da";
			this.it_PathFeat2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathFeat2da.Text = "path Feat.2da";
			this.it_PathFeat2da.Click += new System.EventHandler(this.itclick_PathFeat2da);
			// 
			// it_PathItemPropDef2da
			// 
			this.it_PathItemPropDef2da.Name = "it_PathItemPropDef2da";
			this.it_PathItemPropDef2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathItemPropDef2da.Text = "path ItemPropDef.2da";
			this.it_PathItemPropDef2da.Click += new System.EventHandler(this.itclick_PathItemPropDef2da);
			// 
			// it_PathSkills2da
			// 
			this.it_PathSkills2da.Name = "it_PathSkills2da";
			this.it_PathSkills2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSkills2da.Text = "path Skills.2da";
			this.it_PathSkills2da.Click += new System.EventHandler(this.itclick_PathSkills2da);
			// 
			// it_PathSpells2da
			// 
			this.it_PathSpells2da.Name = "it_PathSpells2da";
			this.it_PathSpells2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSpells2da.Text = "path Spells.2da";
			this.it_PathSpells2da.Click += new System.EventHandler(this.itclick_PathSpells2da);
			// 
			// separator_6
			// 
			this.separator_6.Name = "separator_6";
			this.separator_6.Size = new System.Drawing.Size(219, 6);
			// 
			// it_PathClasses2da
			// 
			this.it_PathClasses2da.Name = "it_PathClasses2da";
			this.it_PathClasses2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathClasses2da.Text = "path Classes.2da";
			this.it_PathClasses2da.Click += new System.EventHandler(this.itclick_PathClasses2da);
			// 
			// it_PathDisease2da
			// 
			this.it_PathDisease2da.Name = "it_PathDisease2da";
			this.it_PathDisease2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathDisease2da.Text = "path Disease.2da";
			this.it_PathDisease2da.Click += new System.EventHandler(this.itclick_PathDisease2da);
			// 
			// it_PathIprpAmmoCost2da
			// 
			this.it_PathIprpAmmoCost2da.Name = "it_PathIprpAmmoCost2da";
			this.it_PathIprpAmmoCost2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpAmmoCost2da.Text = "path Iprp_AmmoCost.2da";
			this.it_PathIprpAmmoCost2da.Click += new System.EventHandler(this.itclick_PathIprpAmmoCost2da);
			// 
			// it_PathIprpFeats2da
			// 
			this.it_PathIprpFeats2da.Name = "it_PathIprpFeats2da";
			this.it_PathIprpFeats2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpFeats2da.Text = "path Iprp_Feats.2da";
			this.it_PathIprpFeats2da.Click += new System.EventHandler(this.itclick_PathIprpFeats2da);
			// 
			// it_PathIprpOnHitSpell2da
			// 
			this.it_PathIprpOnHitSpell2da.Name = "it_PathIprpOnHitSpell2da";
			this.it_PathIprpOnHitSpell2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpOnHitSpell2da.Text = "path Iprp_OnHitSpell.2da";
			this.it_PathIprpOnHitSpell2da.Click += new System.EventHandler(this.itclick_PathIprpOnHitSpells2da);
			// 
			// it_PathIprpSpells2da
			// 
			this.it_PathIprpSpells2da.Name = "it_PathIprpSpells2da";
			this.it_PathIprpSpells2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpSpells2da.Text = "path Iprp_Spells.2da";
			this.it_PathIprpSpells2da.Click += new System.EventHandler(this.itclick_PathIprpSpells2da);
			// 
			// it_PathRaces2da
			// 
			this.it_PathRaces2da.Name = "it_PathRaces2da";
			this.it_PathRaces2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathRaces2da.Text = "path RacialTypes.2da";
			this.it_PathRaces2da.Click += new System.EventHandler(this.itclick_PathRaces2da);
			// 
			// separator_15
			// 
			this.separator_15.Name = "separator_15";
			this.separator_15.Size = new System.Drawing.Size(219, 6);
			// 
			// it_PathCategories2da
			// 
			this.it_PathCategories2da.Name = "it_PathCategories2da";
			this.it_PathCategories2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathCategories2da.Text = "path Categories.2da";
			this.it_PathCategories2da.Click += new System.EventHandler(this.itclick_PathCategories2da);
			// 
			// it_PathRanges2da
			// 
			this.it_PathRanges2da.Name = "it_PathRanges2da";
			this.it_PathRanges2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathRanges2da.Text = "path Ranges.2da";
			this.it_PathRanges2da.Click += new System.EventHandler(this.itclick_PathRanges2da);
			// 
			// it_PathSpellTarget2da
			// 
			this.it_PathSpellTarget2da.Name = "it_PathSpellTarget2da";
			this.it_PathSpellTarget2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSpellTarget2da.Text = "path SpellTarget.2da";
			this.it_PathSpellTarget2da.Click += new System.EventHandler(this.itclick_PathSpellTarget2da);
			// 
			// it_MenuHelp
			// 
			this.it_MenuHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_ReadMe,
			this.it_About});
			this.it_MenuHelp.Name = "it_MenuHelp";
			this.it_MenuHelp.Size = new System.Drawing.Size(44, 20);
			this.it_MenuHelp.Text = "Help";
			this.it_MenuHelp.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_ReadMe
			// 
			this.it_ReadMe.Name = "it_ReadMe";
			this.it_ReadMe.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.it_ReadMe.Size = new System.Drawing.Size(158, 22);
			this.it_ReadMe.Text = "ReadMe.txt";
			this.it_ReadMe.Click += new System.EventHandler(this.helpclick_Help);
			// 
			// it_About
			// 
			this.it_About.Name = "it_About";
			this.it_About.Size = new System.Drawing.Size(158, 22);
			this.it_About.Text = "About";
			this.it_About.Click += new System.EventHandler(this.helpclick_About);
			// 
			// contextEditor
			// 
			this.contextEditor.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.contextEditor.Size = new System.Drawing.Size(202, 248);
			// 
			// context_it_Header
			// 
			this.context_it_Header.Font = new System.Drawing.Font("Verdana", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.context_it_Header.Name = "context_it_Header";
			this.context_it_Header.Size = new System.Drawing.Size(201, 22);
			this.context_it_Header.Text = "_row @ 16";
			this.context_it_Header.Click += new System.EventHandler(this.contextclick_Header);
			// 
			// separator_7
			// 
			this.separator_7.Name = "separator_7";
			this.separator_7.Size = new System.Drawing.Size(198, 6);
			// 
			// context_it_Copy
			// 
			this.context_it_Copy.Name = "context_it_Copy";
			this.context_it_Copy.Size = new System.Drawing.Size(201, 22);
			this.context_it_Copy.Text = "Copy row @ id";
			this.context_it_Copy.Click += new System.EventHandler(this.contextclick_EditCopy);
			// 
			// context_it_Cut
			// 
			this.context_it_Cut.Name = "context_it_Cut";
			this.context_it_Cut.Size = new System.Drawing.Size(201, 22);
			this.context_it_Cut.Text = "Cut row @ id";
			this.context_it_Cut.Click += new System.EventHandler(this.contextclick_EditCut);
			// 
			// separator_8
			// 
			this.separator_8.Name = "separator_8";
			this.separator_8.Size = new System.Drawing.Size(198, 6);
			// 
			// context_it_PasteAbove
			// 
			this.context_it_PasteAbove.Name = "context_it_PasteAbove";
			this.context_it_PasteAbove.Size = new System.Drawing.Size(201, 22);
			this.context_it_PasteAbove.Text = "Paste clip above id";
			this.context_it_PasteAbove.Click += new System.EventHandler(this.contextclick_EditPasteAbove);
			// 
			// context_it_Paste
			// 
			this.context_it_Paste.Name = "context_it_Paste";
			this.context_it_Paste.Size = new System.Drawing.Size(201, 22);
			this.context_it_Paste.Text = "Paste clip @ id";
			this.context_it_Paste.Click += new System.EventHandler(this.contextclick_EditPaste);
			// 
			// context_it_PasteBelow
			// 
			this.context_it_PasteBelow.Name = "context_it_PasteBelow";
			this.context_it_PasteBelow.Size = new System.Drawing.Size(201, 22);
			this.context_it_PasteBelow.Text = "Paste clip below id";
			this.context_it_PasteBelow.Click += new System.EventHandler(this.contextclick_EditPasteBelow);
			// 
			// separator_9
			// 
			this.separator_9.Name = "separator_9";
			this.separator_9.Size = new System.Drawing.Size(198, 6);
			// 
			// context_it_CreateAbove
			// 
			this.context_it_CreateAbove.Name = "context_it_CreateAbove";
			this.context_it_CreateAbove.Size = new System.Drawing.Size(201, 22);
			this.context_it_CreateAbove.Text = "Create blank above id";
			this.context_it_CreateAbove.Click += new System.EventHandler(this.contextclick_EditCreateAbove);
			// 
			// context_it_ClearRow
			// 
			this.context_it_ClearRow.Name = "context_it_ClearRow";
			this.context_it_ClearRow.Size = new System.Drawing.Size(201, 22);
			this.context_it_ClearRow.Text = "Clear fields @ id";
			this.context_it_ClearRow.Click += new System.EventHandler(this.contextclick_EditClear);
			// 
			// context_it_CreateBelow
			// 
			this.context_it_CreateBelow.Name = "context_it_CreateBelow";
			this.context_it_CreateBelow.Size = new System.Drawing.Size(201, 22);
			this.context_it_CreateBelow.Text = "Create blank below id";
			this.context_it_CreateBelow.Click += new System.EventHandler(this.contextclick_EditCreateBelow);
			// 
			// separator_10
			// 
			this.separator_10.Name = "separator_10";
			this.separator_10.Size = new System.Drawing.Size(198, 6);
			// 
			// context_it_DeleteRow
			// 
			this.context_it_DeleteRow.Name = "context_it_DeleteRow";
			this.context_it_DeleteRow.Size = new System.Drawing.Size(201, 22);
			this.context_it_DeleteRow.Text = "Delete @ id";
			this.context_it_DeleteRow.Click += new System.EventHandler(this.contextclick_EditDelete);
			// 
			// statusbar
			// 
			this.statusbar.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.statusbar_label_Cords,
			this.statusbar_label_Info});
			this.statusbar.Location = new System.Drawing.Point(0, 432);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(842, 22);
			this.statusbar.TabIndex = 2;
			// 
			// statusbar_label_Cords
			// 
			this.statusbar_label_Cords.AutoSize = false;
			this.statusbar_label_Cords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statusbar_label_Cords.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusbar_label_Cords.Name = "statusbar_label_Cords";
			this.statusbar_label_Cords.Size = new System.Drawing.Size(160, 17);
			this.statusbar_label_Cords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusbar_label_Info
			// 
			this.statusbar_label_Info.AutoSize = false;
			this.statusbar_label_Info.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			// YataForm
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(842, 454);
			this.Controls.Add(this.panel_ColorFill);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.menubar);
			this.Controls.Add(this.statusbar);
			this.Font = new System.Drawing.Font("Georgia", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.MainMenuStrip = this.menubar;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "YataForm";
			this.Text = "Yata";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.yata_Closing);
			this.Load += new System.EventHandler(this.yata_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.yata_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.yata_DragEnter);
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
