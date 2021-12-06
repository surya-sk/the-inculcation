using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Player;

namespace CultGame.Gameplay
{
    public class Ending : MonoBehaviour
    {
        public Canvas HintCanvas;
        public ThirdPersonCharacterController Player;

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
            yield return new WaitForSeconds(0.1f);
            Destroy(Player.gameObject);
            // TODO Switch to different camera
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
