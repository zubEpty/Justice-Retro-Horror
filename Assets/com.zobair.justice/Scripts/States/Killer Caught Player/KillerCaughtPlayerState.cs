using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCaughtPlayerState : StateBase<KillerBase>
{


    public KillerCaughtPlayerState(KillerBase enemy, StateMachine<KillerBase> enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        entity.KillerCaughtPlayerInstance.DoAnimationTriggerEventogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        entity.KillerCaughtPlayerInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        entity.KillerCaughtPlayerInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        entity.KillerCaughtPlayerInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.KillerCaughtPlayerInstance.DoPhysicsLogic();
    }
}