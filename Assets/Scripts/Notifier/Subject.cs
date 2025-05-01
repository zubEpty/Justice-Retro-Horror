using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject<T>
{
    private List<AbstractObserver<T>> _observers = new();

    public void AddObserver(AbstractObserver<T> observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(AbstractObserver<T> observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(T state)
    {
        _observers.ForEach((_observer) =>
        {
            _observer.OnNotify(state);
        });
    }
}
