using System;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	/// <summary>
	/// Creates an object that watches for external/disk file-changed events.
	/// </summary>
	/// <remarks>Call <c>Dispose()</c> when it goes out of scope. See
	/// <c><see cref="YataForm"/>.ClosePage()</c>.</remarks>
	sealed class FileWatcher
		: Timer
	{
		#region Fields
		readonly YataGrid _grid;
		DateTime _last;
		#endregion Fields


		#region Properties
		internal string Fullpath
		{ private get; set; }

		internal bool BypassFileChanged
		{ private get; set; }

		internal bool BypassFileDeleted
		{ private get; set; }

		/// <summary>
		/// Forces user to reload the 2da-file after a load-sequence finishes.
		/// </summary>
		/// <remarks>Do not show a <c><see cref="FileWatcherDialog"/></c> while
		/// loading a 2da-file. It creates issues if the user has that dialog
		/// open and decides to change or delete the 2da-file from the hardrive.</remarks>
		internal bool ForceReload
		{ get; set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		/// <param name="grid"></param>
		internal FileWatcher(YataGrid grid)
		{
			_grid = grid;
			Fullpath = _grid.Fullpath;
			_last = File.GetLastWriteTime(Fullpath);

			Interval = 300;
			Start();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Handels this <c>FileWatcher's</c> <c>Tick</c> event in C#.
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>Check for a valid <c><see cref="YataGrid"/></c> since
		/// disposal of this <c>FileWatcher</c> could be delayed. See
		/// <c><see cref="YataForm"/>.ClosePage()</c> where the grid is nulled.</remarks>
		protected override void OnTick(EventArgs e)
		{
			if (_grid != null) // ~safety.
			{
				if (!BypassFileDeleted)
				{
					if (!File.Exists(Fullpath))
					{
						BypassFileDeleted = true;

						if (YataGrid._init)
						{
							ForceReload = true;
						}
						else
						{
							using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_DEL))
								fwd.ShowDialog(_grid._f);
						}
					}
					else if (!BypassFileChanged
						&& File.GetLastWriteTime(Fullpath) != _last)
					{
						_last = File.GetLastWriteTime(Fullpath);

						if (YataGrid._init)
						{
							ForceReload = true;
						}
						else
						{
							using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_WSC))
								fwd.ShowDialog(_grid._f);
						}
					}
				}

				if (BypassFileChanged)
				{
					BypassFileChanged = false;
					_last = File.GetLastWriteTime(Fullpath);
				}
			}
		}
		#endregion Handlers (override)
	}
}
