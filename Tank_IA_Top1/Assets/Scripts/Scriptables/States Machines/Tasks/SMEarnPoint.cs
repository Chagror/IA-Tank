using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Earn Point")]
public class SMEarnPoint : SMTask
{
    private PointZoneManager pointZoneManager;
    [SerializeField] private int pointsRate;
    public override void Initialize(StateMachineController controller)
    {
        pointZoneManager = controller.GetComponent<PointZoneManager>();
    }
    
    public override void RunTask()
    {
        if(pointZoneManager.newTeam == pointZoneManager.currentTeam)
            pointZoneManager.currentTeam.AddPoint(pointsRate* Time.deltaTime);
    }
}