using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Enemy
{
    public class Activator : MonoBehaviour
    {
        public GameObject Enemy;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Enemy.SetActive(true);
            }
        }
    }
}
