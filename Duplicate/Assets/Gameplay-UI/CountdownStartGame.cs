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

    private PlayerRespawn _playerRespawn;
    private int currentCount;

    private void Awake()
    {
        _playerRespawn = FindObjectOfType<PlayerRespawn>();
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
        _playerRespawn.StartGame();
        CountdownImage.SetActive(false);
    }
}
