using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// A panel for the col heads.
	/// </summary>
	sealed class YataPanelCols
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;

		/// <summary>
		/// The col-id of a current col-width grab.
		/// </summary>
		int _grabCol;

		/// <summary>
		/// The x-position of the cursor at the start of a col-width grab.
		/// </summary>
		int _grabPos;
		#endregion Fields


		#region Properties
		/// <summary>
		/// Tracks the state of the cursor wrt the col-width adjustor.
		/// <list type="bullet">
		/// <item><c>true</c> - <c>Cursors.VSplit</c></item>
		/// <item><c>false</c> - <c>!Cursors.VSplit</c></item>
		/// </list>
		/// </summary>
		/// <remarks><c>Cursors.Default</c> is the only other <c>Cursor</c> that
		/// is used by Yata at present.
		/// 
		/// 
		/// .net does not track the state of the cursor consistently with its
		/// visual state. So do not rely on checking the state of <c>Cursor</c>.
		/// The cursor's real state can flash briefly on a click.</remarks>
		internal bool IsCursorSplit
		{ private get; set; }

		/// <summary>
		/// <c>true</c> if user has the LMB depressed on the col-width adjustor.
		/// </summary>
		internal bool IsGrab
		{ get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal YataPanelCols(YataGrid grid)
		{
			DrawRegulator.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Top;
			BackColor = Colors.ColheadPanel;

			Height = YataGrid.HeightColhead;

			MouseDown += _grid.click_ColheadPanel;
		}
		#endregion cTor


		#region Handlers (override)
		static int _widthFrozenCached;

		/// <summary>
		/// Overrides the <c>Resize</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="eventargs"></param>
		protected override void OnResize(EventArgs eventargs)
		{
			if (!YataGrid._init)
			{
				_widthFrozenCached = YataGrid.WidthRowhead;
				for (int f = 0; f != _grid.FrozenCount; ++f)
					_widthFrozenCached += _grid.Cols[f].width();
			}

			if (Settings._gradient)
			{
				if (Gradients.ColheadPanel != null)
					Gradients.ColheadPanel.Dispose();

				Gradients.ColheadPanel = new LinearGradientBrush(new Point(0, 0),
																 new Point(0, Height),
																 Color.Lavender, Color.MediumOrchid);
			}
		}


		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> for
		/// <c><see cref="YataGrid.LabelColheads()">YataGrid.LabelColheads()</see></c>.</remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				if (Gradients.ColheadPanel != null) // Settings._gradient
				{
					var rect = new Rectangle(0,0, Width, Height);
					YataGrid.graphics.FillRectangle(Gradients.ColheadPanel, rect);
				}

				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   0,     Height,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width, 0,
										   Width, Height);
				_grid.LabelColheads();
			}
		}


		/// <summary>
		/// Overrides the <c>MouseMove</c> handler on this <c>YataPanelCols</c>.
		/// Changes cursor to a vertical splitter near the right edge of each
		/// unfrozen colhead.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (!IsGrab)
			{
				if (ModifierKeys == Keys.None)
				{
					int c = GetSplitterCol(e.X);
					if (c != -1)
					{
						Cursor = Cursors.VSplit;
						IsCursorSplit = true;
						_grabPos = e.X;
						_grabCol = c;

						return;
					}
				}

				Cursor = Cursors.Default;
				IsCursorSplit = false;
			}
		}

		/// <summary>
		/// Overrides the <c>MouseDown</c> handler on this <c>YataPanelCols</c>.
		/// Accepts or cancels the editor based on
		/// <c><see cref="Settings._acceptedit">Settings._acceptedit</see></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			logfile.Log("YataPanelCols.OnMouseDown()");

			// Don't bother checking MouseButton or ModifierKeys.

			if (_grid._editor.Visible)
			{
				logfile.Log(". _grid._editor.Visible");
				_grid._editor.Visible = false;
			}
			else if (_grid.Propanel != null && _grid.Propanel._editor.Visible)
			{
				logfile.Log(". _grid.Propanel._editor.Visible");
				_grid.Propanel._editor.Visible = false;
			}
			_grid.Select();


			switch (e.Button)
			{
				case MouseButtons.Left:
					IsGrab = ModifierKeys == Keys.None && IsCursorSplit;
					break;

				case MouseButtons.Right:
					if (ModifierKeys == Keys.None && IsCursorSplit)
					{
						Col col = _grid.Cols[_grabCol];
						if (col.UserSized)
						{
							col.UserSized = false;
							_grid.Colwidth(_grabCol);
							_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS);
						}
					}
					break;
			}

			base.OnMouseDown(e); // fire 'YataGrid.click_ColheadPanel()'
		}

		/// <summary>
		/// Overrides the <c>MouseUp</c> handler on this <c>YataPanelCols</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (IsGrab)
			{
				IsGrab = false;
				Cursor = Cursors.Default;
				IsCursorSplit = false;

				if (ModifierKeys == Keys.None
					&& e.Button == MouseButtons.Left
					&& e.X != _grabPos)
				{
					Col col = _grid.Cols[_grabCol];
					col.UserSized = true;

					int w = col.width() + e.X - _grabPos;
					if (w < YataGrid._wId) w = YataGrid._wId;

					col.width(w, true);
					_grid.InitScroll();
					_grid.Invalidator(YataGrid.INVALID_GRID | YataGrid.INVALID_COLS);
				}
			}
			_grid.Select();
		}
		#endregion Handlers (override)


		#region Methods
		/// <summary>
		/// Gets the col-id iff the cursor is on the col-width adjustor.
		/// </summary>
		/// <param name="cur">x-pos of cursor on this <c>YataPanelCols</c></param>
		/// <returns>col-id under the cursor or <c>-1</c></returns>
		internal int GetSplitterCol(int cur)
		{
			if (cur > _widthFrozenCached)
			{
				int x = YataGrid.WidthRowhead - _grid.OffsetHori;
				for (int c = 0; c != _grid.ColCount; ++c)
				{
					if ((x += _grid.Cols[c].width()) - 5 > cur)
						break;

					if (cur < x)
						return c;
				}
			}
			return -1;
		}
		#endregion Methods
	}



	/// <summary>
	/// A panel for the row heads.
	/// </summary>
	sealed class YataPanelRows
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal YataPanelRows(YataGrid grid)
		{
			DrawRegulator.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.RowheadPanel;

			MouseDown += _grid.click_RowheadPanel;
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelRows</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> for
		/// <c><see cref="YataGrid.LabelRowheads()">YataGrid.LabelRowheads()</see></c>.</remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width, 0,
										   Width, Height);
				YataGrid.graphics.DrawLine(Pencils.DarkLine,
										   Width - 1, 0,
										   Width - 1, Height);
				_grid.LabelRowheads();
			}
		}


		/// <summary>
		/// Overrides the <c>MouseDown</c> handler on this <c>YataPanelRows</c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			logfile.Log("YataPanelRows.OnMouseDown()");

			// Don't bother checking MouseButton or ModifierKeys.

			if (_grid._editor.Visible)
			{
				logfile.Log(". _grid._editor.Visible");
				_grid._editor.Visible = false;
			}
			else if (_grid.Propanel != null && _grid.Propanel._editor.Visible)
			{
				logfile.Log(". _grid.Propanel._editor.Visible");
				_grid.Propanel._editor.Visible = false;
			}
			_grid.Select();

			base.OnMouseDown(e); // fire 'YataGrid.click_RowheadPanel()'
		}
		#endregion Handlers (override)
	}



	/// <summary>
	/// A panel for the id-col and any frozen cols that follow it.
	/// </summary>
	sealed class YataPanelFrozen
		: Panel
	{
		#region Fields
		readonly YataGrid _grid;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="w"></param>
		internal YataPanelFrozen(YataGrid grid, int w)
		{
			DrawRegulator.SetDoubleBuffered(this);

			_grid = grid;

			Dock      = DockStyle.Left;
			BackColor = Colors.FrozenPanel;

			Width = w;
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>Paint</c> handler on this <c>YataPanelFrozen</c>.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Assigns <c>e.Graphics</c> to
		/// <c><see cref="YataGrid.graphics">YataGrid.graphics</see></c> for
		/// <c><see cref="YataGrid.PaintFrozenPanel()">YataGrid.PaintFrozenPanel()</see></c>.</remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!YataGrid._init)
			{
				YataGrid.graphics = e.Graphics;
				YataGrid.graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				_grid.PaintFrozenPanel();
			}
		}


		/// <summary>
		/// Overrides the <c>MouseDown</c> handler on this
		/// <c>YataPanelFrozen</c>. Accepts or cancels an active edit.
		/// <list type="bullet">
		/// <item><c>LMB</c> - accept edit</item>
		/// <item><c>RMB</c> - cancel edit</item>
		/// <item><c>MMB</c> - accept or cancel based on
		/// <c><see cref="Settings._acceptedit">Settings._acceptedit</see></c></item>
		/// </list>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			logfile.Log("YataPanelFrozen.OnMouseDown()");

			if (ModifierKeys == Keys.None)
			{
				if (_grid._editor.Visible)
				{
					logfile.Log(". _grid._editor.Visible");

					switch (e.Button)
					{
						case MouseButtons.Left: // accept edit
							_grid._bypassleaveditor = true;
							_grid.ApplyCellEdit(true);
							break;

						case MouseButtons.Right: // cancel edit
							_grid._bypassleaveditor = true;
							_grid.hideditor(YataGrid.INVALID_GRID, true);
							break;

						case MouseButtons.Middle:
							_grid._editor.Visible = false; // do this or else the leave event fires twice.
							_grid.Select();
							break;
					}
				}
				else
				{
					Propanel propanel = _grid.Propanel;
					if (propanel != null && propanel._editor.Visible)
					{
						logfile.Log(". _grid.Propanel._editor.Visible");

						switch (e.Button)
						{
							case MouseButtons.Left: // accept edit
								propanel._bypassleaveditor = true;
								propanel.ApplyCellEdit(true);
								break;

							case MouseButtons.Right: // cancel edit
								propanel._bypassleaveditor = true;
								propanel.hideditor(true);
								break;

							case MouseButtons.Middle:
								propanel._editor.Visible = false; // do this or else the leave event fires twice.
								_grid.Select();
								break;
						}
					}
				}
			}
		}
		#endregion Handlers (override)
	}
}
