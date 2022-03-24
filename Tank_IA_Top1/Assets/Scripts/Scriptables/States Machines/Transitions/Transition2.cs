using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition 2")]
public class Transition2 : Transition
{
    private Complete.TankMovement _tankMovement;
    
    public override void Initialize(StateMachineController controller)
    {
        _tankMovement = controller.GetComponent<Complete.TankMovement>();
    }

    public override bool Check()
    {
        return _tankMovement.GetMovementInputValue() <= 0;
    }
}