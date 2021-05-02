using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Lights_Manager : MonoBehaviour
{
    public List<Light> headlights;

    public virtual void Toggle_Head_Lights()
    {
        foreach(Light head_light in headlights)
        {
            head_light.intensity = head_light.intensity == 0 ? 3 : 0;
        }
    }
}
