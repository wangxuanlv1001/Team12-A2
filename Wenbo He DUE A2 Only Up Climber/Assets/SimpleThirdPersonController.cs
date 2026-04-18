using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleThirdPersonController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("引用")]
    public Transform cam; // 拖入主相机
    public Animator anim; // 拖入子模型

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main.transform;
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 1. 落地检测
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 贴地力
        }

        // 2. 移动输入
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        if (input.magnitude >= 0.1f)
        {
            // 计算相机朝向的旋转角度
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            // 平滑旋转人物
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 移动方向
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            if (anim != null) anim.SetFloat("Speed", 1f); // 激活跑动
        }
        else
        {
            if (anim != null) anim.SetFloat("Speed", 0f); // 恢复待机
        }

        // 3. 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (anim != null) anim.SetTrigger("jump");
        }

        // 4. 重力应用
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        // 限制最大下坠速度，防止穿透地面
    }
}