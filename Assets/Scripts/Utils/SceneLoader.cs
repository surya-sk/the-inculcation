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
        public GameObject LoadingScreen;

        public void ReloadGame()
        {
            StartGame();
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            SceneManager.LoadScene((int)Scenes.MAIN_MENU);
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
                        LoadScene((int)Scenes.SCENE_3);
                        break;
                    case "Scene2.sav":
                        LoadScene((int)Scenes.SCENE_2);
                        break;
                    case "Scene1.sav":
                        LoadScene((int)Scenes.SCENE_1);
                        break;
                }
            }
            else
            {
                LoadScene((int)Scenes.OPENING_TEXT);
            }
        }

        public void RestartGame()
        {
            Directory.Delete(Application.persistentDataPath, true);
            StartGame();
        }

        public void Credits()
        {
            LoadScene((int)Scenes.CREDITS);
        }


        public void LoadScene(int index)
        {
            GameSceneManager.SCENE_TO_LOAD = index;
            LoadingScreen.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void GameOver()
        {
            SceneManager.LoadSceneAsync((int)Scenes.GAME_OVER);
        }

        public string GetSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
