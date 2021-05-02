using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    // Camera Follow
    public GameObject black_car;
    //public GameObject constraint;
    //public float speed;
    public float distance = 2f;
    public float height = 2f;
    public float dampening = 1f;

    // Camera Views
    private int cam_mode = 0;
    public float height_2 = 0f;
    public float distance_2 = 0f;
    public float horizontal_distance = 0f;

    //private void Awake()
    //{
    //    black_car = GameObject.FindGameObjectWithTag("Player");
    //    constraint = black_car.transform.Find("Camera_Constraint").gameObject;
    //}

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            cam_mode = (cam_mode + 1) % 2;
        }

        switch(cam_mode)
        {
            case 1:
                transform.position = black_car.transform.position + black_car.transform.TransformDirection(new Vector3(horizontal_distance, height_2, distance_2));
                transform.rotation = black_car.transform.rotation;
                break;

            default:
                //transform.position = Vector3.Lerp(transform.position, black_car.transform.position + black_car.transform.TransformDirection(new Vector3(0f, height, distance)), dampening * Time.deltaTime);
                ////transform.rotation = Quaternion.Lerp(transform.rotation, black_car.transform.rotation, Time.deltaTime);
                //transform.LookAt(black_car.transform);

                StartCoroutine(Follow());

                break;
        } 
    }   
    
    IEnumerator Follow()
    {
        transform.position = Vector3.Lerp(transform.position, black_car.transform.position + black_car.transform.TransformDirection(new Vector3(0f, height, distance)), dampening * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, black_car.transform.rotation, Time.deltaTime);
        transform.LookAt(black_car.transform);
        yield return new WaitForSeconds(7f);
        dampening = 15;
    }
}
