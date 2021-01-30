using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonCall : MonoBehaviour
{
    //called from unity button
    public void RestartPlayers()
    {
        DataManager.GetValue<SceneTransition>(DataKeys.SCENE_TRANSITION).RestartGame();
    }
}
