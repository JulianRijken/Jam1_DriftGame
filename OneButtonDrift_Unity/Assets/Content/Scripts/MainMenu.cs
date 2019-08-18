using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum Cars
{
    raceCar = 0,
    iceCreamTruck = 1,
    toyota = 2,
    reliant = 3
}


public class MainMenu : MonoBehaviour
{
    private float localTimeElapsed;

    public Slider pickSlider;
    private FadeManager fadeManager;
    public Transform carPlate;

    [SerializeField]
    private int selectedCar;

    private bool loadGame = false;

    public List<GameObject> cars = new List<GameObject>();

    public TextMeshProUGUI hightscoretExt;


    void Start()
    {
        hightscoretExt.text = PlayerPrefs.GetInt("HighScore").ToString();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pickSlider.value = 0.5f;
        fadeManager = GameObject.FindGameObjectWithTag("FadeManager").GetComponent<FadeManager>();

        toAngle.eulerAngles = new Vector3(0, PlayerPrefs.GetInt("sCar") * 90, 0);
        selectedCar = PlayerPrefs.GetInt("sCar");

    }

    void Update()
    {
        localTimeElapsed += Time.deltaTime;

        PickSide();
        LoadGame();


        carPlate.rotation = Quaternion.Slerp(fromAngle, toAngle, switchTimer / 1f);
        switchTimer += Time.deltaTime;

    }


    Quaternion fromAngle;
    Quaternion toAngle;
    private float switchTimer;
    void SwitchCar()
    {
        if(switchTimer >= 1f)
        {
            // Set selected Car
            selectedCar++;
            if (selectedCar > 3)
                selectedCar = 0;

            PlayerPrefs.SetInt("sCar", selectedCar);

            // Set rotate Angle
            fromAngle = carPlate.transform.rotation;
            toAngle = Quaternion.Euler(carPlate.transform.eulerAngles + new Vector3(0, 90, 0));

            PlayerPrefs.SetFloat("toAngle", toAngle.eulerAngles.y);

            // Restet timer
            switchTimer = 0;
        }



    }



    private float loadTime;
    void LoadGame()
    {
        if (loadGame)
        {
            loadTime += Time.deltaTime;
            fadeManager.FadeIU(eFade.on, 0.5f);
            if (loadTime >= 0.6f)
                SceneManager.LoadScene(1);
        }
    }



    void PickSide()
    {

        if(loadGame == false)
        pickSlider.value += (Input.GetAxis("Mouse X") / 50) * Mathf.Clamp(localTimeElapsed / 1, 0, 1);

        if(pickSlider.value == 1)
        {
            loadGame = true;
        }
        else if (pickSlider.value == 0)
        {
            SwitchCar();
        }

    }



}
