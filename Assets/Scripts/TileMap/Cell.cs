using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : GraphNode {

	public Vector2 position = Vector2.zero;
	public bool isObstacle = false;

	public Cell(int index, Vector2 position, bool isObstacle){
		this.index = index;
		this.position = position;
		this.isObstacle = isObstacle;
	}
}
