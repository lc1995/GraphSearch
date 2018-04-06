using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the graph's node.
/// </summary>
public class GraphNode{

	/// ------	Public Variables	------
	public int index;
	public List<GraphEdge> edges = new List<GraphEdge>();

	#region Constructor
	public GraphNode(){}

	public GraphNode(int i){
		index = i;
	}

	public GraphNode(int i, List<GraphEdge> es) : this(i){
		edges = es;
	}
	#endregion
}
