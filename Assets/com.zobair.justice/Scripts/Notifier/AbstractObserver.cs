
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractObserver<T> : MonoBehaviour, IObserver<T>
{
    protected Dictionary<T, System.Action> NotifierContainer;

    public virtual void OnNotify(T state)
    {
        if (NotifierContainer.ContainsKey(state))
            NotifierContainer[state]();

        GenericCall();
    }

    public abstract void PopulateNotifier();

    public virtual void GenericCall()
    {
        Debug.LogError("Calling the Notifier");
    }
}
