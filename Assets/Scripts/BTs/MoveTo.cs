using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using BBUnity.Actions;
using UnityEngine;

[Action("Basic Action/MoveTo")]
[Help("Moves the game object to a given position")]
public class MoveTo : GOAction{

    [InParam("target")]
    [Help("Target position")]
    public Vector2 target;

    [InParam("speed")]
    [Help("Speed")]
    public float speed;

    private Transform transform;

    public override void OnStart(){
        transform = gameObject.transform;
    }

    public override TaskStatus OnUpdate(){
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(Mathf.Approximately(transform.position.sqrMagnitude, target.sqrMagnitude))
            return TaskStatus.COMPLETED;
        else
            return TaskStatus.RUNNING;
    }
	
}
