using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ClosePachinko()
    {
        SceneManager.LoadScene("MainScene");
    }
}