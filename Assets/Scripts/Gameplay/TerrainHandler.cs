using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Gameplay
{
    public class TerrainHandler : MonoBehaviour, ISaveable
    {
        public GameObject terrain;
        bool activated = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                activated = !terrain.activeSelf;
                StartCoroutine(DecideTerrainStatus());
            }
        }

        IEnumerator DecideTerrainStatus()
        {
            yield return new WaitForEndOfFrame();
            terrain.SetActive(activated);
        }

        public object CaptureState()
        {
            return activated;
        }

        public void RestoreState(object state)
        {
            activated = (bool)state;
            StartCoroutine(DecideTerrainStatus());
        }
    }
}
