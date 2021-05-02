using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public Rigidbody car;
    public float max_speed = 0f;
    public float min_needle_angle = 0f;
    public float max_needle_angle = 0f;
    public RectTransform needle;
    private float speed = 0f;

    private void Update()
    {
        speed = car.velocity.magnitude * 3.6f; // 3.6 to convert to kms

        if(needle != null)
        {
            needle.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(min_needle_angle, max_needle_angle, speed / max_speed)); 
        }
    }
}
