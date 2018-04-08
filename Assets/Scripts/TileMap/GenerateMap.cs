using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateMap : MonoBehaviour {

	public int width = 5;
	public int height = 5;

	public GameObject floor;
	public GameObject wall;

	public List<Cell> cells = new List<Cell>();	
	public Graph graph = new Graph();

	private float cellSize;


	// Use this for initialization
	void Start () {		
		cellSize = floor.GetComponent<SpriteRenderer>().size.x;

		InitMap();

		PrimMaze(graph, graph.GetNode(0));

		DrawMaze();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Init the map
	/// </summary>
	private void InitMap(){
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

		// Horizontal
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width - 1; j++){
				Cell c1 = cells[i * width + j];
				Cell c2 = cells[i * width + j + 1];
				if(!c1.isObstacle && !c2.isObstacle){
					graph.AddEdge(new MazeEdge(c1, c2, true));
					graph.AddEdge(new MazeEdge(c2, c1, true));
				}
			}
		}
		// Vertical
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height - 1; j++){
				Cell c1 = cells[j * width + i];
				Cell c2 = cells[(j + 1) * width + i];
				if(!c1.isObstacle && !c2.isObstacle){
					graph.AddEdge(new MazeEdge(c1, c2, true));
					graph.AddEdge(new MazeEdge(c2, c1, true));
				}
			}
		}	
	}

	/// <summary>
	/// Generate a maze by DFS
	/// </summary>
	private void DFSMaze(Graph g, GraphNode s){
		Dictionary<GraphNode, bool> NodeIsVisited = new Dictionary<GraphNode, bool>();
		foreach(GraphNode n in g.nodes){
			NodeIsVisited.Add(n, false);
		}

		dfs(g, s, NodeIsVisited);
	}

	/// <summary>
	/// DFS step
	/// </summary>
	private void dfs(Graph g, GraphNode n, Dictionary<GraphNode, bool> NodeIsVisited){
		NodeIsVisited[n] = true;

		GraphEdge e;
		while((e = TryGetEdge(n, NodeIsVisited)) != null){
			((MazeEdge)e).isWall = false;
				((MazeEdge)g.GetEdge(e.to, n)).isWall = false;

				dfs(g, e.to, NodeIsVisited);
		}
	}

	/// <summary>
	/// Generate a maze by Prim
	/// </summary>
	private void PrimMaze(Graph g, GraphNode s){
		Dictionary<GraphNode, bool> NodeIsVisited = new Dictionary<GraphNode, bool>();
		foreach(GraphNode n in g.nodes){
			NodeIsVisited.Add(n, false);
		}
		NodeIsVisited[s] = true;

		List<MazeEdge> walls = new List<MazeEdge>();
		foreach(GraphEdge e in s.edges){
			walls.Add((MazeEdge) e);
		}

		while(walls.Count > 0){
			MazeEdge e = walls[Random.Range(0, walls.Count)];
			walls.Remove(e);

			if(NodeIsVisited[e.to] == false){
				NodeIsVisited[e.to] = true;
				e.isWall = false;
				((MazeEdge)g.GetEdge(e.to, e.from)).isWall = false;

				foreach(MazeEdge me in e.to.edges){
					walls.Add(me);
				}
			}
		}
	}

	private GraphEdge TryGetEdge(GraphNode n, Dictionary<GraphNode, bool> NodeIsVisited){
		List<GraphEdge> es = new List<GraphEdge>(n.edges);
		foreach(GraphEdge e in n.edges){
			if(NodeIsVisited[e.to] == true)
				es.Remove(e);
		}

		if(es.Count == 0)
			return null;
		else
			return es[Random.Range(0, es.Count)];
	}

	/// <summary>
	/// Draw the maze
	/// </summary>
	private void DrawMaze(){
		// Draw
		foreach(Cell c in cells){
			GameObject cell;
			cell = Instantiate(floor, c.position, Quaternion.identity);

			cell.transform.parent = transform;
		}

		// Outer Walls
		for(int i = 0; i < width; i++){
			Instantiate(wall, new Vector2(i, -0.5f) * cellSize, Quaternion.identity);
			Instantiate(wall, new Vector2(i, height - 0.5f) * cellSize, Quaternion.identity);
		}
		for(int i = 0; i < height; i++){
			GameObject wallObject;
			wallObject = Instantiate(wall, new Vector2(-0.5f, i) * cellSize, Quaternion.identity);
			wallObject.transform.Rotate(0, 0, 90);
			wallObject = Instantiate(wall, new Vector2(width - 0.5f, i) * cellSize, Quaternion.identity);
			wallObject.transform.Rotate(0, 0, 90);
		}

		// Inner Walls
		foreach(MazeEdge e in graph.edges){
			if(!e.isWall)
				continue;

			Cell c1 = (Cell)e.from;
			Cell c2 = (Cell)e.to;

			Vector2 position = (c1.position + c2.position) * 0.5f;

			GameObject wallObject = Instantiate(wall, position, Quaternion.identity);

			if((c1.position.y == c2.position.y))
				wallObject.transform.Rotate(0, 0, 90);
		}
	}
}
