using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("Dialogue");
    }
}
