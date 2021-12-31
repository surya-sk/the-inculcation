using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Audio
{
    [System.Serializable]
    public class Track
    {
        public string Name;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume;

        [Range(0.1f, 3f)]
        public float Pitch = 1f;

        public bool Loop;

        [HideInInspector]
        public AudioSource Source;
    }
}
