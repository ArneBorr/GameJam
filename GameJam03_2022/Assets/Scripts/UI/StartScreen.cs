using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene("LoadingGame");
    }
}
