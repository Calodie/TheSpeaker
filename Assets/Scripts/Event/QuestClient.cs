using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class QuestClient : MonoBehaviour
    {
        public void AddQuest(Quest quest)
        {
            QuestManager.instance.AddQuest(quest);
        }
        public void FinishQuest(Quest quest)
        {
            QuestManager.instance.FinishQuest(quest);
        }
        public void FailQuest(Quest quest)
        {
            QuestManager.instance.FailQuest(quest);
        }
    }
}
