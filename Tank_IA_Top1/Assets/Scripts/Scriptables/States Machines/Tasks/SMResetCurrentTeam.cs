using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Reset Current Team")]
public class SMResetCurrentTeam : SMTask
{
    private PointZoneManager pointZoneManager;
    public override void Initialize(StateMachineController controller)
    {
        pointZoneManager = controller.GetComponent<PointZoneManager>();
    }
    
    public override void RunTask()
    {
        pointZoneManager.currentTeam = null;
    }
}