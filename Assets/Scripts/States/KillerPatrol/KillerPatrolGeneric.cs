using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "Patrol Idle Generic", menuName = "KillerLogic/Patrol Logic/Generic Patrol")]
public class KillerPatrolGeneric : KillerPatrolSOBase
{     

    public override void DoAnimationTriggerEventogic(AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        Debug.LogError("Patrol State Initialized");
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}