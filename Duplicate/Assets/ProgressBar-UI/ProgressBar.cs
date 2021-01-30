using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Required] public Slider SliderL;
    [Required] public Slider SliderR;
    public static ProgressBar _instance;
    private PlayerController _playerController;
    private Vector3 _finishLinePosition;
    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    
    private void Awake()
    {
        if (_instance == null) _instance = this;
        DataManager.SetValue(DataKeys.PROGRESSBAR,this);
    }

    private void Start()
    {
        _finishLinePosition = DataManager.GetValue<Vector3>(DataKeys.PLAYERA_FINISHLINE);
        _playerController = DataManager.GetValue<PlayerController>(DataKeys.PLAYERA);
    }

    private void FixedUpdate()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        //PlayerA
        var playerA = _playerController.transform.position;
        var distance = (_finishLinePosition - playerA).normalized;
        UpdateSliders(1 - distance.x);
    }

    /// <param name="value">zero to one</param>
    [Button]
    public void UpdateSliders(float value)
    {
        SliderL.value = value;
        SliderR.value = value;
    }
}
