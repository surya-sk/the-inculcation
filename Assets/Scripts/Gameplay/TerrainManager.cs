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


        public void SetActiveTerrain(int index)
        {
            currentTerrainIndex = index;
        }
        
        public int GetActiveTerrain()
        {
            return currentTerrainIndex;
        }

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
