using UnityEngine;

public class JinBi : MonoBehaviour
{
    public AudioClip coinSound;
    public float rotateSpeed = 180f;

    private bool isCollected = false;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            if (JinBiWindow.Instance != null)
            {
                JinBiWindow.Instance.AddJinBi(1);
            }

            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}