using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed;

    public bool random;
    public Vector2 min_max;
    private float randNum;

    public bool onlyY;

    private void Start()
    {
        transform.Rotate(new Vector3(0, Random.Range(0,360), 0));

        if (random)
        {
            randNum = Random.Range(min_max.x, min_max.y);
        }
    }

    void Update ()
    {

        if (random)
        {
            transform.Rotate(new Vector3(0, randNum, 0));
            
        }
        else
        {
            if (onlyY)
            {
                transform.eulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.Rotate(new Vector3(0, speed, 0));
            }
        }
    }
}
