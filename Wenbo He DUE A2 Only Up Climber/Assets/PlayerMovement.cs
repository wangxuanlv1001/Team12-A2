using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public AudioClip footstepSound;
    public AudioClip collectSound;   // 吃钥匙音效
    public AudioClip winSound;       // 通关音效
    private AudioSource audioSource; // 播放器引用
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float turnSpeed = 15f;
    public float jumpHeight = 2.0f;
    public float gravity = -25f;

    [Header("引用")]
    public Transform mainCamera;
    public Animator anim;

    [Header("关卡/UI设置")]
    public int keyCount = 0;
    private int totalKeys = 0;
    public TextMeshProUGUI keyText;

    // --- 新增：通关面板引用 ---
    [Header("通关界面")]
    public GameObject winPanel;

    // --- 修改点：使用数组来存放多面墙 ---
    [Header("障碍墙 (拿齐钥匙后消失)")]
    public GameObject[] doorBarriers;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGameOver = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        if (mainCamera == null) mainCamera = Camera.main.transform;
        if (anim == null) anim = GetComponentInChildren<Animator>();

        // 自动统计钥匙总数
        totalKeys = GameObject.FindGameObjectsWithTag("Key").Length;

        // 确保通关面板一开始是隐藏的
        if (winPanel != null) winPanel.SetActive(false);

        // 初始锁定鼠标（如果你有旋转视角需求）
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateKeyUI();
    }

    void Update()
    {
        if (isGameOver) return;

        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            if (anim != null) anim.SetFloat("Speed", inputDir.magnitude);
            Vector3 camForward = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveDir = (v * camForward + h * mainCamera.right).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), turnSpeed * Time.deltaTime);
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (anim != null) anim.SetFloat("Speed", 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (anim != null) anim.SetTrigger("jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void UpdateKeyUI()
    {
        if (keyText != null)
        {
            keyText.text = "Keys: " + keyCount + " / " + totalKeys;
        }

        if (keyCount >= totalKeys && totalKeys > 0)
        {
            foreach (GameObject wall in doorBarriers)
            {
                if (wall != null) wall.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Key"))
        {
            // 1. 播放吃钥匙音效
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            keyCount++;
            UpdateKeyUI();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("House"))
        {
            if (keyCount >= totalKeys)
            {
                // 2. 播放通关音效
                if (winSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(winSound);
                }
                WinGame();
            }
            else
            {
                FailGame();
            }
        }
    }

    void WinGame()
    {
        isGameOver = true;

        // --- 修改点：为了让UI按钮有效，我们不禁用整个物体，只禁用移动和模型渲染 ---
        controller.enabled = false; // 禁用物理控制器
        if (anim != null) anim.gameObject.SetActive(false); // 隐藏模型

        // 显示通关面板
        if (winPanel != null) winPanel.SetActive(true);

        // 释放鼠标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (keyText != null)
        {
            keyText.text = "LEVEL COMPLETE!";
            keyText.color = Color.green;
        }
    }

    void FailGame()
    {
        UnityEngine.Debug.Log("钥匙不够！");
    }

    // --- 新增：给UI按钮调用的公共方法 ---
    public void LoadNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PlayFootstep()
    {
        // 只有在地面上移动时才响
        if (controller.isGrounded && footstepSound != null && audioSource != null)
        {
            // 降低一点音量（0.5），脚步声不宜太大
            audioSource.PlayOneShot(footstepSound, 0.5f);
        }
    }

}