using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButtonCall : MonoBehaviour
{
    public void NextLevel()
    {
        DataManager.MakeItRain<SceneTransition>(DataKeys.SCENE_TRANSITION).NextLevel();
    }
}
