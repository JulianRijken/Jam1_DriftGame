using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Points : MonoBehaviour
{

    [HideInInspector] public Player player;

    public float multyplyBarValue;
    public float timesPoints;
    public Slider multiplyBar;
    public TextMeshProUGUI totalPointsText;
    public TextMeshProUGUI multyplyText;

    private float totalPoints = 0;
    private float streakPoints = 0;
    private float streakTimer;
    private float airTimer;
    private float driftTimer;
    private float angle;
    private TextMeshProUGUI streakPointText;


    private void Update()
    {

        if (totalPoints >= PlayerPrefs.GetInt("HighScore"))    
            PlayerPrefs.SetInt("HighScore", (int)totalPoints);
        

        multyplyBarValue = Mathf.Clamp(multyplyBarValue, 0, 100);
        multiplyBar.value = Mathf.Lerp(multiplyBar.value, multyplyBarValue, Time.deltaTime);

        multyplyText.text = "x" + timesPoints;

        if ((int)multiplyBar.value < 25)
        {
            timesPoints = 1;
        }
        else if ((int)multiplyBar.value < 50)
        {
            timesPoints = 2;
        }
        else if ((int)multiplyBar.value < 75)
        {
            timesPoints = 4;
        }
        else if ((int)multiplyBar.value < 100)
        {
            timesPoints = 8;
        }

        if (streakPointText != null)
        {
            totalPointsText.text = Mathf.Round(totalPoints).ToString();
            streakPointText.text = Mathf.Round(streakPoints).ToString();


            JumpPoints();
            DriftPoints();

            streakTimer += Time.deltaTime;
            if (streakTimer > 1.5f && streakPoints > 0)
            {
                AddTotalPoints(streakPoints);
                streakPoints = 0;
                streakTimer = 0;
            }
        }
        else
        {
            streakPointText = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TextMeshProUGUI>();
        }

        multyplyBarValue -= Time.deltaTime / 2;
    }

    private void AddMultyplyBar(float ammount)
    {
        multyplyBarValue += ammount / 10;
    }

    private void AddStreakPoints(float _point)
    {
        if (player.died == false)
        {
            streakPointText.GetComponent<Animator>().SetTrigger("Add");
            streakPoints += _point;
            streakTimer = 0;
        }
    }

    private void AddTotalPoints(float _point)
    {
        if (player.died == false)
        {
            totalPointsText.GetComponent<Animator>().SetTrigger("AddTotalPoints");
            totalPoints += _point;
        }
    }


    private void JumpPoints()
    {
        if (player.grounded == false && player.speed > 1)
        {
            airTimer += Time.deltaTime;

            if (airTimer >= 0.1f)
            {
                AddStreakPoints((1 * Mathf.Clamp(Mathf.Abs(player.yRot) + Mathf.Abs(player.xRot) * 5, 1, 10)) * timesPoints);
                AddMultyplyBar(1 * Mathf.Clamp(Mathf.Abs(player.yRot) + Mathf.Abs(player.xRot) * 5, 1, 10));

                airTimer = 0;
            }
        }
        else
        {
            airTimer = 0;
        }
    }

    private void DriftPoints()
    {
        angle = Vector3.Angle(player.transform.position, player.transform.TransformDirection(new Vector3(0, 0, 10))) - Vector3.Angle(player.transform.position, player.rig.velocity);
        angle = Mathf.Abs(angle);

        float addPoints = (angle * (player.speed / 15)) / 50;

        if (player.grounded && player.speed > 0 && addPoints > 0.5f)
        {
            driftTimer += Time.deltaTime;

            if (driftTimer >= 0.1f)
            {
                AddStreakPoints(addPoints * timesPoints);
                AddMultyplyBar(addPoints * 5);

                driftTimer = 0;
            }
        }
        else
        {
            driftTimer = 0;
        }
    }
}
