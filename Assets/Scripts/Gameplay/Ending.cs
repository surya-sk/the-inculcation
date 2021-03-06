using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Player;
using TMPro;
using CultGame.Utils;

namespace CultGame.Gameplay
{
    public class Ending : MonoBehaviour
    {
        public Canvas HintCanvas;
        public Canvas BlankCanvas;
        public ThirdPersonCharacterController Player;
        public Camera SecondCamera;
        public Transform CameraEndPoint;
        public TextMeshProUGUI DisciplineText;
        public TextMeshProUGUI PersonText;
        public string[] Disciplines;
        public string[] Artists;
        public float TimeBetweenCredits;
        public SceneLoader SceneLoader;

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

        /// <summary>
        /// Shows the follow hint for 4 seconds
        /// </summary>
        /// <returns></returns>
        IEnumerator PromptJump()
        {
            yield return new WaitForSeconds(4f);
            HintCanvas.enabled = false;
        }

        /// <summary>
        /// Shows a black screen, enables ending camera and setups up interpolation after player has fallen into pit
        /// </summary>
        /// <returns></returns>
        IEnumerator OnFall()
        {
            Player.gameObject.GetComponent<AudioSource>().Stop();
            yield return new WaitForSeconds(0.1f);
            Destroy(Player.gameObject);
            BlankCanvas.enabled = true;
            SecondCamera.enabled = true;
            yield return new WaitForSecondsRealtime(10f);
            BlankCanvas.enabled = false;
            yield return new WaitForSecondsRealtime(5f);
            StartCoroutine(InterpolateCamera());
            yield return null;
        }

        /// <summary>
        /// Moves the camera slowly towards the CameraEndPoint
        /// </summary>
        /// <returns></returns>
        IEnumerator InterpolateCamera()
        {
            float timeElapsed = 0;
            Vector3 valueToLerp = Vector3.zero;

            while(Vector3.Distance(SecondCamera.transform.position, CameraEndPoint.position) > 2)
            {
                valueToLerp = Vector3.Lerp(SecondCamera.transform.position, CameraEndPoint.position, Time.deltaTime);
                timeElapsed += Time.deltaTime;
                SecondCamera.transform.position = valueToLerp;

                yield return null;
            }
            valueToLerp = CameraEndPoint.position;
            StartCoroutine(EndCredits());
        }

        /// <summary>
        /// Shows the short end credits and pulls up the Credits scene
        /// </summary>
        /// <returns></returns>
        IEnumerator EndCredits()
        {
            int counter = 0;
            yield return new WaitForSeconds(TimeBetweenCredits);
            DisciplineText.enabled = true;
            PersonText.enabled = true;
            while (counter < Disciplines.Length)
            {
                DisciplineText.text = Disciplines[counter];
                PersonText.text = Artists[counter];
                yield return new WaitForSeconds(TimeBetweenCredits);
                counter++;
            }
            SceneLoader.Credits();
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
