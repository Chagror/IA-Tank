using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Task 1")]
public class SMTask1 : SMTask
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