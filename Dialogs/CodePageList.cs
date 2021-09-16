using System;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed class CodePageList
		: YataDialog
	{
		#region cTor
		/// <summary>
		/// A window that lists all codepages that are recognized by the user's
		/// OS.
		/// </summary>
		/// <param name="f"><c><see cref="CodePageDialog"/></c></param>
		/// <remarks>This dialog shall be allowed to remain open even if its
		/// caller <c>CodePageDialog</c> is closed.</remarks>
		internal CodePageList(CodePageDialog f)
		{
			// WARNING: CodePageList will hold a pointer to CodePageDialog even
			// if CodePageDialog is closed. The pointer will be released when
			// CodePageList is closed also.
			_f = f;

			InitializeComponent();
			Initialize(tb_Codepages);

			var sb = new StringBuilder();

			EncodingInfo[] encs = Encoding.GetEncodings();
			for (int i = 0; i != encs.Length; ++i)
			{
				if (i != 0) sb.AppendLine();

				sb.AppendLine(encs[i].Name);
				sb.AppendLine(encs[i].DisplayName);
				sb.AppendLine(encs[i].CodePage.ToString());
			}
			tb_Codepages.Text = sb.ToString();

			Show(); // no owner.
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Handles the <c>FormClosing</c> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as CodePageDialog).CloseCodepageList();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Closes this <c>CodePageList</c> when <c>[Esc]</c> is pressed.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Do not use <c>KeyDown</c> because the event will repeat
		/// even if told to suppress itself here and
		/// <c><see cref="CodePageDialog"/></c> would then close also.
		/// 
		/// 
		/// Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)



		#region Designer
		TextBox tb_Codepages;

		void InitializeComponent()
		{
			this.tb_Codepages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_Codepages
			// 
			this.tb_Codepages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_Codepages.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_Codepages.Location = new System.Drawing.Point(0, 0);
			this.tb_Codepages.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Codepages.Multiline = true;
			this.tb_Codepages.Name = "tb_Codepages";
			this.tb_Codepages.ReadOnly = true;
			this.tb_Codepages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tb_Codepages.Size = new System.Drawing.Size(312, 429);
			this.tb_Codepages.TabIndex = 0;
			this.tb_Codepages.WordWrap = false;
			// 
			// CodePageList
			// 
			this.ClientSize = new System.Drawing.Size(312, 429);
			this.Controls.Add(this.tb_Codepages);
			this.Icon = global::yata.Properties.Resources.yata_icon;
			this.KeyPreview = true;
			this.Name = "CodePageList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = " yata - .net Codepages";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
