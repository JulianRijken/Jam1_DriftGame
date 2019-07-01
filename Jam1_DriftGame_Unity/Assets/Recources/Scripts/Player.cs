using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float force = 15;
    public float maxVelocity = 15;
    [HideInInspector] public float speed;
    [HideInInspector] public Rigidbody rig;
    [HideInInspector] public float yRot;
    [HideInInspector] public bool grounded;

    [HideInInspector] public bool died;

    private float xRot;

    void Start()
    {

        rig = GetComponent<Rigidbody>();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
            transform.position = hit.point + new Vector3(0,1,0);
    }

    void Update()
    {

        RotatePlayer();
        speed = rig.velocity.magnitude;


        grounded = (Grounded() > 0 ? true : false);

    }

    public bool OnRoof()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity))
            if (hit.distance > 0.8f && hit.distance < 1.5f)
                return true;

        return false;

    }

    // Rotate
    void RotatePlayer()
    {
        if (died == false)
        {
            if (Grounded() > 0)
            {
                yRot = Mathf.Lerp(yRot, Mathf.Clamp(Input.GetAxis("Mouse X"), -10f, 10f), Time.deltaTime / 0.1f);
                transform.Rotate(0, yRot, 0);
                xRot = 0;
            }
            else
            {
                yRot = Mathf.Lerp(yRot, Mathf.Clamp(Input.GetAxis("Mouse X"), -10f, 10f), Time.deltaTime);
                xRot = Mathf.Lerp(xRot, Mathf.Clamp(Input.GetAxis("Mouse Y") * 2,-10f,10f), Time.deltaTime);
                transform.Rotate(xRot, yRot, 0);
            
            }
        }
            if (Grounded() > 0)
                rig.angularDrag = 1;
            else
                rig.angularDrag = 6;

    }

    // Add Force
    void FixedUpdate()
    {

        if (died == false)
        {
            if (rig.velocity.magnitude < maxVelocity)
            {
                rig.AddForce(transform.forward * force * Grounded() / WheelCount(), ForceMode.Force);
            }
        }

    }


    // Use Functions \\ 
    public int Grounded()
    {
        int times = 0;
        for (int i = 0; i < GetComponentsInChildren<Wheel>().Length; i++)
        {
            if (GetComponentsInChildren<Wheel>()[i].grounded)
            {
                times++;
            }
        }

        return times;
    }

    public int WheelCount()
    {
        return GetComponentsInChildren<Wheel>().Length;
    }
}
