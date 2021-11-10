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
						// TODO: Disable 'it_Reload' but reenable it if user chooses to re-save in FileWatcherDialog.

						BypassFileDeleted = true;

						using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_DEL))
							fwd.ShowDialog(_grid._f);
					}
					else if (!BypassFileChanged
						&& File.GetLastWriteTime(Fullpath) != _last)
					{
						_last = File.GetLastWriteTime(Fullpath);

						using (var fwd = new FileWatcherDialog(_grid, FileWatcherDialog.FILE_WSC))
							fwd.ShowDialog(_grid._f);
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
