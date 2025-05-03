using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerIdleState : StateBase<KillerBase>
{


    public KillerIdleState(KillerBase enemy, StateMachine<KillerBase> enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        entity.KillerIdleBaseInstance.DoAnimationTriggerEventogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        entity.KillerIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        entity.KillerIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        entity.KillerIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.KillerIdleBaseInstance.DoPhysicsLogic();
    }
}