using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition Idle To Capturing")]
public class TransitionIdleToCapturing : Transition
{
    private PointZoneManager _zone;

    public override void Initialize(StateMachineController controller)
    {
        _zone = controller.GetComponent<PointZoneManager>();
    }

    public override bool Check()
    {
        return _zone.newTeam != null;
    }
}