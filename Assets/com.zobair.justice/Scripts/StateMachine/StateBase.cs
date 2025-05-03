using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 


public class StateBase<T>
{
    protected T entity;
    protected StateMachine<T> StateMachine;
    public bool isRoot = true;
    public StateBase(T entity, StateMachine<T> StateMachine)
    {
        this.entity = entity;
        this.StateMachine = StateMachine;
    }


    public virtual void EnterState() { }
    public virtual void ExitState()
    {
        Reset();
        //PlayerManager.Instance.ForceChangeToState = PlayerStateList.None;
    }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(AnimationTriggerType triggerType) { }
    public virtual void Reset() { }
}
