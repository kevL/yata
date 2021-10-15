using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace yata
{
	sealed partial class YataForm
	{
		// okay. Fed up.
		// YataTabs 'tabControl' and PropanelButton 'bu_Propanel' have been
		// moved to YataForm.


		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		IContainer components;

		MenuStrip menubar;

		ToolStripMenuItem it_MenuFile;
		ToolStripMenuItem it_Create;
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
		ToolStripMenuItem it_DeselectAll;
		ToolStripMenuItem it_Search;
		ToolStripMenuItem it_Searchnext;
		ToolStripMenuItem it_Goto;
		ToolStripMenuItem it_GotoLoadchanged;
		ToolStripMenuItem it_Defaultval;
		ToolStripMenuItem it_Defaultclear;

		ToolStripMenuItem it_MenuCells;
		ToolStripMenuItem it_DeselectCell;
		ToolStripMenuItem it_CutCell;
		ToolStripMenuItem it_CopyCell;
		ToolStripMenuItem it_PasteCell;
		ToolStripMenuItem it_DeleteCell;
		ToolStripMenuItem it_Lower;
		ToolStripMenuItem it_Upper;
		ToolStripMenuItem it_Apply;

		ToolStripMenuItem it_MenuRows;
		ToolStripMenuItem it_DeselectRows;
		ToolStripMenuItem it_CutRange;
		ToolStripMenuItem it_CopyRange;
		ToolStripMenuItem it_PasteRange;
		ToolStripMenuItem it_DeleteRange;
		ToolStripMenuItem it_CreateRows;

		ToolStripMenuItem it_MenuCol;
		ToolStripMenuItem it_DeselectCol;
		ToolStripMenuItem it_CreateHead;
		ToolStripMenuItem it_DeleteHead;
		ToolStripMenuItem it_RelabelHead;
		ToolStripMenuItem it_CopyCells;
		ToolStripMenuItem it_PasteCells;

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
		ToolStripMenuItem it_Propanel;
		ToolStripMenuItem it_PropanelLoc;
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
		ToolStripMenuItem it_Settings;

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
		ToolStripSeparator separator_33;
		ToolStripSeparator separator_34;
		ToolStripSeparator separator_35;
		ToolStripSeparator separator_36;
		ToolStripSeparator separator_37;
		ToolStripSeparator separator_38;

		internal ContextMenuStrip ContextRow;
		ToolStripMenuItem rowit_Header;
		ToolStripMenuItem rowit_Cut;
		ToolStripMenuItem rowit_Copy;
		ToolStripMenuItem rowit_PasteAbove;
		ToolStripMenuItem rowit_Paste;
		ToolStripMenuItem rowit_PasteBelow;
		ToolStripMenuItem rowit_CreateAbove;
		ToolStripMenuItem rowit_Clear;
		ToolStripMenuItem rowit_CreateBelow;
		ToolStripMenuItem rowit_Delete;

		internal ContextMenuStrip ContextCell;
		ToolStripMenuItem cellit_Edit;
		ToolStripMenuItem cellit_Cut;
		ToolStripMenuItem cellit_Copy;
		ToolStripMenuItem cellit_Paste;
		ToolStripMenuItem cellit_Clear;
		ToolStripMenuItem cellit_Lower;
		ToolStripMenuItem cellit_Upper;
		ToolStripMenuItem cellit_MergeCe;
		ToolStripMenuItem cellit_MergeRo;
		ToolStripMenuItem cellit_Input;
		ToolStripMenuItem cellit_Strref;
		ToolStripMenuItem cellit_Strref_talktable;
		ToolStripMenuItem cellit_Strref_custom;
		ToolStripMenuItem cellit_Strref_invalid;

		internal ContextMenuStrip ContextTab;
		ToolStripMenuItem tabit_Close;
		ToolStripMenuItem tabit_CloseAll;
		ToolStripMenuItem tabit_CloseAllOthers;
		ToolStripMenuItem tabit_Save;
		ToolStripMenuItem tabit_Reload;
		ToolStripMenuItem tabit_Diff1;
		ToolStripMenuItem tabit_Diff2;
		ToolStripMenuItem tabit_DiffReset;
		ToolStripMenuItem tabit_DiffSync;

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

			FontDefault.Dispose();
			FontAccent .Dispose();

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
			this.menubar = new System.Windows.Forms.MenuStrip();
			this.it_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Create = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_38 = new System.Windows.Forms.ToolStripSeparator();
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
			this.separator_35 = new System.Windows.Forms.ToolStripSeparator();
			this.it_DeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_18 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Search = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Searchnext = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_3 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Goto = new System.Windows.Forms.ToolStripMenuItem();
			this.it_GotoLoadchanged = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_37 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Defaultval = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Defaultclear = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuCells = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeselectCell = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_30 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CutCell = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyCell = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteCell = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeleteCell = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_20 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Lower = new System.Windows.Forms.ToolStripMenuItem();
			this.it_Upper = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_4 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Apply = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeselectRows = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_33 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CutRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_CopyRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeleteRange = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_13 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CreateRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_MenuCol = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeselectCol = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_34 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CreateHead = new System.Windows.Forms.ToolStripMenuItem();
			this.it_DeleteHead = new System.Windows.Forms.ToolStripMenuItem();
			this.it_RelabelHead = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_32 = new System.Windows.Forms.ToolStripSeparator();
			this.it_CopyCells = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PasteCells = new System.Windows.Forms.ToolStripMenuItem();
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
			this.it_ColorRows = new System.Windows.Forms.ToolStripMenuItem();
			this.it_AutoCols = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_12 = new System.Windows.Forms.ToolStripSeparator();
			this.it_freeze1 = new System.Windows.Forms.ToolStripMenuItem();
			this.it_freeze2 = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_17 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Propanel = new System.Windows.Forms.ToolStripMenuItem();
			this.it_PropanelLoc = new System.Windows.Forms.ToolStripMenuItem();
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
			this.separator_36 = new System.Windows.Forms.ToolStripSeparator();
			this.it_Settings = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextRow = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.rowit_Header = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_7 = new System.Windows.Forms.ToolStripSeparator();
			this.rowit_Cut = new System.Windows.Forms.ToolStripMenuItem();
			this.rowit_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_8 = new System.Windows.Forms.ToolStripSeparator();
			this.rowit_PasteAbove = new System.Windows.Forms.ToolStripMenuItem();
			this.rowit_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.rowit_PasteBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_9 = new System.Windows.Forms.ToolStripSeparator();
			this.rowit_CreateAbove = new System.Windows.Forms.ToolStripMenuItem();
			this.rowit_Clear = new System.Windows.Forms.ToolStripMenuItem();
			this.rowit_CreateBelow = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_10 = new System.Windows.Forms.ToolStripSeparator();
			this.rowit_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statbar_lblCords = new System.Windows.Forms.ToolStripStatusLabel();
			this.statbar_lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel_ColorFill = new System.Windows.Forms.Panel();
			this.ContextCell = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cellit_Edit = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_27 = new System.Windows.Forms.ToolStripSeparator();
			this.cellit_Cut = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Clear = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_16 = new System.Windows.Forms.ToolStripSeparator();
			this.cellit_Lower = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Upper = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_24 = new System.Windows.Forms.ToolStripSeparator();
			this.cellit_MergeCe = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_MergeRo = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_28 = new System.Windows.Forms.ToolStripSeparator();
			this.cellit_Strref = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Strref_talktable = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Strref_custom = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Strref_invalid = new System.Windows.Forms.ToolStripMenuItem();
			this.cellit_Input = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tabit_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.tabit_CloseAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tabit_CloseAllOthers = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_14 = new System.Windows.Forms.ToolStripSeparator();
			this.tabit_Save = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_26 = new System.Windows.Forms.ToolStripSeparator();
			this.tabit_Reload = new System.Windows.Forms.ToolStripMenuItem();
			this.separator_23 = new System.Windows.Forms.ToolStripSeparator();
			this.tabit_Diff1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tabit_Diff2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tabit_DiffReset = new System.Windows.Forms.ToolStripMenuItem();
			this.tabit_DiffSync = new System.Windows.Forms.ToolStripMenuItem();
			this.menubar.SuspendLayout();
			this.ContextRow.SuspendLayout();
			this.statusbar.SuspendLayout();
			this.ContextCell.SuspendLayout();
			this.ContextTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// menubar
			// 
			this.menubar.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_MenuFile,
			this.it_MenuEdit,
			this.it_MenuCells,
			this.it_MenuRows,
			this.it_MenuCol,
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
			this.menubar.Size = new System.Drawing.Size(867, 24);
			this.menubar.TabIndex = 0;
			// 
			// it_MenuFile
			// 
			this.it_MenuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_Create,
			this.separator_38,
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
			// it_Create
			// 
			this.it_Create.Name = "it_Create";
			this.it_Create.Size = new System.Drawing.Size(181, 22);
			this.it_Create.Text = "Create ...";
			this.it_Create.Click += new System.EventHandler(this.fileclick_Create);
			// 
			// separator_38
			// 
			this.separator_38.Name = "separator_38";
			this.separator_38.Size = new System.Drawing.Size(178, 6);
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
			this.it_Recent.Visible = false;
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
			this.it_Close.Click += new System.EventHandler(this.fileclick_ClosePage);
			// 
			// it_CloseAll
			// 
			this.it_CloseAll.Enabled = false;
			this.it_CloseAll.Name = "it_CloseAll";
			this.it_CloseAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
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
			this.separator_35,
			this.it_DeselectAll,
			this.separator_18,
			this.it_Search,
			this.it_Searchnext,
			this.separator_3,
			this.it_Goto,
			this.it_GotoLoadchanged,
			this.separator_37,
			this.it_Defaultval,
			this.it_Defaultclear});
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
			this.it_Undo.Size = new System.Drawing.Size(222, 22);
			this.it_Undo.Text = "&Undo";
			this.it_Undo.Click += new System.EventHandler(this.editclick_Undo);
			// 
			// it_Redo
			// 
			this.it_Redo.Enabled = false;
			this.it_Redo.Name = "it_Redo";
			this.it_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.it_Redo.Size = new System.Drawing.Size(222, 22);
			this.it_Redo.Text = "&Redo";
			this.it_Redo.Click += new System.EventHandler(this.editclick_Redo);
			// 
			// separator_35
			// 
			this.separator_35.Name = "separator_35";
			this.separator_35.Size = new System.Drawing.Size(219, 6);
			// 
			// it_DeselectAll
			// 
			this.it_DeselectAll.Enabled = false;
			this.it_DeselectAll.Name = "it_DeselectAll";
			this.it_DeselectAll.ShortcutKeyDisplayString = "Esc";
			this.it_DeselectAll.Size = new System.Drawing.Size(222, 22);
			this.it_DeselectAll.Text = "De&select all";
			this.it_DeselectAll.Click += new System.EventHandler(this.editclick_Deselect);
			// 
			// separator_18
			// 
			this.separator_18.Name = "separator_18";
			this.separator_18.Size = new System.Drawing.Size(219, 6);
			// 
			// it_Search
			// 
			this.it_Search.Name = "it_Search";
			this.it_Search.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.it_Search.Size = new System.Drawing.Size(222, 22);
			this.it_Search.Text = "&Find";
			this.it_Search.Click += new System.EventHandler(this.editclick_Search);
			// 
			// it_Searchnext
			// 
			this.it_Searchnext.Enabled = false;
			this.it_Searchnext.Name = "it_Searchnext";
			this.it_Searchnext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.it_Searchnext.Size = new System.Drawing.Size(222, 22);
			this.it_Searchnext.Text = "Fin&d next";
			this.it_Searchnext.Click += new System.EventHandler(this.editclick_SearchNext);
			// 
			// separator_3
			// 
			this.separator_3.Name = "separator_3";
			this.separator_3.Size = new System.Drawing.Size(219, 6);
			// 
			// it_Goto
			// 
			this.it_Goto.Name = "it_Goto";
			this.it_Goto.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.it_Goto.Size = new System.Drawing.Size(222, 22);
			this.it_Goto.Text = "&Goto";
			this.it_Goto.Click += new System.EventHandler(this.editclick_Goto);
			// 
			// it_GotoLoadchanged
			// 
			this.it_GotoLoadchanged.Enabled = false;
			this.it_GotoLoadchanged.Name = "it_GotoLoadchanged";
			this.it_GotoLoadchanged.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.it_GotoLoadchanged.Size = new System.Drawing.Size(222, 22);
			this.it_GotoLoadchanged.Text = "Goto loadcha&nged";
			this.it_GotoLoadchanged.Click += new System.EventHandler(this.editclick_GotoLoadchanged);
			// 
			// separator_37
			// 
			this.separator_37.Name = "separator_37";
			this.separator_37.Size = new System.Drawing.Size(219, 6);
			// 
			// it_Defaultval
			// 
			this.it_Defaultval.Enabled = false;
			this.it_Defaultval.Name = "it_Defaultval";
			this.it_Defaultval.Size = new System.Drawing.Size(222, 22);
			this.it_Defaultval.Text = "Default &value ...";
			this.it_Defaultval.Click += new System.EventHandler(this.editclick_Defaultval);
			// 
			// it_Defaultclear
			// 
			this.it_Defaultclear.Enabled = false;
			this.it_Defaultclear.Name = "it_Defaultclear";
			this.it_Defaultclear.Size = new System.Drawing.Size(222, 22);
			this.it_Defaultclear.Text = "&Clear Default";
			this.it_Defaultclear.Click += new System.EventHandler(this.editclick_Defaultclear);
			// 
			// it_MenuCells
			// 
			this.it_MenuCells.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuCells.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_DeselectCell,
			this.separator_30,
			this.it_CutCell,
			this.it_CopyCell,
			this.it_PasteCell,
			this.it_DeleteCell,
			this.separator_20,
			this.it_Lower,
			this.it_Upper,
			this.separator_4,
			this.it_Apply});
			this.it_MenuCells.Name = "it_MenuCells";
			this.it_MenuCells.Size = new System.Drawing.Size(47, 20);
			this.it_MenuCells.Text = "&Cells";
			this.it_MenuCells.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_DeselectCell
			// 
			this.it_DeselectCell.Enabled = false;
			this.it_DeselectCell.Name = "it_DeselectCell";
			this.it_DeselectCell.Size = new System.Drawing.Size(150, 22);
			this.it_DeselectCell.Text = "De&select";
			this.it_DeselectCell.Click += new System.EventHandler(this.editcellsclick_Deselect);
			// 
			// separator_30
			// 
			this.separator_30.Name = "separator_30";
			this.separator_30.Size = new System.Drawing.Size(147, 6);
			// 
			// it_CutCell
			// 
			this.it_CutCell.Enabled = false;
			this.it_CutCell.Name = "it_CutCell";
			this.it_CutCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.it_CutCell.Size = new System.Drawing.Size(150, 22);
			this.it_CutCell.Text = "Cu&t";
			this.it_CutCell.Click += new System.EventHandler(this.editcellsclick_CutCell);
			// 
			// it_CopyCell
			// 
			this.it_CopyCell.Enabled = false;
			this.it_CopyCell.Name = "it_CopyCell";
			this.it_CopyCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.it_CopyCell.Size = new System.Drawing.Size(150, 22);
			this.it_CopyCell.Text = "&Copy";
			this.it_CopyCell.Click += new System.EventHandler(this.editcellsclick_CopyCell);
			// 
			// it_PasteCell
			// 
			this.it_PasteCell.Enabled = false;
			this.it_PasteCell.Name = "it_PasteCell";
			this.it_PasteCell.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.it_PasteCell.Size = new System.Drawing.Size(150, 22);
			this.it_PasteCell.Text = "&Paste";
			this.it_PasteCell.Click += new System.EventHandler(this.editcellsclick_PasteCell);
			// 
			// it_DeleteCell
			// 
			this.it_DeleteCell.Enabled = false;
			this.it_DeleteCell.Name = "it_DeleteCell";
			this.it_DeleteCell.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.it_DeleteCell.Size = new System.Drawing.Size(150, 22);
			this.it_DeleteCell.Text = "Clea&r";
			this.it_DeleteCell.Click += new System.EventHandler(this.editcellsclick_Delete);
			// 
			// separator_20
			// 
			this.separator_20.Name = "separator_20";
			this.separator_20.Size = new System.Drawing.Size(147, 6);
			// 
			// it_Lower
			// 
			this.it_Lower.Enabled = false;
			this.it_Lower.Name = "it_Lower";
			this.it_Lower.Size = new System.Drawing.Size(150, 22);
			this.it_Lower.Text = "&Lowercase";
			this.it_Lower.Click += new System.EventHandler(this.editcellsclick_Lower);
			// 
			// it_Upper
			// 
			this.it_Upper.Enabled = false;
			this.it_Upper.Name = "it_Upper";
			this.it_Upper.Size = new System.Drawing.Size(150, 22);
			this.it_Upper.Text = "&Uppercase";
			this.it_Upper.Click += new System.EventHandler(this.editcellsclick_Upper);
			// 
			// separator_4
			// 
			this.separator_4.Name = "separator_4";
			this.separator_4.Size = new System.Drawing.Size(147, 6);
			// 
			// it_Apply
			// 
			this.it_Apply.Enabled = false;
			this.it_Apply.Name = "it_Apply";
			this.it_Apply.Size = new System.Drawing.Size(150, 22);
			this.it_Apply.Text = "&Apply text ...";
			this.it_Apply.Click += new System.EventHandler(this.editcellsclick_Apply);
			// 
			// it_MenuRows
			// 
			this.it_MenuRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.it_MenuRows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_DeselectRows,
			this.separator_33,
			this.it_CutRange,
			this.it_CopyRange,
			this.it_PasteRange,
			this.it_DeleteRange,
			this.separator_13,
			this.it_CreateRows});
			this.it_MenuRows.Name = "it_MenuRows";
			this.it_MenuRows.Size = new System.Drawing.Size(49, 20);
			this.it_MenuRows.Text = "&Rows";
			this.it_MenuRows.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_DeselectRows
			// 
			this.it_DeselectRows.Enabled = false;
			this.it_DeselectRows.Name = "it_DeselectRows";
			this.it_DeselectRows.Size = new System.Drawing.Size(185, 22);
			this.it_DeselectRows.Text = "De&select";
			this.it_DeselectRows.Click += new System.EventHandler(this.editrowsclick_Deselect);
			// 
			// separator_33
			// 
			this.separator_33.Name = "separator_33";
			this.separator_33.Size = new System.Drawing.Size(182, 6);
			// 
			// it_CutRange
			// 
			this.it_CutRange.Enabled = false;
			this.it_CutRange.Name = "it_CutRange";
			this.it_CutRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.X)));
			this.it_CutRange.Size = new System.Drawing.Size(185, 22);
			this.it_CutRange.Text = "Cu&t";
			this.it_CutRange.Click += new System.EventHandler(this.editrowsclick_CutRange);
			// 
			// it_CopyRange
			// 
			this.it_CopyRange.Enabled = false;
			this.it_CopyRange.Name = "it_CopyRange";
			this.it_CopyRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.C)));
			this.it_CopyRange.Size = new System.Drawing.Size(185, 22);
			this.it_CopyRange.Text = "&Copy";
			this.it_CopyRange.Click += new System.EventHandler(this.editrowsclick_CopyRange);
			// 
			// it_PasteRange
			// 
			this.it_PasteRange.Enabled = false;
			this.it_PasteRange.Name = "it_PasteRange";
			this.it_PasteRange.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
			| System.Windows.Forms.Keys.V)));
			this.it_PasteRange.Size = new System.Drawing.Size(185, 22);
			this.it_PasteRange.Text = "&Paste";
			this.it_PasteRange.Click += new System.EventHandler(this.editrowsclick_PasteRange);
			// 
			// it_DeleteRange
			// 
			this.it_DeleteRange.Enabled = false;
			this.it_DeleteRange.Name = "it_DeleteRange";
			this.it_DeleteRange.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
			this.it_DeleteRange.Size = new System.Drawing.Size(185, 22);
			this.it_DeleteRange.Text = "&Delete";
			this.it_DeleteRange.Click += new System.EventHandler(this.editrowsclick_DeleteRange);
			// 
			// separator_13
			// 
			this.separator_13.Name = "separator_13";
			this.separator_13.Size = new System.Drawing.Size(182, 6);
			// 
			// it_CreateRows
			// 
			this.it_CreateRows.Enabled = false;
			this.it_CreateRows.Name = "it_CreateRows";
			this.it_CreateRows.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.it_CreateRows.Size = new System.Drawing.Size(185, 22);
			this.it_CreateRows.Text = "C&reate ...";
			this.it_CreateRows.Click += new System.EventHandler(this.editrowsclick_CreateRows);
			// 
			// it_MenuCol
			// 
			this.it_MenuCol.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.it_DeselectCol,
			this.separator_34,
			this.it_CreateHead,
			this.it_DeleteHead,
			this.it_RelabelHead,
			this.separator_32,
			this.it_CopyCells,
			this.it_PasteCells});
			this.it_MenuCol.Name = "it_MenuCol";
			this.it_MenuCol.Size = new System.Drawing.Size(38, 20);
			this.it_MenuCol.Text = "C&ol";
			this.it_MenuCol.DropDownOpening += new System.EventHandler(this.editcol_dropdownopening);
			// 
			// it_DeselectCol
			// 
			this.it_DeselectCol.Name = "it_DeselectCol";
			this.it_DeselectCol.Size = new System.Drawing.Size(161, 22);
			this.it_DeselectCol.Text = "de&select";
			this.it_DeselectCol.Click += new System.EventHandler(this.editcolclick_Deselect);
			// 
			// separator_34
			// 
			this.separator_34.Name = "separator_34";
			this.separator_34.Size = new System.Drawing.Size(158, 6);
			// 
			// it_CreateHead
			// 
			this.it_CreateHead.Name = "it_CreateHead";
			this.it_CreateHead.Size = new System.Drawing.Size(161, 22);
			this.it_CreateHead.Text = "c&reate head ...";
			this.it_CreateHead.Click += new System.EventHandler(this.editcolclick_CreateHead);
			// 
			// it_DeleteHead
			// 
			this.it_DeleteHead.Name = "it_DeleteHead";
			this.it_DeleteHead.Size = new System.Drawing.Size(161, 22);
			this.it_DeleteHead.Text = "&delete head ...";
			this.it_DeleteHead.Click += new System.EventHandler(this.editcolclick_DeleteHead);
			// 
			// it_RelabelHead
			// 
			this.it_RelabelHead.Name = "it_RelabelHead";
			this.it_RelabelHead.Size = new System.Drawing.Size(161, 22);
			this.it_RelabelHead.Text = "re&label head ...";
			this.it_RelabelHead.Click += new System.EventHandler(this.editcolclick_RelabelHead);
			// 
			// separator_32
			// 
			this.separator_32.Name = "separator_32";
			this.separator_32.Size = new System.Drawing.Size(158, 6);
			// 
			// it_CopyCells
			// 
			this.it_CopyCells.Name = "it_CopyCells";
			this.it_CopyCells.Size = new System.Drawing.Size(161, 22);
			this.it_CopyCells.Text = "&copy cells";
			this.it_CopyCells.Click += new System.EventHandler(this.editcolclick_CopyCol);
			// 
			// it_PasteCells
			// 
			this.it_PasteCells.Name = "it_PasteCells";
			this.it_PasteCells.Size = new System.Drawing.Size(161, 22);
			this.it_PasteCells.Text = "&paste cells";
			this.it_PasteCells.Click += new System.EventHandler(this.editcolclick_PasteCol);
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
			this.it_MenuClipboard.Text = "Clip&board";
			this.it_MenuClipboard.DropDownOpening += new System.EventHandler(this.dropdownopening);
			// 
			// it_ClipExport
			// 
			this.it_ClipExport.Enabled = false;
			this.it_ClipExport.Name = "it_ClipExport";
			this.it_ClipExport.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.it_ClipExport.Size = new System.Drawing.Size(237, 22);
			this.it_ClipExport.Text = "&Export copied row(s)";
			this.it_ClipExport.Click += new System.EventHandler(this.clipclick_ExportCopy);
			// 
			// it_ClipImport
			// 
			this.it_ClipImport.Name = "it_ClipImport";
			this.it_ClipImport.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.it_ClipImport.Size = new System.Drawing.Size(237, 22);
			this.it_ClipImport.Text = "&Import clipboard row(s)";
			this.it_ClipImport.Click += new System.EventHandler(this.clipclick_ImportCopy);
			// 
			// separator_19
			// 
			this.separator_19.Name = "separator_19";
			this.separator_19.Size = new System.Drawing.Size(234, 6);
			// 
			// it_OpenClipEditor
			// 
			this.it_OpenClipEditor.Name = "it_OpenClipEditor";
			this.it_OpenClipEditor.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.it_OpenClipEditor.Size = new System.Drawing.Size(237, 22);
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
			this.it_ColorRows,
			this.it_AutoCols,
			this.separator_12,
			this.it_freeze1,
			this.it_freeze2,
			this.separator_17,
			this.it_Propanel,
			this.it_PropanelLoc,
			this.separator_25,
			this.it_ExternDiff,
			this.separator_21,
			this.it_ClearUr});
			this.it_Menu2daOps.Name = "it_Menu2daOps";
			this.it_Menu2daOps.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.it_Menu2daOps.Size = new System.Drawing.Size(62, 20);
			this.it_Menu2daOps.Text = "2d&a Ops";
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
			// it_ColorRows
			// 
			this.it_ColorRows.Enabled = false;
			this.it_ColorRows.Name = "it_ColorRows";
			this.it_ColorRows.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.it_ColorRows.Size = new System.Drawing.Size(236, 22);
			this.it_ColorRows.Text = "Reco&lor rows";
			this.it_ColorRows.Click += new System.EventHandler(this.opsclick_Recolor);
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
			// it_Propanel
			// 
			this.it_Propanel.Enabled = false;
			this.it_Propanel.Name = "it_Propanel";
			this.it_Propanel.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.it_Propanel.Size = new System.Drawing.Size(236, 22);
			this.it_Propanel.Text = "&Propanel";
			this.it_Propanel.Click += new System.EventHandler(this.opsclick_Propanel);
			// 
			// it_PropanelLoc
			// 
			this.it_PropanelLoc.Enabled = false;
			this.it_PropanelLoc.Name = "it_PropanelLoc";
			this.it_PropanelLoc.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.it_PropanelLoc.Size = new System.Drawing.Size(236, 22);
			this.it_PropanelLoc.Text = "Propanel l&ocation";
			this.it_PropanelLoc.Click += new System.EventHandler(this.opsclick_PropanelLocation);
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
			this.it_PathTalkC.Text = "Path to &custom ...";
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
			this.it_PathItemPropDef2da.Text = "path &ItemPropDef.2da";
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
			this.it_PathIprpOnHitSpell2da.Click += new System.EventHandler(this.itclick_PathIprpOnHitSpell2da);
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
			this.it_MenuFont.Text = "Fo&nt";
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
			this.it_About,
			this.separator_36,
			this.it_Settings});
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
			// separator_36
			// 
			this.separator_36.Name = "separator_36";
			this.separator_36.Size = new System.Drawing.Size(155, 6);
			// 
			// it_Settings
			// 
			this.it_Settings.Name = "it_Settings";
			this.it_Settings.Size = new System.Drawing.Size(158, 22);
			this.it_Settings.Text = "&Options file";
			this.it_Settings.Click += new System.EventHandler(this.helpclick_Settings);
			// 
			// ContextRow
			// 
			this.ContextRow.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ContextRow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.rowit_Header,
			this.separator_7,
			this.rowit_Cut,
			this.rowit_Copy,
			this.separator_8,
			this.rowit_PasteAbove,
			this.rowit_Paste,
			this.rowit_PasteBelow,
			this.separator_9,
			this.rowit_CreateAbove,
			this.rowit_Clear,
			this.rowit_CreateBelow,
			this.separator_10,
			this.rowit_Delete});
			this.ContextRow.Name = "ContextRow";
			this.ContextRow.ShowImageMargin = false;
			this.ContextRow.Size = new System.Drawing.Size(177, 248);
			// 
			// rowit_Header
			// 
			this.rowit_Header.Font = new System.Drawing.Font("Verdana", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rowit_Header.Name = "rowit_Header";
			this.rowit_Header.Size = new System.Drawing.Size(176, 22);
			this.rowit_Header.Text = "_row @ 16";
			this.rowit_Header.Click += new System.EventHandler(this.rowclick_Header);
			// 
			// separator_7
			// 
			this.separator_7.Name = "separator_7";
			this.separator_7.Size = new System.Drawing.Size(173, 6);
			// 
			// rowit_Cut
			// 
			this.rowit_Cut.Name = "rowit_Cut";
			this.rowit_Cut.Size = new System.Drawing.Size(176, 22);
			this.rowit_Cut.Text = "Cut row @ id";
			this.rowit_Cut.Click += new System.EventHandler(this.rowclick_Cut);
			// 
			// rowit_Copy
			// 
			this.rowit_Copy.Name = "rowit_Copy";
			this.rowit_Copy.Size = new System.Drawing.Size(176, 22);
			this.rowit_Copy.Text = "Copy row @ id";
			this.rowit_Copy.Click += new System.EventHandler(this.rowclick_Copy);
			// 
			// separator_8
			// 
			this.separator_8.Name = "separator_8";
			this.separator_8.Size = new System.Drawing.Size(173, 6);
			// 
			// rowit_PasteAbove
			// 
			this.rowit_PasteAbove.Name = "rowit_PasteAbove";
			this.rowit_PasteAbove.Size = new System.Drawing.Size(176, 22);
			this.rowit_PasteAbove.Text = "Paste clip above id";
			this.rowit_PasteAbove.Click += new System.EventHandler(this.rowclick_PasteAbove);
			// 
			// rowit_Paste
			// 
			this.rowit_Paste.Name = "rowit_Paste";
			this.rowit_Paste.Size = new System.Drawing.Size(176, 22);
			this.rowit_Paste.Text = "Paste clip @ id";
			this.rowit_Paste.Click += new System.EventHandler(this.rowclick_Paste);
			// 
			// rowit_PasteBelow
			// 
			this.rowit_PasteBelow.Name = "rowit_PasteBelow";
			this.rowit_PasteBelow.Size = new System.Drawing.Size(176, 22);
			this.rowit_PasteBelow.Text = "Paste clip below id";
			this.rowit_PasteBelow.Click += new System.EventHandler(this.rowclick_PasteBelow);
			// 
			// separator_9
			// 
			this.separator_9.Name = "separator_9";
			this.separator_9.Size = new System.Drawing.Size(173, 6);
			// 
			// rowit_CreateAbove
			// 
			this.rowit_CreateAbove.Name = "rowit_CreateAbove";
			this.rowit_CreateAbove.Size = new System.Drawing.Size(176, 22);
			this.rowit_CreateAbove.Text = "Create blank above id";
			this.rowit_CreateAbove.Click += new System.EventHandler(this.rowclick_CreateAbove);
			// 
			// rowit_Clear
			// 
			this.rowit_Clear.Name = "rowit_Clear";
			this.rowit_Clear.Size = new System.Drawing.Size(176, 22);
			this.rowit_Clear.Text = "Clear fields @ id";
			this.rowit_Clear.Click += new System.EventHandler(this.rowclick_Clear);
			// 
			// rowit_CreateBelow
			// 
			this.rowit_CreateBelow.Name = "rowit_CreateBelow";
			this.rowit_CreateBelow.Size = new System.Drawing.Size(176, 22);
			this.rowit_CreateBelow.Text = "Create blank below id";
			this.rowit_CreateBelow.Click += new System.EventHandler(this.rowclick_CreateBelow);
			// 
			// separator_10
			// 
			this.separator_10.Name = "separator_10";
			this.separator_10.Size = new System.Drawing.Size(173, 6);
			// 
			// rowit_Delete
			// 
			this.rowit_Delete.Name = "rowit_Delete";
			this.rowit_Delete.Size = new System.Drawing.Size(176, 22);
			this.rowit_Delete.Text = "Delete @ id";
			this.rowit_Delete.Click += new System.EventHandler(this.rowclick_Delete);
			// 
			// statusbar
			// 
			this.statusbar.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.statbar_lblCords,
			this.statbar_lblInfo});
			this.statusbar.Location = new System.Drawing.Point(0, 477);
			this.statusbar.Name = "statusbar";
			this.statusbar.Size = new System.Drawing.Size(867, 22);
			this.statusbar.TabIndex = 1;
			// 
			// statbar_lblCords
			// 
			this.statbar_lblCords.AutoSize = false;
			this.statbar_lblCords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statbar_lblCords.Font = new System.Drawing.Font("Verdana", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statbar_lblCords.Name = "statbar_lblCords";
			this.statbar_lblCords.Size = new System.Drawing.Size(160, 17);
			this.statbar_lblCords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statbar_lblInfo
			// 
			this.statbar_lblInfo.AutoSize = false;
			this.statbar_lblInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statbar_lblInfo.Font = new System.Drawing.Font("Verdana", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statbar_lblInfo.Name = "statbar_lblInfo";
			this.statbar_lblInfo.Size = new System.Drawing.Size(692, 17);
			this.statbar_lblInfo.Spring = true;
			this.statbar_lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel_ColorFill
			// 
			this.panel_ColorFill.BackColor = System.Drawing.Color.LightSeaGreen;
			this.panel_ColorFill.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_ColorFill.Location = new System.Drawing.Point(0, 24);
			this.panel_ColorFill.Name = "panel_ColorFill";
			this.panel_ColorFill.Size = new System.Drawing.Size(867, 453);
			this.panel_ColorFill.TabIndex = 2;
			// 
			// ContextCell
			// 
			this.ContextCell.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ContextCell.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cellit_Edit,
			this.separator_27,
			this.cellit_Cut,
			this.cellit_Copy,
			this.cellit_Paste,
			this.cellit_Clear,
			this.separator_16,
			this.cellit_Lower,
			this.cellit_Upper,
			this.separator_24,
			this.cellit_MergeCe,
			this.cellit_MergeRo,
			this.separator_28,
			this.cellit_Strref,
			this.cellit_Input});
			this.ContextCell.Name = "ContextCell";
			this.ContextCell.ShowImageMargin = false;
			this.ContextCell.Size = new System.Drawing.Size(165, 270);
			// 
			// cellit_Edit
			// 
			this.cellit_Edit.Name = "cellit_Edit";
			this.cellit_Edit.Size = new System.Drawing.Size(164, 22);
			this.cellit_Edit.Text = "edit";
			this.cellit_Edit.Click += new System.EventHandler(this.cellclick_Edit);
			// 
			// separator_27
			// 
			this.separator_27.Name = "separator_27";
			this.separator_27.Size = new System.Drawing.Size(161, 6);
			// 
			// cellit_Cut
			// 
			this.cellit_Cut.Name = "cellit_Cut";
			this.cellit_Cut.Size = new System.Drawing.Size(164, 22);
			this.cellit_Cut.Text = "cut";
			this.cellit_Cut.Click += new System.EventHandler(this.cellclick_Cut);
			// 
			// cellit_Copy
			// 
			this.cellit_Copy.Name = "cellit_Copy";
			this.cellit_Copy.Size = new System.Drawing.Size(164, 22);
			this.cellit_Copy.Text = "copy";
			this.cellit_Copy.Click += new System.EventHandler(this.cellclick_Copy);
			// 
			// cellit_Paste
			// 
			this.cellit_Paste.Name = "cellit_Paste";
			this.cellit_Paste.Size = new System.Drawing.Size(164, 22);
			this.cellit_Paste.Text = "paste";
			this.cellit_Paste.Click += new System.EventHandler(this.cellclick_Paste);
			// 
			// cellit_Clear
			// 
			this.cellit_Clear.Name = "cellit_Clear";
			this.cellit_Clear.Size = new System.Drawing.Size(164, 22);
			this.cellit_Clear.Text = "clear";
			this.cellit_Clear.Click += new System.EventHandler(this.cellclick_Delete);
			// 
			// separator_16
			// 
			this.separator_16.Name = "separator_16";
			this.separator_16.Size = new System.Drawing.Size(161, 6);
			// 
			// cellit_Lower
			// 
			this.cellit_Lower.Name = "cellit_Lower";
			this.cellit_Lower.Size = new System.Drawing.Size(164, 22);
			this.cellit_Lower.Text = "lowercase";
			this.cellit_Lower.Click += new System.EventHandler(this.cellclick_Lower);
			// 
			// cellit_Upper
			// 
			this.cellit_Upper.Name = "cellit_Upper";
			this.cellit_Upper.Size = new System.Drawing.Size(164, 22);
			this.cellit_Upper.Text = "uppercase";
			this.cellit_Upper.Click += new System.EventHandler(this.cellclick_Upper);
			// 
			// separator_24
			// 
			this.separator_24.Name = "separator_24";
			this.separator_24.Size = new System.Drawing.Size(161, 6);
			// 
			// cellit_MergeCe
			// 
			this.cellit_MergeCe.Name = "cellit_MergeCe";
			this.cellit_MergeCe.Size = new System.Drawing.Size(164, 22);
			this.cellit_MergeCe.Text = "merge to other - Ce";
			this.cellit_MergeCe.Click += new System.EventHandler(this.cellclick_MergeCe);
			// 
			// cellit_MergeRo
			// 
			this.cellit_MergeRo.Name = "cellit_MergeRo";
			this.cellit_MergeRo.Size = new System.Drawing.Size(164, 22);
			this.cellit_MergeRo.Text = "merge to other - Ro";
			this.cellit_MergeRo.Click += new System.EventHandler(this.cellclick_MergeRo);
			// 
			// separator_28
			// 
			this.separator_28.Name = "separator_28";
			this.separator_28.Size = new System.Drawing.Size(161, 6);
			// 
			// cellit_Strref
			// 
			this.cellit_Strref.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cellit_Strref_talktable,
			this.cellit_Strref_custom,
			this.cellit_Strref_invalid});
			this.cellit_Strref.Name = "cellit_Strref";
			this.cellit_Strref.Size = new System.Drawing.Size(164, 22);
			this.cellit_Strref.Text = "STRREF";
			this.cellit_Strref.DropDownOpening += new System.EventHandler(this.dropdownopening_Strref);
			// 
			// cellit_Strref_talktable
			// 
			this.cellit_Strref_talktable.Name = "cellit_Strref_talktable";
			this.cellit_Strref_talktable.Size = new System.Drawing.Size(160, 22);
			this.cellit_Strref_talktable.Text = "TalkTable";
			this.cellit_Strref_talktable.Click += new System.EventHandler(this.cellclick_Strref_talktable);
			// 
			// cellit_Strref_custom
			// 
			this.cellit_Strref_custom.Name = "cellit_Strref_custom";
			this.cellit_Strref_custom.Size = new System.Drawing.Size(160, 22);
			this.cellit_Strref_custom.Text = "set Custom";
			this.cellit_Strref_custom.Click += new System.EventHandler(this.cellclick_Strref_custom);
			// 
			// cellit_Strref_invalid
			// 
			this.cellit_Strref_invalid.Name = "cellit_Strref_invalid";
			this.cellit_Strref_invalid.Size = new System.Drawing.Size(160, 22);
			this.cellit_Strref_invalid.Text = "set Invalid (-1)";
			this.cellit_Strref_invalid.Click += new System.EventHandler(this.cellclick_Strref_invalid);
			// 
			// cellit_Input
			// 
			this.cellit_Input.Name = "cellit_Input";
			this.cellit_Input.Size = new System.Drawing.Size(164, 22);
			this.cellit_Input.Text = "InfoInput";
			this.cellit_Input.Click += new System.EventHandler(this.cellclick_InfoInput);
			// 
			// ContextTab
			// 
			this.ContextTab.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ContextTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tabit_Close,
			this.tabit_CloseAll,
			this.tabit_CloseAllOthers,
			this.separator_14,
			this.tabit_Save,
			this.separator_26,
			this.tabit_Reload,
			this.separator_23,
			this.tabit_Diff1,
			this.tabit_Diff2,
			this.tabit_DiffReset,
			this.tabit_DiffSync});
			this.ContextTab.Name = "ContextTab";
			this.ContextTab.ShowImageMargin = false;
			this.ContextTab.Size = new System.Drawing.Size(139, 220);
			this.ContextTab.Opening += new System.ComponentModel.CancelEventHandler(this.ContextTab_opening);
			// 
			// tabit_Close
			// 
			this.tabit_Close.Name = "tabit_Close";
			this.tabit_Close.Size = new System.Drawing.Size(138, 22);
			this.tabit_Close.Text = "Close";
			this.tabit_Close.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tabit_Close.Click += new System.EventHandler(this.fileclick_ClosePage);
			// 
			// tabit_CloseAll
			// 
			this.tabit_CloseAll.Name = "tabit_CloseAll";
			this.tabit_CloseAll.Size = new System.Drawing.Size(138, 22);
			this.tabit_CloseAll.Text = "Close all";
			this.tabit_CloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tabit_CloseAll.Click += new System.EventHandler(this.fileclick_CloseAllTabs);
			// 
			// tabit_CloseAllOthers
			// 
			this.tabit_CloseAllOthers.Name = "tabit_CloseAllOthers";
			this.tabit_CloseAllOthers.Size = new System.Drawing.Size(138, 22);
			this.tabit_CloseAllOthers.Text = "Close all others";
			this.tabit_CloseAllOthers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tabit_CloseAllOthers.Click += new System.EventHandler(this.tabclick_CloseAllOtherTabs);
			// 
			// separator_14
			// 
			this.separator_14.Name = "separator_14";
			this.separator_14.Size = new System.Drawing.Size(135, 6);
			// 
			// tabit_Save
			// 
			this.tabit_Save.Name = "tabit_Save";
			this.tabit_Save.Size = new System.Drawing.Size(138, 22);
			this.tabit_Save.Text = "Save";
			this.tabit_Save.Click += new System.EventHandler(this.fileclick_Save);
			// 
			// separator_26
			// 
			this.separator_26.Name = "separator_26";
			this.separator_26.Size = new System.Drawing.Size(135, 6);
			// 
			// tabit_Reload
			// 
			this.tabit_Reload.Name = "tabit_Reload";
			this.tabit_Reload.Size = new System.Drawing.Size(138, 22);
			this.tabit_Reload.Text = "Reload";
			this.tabit_Reload.Click += new System.EventHandler(this.fileclick_Reload);
			// 
			// separator_23
			// 
			this.separator_23.Name = "separator_23";
			this.separator_23.Size = new System.Drawing.Size(135, 6);
			// 
			// tabit_Diff1
			// 
			this.tabit_Diff1.Name = "tabit_Diff1";
			this.tabit_Diff1.Size = new System.Drawing.Size(138, 22);
			this.tabit_Diff1.Text = "Select diff1";
			this.tabit_Diff1.Click += new System.EventHandler(this.tabclick_Diff1);
			// 
			// tabit_Diff2
			// 
			this.tabit_Diff2.Enabled = false;
			this.tabit_Diff2.Name = "tabit_Diff2";
			this.tabit_Diff2.Size = new System.Drawing.Size(138, 22);
			this.tabit_Diff2.Text = "Select diff2";
			this.tabit_Diff2.Click += new System.EventHandler(this.tabclick_Diff2);
			// 
			// tabit_DiffReset
			// 
			this.tabit_DiffReset.Enabled = false;
			this.tabit_DiffReset.Name = "tabit_DiffReset";
			this.tabit_DiffReset.Size = new System.Drawing.Size(138, 22);
			this.tabit_DiffReset.Text = "Reset diffs";
			this.tabit_DiffReset.Click += new System.EventHandler(this.tabclick_DiffReset);
			// 
			// tabit_DiffSync
			// 
			this.tabit_DiffSync.Enabled = false;
			this.tabit_DiffSync.Name = "tabit_DiffSync";
			this.tabit_DiffSync.Size = new System.Drawing.Size(138, 22);
			this.tabit_DiffSync.Text = "Sync tables";
			this.tabit_DiffSync.Click += new System.EventHandler(this.tabclick_DiffSync);
			// 
			// YataForm
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(867, 499);
			this.Controls.Add(this.panel_ColorFill);
			this.Controls.Add(this.menubar);
			this.Controls.Add(this.statusbar);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menubar;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "YataForm";
			this.Text = " Yata";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.yata_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.yata_DragEnter);
			this.menubar.ResumeLayout(false);
			this.menubar.PerformLayout();
			this.ContextRow.ResumeLayout(false);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.ContextCell.ResumeLayout(false);
			this.ContextTab.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
