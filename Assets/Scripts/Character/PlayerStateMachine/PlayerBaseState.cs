using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine context;
    protected PlayerStateFactory states;
    protected PlayerBaseState superState;
    protected PlayerBaseState subState;
    protected bool isRoot = false;

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

    public void UpdateStates() {
        UpdateState();

        if(subState != null) {
            subState.UpdateStates();
        }
    }
    protected void SwitchState(PlayerBaseState newState) {
        context.CurrentState.ExitState();
        newState.EnterState();

        if(newState.isRoot) {
            context.CurrentState = newState;
        } else if(superState != null) {
            superState.SetSubstate(newState);
        }
    }
    protected void SetSuperState(PlayerBaseState newSuperState) {
        this.superState = newSuperState;
    }
    protected void SetSubstate(PlayerBaseState newSubState) {
        this.subState = newSubState;
        this.subState.SetSuperState(this);
    }
}
