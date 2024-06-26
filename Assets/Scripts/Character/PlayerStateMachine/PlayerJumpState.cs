using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) { 
        
    }

    public override void CheckSwitchState()
    {
        if(context.isGrounded) {
            SwitchState(states.Grounded());
        }
    }

    public override void EnterState()
    {
        
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

    private void HandleJump() {

    }
    
    private void HandleGravity() {
        
    }
}
