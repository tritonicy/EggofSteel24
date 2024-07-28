using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine context, PlayerStateFactory states) : base(context, states)
    {
    }

    public override void CheckSwitchState()
    {
        if(context.isWalkingPressed && !context.isDashing) {
            SwitchState(states.Walk());
        }
        else if(!context.isWalkingPressed && !context.isDashing){
            SwitchState(states.Idle());
        }
    }

    public override void EnterState()
    {
        //TODO : animasyon setboollari ayarla buraya
        Dash();
        // Debug.Log("dash");
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSubstate()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    private void Dash() {
        if(context.DashTimeCounter < context.DashTimer) {
            return;
        }
        context.DashTimeCounter = 0f;
        
        context.isDashing = true;
        context.delayedForceToApply = context.transform.forward * context.dashForce;
        
        context.Invoke(nameof(context.DelayedDashForce), 0.025f);
        context.Invoke(nameof(context.ResetDash), context.dashDuration);
    }

}
