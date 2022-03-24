using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition Capturing To Uncapturing")]
public class TransitionCapturingToUncapturing : Transition
{
    private int TankAmountToLoseTheZone = 0;
    private PointZoneManager _zone;

    public override void Initialize(StateMachineController controller)
    {
        _zone = controller.GetComponent<PointZoneManager>();
    }

    public override bool Check()
    {
        return _zone.GetTanks().Count <= TankAmountToLoseTheZone || _zone.currentTeam != _zone.newTeam;
    }
    
    
}