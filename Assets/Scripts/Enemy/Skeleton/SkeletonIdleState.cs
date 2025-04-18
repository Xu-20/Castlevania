using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CloseCountAttackWindow(); // 关闭计数攻击窗口
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(24, enemy.transform); // 播放音效
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);

    }
}
