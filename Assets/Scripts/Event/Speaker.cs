using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class Speaker : MonoBehaviour
    {
        public static List<Speaker> speakers = new List<Speaker>();

        public static Speaker FindSpeakerWithObjectName(string name)
        {
            foreach(Speaker speaker in speakers)
            {
                if(speaker.gameObject.name == name)
                {
                    return speaker;
                }
            }
            return null;
        }

        /// <summary>
        /// 当前对话
        /// </summary>
        [SerializeField]
        private Dialog currentDialog;

        private void Awake()
        {
            speakers.Add(this);
        }

        /// <summary>
        /// 设置当前对话
        /// </summary>
        /// <param name="dialog"></param>
        public void SetDialog(Dialog dialog)
        {
            currentDialog = dialog;
        }

        public void Speak()
        {
            currentDialog.speaker = gameObject.name;
            UIDialog.instance.gameObject.SetActive(true);
            UIDialog.instance.SetDialog(currentDialog);
            UIDialog.instance.StartDialog();
        }

        private void OnDestroy()
        {
            speakers.Remove(this);
        }
    }
}
