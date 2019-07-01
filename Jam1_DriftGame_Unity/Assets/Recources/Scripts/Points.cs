using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Points : MonoBehaviour
{

    [HideInInspector] public Player player;

    private float totalPoints = 0;
    public TextMeshProUGUI totalPointsText;
    private float streakPoints = 0;
    private TextMeshProUGUI streakPointText;
    private float streakTimer;

    private float airTimer;

    private float driftTimer;
    private float angle;



    void Update()
    {
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

    }

    void AddStreakPoints(float _point)
    {
        if (player.died == false)
        {
            streakPointText.GetComponent<Animator>().SetTrigger("Add");
            streakPoints += _point;
            streakTimer = 0;
        }
    }

    void AddTotalPoints(float _point)
    {
        if (player.died == false)
        {
            totalPointsText.GetComponent<Animator>().SetTrigger("AddTotalPoints");

            totalPoints += _point;

        }
    }


    void JumpPoints()
    {
        if (player.grounded == false && player.speed > 1)
        {
            airTimer += Time.deltaTime;

            if (airTimer >= 0.1f)
            {
                AddStreakPoints(1 * Mathf.Clamp(Mathf.Abs(player.yRot), 1, 10));

                airTimer = 0;
            }
        }
        else
        {
            airTimer = 0;
        }
    }


    void DriftPoints()
    {

        


        angle = Vector3.Angle(player.transform.position, player.transform.TransformDirection(new Vector3(0, 0, 10))) - Vector3.Angle(player.transform.position, player.rig.velocity);
        angle = Mathf.Abs(angle);

        float addPoints = (angle * (player.speed / 15)) / 50;

        if (player.grounded && player.speed > 0 && addPoints > 0.5f)
        {
            driftTimer += Time.deltaTime;

            if (driftTimer >= 0.1f)
            {
                AddStreakPoints(addPoints);

                driftTimer = 0;
            }
        }
        else
        {
            driftTimer = 0;
        }
    }
}
