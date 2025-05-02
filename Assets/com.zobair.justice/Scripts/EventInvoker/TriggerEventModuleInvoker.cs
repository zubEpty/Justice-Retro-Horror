using UnityEngine;
using UnityEngine.Events;

public class TriggerEventModuleInvoker : MonoBehaviour
{
    public UnityEvent OnTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Killer"))
            OnTriggerEvent?.Invoke();
    }
}
