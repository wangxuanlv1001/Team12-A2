using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 7f;            // 跑步速度
    public float jumpHeight = 2.5f;         // 跳跃高度
    public float gravity = -35f;            // 重力

    [Header("平滑参数")]
    public float turnSmoothTime = 0.1f;     // 转向平滑时间
    private float turnSmoothVelocity;

    [Header("引用")]
    public Transform cam;                   // 主相机
    public Animator anim;                   // 子模型

    private CharacterController controller;
    private Vector3 velocity;

    // --- 跳跃加固变量 ---
    private float groundedTimer;            // 落地缓冲计时器

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main.transform;
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 1. 增强版落地检测
        if (controller.isGrounded)
        {
            groundedTimer = 0.2f; // 只要着地，就刷新 0.2 秒的容错时间
        }
        else
        {
            groundedTimer -= Time.deltaTime; // 离地开始倒计时
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -5f; // 强力压在地面，防止跑步颠簸导致离地
        }

        // 2. 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 3. 更新动画机参数 (对应你的 2D 混合树)
        if (anim != null)
        {
            anim.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            anim.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
            anim.SetFloat("Speed", direction.magnitude, 0.1f, Time.deltaTime);
        }

        // 4. 处理移动与旋转
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // 5. 灵敏跳跃 (使用 groundedTimer 替代 controller.isGrounded)
        if (Input.GetKeyDown(KeyCode.Space) && groundedTimer > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (anim != null) anim.SetTrigger("jump");
            groundedTimer = 0; // 跳起瞬间清空计时器，防止连跳
        }

        // 6. 应用重力
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -20f) velocity.y = -20f; // 限制最大下坠速度

        controller.Move(velocity * Time.deltaTime);
    }
}