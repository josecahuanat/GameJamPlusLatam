using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public RectTransform canvas;
    public Text hourText, minuteText, dotsText;
    public float textGameplayOffset;
    public float animationDuration;
    public Ease animationEase;


    int hour, minute;
    RectTransform hourRT, minuteRT;
    Vector2 hourMenuPos, minuteMenuPos;
    Vector2 hourMenuSize, minuteMenuSize;


    bool _playing;
    bool playing
    {
        get{ return _playing; }
        set
        {
            if (playing == value)
                return;

            _playing = value;

            dotsText.DOKill();
            dotsText.text = string.Empty;

            if (playing)
            {
                var hourGameplayPos = new Vector2((canvas.sizeDelta.x / -2f) + textGameplayOffset, (canvas.sizeDelta.y / 2f));
                hourRT.DOAnchorPos(hourGameplayPos, animationDuration).SetEase(animationEase);
                hourRT.DOSizeDelta(new Vector2(hourMenuSize.x / 2f, hourMenuSize.y / 2f), animationDuration).SetEase(animationEase);

                var minuteGameplayPos = new Vector2((canvas.sizeDelta.x / 2f) - textGameplayOffset, (canvas.sizeDelta.y / 2f));
                minuteRT.DOAnchorPos(minuteGameplayPos, animationDuration).SetEase(animationEase);
                minuteRT.DOSizeDelta(new Vector2(minuteMenuSize.x / 2f, minuteMenuSize.y / 2f), animationDuration).SetEase(animationEase);
            }
            else
            {
                hourRT.DOAnchorPos(hourMenuPos, animationDuration).SetEase(animationEase);

                minuteRT.DOAnchorPos(minuteMenuPos, animationDuration).SetEase(animationEase);

                dotsText.DOText(":", animationDuration, false, ScrambleMode.None).SetEase(animationEase).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }


    void Start()
    {
        hourRT = hourText.GetComponent<RectTransform>();
        hourMenuPos = hourRT.anchoredPosition;
        hourMenuSize = hourRT.sizeDelta;

        minuteRT = minuteText.GetComponent<RectTransform>();
        minuteMenuPos = minuteRT.anchoredPosition;
        minuteMenuSize = minuteRT.sizeDelta;

        dotsText.text = string.Empty;
        dotsText.DOText(":", animationDuration, false, ScrambleMode.None).SetEase(animationEase).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        if (playing)
        {

        }
        else
        {
            SetTime();
        }
    }

    public void Play()
    {
        playing = true;
    }

    public void GoToMainMenu()
    {
        playing = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void SetTime()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;

        // Debug.Log($"{hour}, {minute}");

        hourText.text = hour.ToString("00");
        minuteText.text = minute.ToString("00");
    }


}
