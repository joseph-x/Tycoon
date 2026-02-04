using UnityEngine;
using UnityEngine.UI;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField] private Text timeDisplayText;
    [SerializeField] private Text dateDisplayText;

    private void Start()
    {
        // 订阅时间更新事件
        TimeManager.Instance.OnGameTimeUpdated += UpdateTimeDisplay;
        TimeManager.Instance.OnDayChanged += OnDayChanged;

        // 初始化显示
        UpdateTimeDisplay(0);
    }

    private void OnDestroy()
    {
        // 取消订阅事件（避免内存泄漏）
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnGameTimeUpdated -= UpdateTimeDisplay;
            TimeManager.Instance.OnDayChanged -= OnDayChanged;
        }
    }

    // 更新时间显示
    private void UpdateTimeDisplay(float gameTime)
    {
        timeDisplayText.text = TimeManager.Instance.GetHourMinuteString();
        dateDisplayText.text = TimeManager.Instance.GetDateString();
    }

    // 每天变化时触发
    private void OnDayChanged(int newDay)
    {
        Debug.Log("新的一天开始了: " + TimeManager.Instance.GetDateString());
        // 可以在这里触发每日事件，如税收计算、资源更新等
    }
}