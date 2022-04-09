using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Utilities
{
    public static class For
    {
        public static void ForFixedArrayIncrease<T>(T[] array, Action<T> everycallback)
        {
            long co = array.LongLength;
            for (long i = 0; i < co; i++)
            {
                everycallback(array[i]);
            }
        }
        public static void ForArrayIncrease<T>(T[] array, Action<T> everycallback)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                everycallback(array[i]);
            }
        }
        public static void ForFixedArrayDecrease<T>(T[] array, Action<T> everycallback)
        {
            long co = array.LongLength;
            for (long i = co; i > co; i--)
            {
                everycallback(array[i]);
            }
        }
        public static void ForArrayDecrease<T>(T[] array, Action<T> everycallback)
        {
            for (long i = array.LongLength; i > 0; i--)
            {
                everycallback(array[i]);
            }
        }

        public static void ForFixedListIncrease<T>(List<T> list, Action<T> everycallback)
        {
            int co = list.Count;
            for (int i = 0; i < co; i++)
            {
                everycallback(list[i]);
            }
        }
        public static void ForListIncrease<T>(List<T> list, Action<T> everycallback)
        {
            for (int i = 0; i < list.Count; i++)
            {
                everycallback(list[i]);
            }
        }
        public static void ForFixedListDecrease<T>(List<T> list, Action<T> everycallback)
        {
            int co = list.Count;
            for (int i = co; i > 0; i--)
            {
                everycallback(list[i]);
            }
        }
        public static void ForListDecrease<T>(List<T> list, Action<T> everycallback)
        {
            for (int i = list.Count; i > 0; i--)
            {
                everycallback(list[i]);
            }
        }

        public static void ForFixedIncrease(int from, int to, Action<int> everycallback)
        {
            for (int i = 0; i < to; i++)
            {
                everycallback(i);
            }
        }
        public static void ForFixedIncrease(uint from, uint to, Action<uint> everycallback)
        {
            for (uint i = 0; i < to; i++)
            {
                everycallback(i);
            }
        }
        public static void ForFixedIncrease(long from, long to, Action<long> everycallback)
        {
            for (uint i = 0; i < to; i++)
            {
                everycallback(i);
            }
        }
        public static void ForFixedIncrease(ulong from, ulong to, Action<ulong> everycallback)
        {
            for (uint i = 0; i < to; i++)
            {
                everycallback(i);
            }
        }
    }
}
