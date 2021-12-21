using UnityEngine;
using CultGame.Saving;

namespace CultGame.Car
{
    public class Driving : MonoBehaviour, ISaveable
    {
        public Canvas carHintCanvas;
        public GameObject playerRef;
        public GameObject mainCamera;
        public GameObject cinemachineCamera;
        public GameObject carCamera;
        public bool isDriving;
        public bool canGetOut;
        public GameObject nextObjectiveToExitCar;
        bool hasStoppedDriving = false;

        public AudioSource engineStartSound;

        private void Start()
        {
            carHintCanvas.enabled = false;
            carCamera.SetActive(false);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Y) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                isDriving = !isDriving;
            }
            if(isDriving)
            {
                ActivateDriving();
            }
            else
            {
                if(!hasStoppedDriving)
                    DeactivateDriving();
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

        private void DeactivateDriving()
        {
            playerRef.transform.position = new Vector3(transform.position.x - 4, transform.position.y, transform.position.z);
            playerRef.SetActive(true);
            mainCamera.SetActive(true);
            cinemachineCamera.SetActive(true);
            carCamera.SetActive(false);
            carHintCanvas.enabled = false;
            GetComponent<CarController>().enabled = false;
            engineStartSound.Stop();
            isDriving = false;
            hasStoppedDriving = true;
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

        public object CaptureState()
        {
            return isDriving;
        }

        public void RestoreState(object state)
        {
            isDriving = (bool)state;
        }
    }
}