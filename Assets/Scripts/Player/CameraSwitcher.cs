using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Player
{
    public class CameraSwitcher : MonoBehaviour, ISaveable
    {
        public GameObject ThirdPersonCamera;
        public GameObject FirstPersonCamera;
        public GameObject Player;
        public int CameraMode;
        public bool CanChange = true;

        bool m_changed = false;

        private void Start()
        {
            StartCoroutine(ChangeCamera());
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.X) && CanChange)
            {
                CameraMode = CameraMode == 1 ? 0 : 1;
                m_changed = true;
            }
            if(m_changed)
                StartCoroutine(ChangeCamera());
        }

        public void UpdateCamera(int cameraMode)
        {
            CameraMode = cameraMode;
            m_changed = true;
        }

        IEnumerator ChangeCamera()
        {
            yield return new WaitForSeconds(0.01f);
            if(CameraMode == 0)
            {
                ThirdPersonCamera.SetActive(true);
                Player.GetComponent<ThirdPersonCharacterController>().enabled = true;
                FirstPersonCamera.SetActive(false);
                Player.GetComponent<FirstPersonCharacterController>().enabled = false;
            }
            else
            {
                ThirdPersonCamera.SetActive(false);
                Player.GetComponent<ThirdPersonCharacterController>().enabled = false;
                FirstPersonCamera.SetActive(true);
                Player.GetComponent<FirstPersonCharacterController>().enabled = true;
            }
            m_changed=false;
        }

        public object CaptureState()
        {
            return CameraMode;
        }

        public void RestoreState(object state)
        {
            CameraMode = (int)state;
        }
    }

}