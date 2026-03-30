public enum EventType
{
    /// <summary>
    /// 无参
    /// </summary>
    StartGame,
    /// <summary>
    /// 无参
    /// </summary>
    PauseGame,

    /// <summary>
    /// Track类型参数
    /// </summary>
    ScreenInputTrackDown,
    /// <summary>
    /// Track类型参数
    /// </summary>
    ScreenInputTrackUp,

    /// <summary>
    /// (Track,KeyInputType)元组参数
    /// </summary>
    ScreenInput,

    /// <summary>
    /// KeyInputType参数类型
    /// </summary>
    Track_1,
    /// <summary>
    /// KeyInputType参数类型
    /// </summary>
    Track_2,
    /// <summary>
    /// KeyInputType参数类型
    /// </summary>
    Track_3,
    /// <summary>
    /// KeyInputType参数类型
    /// </summary>
    Track_4,

    /// <summary>
    /// 无参
    /// </summary>
    Update_SettingData,

    /// <summary>
    /// ResultType参数
    /// </summary>
    Update_InputResult,
}
