using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;


namespace yata
{
	sealed class InfoHexDialog
		:
			Form
	{
		#region Fields
		YataForm _f;
		YataGrid _grid;
		Cell _cell;
		#endregion Fields


		#region cTor
		internal InfoHexDialog(YataGrid grid)
		{
			InitializeComponent();

			_grid =  grid;
			_f    = _grid._f;
			_cell = _grid.getSelectedCell();

			if (Settings._font2 != null)
				Font = Settings._font2;
			else
				Font = _f.Font;

			texts();
		}
		#endregion cTor


		#region Methods
		void texts()
		{
			string val = _cell.text;
			if (!String.IsNullOrEmpty(val))
			{
				switch (_cell.x)
				{
					case 8: // "TargetType"
						//internal const int TARGET_NONE       = 0x00; //  0
						//internal const int TARGET_SELF       = 0x01; //  1
						//internal const int TARGET_CREATURE   = 0x02; //  2
						//internal const int TARGET_GROUND     = 0x04; //  4
						//internal const int TARGET_ITEMS      = 0x08; //  8
						//internal const int TARGET_DOORS      = 0x10; // 16
						//internal const int TARGET_PLACEABLES = 0x20; // 32
						//internal const int TARGET_TRIGGERS   = 0x40; // 64

						cb_0.Text = "(1)Self";
						cb_1.Text = "(2)Creatures";
						cb_2.Text = "(4)Ground";
						cb_3.Text = "(8)Items";
						cb_4.Text = "(16)Doors";
						cb_5.Text = "(32)Placeables";
						cb_6.Text = "(64)Triggers";
						cb_7.Text = "";

						int result;
						if (Int32.TryParse(val.Substring(2),
										   NumberStyles.AllowHexSpecifier,
										   CultureInfo.InvariantCulture,
										   out result))
						{
							cb_0.Checked = ((result & YataForm.TARGET_SELF)       != 0);
							cb_1.Checked = ((result & YataForm.TARGET_CREATURE)   != 0);
							cb_2.Checked = ((result & YataForm.TARGET_GROUND)     != 0);
							cb_3.Checked = ((result & YataForm.TARGET_ITEMS)      != 0);
							cb_4.Checked = ((result & YataForm.TARGET_DOORS)      != 0);
							cb_5.Checked = ((result & YataForm.TARGET_PLACEABLES) != 0);
							cb_6.Checked = ((result & YataForm.TARGET_TRIGGERS)   != 0);
						}
						else
						{
							cb_0.Checked =
							cb_1.Checked =
							cb_2.Checked =
							cb_3.Checked =
							cb_4.Checked =
							cb_5.Checked =
							cb_6.Checked = false;
						}

						cb_7.Checked =
						cb_7.Enabled = false;

						_f._original = result;
						lbl_Val.Text = "0x" + result.ToString("X2");
						break;
				}
			}
		}
		#endregion Methods


		#region Events (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
			else
				base.OnKeyDown(e);
		}
		#endregion Events (override)


		#region Events
		//internal const int TARGET_NONE       = 0x00; //  0
		//internal const int TARGET_SELF       = 0x01; //  1
		//internal const int TARGET_CREATURE   = 0x02; //  2
		//internal const int TARGET_GROUND     = 0x04; //  4
		//internal const int TARGET_ITEMS      = 0x08; //  8
		//internal const int TARGET_DOORS      = 0x10; // 16
		//internal const int TARGET_PLACEABLES = 0x20; // 32
		//internal const int TARGET_TRIGGERS   = 0x40; // 64

		void changed(object sender, EventArgs e)
		{
			var cb = sender as CheckBox;
			if (cb == cb_0)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_SELF;
				else            _grid._f._input &= ~YataForm.TARGET_SELF;
			}
			else if (cb == cb_1)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_CREATURE;
				else            _grid._f._input &= ~YataForm.TARGET_CREATURE;
			}
			else if (cb == cb_2)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_GROUND;
				else            _grid._f._input &= ~YataForm.TARGET_GROUND;
			}
			else if (cb == cb_3)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_ITEMS;
				else            _grid._f._input &= ~YataForm.TARGET_ITEMS;
			}
			else if (cb == cb_4)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_DOORS;
				else            _grid._f._input &= ~YataForm.TARGET_DOORS;
			}
			else if (cb == cb_5)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_PLACEABLES;
				else            _grid._f._input &= ~YataForm.TARGET_PLACEABLES;
			}
			else if (cb == cb_6)
			{
				if (cb.Checked) _grid._f._input |=  YataForm.TARGET_TRIGGERS;
				else            _grid._f._input &= ~YataForm.TARGET_TRIGGERS;
			}

			lbl_Val.Text = "0x" + _grid._f._input.ToString("X2");
		}

/*		void click_Accept(object sender, EventArgs e)
		{} */
		#endregion Events


		#region Windows Form Designer generated code
		Container components = null;

		CheckBox cb_0;
		CheckBox cb_1;
		CheckBox cb_2;
		CheckBox cb_3;
		CheckBox cb_4;
		CheckBox cb_5;
		CheckBox cb_6;
		CheckBox cb_7;
		Button btn_Accept;
		private System.Windows.Forms.Label lbl_Val;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify the contents of
		/// this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cb_0 = new System.Windows.Forms.CheckBox();
			this.cb_1 = new System.Windows.Forms.CheckBox();
			this.cb_2 = new System.Windows.Forms.CheckBox();
			this.cb_3 = new System.Windows.Forms.CheckBox();
			this.cb_4 = new System.Windows.Forms.CheckBox();
			this.cb_5 = new System.Windows.Forms.CheckBox();
			this.cb_6 = new System.Windows.Forms.CheckBox();
			this.cb_7 = new System.Windows.Forms.CheckBox();
			this.btn_Accept = new System.Windows.Forms.Button();
			this.lbl_Val = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cb_0
			// 
			this.cb_0.Location = new System.Drawing.Point(5, 25);
			this.cb_0.Margin = new System.Windows.Forms.Padding(0);
			this.cb_0.Name = "cb_0";
			this.cb_0.Size = new System.Drawing.Size(115, 20);
			this.cb_0.TabIndex = 0;
			this.cb_0.Text = "cb_0";
			this.cb_0.UseVisualStyleBackColor = true;
			this.cb_0.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_1
			// 
			this.cb_1.Location = new System.Drawing.Point(5, 45);
			this.cb_1.Margin = new System.Windows.Forms.Padding(0);
			this.cb_1.Name = "cb_1";
			this.cb_1.Size = new System.Drawing.Size(115, 20);
			this.cb_1.TabIndex = 1;
			this.cb_1.Text = "cb_1";
			this.cb_1.UseVisualStyleBackColor = true;
			this.cb_1.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_2
			// 
			this.cb_2.Location = new System.Drawing.Point(5, 65);
			this.cb_2.Margin = new System.Windows.Forms.Padding(0);
			this.cb_2.Name = "cb_2";
			this.cb_2.Size = new System.Drawing.Size(115, 20);
			this.cb_2.TabIndex = 2;
			this.cb_2.Text = "cb_2";
			this.cb_2.UseVisualStyleBackColor = true;
			this.cb_2.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_3
			// 
			this.cb_3.Location = new System.Drawing.Point(5, 85);
			this.cb_3.Margin = new System.Windows.Forms.Padding(0);
			this.cb_3.Name = "cb_3";
			this.cb_3.Size = new System.Drawing.Size(115, 20);
			this.cb_3.TabIndex = 3;
			this.cb_3.Text = "cb_3";
			this.cb_3.UseVisualStyleBackColor = true;
			this.cb_3.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_4
			// 
			this.cb_4.Location = new System.Drawing.Point(5, 105);
			this.cb_4.Margin = new System.Windows.Forms.Padding(0);
			this.cb_4.Name = "cb_4";
			this.cb_4.Size = new System.Drawing.Size(115, 20);
			this.cb_4.TabIndex = 4;
			this.cb_4.Text = "cb_4";
			this.cb_4.UseVisualStyleBackColor = true;
			this.cb_4.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_5
			// 
			this.cb_5.Location = new System.Drawing.Point(5, 125);
			this.cb_5.Margin = new System.Windows.Forms.Padding(0);
			this.cb_5.Name = "cb_5";
			this.cb_5.Size = new System.Drawing.Size(115, 20);
			this.cb_5.TabIndex = 5;
			this.cb_5.Text = "cb_5";
			this.cb_5.UseVisualStyleBackColor = true;
			this.cb_5.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_6
			// 
			this.cb_6.Location = new System.Drawing.Point(5, 145);
			this.cb_6.Margin = new System.Windows.Forms.Padding(0);
			this.cb_6.Name = "cb_6";
			this.cb_6.Size = new System.Drawing.Size(115, 20);
			this.cb_6.TabIndex = 6;
			this.cb_6.Text = "cb_6";
			this.cb_6.UseVisualStyleBackColor = true;
			this.cb_6.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// cb_7
			// 
			this.cb_7.Location = new System.Drawing.Point(5, 165);
			this.cb_7.Margin = new System.Windows.Forms.Padding(0);
			this.cb_7.Name = "cb_7";
			this.cb_7.Size = new System.Drawing.Size(115, 20);
			this.cb_7.TabIndex = 7;
			this.cb_7.Text = "cb_7";
			this.cb_7.UseVisualStyleBackColor = true;
			this.cb_7.CheckedChanged += new System.EventHandler(this.changed);
			// 
			// btn_Accept
			// 
			this.btn_Accept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.btn_Accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_Accept.Location = new System.Drawing.Point(5, 190);
			this.btn_Accept.Margin = new System.Windows.Forms.Padding(0);
			this.btn_Accept.Name = "btn_Accept";
			this.btn_Accept.Size = new System.Drawing.Size(115, 25);
			this.btn_Accept.TabIndex = 8;
			this.btn_Accept.Text = "accept";
			this.btn_Accept.UseVisualStyleBackColor = true;
			// 
			// lbl_Val
			// 
			this.lbl_Val.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lbl_Val.Location = new System.Drawing.Point(5, 5);
			this.lbl_Val.Margin = new System.Windows.Forms.Padding(0);
			this.lbl_Val.Name = "lbl_Val";
			this.lbl_Val.Size = new System.Drawing.Size(115, 15);
			this.lbl_Val.TabIndex = 9;
			this.lbl_Val.Text = "lbl_Val";
			this.lbl_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// InfoHexDialog
			// 
			this.AcceptButton = this.btn_Accept;
			this.ClientSize = new System.Drawing.Size(124, 216);
			this.Controls.Add(this.lbl_Val);
			this.Controls.Add(this.btn_Accept);
			this.Controls.Add(this.cb_7);
			this.Controls.Add(this.cb_6);
			this.Controls.Add(this.cb_5);
			this.Controls.Add(this.cb_4);
			this.Controls.Add(this.cb_3);
			this.Controls.Add(this.cb_2);
			this.Controls.Add(this.cb_1);
			this.Controls.Add(this.cb_0);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoHexDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
		#endregion
	}
}
