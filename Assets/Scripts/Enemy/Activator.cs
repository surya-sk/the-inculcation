using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Enemy
{
    /// <summary>
    /// Activates enemy when a collider is triggered
    /// </summary>
    public class Activator : MonoBehaviour
    {
        public GameObject Enemy;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Enemy.GetComponent<AI>().enabled = true;
            }
        }
    }
}
