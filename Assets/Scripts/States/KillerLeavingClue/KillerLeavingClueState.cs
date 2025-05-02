using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerLeavingClueState : StateBase<KillerBase>
{


    public KillerLeavingClueState(KillerBase enemy, StateMachine<KillerBase> enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        entity.KillerLeavingClueInstance.DoAnimationTriggerEventogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        entity.KillerLeavingClueInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        entity.KillerLeavingClueInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        entity.KillerLeavingClueInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.KillerLeavingClueInstance.DoPhysicsLogic();
    }
}