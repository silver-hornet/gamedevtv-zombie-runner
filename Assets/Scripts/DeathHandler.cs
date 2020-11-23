using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;

    void Start()
    {
        gameOverCanvas.enabled = false;
    }

    public void HandleDeath()
    {
        gameOverCanvas.enabled = true;
        Time.timeScale = 0; // stop time so that we don't have any weird conflict between the game wanting to use the cursor and the cursor being needed for the game over screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
