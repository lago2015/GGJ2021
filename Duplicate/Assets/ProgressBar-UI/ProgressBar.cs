using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// - Need to know total distance from finish and current position
///     - make it normalized so its readable for the progress bar
/// 
/// </summary>

public class ProgressBar : MonoBehaviour
{
    [Required] public Slider SliderL;
    [Required] public Slider SliderR;
    public static ProgressBar _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    /// <param name="value">zero to one</param>
    [Button]
    public void UpdateSliders(float value)
    {
        SliderL.value = value;
        SliderR.value = value;
    }
}
