using UnityEngine;
using UnityEngine.Events;

public class TriggerEventModuleInvoker : MonoBehaviour
{
    public UnityEvent OnTriggerEvent;
    [SerializeField] private string tag;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(tag))
            OnTriggerEvent?.Invoke();
    }
}
