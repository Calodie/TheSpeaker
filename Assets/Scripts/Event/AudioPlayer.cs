using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class AudioPlayer : MonoBehaviour
    {
        public string bgmName;

        public void Play()
        {
            AudioManager.instance.SetBGM(AudioDictionary.instance.FindAudioClipWithName(bgmName));
        }
        /*
        public void Stop()
        {
            AudioManager.instance.StopBGM();
        }*/
    }
}

