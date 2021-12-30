using CultGame.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Gameplay
{
    public class Disclaimer : MonoBehaviour
    {
        public SceneLoader SceneLoader;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadMainMenu());
        }

        IEnumerator LoadMainMenu()
        {
            yield return new WaitForSeconds(7f);
            SceneLoader.MainMenu();
        }
    }
}
