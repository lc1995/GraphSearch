using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Timer allows you to get the time cost of a function.
/// </summary>
public class Timer{

	public delegate void function();

	public static void Play(function f){
		float time = Time.realtimeSinceStartup;
		f();
		
		Debug.Log(f.Method.Name + " cost: " + (Time.realtimeSinceStartup - time).ToString("F3") + "s");
	}
}
