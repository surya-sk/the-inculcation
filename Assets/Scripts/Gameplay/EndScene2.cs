using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CultGame.Gameplay
{
    public class EndScene2 : MonoBehaviour
    {
        public AudioSource hitSound;
      
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
                // TODO: Switch to first person here
                yield return new WaitForSeconds(3f);
                hitSound.Play();
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(5);
            }
        }
    }
}
