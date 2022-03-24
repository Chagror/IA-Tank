using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Empty Task")]
public class SMEmptyTask : SMTask
{
    
    private PointZoneManager pointZoneManager;
    public override void Initialize(StateMachineController controller)
    {
        pointZoneManager = controller.GetComponent<PointZoneManager>();
    }
    
    public override void RunTask()
    {
        
    }
}