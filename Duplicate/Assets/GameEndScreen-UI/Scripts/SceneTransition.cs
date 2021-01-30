using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    private void Awake()
    {
        DataManager.SetValue(DataKeys.SCENE_TRANSITION,this);
    }

    //Called from a unity button
    public void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }
    //Called from a unity button
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
