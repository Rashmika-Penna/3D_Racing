using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anti_Roll : MonoBehaviour
{
    public WheelCollider wheel_L;
    public WheelCollider wheel_R;
    private Rigidbody car;
    public float anti_roll = 5000.0f;

    private void Start()
    {
        car = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        WheelHit hit = new WheelHit();
        float travel_L = 1.0f;
        float travel_R = 1.0f;

        bool grounded_L = wheel_L.GetGroundHit(out hit);

        if(grounded_L)
        {
            travel_L = (-wheel_L.transform.InverseTransformPoint(hit.point).y - wheel_L.radius) / wheel_L.suspensionDistance;
        }

        bool grounded_R = wheel_R.GetGroundHit(out hit);

        if(grounded_R)
        {
            travel_R = (-wheel_R.transform.InverseTransformPoint(hit.point).y - wheel_R.radius) / wheel_R.suspensionDistance;
        }

        var anti_roll_force = (travel_L - travel_R) * anti_roll;

        if(grounded_L)
        {
            car.AddForceAtPosition(wheel_L.transform.up * -anti_roll_force, wheel_L.transform.position);
        }

        if(grounded_R)
        {
            car.AddForceAtPosition(wheel_R.transform.up * anti_roll_force, wheel_R.transform.position);
        }
    }
}
