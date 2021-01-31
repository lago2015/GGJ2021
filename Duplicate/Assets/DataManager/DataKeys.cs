using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class DataKeys
{
    public const string PLAYERA = "PlayerA"; // type : PlayerController

    public const string PLAYERB = "PlayerB"; // type : PlayerController

    public const string PLAYERA_FINISHLINE = "PlayerA_FinishLine"; // type : Vector 3

    public const string PLAYERB_FINISHLINE = "PlayerB_StartLine"; // type : Vector 3
    public static string PLAYERA_STARTLINE = "START_POSITION_A"; // type : Vector3
    public static string CURRENT_LEVEL_THEME = "CURRENT_LEVEL_THEME"; // type : LevelThemeData.cs

    public const string PLAYER_RESPAWN = "PlayerRespawn";

    public const string PROGRESSBAR = "ProgressBar";

    public const string COUNTDOWN_STARTGAME = "Countdown_StartGame";

    public const string SCENE_TRANSITION = "Scene_Transition";

    public const string FUSION_SEQUENCE = "Fusion_Sequence";
}
