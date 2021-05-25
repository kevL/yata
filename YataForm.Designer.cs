using System;
using System.ComponentModel;
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
		IContainer components;

		MenuStrip menubar;

		ToolStripMenuItem it_MenuFile;
		ToolStripMenuItem it_OpenFolder;
		ToolStripMenuItem it_Open;
		ToolStripMenuItem it_Reload;
		ToolStripMenuItem it_Recent;
		ToolStripMenuItem it_Readonly;
		ToolStripMenuItem it_Save;
		ToolStripMenuItem it_SaveAs;
		ToolStripMenuItem it_SaveAll;
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
		ToolStripMenuItem it_Stars;
		ToolStripMenuItem it_Lower;
		ToolStripMenuItem it_Upper;
		ToolStripMenuItem it_Paste;
		ToolStripMenuItem it_CopyRange;
		ToolStripMenuItem it_PasteRange;
		ToolStripMenuItem it_CreateRows;

		ToolStripMenuItem it_MenuEditcol;
		ToolStripMenuItem it_CreateHead;
		ToolStripMenuItem it_DeleteHead;
		ToolStripMenuItem it_CopyCol;
		ToolStripMenuItem it_PasteCol;

		internal ToolStripTextBox tb_Goto;

		ToolStripTextBox tb_Search;
		ToolStripComboBox cb_SearchOption;

		ToolStripMenuItem it_MenuClipboard;
		ToolStripMenuItem it_ClipExport;
		ToolStripMenuItem it_ClipImport;
		ToolStripMenuItem it_OpenClipEditor;

		ToolStripMenuItem it_Menu2daOps;
		ToolStripMenuItem it_OrderRows;
		ToolStripMenuItem it_CheckRows;
		ToolStripMenuItem it_ColorRows;
		ToolStripMenuItem it_AutoCols;
		ToolStripMenuItem it_freeze1;
		ToolStripMenuItem it_freeze2;
		ToolStripMenuItem it_ppOnOff;
		ToolStripMenuItem it_ppLocation;
		ToolStripMenuItem it_ExternDiff;
		ToolStripMenuItem it_ClearUr;

		ToolStripMenuItem it_MenuTalkTable;
		internal ToolStripMenuItem it_PathTalkD;
		internal ToolStripMenuItem it_PathTalkC;

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
		ToolStripMenuItem it_PathMasterFeats2da;
		ToolStripMenuItem it_PathCombatModes2da;

		ToolStripMenuItem it_MenuFont;
		ToolStripMenuItem it_Font;
		ToolStripMenuItem it_FontDefault;

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
		ToolStripSeparator separator_22;
		ToolStripSeparator separator_23;
		ToolStripSeparator separator_24;
		ToolStripSeparator separator_25;
		ToolStripSeparator separator_26;
		ToolStripSeparator separator_27;
		ToolStripSeparator separator_28;
		ToolStripSeparator separator_29;
		ToolStripSeparator separator_30;
		ToolStripSeparator separator_31;
		ToolStripSeparator separator_32;

		internal ContextMenuStrip ContextEditor;
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

		internal ContextMenuStrip tabMenu;
		ToolStripMenuItem it_tabClose;
		ToolStripMenuItem it_tabCloseAll;
		ToolStripMenuItem it_tabCloseAllOthers;
		ToolStripMenuItem it_tabSave;
		ToolStripMenuItem it_tabReload;
		ToolStripMenuItem it_tabDiff1;
		ToolStripMenuItem it_tabDiff2;
		ToolStripMenuItem it_tabDiffReset;
		ToolStripMenuItem it_tabDiffSync;

		internal ContextMenuStrip cellMenu;
		ToolStripMenuItem it_cellEdit;
		ToolStripMenuItem it_cellCopy;
		ToolStripMenuItem it_cellPaste;
		ToolStripMenuItem it_cellStars;
		ToolStripMenuItem it_cellLower;
		ToolStripMenuItem it_cellUpper;
		ToolStripMenuItem it_cellMergeCe;
		ToolStripMenuItem it_cellMergeRo;
		ToolStripMenuItem it_cellInput;
		ToolStripMenuItem it_cellStrref;
		ToolStripMenuItem it_cellStrref_talktable;
		ToolStripMenuItem it_cellStrref_custom;
		ToolStripMenuItem it_cellStrref_invalid;

		StatusStrip statusbar;
		ToolStripStatusLabel statbar_lblCords;
		ToolStripStatusLabel statbar_lblInfo;

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
			this.it_tabSave = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_26 = new System.Windows.Forms.ToolStripSeparator();
			this.it_tabReload = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_23 = new System.Windows.Forms.ToolStripSeparator();
			this.it_tabDiff1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabDiff2 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabDiffReset = new System.Windows.Forms.ToolStripMenuItem();
			this.it_tabDiffSync = new System.Windows.Forms.ToolStripMenuItem();
			this.menubar = new System.Windows.Forms.MenuStrip();
			this.it_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.it_OpenFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Reload = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Recent = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_29 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Readonly = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_1 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Save = new System.Windows.Forms.ToolStripMenuItem();
			this.it_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.it_SaveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_22 = new System.Windows.Forms.ToolStripSeparator();
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
			this.it_Stars = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Lower = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Upper = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_30 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_13 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CreateRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuEditcol = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CreateHead = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeleteHead = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_32 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyCol = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteCol = new System.Windows.Forms.ToolStripMenuItem();
			this.tb_Goto = new System.Windows.Forms.ToolStripTextBox();
			this.tb_Search = new System.Windows.Forms.ToolStripTextBox();
			this.cb_SearchOption = new System.Windows.Forms.ToolStripComboBox();
			this.it_MenuClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ClipExport = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ClipImport = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_19 = new System.Windows.Forms.ToolStripSeparator();
			this.it_OpenClipEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Menu2daOps = new System.Windows.Forms.ToolStripMenuItem();
			this.it_OrderRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CheckRows = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_11 = new System.Windows.Forms.ToolStripSeparator();
			this.it_AutoCols = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ColorRows = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_12 = new System.Windows.Forms.ToolStripSeparator();
			this.it_freeze1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_freeze2 = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_17 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ppOnOff = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ppLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_25 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ExternDiff = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_21 = new System.Windows.Forms.ToolStripSeparator();
			this.it_ClearUr = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuTalkTable = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathTalkD = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathTalkC = new System.Windows.Forms.ToolStripMenuItem();
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
			this.separator_31 = new System.Windows.Forms.ToolStripSeparator();
			this.it_PathMasterFeats2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PathCombatModes2da = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuFont = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Font = new System.Windows.Forms.ToolStripMenuItem();
			this.it_FontDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.it_ReadMe = new System.Windows.Forms.ToolStripMenuItem();
			this.it_About = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
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
			this.statbar_lblCords = new System.Windows.Forms.ToolStripStatusLabel();
			this.statbar_lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel_ColorFill = new System.Windows.Forms.Panel();
			this.cellMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.it_cellEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_27 = new System.Windows.Forms.ToolStripSeparator();
			this.it_cellCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_16 = new System.Windows.Forms.ToolStripSeparator();
			this.it_cellStars = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellLower = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellUpper = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_24 = new System.Windows.Forms.ToolStripSeparator();
			this.it_cellMergeCe = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellMergeRo = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_28 = new System.Windows.Forms.ToolStripSeparator();
			this.it_cellStrref = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellStrref_talktable = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellStrref_custom = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellStrref_invalid = new System.Windows.Forms.ToolStripMenuItem();
			this.it_cellInput = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMenu.SuspendLayout();
			this.menubar.SuspendLayout();
			this.ContextEditor.SuspendLayout();
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
			this.it_tabSave,
			this.separator_26,
			this.it_tabReload,
			this.separator_23,
			this.it_tabDiff1,
			this.it_tabDiff2,
			this.it_tabDiffReset,
			this.it_tabDiffSync});
			this.tabMenu.Name = "tabMenu";
			this.tabMenu.ShowImageMargin = false;
			this.tabMenu.Size = new System.Drawing.Size(139, 220);
			this.tabMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tabMenu_Opening);
			// 
			// it_tabClose
			// 
			this.it_tabClose.Name = "it_tabClose";
			this.it_tabClose.Size = new System.Drawing.Size(138, 22);
			this.it_tabClose.Text = "Close";
			this.it_tabClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabClose.Click += new System.EventHandler(this.fileclick_CloseTab);
			// 
			// it_tabCloseAll
			// 
			this.it_tabCloseAll.Name = "it_tabCloseAll";
			this.it_tabCloseAll.Size = new System.Drawing.Size(138, 22);
			this.it_tabCloseAll.Text = "Close all";
			this.it_tabCloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.it_tabCloseAll.Click += new System.EventHandler(this.fileclick_CloseAllTabs);
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
			// it_tabSave
			// 
			this.it_tabSave.Name = "it_tabSave";
			this.it_tabSave.Size = new System.Drawing.Size(138, 22);
			this.it_tabSave.Text = "Save";
			this.it_tabSave.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// separator_26
			// 
			this.separator_26.Name = "separator_26";
			this.separator_26.Size = new System.Drawing.Size(135, 6);
			// 
			// it_tabReload
			// 
			this.it_tabReload.Name = "it_tabReload";
			this.it_tabReload.Size = new System.Drawing.Size(138, 22);
			this.it_tabReload.Text = "Reload";
			this.it_tabReload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// separator_23
			// 
			this.separator_23.Name = "separator_23";
			this.separator_23.Size = new System.Drawing.Size(135, 6);
			// 
			// it_tabDiff1
			// 
			this.it_tabDiff1.Name = "it_tabDiff1";
			this.it_tabDiff1.Size = new System.Drawing.Size(138, 22);
			this.it_tabDiff1.Text = "Select diff1";
			this.it_tabDiff1.Click += new System.EventHandler(this.tabclick_Diff1);
			// 
			// it_tabDiff2
			// 
			this.it_tabDiff2.Enabled = false;
			this.it_tabDiff2.Name = "it_tabDiff2";
			this.it_tabDiff2.Size = new System.Drawing.Size(138, 22);
			this.it_tabDiff2.Text = "Select diff2";
			this.it_tabDiff2.Click += new System.EventHandler(this.tabclick_Diff2);
			// 
			// it_tabDiffReset
			// 
			this.it_tabDiffReset.Enabled = false;
			this.it_tabDiffReset.Name = "it_tabDiffReset";
			this.it_tabDiffReset.Size = new System.Drawing.Size(138, 22);
			this.it_tabDiffReset.Text = "Reset diffs";
			this.it_tabDiffReset.Click += new System.EventHandler(this.tabclick_DiffReset);
			// 
			// it_tabDiffSync
			// 
			this.it_tabDiffSync.Enabled = false;
			this.it_tabDiffSync.Name = "it_tabDiffSync";
			this.it_tabDiffSync.Size = new System.Drawing.Size(138, 22);
			this.it_tabDiffSync.Text = "Sync tables";
			this.it_tabDiffSync.Click += new System.EventHandler(this.tabclick_DiffSync);
			// 
			// menubar
			// 
			this.menubar.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_MenuFile,
			this.it_MenuEdit,
			this.it_MenuEditcol,
			this.tb_Goto,
			this.tb_Search,
			this.cb_SearchOption,
			this.it_MenuClipboard,
			this.it_Menu2daOps,
			this.it_MenuTalkTable,
			this.it_MenuPaths,
			this.it_MenuFont,
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
			this.it_Recent,
			this.separator_29,
			this.it_Readonly,
			this.separator_1,
			this.it_Save,
			this.it_SaveAs,
			this.it_SaveAll,
			this.separator_22,
			this.it_Close,
			this.it_CloseAll,
			this.separator_2,
			this.it_Quit});
			this.it_MenuFile.Name = "it_MenuFile";
			this.it_MenuFile.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuFile.Size = new System.Drawing.Size(34, 20);
			this.it_MenuFile.Text = "&File";
			this.it_MenuFile.DropDownOpening += new System.EventHandler(this.file_dropdownopening);
			// 
			// it_OpenFolder
			// 
			this.it_OpenFolder.Name = "it_OpenFolder";
			this.it_OpenFolder.Size = new System.Drawing.Size(181, 22);
			this.it_OpenFolder.Text = "Ope&n ... @ folder";
			this.it_OpenFolder.Visible = false;
			// 
			// it_Open
			// 
			this.it_Open.Name = "it_Open";
			this.it_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.it_Open.Size = new System.Drawing.Size(181, 22);
			this.it_Open.Text = "&Open ...";
			this.it_Open.Click += new System.EventHandler(this.fileclick_Open);
			// 
			// it_Reload
			// 
			this.it_Reload.Enabled = false;
			this.it_Reload.Name = "it_Reload";
			this.it_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.it_Reload.Size = new System.Drawing.Size(181, 22);
			this.it_Reload.Text = "&Reload";
			this.it_Reload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// it_Recent
			// 
			this.it_Recent.Name = "it_Recent";
			this.it_Recent.Size = new System.Drawing.Size(181, 22);
			this.it_Recent.Text = "Recen&t";
			// 
			// separator_29
			// 
			this.separator_29.Name = "separator_29";
			this.separator_29.Size = new System.Drawing.Size(178, 6);
			// 
			// it_Readonly
			// 
			this.it_Readonly.CheckOnClick = true;
			this.it_Readonly.Enabled = false;
			this.it_Readonly.Name = "it_Readonly";
			this.it_Readonly.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.it_Readonly.Size = new System.Drawing.Size(181, 22);
			this.it_Readonly.Text = "Rea&donly";
			this.it_Readonly.Click += new System.EventHandler(this.fileclick_Readonly);
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
			this.it_Save.Text = "&Save";
			this.it_Save.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// it_SaveAs
			// 
			this.it_SaveAs.Enabled = false;
			this.it_SaveAs.Name = "it_SaveAs";
			this.it_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.it_SaveAs.Size = new System.Drawing.Size(181, 22);
			this.it_SaveAs.Text = "Sav&e As ...";
			this.it_SaveAs.Click += new System.EventHandler(this.fileclick_SaveAs);
			// 
			// it_SaveAll
			// 
			this.it_SaveAll.Enabled = false;
			this.it_SaveAll.Name = "it_SaveAll";
			this.it_SaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.it_SaveAll.Size = new System.Drawing.Size(181, 22);
			this.it_SaveAll.Text = "Save &All";
			this.it_SaveAll.Click += new System.EventHandler(this.fileclick_SaveAll);
			// 
			// separator_22
			// 
			this.separator_22.Name = "separator_22";
			this.separator_22.Size = new System.Drawing.Size(178, 6);
			// 
			// it_Close
			// 
			this.it_Close.Enabled = false;
			this.it_Close.Name = "it_Close";
			this.it_Close.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.it_Close.Size = new System.Drawing.Size(181, 22);
			this.it_Close.Text = "&Close";
			this.it_Close.Click += new System.EventHandler(this.fileclick_CloseTab);
			// 
			// it_CloseAll
			// 
			this.it_CloseAll.Enabled = false;
			this.it_CloseAll.Name = "it_CloseAll";
			this.it_CloseAll.Size = new System.Drawing.Size(181, 22);
			this.it_CloseAll.Text = "Close &all";
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
			this.it_Quit.Text = "&Quit";
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
			this.it_Stars,
			this.it_Lower,
			this.it_Upper,
			this.it_Paste,
			this.separator_30,
			this.it_CopyRange,
			this.it_PasteRange,
			this.separator_13,
			this.it_CreateRows});
			this.it_MenuEdit.Name = "it_MenuEdit";
			this.it_MenuEdit.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuEdit.Size = new System.Drawing.Size(36, 20);
			this.it_MenuEdit.Text = "&Edit";
			this.it_MenuEdit.DropDownOpening += new System.EventHandler(this.edit_dropdownopening);
			// 
			// it_Undo
			// 
			this.it_Undo.Enabled = false;
			this.it_Undo.Name = "it_Undo";
			this.it_Undo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.it_Undo.Size = new System.Drawing.Size(226, 22);
			this.it_Undo.Text = "&Undo";
			this.it_Undo.Click += new System.EventHandler(this.editclick_Undo);
			// 
			// it_Redo
			// 
			this.it_Redo.Enabled = false;
			this.it_Redo.Name = "it_Redo";
			this.it_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.it_Redo.Size = new System.Drawing.Size(226, 22);
			this.it_Redo.Text = "&Redo";
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
			this.it_Search.Text = "&Find";
			this.it_Search.Click += new System.EventHandler(this.editclick_Search);
			// 
			// it_Searchnext
			// 
			this.it_Searchnext.Name = "it_Searchnext";
			this.it_Searchnext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.it_Searchnext.Size = new System.Drawing.Size(226, 22);
			this.it_Searchnext.Text = "Find &next";
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
			this.it_Goto.Text = "&Goto";
			this.it_Goto.Click += new System.EventHandler(this.editclick_Goto);
			// 
			// it_GotoLoadchanged
			// 
			this.it_GotoLoadchanged.Name = "it_GotoLoadchanged";
			this.it_GotoLoadchanged.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.it_GotoLoadchanged.Size = new System.Drawing.Size(226, 22);
			this.it_GotoLoadchanged.Text = "Goto loadcha&nged";
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
			this.it_CopyCell.Text = "&Copy cell";
			this.it_CopyCell.Click += new System.EventHandler(this.editclick_CopyCell);
			// 
			// it_PasteCell
			// 
			this.it_PasteCell.Enabled = false;
			this.it_PasteCell.Name = "it_PasteCell";
			this.it_PasteCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.it_PasteCell.Size = new System.Drawing.Size(226, 22);
			this.it_PasteCell.Text = "&Paste cell";
			this.it_PasteCell.Click += new System.EventHandler(this.editclick_PasteCell);
			// 
			// separator_20
			// 
			this.separator_20.Name = "separator_20";
			this.separator_20.Size = new System.Drawing.Size(223, 6);
			// 
			// it_Stars
			// 
			this.it_Stars.Enabled = false;
			this.it_Stars.Name = "it_Stars";
			this.it_Stars.ShowShortcutKeys = false;
			this.it_Stars.Size = new System.Drawing.Size(226, 22);
			this.it_Stars.Text = "****";
			this.it_Stars.Click += new System.EventHandler(this.editclick_Stars);
			// 
			// it_Lower
			// 
			this.it_Lower.Enabled = false;
			this.it_Lower.Name = "it_Lower";
			this.it_Lower.ShowShortcutKeys = false;
			this.it_Lower.Size = new System.Drawing.Size(226, 22);
			this.it_Lower.Text = "Lowercase";
			this.it_Lower.Click += new System.EventHandler(this.editclick_Lower);
			// 
			// it_Upper
			// 
			this.it_Upper.Enabled = false;
			this.it_Upper.Name = "it_Upper";
			this.it_Upper.ShowShortcutKeys = false;
			this.it_Upper.Size = new System.Drawing.Size(226, 22);
			this.it_Upper.Text = "Uppercase";
			this.it_Upper.Click += new System.EventHandler(this.editclick_Upper);
			// 
			// it_Paste
			// 
			this.it_Paste.Enabled = false;
			this.it_Paste.Name = "it_Paste";
			this.it_Paste.ShowShortcutKeys = false;
			this.it_Paste.Size = new System.Drawing.Size(226, 22);
			this.it_Paste.Text = "Apply text to selected ...";
			this.it_Paste.Click += new System.EventHandler(this.editclick_Text);
			// 
			// separator_30
			// 
			this.separator_30.Name = "separator_30";
			this.separator_30.Size = new System.Drawing.Size(223, 6);
			// 
			// it_CopyRange
			// 
			this.it_CopyRange.Enabled = false;
			this.it_CopyRange.Name = "it_CopyRange";
			this.it_CopyRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.C)));
			this.it_CopyRange.Size = new System.Drawing.Size(226, 22);
			this.it_CopyRange.Text = "C&opy row(s)";
			this.it_CopyRange.Click += new System.EventHandler(this.editclick_CopyRange);
			// 
			// it_PasteRange
			// 
			this.it_PasteRange.Enabled = false;
			this.it_PasteRange.Name = "it_PasteRange";
			this.it_PasteRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.V)));
			this.it_PasteRange.Size = new System.Drawing.Size(226, 22);
			this.it_PasteRange.Text = "P&aste row(s)";
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
			this.it_CreateRows.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.it_CreateRows.Size = new System.Drawing.Size(226, 22);
			this.it_CreateRows.Text = "Crea&te row(s) ...";
			this.it_CreateRows.Click += new System.EventHandler(this.editclick_CreateRows);
			// 
			// it_MenuEditcol
			// 
			this.it_MenuEditcol.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_CreateHead,
			this.it_DeleteHead,
			this.separator_32,
			this.it_CopyCol,
			this.it_PasteCol});
			this.it_MenuEditcol.Name = "it_MenuEditcol";
			this.it_MenuEditcol.Size = new System.Drawing.Size(56, 20);
			this.it_MenuEditcol.Text = "Editcol";
			this.it_MenuEditcol.DropDownOpening += new System.EventHandler(this.editcol_dropdownopening);
			// 
			// it_CreateHead
			// 
			this.it_CreateHead.Name = "it_CreateHead";
			this.it_CreateHead.Size = new System.Drawing.Size(158, 22);
			this.it_CreateHead.Text = "create head ...";
			this.it_CreateHead.Click += new System.EventHandler(this.editcolclick_CreateHead);
			// 
			// it_DeleteHead
			// 
			this.it_DeleteHead.Name = "it_DeleteHead";
			this.it_DeleteHead.Size = new System.Drawing.Size(158, 22);
			this.it_DeleteHead.Text = "delete head ...";
			this.it_DeleteHead.Click += new System.EventHandler(this.editcolclick_DeleteHead);
			// 
			// separator_32
			// 
			this.separator_32.Name = "separator_32";
			this.separator_32.Size = new System.Drawing.Size(155, 6);
			// 
			// it_CopyCol
			// 
			this.it_CopyCol.Name = "it_CopyCol";
			this.it_CopyCol.Size = new System.Drawing.Size(158, 22);
			this.it_CopyCol.Text = "copy col";
			// 
			// it_PasteCol
			// 
			this.it_PasteCol.Name = "it_PasteCol";
			this.it_PasteCol.Size = new System.Drawing.Size(158, 22);
			this.it_PasteCol.Text = "paste col";
			// 
			// tb_Goto
			// 
			this.tb_Goto.AutoSize = false;
			this.tb_Goto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Goto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.tb_Goto.Name = "tb_Goto";
			this.tb_Goto.Size = new System.Drawing.Size(36, 18);
			this.tb_Goto.Text = "goto";
			this.tb_Goto.Enter += new System.EventHandler(this.enter_Goto);
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
			this.tb_Search.Click += new System.EventHandler(this.click_Search);
			this.tb_Search.TextChanged += new System.EventHandler(this.textchanged_Search);
			// 
			// cb_SearchOption
			// 
			this.cb_SearchOption.AutoSize = false;
			this.cb_SearchOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_SearchOption.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cb_SearchOption.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
			this.cb_SearchOption.Name = "cb_SearchOption";
			this.cb_SearchOption.Size = new System.Drawing.Size(100, 18);
			// 
			// it_MenuClipboard
			// 
			this.it_MenuClipboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuClipboard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_ClipExport,
			this.it_ClipImport,
			this.separator_19,
			this.it_OpenClipEditor});
			this.it_MenuClipboard.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this.it_MenuClipboard.Name = "it_MenuClipboard";
			this.it_MenuClipboard.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuClipboard.Size = new System.Drawing.Size(70, 20);
			this.it_MenuClipboard.Text = "&Clipboard";
			this.it_MenuClipboard.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_ClipExport
			// 
			this.it_ClipExport.Name = "it_ClipExport";
			this.it_ClipExport.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.it_ClipExport.Size = new System.Drawing.Size(222, 22);
			this.it_ClipExport.Text = "&Export copied row(s)";
			this.it_ClipExport.Click += new System.EventHandler(this.clipclick_ExportCopy);
			// 
			// it_ClipImport
			// 
			this.it_ClipImport.Name = "it_ClipImport";
			this.it_ClipImport.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.it_ClipImport.Size = new System.Drawing.Size(222, 22);
			this.it_ClipImport.Text = "&Import copied row(s)";
			this.it_ClipImport.Click += new System.EventHandler(this.clipclick_ImportCopy);
			// 
			// separator_19
			// 
			this.separator_19.Name = "separator_19";
			this.separator_19.Size = new System.Drawing.Size(219, 6);
			// 
			// it_OpenClipEditor
			// 
			this.it_OpenClipEditor.Name = "it_OpenClipEditor";
			this.it_OpenClipEditor.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.it_OpenClipEditor.Size = new System.Drawing.Size(222, 22);
			this.it_OpenClipEditor.Text = "&Open clip editor";
			this.it_OpenClipEditor.Click += new System.EventHandler(this.clipclick_ViewClipboard);
			// 
			// it_Menu2daOps
			// 
			this.it_Menu2daOps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_Menu2daOps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_OrderRows,
			this.it_CheckRows,
			this.separator_11,
			this.it_AutoCols,
			this.it_ColorRows,
			this.separator_12,
			this.it_freeze1,
			this.it_freeze2,
			this.separator_17,
			this.it_ppOnOff,
			this.it_ppLocation,
			this.separator_25,
			this.it_ExternDiff,
			this.separator_21,
			this.it_ClearUr});
			this.it_Menu2daOps.Name = "it_Menu2daOps";
			this.it_Menu2daOps.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_Menu2daOps.Size = new System.Drawing.Size(62, 20);
			this.it_Menu2daOps.Text = "2&da Ops";
			this.it_Menu2daOps.DropDownOpening += new System.EventHandler(this.ops_dropdownopening);
			// 
			// it_OrderRows
			// 
			this.it_OrderRows.Enabled = false;
			this.it_OrderRows.Name = "it_OrderRows";
			this.it_OrderRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.it_OrderRows.Size = new System.Drawing.Size(236, 22);
			this.it_OrderRows.Text = "Or&der row ids";
			this.it_OrderRows.Click += new System.EventHandler(this.opsclick_Order);
			// 
			// it_CheckRows
			// 
			this.it_CheckRows.Enabled = false;
			this.it_CheckRows.Name = "it_CheckRows";
			this.it_CheckRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.it_CheckRows.Size = new System.Drawing.Size(236, 22);
			this.it_CheckRows.Text = "&Test row order";
			this.it_CheckRows.Click += new System.EventHandler(this.opsclick_TestOrder);
			// 
			// separator_11
			// 
			this.separator_11.Name = "separator_11";
			this.separator_11.Size = new System.Drawing.Size(233, 6);
			// 
			// it_AutoCols
			// 
			this.it_AutoCols.Enabled = false;
			this.it_AutoCols.Name = "it_AutoCols";
			this.it_AutoCols.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.it_AutoCols.Size = new System.Drawing.Size(236, 22);
			this.it_AutoCols.Text = "Autos&ize cols";
			this.it_AutoCols.Click += new System.EventHandler(this.opsclick_AutosizeCols);
			// 
			// it_ColorRows
			// 
			this.it_ColorRows.Enabled = false;
			this.it_ColorRows.Name = "it_ColorRows";
			this.it_ColorRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.it_ColorRows.Size = new System.Drawing.Size(236, 22);
			this.it_ColorRows.Text = "Reco&lor rows";
			this.it_ColorRows.Click += new System.EventHandler(this.opsclick_Recolor);
			// 
			// separator_12
			// 
			this.separator_12.Name = "separator_12";
			this.separator_12.Size = new System.Drawing.Size(233, 6);
			// 
			// it_freeze1
			// 
			this.it_freeze1.Enabled = false;
			this.it_freeze1.Name = "it_freeze1";
			this.it_freeze1.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.it_freeze1.Size = new System.Drawing.Size(236, 22);
			this.it_freeze1.Text = "Freeze &1st col";
			this.it_freeze1.Click += new System.EventHandler(this.opsclick_Freeze1stCol);
			// 
			// it_freeze2
			// 
			this.it_freeze2.Enabled = false;
			this.it_freeze2.Name = "it_freeze2";
			this.it_freeze2.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.it_freeze2.Size = new System.Drawing.Size(236, 22);
			this.it_freeze2.Text = "Freeze &2nd col";
			this.it_freeze2.Click += new System.EventHandler(this.opsclick_Freeze2ndCol);
			// 
			// separator_17
			// 
			this.separator_17.Name = "separator_17";
			this.separator_17.Size = new System.Drawing.Size(233, 6);
			// 
			// it_ppOnOff
			// 
			this.it_ppOnOff.Enabled = false;
			this.it_ppOnOff.Name = "it_ppOnOff";
			this.it_ppOnOff.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.it_ppOnOff.Size = new System.Drawing.Size(236, 22);
			this.it_ppOnOff.Text = "&PropertyPanel";
			this.it_ppOnOff.Click += new System.EventHandler(this.opsclick_PropertyPanelOnOff);
			// 
			// it_ppLocation
			// 
			this.it_ppLocation.Enabled = false;
			this.it_ppLocation.Name = "it_ppLocation";
			this.it_ppLocation.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.it_ppLocation.Size = new System.Drawing.Size(236, 22);
			this.it_ppLocation.Text = "PropertyPanel l&ocation";
			this.it_ppLocation.Click += new System.EventHandler(this.opsclick_PropertyPanelLocation);
			// 
			// separator_25
			// 
			this.separator_25.Name = "separator_25";
			this.separator_25.Size = new System.Drawing.Size(233, 6);
			// 
			// it_ExternDiff
			// 
			this.it_ExternDiff.Enabled = false;
			this.it_ExternDiff.Name = "it_ExternDiff";
			this.it_ExternDiff.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
			this.it_ExternDiff.Size = new System.Drawing.Size(236, 22);
			this.it_ExternDiff.Text = "External diff/&merger";
			this.it_ExternDiff.Click += new System.EventHandler(this.opsclick_ExternalDiff);
			// 
			// separator_21
			// 
			this.separator_21.Name = "separator_21";
			this.separator_21.Size = new System.Drawing.Size(233, 6);
			// 
			// it_ClearUr
			// 
			this.it_ClearUr.Enabled = false;
			this.it_ClearUr.Name = "it_ClearUr";
			this.it_ClearUr.Size = new System.Drawing.Size(236, 22);
			this.it_ClearUr.Text = "&Clear undo/redo";
			this.it_ClearUr.Click += new System.EventHandler(this.opsclick_ClearUr);
			// 
			// it_MenuTalkTable
			// 
			this.it_MenuTalkTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuTalkTable.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_PathTalkD,
			this.it_PathTalkC});
			this.it_MenuTalkTable.Name = "it_MenuTalkTable";
			this.it_MenuTalkTable.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuTalkTable.Size = new System.Drawing.Size(70, 20);
			this.it_MenuTalkTable.Text = "&TalkTable";
			this.it_MenuTalkTable.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_PathTalkD
			// 
			this.it_PathTalkD.Name = "it_PathTalkD";
			this.it_PathTalkD.Size = new System.Drawing.Size(175, 22);
			this.it_PathTalkD.Text = "&Path to ...";
			this.it_PathTalkD.Click += new System.EventHandler(this.itclick_PathTalkD);
			// 
			// it_PathTalkC
			// 
			this.it_PathTalkC.Name = "it_PathTalkC";
			this.it_PathTalkC.Size = new System.Drawing.Size(175, 22);
			this.it_PathTalkC.Text = "&Path to custom ...";
			this.it_PathTalkC.Click += new System.EventHandler(this.itclick_PathTalkC);
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
			this.it_PathSpellTarget2da,
			this.separator_31,
			this.it_PathMasterFeats2da,
			this.it_PathCombatModes2da});
			this.it_MenuPaths.Name = "it_MenuPaths";
			this.it_MenuPaths.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuPaths.Size = new System.Drawing.Size(46, 20);
			this.it_MenuPaths.Text = "&Paths";
			this.it_MenuPaths.Visible = false;
			this.it_MenuPaths.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_PathAll
			// 
			this.it_PathAll.Name = "it_PathAll";
			this.it_PathAll.Size = new System.Drawing.Size(222, 22);
			this.it_PathAll.Text = "Path &all ...";
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
			this.it_PathBaseItems2da.Text = "path &BaseItems.2da";
			this.it_PathBaseItems2da.Click += new System.EventHandler(this.itclick_PathBaseItems2da);
			// 
			// it_PathFeat2da
			// 
			this.it_PathFeat2da.Name = "it_PathFeat2da";
			this.it_PathFeat2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathFeat2da.Text = "path &Feat.2da";
			this.it_PathFeat2da.Click += new System.EventHandler(this.itclick_PathFeat2da);
			// 
			// it_PathItemPropDef2da
			// 
			this.it_PathItemPropDef2da.Name = "it_PathItemPropDef2da";
			this.it_PathItemPropDef2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathItemPropDef2da.Text = "path Item&PropDef.2da";
			this.it_PathItemPropDef2da.Click += new System.EventHandler(this.itclick_PathItemPropDef2da);
			// 
			// it_PathSkills2da
			// 
			this.it_PathSkills2da.Name = "it_PathSkills2da";
			this.it_PathSkills2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSkills2da.Text = "path S&kills.2da";
			this.it_PathSkills2da.Click += new System.EventHandler(this.itclick_PathSkills2da);
			// 
			// it_PathSpells2da
			// 
			this.it_PathSpells2da.Name = "it_PathSpells2da";
			this.it_PathSpells2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSpells2da.Text = "path &Spells.2da";
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
			this.it_PathClasses2da.Text = "path &Classes.2da";
			this.it_PathClasses2da.Click += new System.EventHandler(this.itclick_PathClasses2da);
			// 
			// it_PathDisease2da
			// 
			this.it_PathDisease2da.Name = "it_PathDisease2da";
			this.it_PathDisease2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathDisease2da.Text = "path &Disease.2da";
			this.it_PathDisease2da.Click += new System.EventHandler(this.itclick_PathDisease2da);
			// 
			// it_PathIprpAmmoCost2da
			// 
			this.it_PathIprpAmmoCost2da.Name = "it_PathIprpAmmoCost2da";
			this.it_PathIprpAmmoCost2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpAmmoCost2da.Text = "path Iprp_A&mmoCost.2da";
			this.it_PathIprpAmmoCost2da.Click += new System.EventHandler(this.itclick_PathIprpAmmoCost2da);
			// 
			// it_PathIprpFeats2da
			// 
			this.it_PathIprpFeats2da.Name = "it_PathIprpFeats2da";
			this.it_PathIprpFeats2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpFeats2da.Text = "path Iprp_F&eats.2da";
			this.it_PathIprpFeats2da.Click += new System.EventHandler(this.itclick_PathIprpFeats2da);
			// 
			// it_PathIprpOnHitSpell2da
			// 
			this.it_PathIprpOnHitSpell2da.Name = "it_PathIprpOnHitSpell2da";
			this.it_PathIprpOnHitSpell2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpOnHitSpell2da.Text = "path Iprp_On&HitSpell.2da";
			this.it_PathIprpOnHitSpell2da.Click += new System.EventHandler(this.itclick_PathIprpOnHitSpells2da);
			// 
			// it_PathIprpSpells2da
			// 
			this.it_PathIprpSpells2da.Name = "it_PathIprpSpells2da";
			this.it_PathIprpSpells2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathIprpSpells2da.Text = "path Iprp_Spe&lls.2da";
			this.it_PathIprpSpells2da.Click += new System.EventHandler(this.itclick_PathIprpSpells2da);
			// 
			// it_PathRaces2da
			// 
			this.it_PathRaces2da.Name = "it_PathRaces2da";
			this.it_PathRaces2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathRaces2da.Text = "path &RacialTypes.2da";
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
			this.it_PathCategories2da.Text = "path Categ&ories.2da";
			this.it_PathCategories2da.Click += new System.EventHandler(this.itclick_PathCategories2da);
			// 
			// it_PathRanges2da
			// 
			this.it_PathRanges2da.Name = "it_PathRanges2da";
			this.it_PathRanges2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathRanges2da.Text = "path Ra&nges.2da";
			this.it_PathRanges2da.Click += new System.EventHandler(this.itclick_PathRanges2da);
			// 
			// it_PathSpellTarget2da
			// 
			this.it_PathSpellTarget2da.Name = "it_PathSpellTarget2da";
			this.it_PathSpellTarget2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathSpellTarget2da.Text = "path Spell&Target.2da";
			this.it_PathSpellTarget2da.Click += new System.EventHandler(this.itclick_PathSpellTarget2da);
			// 
			// separator_31
			// 
			this.separator_31.Name = "separator_31";
			this.separator_31.Size = new System.Drawing.Size(219, 6);
			// 
			// it_PathMasterFeats2da
			// 
			this.it_PathMasterFeats2da.Name = "it_PathMasterFeats2da";
			this.it_PathMasterFeats2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathMasterFeats2da.Text = "path MasterFeats.2da";
			this.it_PathMasterFeats2da.Click += new System.EventHandler(this.itclick_PathMasterFeats2da);
			// 
			// it_PathCombatModes2da
			// 
			this.it_PathCombatModes2da.Name = "it_PathCombatModes2da";
			this.it_PathCombatModes2da.Size = new System.Drawing.Size(222, 22);
			this.it_PathCombatModes2da.Text = "path CombatModes.2da";
			this.it_PathCombatModes2da.Click += new System.EventHandler(this.itclick_PathCombatModes2da);
			// 
			// it_MenuFont
			// 
			this.it_MenuFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuFont.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Font,
			this.it_FontDefault});
			this.it_MenuFont.Name = "it_MenuFont";
			this.it_MenuFont.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuFont.Size = new System.Drawing.Size(39, 20);
			this.it_MenuFont.Text = "&Font";
			this.it_MenuFont.DropDownOpening += new System.EventHandler(this.font_dropdownopening);
			// 
			// it_Font
			// 
			this.it_Font.Name = "it_Font";
			this.it_Font.Size = new System.Drawing.Size(175, 22);
			this.it_Font.Text = "Font ... &be patient";
			this.it_Font.Click += new System.EventHandler(this.fontclick_Font);
			// 
			// it_FontDefault
			// 
			this.it_FontDefault.Name = "it_FontDefault";
			this.it_FontDefault.Size = new System.Drawing.Size(175, 22);
			this.it_FontDefault.Text = "&Load default font";
			this.it_FontDefault.Click += new System.EventHandler(this.fontclick_Default);
			// 
			// it_MenuHelp
			// 
			this.it_MenuHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_ReadMe,
			this.it_About});
			this.it_MenuHelp.Name = "it_MenuHelp";
			this.it_MenuHelp.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_MenuHelp.Size = new System.Drawing.Size(40, 20);
			this.it_MenuHelp.Text = "&Help";
			this.it_MenuHelp.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_ReadMe
			// 
			this.it_ReadMe.Name = "it_ReadMe";
			this.it_ReadMe.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.it_ReadMe.Size = new System.Drawing.Size(158, 22);
			this.it_ReadMe.Text = "&ReadMe.txt";
			this.it_ReadMe.Click += new System.EventHandler(this.helpclick_Help);
			// 
			// it_About
			// 
			this.it_About.Name = "it_About";
			this.it_About.Size = new System.Drawing.Size(158, 22);
			this.it_About.Text = "&About";
			this.it_About.Click += new System.EventHandler(this.helpclick_About);
			// 
			// ContextEditor
			// 
			this.ContextEditor.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ContextEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
			this.ContextEditor.Name = "ContextEditor";
			this.ContextEditor.ShowImageMargin = false;
			this.ContextEditor.Size = new System.Drawing.Size(177, 248);
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
			this.statbar_lblCords,
			this.statbar_lblInfo});
			this.statusbar.Location = new System.Drawing.Point(0, 432);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(842, 22);
			this.statusbar.TabIndex = 1;
			// 
			// statbar_lblCords
			// 
			this.statbar_lblCords.AutoSize = false;
			this.statbar_lblCords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statbar_lblCords.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statbar_lblCords.Name = "statbar_lblCords";
			this.statbar_lblCords.Size = new System.Drawing.Size(160, 17);
			this.statbar_lblCords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statbar_lblInfo
			// 
			this.statbar_lblInfo.AutoSize = false;
			this.statbar_lblInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statbar_lblInfo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statbar_lblInfo.Name = "statbar_lblInfo";
			this.statbar_lblInfo.Size = new System.Drawing.Size(667, 17);
			this.statbar_lblInfo.Spring = true;
			this.statbar_lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.it_cellEdit,
			this.separator_27,
			this.it_cellCopy,
			this.it_cellPaste,
			this.separator_16,
			this.it_cellStars,
			this.it_cellLower,
			this.it_cellUpper,
			this.separator_24,
			this.it_cellMergeCe,
			this.it_cellMergeRo,
			this.separator_28,
			this.it_cellStrref,
			this.it_cellInput});
			this.cellMenu.Name = "cellMenu";
			this.cellMenu.ShowImageMargin = false;
			this.cellMenu.Size = new System.Drawing.Size(165, 248);
			// 
			// it_cellEdit
			// 
			this.it_cellEdit.Name = "it_cellEdit";
			this.it_cellEdit.Size = new System.Drawing.Size(164, 22);
			this.it_cellEdit.Text = "edit";
			this.it_cellEdit.Click += new System.EventHandler(this.cellclick_EditCell);
			// 
			// separator_27
			// 
			this.separator_27.Name = "separator_27";
			this.separator_27.Size = new System.Drawing.Size(161, 6);
			// 
			// it_cellCopy
			// 
			this.it_cellCopy.Name = "it_cellCopy";
			this.it_cellCopy.Size = new System.Drawing.Size(164, 22);
			this.it_cellCopy.Text = "copy cell";
			this.it_cellCopy.Click += new System.EventHandler(this.editclick_CopyCell);
			// 
			// it_cellPaste
			// 
			this.it_cellPaste.Name = "it_cellPaste";
			this.it_cellPaste.Size = new System.Drawing.Size(164, 22);
			this.it_cellPaste.Text = "paste cell";
			this.it_cellPaste.Click += new System.EventHandler(this.editclick_PasteCell);
			// 
			// separator_16
			// 
			this.separator_16.Name = "separator_16";
			this.separator_16.Size = new System.Drawing.Size(161, 6);
			// 
			// it_cellStars
			// 
			this.it_cellStars.Name = "it_cellStars";
			this.it_cellStars.Size = new System.Drawing.Size(164, 22);
			this.it_cellStars.Text = "****";
			this.it_cellStars.Click += new System.EventHandler(this.cellclick_Stars);
			// 
			// it_cellLower
			// 
			this.it_cellLower.Name = "it_cellLower";
			this.it_cellLower.Size = new System.Drawing.Size(164, 22);
			this.it_cellLower.Text = "lowercase";
			this.it_cellLower.Click += new System.EventHandler(this.cellclick_Lowercase);
			// 
			// it_cellUpper
			// 
			this.it_cellUpper.Name = "it_cellUpper";
			this.it_cellUpper.Size = new System.Drawing.Size(164, 22);
			this.it_cellUpper.Text = "uppercase";
			this.it_cellUpper.Click += new System.EventHandler(this.cellclick_Uppercase);
			// 
			// separator_24
			// 
			this.separator_24.Name = "separator_24";
			this.separator_24.Size = new System.Drawing.Size(161, 6);
			// 
			// it_cellMergeCe
			// 
			this.it_cellMergeCe.Name = "it_cellMergeCe";
			this.it_cellMergeCe.Size = new System.Drawing.Size(164, 22);
			this.it_cellMergeCe.Text = "merge to other - Ce";
			this.it_cellMergeCe.Click += new System.EventHandler(this.cellclick_MergeCe);
			// 
			// it_cellMergeRo
			// 
			this.it_cellMergeRo.Name = "it_cellMergeRo";
			this.it_cellMergeRo.Size = new System.Drawing.Size(164, 22);
			this.it_cellMergeRo.Text = "merge to other - Ro";
			this.it_cellMergeRo.Click += new System.EventHandler(this.cellclick_MergeRo);
			// 
			// separator_28
			// 
			this.separator_28.Name = "separator_28";
			this.separator_28.Size = new System.Drawing.Size(161, 6);
			// 
			// it_cellStrref
			// 
			this.it_cellStrref.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_cellStrref_talktable,
			this.it_cellStrref_custom,
			this.it_cellStrref_invalid});
			this.it_cellStrref.Name = "it_cellStrref";
			this.it_cellStrref.Size = new System.Drawing.Size(164, 22);
			this.it_cellStrref.Text = "STRREF";
			this.it_cellStrref.DropDownOpening += new System.EventHandler(this.dropdownopening_Strref);
			// 
			// it_cellStrref_talktable
			// 
			this.it_cellStrref_talktable.Name = "it_cellStrref_talktable";
			this.it_cellStrref_talktable.Size = new System.Drawing.Size(160, 22);
			this.it_cellStrref_talktable.Text = "TalkTable";
			this.it_cellStrref_talktable.Click += new System.EventHandler(this.cellclick_Strref_talktable);
			// 
			// it_cellStrref_custom
			// 
			this.it_cellStrref_custom.Name = "it_cellStrref_custom";
			this.it_cellStrref_custom.Size = new System.Drawing.Size(160, 22);
			this.it_cellStrref_custom.Text = "set Custom";
			this.it_cellStrref_custom.Click += new System.EventHandler(this.cellclick_Strref_custom);
			// 
			// it_cellStrref_invalid
			// 
			this.it_cellStrref_invalid.Name = "it_cellStrref_invalid";
			this.it_cellStrref_invalid.Size = new System.Drawing.Size(160, 22);
			this.it_cellStrref_invalid.Text = "set Invalid (-1)";
			this.it_cellStrref_invalid.Click += new System.EventHandler(this.cellclick_Strref_invalid);
			// 
			// it_cellInput
			// 
			this.it_cellInput.Name = "it_cellInput";
			this.it_cellInput.Size = new System.Drawing.Size(164, 22);
			this.it_cellInput.Text = "InfoInput";
			this.it_cellInput.Click += new System.EventHandler(this.cellclick_InfoInput);
			// 
			// YataForm
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(842, 454);
			this.Controls.Add(this.panel_ColorFill);
			this.Controls.Add(this.menubar);
			this.Controls.Add(this.statusbar);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menubar;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "YataForm";
			this.Text = " Yata";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.yata_Closing);
			this.Load += new System.EventHandler(this.yata_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.yata_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.yata_DragEnter);
			this.tabMenu.ResumeLayout(false);
			this.menubar.ResumeLayout(false);
			this.menubar.PerformLayout();
			this.ContextEditor.ResumeLayout(false);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.cellMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
