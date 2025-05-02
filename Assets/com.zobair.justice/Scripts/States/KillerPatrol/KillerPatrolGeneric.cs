using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "Patrol Idle Generic", menuName = "KillerLogic/Patrol Logic/Generic Patrol")]
public class KillerPatrolGeneric : KillerPatrolSOBase
{              
    public override void DoAnimationTriggerEventogic(AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventogic(triggerType);

        
    }

    public override void DoEnterLogic()
    {
        killer.GetNavAgent.speed = 1.25f;
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
        killer.ExecuteNavMeshAction();
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