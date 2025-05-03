using UnityEngine;
using UnityEngine.Events;

public class CustomTriggerEventHandler : MonoBehaviour
{
    public UnityEvent OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            OnTriggered.Invoke();
        }
    }



}
