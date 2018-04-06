using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSearch{

	private const float Infinity = float.MaxValue * 0.5f;

	/// <summary>
	/// Depth first search
	/// </summary>
	public static bool DFS(Graph g, GraphNode s, GraphNode t, out List<GraphEdge> result){
		List<GraphEdge> path = new List<GraphEdge>();

		Dictionary<GraphNode, bool> nodesRecord = InitNodeTagVisited(g);
		if(!dfs(s, t, nodesRecord, path)){
			result = path;
			Debug.Log("Depth First Search fails.");
			return false;
		}else{
			result = path;
			result.Reverse();
			return true;
		}
	}
	private static bool dfs(GraphNode n, GraphNode t, Dictionary<GraphNode, bool> nodesRecord, List<GraphEdge> path){
		nodesRecord[n] = true;
		foreach(GraphEdge e in n.edges){
			if(e.to == t){
				path.Add(e);
				return true;
			}
			if(nodesRecord[e.to] == false)
				if(dfs(e.to, t, nodesRecord, path)){
					path.Add(e);
					return true;
				}
		}

		return false;
	}

	/// <summary>
	/// Breadth first search
	/// </summary>
	public static bool BFS(Graph g, GraphNode s, GraphNode t, out List<GraphEdge> result){
		List<GraphEdge> path = new List<GraphEdge>();

		Dictionary<GraphNode, GraphNode> nodesRecord = InitNodeTagFrom(g);
		Queue<GraphNode> nodesQueue = new Queue<GraphNode>();
		nodesRecord[s] = s;
		nodesQueue.Enqueue(s);

		while(nodesQueue.Count > 0){
			GraphNode n = nodesQueue.Dequeue();

			foreach(GraphEdge e in n.edges){
				if(nodesRecord[e.to] == null){
					nodesRecord[e.to] = n;

					if(e.to == t){
						nodesQueue.Clear();
						break;
					}
					nodesQueue.Enqueue(e.to);
				}
			}
		}

		GraphNode tmp = t;
		while(tmp != s){
			path.Add(g.GetEdge(nodesRecord[tmp], tmp));
			tmp = nodesRecord[tmp];
		}

		result = path;
		result.Reverse();
		return false;
	}

	/// <summary>
	/// Dijkstra search
	/// </summary>
	public static bool Dijkstra(Graph g, GraphNode s, GraphNode t, out List<GraphEdge> result){
		List<GraphEdge> path = new List<GraphEdge>();
		result = path;

		Dictionary<GraphNode, GraphNode> nodesFrom = InitNodeTagFrom(g);
		Dictionary<GraphNode, float> nodesCost = InitNodeTagCost(g);
		List<GraphNode> candidateNodes = new List<GraphNode>();
		nodesFrom[s] = s;
		nodesCost[s] = 0;
		candidateNodes.Add(s);

		while(candidateNodes.Count > 0){
			GraphNode node = ExtractMin(candidateNodes, nodesCost);
			if(node == t)
				break;

			foreach(GraphEdge e in node.edges){
				if(nodesCost[node] + e.cost < nodesCost[e.to]){
					nodesCost[e.to] = nodesCost[node] + e.cost;
					nodesFrom[e.to] = node;
					candidateNodes.Add(e.to);
				}
			}
		}

		if(nodesFrom[t] == null)
			return false;
		
		GraphNode tmp = t;
		while(tmp != s){
			GraphNode ftmp = nodesFrom[tmp];
			path.Add(g.GetEdge(ftmp, tmp));
			tmp = ftmp;
		}

		result = path;
		result.Reverse();
		return true;
	}


	public delegate float Heuristic(GraphNode s, GraphNode t);
	/// <summary>
	/// AStar
	/// </summary>
	public static bool AStar(Graph g, GraphNode s, GraphNode t, Heuristic heuristic, out List<GraphEdge> result){
		List<GraphEdge> path = new List<GraphEdge>();
		result = path;

		Dictionary<GraphNode, GraphNode> nodesFrom = InitNodeTagFrom(g);
		Dictionary<GraphNode, float> nodesCost = InitNodeTagCost(g);
		Dictionary<GraphNode, float> nodesHeuristic = InitNodeTagCost(g);
		List<GraphNode> candidateNodes = new List<GraphNode>();
		nodesFrom[s] = s;
		nodesCost[s] = 0;
		nodesHeuristic[s] = heuristic(s, t);
		candidateNodes.Add(s);

		while(candidateNodes.Count > 0){
			GraphNode node = ExtractMin(candidateNodes, nodesCost, heuristic, t);
			if(node == t)
				break;

			foreach(GraphEdge e in node.edges){
				if(nodesCost[node] + e.cost < nodesCost[e.to]){
					nodesCost[e.to] = nodesCost[node] + e.cost;
					nodesFrom[e.to] = node;
					candidateNodes.Add(e.to);
				}
			}
		}

		if(nodesFrom[t] == null)
			return false;
		
		GraphNode tmp = t;
		while(tmp != s){
			GraphNode ftmp = nodesFrom[tmp];
			path.Add(g.GetEdge(ftmp, tmp));
			tmp = ftmp;
		}

		result = path;
		result.Reverse();
		return true;
	}

	/// <summary>
	/// Tag each node with tag representing visited or not.
	/// </summary>
	private static Dictionary<GraphNode, bool> InitNodeTagVisited(Graph g){
		Dictionary<GraphNode, bool> nodesWithTag = new Dictionary<GraphNode, bool>();

		foreach(GraphNode n in g.nodes){
			nodesWithTag.Add(n, false);
		}

		return nodesWithTag;
	}

	/// <summary>
	/// Tag each node with tag representing which node it comes from.
	/// </summary>
	private static Dictionary<GraphNode, GraphNode> InitNodeTagFrom(Graph g){
		Dictionary<GraphNode, GraphNode> nodesWithTag = new Dictionary<GraphNode, GraphNode>();

		foreach(GraphNode n in g.nodes){
			nodesWithTag.Add(n, null);
		}

		return nodesWithTag;
	}

	/// <summary>
	/// Tag each node with its cost
	/// </summary>
	private static Dictionary<GraphNode, float> InitNodeTagCost(Graph g){
		Dictionary<GraphNode, float> nodesWithTag = new Dictionary<GraphNode, float>();

		foreach(GraphNode n in g.nodes){
			nodesWithTag.Add(n, Infinity);
		}

		return nodesWithTag;
	}

	/// <summary>
	/// Extract the node with the minimum cost
	/// </summary>
	private static GraphNode ExtractMin(List<GraphNode> nodes, Dictionary<GraphNode, float> nodesCost){
		float cost = Infinity;
		GraphNode minNode = null;

		foreach(GraphNode n in nodes){
			if(nodesCost[n] < cost){
				cost = nodesCost[n];
				minNode = n;
			}
		}

		nodes.Remove(minNode);

		return minNode;
	}

	/// <summary>
	/// Extract the node with the minimum cost(real cost + heuristic cost)
	/// </summary>
	private static GraphNode ExtractMin(List<GraphNode> nodes, Dictionary<GraphNode, float> nodesCost, Heuristic heuristic, GraphNode t){
		float cost = Infinity;
		GraphNode minNode = null;

		foreach(GraphNode n in nodes){
			float hCost = heuristic(n, t);
			if(nodesCost[n] + hCost < cost){
				cost = nodesCost[n] + hCost;
				minNode = n;
			}
		}

		nodes.Remove(minNode);

		return minNode;
	}
}
