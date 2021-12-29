using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CultGame.Utils
{
    public class LoadingScreen : MonoBehaviour
    {
        public Slider LoadingSlider;
        public GameObject OtherUI;

        private int m_sceneToLoad;
        private AsyncOperation m_operation;
        // Start is called before the first frame update
        void Start()
        {
            OtherUI.SetActive(false);
            m_sceneToLoad = GameSceneManager.SCENE_TO_LOAD;
            GameSceneManager.SCENE_TO_LOAD = -1;
            if(m_sceneToLoad != -1)
            {
                m_operation = SceneManager.LoadSceneAsync(m_sceneToLoad);
            }
        }

        // Update is called once per frame
        void Update()
        {
            LoadingSlider.value = Mathf.Clamp01(m_operation.progress / 0.9f);
        }
    }
}
