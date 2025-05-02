
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "Killer Marcos Caught Player Generic", menuName = "KillerLogic/Caught Player Logic/Marcos Caught Player")]
public class KillerMarcosCaughtPlayer : KillerCaughtPlayerSOBase
{
    public float randomSpeed = 10f;
    public float randomMovementRange = 10f;

    GameObject moveTo;

    private int currentMovementIndex;

    public override void DoAnimationTriggerEventogic(AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        Debug.LogError("Marcos Killer Caught Player State Initialized");
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
