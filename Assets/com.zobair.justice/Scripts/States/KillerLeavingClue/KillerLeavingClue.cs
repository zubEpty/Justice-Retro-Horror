using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "Killer Leaving Clue", menuName = "KillerLogic/Leaving Clue Logic/Leaving Clue")]
public class KillerLeavingClue : KillerLeavingClueSOBase
{
    public List<GameObject> killerClueInvoker;

    public override void DoAnimationTriggerEventogic(AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        killerClueInvoker[0].GetComponent<ClueBase>()?.LeaveClue();
        Debug.LogError("Leaving Clue State Initialized");
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
