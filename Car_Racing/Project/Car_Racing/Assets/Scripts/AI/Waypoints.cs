using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();
    public Color gizmos_color;
    [Range(0, 1)] public float sphere_radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmos_color;

        Transform[] path = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 1; i < path.Length; i++)
        {
            nodes.Add(path[i]);
        }
        
        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 current_waypoint = nodes[i].position;
            Vector3 previous_waypoint = Vector3.zero;

            if(i != 0)
            {
                previous_waypoint = nodes[i - 1].position;
            }
            else if(i==0)
            {
                previous_waypoint = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previous_waypoint, current_waypoint);
            Gizmos.DrawSphere(current_waypoint, sphere_radius);
        }
    }
}
