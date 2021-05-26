using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class AudioDictionary : MonoBehaviour
    {
        public static AudioDictionary instance;

        [SerializeField]
        private AudioClip[] clips;
        [SerializeField]
        private AudioClip unknownClip;

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

            clips = Resources.LoadAll<AudioClip>("Audios");
        }

        public AudioClip FindAudioClipWithName(string audioName)
        {
            foreach (AudioClip clip in clips)
            {
                if (clip.name == audioName)
                {
                    return clip;
                }
            }
            return unknownClip;
        }

    }
}

