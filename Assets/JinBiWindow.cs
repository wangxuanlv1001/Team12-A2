using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JinBiWindow : MonoBehaviour
{
    public static JinBiWindow Instance;

    [Header("Coin Settings")]
    public int jinbi = 0;
    public int targetCoinCount = 17;

    [Header("UI")]
    public Text coinText;         // 左上角金币数量
    public GameObject winPanel;   // 胜利面板
    public Text winText;          // 面板上的 You Win!
    public Button restartButton;  // 重新开始按钮

    [Header("Audio")]
    public AudioSource bgmSource; // 背景音乐 AudioSource
    public AudioClip winSound;    // 胜利音效

    private bool isWin = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCoinUI();

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }

        // 如果场景一开始就有背景音乐 AudioSource，这里确保它在播放
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void AddJinBi(int count)
    {
        if (isWin) return;

        jinbi += count;
        UpdateCoinUI();

        if (jinbi >= targetCoinCount)
        {
            ShowWinPanel();
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coin: " + jinbi;
        }
    }

    private void ShowWinPanel()
    {
        isWin = true;

        // 停止背景音乐
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        // 显示胜利面板
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // 显示胜利文字
        if (winText != null)
        {
            winText.gameObject.SetActive(true);
            winText.text = "You Win!";
            winText.color = Color.black;
        }

        // 播放胜利音效
        if (winSound != null)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}