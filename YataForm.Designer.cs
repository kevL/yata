using System.Windows.Forms;


namespace yata
{
	partial class YataForm
	{
		// okay. Fed up.
		// YataTabs 'tabControl' and PropertyPanelButton 'btn_PropertyPanel'
		// have been moved to YataForm.


		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		System.ComponentModel.IContainer components = null;

		MenuStrip menubar;

		ToolStripMenuItem it_MenuFile;
		ToolStripMenuItem it_OpenFolder;
		ToolStripMenuItem it_Open;
		ToolStripMenuItem it_Reload;
		ToolStripMenuItem it_Save;
		ToolStripMenuItem it_SaveAs;
		ToolStripMenuItem it_Close;
		ToolStripMenuItem it_CloseAll;
		ToolStripMenuItem it_Quit;

		ToolStripMenuItem it_MenuEdit;
		ToolStripMenuItem it_Undo;
		ToolStripMenuItem it_Redo;
		ToolStripMenuItem it_Search;
		ToolStripMenuItem it_Searchnext;
		ToolStripMenuItem it_Goto;
		ToolStripMenuItem it_GotoLoadchanged;
		internal ToolStripMenuItem it_CopyCell;
		internal ToolStripMenuItem it_PasteCell;
		ToolStripMenuItem it_CopyRange;
		ToolStripMenuItem it_PasteRange;
		ToolStripMenuItem it_CreateRows;

		ToolStripTextBox tb_Goto;

		ToolStripTextBox tb_Search;
		ToolStripComboBox cb_SearchOption;

		ToolStripMenuItem it_MenuClipboard;
		ToolStripMenuItem it_CopyToClipboard;
		ToolStripMenuItem it_CopyFromClipboard;
		ToolStripMenuItem it_ViewClipboardContents;

		ToolStripMenuItem it_Menu2daOps;
		ToolStripMenuItem it_OrderRows;
		ToolStripMenuItem it_CheckRows;
		ToolStripMenuItem it_RecolorRows;
		ToolStripMenuItem it_AutoCols;
		ToolStripMenuItem it_freeze1;
		ToolStripMenuItem it_freeze2;
		ToolStripMenuItem it_ppOnOff;
		ToolStripMenuItem it_ppTopBot;
		ToolStripMenuItem it_ClearUndoRedo;

		ToolStripMenuItem it_MenuFont;
		ToolStripMenuItem it_Font;
		ToolStripMenuItem it_CurrentFont;
		ToolStripMenuItem it_FontDefault;

		ToolStripMenuItem it_MenuPaths;
		ToolStripMenuItem it_PathAll;
		ToolStripMenuItem it_PathBaseItems2da;
		ToolStripMenuItem it_PathFeat2da;
		ToolStripMenuItem it_PathItemPropDef2da;
		ToolStripMenuItem it_PathSkills2da;
		ToolStripMenuItem it_PathSpells2da;
		ToolStripMenuItem it_PathClasses2da;
		ToolStripMenuItem it_PathDisease2da;
		ToolStripMenuItem it_PathIprpAmmoCost2da;
		ToolStripMenuItem it_PathIprpFeats2da;
		ToolStripMenuItem it_PathIprpOnHitSpell2da;
		ToolStripMenuItem it_PathIprpSpells2da;
		ToolStripMenuItem it_PathRaces2da;
		ToolStripMenuItem it_PathCategories2da;
		ToolStripMenuItem it_PathRanges2da;
		ToolStripMenuItem it_PathSpellTarget2da;

		ToolStripMenuItem it_MenuHelp;
		ToolStripMenuItem it_ReadMe;
		ToolStripMenuItem it_About;

		ToolStripSeparator separator_1;
		ToolStripSeparator separator_2;
		ToolStripSeparator separator_3;
		ToolStripSeparator separator_4;
		ToolStripSeparator separator_5;
		ToolStripSeparator separator_6;
		ToolStripSeparator separator_7;
		ToolStripSeparator separator_8;
		ToolStripSeparator separator_9;
		ToolStripSeparator separator_10;
		ToolStripSeparator separator_11;
		ToolStripSeparator separator_12;
		ToolStripSeparator separator_13;
		ToolStripSeparator separator_14;
		ToolStripSeparator separator_15;
		ToolStripSeparator separator_16;
		ToolStripSeparator separator_17;
		ToolStripSeparator separator_18;
		ToolStripSeparator separator_19;
		ToolStripSeparator separator_20;
		ToolStripSeparator separator_21;

		ContextMenuStrip contextEditor;
		ToolStripMenuItem context_it_Header;
		ToolStripMenuItem context_it_Copy;
		ToolStripMenuItem context_it_Cut;
		ToolStripMenuItem context_it_PasteAbove;
		ToolStripMenuItem context_it_Paste;
		ToolStripMenuItem context_it_PasteBelow;
		ToolStripMenuItem context_it_CreateAbove;
		ToolStripMenuItem context_it_ClearRow;
		ToolStripMenuItem context_it_CreateBelow;
		ToolStripMenuItem context_it_DeleteRow;

		ContextMenuStrip tabMenu;
		ToolStripMenuItem it_tabClose;
		ToolStripMenuItem it_tabCloseAll;
		ToolStripMenuItem it_tabCloseAllOthers;
		ToolStripMenuItem it_tabReload;

		ContextMenuStrip cellMenu;
		ToolStripMenuItem it_cellCopy;
		ToolStripMenuItem it_cellPaste;
		ToolStripMenuItem it_cellStars;

		StatusStrip statusbar;
		ToolStripStatusLabel statusbar_label_Cords;
		ToolStripStatusLabel statusbar_label_Info;

		Panel panel_ColorFill;


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
			this.it_Undo = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Redo = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_18 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Search = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Searchnext = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_3 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Goto = new System.Windows.Forms.ToolStripMenuItem();
			this.it_GotoLoadchanged = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_4 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyCell = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteCell = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_20 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_13 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CreateRows = new System.Windows.Forms.ToolStripMenuItem();
			this.tb_Goto = new System.Windows.Forms.ToolStripTextBox();
			this.tb_Search = new System.Windows.Forms.ToolStripTextBox();
			this.cb_SearchOption = new System.Windows.Forms.ToolStripComboBox();
			this.it_MenuClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_19 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ViewClipboardContents = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Menu2daOps = new System.Windows.Forms.ToolStripMenuItem();
			this.it_OrderRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CheckRows = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_11 = new System.Windows.Forms.ToolStripSeparator();
			this.it_RecolorRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_AutoCols = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_12 = new System.Windows.Forms.ToolStripSeparator();
			this.it_freeze1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_freeze2 = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_17 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ppOnOff = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ppTopBot = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_21 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ClearUndoRedo = new System.Windows.Forms.ToolStripMenuItem();
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
			this.cellMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.it_cellCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_16 = new System.Windows.Forms.ToolStripSeparator();
			this.it_cellStars = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMenu.SuspendLayout();
			this.menubar.SuspendLayout();
			this.contextEditor.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.cellMenu.SuspendLayout();
			this.SuspendLayout();
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
			this.tabMenu.ShowImageMargin = false;
			this.tabMenu.Size = new System.Drawing.Size(139, 98);
			this.tabMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tabMenu_Opening);
			// 
			// it_tabClose
			// 
			this.it_tabClose.Name = "it_tabClose";
			this.it_tabClose.Size = new System.Drawing.Size(138, 22);
			this.it_tabClose.Text = "Close";
			this.it_tabClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabClose.Click += new System.EventHandler(this.tabclick_Close);
			// 
			// it_tabCloseAll
			// 
			this.it_tabCloseAll.Name = "it_tabCloseAll";
			this.it_tabCloseAll.Size = new System.Drawing.Size(138, 22);
			this.it_tabCloseAll.Text = "Close all";
			this.it_tabCloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAll.Click += new System.EventHandler(this.tabclick_CloseAll);
			// 
			// it_tabCloseAllOthers
			// 
			this.it_tabCloseAllOthers.Name = "it_tabCloseAllOthers";
			this.it_tabCloseAllOthers.Size = new System.Drawing.Size(138, 22);
			this.it_tabCloseAllOthers.Text = "Close all others";
			this.it_tabCloseAllOthers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAllOthers.Click += new System.EventHandler(this.tabclick_CloseAllOtherTabs);
			// 
			// separator_14
			// 
			this.separator_14.Name = "separator_14";
			this.separator_14.Size = new System.Drawing.Size(135, 6);
			// 
			// it_tabReload
			// 
			this.it_tabReload.Name = "it_tabReload";
			this.it_tabReload.Size = new System.Drawing.Size(138, 22);
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
			this.it_MenuClipboard,
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
			this.it_OpenFolder.Size = new System.Drawing.Size(181, 22);
			this.it_OpenFolder.Text = "Open ... @ folder";
			this.it_OpenFolder.Visible = false;
			// 
			// it_Open
			// 
			this.it_Open.Name = "it_Open";
			this.it_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.it_Open.Size = new System.Drawing.Size(181, 22);
			this.it_Open.Text = "Open ...";
			this.it_Open.Click += new System.EventHandler(this.fileclick_Open);
			// 
			// it_Reload
			// 
			this.it_Reload.Enabled = false;
			this.it_Reload.Name = "it_Reload";
			this.it_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.it_Reload.Size = new System.Drawing.Size(181, 22);
			this.it_Reload.Text = "Reload";
			this.it_Reload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// separator_1
			// 
			this.separator_1.Name = "separator_1";
			this.separator_1.Size = new System.Drawing.Size(178, 6);
			// 
			// it_Save
			// 
			this.it_Save.Enabled = false;
			this.it_Save.Name = "it_Save";
			this.it_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.it_Save.Size = new System.Drawing.Size(181, 22);
			this.it_Save.Text = "Save";
			this.it_Save.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// it_SaveAs
			// 
			this.it_SaveAs.Enabled = false;
			this.it_SaveAs.Name = "it_SaveAs";
			this.it_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.it_SaveAs.Size = new System.Drawing.Size(181, 22);
			this.it_SaveAs.Text = "Save As ...";
			this.it_SaveAs.Click += new System.EventHandler(this.fileclick_SaveAs);
			// 
			// it_Close
			// 
			this.it_Close.Enabled = false;
			this.it_Close.Name = "it_Close";
			this.it_Close.Size = new System.Drawing.Size(181, 22);
			this.it_Close.Text = "Close";
			this.it_Close.Click += new System.EventHandler(this.fileclick_CloseTab);
			// 
			// it_CloseAll
			// 
			this.it_CloseAll.Enabled = false;
			this.it_CloseAll.Name = "it_CloseAll";
			this.it_CloseAll.Size = new System.Drawing.Size(181, 22);
			this.it_CloseAll.Text = "Close all";
			this.it_CloseAll.Click += new System.EventHandler(this.fileclick_CloseAllTabs);
			// 
			// separator_2
			// 
			this.separator_2.Name = "separator_2";
			this.separator_2.Size = new System.Drawing.Size(178, 6);
			// 
			// it_Quit
			// 
			this.it_Quit.Name = "it_Quit";
			this.it_Quit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.it_Quit.Size = new System.Drawing.Size(181, 22);
			this.it_Quit.Text = "Quit";
			this.it_Quit.Click += new System.EventHandler(this.fileclick_Quit);
			// 
			// it_MenuEdit
			// 
			this.it_MenuEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Undo,
			this.it_Redo,
			this.separator_18,
			this.it_Search,
			this.it_Searchnext,
			this.separator_3,
			this.it_Goto,
			this.it_GotoLoadchanged,
			this.separator_4,
			this.it_CopyCell,
			this.it_PasteCell,
			this.separator_20,
			this.it_CopyRange,
			this.it_PasteRange,
			this.separator_13,
			this.it_CreateRows});
			this.it_MenuEdit.Name = "it_MenuEdit";
			this.it_MenuEdit.Size = new System.Drawing.Size(40, 20);
			this.it_MenuEdit.Text = "Edit";
			this.it_MenuEdit.DropDownOpening += new System.EventHandler(this.edit_dropdownopening);
			// 
			// it_Undo
			// 
			this.it_Undo.Enabled = false;
			this.it_Undo.Name = "it_Undo";
			this.it_Undo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.it_Undo.Size = new System.Drawing.Size(226, 22);
			this.it_Undo.Text = "Undo";
			this.it_Undo.Click += new System.EventHandler(this.editclick_Undo);
			// 
			// it_Redo
			// 
			this.it_Redo.Enabled = false;
			this.it_Redo.Name = "it_Redo";
			this.it_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.it_Redo.Size = new System.Drawing.Size(226, 22);
			this.it_Redo.Text = "Redo";
			this.it_Redo.Click += new System.EventHandler(this.editclick_Redo);
			// 
			// separator_18
			// 
			this.separator_18.Name = "separator_18";
			this.separator_18.Size = new System.Drawing.Size(223, 6);
			// 
			// it_Search
			// 
			this.it_Search.Name = "it_Search";
			this.it_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.it_Search.Size = new System.Drawing.Size(226, 22);
			this.it_Search.Text = "Find";
			this.it_Search.Click += new System.EventHandler(this.editclick_Search);
			// 
			// it_Searchnext
			// 
			this.it_Searchnext.Enabled = false;
			this.it_Searchnext.Name = "it_Searchnext";
			this.it_Searchnext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.it_Searchnext.Size = new System.Drawing.Size(226, 22);
			this.it_Searchnext.Text = "Find next";
			this.it_Searchnext.Click += new System.EventHandler(this.editclick_SearchNext);
			// 
			// separator_3
			// 
			this.separator_3.Name = "separator_3";
			this.separator_3.Size = new System.Drawing.Size(223, 6);
			// 
			// it_Goto
			// 
			this.it_Goto.Name = "it_Goto";
			this.it_Goto.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.it_Goto.Size = new System.Drawing.Size(226, 22);
			this.it_Goto.Text = "Goto";
			this.it_Goto.Click += new System.EventHandler(this.editclick_Goto);
			// 
			// it_GotoLoadchanged
			// 
			this.it_GotoLoadchanged.Name = "it_GotoLoadchanged";
			this.it_GotoLoadchanged.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.it_GotoLoadchanged.Size = new System.Drawing.Size(226, 22);
			this.it_GotoLoadchanged.Text = "Goto loadchanged";
			this.it_GotoLoadchanged.Click += new System.EventHandler(this.editclick_GotoLoadchanged);
			// 
			// separator_4
			// 
			this.separator_4.Name = "separator_4";
			this.separator_4.Size = new System.Drawing.Size(223, 6);
			// 
			// it_CopyCell
			// 
			this.it_CopyCell.Enabled = false;
			this.it_CopyCell.Name = "it_CopyCell";
			this.it_CopyCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.it_CopyCell.Size = new System.Drawing.Size(226, 22);
			this.it_CopyCell.Text = "copy cell";
			this.it_CopyCell.Click += new System.EventHandler(this.editclick_CopyCell);
			// 
			// it_PasteCell
			// 
			this.it_PasteCell.Enabled = false;
			this.it_PasteCell.Name = "it_PasteCell";
			this.it_PasteCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.it_PasteCell.Size = new System.Drawing.Size(226, 22);
			this.it_PasteCell.Text = "paste cell";
			this.it_PasteCell.Click += new System.EventHandler(this.editclick_PasteCell);
			// 
			// separator_20
			// 
			this.separator_20.Name = "separator_20";
			this.separator_20.Size = new System.Drawing.Size(223, 6);
			// 
			// it_CopyRange
			// 
			this.it_CopyRange.Enabled = false;
			this.it_CopyRange.Name = "it_CopyRange";
			this.it_CopyRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.C)));
			this.it_CopyRange.Size = new System.Drawing.Size(226, 22);
			this.it_CopyRange.Text = "copy row(s)";
			this.it_CopyRange.Click += new System.EventHandler(this.editclick_CopyRange);
			// 
			// it_PasteRange
			// 
			this.it_PasteRange.Enabled = false;
			this.it_PasteRange.Name = "it_PasteRange";
			this.it_PasteRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.V)));
			this.it_PasteRange.Size = new System.Drawing.Size(226, 22);
			this.it_PasteRange.Text = "paste row(s)";
			this.it_PasteRange.Click += new System.EventHandler(this.editclick_PasteRange);
			// 
			// separator_13
			// 
			this.separator_13.Name = "separator_13";
			this.separator_13.Size = new System.Drawing.Size(223, 6);
			// 
			// it_CreateRows
			// 
			this.it_CreateRows.Enabled = false;
			this.it_CreateRows.Name = "it_CreateRows";
			this.it_CreateRows.Size = new System.Drawing.Size(226, 22);
			this.it_CreateRows.Text = "Create row(s) ...";
			this.it_CreateRows.Click += new System.EventHandler(this.editclick_CreateRows);
			// 
			// tb_Goto
			// 
			this.tb_Goto.AutoSize = false;
			this.tb_Goto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Goto.Margin = new System.Windows.Forms.Padding(1, 0, 5, 0);
			this.tb_Goto.Name = "tb_Goto";
			this.tb_Goto.Size = new System.Drawing.Size(36, 18);
			this.tb_Goto.Text = "goto";
			this.tb_Goto.Enter += new System.EventHandler(this.enter_Goto);
			this.tb_Goto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keypress_Goto);
			this.tb_Goto.Click += new System.EventHandler(this.click_Goto);
			this.tb_Goto.TextChanged += new System.EventHandler(this.textchanged_Goto);
			// 
			// tb_Search
			// 
			this.tb_Search.AutoSize = false;
			this.tb_Search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Search.Name = "tb_Search";
			this.tb_Search.Size = new System.Drawing.Size(125, 18);
			this.tb_Search.Text = "search";
			this.tb_Search.Enter += new System.EventHandler(this.enter_Search);
			this.tb_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keypress_Search);
			this.tb_Search.Click += new System.EventHandler(this.click_Search);
			this.tb_Search.TextChanged += new System.EventHandler(this.textchanged_Search);
			// 
			// cb_SearchOption
			// 
			this.cb_SearchOption.AutoSize = false;
			this.cb_SearchOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_SearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cb_SearchOption.Name = "cb_SearchOption";
			this.cb_SearchOption.Size = new System.Drawing.Size(100, 18);
			this.cb_SearchOption.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keypress_Search);
			// 
			// it_MenuClipboard
			// 
			this.it_MenuClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuClipboard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_CopyToClipboard,
			this.it_CopyFromClipboard,
			this.separator_19,
			this.it_ViewClipboardContents});
			this.it_MenuClipboard.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.it_MenuClipboard.Name = "it_MenuClipboard";
			this.it_MenuClipboard.Size = new System.Drawing.Size(74, 20);
			this.it_MenuClipboard.Text = "Clipboard";
			// 
			// it_CopyToClipboard
			// 
			this.it_CopyToClipboard.Enabled = false;
			this.it_CopyToClipboard.Name = "it_CopyToClipboard";
			this.it_CopyToClipboard.Size = new System.Drawing.Size(226, 22);
			this.it_CopyToClipboard.Text = "export row(s) to Clipboard";
			// 
			// it_CopyFromClipboard
			// 
			this.it_CopyFromClipboard.Enabled = false;
			this.it_CopyFromClipboard.Name = "it_CopyFromClipboard";
			this.it_CopyFromClipboard.Size = new System.Drawing.Size(226, 22);
			this.it_CopyFromClipboard.Text = "import Clipboard to row(s)";
			// 
			// separator_19
			// 
			this.separator_19.Name = "separator_19";
			this.separator_19.Size = new System.Drawing.Size(223, 6);
			// 
			// it_ViewClipboardContents
			// 
			this.it_ViewClipboardContents.Name = "it_ViewClipboardContents";
			this.it_ViewClipboardContents.Size = new System.Drawing.Size(226, 22);
			this.it_ViewClipboardContents.Text = "open Clipboard editor";
			// 
			// it_Menu2daOps
			// 
			this.it_Menu2daOps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_Menu2daOps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_OrderRows,
			this.it_CheckRows,
			this.separator_11,
			this.it_RecolorRows,
			this.it_AutoCols,
			this.separator_12,
			this.it_freeze1,
			this.it_freeze2,
			this.separator_17,
			this.it_ppOnOff,
			this.it_ppTopBot,
			this.separator_21,
			this.it_ClearUndoRedo});
			this.it_Menu2daOps.Name = "it_Menu2daOps";
			this.it_Menu2daOps.Size = new System.Drawing.Size(66, 20);
			this.it_Menu2daOps.Text = "2da Ops";
			this.it_Menu2daOps.DropDownOpening += new System.EventHandler(this.ops_dropdownopening);
			// 
			// it_OrderRows
			// 
			this.it_OrderRows.Enabled = false;
			this.it_OrderRows.Name = "it_OrderRows";
			this.it_OrderRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.it_OrderRows.Size = new System.Drawing.Size(241, 22);
			this.it_OrderRows.Text = "order row ids";
			this.it_OrderRows.Click += new System.EventHandler(this.opsclick_Reorder);
			// 
			// it_CheckRows
			// 
			this.it_CheckRows.Enabled = false;
			this.it_CheckRows.Name = "it_CheckRows";
			this.it_CheckRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.it_CheckRows.Size = new System.Drawing.Size(241, 22);
			this.it_CheckRows.Text = "test row order";
			this.it_CheckRows.Click += new System.EventHandler(this.opsclick_CheckRowOrder);
			// 
			// separator_11
			// 
			this.separator_11.Name = "separator_11";
			this.separator_11.Size = new System.Drawing.Size(238, 6);
			// 
			// it_RecolorRows
			// 
			this.it_RecolorRows.Enabled = false;
			this.it_RecolorRows.Name = "it_RecolorRows";
			this.it_RecolorRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.it_RecolorRows.Size = new System.Drawing.Size(241, 22);
			this.it_RecolorRows.Text = "recolor rows";
			this.it_RecolorRows.Click += new System.EventHandler(this.opsclick_Recolor);
			// 
			// it_AutoCols
			// 
			this.it_AutoCols.Enabled = false;
			this.it_AutoCols.Name = "it_AutoCols";
			this.it_AutoCols.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.it_AutoCols.Size = new System.Drawing.Size(241, 22);
			this.it_AutoCols.Text = "autosize cols";
			this.it_AutoCols.Click += new System.EventHandler(this.opsclick_AutosizeCols);
			// 
			// separator_12
			// 
			this.separator_12.Name = "separator_12";
			this.separator_12.Size = new System.Drawing.Size(238, 6);
			// 
			// it_freeze1
			// 
			this.it_freeze1.Enabled = false;
			this.it_freeze1.Name = "it_freeze1";
			this.it_freeze1.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.it_freeze1.Size = new System.Drawing.Size(241, 22);
			this.it_freeze1.Text = "freeze 1st col";
			this.it_freeze1.Click += new System.EventHandler(this.opsclick_Freeze1stCol);
			// 
			// it_freeze2
			// 
			this.it_freeze2.Enabled = false;
			this.it_freeze2.Name = "it_freeze2";
			this.it_freeze2.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.it_freeze2.Size = new System.Drawing.Size(241, 22);
			this.it_freeze2.Text = "freeze 2nd col";
			this.it_freeze2.Click += new System.EventHandler(this.opsclick_Freeze2ndCol);
			// 
			// separator_17
			// 
			this.separator_17.Name = "separator_17";
			this.separator_17.Size = new System.Drawing.Size(238, 6);
			// 
			// it_ppOnOff
			// 
			this.it_ppOnOff.Enabled = false;
			this.it_ppOnOff.Name = "it_ppOnOff";
			this.it_ppOnOff.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.it_ppOnOff.Size = new System.Drawing.Size(241, 22);
			this.it_ppOnOff.Text = "PropertyPanel on/off";
			this.it_ppOnOff.Click += new System.EventHandler(this.opsclick_PropertyPanelOnOff);
			// 
			// it_ppTopBot
			// 
			this.it_ppTopBot.Enabled = false;
			this.it_ppTopBot.Name = "it_ppTopBot";
			this.it_ppTopBot.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.it_ppTopBot.Size = new System.Drawing.Size(241, 22);
			this.it_ppTopBot.Text = "PropertyPanel top/bottom";
			this.it_ppTopBot.Click += new System.EventHandler(this.opsclick_PropertyPanelTopBot);
			// 
			// separator_21
			// 
			this.separator_21.Name = "separator_21";
			this.separator_21.Size = new System.Drawing.Size(238, 6);
			// 
			// it_ClearUndoRedo
			// 
			this.it_ClearUndoRedo.Enabled = false;
			this.it_ClearUndoRedo.Name = "it_ClearUndoRedo";
			this.it_ClearUndoRedo.Size = new System.Drawing.Size(241, 22);
			this.it_ClearUndoRedo.Text = "clear undo/redo";
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
			this.contextEditor.ShowImageMargin = false;
			this.contextEditor.Size = new System.Drawing.Size(177, 248);
			// 
			// context_it_Header
			// 
			this.context_it_Header.Font = new System.Drawing.Font("Verdana", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.context_it_Header.Name = "context_it_Header";
			this.context_it_Header.Size = new System.Drawing.Size(176, 22);
			this.context_it_Header.Text = "_row @ 16";
			this.context_it_Header.Click += new System.EventHandler(this.contextclick_Header);
			// 
			// separator_7
			// 
			this.separator_7.Name = "separator_7";
			this.separator_7.Size = new System.Drawing.Size(173, 6);
			// 
			// context_it_Copy
			// 
			this.context_it_Copy.Name = "context_it_Copy";
			this.context_it_Copy.Size = new System.Drawing.Size(176, 22);
			this.context_it_Copy.Text = "Copy row @ id";
			this.context_it_Copy.Click += new System.EventHandler(this.contextclick_EditCopy);
			// 
			// context_it_Cut
			// 
			this.context_it_Cut.Name = "context_it_Cut";
			this.context_it_Cut.Size = new System.Drawing.Size(176, 22);
			this.context_it_Cut.Text = "Cut row @ id";
			this.context_it_Cut.Click += new System.EventHandler(this.contextclick_EditCut);
			// 
			// separator_8
			// 
			this.separator_8.Name = "separator_8";
			this.separator_8.Size = new System.Drawing.Size(173, 6);
			// 
			// context_it_PasteAbove
			// 
			this.context_it_PasteAbove.Name = "context_it_PasteAbove";
			this.context_it_PasteAbove.Size = new System.Drawing.Size(176, 22);
			this.context_it_PasteAbove.Text = "Paste clip above id";
			this.context_it_PasteAbove.Click += new System.EventHandler(this.contextclick_EditPasteAbove);
			// 
			// context_it_Paste
			// 
			this.context_it_Paste.Name = "context_it_Paste";
			this.context_it_Paste.Size = new System.Drawing.Size(176, 22);
			this.context_it_Paste.Text = "Paste clip @ id";
			this.context_it_Paste.Click += new System.EventHandler(this.contextclick_EditPaste);
			// 
			// context_it_PasteBelow
			// 
			this.context_it_PasteBelow.Name = "context_it_PasteBelow";
			this.context_it_PasteBelow.Size = new System.Drawing.Size(176, 22);
			this.context_it_PasteBelow.Text = "Paste clip below id";
			this.context_it_PasteBelow.Click += new System.EventHandler(this.contextclick_EditPasteBelow);
			// 
			// separator_9
			// 
			this.separator_9.Name = "separator_9";
			this.separator_9.Size = new System.Drawing.Size(173, 6);
			// 
			// context_it_CreateAbove
			// 
			this.context_it_CreateAbove.Name = "context_it_CreateAbove";
			this.context_it_CreateAbove.Size = new System.Drawing.Size(176, 22);
			this.context_it_CreateAbove.Text = "Create blank above id";
			this.context_it_CreateAbove.Click += new System.EventHandler(this.contextclick_EditCreateAbove);
			// 
			// context_it_ClearRow
			// 
			this.context_it_ClearRow.Name = "context_it_ClearRow";
			this.context_it_ClearRow.Size = new System.Drawing.Size(176, 22);
			this.context_it_ClearRow.Text = "Clear fields @ id";
			this.context_it_ClearRow.Click += new System.EventHandler(this.contextclick_EditClear);
			// 
			// context_it_CreateBelow
			// 
			this.context_it_CreateBelow.Name = "context_it_CreateBelow";
			this.context_it_CreateBelow.Size = new System.Drawing.Size(176, 22);
			this.context_it_CreateBelow.Text = "Create blank below id";
			this.context_it_CreateBelow.Click += new System.EventHandler(this.contextclick_EditCreateBelow);
			// 
			// separator_10
			// 
			this.separator_10.Name = "separator_10";
			this.separator_10.Size = new System.Drawing.Size(173, 6);
			// 
			// context_it_DeleteRow
			// 
			this.context_it_DeleteRow.Name = "context_it_DeleteRow";
			this.context_it_DeleteRow.Size = new System.Drawing.Size(176, 22);
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
			this.statusbar.TabIndex = 1;
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
			this.statusbar_label_Info.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statusbar_label_Info.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusbar_label_Info.Name = "statusbar_label_Info";
			this.statusbar_label_Info.Size = new System.Drawing.Size(640, 17);
			this.statusbar_label_Info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel_ColorFill
			// 
			this.panel_ColorFill.BackColor = System.Drawing.Color.LightSeaGreen;
			this.panel_ColorFill.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_ColorFill.Location = new System.Drawing.Point(0, 24);
			this.panel_ColorFill.Name = "panel_ColorFill";
			this.panel_ColorFill.Size = new System.Drawing.Size(842, 408);
			this.panel_ColorFill.TabIndex = 2;
			// 
			// cellMenu
			// 
			this.cellMenu.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cellMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_cellCopy,
			this.it_cellPaste,
			this.separator_16,
			this.it_cellStars});
			this.cellMenu.Name = "cellMenu";
			this.cellMenu.ShowImageMargin = false;
			this.cellMenu.Size = new System.Drawing.Size(104, 76);
			// 
			// it_cellCopy
			// 
			this.it_cellCopy.Name = "it_cellCopy";
			this.it_cellCopy.Size = new System.Drawing.Size(103, 22);
			this.it_cellCopy.Text = "copy cell";
			this.it_cellCopy.Click += new System.EventHandler(this.editclick_CopyCell);
			// 
			// it_cellPaste
			// 
			this.it_cellPaste.Name = "it_cellPaste";
			this.it_cellPaste.Size = new System.Drawing.Size(103, 22);
			this.it_cellPaste.Text = "paste cell";
			this.it_cellPaste.Click += new System.EventHandler(this.editclick_PasteCell);
			// 
			// separator_16
			// 
			this.separator_16.Name = "separator_16";
			this.separator_16.Size = new System.Drawing.Size(100, 6);
			// 
			// it_cellStars
			// 
			this.it_cellStars.Name = "it_cellStars";
			this.it_cellStars.Size = new System.Drawing.Size(103, 22);
			this.it_cellStars.Text = "****";
			this.it_cellStars.Click += new System.EventHandler(this.cellclick_Stars);
			// 
			// YataForm
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(842, 454);
			this.Controls.Add(this.panel_ColorFill);
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
			this.cellMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
