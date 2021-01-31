using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds a reference to the current level's color theme scriptable.
/// Different reference is set in every level.
/// 
/// On awake, post it up into the cloud.
/// </summary>
public class RegisterLevelTheme : MonoBehaviour
{
    public int currentLevel; // Index into the list. Post this element into the datamanager for others to use..
    public List<LevelThemeData> AllLevelThemes;
    private void Awake()
    {
        DataManager.ToTheCloud(DataKeys.CURRENT_LEVEL_THEME, AllLevelThemes[currentLevel]);
    }
}
