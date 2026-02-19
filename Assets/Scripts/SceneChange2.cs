using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
