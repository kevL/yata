using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace yata
{
	sealed partial class DataZipListDialog
		: YataDialog
	{
		#region Fields (static)
		const string TitlePrefi = " yata - ";
//		const string TitleDeflt = "Data/zip list";

		static string _filter = String.Empty;

		const int marginhori_button_outer =  6;
		const int marginhori_button_inner = 10;
		const int marginvert_list = 4;
		const int marginvert_butt = 6;
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// File+extension of the currently opened Zipfile.
		/// </summary>
		string _zipfe;

		/// <summary>
		/// The cache of all file-labels in the currently opened Zipfile.
		/// </summary>
		readonly List<string> _filelist = new List<string>();

		/// <summary>
		/// Tracks the top-id in the file-list.
		/// </summary>
		int _tid = -1;

		readonly Timer _t1 = new Timer();
		#endregion Fields


		#region cTor
		/// <summary>
		/// Instantiates this <c><see cref="YataDialog"/></c>.
		/// </summary>
		/// <param name="f"></param>
		/// <param name="pfe"></param>
		internal DataZipListDialog(Yata f, string pfe)
		{
			_f = f;

			InitializeComponent();

			int wMin = bu_Load.Width + bu_Accept.Width + bu_Cancel.Width
					 + marginhori_button_outer + marginhori_button_inner
					 + Width - ClientSize.Width;
			int hMin = la_Filter.Height + lb_List.ItemHeight * 3 + bu_Load.Height
					 + marginvert_list + marginvert_butt
					 + Height - ClientSize.Height;
			MinimumSize = new Size(wMin, hMin);

			Initialize(METRIC_FUL);

			// cf. Settings.SetFonts() ->
			if (Settings._fontf != null)
			{
				lb_List.Font.Dispose();
				lb_List.Font = Settings._fontf;
			}
			lb_List.BackColor = Colors.TextboxBackground;


			using (var fs = new FileStream(pfe, FileMode.Open, FileAccess.Read))
			using (var zf = new ZipFile(fs))
			{
				ZipEntry[] entries = zf.GetEntries();
				foreach (var ze in entries)
				{
					string label = Path.GetFileNameWithoutExtension(ze.Label);
					if (label.Length != 0 && !_filelist.Contains(label))
					{
						_filelist.Add(label);
						lb_List.Items.Add(label);
					}
				}
			}

			_zipfe = Path.GetFileName(pfe);

//			if (_filelist.Count != 0) // TODO: is not robust - Zip could have no entries eg, Music_X1.zip
			Text = TitlePrefi + _zipfe;
//			else
//				Text = TitlePrefi + TitleDeflt;

			tb_Filter.Text = _filter;

			_t1.Tick += t1_tick;
			_t1.Interval = 100;
			_t1.Start();
		}
		#endregion cTor


		#region Handlers (override)
		/// <summary>
		/// Overrides the <c>FormClosing</c> handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_t1.Dispose();
			base.OnFormClosing(e);
		}

		/// <summary>
		/// Overrides the <c>Resize</c> handler. Restores
		/// <c><see cref="_tid"/></c>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				lb_List.BeginUpdate();
				base.OnResize(e);

				if (_tid != -1) lb_List.TopIndex = _tid;
				lb_List.EndUpdate();
			}
			else
				base.OnResize(e);
		}
		#endregion Handlers (override)


		#region Handlers
		/// <summary>
		/// Determines if the Accept button should be enabled.
		/// </summary>
		/// <param name="sender"><c><see cref="lb_List"/></c></param>
		/// <param name="e"></param>
		void selectedindexchanged_list(object sender, EventArgs e)
		{
			bu_Accept.Enabled = lb_List.SelectedIndex != -1;
		}

		/// <summary>
		/// Accepts <c>SelectedItem</c> in <c><see cref="lb_List"/></c> if
		/// valid.
		/// </summary>
		/// <param name="sender"><c><see cref="lb_List"/></c></param>
		/// <param name="e"></param>
		void doubleclick_list(object sender, EventArgs e)
		{
			if (lb_List.SelectedIndex != -1)
				DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Updates the displayed file-list wrt <c><see cref="_filter"/></c>.
		/// </summary>
		/// <param name="sender"><c><see cref="tb_Filter"/></c></param>
		/// <param name="e"></param>
		void textchanged_filter(object sender, EventArgs e)
		{
			lb_List.BeginUpdate();

			_filter = tb_Filter.Text;

			lb_List.Items.Clear();
			foreach (var file in _filelist)
			{
				if (file.IndexOf(_filter, StringComparison.InvariantCultureIgnoreCase) != -1)
					lb_List.Items.Add(file);
			}

			_tid = -1;
			bu_Accept.Enabled = false;

			lb_List.EndUpdate();
		}

		/// <summary>
		/// Tracks the top-id.
		/// </summary>
		/// <param name="sender"><c><see cref="_t1"/></c></param>
		/// <param name="e"></param>
		void t1_tick(object sender, EventArgs e)
		{
			if (lb_List.Items.Count != 0)
				_tid = lb_List.TopIndex;
			else
				_tid = -1;
		}

		/// <summary>
		/// Invokes an <c>OpenFileDialog</c> for the user to open a different
		/// zip-archive.
		/// </summary>
		/// <param name="sender"><c><see cref="bu_Load"/></c></param>
		/// <param name="e"></param>
		void click_Load(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Title  = " Open Data/zip";
				ofd.Filter = Yata.GetFileFilter("zip");

				ofd.FileName = _zipfe;
				ofd.AutoUpgradeEnabled = false;


				if (ofd.ShowDialog() == DialogResult.OK)
				{
					lb_List.BeginUpdate();

					_filelist.Clear();
					lb_List.Items.Clear();

					using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
					using (var zf = new ZipFile(fs))
					{
						ZipEntry[] entries = zf.GetEntries();
						foreach (var ze in entries)
						{
							string label = Path.GetFileNameWithoutExtension(ze.Label);
							if (label.Length != 0 && !_filelist.Contains(label))
							{
								_filelist.Add(label);

								if (label.IndexOf(_filter, StringComparison.InvariantCultureIgnoreCase) != -1)
									lb_List.Items.Add(label);
							}
						}
					}

					_zipfe = Path.GetFileName(ofd.FileName);

//					if (_filelist.Count != 0) // TODO: is not robust - Zip could have no entries eg, Music_X1.zip
					Text = TitlePrefi + _zipfe;
//					else
//						Text = TitlePrefi + TitleDeflt;

					_tid = -1;
					bu_Accept.Enabled = false;

					lb_List.EndUpdate();
				}
			}
		}
		#endregion Handlers


		#region Methods
		/// <summary>
		/// Gets the <c>SelectedItem</c> in <c><see cref="lb_List"/></c> as a
		/// <c>string</c>.
		/// </summary>
		/// <returns></returns>
		internal string GetSelectedFile()
		{
			return lb_List.SelectedItem as string;
		}
		#endregion Methods
	}
}
