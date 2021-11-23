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
            string[] fileNames = Directory.GetFiles(Application.persistentDataPath, "*.sav");
            int len = fileNames.Length;
            if(len > 0)
            {
                switch(Path.GetFileName(fileNames[len - 1]))
                {
                    case "Scene3.sav":
                        LoadScene(5);
                        break;
                    case "Scene2.sav":
                        LoadScene(4);
                        break;
                    case "Scene1.sav":
                        LoadScene(2);
                        break;
                }
            }
            else
            {
                LoadScene(1);
            }
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
            SceneManager.LoadSceneAsync(6);
        }

        public string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
