using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // 每次点击旋转的角度，比如 15 度
    public float rotateStep = 15f;

    // 向左转的函数
    public void ClickRotateLeft()
    {
        // 3D 游戏通常绕 Y 轴转 (0, -15, 0)
        // 2D 游戏请将 Vector3.up 改为 Vector3.forward
        transform.Rotate(Vector3.up * -rotateStep);
    }

    // 向右转的函数
    public void ClickRotateRight()
    {
        transform.Rotate(Vector3.up * rotateStep);
    }
}