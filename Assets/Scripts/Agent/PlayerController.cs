using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Transform mapManager;
	public float speed = 5f;

	private Graph graph = new Graph();
	private Vector2 targetPosition;
	private Cell closestSourceNode;
	private Cell closestTargetNode;
	private Queue<Vector2> path = new Queue<Vector2>();

	private Vector2 currentTarget = Vector2.zero;
	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		graph = mapManager.GetComponent<Map>().graph;
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			// Get position
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if(!CheckPointInObstacle(targetPosition)){
				// Reset path
				path.Clear();
				currentTarget = Vector2.zero;

				// Get Closest Node
				float sourceDist = float.MaxValue;
				float targetDist = float.MaxValue;
				foreach(Cell c in graph.nodes){
					// Source
					float distance = (c.position - (Vector2)transform.position).sqrMagnitude;
					if(distance < sourceDist){
						sourceDist = distance;
						closestSourceNode = c;
					}

					// Target
					distance = (c.position - (Vector2)targetPosition).sqrMagnitude;
					if(distance < targetDist){
						targetDist = distance;
						closestTargetNode = c;
					}
				}

				// Plan Path
				List<GraphEdge> edges = new List<GraphEdge>();
				if(!GraphSearch.AStar(graph, closestSourceNode, closestTargetNode, Euclidean, out edges)){
					Debug.Log("No path from source to target.");
					return;
				}
				/* 
				path.Enqueue(closestSourceNode.position);
				foreach(GraphEdge e in edges)
					path.Enqueue(((Cell)e.to).position);
				path.Enqueue(targetPosition);
				*/
				path = PathSmoothing(edges);

				currentTarget = path.Dequeue();
				isMoving = true;
			}
		}

		if(!isMoving)
			return;

		// Check is target arrived at
		if(Mathf.Approximately(((Vector2)transform.position - currentTarget).sqrMagnitude, 0f)){
			if(path.Count != 0)
				currentTarget = path.Dequeue();
			else
				isMoving = false;
		}

		// Move
		transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
	}

	private Queue<Vector2> PathSmoothing(List<GraphEdge> edges){
		Queue<Vector2> path = new Queue<Vector2>();
		Vector2 p1 = transform.position;
		Vector2 p2;

		while(edges.Count != 0){
			p2 = ((Cell)edges[0].to).position;

			if(!CheckLineInObstacle(p1, p2)){
				// path.Enqueue(p2);
			}else{
				p1 = ((Cell)edges[0].from).position;
				path.Enqueue(p1);
			}

			edges.RemoveAt(0);
		}

		p2 = targetPosition;
		if(CheckLineInObstacle(p1, targetPosition)){
			path.Enqueue(p2);
		}
		path.Enqueue(targetPosition);

		return path;
	}



	private float Manhattan(GraphNode a, GraphNode b){
		Cell c1 = (Cell)a;
		Cell c2 = (Cell)b;

		return Mathf.Abs(c1.position.x - c2.position.x) + Mathf.Abs(c1.position.y - c2.position.y);
	}

	private float Euclidean(GraphNode a, GraphNode b){
		Cell c1 = (Cell)a;
		Cell c2 = (Cell)b;

		return (c1.position - c2.position).magnitude;
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
}
