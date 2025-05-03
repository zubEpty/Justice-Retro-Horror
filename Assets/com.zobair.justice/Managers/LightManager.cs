using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private List<Light> houseLights;
    public List<Light> GetLights => houseLights;

    private bool isRoomPowered = true;

    [ContextMenu("Toggle Poiwer to All Room Point Lights")]
    public void ToggleRoomPower()
    {
        isRoomPowered = !isRoomPowered;

        foreach (Light light in houseLights)
        {
            light.enabled = isRoomPowered;
        }        
    }
}
