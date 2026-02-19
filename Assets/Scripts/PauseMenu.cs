using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePause : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool isPaused = false;

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}