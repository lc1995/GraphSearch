using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

[Action("Basic Action/GetMousePosition")]
[Help("Get mouse position in 2D space")]
public class GetMousePosition : BasePrimitiveAction{

    [OutParam("position")]
    [Help("Mouse position")]
    public Vector2 position;

    public override void OnStart(){
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override TaskStatus OnUpdate(){
        return TaskStatus.COMPLETED;
    }
}

