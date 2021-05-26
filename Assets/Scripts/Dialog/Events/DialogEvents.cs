using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(Dialog))]
    public class DialogEvents : MonoBehaviour
    {
        [HideInInspector]
        public Dialog _dialog;

        private Speaker speaker;

        internal virtual void Awake()
        {
            _dialog = GetComponent<Dialog>();

            speaker = Speaker.FindSpeakerWithObjectName(_dialog.speaker);
        }

        internal virtual void Start()
        {

        }

        internal virtual void Update()
        {

        }
    }
}
