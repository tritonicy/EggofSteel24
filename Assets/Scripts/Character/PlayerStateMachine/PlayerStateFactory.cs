using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory : MonoBehaviour
{
    PlayerStateMachine context;

    public PlayerStateFactory(PlayerStateMachine context) {
        this.context = context;
    }

    public PlayerBaseState Idle() {
        return new PlayerIdleState(context, this);
    }
    public PlayerBaseState Walk() {
        return new PlayerWalkState(context, this);
    }
    public PlayerBaseState Jump() {
        return new PlayerJumpState(context, this);
    }
    public PlayerBaseState Grounded() {
        return new PlayerGroundedState(context, this);
    }
}
