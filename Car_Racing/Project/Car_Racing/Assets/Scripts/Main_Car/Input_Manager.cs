using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    // AI System
    [Header("AI System")]
    [SerializeField] driver driver_controller;
    public Waypoints waypoints;
    public List<Transform> nodes = new List<Transform>();
    public Transform current_waypoint;
    [Range(0, 10)] public int distance_offset;
    [Range(0, 5)] public float steer_force;

    internal enum driver
    {
        AI,
        Keyboard
    }

    // AI Sensors
    [Header ("Sensors")]
    public float sensor_length = 2f;
    public float sensor_height = 0.2f;
    public float front_middle_sensor_position = 2.11f;    
    public float front_side_sensor_position = 0.9f;
    //public float front_side_sensor_angle = 30;
   
    // Movement Input
    [Header("Movement Input")]
    public float throttle;
    public float steer;

    // Toggle Headlights
    [Header("Headlights")]
    public bool light;

    // Brake
    [Header("Brake")]
    public bool brake;

    private void Awake()
    {
        waypoints = GameObject.FindGameObjectWithTag("Path").GetComponent<Waypoints>();
        nodes = waypoints.nodes;
    }

    private void FixedUpdate()
    {
        switch (driver_controller)
        {
            case driver.AI:
                AI_Drive();
                break;

            case driver.Keyboard:
                Keyboard_Drive();
                break;
        }

        Distance_Between_Waypoints();
    }

    private void AI_Drive()
    {
        throttle = 0.3f;
        AI_Steer();
        Sensors();
    }

    private void Distance_Between_Waypoints()
    {
        Vector3 current_position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 difference = nodes[i].transform.position - current_position;
            float current_distance = difference.magnitude;

            if (current_distance < distance)
            {
                current_waypoint = nodes[i + distance_offset];
                distance = current_distance;
            }
        }
    }

    private void AI_Steer()
    {
        Vector3 relative = transform.InverseTransformPoint(current_waypoint.transform.position);
        relative /= relative.magnitude;

        steer = (relative.x / relative.magnitude) * steer_force;
    }

    private void Keyboard_Drive()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        light = Input.GetKeyDown(KeyCode.L);

        brake = Input.GetKey(KeyCode.Space);
    }    

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensor_starting_position = transform.position;
        sensor_starting_position.x -= front_middle_sensor_position;
        sensor_starting_position.y += sensor_height;

        // Front Middle Sensor
        Debug.DrawLine(sensor_starting_position, sensor_starting_position + transform.forward * sensor_length, Color.blue);
        if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        {
            Debug.Log("Middle Sensor");
        }

        // Front Right Sensor
        sensor_starting_position.z += front_side_sensor_position;
        Debug.DrawLine(sensor_starting_position, sensor_starting_position + transform.forward * sensor_length, Color.red);
        if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        {
            Debug.Log("Right Sensor");
        }

        // Front RIght Right Sensor
        sensor_starting_position.z += front_middle_sensor_position * 0.5f;
        Debug.DrawLine(sensor_starting_position, sensor_starting_position + transform.forward * sensor_length, Color.yellow);
        if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        {
            Debug.Log("Right Sensor");
        }

        //// Front Right Angled Sensor
        //Debug.DrawLine(sensor_starting_position, Quaternion.AngleAxis(front_side_sensor_angle, transform.up) * (sensor_starting_position + transform.forward * sensor_length), Color.yellow);
        //if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        //{
        //    Debug.Log("Right Angled sensor");
        //}

        // Front Left Sensor
        sensor_starting_position.z -= 3 * front_side_sensor_position;
        Debug.DrawLine(sensor_starting_position, sensor_starting_position + transform.forward * sensor_length, Color.red);
        if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        {
            Debug.Log("Left Sensor");
        }

        // Front left left sensor
        sensor_starting_position.z -= front_middle_sensor_position * 0.5f;
        Debug.DrawLine(sensor_starting_position, sensor_starting_position + transform.forward * sensor_length, Color.yellow);
        if (Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        {
            Debug.Log("Left Sensor");
        }

        //// Front Left Angled Sensor
        //Debug.DrawLine(sensor_starting_position, Quaternion.AngleAxis(-0.5f, transform.up) * (sensor_starting_position + transform.forward * sensor_length), Color.yellow);
        //if(Physics.Raycast(sensor_starting_position, transform.forward, out hit, sensor_length))
        //{
        //    Debug.Log("Left Angled Sensor");
        //}
    }
}
