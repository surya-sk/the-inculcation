using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Player;

namespace CultGame.Gameplay
{
    public class Ending : MonoBehaviour
    {
        public Canvas HintCanvas;
        public Canvas BlankCanvas;
        public ThirdPersonCharacterController Player;
        public Camera SecondCamera;

        private bool m_Falling = false;
        private bool m_HasFallen = false;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(PromptJump());
        }

        private void Update()
        {
            if (m_Falling)
            {
                Time.timeScale = 0.1f;
            }
            if(m_HasFallen)
            {
                Time.timeScale = 1f;
                StartCoroutine(OnFall());
                m_HasFallen=false;
            }
        }

        IEnumerator PromptJump()
        {
            yield return new WaitForSeconds(4f);
            HintCanvas.enabled = false;
        }

        IEnumerator OnFall()
        {
            Player.gameObject.GetComponent<AudioSource>().Stop();
            yield return new WaitForSeconds(0.1f);
            Destroy(Player.gameObject);
            BlankCanvas.enabled = true;
            SecondCamera.enabled = true;
            yield return new WaitForSecondsRealtime(10f);
            BlankCanvas.enabled = false;
            yield return new WaitForSeconds(0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!m_Falling)
            {
                m_Falling = true;
            }
            else
            {
                m_HasFallen = true;
            }
        }
    }
}
