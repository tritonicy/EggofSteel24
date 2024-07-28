using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) {
        InitializeSubstate();
        isRoot = true;
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
        HandleJump();
        CheckSwitchState();
    }

    private void HandleJump() {

        context.coyoteCounter -= Time.deltaTime;

        if(context.isJumpPressed) {
            context.jumpBufferCounter = context.jumpBufferTime;
        }
        else{
            context.jumpBufferCounter -= Time.deltaTime;
        }

        if(context.coyoteCounter > 0 && context.jumpBufferCounter > 0f) {
            context.rb.velocity = new Vector3(context.rb.velocity.x, context.jumpSpeed ,context.rb.velocity.z);

            context.isJumping = true;
            context.jumpBufferCounter = 0f;
        }

        if(!context.isJumpPressed && context.rb.velocity.y > 0f && context.coyoteCounter < 0f) {
            context.rb.velocity = new Vector3(context.rb.velocity.x, context.rb.velocity.y * 0.8f ,context.rb.velocity.z);

            context.coyoteCounter = 0f;
        }
    }
}
