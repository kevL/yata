using System;
using System.IO;


namespace yata
{
	/// <summary>
	/// An input buffer customised for use by
	/// <c><see cref="InflatorInputStream"/></c>.
	/// </summary>
	/// <remarks>The buffer supports decryption of incoming data.</remarks>
	public class InflatorInputBuffer
	{
		#region Fields
		readonly Stream _inputStream;
		readonly byte[] _rawData;
		readonly byte[] _clearText;

		int _rawLength;
		int _clearTextLength;
		#endregion Fields


		#region Properties
		/// <summary>
		/// Gets/sets the count of bytes available.
		/// </summary>
		internal int Available
		{ get; private set; }
		#endregion Properties


		#region cTor
		/// <summary>
		/// Initializes a new instance of
		/// <c><see cref="InflatorInputBuffer"/></c>.
		/// </summary>
		/// <param name="stream">the stream to buffer</param>
		/// <param name="bufferSize">the size to use for the buffer</param>
		internal InflatorInputBuffer(Stream stream, int bufferSize)
		{
			_inputStream = stream;

			_rawData = new byte[bufferSize];
			_clearText = _rawData;
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Fills the buffer from the underlying input stream.
		/// </summary>
		internal void Fill()
		{
			_rawLength = 0;
			int toRead = _rawData.Length;

			while (toRead > 0 && _inputStream.CanRead)
			{
				int count = _inputStream.Read(_rawData, _rawLength, toRead);
				if (count <= 0)
					break;

				_rawLength += count;
				toRead -= count;
			}

			_clearTextLength = _rawLength;
			Available = _clearTextLength;
		}

		/// <summary>
		/// Calls
		/// <c><see cref="Inflator.SetInput(byte[],int,int)">Inflator.SetInput(byte[],int,int)</see></c>
		/// passing the current cleartext buffer-contents.
		/// </summary>
		/// <param name="inf">the inflator to set input for</param>
		internal void SetInflaterInput(Inflator inf)
		{
			if (Available > 0)
			{
				inf.SetInput(_clearText, _clearTextLength - Available, Available);
				Available = 0;
			}
		}
		#endregion Methods
	}
}
