using UnityEngine;

public class CursorEnabler : MonoBehaviour
{
   
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

  
}
