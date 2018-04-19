using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public float edgeSize = 0.1f;

	public GameObject cellPrefab;
	public GameObject edgePrefab;

	public Graph graph = new Graph();

	private Transform cellsContainer;
	private Transform edgesContainer;

	private float edgePrefabLength = 1f;

	void Start(){
		edgePrefabLength = edgePrefab.GetComponent<SpriteRenderer>().size.x;

		Timer.Play(InitMaps);

		Debug.Log(graph.nodes.Count);
	}

	private float Heuristic(GraphNode a, GraphNode b){
		Cell c1 = (Cell)a;
		Cell c2 = (Cell)b;

		return Mathf.Abs(c1.position.x - c2.position.x) + Mathf.Abs(c1.position.y - c2.position.y);
	}

	private void InitMaps(){
		// Initialize containers
		InitContainers();

		Queue<Cell> candidateCells = new Queue<Cell>();

		// Starting point
		Vector2 point = transform.position;
		Cell newCell;
		int cellIndex = 0;

		// Check starting point
		if(!CheckPointInObstacle(point)){
			newCell = new Cell(cellIndex, point);
			graph.AddNode(newCell);
			candidateCells.Enqueue(newCell);
			cellIndex++;
		}else{
			return;
		}

		// Extend edges and nodes
		while(candidateCells.Count > 0){
			Cell candidate = candidateCells.Dequeue();

			// Left
			point = candidate.position + new Vector2(-edgeSize, 0);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex);
			// Right
			point = candidate.position + new Vector2(edgeSize, 0);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex);
			// Top
			point = candidate.position + new Vector2(0, edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex);
			// Bottom
			point = candidate.position + new Vector2(0, -edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex);

			// Left Top
			point = candidate.position + new Vector2(-edgeSize, edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex, 1.414f);

			// Left Bottom
			point = candidate.position + new Vector2(-edgeSize, -edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex, 1.414f);

			// Right Top
			point = candidate.position + new Vector2(edgeSize, edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex, 1.414f);

			// Right Bottom
			point = candidate.position + new Vector2(edgeSize, -edgeSize);
			ExtendEdgeAndNode(point, candidate, candidateCells, ref cellIndex, 1.414f);
		}

		// Draw cells
		DrawCell(graph.nodes);

		// Draw edges
		DrawEdge(graph.edges);
	}

	private void ExtendEdgeAndNode(Vector2 point, Cell candidate, Queue<Cell> candidateCells, ref int cellIndex, float cost=1){
		if(CheckLineInObstacle(candidate.position, point))
			return;

		Cell cell = CheckPointAlreadyCreated(point);
		if(cell == null){
			cell = new Cell(cellIndex, point);
			graph.AddNode(cell);
			candidateCells.Enqueue(cell);
			cellIndex++;

			graph.AddEdge(candidate, cell);
		}else{
			graph.AddEdge(candidate, cell);
		}
	}

	private Cell CheckPointAlreadyCreated(Vector2 point){
		foreach(Cell c in graph.nodes){
			if((c.position - point).magnitude < edgeSize * 0.5f)
				return c;
		}

		return null;
	}

	private bool CheckPointInObstacle(Vector2 point){
		foreach(Collider2D ob in GameManager.obstacles){
			if(ob.bounds.Contains(point))
				return true;
		}

		return false;
	}

	private bool CheckLineInObstacle(Vector2 from, Vector2 to){
		if(from == to)
			return false;

		RaycastHit2D ray = Physics2D.Raycast(from, to - from, (to - from).magnitude);
		
		if(ray.collider == null)
			return false;
		else
			return true;
	}

	private void InitContainers(){
		cellsContainer = new GameObject().transform;
		cellsContainer.name = "Cells";
		cellsContainer.parent = transform;

		edgesContainer = new GameObject().transform;
		edgesContainer.name = "Edges";
		edgesContainer.parent = transform;
	}

	private void DrawCell(List<GraphNode> cells){
		foreach(Cell c in cells){
			GameObject cell = Instantiate(cellPrefab, c.position, Quaternion.identity);
			cell.transform.parent = cellsContainer;
		}
	}

	private void DrawEdge(List<GraphEdge> edges){
		foreach(GraphEdge e in edges){
			Cell c1 = (Cell)e.from;
			Cell c2 = (Cell)e.to;

			Vector2 position = (c1.position + c2.position) * 0.5f;
			Quaternion rotation = Quaternion.Euler(0, 0, Vector2Angle(c2.position - c1.position));

			GameObject edge = Instantiate(edgePrefab, position, rotation);
			edge.transform.parent = edgesContainer;

			float edgeLength = (c2.position - c1.position).magnitude;
			edge.transform.localScale += new Vector3(edgeLength / edgePrefabLength - 1, 0, 0);
		}
	}

	private float Vector2Angle(Vector2 v){
		float rad = Mathf.Atan2(v.y, v.x);

		return Mathf.Rad2Deg * rad;
	}
}
