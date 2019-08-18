using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerComponent;
    private FadeManager fadeManager;
    private float fadeTime;
    public List<GameObject> cars = new List<GameObject>();
    public Transform speedometer;
    private float diedTimer;

    private float timeElapsed = 0;



    void Start()
    {
        

        // Spawn Player
        int randChild = Random.Range(0, transform.childCount);
        player = Instantiate(cars[PlayerPrefs.GetInt("sCar")], transform.GetChild(randChild).position, transform.GetChild(randChild).transform.rotation);
        Camera.main.GetComponent<FollowObject>().player = player.transform;
        player.GetComponent<Rigidbody>().AddForce(transform.GetChild(randChild).transform.forward * 15, ForceMode.Impulse);
        playerComponent = player.GetComponent<Player>();

        Camera.main.transform.position = transform.GetChild(randChild).position;

        lastPlayerSpeed = playerComponent.speed;

        // Lock Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Get Components
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager").GetComponent<FadeManager>();

        speedometer.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, -270, playerComponent.speed / playerComponent.maxVelocity)));

        GetComponent<Points>().player = playerComponent;
    }

    private float lastPlayerSpeed;
    private float velocityChange;

    void Update()
    {

        KillPLayer();
        LoadMenu();

        speedometer.rotation = Quaternion.Slerp(speedometer.rotation, Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, -270, playerComponent.speed / playerComponent.maxVelocity))), Time.deltaTime / 0.01f);

    }

    void LoadMenu()
    {

        if (playerComponent.died)
        {
            fadeTime += Time.deltaTime;

            fadeManager.FadeIU(eFade.on, 1);
            if (fadeTime >= 1)
            {
                SceneManager.LoadScene(0);
            }
        }

    }


    void KillPLayer()
    {


        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 0.5f)
        {
            velocityChange = transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).z - lastPlayerSpeed;
            lastPlayerSpeed = transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).z;

        }

        if (playerComponent.speed < 5)
        {
            diedTimer += Time.deltaTime;
            if (diedTimer >= 1.5f)
            {
                playerComponent.died = true;
            }
        }
        else
        {
            diedTimer = 0;
        }


        if (velocityChange < -5)
        {
            playerComponent.died = true;
        }

        if (playerComponent.OnRoof())
        {
            playerComponent.died = true;
        }

    }
}
