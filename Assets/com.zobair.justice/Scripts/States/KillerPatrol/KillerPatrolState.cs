using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerPatrolState : StateBase<KillerBase>
{


    public KillerPatrolState(KillerBase enemy, StateMachine<KillerBase> enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        entity.KillerPatrolBaseInstance.DoAnimationTriggerEventogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        entity.KillerPatrolBaseInstance.DoEnterLogic();

    }

    public override void ExitState()
    {
        base.ExitState();
        entity.KillerPatrolBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        entity.KillerPatrolBaseInstance.DoFrameUpdateLogic();

             
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.KillerPatrolBaseInstance.DoPhysicsLogic();
    }
}