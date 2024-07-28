using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState (PlayerStateMachine context, PlayerStateFactory states) : base(context,states) { 
        
    }
    public override void CheckSwitchState()
    {
        if(context.isDashPressed) {
            SwitchState(states.Dashing());
        } else if(!context.isWalkingPressed) {
            SwitchState(states.Idle());
        }
    }

    public override void EnterState()
    {
        //TODO : animasyon setboollari ayarla buraya
        // Debug.Log("Walk");
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
