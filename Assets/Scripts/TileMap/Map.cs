using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public int width = 5;
	public int height = 5;

	public GameObject floor;
	public GameObject obstacle;

	public List<Cell> cells = new List<Cell>();
	public Graph graph = new Graph();
	public List<GameObject> cellObjects = new List<GameObject>();

	private float cellSize;

	void Start(){
		cellSize = floor.GetComponent<SpriteRenderer>().size.x * 1.1f;

		InitMaps(width, height);

		List<GraphEdge> path;
		GraphSearch.AStar(graph, graph.GetNode(0), graph.GetNode(99), Heuristic, out path);
		float cost = 0;
		foreach(GraphEdge e in path){
			Debug.Log(e.from.index + " - " + e.to.index);
			cost += e.cost;

			cellObjects[e.from.index].GetComponent<SpriteRenderer>().color = Color.red;
		}
		Debug.Log("Total Cost: " + cost);
	}

	private float Heuristic(GraphNode a, GraphNode b){
		Cell c1 = (Cell)a;
		Cell c2 = (Cell)b;

		return Mathf.Abs(c1.position.x - c2.position.x) + Mathf.Abs(c1.position.y - c2.position.y);
	}

	private void InitMaps(int width, int height){
		// Add cells
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				Cell newCell = new Cell(i * width + j, new Vector2(j, i) * cellSize, false);
				cells.Add(newCell);
			}
		}

		foreach(Cell c in cells){
			graph.AddNode(c);
		}

		// Add obstacles
		cells[2].isObstacle = true;
		cells[7].isObstacle = true;
		cells[12].isObstacle = true;
		cells[13].isObstacle = true;	

		// Horizontal
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width - 1; j++){
				Cell c1 = cells[i * width + j];
				Cell c2 = cells[i * width + j + 1];
				if(!c1.isObstacle && !c2.isObstacle){
					graph.AddEdge(c1, c2);
					graph.AddEdge(c2, c1);
				}
			}
		}
		// Vertical
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height - 1; j++){
				Cell c1 = cells[j * width + i];
				Cell c2 = cells[(j + 1) * width + i];
				if(!c1.isObstacle && !c2.isObstacle){
					graph.AddEdge(c1, c2);
					graph.AddEdge(c2, c1);
				}
			}
		}	

		// Draw
		foreach(Cell c in cells){
			GameObject cell;
			if(!c.isObstacle)
				cell = Instantiate(floor, c.position, Quaternion.identity);
			else
				cell = Instantiate(obstacle, c.position, Quaternion.identity);

			cell.transform.parent = transform;
			cellObjects.Add(cell);
		}
	}
}
