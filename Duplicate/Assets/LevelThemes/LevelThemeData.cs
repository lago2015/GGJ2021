using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "LevelThemeData", menuName = "Duplicate/LevelThemeData", order = 0)]
public class LevelThemeData : SerializedScriptableObject
{
    [Title("Changes player's base color")]
    public Material PlayerA_Material;
    public Material PlayerB_Material;

    [Title("Changes color of vfx")]
    public Color PlayerA_VFX;
    public Color PlayerB_VFX;
}
