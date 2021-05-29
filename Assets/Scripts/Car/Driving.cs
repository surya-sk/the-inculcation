using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Car
{
    public class Driving : MonoBehaviour
    {
        public Canvas carHintCanvas;
        public GameObject playerRef;
        public GameObject mainCamera;
        public GameObject cinemachineCamera;
        public GameObject carCamera;
        public bool isDriving;

        public AudioSource engineStartSound;

        private void Start()
        {
            carHintCanvas.enabled = false;
            carCamera.SetActive(false);
        }

        private void Update()
        {
            if (carHintCanvas.enabled)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Y) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    isDriving = true;
                }
            }
            if(isDriving)
            {
                ActivateDriving();
            }
        }

        private void ActivateDriving()
        {
            playerRef.SetActive(false);
            mainCamera.SetActive(false);
            cinemachineCamera.SetActive(false);
            carCamera.SetActive(true);
            carHintCanvas.enabled = false;
            GetComponent<CarController>().enabled = true;
            engineStartSound.Play();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                carHintCanvas.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                carHintCanvas.enabled = false;
            }
        }
    }
}