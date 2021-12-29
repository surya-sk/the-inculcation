using CultGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CultGame.Gameplay
{
    public class EndScene2 : MonoBehaviour
    {
        public AudioSource hitSound;
        public CameraSwitcher CameraSwitcher;
        public Canvas BlankCanvas;
      
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(SimulateHit());
            }
        }

        /// <summary>
        /// Simuates the player getting hit
        /// </summary>
        /// <returns></returns>
        IEnumerator SimulateHit()
        {
            while (gameObject.activeSelf)
            {
                CameraSwitcher.UpdateCamera(1);
                yield return new WaitForSeconds(3f);
                hitSound.Play();
                BlankCanvas.enabled = true;
                yield return new WaitForSeconds(5f);
                SceneManager.LoadScene(5);
            }
        }
    }
}
