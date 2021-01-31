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
    private Vector3 _startLinePosition; // player A
    private float _totalDistance;
    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if(value) UpdateSliders(0);
            _isActive = value;  
        } 
    }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        DataManager.ToTheCloud(DataKeys.PROGRESSBAR, this);
    }

    private void Start()
    {
        _startLinePosition = DataManager.MakeItRain<Vector3>(DataKeys.PLAYERA_STARTLINE);
        _finishLinePosition = DataManager.MakeItRain<Vector3>(DataKeys.PLAYERA_FINISHLINE);
        _totalDistance = Vector2.Distance(_finishLinePosition, _startLinePosition);
        _playerController = DataManager.MakeItRain<PlayerController>(DataKeys.PLAYERA);
        ReskinHandlesAndBars();
    }

    private void ReskinHandlesAndBars()
    {
        var levelTheme = DataManager.MakeItRain<LevelThemeData>(DataKeys.CURRENT_LEVEL_THEME);

        SliderL.targetGraphic.material = new Material(levelTheme.PlayerA_Material);
        SliderR.targetGraphic.material = new Material(levelTheme.PlayerB_Material);
        SliderL.fillRect.GetComponent<Image>().material = new Material(levelTheme.PlayerA_Material);
        SliderR.fillRect.GetComponent<Image>().material = new Material(levelTheme.PlayerB_Material);
    }

    private void FixedUpdate()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        var curPos = _playerController.transform.position;
        var distanceToFinish = Mathf.Abs(_finishLinePosition.x - curPos.x);
        var progress = 1 - (distanceToFinish / _totalDistance);

        UpdateSliders(progress);
    }

    /// <param name="value">zero to one</param>
    [Button]
    public void UpdateSliders(float value)
    {
        SliderL.value = value;
        SliderR.value = value;
    }
}
