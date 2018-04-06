using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraph : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Graph g = new Graph();

		for(int i = 1; i < 6; i++){
			g.AddNode(i);
		}

		g.AddEdge(1, 2);
		g.AddEdge(1, 3);
		g.AddEdge(2, 4, 7);
		g.AddEdge(4, 5);
		g.AddEdge(3, 5, 5f);


		List<GraphEdge> path;
		GraphSearch.Dijkstra(g, g.GetNode(1), g.GetNode(5), out path);
		float cost = 0;
		foreach(GraphEdge e in path){
			Debug.Log(e.from.index + " - " + e.to.index);
			cost += e.cost;
		}
		Debug.Log("Total Cost: " + cost);
	}
}
