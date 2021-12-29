using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Audio
{
    public class Track : MonoBehaviour
    {
        public string Name;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume;

        [Range(0.1f, 3f)]
        public float Pitch;

        public bool Loop;

        [HideInInspector]
        public AudioSource Source;
    }
}
