using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace yata
{
	sealed partial class ClipboardEditor
		: YataDialog
	{
		#region Enums
		enum Current
		{
			Clipboard,
			RowsBuffer,
			ColBuffer,
			CellsBuffer
		}
		#endregion Enums


		#region Fields (static)
		static Current _current = Current.Clipboard;
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// Prevents <c><see cref="OnActivated()">OnActivated()</see></c> from
		/// doing anything unnecessary.
		/// </summary>
		bool _inited;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Instantiates Yata's clipboard dialog.
		/// </summary>
		internal ClipboardEditor(YataForm f)
		{
			_f = f;

			InitializeComponent();
			Initialize(YataDialog.METRIC_FUL);

			rtb_Clip.SelectionTabs = new int[] { 29, 58, 87, 116, 145, 174, 203, 232, 261, 290 };

			switch (_current)
			{
				case Current.Clipboard:   rb_Clipboard  .Checked = true; break;
				case Current.RowsBuffer:  rb_RowsBuffer .Checked = true; break;
				case Current.ColBuffer:   rb_ColBuffer  .Checked = true; break;
				case Current.CellsBuffer: rb_CellsBuffer.Checked = true; break;
			}

//			rtb_Clip.Select();
			bu_Begone.Select();

			Show(_f); // Yata is owner.
			_inited = true;
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			(_f as YataForm).CloseClipEditor();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Closes this <c>ClipboardEditor</c> on <c>[F11]</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Requires <c>KeyPreview</c> <c>true</c>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyData == Keys.F11)
			{
				e.SuppressKeyPress = true;
				Close();
			}
		}

		/// <summary>
		/// Gets the current Windows Clipboard text each time this
		/// <c>ClipboardEditor's</c> <c>Activated</c> event fires iff
		/// <c><see cref="rb_Clipboard"/></c> is currently checked.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnActivated(EventArgs e)
		{
			if (_inited && rb_Clipboard.Checked)
				rtb_Clip.Text = ClipboardService.GetText();
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Handles <c>Click</c> on the Get button.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="bu_Get"/></c></item>
		/// <item><c><see cref="YataForm"/>.it_ClipExport</c></item>
		/// </list></param>
		/// <param name="e"></param>
		/// <remarks>Invoked by
		/// <list type="bullet">
		/// <item>Get button</item>
		/// <item><c><see cref="YataForm"/>.clipclick_ExportCopy()</c></item>
		/// </list></remarks>
		internal void click_Get(object sender, EventArgs e)
		{
			if (rb_Clipboard.Checked)
				rtb_Clip.Text = ClipboardService.GetText();

//			rtb_Clip.Select();
		}

		/// <summary>
		/// Handles <c>Click</c> on the Set button.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Set"/></c></param>
		/// <param name="e"></param>
		void click_Set(object sender, EventArgs e)
		{
			if (rb_Clipboard.Checked)
				ClipboardService.SetText(rtb_Clip.Text.Replace("\n", Environment.NewLine));

//			rtb_Clip.Select();
		}


		/// <summary>
		/// Sets <c><see cref="_current"/></c> when a <c>RadioButton</c> fires
		/// <c>CheckedChanged</c>.
		/// </summary>
		/// <param name="sender">
		/// <list type="bullet">
		/// <item><c><see cref="rb_Clipboard"/></c></item>
		/// <item><c><see cref="rb_RowsBuffer"/></c></item>
		/// <item><c><see cref="rb_ColBuffer"/></c></item>
		/// <item><c><see cref="rb_CellsBuffer"/></c></item>
		/// </list></param>
		/// <param name="e"></param>
		void checkedchanged(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked)
			{
				if (sender == rb_Clipboard)
				{
					_current = Current.Clipboard;
					rtb_Clip.Text = ClipboardService.GetText();
				}
				else if (sender == rb_RowsBuffer)
				{
					_current = Current.RowsBuffer;
					SetRowsBufferText();
				}
				else if (sender == rb_ColBuffer)
				{
					_current = Current.ColBuffer;
					SetColBufferText();
				}
				else // sender == rb_CellsBuffer
				{
					_current = Current.CellsBuffer;
					SetCellsBufferText();
				}

				rtb_Clip.ReadOnly = sender != rb_Clipboard;

				bu_Get.Enabled =
				bu_Set.Enabled = sender == rb_Clipboard;
			}

//			rtb_Clip.Select();
		}


		/// <summary>
		/// Draws lovely lines on the top-panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_top"/></c></param>
		/// <param name="e"></param>
		void paint_Top(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, pa_top.Width, 0);
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, pa_top.Height);
		}

		/// <summary>
		/// Draws a lovely line on the bot-panel.
		/// </summary>
		/// <param name="sender"><c><see cref="pa_bot"/></c></param>
		/// <param name="e"></param>
		void paint_Bot(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, pa_bot.Height);
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		internal void SetRowsBufferText()
		{
			if (rb_RowsBuffer.Checked)
			{
				List<string[]> copyr = (_f as YataForm)._copyr;

				var sb = new StringBuilder();

				for (int i = 0; i != copyr.Count; ++i)
				{
					for (int j = 0; j != copyr[i].Length; ++j)
					{
						if (j != 0) sb.Append(' ');
						sb.Append(copyr[i][j]);
					}

					if (i != copyr.Count - 1) sb.AppendLine();
				}

				rtb_Clip.Text = sb.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		internal void SetColBufferText()
		{
			if (rb_ColBuffer.Checked)
			{
				List<string> copyc = (_f as YataForm)._copyc;

				var sb = new StringBuilder();

				for (int i = 0; i != copyc.Count; ++i)
				{
					sb.Append(copyc[i]);
					if (i != copyc.Count - 1) sb.AppendLine();
				}

				rtb_Clip.Text = sb.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		internal void SetCellsBufferText()
		{
			if (rb_CellsBuffer.Checked)
			{
				string[,] copyt = (_f as YataForm)._copytext;

				var sb = new StringBuilder();

				for (int i = 0; i != copyt.GetLength(0); ++i)
				{
					for (int j = 0; j != copyt.GetLength(1); ++j)
					{
						if (j != 0) sb.Append(' ');
						sb.Append(copyt[i,j]);
					}

					if (i != copyt.GetLength(0) - 1) sb.AppendLine();
				}

				rtb_Clip.Text = sb.ToString();
			}
		}
		#endregion Methods
	}
}
