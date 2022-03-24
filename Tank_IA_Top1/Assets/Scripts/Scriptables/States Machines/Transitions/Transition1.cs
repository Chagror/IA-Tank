using UnityEngine;

[CreateAssetMenu(menuName = "AI/State Machine/Transition/Transition 1")]
public class Transition1 : Transition
{
    private Complete.TankMovement _tankMovement;
    public float _inputThreshold = 0f;

    public override void Initialize(StateMachineController controller)
    {
        _tankMovement = controller.GetComponent<Complete.TankMovement>();
    }

    public override bool Check()
    {
        return _tankMovement.GetMovementInputValue() > _inputThreshold;
    }
}