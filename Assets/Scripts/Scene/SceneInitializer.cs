using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class SceneInitializer : MonoBehaviour
    {
        public Quest[] testQuest;

        private void Start()
        {
            foreach(Quest quest in testQuest)
            {
                QuestManager.instance.AddQuest(quest);
            }
            Speaker speaker = GetComponent<Speaker>();
            if(speaker)
            {
                speaker.Speak();
            }
        }
    }
}
