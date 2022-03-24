using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Task 2")]
public class SMTask2 : SMTask
{
    public override void Initialize(StateMachineController controller)
    {
        _controller = controller;
        Debug.Log($"Initialize task : {name}");
    }
    
    public override void RunTask()
    {
        Debug.Log($"Running task : {name}");
    }
}
