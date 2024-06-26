using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine context;
    protected PlayerStateFactory states;
    protected PlayerBaseState superState;
    protected PlayerBaseState subState;


    protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory states)
    {
        this.context = context;
        this.states = states;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubstate();

    void UpdateStates() {

    }
    protected void SwitchState(PlayerBaseState newState) {
        context.CurrentState.ExitState();
        newState.EnterState();

        context.CurrentState = newState;
    }
    protected void SetSuperState(PlayerBaseState newSuperState) {
        this.superState = newSuperState;
    }
    protected void SetSubstate(PlayerBaseState newSubState) {
        this.subState = newSubState;
        this.subState.SetSuperState(this);
    }
}
