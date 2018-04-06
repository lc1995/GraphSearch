using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the graph.
/// </summary>
public class Graph{

	public List<GraphNode> nodes = new List<GraphNode>();
	public List<GraphEdge> edges = new List<GraphEdge>();

	public int NodesCount{
		get{
			return nodes.Count;
		}
		private set{
			Debug.LogWarning("It's not valid to set the count of nodes.");
		}
	}
	public int EdgesCount{
		get{
			return edges.Count;
		}
		private set{
			Debug.LogWarning("It's not valid to set the count of edges");
		}
	}

	#region Constructors
	public Graph(){}

	public Graph(List<GraphNode> ns){
		nodes = ns;
	}

	public Graph(List<GraphNode> ns, List<GraphEdge> es) : this(ns){
		edges = es;
	}
	#endregion

	/// <summary>
	/// Add node to the graph
	/// </summary>
	public void AddNode(GraphNode node){
		if(GetNode(node.index) == null)
			nodes.Add(node);
		else
			Debug.LogWarning("Node " + node.index + " is already been added.");	
	}
	public void AddNode(int index){
		AddNode(new GraphNode(index));
	}

	/// <summary>
	/// Add edge to the graph
	/// </summary>
	public void AddEdge(GraphEdge edge){
		if(GetEdge(edge.from.index, edge.to.index) == null)
			edges.Add(edge);
		else
			Debug.LogWarning("Edge " + edge.from.index + " - " + edge.to.index + " is already been added.");
	}
	public void AddEdge(GraphNode n1, GraphNode n2, float cost=1f){
		AddEdge(new GraphEdge(n1, n2, cost));
	}
	public void AddEdge(int n1i, int n2i, float cost=1f){
		GraphNode n1 = GetNode(n1i);
		GraphNode n2 = GetNode(n2i);

		if(n1 != null && n2 != null)
			AddEdge(n1, n2, cost);
	}

	/// <summary>
	/// Get a node from the graph by an index
	/// </summary>
	public GraphNode GetNode(int node){
		foreach(GraphNode n in nodes){
			if(n.index == node)
				return n;
		}

		return null;
	}

	/// <summary>
	/// Get an edge from the graph by two indexes
	/// </summary>
	public GraphEdge GetEdge(int from, int to){
		foreach(GraphEdge e in edges){
			if(e.from.index == from && e.to.index == to)
				return e;
		}

		return null;
	}

	/// <summary>
	/// Get edges from the graph by the target's index of the node
	/// </summary>
	public List<GraphEdge> GetEdgesWithTo(int to){
		List<GraphEdge> es = new List<GraphEdge>();
		foreach(GraphEdge e in edges){
			if(e.to.index == to)
				es.Add(e);
		}

		return es;
	}

	/// <summary>
	/// Get edges from the graph by the target's node
	/// </summary>
	public List<GraphEdge> GetEdgesWithTo(GraphNode to){
		return GetEdgesWithTo(to.index);
	}

	/// <summary>
	/// Get an edge from the graph by two nodes
	/// </summary>
	public GraphEdge GetEdge(GraphNode from, GraphNode to){
		return GetEdge(from.index, to.index);
	}

	/// <summary>
	/// Remove a node from the graph by index
	/// </summary>
	public void RemoveNode(int node){
		GraphNode n = GetNode(node);
		if(n != null)
			nodes.Remove(n);
		else
			Debug.LogWarning("Node " + node + " is already been removed.");
	}

	/// <summary>
	/// Remove an edge from the graph by two indexes
	/// </summary>
	public void RemoveEdge(int from, int to){
		GraphEdge e = GetEdge(from, to);
		if(e != null)
			edges.Remove(e);
		else
			Debug.LogWarning("Edge " + edges + " is already been removed.");
	}
}
