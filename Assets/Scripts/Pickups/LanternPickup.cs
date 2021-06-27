using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Player;

 namespace CultGame.Pickups
{
    public class LanternPickup : MonoBehaviour
    {
        public Canvas lanternCanvas;
        public ThirdPersonCharacterController playerRef;
        void Start()
        {
            lanternCanvas.enabled = false;
        }

        private void Update()
        {
            if (lanternCanvas.enabled)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.X))
                {
                    playerRef.ActivateLantern();
                    lanternCanvas.enabled = false;
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                lanternCanvas.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                lanternCanvas.enabled = false;
            }
        }
    }

}
