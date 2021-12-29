using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public Track[] Tracks;

        public static MusicManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            foreach(Track track in Tracks)
            {
                track.Source = gameObject.AddComponent<AudioSource>();
                track.Source.clip = track.Clip;
                track.Source.loop = track.Loop;
                track.Source.volume = track.Volume;
                track.Source.pitch = track.Pitch;
            }
        }

        public void Play(string name)
        {
            StopAll();
            Track track = Array.Find(Tracks, t => t.Name == name);
            if(track == null)
            {
                Debug.LogError($"Track named {name} not found");
                return;
            }
            track.Source.Play();
        }

        public void StopAll()
        {
            foreach (Track track in Tracks)
            {
                if (track.Source.isPlaying)
                {
                    track.Source.Stop();
                }
            }
        }
    }
}
