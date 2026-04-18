using UnityEngine;

public class Eatable : MonoBehaviour
{
    [Header("动画设置")]
    public float amplitude = 0.2f;      // 上下跳动的幅度
    public float frequency = 2f;        // 跳动的频率
    public float rotateSpeed = 100f;    // 自转的速度

    private Vector3 _startPos;          // 记录初始位置

    void Start()
    {
        // 记录钥匙刚放进场景时的位置
        _startPos = transform.position;
    }

    void Update()
    {
        // 1. 实现上下漂浮 (使用正弦曲线)
        float newY = _startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);

        // 2. 实现左右自转
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    // 当玩家碰到钥匙时触发
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞到的物体标签是否为 Player
        if (other.CompareTag("Player"))
        {
            // 尝试从玩家身上获取 PlayerMovement 脚本
            PlayerMovement pm = other.GetComponent<PlayerMovement>();

            if (pm != null)
            {
                // 增加玩家身上的钥匙计数
                pm.keyCount++;

                // 调用玩家脚本里的刷新 UI 方法，让屏幕上的文字更新
                pm.UpdateKeyUI();

                UnityEngine.Debug.Log("吃到钥匙了！当前总数: " + pm.keyCount);
            }
            else
            {
                // 如果进到这里，说明你的父物体 Player_Root 身上没挂 PlayerMovement 脚本
                UnityEngine.Debug.LogWarning("警告：碰到了玩家，但在该物体上找不到 PlayerMovement 脚本！");
            }

            // 销毁钥匙自己
            Destroy(gameObject);
        }
    }
}