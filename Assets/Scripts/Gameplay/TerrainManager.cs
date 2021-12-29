using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Gameplay
{
    public class TerrainManager : MonoBehaviour, ISaveable
    {
        public GameObject[] terrains;
        private int currentTerrainIndex;

        public TerrainManager() { }

        /// <summary>
        /// Keeps track of current active terrain
        /// </summary>
        /// <param name="index"></param>
        public void SetActiveTerrain(int index)
        {
            currentTerrainIndex = index;
        }
        
        /// <summary>
        /// Gets the terrain that's currently active
        /// </summary>
        /// <returns>Index of the terrain that's currently active</returns>
        public int GetActiveTerrain()
        {
            return currentTerrainIndex;
        }

        /// <summary>
        /// Deactivates all terrains excepts the active one, the one before and after it
        /// </summary>
        private void DeactivateOtherTerrains()
        {
            for (int i = 0; i < terrains.Length; i++)
            {
                if (i != currentTerrainIndex && i!=currentTerrainIndex-1)
                {
                    terrains[i].SetActive(false);
                }
            }
        }

        public object CaptureState()
        {
            print("Writing current index " + currentTerrainIndex);
            return currentTerrainIndex;
        }

        public void RestoreState(object state)
        {
            currentTerrainIndex = (int)state;
            terrains[currentTerrainIndex].SetActive(true);
            DeactivateOtherTerrains();
        }
    }
}
