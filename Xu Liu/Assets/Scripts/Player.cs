using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public Text tex;
    public int i = 0;
    int s = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coin"))
        {
            i=i+1*s;
            tex.text = i.ToString();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("win"))
        {
            SceneManager.LoadScene(2);
        }
        if (other.CompareTag("failed"))
        {
            SceneManager.LoadScene(3);
        }
        if (other.CompareTag("a"))
        {
            Destroy(other.gameObject);

            s = 2;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
