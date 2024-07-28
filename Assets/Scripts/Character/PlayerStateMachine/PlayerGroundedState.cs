using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) { 
        InitializeSubstate();
        isRoot = true;
    }
    public override void CheckSwitchState()
    {
        // if a player is grounded and jump is pressed, switch to jump state
        if(context.isJumpPressed || !context.isGrounded) {
            SwitchState(states.Jump());
        }
        if(context.jumpBufferCounter > 0f) {

            context.rb.velocity = new Vector3(context.rb.velocity.x, 5f ,context.rb.velocity.z);

            context.isJumping = true;
            context.jumpBufferCounter = 0f;
            SwitchState(states.Jump());
        }
    }

    public override void EnterState()
    {
        context.isJumping = false;
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubstate()
    {
        if(context.isDashPressed) {
            SetSubstate(states.Dashing());
        }
        else if (context.isWalkingPressed) {
            SetSubstate(states.Walk());
        }
        else{
            SetSubstate(states.Idle());
        }
    }

    public override void UpdateState()
    {
        context.coyoteCounter = context.coyoteTime;
        CheckSwitchState();
    }
}
