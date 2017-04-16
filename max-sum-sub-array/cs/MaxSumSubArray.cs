using System;
using System.Diagnostics;

public class Application
{
	private static int MaxSumSubArrayOrderN(int[] values, out uint start,
			out uint end)
	{
		start = end = 0;
		int maxSum = int.MinValue;
		int curSum = 0;
		uint curStart = 0;
		uint curEnd = 0;

		for (uint i = 0; i < (uint) values.Length; ++i)
			{

			// Get current running sum.

			curSum += values[i];

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

			// If our sum as gone to less than or equal to zero then we
			// can start a new running sum at the next index.

			if (curSum <= 0)
				{
				curSum = 0;
				curStart = i + 1;
				curEnd = i;
				}

			curEnd++;
			}

		return maxSum;
	}

	private static int MaxSumSubArrayOrderNSquared(int[] values,
			out uint start, out uint end)
	{
		int maxSum = int.MinValue;

		start = end = 0;
		for (uint i = 0; i < values.Length; ++i)
			{
			int curSum = 0;

			// Calculate all sums which start from k.

			for (uint j = i; j < (uint) values.Length; ++j)
				{
				curSum += values[j];

				// If greater than max or equal to it with a shorter length,
				// record the sum and the start/end indexes.

				if (curSum >= maxSum)
					{
					if ((curSum != maxSum) || ((j - i) < (end - start)))
						{
						maxSum = curSum;
						start = i;
						end = j;
						}
					}
				}
			}

		return maxSum;
	}

	public static void Main(string[] args)
	{
//		int[] values = new int[] {30, -25, -6, 8, 23, 0};
//		int[] values1 = new int[] {-30, -40, -50, -60, -7, -3, -100, -30, -70};
//		int[] values2 = new int[] {-30, -40, -50, -60, -7, -3, 20, -19, 1, -3};
//		int[] values3 = new int[] {-30, -40, -50, -60, -7, -3, 20, -19, 1, 19, -10};

		int[] values = new int[128 * 1024];

		// Initialize values.

		Random random = new Random();
		for (uint i = 0; i < values.Length; ++i)
			{
			values[i] = random.Next(-1000, 1000);
			}

		uint start;
		uint end;

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
/*
		uint start;
		uint end;

		Stopwatch stopwatch = new Stopwatch();

		stopwatch.Start();
		int sum = MaxSumSubArrayOrderNSquared(values1, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("1: order nsquared, max sum: {0}, start: {1}, " +
				"end: {2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();
	
		// Order n.

		stopwatch.Start();
		sum = MaxSumSubArrayOrderN(values1, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("1: order n, max sum: {0}, start: {1}, end: " +
				"{2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();

		stopwatch.Start();
		sum = MaxSumSubArrayOrderNSquared(values2, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("2: order nsquared, max sum: {0}, start: {1}, " +
				"end: {2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();
	
		// Order n.

		stopwatch.Start();
		sum = MaxSumSubArrayOrderN(values2, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("2: order n, max sum: {0}, start: {1}, end: " +
				"{2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();
	
		stopwatch.Start();
		sum = MaxSumSubArrayOrderNSquared(values3, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("3: order nsquared, max sum: {0}, start: {1}, " +
				"end: {2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);

		stopwatch.Reset();
	
		// Order n.

		stopwatch.Start();
		sum = MaxSumSubArrayOrderN(values3, out start, out end);
		stopwatch.Stop();
		Console.WriteLine("3: order n, max sum: {0}, start: {1}, end: " +
				"{2}, duration: {3}", sum, start, end,
				stopwatch.ElapsedMilliseconds);
*/
	}
}
