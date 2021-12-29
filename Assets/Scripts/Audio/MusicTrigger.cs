using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Audio
{
    public class MusicTrigger : MonoBehaviour
    {
        public string TrackName;
        public MusicManager MusicManager;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                MusicManager.Play(TrackName);
            }
        }
    }
}
