using Algorithm;


namespace QuickSortRealization
{
    public class QuickSortRealization<T> : IQuickSort<T> where T : IComparable
    {
        public T[] QuickSort(T[] array)
        {
            return QuickSort(array, 0, array.Length - 1);
        }

        private static void Swap(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private static int FindIndex(T[] array, int minIndex, int maxIndex)
        {
            var current = array[maxIndex];
            var index = minIndex - 1;
            for (int i = minIndex; i < maxIndex; i++)
            {
                if (array[i].CompareTo(current) == -1)
                {
                    index += 1;
                    Swap(ref array[index], ref array[i]);
                }
            }
            index += 1;
            Swap(ref array[index], ref array[maxIndex]);
            return index;
        }

        public static T[] QuickSort(T[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex) return array;

            var index = FindIndex(array, minIndex, maxIndex);
            Parallel.Invoke(
                () => QuickSort(array, minIndex, index - 1),
                () => QuickSort(array, index + 1, maxIndex)
            );

            return array;
        }
    }
}