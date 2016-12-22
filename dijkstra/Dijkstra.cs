using System;
using System.Collections;

public static class Dijkstra
{
	private class DistanceCompare : IComparer
	{
		private uint[] _distance;
		
		public DistanceCompare(uint[] distance)
		{
			this._distance = distance;
		}
		
		public int Compare(object l, object r)
		{

			// Sort vertex based on distance.
			
			if (this._distance[(uint) l] < this._distance[(uint) r])
				return -1;
			else if (this._distance[(uint) l] == this._distance[(uint) r])
				return 0;
			else
				return 1;
		}
	}

	// Calculate shortest path(s) using Dijkstra's algorithm.  Note that
	// the matrix contains edge distance between vertexes.  A zero value
	// indicates no path between the verticies.  Since zero could be a
	// concievable distance, we add 1 to all distances such that we can
	// use zero to indicate no path between the verticies.  This means
	// the actual length is the distance value minus 1.
	
	public static uint[,] ShortestPaths(uint[,] graph, uint start, uint end)
	{

		// Do some validation.
		
		if (graph.Rank != 2)
			throw(new InvalidOperationException("graph matrix must be two " +
					"dimensional"));
		if (graph.GetLength(0) != graph.GetLength(1))
			throw(new InvalidOperationException("graph matrix must be square"));

		// Create an array to hold the distance to each vertex while calculating
		// the shortest path.
		
		uint[] distance = new uint[graph.GetLength(0)];
		for (int i = 0; i < distance.Length; ++i)
			distance[i] = uint.MaxValue;

		// Set the distance of our start vertex to zero.  This will cause it to
		// get sorted to the front of the queue and thus get processed first.
		
		distance[start] = 0;

		// This queue initially contains all the indexes.
		
		uint[] queue = new uint[distance.Length];
		for (int i = 0; i < queue.Length; ++i)
			queue[i] = (uint) i;
		int head = 0;
		
		IComparer distanceCompare = new DistanceCompare(distance);

		// Go until queue is empty.
		
		while (head != queue.Length)
			{

			// Sort the queue of indexes by distance.
		
			Array.Sort(queue, head, queue.Length - head, distanceCompare);

			// Get the vertex with the shortest distance.
			
			uint vertex = queue[head];

			// Remove front of queue.

			head++;

			// Check each neighbor of vertex and see if we need to update
			// the distance to that neighbor.

			for (int i = 0; i < graph.GetLength(0); ++i)
				{
				
				// Is this a neighbor?
				
				if (graph[vertex, i] > 1)
					{
							
					// If we found a shorter distance then update the distance.

					if (distance[i] > distance[vertex] + (graph[vertex, i] - 1))
						distance[i] = distance[vertex] + (graph[vertex, i] - 1);
					}
				}
			}

		// If no path found then return null.
		
		if (distance[end] == uint.MaxValue)
			return null;
			
		else
			{

			// Create a return graph with just lengths for the shortest
			// paths.  Initialize all edges to 0 (meaning no path).
			
			uint[,] paths = new uint[graph.GetLength(0), graph.GetLength(1)];
			for (int i = 0; i < graph.GetLength(0); ++i)
				{
				for (int k = 0; k < graph.GetLength(1); ++k)
					{
					paths[i, k] = 0;
					}
				}

			// Flags to tell whether we already handled a vertex when
			// generating the paths (eg. backtracing).
			
			bool[] handledVertex = new bool[graph.GetLength(0)];
			for (int i = 0; i < handledVertex.Length; ++i)
				handledVertex[i] = false;

			// Vertexes queue used when determining the paths.
			
			uint[] vertexes = new uint[graph.GetLength(0)];
			vertexes[0] = end;
			uint cur = 0;
			uint avail = 1;

			// Go from end to start along the shortest paths found to
			// fill in the paths graph.
			
			while (cur != avail)
				{
				for (uint i = 0; i < graph.GetLength(0); ++i)
					{

					// Rows in the graph indicate outgoing edges from the vertex.
					// Columns in the graph indicate incoming edges to the vertex.

					// I'm checking down column vertexes[cur] to see if there
					// in an incoming edge from vertex i.
					
					if (graph[i, vertexes[cur]] > 1)
						{

						// If the edge was part of a shortest path, eg. distance
						// of current vertex is the sum of the distance of the
						// incoming vertex plus the edge, then the incoming vertex
						// was part of a shortest path.
						
						if (distance[vertexes[cur]] ==
								(distance[i] + (graph[i, vertexes[cur]] - 1)))
							{

							// This incoming vertex was part of a shortest path
							// so copy over its edge from the original graph.
							
							paths[i, vertexes[cur]] = graph[i, vertexes[cur]];

							// If we didn't handle this incoming vertex already
							// add it to the queue.
							
							if (handledVertex[i] == false)
								{
								vertexes[avail++] = i;
								handledVertex[i] = true;
								}
							}
						}
					}

				// Move to next vertex to process.
				
				cur++;
				}

			return paths;
			}
	}
}

public class Application
{
	private static void ShowPaths(uint[,] graph, uint vertex, uint end,
			uint[] path, uint verticies, uint length)
	{

		// Add vertex to path.
		
		path[verticies++] = vertex;

		// If this vertex is the end then dump out the path.
		
		if (vertex == end)
			{

			// Dump path.  Format a bit to ensure long paths are
			// reasonably displayed.
			
			Console.WriteLine("path, length = {0}:\n", length);
			for (uint i = 0; i < verticies; ++i)
				{
				Console.Write("{0}", path[i]);
				if (i != (verticies - 1))
					Console.Write(",");
				if (((i + 1) % 16) == 0)
					Console.WriteLine();
				else
					Console.Write(" ");
				}
			Console.WriteLine("\n");
			}

		// If we're not at the end then recurse with all outgoing
		// verticies (rows) of this vertex.
		
		else
			{
			for (uint i = 0; i < (uint) graph.GetLength(0); ++i)
				{

				// Path between this vertex and i?
				
				if (graph[vertex, i] != 0)
					{

					// Yup, recurse with this outgoing vertex (i).
					
					ShowPaths(graph, i, end, path, verticies, length +
							graph[vertex, i] - 1);
					}
				}
			}
	}
	
	public static void Main(string[] args)
	{
		uint[,] graph = new uint[,] {
				{1, 4, 0, 5, 0, 13},
				{0, 1, 2, 0, 2, 0},
				{0, 0, 1, 0, 2, 3},
				{0, 0, 6, 1, 5, 0},
				{0, 0, 0, 0, 1, 3},
				{0, 0, 0, 0, 0, 1}};

		uint start = 0;
		uint end = 5;
		
		// Calculate shortest path(s).
		
		uint[,] paths = Dijkstra.ShortestPaths(graph, start, end);

		if (paths == null)
			Console.WriteLine("No paths found from {0} to {1}", start, end);
		else
			{

			// Print out the paths.  A path can't be longer than the number
			// of verticies.

			uint[] path = new uint[paths.GetLength(0)];
			uint verticies = 0;
			uint length = 0;			
			ShowPaths(paths, start, end, path, verticies, length);
			}
	}
}
