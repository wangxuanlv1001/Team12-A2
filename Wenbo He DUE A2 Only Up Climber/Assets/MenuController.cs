using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("设置关联物体")]
    public GameObject menuPanel;    // 拖入你的整个黑色半透明菜单面板
    public MonoBehaviour playerScript; // 拖入你玩家身上的移动脚本（比如 PlayerMovement）

    void Start()
    {
        // 游戏刚开始时：
        menuPanel.SetActive(true);    // 显示菜单
        playerScript.enabled = false; // 禁用玩家移动脚本

        // 显示鼠标并解锁，方便点击按钮
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        // 隐藏菜单
        menuPanel.SetActive(false);
        // 开启玩家移动
        playerScript.enabled = true;

        // --- 修改这里 ---
        Cursor.lockState = CursorLockMode.None; // 设置为 None 表示不锁定鼠标
        Cursor.visible = true;                  // 设置为 true 表示鼠标可见
                                                // ----------------
    }
}