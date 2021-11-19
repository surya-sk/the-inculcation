using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Gameplay
{
    public class ChaseTrigger : MonoBehaviour
    {
        public bool PlayerHasCrossed { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerHasCrossed = true;
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }

    }
}
