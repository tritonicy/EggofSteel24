using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) { 
        
    }
    public override void CheckSwitchState()
    {
        if(context.isDashPressed) {
            SwitchState(states.Dashing());
        }
        else if(context.isWalkingPressed) {
            SwitchState(states.Walk());
        }
    }

    public override void EnterState()
    {
        //TODO : animasyon setboollari ayarla buraya
        // Debug.Log("idle");
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
}
