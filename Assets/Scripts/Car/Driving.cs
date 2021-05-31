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
        public bool canGetOut;
        public GameObject nextObjectiveToExitCar;

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
            if(nextObjectiveToExitCar.GetComponent<BoxCollider>().enabled)
            {
                canGetOut = true;
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
            if(canGetOut)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Y) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    DeactivateDriving();
                }
            }
        }

        private void DeactivateDriving()
        {
            playerRef.transform.position = new Vector3(transform.position.x - 4, 0, transform.position.z);
            playerRef.SetActive(true);
            mainCamera.SetActive(true);
            cinemachineCamera.SetActive(true);
            carCamera.SetActive(false);
            carHintCanvas.enabled = false;
            GetComponent<CarController>().enabled = false;
            engineStartSound.Stop();
            isDriving = false;
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