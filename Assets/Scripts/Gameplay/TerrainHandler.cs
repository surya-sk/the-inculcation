using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Gameplay
{
    /// <summary>
    /// Handles terrains activation and deactivation
    /// </summary>
    public class TerrainHandler : MonoBehaviour, ISaveable
    {
        public GameObject terrain;
        public int index;
        public TerrainManager terrainManager;
        bool activated = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                activated = !terrain.activeSelf;
                if(activated)
                {
                    terrainManager.SetActiveTerrain(index);
                }
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
