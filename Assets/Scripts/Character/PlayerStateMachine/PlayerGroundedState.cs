using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) { 
        
    }
    public override void CheckSwitchState()
    {
        // if a player is grounded and jump is pressed, switch to jump state
        if(context.isJumpPressed && context.isGrounded) {
            SwitchState(states.Jump());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Hello1");
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubstate()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
