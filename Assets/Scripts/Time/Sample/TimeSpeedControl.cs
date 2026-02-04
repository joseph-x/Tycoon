using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedControl : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button fastSpeedButton;
    [SerializeField] private Button fastestSpeedButton;

    private void Start()
    {
        // 绑定按钮事件
        pauseButton.onClick.AddListener(() => TimeManager.Instance.SetTimeSpeed(TimeManager.TimeSpeed.Paused));
        normalSpeedButton.onClick.AddListener(() => TimeManager.Instance.SetTimeSpeed(TimeManager.TimeSpeed.Normal));
        fastSpeedButton.onClick.AddListener(() => TimeManager.Instance.SetTimeSpeed(TimeManager.TimeSpeed.Fast));
        fastestSpeedButton.onClick.AddListener(() => TimeManager.Instance.SetTimeSpeed(TimeManager.TimeSpeed.Fastest));

        // 订阅时间速度变化事件，更新UI状态
        TimeManager.Instance.OnTimeSpeedChanged += UpdateButtonStates;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeSpeedChanged -= UpdateButtonStates;
        }
    }

    // 更新按钮状态（高亮当前选择的速度）
    private void UpdateButtonStates(TimeManager.TimeSpeed currentSpeed)
    {
        SetButtonActive(pauseButton, currentSpeed == TimeManager.TimeSpeed.Paused);
        SetButtonActive(normalSpeedButton, currentSpeed == TimeManager.TimeSpeed.Normal);
        SetButtonActive(fastSpeedButton, currentSpeed == TimeManager.TimeSpeed.Fast);
        SetButtonActive(fastestSpeedButton, currentSpeed == TimeManager.TimeSpeed.Fastest);
    }

    private void SetButtonActive(Button button, bool isActive)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = isActive ? Color.green : Color.gray;
        button.colors = colors;
    }
}