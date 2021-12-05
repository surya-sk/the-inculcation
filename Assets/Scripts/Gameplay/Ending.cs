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

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(PromptJump());
        }

        IEnumerator PromptJump()
        {
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Initiate jump
        }
    }
}
