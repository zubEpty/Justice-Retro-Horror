using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCaughtPlayerSOBase : ScriptableObject
{
    protected KillerBase killer;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    public virtual void Initialize(GameObject gameObject, KillerBase killer)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.killer = killer;
    }

    public virtual void DoEnterLogic() { }
    public virtual void DoExitLogic() { ResetValues(); }
    public virtual void DoFrameUpdateLogic() { }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventogic(AnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }
}
