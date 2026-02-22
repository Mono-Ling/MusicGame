using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform
{
    public static bool IsPCPlatform()
    {
        switch (Application.platform)
        {
            // 包含所有 PC 相关平台
            case RuntimePlatform.WindowsPlayer:    // Windows 玩家端
            case RuntimePlatform.OSXPlayer:        // Mac 玩家端
            case RuntimePlatform.LinuxPlayer:      // Linux 玩家端
            // 编辑器内预览时的判定（可选，根据需求添加）
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.LinuxEditor:
                return true;
            default:
                return false;
        }
    }
}
