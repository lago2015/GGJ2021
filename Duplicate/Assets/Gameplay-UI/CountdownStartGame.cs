﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountdownStartGame : MonoBehaviour
{
    public GameObject CountdownImage;
    public TMPro.TextMeshProUGUI CountdownText;
    public Animator countdownAnim;

    private int currentCount;

    private void Awake()
    {
        DataManager.ToTheCloud(DataKeys.COUNTDOWN_STARTGAME,this);
    }

    private void OnEnable()
    {
        SetUpCountdown();
        if(countdownAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "GO") countdownAnim.SetTrigger("Flip");
    }

    public void SetUpCountdown()
    {
        CountdownImage.SetActive(true);
        currentCount = 3;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        CountdownText.text = $"{currentCount}";
        currentCount--;
        yield return new WaitForSeconds(1);
        CountdownText.text = $"{currentCount}";
        currentCount--;
        yield return new WaitForSeconds(1);
        CountdownText.text = $"{currentCount}";
        yield return new WaitForSeconds(1);
        CountdownText.text = $"GO!";
        countdownAnim.SetTrigger("Flip");
        yield return new WaitForSeconds(0.25f);
        DataManager.MakeItRain<PlayerRespawn>(DataKeys.PLAYER_RESPAWN).StartGame();
        CountdownImage.SetActive(false);
    }
}
