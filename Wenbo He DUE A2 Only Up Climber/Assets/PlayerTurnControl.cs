using UnityEngine;
// 把这一行删掉（如果有的话）：using System.Diagnostics; 

public class PlayerTurnControl : MonoBehaviour
{
    public float turnAngle = 10f;

    public void SnapTurnLeft()
    {
        transform.Rotate(0, -turnAngle, 0);
        UnityEngine.Debug.Log("向左转了"); // 或者直接写 Debug.Log
    }

    public void SnapTurnRight()
    {
        transform.Rotate(0, turnAngle, 0);
        UnityEngine.Debug.Log("向右转了");
    }
}