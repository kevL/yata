using System;


namespace yata
{
	/// <summary>
	/// Contains the output from the Inflation process. We need to have a window
	/// so that we can refer backwards into the output stream to repeat stuff.
	/// </summary>
	/// <remarks>Author of the original java version: John Leuner</remarks>
	public class OutputWindow
	{
		#region Fields (static)
		const int WindowSize = 0x8000; // 1 << 15 // the window is 2^15 bytes
		const int WindowMask = 0x7FFF; // WindowSize - 1
		#endregion Fields (static)


		#region Fields
		readonly byte[] _window = new byte[WindowSize];

		int _windowEnd;
		int _windowFilled;
		#endregion Fields


		/// <summary>
		/// Writes a byte to this <c>OutputWindow</c>.
		/// </summary>
		/// <param name="value">value to write</param>
		internal void Write(int value)
		{
			++_windowFilled;

			_window[_windowEnd++] = (byte)value;
			_windowEnd &= WindowMask;
		}

		/// <summary>
		/// Appends a byte pattern already in the window itself.
		/// </summary>
		/// <param name="length">length of pattern to copy</param>
		/// <param name="distance">distance from end of window pattern occurs</param>
		internal void Repeat(int length, int distance)
		{
			_windowFilled += length;

			int repStart = (_windowEnd - distance) & WindowMask;
			int border = WindowSize - length;
			if ((repStart <= border) && (_windowEnd < border))
			{
				if (length <= distance)
				{
					Array.Copy(_window, repStart, _window, _windowEnd, length);
					_windowEnd += length;
				}
				else
				{
					// We have to copy manually, since the repeat pattern overlaps.
					while (length-- > 0)
						_window[_windowEnd++] = _window[repStart++];
				}
			}
			else
				SlowRepeat(repStart, length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="repStart"></param>
		/// <param name="length"></param>
		void SlowRepeat(int repStart, int length)
		{
			while (length-- > 0)
			{
				_window[_windowEnd++] = _window[repStart++];
				_windowEnd &= WindowMask;
				repStart  &= WindowMask;
			}
		}

		/// <summary>
		/// Copies <c><see cref="StreamManipulator"/></c> input to
		/// <c><see cref="_window"/></c>.
		/// </summary>
		/// <param name="input">source of data</param>
		/// <param name="length">length of data to copy</param>
		/// <returns>the count of bytes copied</returns>
		internal int CopyStored(StreamManipulator input, int length)
		{
			length = Math.Min(Math.Min(length, WindowSize - _windowFilled), input.AvailableBytes);
			int copied;

			int tailLen = WindowSize - _windowEnd;
			if (length > tailLen)
			{
				copied = input.CopyBytes(_window, _windowEnd, tailLen);
				if (copied == tailLen)
					copied += input.CopyBytes(_window, 0, length - tailLen);
			}
			else
				copied = input.CopyBytes(_window, _windowEnd, length);

			_windowEnd = (_windowEnd + copied) & WindowMask;
			_windowFilled += copied;

			return copied;
		}

		/// <summary>
		/// Get remaining unfilled space in this <c>OutputWindow</c>.
		/// </summary>
		/// <returns>count of bytes left in window</returns>
		internal int GetFreeSpace()
		{
			return WindowSize - _windowFilled;
		}

		/// <summary>
		/// Gets bytes available for output in this <c>OutputWindow</c>.
		/// </summary>
		/// <returns>count of bytes filled</returns>
		internal int GetAvailable()
		{
			return _windowFilled;
		}

		/// <summary>
		/// Copy contents of this <c>OutputWindow</c> to
		/// <paramref name="output"/>.
		/// </summary>
		/// <param name="output">buffer to copy to</param>
		/// <param name="offset">offset to start at</param>
		/// <param name="len">length of bytes to count</param>
		/// <returns>the count of bytes copied</returns>
		internal int CopyOutput(Array output, int offset, int len)
		{
			int copyEnd = _windowEnd;

			if (len > _windowFilled)
				len = _windowFilled;
			else
				copyEnd = (_windowEnd - _windowFilled + len) & WindowMask;

			int copied = len;
			int tailLen = len - copyEnd;

			if (tailLen > 0)
			{
				Array.Copy(_window, WindowSize - tailLen, output, offset, tailLen);
				offset += tailLen;
				len = copyEnd;
			}

			Array.Copy(_window, copyEnd - len, output, offset, len);
			_windowFilled -= copied;

			return copied;
		}
	}
}
