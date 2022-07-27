using System;
using System.IO;


namespace yata
{
	/// <summary>
	/// This filter stream is used to decompress data compressed using the
	/// deflate format. The deflate format is described in RFC 1951.
	/// </summary>
	/// <remarks>Author of the original java version: John Leuner</remarks>
	class InflatorInputStream
		: Stream
	{
		#region Fields
		/// <summary>
		/// Base stream the inflator reads from.
		/// </summary>
		readonly Stream _baseInputStream;

		/// <summary>
		/// Decompressor for this stream.
		/// </summary>
		readonly Inflator _inf;

		/// <summary>
		/// <c><see cref="InflatorInputBuffer"/></c> for this stream.
		/// </summary>
		readonly InflatorInputBuffer _inputBuffer;
		#endregion Fields


		#region cTor
		/// <summary>
		/// Creates an <c>InflatorInputStream</c> with the specified
		/// decompressor and the specified buffer size.
		/// </summary>
		/// <param name="baseInputStream">the stream to read bytes from</param>
		/// <param name="inf">the decompressor to use</param>
		/// <param name="bufferSize">size of the buffer to use</param>
		internal InflatorInputStream(Stream baseInputStream, Inflator inf, int bufferSize)
		{
			_baseInputStream = baseInputStream;
			_inf = inf;

			_inputBuffer = new InflatorInputBuffer(baseInputStream, bufferSize);
		}
		#endregion cTor


		/// <summary>
		/// Fills the buffer with more data to decompress.
		/// </summary>
		void Fill()
		{
			if (_inputBuffer.Available <= 0) // protect against redundant calls
				_inputBuffer.Fill();

			_inputBuffer.SetInflaterInput(_inf);
		}


		#region Stream req.
		#region Properties (override)
		/// <summary>
		/// Gets a value indicating whether the stream supports reading.
		/// </summary>
		public override bool CanRead
		{
			get { return _baseInputStream.CanRead; }
		}

		/// <summary>
		/// Gets a value of <c>false</c> indicating that this stream is not
		/// writeable.
		/// </summary>
		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value of <c>false</c> indicating seeking is not supported for
		/// this stream.
		/// </summary>
		public override bool CanSeek
		{
			get { return false; }
		}

		/// <summary>
		/// A value representing the length of the stream in bytes.
		/// </summary>
		public override long Length
		{
			get { return 0; } // not used.
		}

		long _pos;
		/// <summary>
		/// The current position within the stream.
		/// </summary>
		public override long Position
		{
			get { return _baseInputStream.Position; }
			set { _pos = value; } // not used.
		}
		#endregion Properties (override)


		#region Methods (override)
		/// <summary>
		/// Reads decompressed data into the provided buffer byte array.
		/// </summary>
		/// <param name="buffer">the array to read and decompress data into</param>
		/// <param name="offset">the offset indicating where the data should be
		/// placed</param>
		/// <param name="count">the number of bytes to decompress</param>
		/// <returns>the number of bytes read; <c>0</c> signals the end of
		/// stream</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesLeft = count;
			while (true)
			{
				int bytesRead = _inf.Inflate(buffer, offset, bytesLeft);
				offset    += bytesRead;
				bytesLeft -= bytesRead;

				if (bytesLeft == 0 || _inf.IsFinished)
					break;

				if (_inf.IsNeedingInput)
					Fill();
			}
			return count - bytesLeft;
		}

		/// <summary>
		/// not used.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public override void Write(byte[] buffer, int offset, int count)
		{}

		/// <summary>
		/// not used.
		/// </summary>
		/// <param name="value"></param>
		public override void SetLength(long value)
		{}

		/// <summary>
		/// not used.
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0;
		}

		/// <summary>
		/// Flushes <c><see cref="_baseInputStream"/></c>.
		/// </summary>
		public override void Flush()
		{
			_baseInputStream.Flush();
		}
		#endregion Methods (override)
		#endregion Stream req.


/*		/// <summary>
		/// Flag indicating whether this instance has been closed or not.
		/// </summary>
		private bool isClosed; */

/*		/// <summary>
		/// Closes the input stream. When <c><see cref="IsStreamOwner"/></c> is
		/// true the underlying stream is also closed.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (!isClosed)
			{
				isClosed = true;

				if (IsStreamOwner)
					baseInputStream.Dispose();
			}
		} */
	}
}
