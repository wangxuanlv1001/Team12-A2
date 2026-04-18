using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0, 1.6f, 0);

    [Header("鼠标灵敏度")]
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 3f;

    [Header("视角限制")]
    public float minYAngle = -25f;
    public float maxYAngle = 65f;

    [Header("相机距离")]
    public float defaultDistance = 4.5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;
    public float scrollSpeed = 10f;

    [Header("==== 平滑值（越大越跟得快）====")]
    public float smoothRotateSpeed = 20f;  // 视角平滑
    public float smoothMoveSpeed = 15f;    // 位置平滑

    [Header("防穿模")]
    public bool enableObstacleAvoidance = true;
    public LayerMask obstacleLayer;

    private float currentDistance;
    private float desiredDistance;
    private float currentX;
    private float currentY;

    void Start()
    {
        currentDistance = defaultDistance;
        desiredDistance = defaultDistance;
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // 鼠标输入
        currentX += Input.GetAxis("Mouse X") * mouseSensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // 滚轮缩放
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        // 计算旋转
        Quaternion targetRot = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, smoothRotateSpeed * Time.deltaTime);

        // 计算距离
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, smoothMoveSpeed * Time.deltaTime);

        // 最终位置
        Vector3 targetPos = target.position + targetOffset;
        Vector3 finalPos = targetPos - transform.forward * currentDistance;

        // 防穿模
        if (enableObstacleAvoidance)
        {
            if (Physics.Linecast(targetPos, finalPos, out RaycastHit hit, obstacleLayer))
            {
                finalPos = hit.point;
            }
        }

        // 平滑移动相机（核心：丝滑不卡顿的关键）
        transform.position = Vector3.Lerp(transform.position, finalPos, smoothMoveSpeed * Time.deltaTime);
    }
}