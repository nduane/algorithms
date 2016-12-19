#include <stdio.h>
#include <stdlib.h>
#include <limits.h>
#include <time.h>

#define DIMENSION_OF(a)	(sizeof(a) / sizeof(a[0]))

int max_sum_sub_array_order_n(const int* values, unsigned int size,
		int* start, int* end)
{
	int max_sum = INT_MIN;
	int max_start = 0;
	int max_end = 0;
	int cur_sum = 0;
	int cur_start = 0;
	int cur_end = 0;
	unsigned int j;

	for (j = 0; j < size; ++j)
		{

		// Get current running sum.

		cur_sum += values[j];

		// If greater than max or equal to it with a shorter length,
		// record the sum and the start/end indexes.

		if (cur_sum >= max_sum)
			{
			if ((cur_sum != max_sum) ||
					((cur_end - cur_start) < (max_end - max_start)))
				{
				max_sum = cur_sum;
				max_start = cur_start;
				max_end = cur_end;
				}
			}

		// Else if our sum as gone to less than or equal to zero
		// then we can start a new running sum at the next index.

		else if (cur_sum <= 0)
			{
			cur_sum = 0;
			cur_start = j + 1;
			cur_end = j;
			}

		cur_end++;
		}

	*start = max_start;
	*end = max_end;
	return max_sum;
}

int max_sum_sub_array_order_n_squared(const int* values, unsigned int size,
		int* start, int* end)
{
	int max_sum = INT_MIN;
	int max_start = 0;
	int max_end = 0;
	unsigned int j;
	unsigned int k;

	for (j = 0; j < size; ++j)
		{
		int cur_sum = 0;

		// Calculate all sums which start from k.

		for (k = j; k < size; ++k)
			{
			cur_sum += values[k];

			// If greater than max or equal to it with a shorter length,
			// record the sum and the start/end indexes.

			if (cur_sum >= max_sum)
				{
				if ((cur_sum != max_sum) || ((k - j) < (max_end - max_start)))
					{
					max_sum = cur_sum;
					max_start = j;
					max_end = k;
					}
				}
			}
		}

	*start = max_start;
	*end = max_end;
	return max_sum;
}

void main(int argc, char* argv[])
{
//	int values[] = {30, -25, -6, 8, 23, 0};
	int values[128 * 1024];
	int sum;
	int start;
	int end;
	unsigned int j;
	time_t start_time;
	time_t end_time;

	// Initialize values.

	srand(1313);
	for (j = 0; j < DIMENSION_OF(values); ++j)
		{
		values[j] = (rand() % 2000) - 1000;
		}

	// Order n squared first.

	start_time = time(NULL);
	sum = max_sum_sub_array_order_n_squared(values, DIMENSION_OF(values),
			&start, &end);
	end_time = time(NULL);
	printf("order nsquared, max sum: %d, start: %d, end: %d, duration: %ld\n",
			sum, start, end, (long) (end_time - start_time));

	// Order n.

	start_time = time(NULL);
	sum = max_sum_sub_array_order_n(values, DIMENSION_OF(values),
			&start, &end);
	end_time = time(NULL);
	printf("order n, max sum: %d, start: %d, end: %d, duration: %ld\n",
			sum, start, end, (long) (end_time - start_time));
}
