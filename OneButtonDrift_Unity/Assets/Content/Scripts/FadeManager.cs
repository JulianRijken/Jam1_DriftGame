using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eFade
{
    off = 0,
    on = 1
}

public class FadeManager : MonoBehaviour
{
    [Header("THIS WILL ONLY EFFECT THE START")]
    [Header("")]
    [SerializeField]
    private eFade startFromState = eFade.on;
    [SerializeField]
    private eFade startToState = eFade.off;
    [SerializeField]
    private Color startColor = Color.white;
    [SerializeField]
    private float startTime = 1;
    [SerializeField]
    private float startTimeBeforeFade = 0;
    [SerializeField]
    private bool unscaled = true;

    private Image img;


    void Start()
    {
        img = GetComponent<Image>();

        SetFadeColor(startColor);
        img.color = new Color(col.r, col.g, col.b, (float)startFromState);

        StartCoroutine(FadeDelay());
    }

    private IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(startTimeBeforeFade);
        procent = 0;
        lastAlpha = (int)startFromState;
        currentState = (int)startToState;
        fadeTime = startTime;
        inFade = true;
    }

    [HideInInspector] public float procent = 0;
    private float lastAlpha = 0;
    private float fadeTime = 0;
    private int currentState;
    private Color col;

    [HideInInspector]
    public bool inFade = false;

    public void SetFadeColor(Color _col)
    {
        col = _col;
    }

    public void Fade(int state, float _fadeTime = 1)
    {
        DoFade(state , false, _fadeTime);
    }
    public void Fade(eFade state, float _fadeTime = 1)
    {
        DoFade((int)state, false, _fadeTime);
    }
    public void FadeIU(int state, float _fadeTime = 1)
    {
        DoFade(state, true, _fadeTime);
    }
    public void FadeIU(eFade state, float _fadeTime = 1)
    {
        DoFade((int)state, true, _fadeTime);
    }


    private void DoFade(int state, bool inUpdate = false, float _fadeTime = 1)
    {

        if (currentState != state)
        {
            if (inUpdate)
            {
                if (inFade == false)
                {
                    procent = 0;
                    lastAlpha = img.color.a;
                    currentState = state;
                    fadeTime = _fadeTime;

                    inFade = true;
                }
            }
            else
            {
                procent = 0;
                lastAlpha = img.color.a;
                currentState = state;
                fadeTime = _fadeTime;

                inFade = true;
            }
        }

    }

    void Update()
    {
        if (inFade)
        {
            procent += (unscaled == false ?  Time.deltaTime : Time.unscaledDeltaTime);

            float alpha = Mathf.Lerp(lastAlpha, currentState, procent / fadeTime);

            img.color = new Color(col.r, col.g, col.b, alpha);

            if (procent / fadeTime >= 1)
            {
                inFade = false;
            }
        }
    }
}
