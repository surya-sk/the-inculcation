using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Utils
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject otherUI;

        public static bool isPaused = false;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton7)) 
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        void Pause()
        {
            otherUI.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            AudioListener.pause = true;
            isPaused = true;
        }

        public void Resume()
        {
            otherUI.SetActive(true);
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            AudioListener.pause = false;
            isPaused = false;
        }
    }
}
