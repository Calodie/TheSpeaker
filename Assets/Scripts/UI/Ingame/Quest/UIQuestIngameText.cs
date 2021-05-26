using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIQuestIngameText : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform _rectTransform;

        public Image imageMark;

        public Text textQuest;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(Quest quest)
        {
            textQuest.text = quest.questName;

            if (quest.questType == QuestTypes.toFinishAll && QuestManager.instance.GoalDictionary.ContainsKey(quest))
            {
                textQuest.text += $" ({QuestManager.instance.GoalDictionary[quest]}/{quest.goalCount})";
            }

            imageMark.color = QuestManager.instance.GetQuestColor(quest.questType);
            imageMark.gameObject.SetActive(quest.tracking);
        }
    }
}
