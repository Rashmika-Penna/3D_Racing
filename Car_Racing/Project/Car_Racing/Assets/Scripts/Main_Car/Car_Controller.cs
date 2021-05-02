using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Input_Manager))] // The script is going to run only if we have this - "Require Component"
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Car_Lights_Manager))]

public class Car_Controller : MonoBehaviour
{
    // Headlights
    public Car_Lights_Manager light_manager;

    // Wheel COlliders
    public Input_Manager input;
    public List<WheelCollider> throttle_wheels;
    public List<GameObject> steering_wheels;

    // Moving the car
    public float strength_coeff = 20000f;
    public float max_turn= 20f;

    // Center of mass of the car
    public Transform center_of_mass;
    public Rigidbody rb;

    // Moving Wheels
    public List<GameObject> wheel_meshes;
    public float radius = 6;

    // Brake
    public float brake_force;
    public GameObject brake_lights;

    // Down Force
    public float down_force = 50f;

    // Engine
    public float wheels_rpm;
    public AnimationCurve engine_torque;
    public float total_power;

    private void Start()
    {
        input = GetComponent<Input_Manager>();
        rb = GetComponent<Rigidbody>();

        if(center_of_mass)
        {
            rb.centerOfMass = center_of_mass.localPosition;
        }
    }

    private void Update()
    {
        if(input.light)
        {
            light_manager.Toggle_Head_Lights();
        }
        
        brake_lights.GetComponent<Renderer>().material.SetColor("_EmissionColor", input.brake ? new Color(1.129412f, 0.1647059f, 0.1647059f) : Color.black);
    }

    private void FixedUpdate()
    {
        Down_Force();
        Spinning_Wheels();
        Steering_Wheels();
        Brake();        
    }

    private void Steering_Wheels()
    {
        //foreach (GameObject wheel in steering_wheels)
        //{
        //    wheel.GetComponent<WheelCollider>().steerAngle = max_turn * input.steer;
        //    wheel.transform.localEulerAngles = new Vector3(0f, input.steer * max_turn, 0f);
        //}

        if(input.steer > 0)
        {
            steering_wheels[0].GetComponent<WheelCollider>().steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * input.steer;
            steering_wheels[1].GetComponent<WheelCollider>().steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * input.steer;
        }        
        else if(input.steer < 0)
        {
            steering_wheels[0].GetComponent<WheelCollider>().steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * input.steer;
            steering_wheels[1].GetComponent<WheelCollider>().steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * input.steer;
        }
        else
        {
            foreach(GameObject wheel in steering_wheels)
            {
                wheel.GetComponent<WheelCollider>().steerAngle = 0;
            }
        }
    }

    private void Spinning_Wheels()
    {
        Vector3 wheel_position = Vector3.zero;
        Quaternion wheel_rotation = Quaternion.identity;

        //foreach (GameObject wheel_mesh in wheel_meshes)
        //{
        //    //wheel_mesh.transform.Rotate(rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z) >= 0 ? 1 : -1 / (2 * Mathf.PI * 0.34f), 0f, 0f);
        //    //wheel_mesh.GetWorldPose(out wheel_position, out wheel_rotation);
        //}

        for(int i = 0; i < 4; i++)
        {
            throttle_wheels[i].GetWorldPose(out wheel_position, out wheel_rotation);
            wheel_meshes[i].transform.position = wheel_position;
            wheel_meshes[i].transform.rotation = wheel_rotation;
        }


    }

    private void Brake()
    {
        foreach (WheelCollider wheel in throttle_wheels)
        {
            if (input.brake)
            {
                wheel.motorTorque = 0f;
                wheel.brakeTorque = brake_force * Time.deltaTime;
            }
            else
            {
                wheel.motorTorque = strength_coeff * Time.deltaTime * input.throttle;
                wheel.brakeTorque = 0f;
            }
        }
    }

    private void Down_Force()
    {
        rb.AddForce(-transform.up * down_force * rb.velocity.magnitude);
    }

    private void Wheel_RPM()
    {
        float sum = 0;
        int r = 0;
        
        for(int i=0; i<4; i++)
        {
            sum += throttle_wheels[i].rpm;
            r++;
        }
        wheels_rpm = (r != 0) ? sum / r : 0;
    }

    
}
