using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esc键切换鼠标光标显示/隐藏，支持UI交互
/// </summary>
public class CursorToggleManager : MonoBehaviour
{
    // 标记当前光标是否处于显示状态
    private bool _isCursorVisible = false;
    public List<MonoBehaviour> monoBehaviour;
    void Start()
    {
        // 游戏启动时默认隐藏光标（可根据你的需求修改）
        SetCursorState(false);
    }

    void Update()
    {
        // 检测Esc键按下（GetKeyDown确保每次按键只触发一次切换）
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 切换光标状态（显示↔隐藏）
            _isCursorVisible = !_isCursorVisible;
            SetCursorState(_isCursorVisible);
        }


    }

    /// <summary>
    /// 统一设置光标状态的核心方法
    /// </summary>
    /// <param name="isVisible">是否显示光标</param>
    private void SetCursorState(bool isVisible)
    {
        if (isVisible)
        {
            // 显示光标：解锁光标、可见、允许点击UI
            Cursor.lockState = CursorLockMode.None; // 解锁光标，允许自由移动
            Cursor.visible = true; // 显示光标

            foreach (var item in monoBehaviour)
            {
                item.enabled = false;
            }


        }
        else
        {
            // 隐藏光标：锁定光标到屏幕中心、不可见、恢复游戏内鼠标控制
            Cursor.lockState = CursorLockMode.Locked; // 锁定光标到屏幕中心（避免鼠标移出游戏窗口）
            Cursor.visible = false; // 隐藏光标
            foreach (var item in monoBehaviour)
            {
                item.enabled = true;
            }
        }
    }

    // 提供外部调用接口（可选，比如其他脚本需要主动切换光标状态时使用）
    public void ToggleCursor()
    {
        _isCursorVisible = !_isCursorVisible;
        SetCursorState(_isCursorVisible);
    }
}