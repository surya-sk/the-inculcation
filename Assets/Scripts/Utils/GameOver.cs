using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CultGame.Utils
{
    public class GameOver : MonoBehaviour
    {
        public Canvas gameOverCanvas;
        public TextMeshProUGUI reasonText;  // reason why game is over
        public SceneLoader sceneLoader;
        public GameObject pauseMenu;
        public GameObject otherUI;
        // Start is called before the first frame update
        void Start()
        {
            gameOverCanvas.enabled = false;
        }

        public void EndGame(string reason)
        {
            reasonText.text = reason;
            StartCoroutine(TriggerGameOver());
        }

        IEnumerator TriggerGameOver()
        {
            gameOverCanvas.enabled = true;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseMenu.SetActive(false);
            otherUI.SetActive(false);
            yield return new WaitForSeconds(4);
            sceneLoader.GameOver();
        }
    }

}