using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 v1 = new Vector2(-1, 0);
		Vector2 v2 = new Vector2(-1, 1);

		Debug.Log(Vector2.Angle(v1, v2));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
