using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneAssigner : MonoBehaviour
{
    public GameObject EndGameCanvas;
    public GameObject NextLevel;
    public GameObject RespawnButton;
    public GameObject RestartButton;
    public GameObject MiddleImage;
    public TMPro.TextMeshProUGUI EndGameMessage;
    public Animator Animator;
    public FusionSequence FusionSequence;
    private PlayerRespawn _playerRespawn;

    private void Awake()
    {
        _playerRespawn = FindObjectOfType<PlayerRespawn>();
        if (!_playerRespawn) return;

        _playerRespawn.NextLevel = NextLevel;
        _playerRespawn.EndGameCanvas = EndGameCanvas;
        _playerRespawn.middle = MiddleImage;
        _playerRespawn.respawn = RespawnButton;
        _playerRespawn.restart = RestartButton;
        _playerRespawn.EndGameMessage = EndGameMessage;
        _playerRespawn.uiAnimator = Animator;

        DataManager.ToTheCloud(DataKeys.FUSION_SEQUENCE, FusionSequence);
    }
}
