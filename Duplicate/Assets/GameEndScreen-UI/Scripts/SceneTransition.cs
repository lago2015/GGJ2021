using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    private void Awake()
    {
        DataManager.ToTheCloud(DataKeys.SCENE_TRANSITION,this);
    }

    //Called from a unity button
    public void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }

    public void NextLevelWithDelay()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if(currentLevel + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            DataManager.MakeItRain<PlayerRespawn>(DataKeys.PLAYER_RESPAWN).SetUpGameCompleteScreen();
        }
        else
        {
            StartCoroutine(DelayToNextLevel());    
        }
    }

    IEnumerator DelayToNextLevel()
    {
        yield return new WaitForSeconds(0.5f);
        NextLevel();
    }
    
    //Called from a unity button
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
