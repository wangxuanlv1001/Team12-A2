using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 摄像机要跟随的目标 (Player)

    // 摄像机相对于目标的偏移量 (X, Y, Z)
    // 比如：Y=5 代表在头顶5米，Z=-8 代表在背后8米
    public Vector3 offset = new Vector3(0f, 5f, -8f);

    void LateUpdate()
    {
        if (target != null)
        {
            // 更新摄像机的位置
            transform.position = target.position + offset;

            // 让摄像机永远看着目标
            transform.LookAt(target);
        }
    }
}