#define _GNU_SOURCE
#include <stdio.h>
#include <string.h>
#include <limits.h>
#include <stdlib.h>
#include <stdbool.h>

#define DIMENSION_OF(a) (sizeof(a) / sizeof(a[0]))

int distance_compare(const void* elem1, const void* elem2,
		void* distance)
{

	// Sort vertex based on distance.

	unsigned int elem1_value =
			((unsigned int*) distance)[*((unsigned int*) elem1)];
	unsigned int difference =  elem1_value -
			((unsigned int*) distance)[*((unsigned int*) elem2)];

	// The distances are all unsigned values.  Thus if I subtract an unsigned
	// value from elem1 and I get a greater value than elem1 then the value
	// I subtracted from elem1 must have been larger than it.
		
	if (difference > elem1_value)
		return -1;
	else if (difference == 0)
		return 0;
	else
		return 1;
}

// Calculate shortest path(s) using Dijkstra's algorithm.  Note that
// the matrix contains edge distance between vertexes.  A zero value
// indicates no path between the verticies.  Since zero could be a
// concievable distance, we add 1 to all distances such that we can
// use zero to indicate no path between the verticies.  This means
// the actual length is the distance value minus 1.

bool dijkstra_shortest_paths(const unsigned int* graph,
		unsigned int n, unsigned int start, unsigned int end,
		unsigned int *paths)
{

	// Initialze the vertex distance array.
		
	unsigned int distance[n];
	for (unsigned int i = 0; i < n; ++i)
		distance[i] = UINT_MAX;
		
	// Set the distance of our start vertex to zero.  This will cause it to
	// get sorted to the front of the queue and thus get processed first.
		
	distance[start] = 0;

	// Fill in the queue with the indexes.
		
	unsigned int queue[n];
	for (unsigned int i = 0; i < n; ++i)
		queue[i] = i;

	// Go until queue is empty.
		
	unsigned int head = 0;
	while (head != n)
		{

		// Sort the queue of indexes by distance.

		qsort_r(&queue[head], n - head, sizeof(unsigned int),
				distance_compare, distance);
			
		// Get the vertex with the shortest distance.
			
		unsigned int vertex = queue[head];

		// Remove front of queue.

		head++;

		// Check each neighbor of vertex and see if we need to update
		// the distance to that neighbor.

		for (unsigned int i = 0; i < n; ++i)
			{
				
			// Is this a neighbor?
				
			if (graph[(vertex * n) + i] > 1)
				{
							
				// If we found a shorter distance then update the distance.

				if (distance[i] > distance[vertex] +
						(graph[(vertex * n) + i] - 1))
					{
					distance[i] = distance[vertex] +
							(graph[(vertex * n) + i] - 1);
					}
				}
			}
		}

	// If no path found then return null.
		
	if (distance[end] == UINT_MAX)
		return false;
			
	else
		{

		// Initialize the paths matrix.
		
		memset(paths, 0, n * n * sizeof(unsigned int));
		
		// Flags to tell whether we already handled a vertex when
		// generating the paths (eg. backtracing).
			
		bool handled_vertex[n];
		for (unsigned int i = 0; i < n; ++i)
			handled_vertex[i] = false;

		// Vertexes queue used when determining the paths.
			
		unsigned int vertexes[n];
		vertexes[0] = end;
		unsigned int cur = 0;
		unsigned int avail = 1;

		// Go from end to start along the shortest paths found to
		// fill in the paths graph.
			
		while (cur != avail)
			{
			for (unsigned int i = 0; i < n; ++i)
				{

				// Rows in the graph indicate outgoing edges from the vertex.
				// Columns in the graph indicate incoming edges to the vertex.

				// I'm checking down column vertexes[cur] to see if there
				// in an incoming edge from vertex i.
					
				if (graph[(i * n) + vertexes[cur]] > 1)
					{

					// If the edge was part of a shortest path, eg. distance
					// of current vertex is the sum of the distance of the
					// incoming vertex plus the edge, then the incoming vertex
					// was part of a shortest path.
						
					if (distance[vertexes[cur]] ==
							(distance[i] +
							(graph[(i * n) + vertexes[cur]] - 1)))
						{

						// This incoming vertex was part of a shortest path
						// so copy over its edge from the original graph.
							
						paths[(i * n) + vertexes[cur]] =
								graph[(i * n) + vertexes[cur]];

						// If we didn't handle this incoming vertex already
						// add it to the queue.
							
						if (handled_vertex[i] == false)
							{
							vertexes[avail++] = i;
							handled_vertex[i] = true;
							}
						}
					}
				}

			// Move to next vertex to process.
				
			cur++;
			}

		return true;
		}
}
	
void show_paths(const unsigned int* graph, unsigned int n,
		unsigned int vertex, unsigned int end, unsigned int* path,
		unsigned int verticies, unsigned int length)
{

	// Add vertex to path.
		
	path[verticies++] = vertex;

	// If this vertex is the end then dump out the path.
		
	if (vertex == end)
		{

		// Dump path.  Format a bit to ensure long paths are
		// reasonably displayed.
			
		printf("path, length = %d:\n\n", length);
		for (unsigned int i = 0; i < verticies; ++i)
			{
			printf("%d", path[i]);
			if (i != (verticies - 1))
				printf(",");
			if (((i + 1) % 16) == 0)
				printf("\n");
			else
				printf(" ");
			}
		printf("\n\n");
		}

	// If we're not at the end then recurse with all outgoing
	// verticies (rows) of this vertex.
		
	else
		{
		for (unsigned int i = 0; i < n; ++i)
			{

			// Path between this vertex and i?
				
			if (graph[(vertex * n) + i] != 0)
				{

				// Yup, recurse with this outgoing vertex (i).
					
				show_paths(graph, n, i, end, path, verticies, length +
					graph[(vertex * n) + i] - 1);
				}
			}
		}
}

void main(int argc, char* argv[])
{
	unsigned int graph[6][6] = {
			{1, 4, 0, 5, 0, 13},
			{0, 1, 2, 0, 2, 0},
			{0, 0, 1, 0, 2, 3},
			{0, 0, 6, 1, 5, 0},
			{0, 0, 0, 0, 1, 3},
			{0, 0, 0, 0, 0, 1}};
	unsigned int paths[6][6];
	
	unsigned int start = 0;
	unsigned int end = 5;
		
	// Calculate shortest path(s).
		
	bool found = dijkstra_shortest_paths(&graph[0][0],
			DIMENSION_OF(graph), start, end, &paths[0][0]);

	if (found == false)
		printf("No paths found from %d to %d", start, end);
	else
		{

		// Print out the paths.  A path can't be longer than the number
		// of verticies.

		unsigned int path[DIMENSION_OF(graph)];
		unsigned int verticies = 0;
		unsigned int length = 0;			
		show_paths(&paths[0][0], DIMENSION_OF(graph), start, end, path,
				verticies, length);
		}
}
