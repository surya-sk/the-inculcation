using CultGame.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Audio
{
    public class DialogTrigger : MonoBehaviour, ISaveable
    {
        public AudioSource DialogAudioSource;
        public AudioClip DialogClip;

        bool m_Triggered;

        // Start is called before the first frame update
        void Start()
        {
            m_Triggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(PlayDialog());
            }
        }

        /// <summary>
        /// A coroutine that plays the audioclip once
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayDialog()
        {
            m_Triggered = true;
            DialogAudioSource.PlayOneShot(DialogClip);
            GetComponent<BoxCollider>().enabled = !m_Triggered;
            yield return null;
        }

        public object CaptureState()
        {
            return m_Triggered;
        }

        public void RestoreState(object state)
        {
            m_Triggered = (bool)state;
            GetComponent<BoxCollider>().enabled = !m_Triggered;
        }
    }
}
