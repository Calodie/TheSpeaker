using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [HideInInspector]
        public AudioSource _audioSource;

        public float maxVolume;

        public float fadeOutDuration;

        private float fadeTick;

        private AudioClip nextClip;

        private bool needSwitch = false;

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }

            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            FadeOut();
        }

        private void FadeOut()
        {
            fadeTick = Mathf.Max(fadeTick - Time.deltaTime, 0);
            if (fadeTick > 0)
            {
                _audioSource.volume = fadeTick / fadeOutDuration * maxVolume;
            }
            else if (fadeTick <= 0 && needSwitch)
            {
                needSwitch = false;

                _audioSource.Stop();
                _audioSource.clip = nextClip;
                _audioSource.Play();
                _audioSource.volume = maxVolume;

                nextClip = null;
            }
        }

        /// <summary>
        /// 播放bgm
        /// </summary>
        public void SetBGM(AudioClip clip)
        {
            if (_audioSource.clip == clip)
            {
                return;
            }

            needSwitch = true;

            nextClip = clip;
            fadeTick = fadeOutDuration;
        }

        public void StopBGM()
        {
            fadeTick = 0;
            _audioSource.Stop();
            _audioSource.clip = null;
            nextClip = null;
        }
    }
}

