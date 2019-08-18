using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public Transform player;
    public float smoothTime;
    public bool smooth;
    public Vector3 offset;

    public bool rotate;

    private Vector3 target;

    public float distance = 0;

    void Update ()
    {


         Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45 + player.GetComponent<Player>().speed * 2, Time.deltaTime);
       

        if (player != null)
        {
            target = player.position + offset + (player.GetComponent<Player>().rig.velocity);


            if (smooth == true)
            {
                transform.position = Vector3.Lerp(transform.position, target, Time.unscaledDeltaTime / smoothTime);
            }
            else
            {
                transform.position = target;
            }


            if (rotate)
            {
                transform.localEulerAngles = new Vector3(90, player.localEulerAngles.y, 0.0f);
            }
        }

    }
}
