using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Saber.Base;

public class TimeManager : Singleton<TimeManager>
{

    #region 时间配置
    [Header("=== 时间配置 ===")]

    [Tooltip("现实世界1秒 = 游戏内多少分钟")]
    [SerializeField] private float secondsPerGameMinute = 0.5f;

    [Tooltip("游戏内1小时有多少分钟")]
    [SerializeField] private int minutesPerGameHour = 60;

    [Tooltip("游戏内1天有多少小时")]
    [SerializeField] private int hoursPerGameDay = 24;

    [Tooltip("游戏内1月有多少天")]
    [SerializeField] private int daysPerGameMonth = 30;

    [Tooltip("游戏内1年有多少月")]
    [SerializeField] private int monthsPerGameYear = 12;

    #endregion

    #region 时间状态
    [Header("=== 时间状态 ===")]

    [Tooltip("当前游戏内总时间（秒）")]
    private float _currentGameTimeSeconds = 0f;

    [Tooltip("当前时间速度")]
    [SerializeField] private TimeSpeed _currentTimeSpeed = TimeSpeed.Normal;

    #endregion

    #region 时间坐标（年/月/日/时/分）
    /// <summary>当前年份</summary>
    public int CurrentYear { get; private set; } = 1;

    /// <summary>当前月份（1-12）</summary>
    public int CurrentMonth { get; private set; } = 1;

    /// <summary>当前日期（1-30）</summary>
    public int CurrentDay { get; private set; } = 1;

    /// <summary>当前小时（0-23）</summary>
    public int CurrentHour { get; private set; } = 0;

    /// <summary>当前分钟（0-59）</summary>
    public int CurrentMinute { get; private set; } = 0;

    #endregion

    #region 时间事件系统
    /// <summary>游戏时间更新事件（参数：总游戏时间秒数）</summary>
    public event Action<float> OnGameTimeUpdated;

    /// <summary>分钟变化事件（参数：当前分钟）</summary>
    public event Action<int> OnMinuteChanged;

    /// <summary>小时变化事件（参数：当前小时）</summary>
    public event Action<int> OnHourChanged;

    /// <summary>日期变化事件（参数：当前日期）</summary>
    public event Action<int> OnDayChanged;

    /// <summary>月份变化事件（参数：当前月份）</summary>
    public event Action<int> OnMonthChanged;

    /// <summary>年份变化事件（参数：当前年份）</summary>
    public event Action<int> OnYearChanged;

    /// <summary>时间速度变化事件（参数：新的时间速度）</summary>
    public event Action<TimeSpeed> OnTimeSpeedChanged;

    #endregion

    #region 时间速度枚举
    /// <summary>
    /// 时间速度枚举
    /// </summary>
    public enum TimeSpeed
    {
        Paused = 0,   // 暂停
        Normal = 1,   // 正常速度
        Fast = 2,     // 2倍速
        Faster = 3,   // 3倍速
        Fastest = 4   // 4倍速
    }

    #endregion

    #region 时间速度倍率映射
    /// <summary>
    /// 时间速度对应的倍率映射
    /// </summary>
    private readonly Dictionary<TimeSpeed, float> _speedMultipliers = new Dictionary<TimeSpeed, float>
    {
        { TimeSpeed.Paused, 0f },
        { TimeSpeed.Normal, 1f },
        { TimeSpeed.Fast, 2f },
        { TimeSpeed.Faster, 4f },
        { TimeSpeed.Fastest, 8f }
    };

    #endregion

    #region 生命周期
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (_currentTimeSpeed == TimeSpeed.Paused)
            return;

        // 更新游戏时间
        UpdateGameTime();
    }

    #endregion

    #region 初始化与更新
    /// <summary>
    /// 初始化时间状态
    /// </summary>
    private void InitializeTimeState()
    {
        // 从存档加载时间状态（如果有）
        // LoadTimeFromSave();

        // 初始时间状态（如果没有存档）
        _currentGameTimeSeconds = 0f;
        CurrentYear = 1;
        CurrentMonth = 1;
        CurrentDay = 1;
        CurrentHour = 0;
        CurrentMinute = 0;
    }

    /// <summary>
    /// 更新游戏时间
    /// </summary>
    private void UpdateGameTime()
    {
        // 计算当前帧的时间增量（转换为游戏内时间）
        float timeMultiplier = _speedMultipliers[_currentTimeSpeed];
        float deltaGameSeconds = Time.deltaTime * timeMultiplier * (60f / secondsPerGameMinute);

        // 更新总游戏时间
        float previousGameTime = _currentGameTimeSeconds;
        _currentGameTimeSeconds += deltaGameSeconds;

        // 更新时间坐标（年/月/日/时/分）
        UpdateTimeCoordinates();

        // 触发时间更新事件
        OnGameTimeUpdated?.Invoke(_currentGameTimeSeconds);
    }

    /// <summary>
    /// 更新时间坐标（年/月/日/时/分）
    /// </summary>
    private void UpdateTimeCoordinates()
    {
        // 保存之前的时间状态用于比较
        int previousMinute = CurrentMinute;
        int previousHour = CurrentHour;
        int previousDay = CurrentDay;
        int previousMonth = CurrentMonth;
        int previousYear = CurrentYear;

        // 计算总分钟数
        float totalMinutes = _currentGameTimeSeconds / 60f;

        // 计算各时间单位
        int totalHours = Mathf.FloorToInt(totalMinutes / minutesPerGameHour);
        int totalDays = Mathf.FloorToInt(totalHours / hoursPerGameDay);
        int totalMonths = Mathf.FloorToInt(totalDays / daysPerGameMonth);
        int totalYears = Mathf.FloorToInt(totalMonths / monthsPerGameYear);

        // 更新当前时间坐标
        CurrentYear = totalYears + 1; // 年份从1开始
        CurrentMonth = (totalMonths % monthsPerGameYear) + 1; // 月份从1开始
        CurrentDay = (totalDays % daysPerGameMonth) + 1; // 日期从1开始
        CurrentHour = totalHours % hoursPerGameDay;
        CurrentMinute = Mathf.FloorToInt(totalMinutes % minutesPerGameHour);

        // 检查并触发时间变化事件
        CheckAndFireTimeEvents(previousMinute, previousHour, previousDay, previousMonth, previousYear);
    }

    /// <summary>
    /// 检查并触发时间变化事件
    /// </summary>
    private void CheckAndFireTimeEvents(int prevMinute, int prevHour, int prevDay, int prevMonth, int prevYear)
    {
        if (CurrentMinute != prevMinute)
            OnMinuteChanged?.Invoke(CurrentMinute);

        if (CurrentHour != prevHour)
            OnHourChanged?.Invoke(CurrentHour);

        if (CurrentDay != prevDay)
            OnDayChanged?.Invoke(CurrentDay);

        if (CurrentMonth != prevMonth)
            OnMonthChanged?.Invoke(CurrentMonth);

        if (CurrentYear != prevYear)
            OnYearChanged?.Invoke(CurrentYear);
    }

    #endregion

    #region 时间速度控制
    /// <summary>
    /// 设置时间速度
    /// </summary>
    /// <param name="speed">新的时间速度</param>
    public void SetTimeSpeed(TimeSpeed speed)
    {
        if (_currentTimeSpeed != speed)
        {
            _currentTimeSpeed = speed;
            OnTimeSpeedChanged?.Invoke(speed);
        }
    }

    /// <summary>
    /// 获取当前时间速度
    /// </summary>
    public TimeSpeed GetCurrentTimeSpeed()
    {
        return _currentTimeSpeed;
    }

    /// <summary>
    /// 暂停/继续游戏时间
    /// </summary>
    public void TogglePause()
    {
        if (_currentTimeSpeed == TimeSpeed.Paused)
            SetTimeSpeed(TimeSpeed.Normal);
        else
            SetTimeSpeed(TimeSpeed.Paused);
    }

    /// <summary>
    /// 获取当前是否暂停
    /// </summary>
    public bool IsPaused()
    {
        return _currentTimeSpeed == TimeSpeed.Paused;
    }

    #endregion

    #region 时间计算与转换
    /// <summary>
    /// 获取两个时间点之间的分钟差
    /// </summary>
    public float GetMinutesBetween(float time1, float time2)
    {
        return Mathf.Abs(time2 - time1) / 60f;
    }

    /// <summary>
    /// 获取两个时间点之间的小时差
    /// </summary>
    public float GetHoursBetween(float time1, float time2)
    {
        return GetMinutesBetween(time1, time2) / minutesPerGameHour;
    }

    /// <summary>
    /// 获取两个时间点之间的天数差
    /// </summary>
    public float GetDaysBetween(float time1, float time2)
    {
        return GetHoursBetween(time1, time2) / hoursPerGameDay;
    }

    /// <summary>
    /// 将游戏内时间转换为总分钟数
    /// </summary>
    public float ConvertToTotalMinutes(int year, int month, int day, int hour, int minute)
    {
        int totalMonths = (year - 1) * monthsPerGameYear + (month - 1);
        int totalDays = totalMonths * daysPerGameMonth + (day - 1);
        int totalHours = totalDays * hoursPerGameDay + hour;
        return totalHours * minutesPerGameHour + minute;
    }

    /// <summary>
    /// 将总游戏时间（秒）转换为时间坐标
    /// </summary>
    public void ConvertToTimeCoordinates(float totalSeconds, out int year, out int month, out int day, out int hour, out int minute)
    {
        float totalMinutes = totalSeconds / 60f;
        int totalHours = Mathf.FloorToInt(totalMinutes / minutesPerGameHour);
        int totalDays = Mathf.FloorToInt(totalHours / hoursPerGameDay);
        int totalMonths = Mathf.FloorToInt(totalDays / daysPerGameMonth);
        int totalYears = Mathf.FloorToInt(totalMonths / monthsPerGameYear);

        year = totalYears + 1;
        month = (totalMonths % monthsPerGameYear) + 1;
        day = (totalDays % daysPerGameMonth) + 1;
        hour = totalHours % hoursPerGameDay;
        minute = Mathf.FloorToInt(totalMinutes % minutesPerGameHour);
    }

    #endregion

    #region 时间格式化
    /// <summary>
    /// 获取格式化的时间字符串（完整格式：年/月/日 时:分）
    /// </summary>
    public string GetFullTimeString()
    {
        return $"{CurrentYear}年{CurrentMonth}月{CurrentDay}日 {CurrentHour:00}:{CurrentMinute:00}";
    }

    /// <summary>
    /// 获取简化的时间字符串（月/日 时:分）
    /// </summary>
    public string GetShortTimeString()
    {
        return $"{CurrentMonth}/{CurrentDay} {CurrentHour:00}:{CurrentMinute:00}";
    }

    /// <summary>
    /// 获取当前时间的小时:分钟格式
    /// </summary>
    public string GetHourMinuteString()
    {
        return $"{CurrentHour:00}:{CurrentMinute:00}";
    }

    /// <summary>
    /// 获取当前日期字符串（年/月/日）
    /// </summary>
    public string GetDateString()
    {
        return $"{CurrentYear}年{CurrentMonth}月{CurrentDay}日";
    }

    #endregion

    #region 时间事件辅助
    /// <summary>
    /// 检查当前是否是指定的小时
    /// </summary>
    public bool IsCurrentHour(int hour)
    {
        return CurrentHour == hour;
    }

    /// <summary>
    /// 检查当前是否是指定的日期
    /// </summary>
    public bool IsCurrentDay(int day)
    {
        return CurrentDay == day;
    }

    /// <summary>
    /// 检查当前是否是指定的月份
    /// </summary>
    public bool IsCurrentMonth(int month)
    {
        return CurrentMonth == month;
    }

    /// <summary>
    /// 计算从现在到指定时间的剩余时间（秒）
    /// </summary>
    public float GetRemainingTimeTo(int targetYear, int targetMonth, int targetDay, int targetHour, int targetMinute)
    {
        float targetTotalSeconds = ConvertToTotalMinutes(targetYear, targetMonth, targetDay, targetHour, targetMinute) * 60f;
        return Mathf.Max(0f, targetTotalSeconds - _currentGameTimeSeconds);
    }

    #endregion

    #region 存档与读档
    /// <summary>
    /// 保存时间状态到存档
    /// </summary>
    public void SaveTimeState(Dictionary<string, object> saveData)
    {
        saveData["GameTimeSeconds"] = _currentGameTimeSeconds;
        saveData["CurrentYear"] = CurrentYear;
        saveData["CurrentMonth"] = CurrentMonth;
        saveData["CurrentDay"] = CurrentDay;
        saveData["CurrentHour"] = CurrentHour;
        saveData["CurrentMinute"] = CurrentMinute;
        saveData["TimeSpeed"] = (int)_currentTimeSpeed;
    }

    /// <summary>
    /// 从存档加载时间状态
    /// </summary>
    public void LoadTimeState(Dictionary<string, object> saveData)
    {
        if (saveData.ContainsKey("GameTimeSeconds"))
            _currentGameTimeSeconds = Convert.ToSingle(saveData["GameTimeSeconds"]);

        if (saveData.ContainsKey("CurrentYear"))
            CurrentYear = Convert.ToInt32(saveData["CurrentYear"]);

        if (saveData.ContainsKey("CurrentMonth"))
            CurrentMonth = Convert.ToInt32(saveData["CurrentMonth"]);

        if (saveData.ContainsKey("CurrentDay"))
            CurrentDay = Convert.ToInt32(saveData["CurrentDay"]);

        if (saveData.ContainsKey("CurrentHour"))
            CurrentHour = Convert.ToInt32(saveData["CurrentHour"]);

        if (saveData.ContainsKey("CurrentMinute"))
            CurrentMinute = Convert.ToInt32(saveData["CurrentMinute"]);

        if (saveData.ContainsKey("TimeSpeed"))
            _currentTimeSpeed = (TimeSpeed)Convert.ToInt32(saveData["TimeSpeed"]);
    }

    #endregion
}
