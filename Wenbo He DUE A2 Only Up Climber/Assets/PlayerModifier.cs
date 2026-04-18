using UnityEngine; // 必须只有这一行

public class PlayerModifier : MonoBehaviour
{
    public GameObject player;

    private Vector3 initialScale;

    void Start()
    {
        if (player != null)
        {
            // 记录游戏开始时你设置的初始大小
            initialScale = player.transform.localScale;
        }
    }

    // --- 变大 ---
    public void MakeBigger()
    {
        if (player != null) player.transform.localScale *= 1.2f;
    }

    // --- 缩小 ---
    public void MakeSmaller()
    {
        if (player != null) player.transform.localScale *= 0.8f;
    }

    // --- 向左平移 ---
    public void MoveLeft()
    {
        if (player != null) player.transform.Translate(Vector3.left * 1.0f);
    }

    // --- 向右平移 ---
    public void MoveRight()
    {
        if (player != null) player.transform.Translate(Vector3.right * 1.0f);
    }

    // --- 恢复原状 ---
    public void ResetPlayer()
    {
        if (player != null)
        {
            // 恢复到 Start() 里记录的那个初始大小
            player.transform.localScale = initialScale;
            // 如果你想让位置也回去，可以去掉下面这一行的注释：
            // player.transform.position = new Vector3(0, 1, 0); 

            UnityEngine.Debug.Log("已恢复到初始设置的大小");
        }
    }
}