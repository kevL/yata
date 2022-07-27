using System;


namespace yata
{
	/// <summary>
	/// This class allows us to retrieve a specified number of bits from the
	/// input buffer as well as copy big byte blocks.<br/><br/>
	/// It uses an <c>int</c> buffer to store up to 31 bits for direct
	/// manipulation. This guarantees that we can get at least 16 bits, but we
	/// only need at most 15 so this is all safe.<br/><br/>
	/// There are some optimizations in this class; for example, you must never
	/// peek more than 8 bits more than needed and you must first peek bits
	/// before you may drop them. This is not a general purpose class but
	/// optimized for the behaviour of the Inflator.
	/// </summary>
	/// <remarks>Authors of the original java version: John Leuner and Jochen
	/// Hoenicke</remarks>
	public class StreamManipulator
	{
		#region Fields
		byte[] _window;
		int _windowStart;
		int _windowEnd;
		uint _buffer;
		int _bitsInBuffer;
		#endregion Fields


		#region Properties
		/// <summary>
		/// Gets the count of bits available in the bit buffer. This must be
		/// only called when a previous
		/// <c><see cref="PeekBits()">PeekBits()</see></c> returned <c>-1</c>.
		/// </summary>
		internal int AvailableBits
		{
			get { return _bitsInBuffer; }
		}

		/// <summary>
		/// Gets the count of bytes available.
		/// </summary>
		internal int AvailableBytes
		{
			get { return _windowEnd - _windowStart + (_bitsInBuffer >> 3); }
		}

		/// <summary>
		/// <c>true</c> when <c><see cref="SetInput()">SetInput()</see></c> can
		/// be called.
		/// </summary>
		internal bool IsNeedingInput
		{
			get { return _windowStart == _windowEnd; }
		}
		#endregion Properties


		#region Methods
		/// <summary>
		/// Gets the next sequence of bits but doesn't advance input pointer.
		/// </summary>
		/// <param name="bitCount">the count of bits to peek at</param>
		/// <returns>the value of the bits or -1 if not enough bits available</returns>
		/// <remarks><paramref name="bitCount"/> must be less than or equal to
		/// 16 and if this call succeeds you must drop at least n-8 bits in the
		/// next call.</remarks>
		internal int PeekBits(int bitCount)
		{
			if (_bitsInBuffer < bitCount)
			{
				if (_windowStart == _windowEnd)
					return -1;

				_buffer |= (uint)((_window[_windowStart++] & 0xff
								| (_window[_windowStart++] & 0xff) << 8) << _bitsInBuffer);
				_bitsInBuffer += 16;
			}
			return (int)(_buffer & ((1 << bitCount) - 1));
		}

		/// <summary>
		/// Tries to grab the next <paramref name="bitCount"/> bits from the
		/// input and sets <paramref name="output"/> to the value, adding
		/// <paramref name="outputOffset"/>.
		/// </summary>
		/// <returns><c>true</c> if enough bits could be read</returns>
		internal bool TryGetBits(int bitCount, ref int output, int outputOffset)
		{
			int bits = PeekBits(bitCount);
			if (bits < 0)
				return false;

			output = bits + outputOffset;

			DropBits(bitCount);

			return true;
		}

		/// <summary>
		/// Tries to grab the next <paramref name="bitCount"/> bits from the
		/// input and sets <paramref name="id"/> of <paramref name="array"/>
		/// to the value.
		/// </summary>
		/// <returns><c>true</c> if enough bits could be read</returns>
		internal bool TryGetBits(int bitCount, ref byte[] array, int id)
		{
			int bits = PeekBits(bitCount);
			if (bits < 0)
				return false;

			array[id] = (byte)bits;

			DropBits(bitCount);

			return true;
		}

		/// <summary>
		/// Drops the next <paramref name="bitCount"/> bits from the input.
		/// </summary>
		/// <param name="bitCount">the number of bits to drop</param>
		/// <remarks>You should have called
		/// <c><see cref="PeekBits()"/>PeekBits()</c> with equal to or bigger
		/// than <paramref name="bitCount"/> before to make sure that enough
		/// bits are in the bit-buffer.</remarks>
		internal void DropBits(int bitCount)
		{
			_buffer >>= bitCount;
			_bitsInBuffer -= bitCount;
		}

		/// <summary>
		/// Skips to the next byte-boundary.
		/// </summary>
		internal void SkipToByteBoundary()
		{
			_buffer >>= (_bitsInBuffer & 7);
			_bitsInBuffer &= ~7;
		}

		/// <summary>
		/// Copies bytes from input buffer to output buffer starting at
		/// <c>output[offset]</c>.
		/// </summary>
		/// <param name="output">the buffer to copy bytes to</param>
		/// <param name="offset">the offset in the buffer at which copying
		/// starts</param>
		/// <param name="length">the length to copy; 0 is allowed</param>
		/// <returns>the count of bytes copied; 0 if no bytes were available</returns>
		/// <remarks>You have to make sure that the buffer is byte-aligned. If
		/// not enough bytes are available copies fewer bytes.</remarks>
		internal int CopyBytes(byte[] output, int offset, int length)
		{
			int count = 0;

			while (_bitsInBuffer > 0 && length > 0)
			{
				output[offset++] = (byte)_buffer;
				_buffer >>= 8;
				_bitsInBuffer -= 8;

				--length;
				++count;
			}

			if (length == 0)
				return count;

			int avail = _windowEnd - _windowStart;
			if (length > avail)
				length = avail;

			Array.Copy(_window, _windowStart, output, offset, length);
			_windowStart += length;

			if (((_windowStart - _windowEnd) & 1) != 0)
			{
				// always want an even number of bytes in input - see PeekBits()
				_buffer = (uint)(_window[_windowStart++] & 0xff);
				_bitsInBuffer = 8;
			}

			return count + length;
		}

		/// <summary>
		/// Adds more input for consumption.
		/// </summary>
		/// <param name="buffer">data to be input</param>
		/// <param name="offset">offset of first byte of input</param>
		/// <param name="count">count of bytes of input to add</param>
		/// <remarks>Only call when <c><see cref="IsNeedingInput"/></c> is
		/// <c>true</c>.</remarks>
		internal void SetInput(byte[] buffer, int offset, int count)
		{
			int end = offset + count;

			if ((count & 1) != 0)
			{
				// always want an even number of bytes in input - see PeekBits()
				_buffer |= (uint)((buffer[offset++] & 0xff) << _bitsInBuffer);
				_bitsInBuffer += 8;
			}

			_window      = buffer;
			_windowStart = offset;
			_windowEnd   = end;
		}
		#endregion Methods
	}
}
