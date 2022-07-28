using System;
using System.IO;
using System.Text;


namespace yata
{
	sealed class ZipFile
		: IDisposable
	{
		#region Fields (static)
		const long OffsetOfFirstEntry = 0;
		#endregion Fields (static)


		#region Fields
		readonly string _label;
		readonly Stream _file;

		ZipEntry[] _entries;
		#endregion Fields


		#region cTor
		/// <summary>
		/// And NwN2 Data compatible Zipfile.
		/// </summary>
		/// <param name="file"></param>
		internal ZipFile(FileStream file)
		{
			_file  = file;
			_label = file.Name;

			ReadEntries();
		}
		#endregion cTor


		#region Dispose
		bool _disposed;

		/// <summary>
		/// Closes this <c>ZipFile</c> and the underlying input stream.
		/// </summary>
		public void Dispose()
		{
//			logfile.Log("ZipFile.Dispose()");
//			logfile.Log("----");

			if (!_disposed)
			{
				_disposed = true;
				_entries = new ZipEntry[0];

				if (_file != null)
				{
					lock (_file)
					{
						_file.Dispose();
					}
				}
			}

			GC.SuppressFinalize(this);
		}
		#endregion Dispose


		#region Methods
		void ReadEntries()
		{
//			logfile.Log("ZipFile.ReadEntries()");

			// force Encoding ->
			Encoding enc = Encoding.GetEncoding(1252); // Western European (Windows)
//			Encoding enc = Encoding.GetEncoding(437);  // OEM United States
			if (enc == null) enc = Encoding.GetEncoding(0); // safety.

//			logfile.Log("system Enc= " + enc.EncodingName + " " + enc.CodePage);


			// Search for the End Of Central Directory. When a zip comment is
			// present the directory will start earlier
			//
			// The search is limited to 64K which is the maximum size of a
			// trailing comment field to aid speed. This should be compatible
			// with both SFX and ZIP files but has only been tested for Zip
			// files. If a SFX file has the Zip data attached as a resource and
			// there are other resources occurring later then this could be
			// invalid.
			//
			// Could also speed this up by reading memory in larger blocks.

			long locatedEndOfCentralDir = LocateEndOfCentralDirectory(); // not used. advances read-position.

			// read end of central directory record
			ushort thisDiskNumber            = _file.ReadLEUshort();
			ushort startCentralDirDisk       = _file.ReadLEUshort();
			ulong  entriesForThisDisk        = _file.ReadLEUshort();	//logfile.Log(". entriesForThisDisk= " + entriesForThisDisk);
			ulong  entriesForWholeCentralDir = _file.ReadLEUshort();
			ulong  centralDirSize            = _file.ReadLEUint();
			long   offsetOfCentralDir        = _file.ReadLEUint();

			_entries = new ZipEntry[entriesForThisDisk];

			_file.Seek(OffsetOfFirstEntry + offsetOfCentralDir, SeekOrigin.Begin);

			for (ulong i = 0; i < entriesForThisDisk; i++)
			{
//				logfile.Log("i= " + i);

				uint centralHeaderSignature = _file.ReadLEUint();	//logfile.Log(". centralHeaderSignature= " + centralHeaderSignature);
				int  versionMadeBy          = _file.ReadLEUshort();	//logfile.Log(". versionMadeBy= " + versionMadeBy);
				int  versionToExtract       = _file.ReadLEUshort();	//logfile.Log(". versionToExtract= " + versionToExtract);
				int  bitFlags               = _file.ReadLEUshort();	//logfile.Log(". bitFlags= " + bitFlags);
				int  method                 = _file.ReadLEUshort();	//logfile.Log(". method= " + method);
				uint dostime                = _file.ReadLEUint();	//logfile.Log(". dostime= " + dostime);
				uint crc                    = _file.ReadLEUint();	//logfile.Log(". crc= " + crc);
				long csize            = (long)_file.ReadLEUint();	//logfile.Log(". csize= " + csize);
				long size             = (long)_file.ReadLEUint();	//logfile.Log(". size= " + size);
				int  lenLabel               = _file.ReadLEUshort();	//logfile.Log(". lenLabel= " + lenLabel);
				int  lenExtra               = _file.ReadLEUshort();	//logfile.Log(". lenExtra= " + lenExtra);
				int  lenComment             = _file.ReadLEUshort();	//logfile.Log(". lenComment= " + lenComment);
				int  diskStartNo            = _file.ReadLEUshort();	//logfile.Log(". diskStartNo= " + diskStartNo);
				int  internalAttributes     = _file.ReadLEUshort();	//logfile.Log(". internalAttributes= " + internalAttributes);
				uint externalAttributes     = _file.ReadLEUint();	//logfile.Log(". externalAttributes= " + externalAttributes);
				long offset                 = _file.ReadLEUint();	//logfile.Log(". offset= " + offset);

				var buffer = new byte[Math.Max(lenLabel, lenComment)];
				ReadFully(_file, buffer, 0, lenLabel);
				string label = enc.GetString(buffer, 0, lenLabel);

				var entry = new ZipEntry(label, (Method)method)
				{
//					Crc                    = crc   & 0xffffffffL,
//					Size                   = size  & 0xffffffffL,
					CompressedSize         = csize & 0xffffffffL,
//					Flags                  = bitFlags,
//					DosTime                = dostime,
//					Id                     = (long)i,
					Offset                 = offset,
//					ExternalFileAttributes = (int)externalAttributes
				};
				_entries[i] = entry;
			}
		}

		/// <summary>
		/// Locates the byte after
		/// <c><see cref="ZipConstants.EndOfCentralDirectorySignature">ZipConstants.EndOfCentralDirectorySignature</see></c>.
		/// </summary>
		/// <returns>the offset of the first byte after the CentralDirectory
		/// else -1</returns>
		long LocateEndOfCentralDirectory()
		{
			long pos = _file.Length - ZipConstants.EndOfCentralRecordBaseSize;
			if (pos >= 0)
			{
				long giveUpMarker = Math.Max(0, pos - 0xffff);

				do
				{
					if (pos < giveUpMarker)
						return -1;

					_file.Seek(pos--, SeekOrigin.Begin);
				}
				while (_file.ReadLEInt() != ZipConstants.EndOfCentralDirectorySignature);

				return _file.Position;
			}
			return -1;
		}


		/// <summary>
		/// Searches for a <c><see cref="ZipEntry"/></c> in this archive with
		/// a specified <paramref name="label"/>.
		/// </summary>
		/// <param name="label">the label to find - could contain directory
		/// components separated by slashes</param>
		/// <returns>a clone of the <c>ZipEntry</c> or <c>null</c> if no entry
		/// with that label exists</returns>
		/// <remarks>String comparison is case insensitive.</remarks>
		internal ZipEntry GetEntry(string label)
		{
//			logfile.Log("ZipFile.GetEntry() label= " + label);

			int id;
			for (id = 0; id != _entries.Length; ++id)
			{
//				logfile.Log("_entries[" + id + "].Label= " + _entries[id].Label);

				if (_entries[id].Label.EndsWith(label, StringComparison.OrdinalIgnoreCase))
					break;
			}

			if (id != _entries.Length)
				return _entries[id].Clone() as ZipEntry;

			return null;
		}

		/// <summary>
		/// Gets an input stream for reading a specified
		/// <c><see cref="ZipEntry">ZipEntry's</see></c> data in uncompressed
		/// form.
		/// </summary>
		/// <param name="entry">the <c>ZipEntry</c> to obtain a data-stream for</param>
		/// <returns>an input <c><see cref="Stream"/></c> containing data for
		/// <paramref name="entry"/></returns>
		/// <remarks>Normally the <c>ZipEntry</c> should be passed by
		/// <c><see cref="GetEntry()">GetEntry()</see></c>.</remarks>
		internal Stream GetInputStream(ZipEntry entry)
		{
//			logfile.Log("ZipFile.GetInputStream() id= " + entry.ZipFileIndex);

			long start = GetEntryDataOffset(entry);
			Stream data = new PartialInputStream(this, start, entry.CompressedSize);

			if (entry.Method == Method.Deflated)
			{
				data = new InflatorInputStream(data, new Inflator(), 4096);
			}
			// else Method.Stored // the directory-label of the compressed files is stored uncompressed

			return data;
		}

		/// <summary>
		/// Gets the offset of a specified
		/// <c><see cref="ZipEntry">ZipEntry's</see></c> data in the file.
		/// </summary>
		/// <param name="entry">the entry to test against</param>
		/// <returns>the offset of the entry's data in the file</returns>
		long GetEntryDataOffset(ZipEntry entry)
		{
			lock (_file)
			{
				_file.Seek(OffsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);

				int   signature         =      (int)_file.ReadLEUint();
				short extractVersion    =   (short)(_file.ReadLEUshort() & 0x00ff);
				var   localFlags        = (Bitflags)_file.ReadLEUshort();
				var   compressionMethod =   (Method)_file.ReadLEUshort();
				short fileTime          =    (short)_file.ReadLEUshort();
				short fileDate          =    (short)_file.ReadLEUshort();
				uint  crcValue          =           _file.ReadLEUint();
				long  compressedSize    =     (long)_file.ReadLEUint();
				long  size              =     (long)_file.ReadLEUint();
				int   storedNameLength  =      (int)_file.ReadLEUshort();
				int   extraDataLength   =      (int)_file.ReadLEUshort();

				return OffsetOfFirstEntry
					 + entry.Offset
					 + ZipConstants.LocalHeaderBaseSize
					 + storedNameLength
					 + extraDataLength;
			}
		}
		#endregion Methods


		#region Methods (static)
		/// <summary>
		/// Read from a <c><see cref="Stream"/></c> ensuring all the required
		/// data is read.
		/// </summary>
		/// <param name="stream">the stream to read data from</param>
		/// <param name="buffer">the buffer to store data in</param>
		/// <param name="offset">the offset at which to begin storing data</param>
		/// <param name="count">the count of bytes of data to store</param>
		static void ReadFully(Stream stream, byte[] buffer, int offset, int count)
		{
			int read;
			while (count > 0)
			{
				if ((read = stream.Read(buffer, offset, count)) > 0)
				{
					offset += read;
					count  -= read;
				}
			}
		}
		#endregion Methods (static)


		#region PartialInputStream
		/// <summary>
		/// A <c><see cref="PartialInputStream"/></c> is a <c>Stream</c> whose
		/// data is only a part or subsection of a <c><see cref="ZipFile"/></c>.
		/// </summary>
		sealed class PartialInputStream
			: Stream
		{
			#region Fields
			readonly ZipFile _zipFile;
			readonly Stream _baseStream;

			readonly long _start;
			readonly long _end;
			#endregion Fields


			#region cTor
			/// <summary>
			/// Initializes a new instance of the
			/// <c><see cref="PartialInputStream"/></c> class.
			/// </summary>
			/// <param name="zipFile">the <c><see cref="ZipFile"/></c>
			/// containing the underlying stream to use for IO</param>
			/// <param name="start">the start of the partial data</param>
			/// <param name="length">the length of the partial data</param>
			internal PartialInputStream(ZipFile zipFile, long start, long length)
			{
				// Although this is the only time the zipfile is used
				// keeping a reference here prevents premature closure of
				// this zip file and thus the _baseStream.

				// Code like this will cause apparently random failures depending
				// on the size of the files and when garbage is collected.
				//
				// ZipFile zf = new ZipFile (stream);
				// Stream reader = zf.GetInputStream(0);
				// uses reader here....

				_baseStream = (_zipFile = zipFile)._file;
				_end = (_pos = _start = start) + (_length = length);
			}
			#endregion cTor


			#region Stream req.
			#region Properties (override)
			/// <summary>
			/// Gets a value indicating whether the current stream supports
			/// reading.
			/// </summary>
			/// <value><c>true</c></value>
			/// <returns><c>true</c> if the stream supports reading - which it
			/// does</returns>
			public override bool CanRead
			{
				get { return true; }
			}

			/// <summary>
			/// Gets a value indicating whether the current stream supports
			/// writing.
			/// </summary>
			/// <value><c>false</c></value>
			/// <returns><c>true</c> if the stream supports writing - which it
			/// doesn't</returns>
			public override bool CanWrite
			{
				get { return false; }
			}

			/// <summary>
			/// Gets a value indicating whether the current stream supports
			/// seeking.
			/// </summary>
			/// <value><c>true</c></value>
			/// <returns><c>true</c> if the stream supports seeking - which it
			/// does</returns>
			public override bool CanSeek
			{
				get { return true; }
			}

			readonly long _length;
			/// <summary>
			/// Gets the length in bytes of the stream.
			/// </summary>
			/// <returns>a <c>long</c> representing the length of the stream in
			/// bytes</returns>
			public override long Length
			{
				get { return _length; }
			}

			long _pos;
			/// <summary>
			/// Gets/sets the current position within the stream.
			/// </summary>
			/// <returns>the current position within the stream</returns>
			public override long Position
			{
				get { return _pos - _start; }
				set { _pos = _start + value; }
			}
			#endregion Properties (override)


			#region Methods (override)
			/// <summary>
			/// Reads a sequence of bytes from the stream and advances the
			/// position within the stream by the number of bytes read.
			/// </summary>
			/// <param name="buffer">an array of bytes. When this method returns
			/// the buffer contains the specified byte array with the values
			/// between offset and (offset + count - 1) replaced by the bytes
			/// read from the current source</param>
			/// <param name="offset">the zero-based byte offset in buffer at
			/// which to begin storing the data read from the current stream</param>
			/// <param name="count">the maximum number of bytes to be read from
			/// the current stream</param>
			/// <returns>the total number of bytes read into the buffer. This
			/// can be less than the number of bytes requested if that many
			/// bytes are not currently available or <c>0</c> if the end of the
			/// stream has been reached</returns>
			public override int Read(byte[] buffer, int offset, int count)
			{
				lock (_baseStream)
				{
					if (count > _end - _pos
						&& (count = (int)(_end - _pos)) == 0)
					{
						return 0;
					}

					// protect against Stream implementations that throw away
					// their buffer on every Seek (eg, Mono FileStream)
					if (_baseStream.Position != _pos)
						_baseStream.Seek(_pos, SeekOrigin.Begin);

					int readCount = _baseStream.Read(buffer, offset, count);
					if (readCount > 0)
						_pos += readCount;

					return readCount;
				}
			}

			/// <summary>
			/// When overridden in a derived class sets the position within the
			/// stream.
			/// </summary>
			/// <param name="offset">a byte offset relative to
			/// <paramref name="origin"/></param>
			/// <param name="origin">a value of type
			/// <see cref="System.IO.SeekOrigin"/> indicating the reference
			/// point used to obtain the new position</param>
			/// <returns>the new position within the current stream</returns>
			public override long Seek(long offset, SeekOrigin origin)
			{
				long pos = _pos;

				switch (origin)
				{
					case SeekOrigin.Begin:   pos = _start; break;
					case SeekOrigin.Current: pos = _pos;   break;
					case SeekOrigin.End:     pos = _end;   break;
				}
				pos += offset;

				return (_pos = pos);
			}

			/// <summary>
			/// Writes a sequence of bytes to the stream and advances the
			/// current position within this stream by the number of bytes
			/// written.
			/// </summary>
			/// <param name="buffer">an array of bytes. This method copies count
			/// bytes from buffer to the current stream</param>
			/// <param name="offset">the zero-based byte offset in buffer at
			/// which to begin copying bytes to the current stream</param>
			/// <param name="count">the number of bytes to be written to the
			/// stream.</param>
			public override void Write(byte[] buffer, int offset, int count)
			{}

			/// <summary>
			/// When overridden in a derived class sets the length of the
			/// stream.
			/// </summary>
			/// <param name="value">the desired length of the stream in bytes</param>
			public override void SetLength(long value)
			{}

			/// <summary>
			/// Clears all buffers for this stream and causes any buffered data
			/// to be written to the underlying device.
			/// </summary>
			public override void Flush()
			{}
			#endregion Methods (override)
			#endregion Stream req.
		}
		#endregion PartialInputStream
	}
}
