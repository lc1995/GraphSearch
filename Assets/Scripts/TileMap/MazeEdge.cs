using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEdge : GraphEdge {

	public bool isWall;

	public MazeEdge(Cell c1, Cell c2, bool isW=true){
		from = c1;
		to = c2;
		isWall = isW;

		c1.edges.Add(this);
	}
}
