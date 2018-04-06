using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the graph's edge.
/// </summary>
public class GraphEdge{

	/// ------	Public Variables	------
	public GraphNode from = null;
	public GraphNode to = null;
	public float cost = 1f;

	#region Constructors
	public GraphEdge(){}

	public GraphEdge(GraphNode f, GraphNode t){
		from = f;
		to = t;

		f.edges.Add(this);
	}

	public GraphEdge(GraphNode f, GraphNode t, float c) : this(f, t){
		cost = c;
	}
	#endregion


}
