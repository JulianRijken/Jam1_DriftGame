using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameManager gm;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(gm.player.tag))
        {
            gm.playerComponent.died = true;
        }
    }
}
