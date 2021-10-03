using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CultGame.Utils
{
    public class SceneLoader : MonoBehaviour
    {
        public TextMeshProUGUI loadingText;
        public void ReloadGame()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            SceneManager.LoadScene(0);
        }

        public void StartGame()
        {
            //if (File.Exists(Path.Combine(Application.persistentDataPath, "Chapter 3.sav")))
            //{
            //}
            //else if (File.Exists(Path.Combine(Application.persistentDataPath, "Chapter 2.sav")))
            //{
            //}
            //else
            //{
            //}
        }

        public void RestartGame()
        {
            Directory.Delete(Application.persistentDataPath, true);
            StartGame();
        }


        public void LoadScene(int index)
        {
            StartCoroutine(LoadAsync(index));
        }

        IEnumerator LoadAsync(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            if (operation.progress < 1.0)
            {
                if(loadingText != null)
                {
                    loadingText.text = "Loading...";
                }
                yield return new WaitForSeconds(3);
            }
            loadingText.text = string.Empty;
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GameOver()
        {
            SceneManager.LoadSceneAsync(5);
        }

        public string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
