using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Utils
{
    public class TutorialObject : MonoBehaviour
    {
        public Canvas tutorialCanvas;

        private void Start()
        {
            tutorialCanvas.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                tutorialCanvas.enabled = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                tutorialCanvas.enabled = false;
            }
        }
    }
}
