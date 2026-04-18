using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;           // 拖入 Player

    [Header("视角参数")]
    public float distance = 10.0f;     // 相机离多远
    public float lookAtHeight = 1.5f;  // 注视点偏移（建议设为人物胸口或头部高度，比如1.5）

    [Header("鼠标控制参数")]
    public float mouseSensitivity = 3f; // 鼠标灵敏度
    public float minYAngle = -15f;      // 最小仰角（限制低头看地面的极限）
    public float maxYAngle = 80f;       // 最大仰角（限制抬头看天的极限）

    private float rotationX = 0f;       // 水平旋转角度
    private float rotationY = 25f;      // 垂直旋转角度

    void Start()
    {
        // 隐藏并锁定鼠标到屏幕中心 (按 ESC 键可以在编辑器中显示鼠标)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 如果你需要鼠标点UI，请把上面两行删掉或者注释掉
    }

    void LateUpdate()
    {
        if (target == null || !target.gameObject.activeSelf) return;
        if (target == null) return;

        // --- 核心修改：获取鼠标滑动的增量 ---
        // Input.GetAxis("Mouse X") 鼠标左右滑动
        // Input.GetAxis("Mouse Y") 鼠标上下滑动
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 累加旋转角度 
        rotationX += mouseX;
        // 鼠标往上推时 mouseY 为正，但我们希望相机视角往下（抬头），所以用减号
        rotationY -= mouseY;

        // 限制垂直视角，防止相机 360 度翻转跌入地下或翻过头顶
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        // 计算当前相机的旋转角度
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // 计算注视点（玩家位置 + 向上偏移，让相机围绕玩家上半身旋转而不是脚底）
        Vector3 lookAtPoint = target.position + Vector3.up * lookAtHeight;

        // 计算相机位置（围绕注视点向后退 distance 的距离）
        Vector3 position = rotation * new Vector3(0, 0, -distance) + lookAtPoint;

        // 应用位置和旋转
        transform.position = position;
        transform.LookAt(lookAtPoint);
    }
}