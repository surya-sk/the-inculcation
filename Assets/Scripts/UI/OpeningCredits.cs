using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CultGame.Utils;

namespace CultGame.UI
{
    public class OpeningCredits : MonoBehaviour
    {
        public TextMeshProUGUI Title;
        public SceneLoader SceneLoader;

        private void Start()
        {
            Title.enabled = false;
            StartCoroutine(Credits());
        }
        IEnumerator Credits()
        {
            Title.enabled = true;
            yield return new WaitForSeconds(5.0f);
            SceneLoader.LoadScene((int)Scenes.SCENE_2);
        }
    }
}
