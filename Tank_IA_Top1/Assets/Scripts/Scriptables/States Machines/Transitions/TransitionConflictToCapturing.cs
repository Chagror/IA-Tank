using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition Conflict To Capturing")]
public class TransitionConflictToCapturing : Transition
{
    private PointZoneManager _zone;

    public override void Initialize(StateMachineController controller)
    {
        _zone = controller.GetComponent<PointZoneManager>();
    }

    public override bool Check()
    {
        return _zone.newTeam == _zone.currentTeam;
    }
}