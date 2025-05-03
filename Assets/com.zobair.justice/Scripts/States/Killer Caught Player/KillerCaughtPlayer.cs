
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "Killer Caught Player Generic", menuName = "KillerLogic/Caught Player Logic/Caught Player")]
public class KillerCaughtPlayer : KillerCaughtPlayerSOBase
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
        killer.GetNavAgent.speed = 4f;
        Debug.LogError("Killer Caught Player State Initialized"); 
        
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        killer.GetNavAgent.destination = PlayerManager.Instance.player.transform.position;
        base.DoFrameUpdateLogic();

        if (Vector3.Distance(killer.transform.position, PlayerManager.Instance.player.transform.position) < 2f)
        {
            killer.GetNavAgent.speed = 0f;

            GameManager.Instance.LoadGameOverUi();
            //put Game over logic here
        }
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
