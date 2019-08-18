using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private TrailRenderer lr;

    public bool frontWheel;
    private float frontWheelRotation;

    public bool grounded;

    private void Start()
    {
        lr = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        RotateFrontWheel();
        CheckGround();

        if (grounded)
            lr.emitting = true;
        else
            lr.emitting = false;        
    }

    void RotateFrontWheel()
    {
        if (frontWheel)
        {
            frontWheelRotation = Mathf.Lerp(frontWheelRotation, -Input.GetAxis("Mouse X") * 25, Time.deltaTime) * 1;
            frontWheelRotation = Mathf.Clamp(frontWheelRotation, -40, 40);
            transform.localRotation = Quaternion.Euler(0, frontWheelRotation, 0);
        }
    }

    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))       
            if (hit.distance <= 0.5f)
            {
                grounded = true;
                return;
            }
        

        grounded = false;
    }
}









//if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.5f)           