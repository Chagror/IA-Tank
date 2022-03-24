using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition Conflict To Uncapturing")]
public class TransitionConflictToUncapturing : Transition
{
    private PointZoneManager _zone;

    public override void Initialize(StateMachineController controller)
    {
        _zone = controller.GetComponent<PointZoneManager>();
    }

    public override bool Check()
    {
        return _zone.newTeam != _zone.currentTeam && _zone.GetTeamAmount() < 2;
    }
}