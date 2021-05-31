using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Utils
{
    public class ChangeScene : MonoBehaviour
    {
        public int sceneNumber;
        public SceneLoader sceneLoader;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                sceneLoader.LoadScene(sceneNumber);
            }
        }
    }
}
