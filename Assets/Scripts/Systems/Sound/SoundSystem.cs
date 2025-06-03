using System;
using Base;
using UnityEngine;

namespace Systems.Sound
{
    public class SoundSystem : MonoBehaviourSingleton<SoundSystem>
    {
        public AudioClip alienDied;
        public AudioClip bulletPew;
        public AudioClip planetHit;
        public AudioClip alienSound;
        public AudioClip alienBoss;
        public AudioSource AudioSource;

        public void Start()
        {
            if (!AudioSource) AudioSource = Camera.main.gameObject.AddComponent<AudioSource>();
        }

        public void PlayTheOneWhereAnAlienDied()
        {
            AudioSource.clip = alienDied;
            AudioSource.Play();
        }

        public void PlayThisSound(AudioClip audioClipToPlay)
        {
            AudioSource.clip = audioClipToPlay;
            AudioSource.Play();
            // Debug.LogWarning($"I was going to play {audioClipToPlay.name} but I need your help!");
        }
    }
}