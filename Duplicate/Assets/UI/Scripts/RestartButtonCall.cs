using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonCall : MonoBehaviour
{
    public SceneTransition _sceneTransition;
    
    //called from unity button
    public void RestartPlayers()
    {
        if(!_sceneTransition) return;
        _sceneTransition.RestartGame();
    }
    
}
