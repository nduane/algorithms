using System;

public static class MergeSort
{
	private static void SortInternal<T>(T[] a, T[] b, uint start, uint end,
			Comparison<T> compare)
	{

		// run of 1 is sorted, anything greater we need to handle.
		
		if (start < end)
			{

			// Split.
			
			uint length = end - start + 1;
			uint start2 = start + (length / 2);

			// Note the swapping of a & b.  This is done on purpose otherwise
			// the moves to the b array below will not be seen when comparing
			// the values in the a array.  Thus on each recursion step we swap
			// the array we're reading from and writing to.

			// Sort the runs.
			
			SortInternal(b, a, start, start2 - 1, compare);
			SortInternal(b, a, start2, end, compare);

			// Merge.

			for (uint i = start, j = start2, k = start; true; )
				{

				// We're done if we're at the end of both runs?
				
				if ((i >= start2) && (j > end))
					break;

				// End of the left run?
				
				else if (i >= start2)
					b[k++] = a[j++];

				// End of right run?
				
				else if (j > end)
					b[k++] = a[i++];

				// Else we need to compare the items at the front
				// of both runs.
				
				else
					{
					int diff = compare(a[i], a[j]);
					if (diff <= 0)
						b[k++] = a[i++];
					else
						b[k++] = a[j++];
					}
				}
			}
	}

	public static void Sort<T>(T[] a, uint start, uint end, Comparison<T> compare)
	{
		T[] b = new T[a.Length];
		a.CopyTo(b, 0);
		SortInternal(b, a, start, end, compare);
	}
	
	public static void Sort<T>(T[] a, Comparison<T> compare)
	{
		if (a.Length != 0)
			Sort(a, 0, (uint) a.Length - 1, compare);
	}
}

public class Application
{

	private static int Compare(int left, int right)
	{
		return left - right;
	}
	
	private static void DumpArray(int[] ar)
	{
		for (uint i = 0; i < ar.Length; ++i)
			{
			Console.Write("{0}", ar[i]);
			if (i != ar.Length - 1)
				Console.Write(", ");
			if (((i + 1) % 16) == 0)
				Console.WriteLine();
			}
		Console.WriteLine();
	}
	
	public static void Main(string[] args)
	{
		int[] numbers = new int[] {-9, 0, 0, 25, -13, 0, 66, 103, 2, 2, 55};

		DumpArray(numbers);
		MergeSort.Sort<int>(numbers, Compare);
		DumpArray(numbers);
	}
}
