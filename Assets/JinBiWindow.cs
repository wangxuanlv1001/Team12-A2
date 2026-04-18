using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JinBiWindow : MonoBehaviour
{
    public static JinBiWindow Instance;

    public int jinbi = 0;

    public Text coinText;         // 左上角金币数量
    public GameObject winPanel;   // 结束面板
    public Text winText;          // 结束面板上的 You Win
    public Button restartButton;  // 重新开始按钮

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
    }

    public void AddJinBi(int count)
    {
        jinbi += count;
        UpdateCoinUI();

        if (!isWin && jinbi >= 17)
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

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (winText != null)
        {
            winText.gameObject.SetActive(true);
            winText.text = "You Win!";
            winText.color = Color.black;
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}