using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Transform obstaclesContainer;

	public static List<Collider2D> obstacles = new List<Collider2D>();

	void Awake(){
		foreach(Collider2D ob in obstaclesContainer.GetComponentsInChildren<Collider2D>())
			obstacles.Add(ob);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
