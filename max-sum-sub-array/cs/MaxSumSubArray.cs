using System;
using System.Diagnostics;

public class Application
{
	private static int MaxSumSubArrayOrderN(int[] values, out int start,
			out int end)
	{
		start = end = 0;
		int maxSum = int.MinValue;
		int curSum = 0;
		int curStart = 0;
		int curEnd = 0;

		for (int j = 0; j < values.Length; ++j)
			{

			// Get current running sum.

			curSum += values[j];

			// If greater than max or equal to it with a shorter length,
			// record the sum and the start/end indexes.

			if (curSum >= maxSum)
				{
				if ((curSum != maxSum) || ((curEnd - curStart) < (end - start)))
					{
					maxSum = curSum;
					start = curStart;
					end = curEnd;
					}
				}

			// Else if our sum as gone to less than or equal to zero
			// then we can start a new running sum at the next index.

			else if (curSum <= 0)
				{
				curSum = 0;
				curStart = j + 1;
				curEnd = j;
				}

			curEnd++;
			}

		return maxSum;
	}

	private static int MaxSumSubArrayOrderNSquared(int[] values, out int start,
			out int end)
	{
		int maxSum = int.MinValue;

		start = end = 0;
		for (int j = 0; j < values.Length; ++j)
			{
			int curSum = 0;

			// Calculate all sums which start from k.

			for (int k = j; k < values.Length; ++k)
				{
				curSum += values[k];

				// If greater than max or equal to it with a shorter length,
				// record the sum and the start/end indexes.

				if (curSum >= maxSum)
					{
					if ((curSum != maxSum) || ((k - j) < (end - start)))
						{
						maxSum = curSum;
						start = j;
						end = k;
						}
					}
				}
			}

		return maxSum;
	}

	public static void Main(string[] args)
	{
//		int[] values = new int[] {30, -25, -6, 8, 23, 0};
		int[] values = new int[128 * 1024];

		// Initialize values.

		Random random = new Random();
		for (int j = 0; j < values.Length; ++j)
			{
			values[j] = random.Next(-1000, 1000);
			}

		int start;
		int end;

		// Order n squared first.

		Stopwatch stopwatch = new Stopwatch();

		stopwatch.Start();
		int sum = MaxSumSubArrayOrderNSquared(values, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("order nsquared, max sum: {0}, start: {1}, end: " +
				"{2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();

		// Order n.

		stopwatch.Start();
		sum = MaxSumSubArrayOrderN(values, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("order n, max sum: {0}, start: {1}, end: {2}, " +
				"duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);
	}
}
