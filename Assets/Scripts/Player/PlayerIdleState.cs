using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() && xInput * player.facingDir > 0)
            return;
        if(xInput !=0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
