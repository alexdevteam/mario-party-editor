﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioPartyEditor.ROM
{
    public struct ByteSlice
    {
        public byte[] Source { get; set; }
        public int SliceStart { get; set; }
        // Where the slice ends (Exclusive)
        public int SliceEnd { get; set; }
        public int Size { get => SliceEnd - SliceStart; }

        /// <summary>
        /// Returns a full slice referencing a data source.
        /// </summary>
        /// <param name="source">The data to reference.</param>
        public ByteSlice(byte[] source)
        {
            Source = source;
            SliceStart = 0;
            SliceEnd = Source.Length;
        }

        /// <summary>
        /// Returns a slice referencing part of a data source.
        /// </summary>
        /// <param name="source">The data to reference.</param
        /// <param name="start">The start of the slice.</param>
        /// <param name="end">The end of the slice. Clamps to the source size.</param>
        public ByteSlice(byte[] source, int start, int end)
        {
            Source = source;
            SliceStart = start;
            SliceEnd = Math.Min(source.Length, end);
        }

        public ByteSlice Slice(int index, int length) =>
            new ByteSlice(Source, SliceStart + index, SliceStart + index + length);

        public void ReplaceWith(ByteSlice origin)
        {
            if (Size != origin.Size) throw new Exception("Origin and target size must be equal.");
            if (SliceStart + origin.Size > SliceEnd) throw new IndexOutOfRangeException();

            for (int i = 0; i < origin.Size; ++i)
            {
                Source[SliceStart + i] = origin.Source[origin.SliceStart + i];
            }
        }

        public byte[] GetAsArrayCopy() => Source.SubArray(SliceStart, Size);
        public byte this[int index]
        {
            get
            {
                if (SliceStart + index > SliceEnd) throw new IndexOutOfRangeException();
                return Source[SliceStart + index];
            }
            set
            {
                if (SliceStart + index > SliceEnd) throw new IndexOutOfRangeException();
                Source[SliceStart + index] = value;
            }
        }
    }
}