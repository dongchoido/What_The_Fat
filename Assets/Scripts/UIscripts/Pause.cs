using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        Debug.Log("Paused: " + isPaused);
    }
}
