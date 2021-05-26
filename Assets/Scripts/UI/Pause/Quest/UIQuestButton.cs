using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIQuestButton : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform _rectTransform;

        [HideInInspector]
        public Quest quest;

        public Image imageMark;

        public Text textTitle;

        public Text textContent;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            textTitle.text = quest.questName;
            textContent.text = quest.content;
            textTitle.color = QuestManager.instance. GetStageColor(quest.stage);
            textContent.color = textTitle.color;

            if (quest.questType == QuestTypes.toFinishAll && QuestManager.instance.GoalDictionary.ContainsKey(quest))
            {
                textTitle.text += $" ({QuestManager.instance.GoalDictionary[quest]}/{quest.goalCount})";
            }

            if (quest.prevs.Length > 0)
            {
                textTitle.text = "    " + textTitle.text;
                textContent.text = "    " + textContent.text;
            }

            imageMark.gameObject.SetActive(quest.tracking);
            imageMark.color = QuestManager.instance.GetQuestColor(quest.questType);
        }

        /// <summary>
        /// 追踪/取消追踪任务
        /// </summary>
        public void SetTracking()
        {
            if(quest.stage == QuestStage.ongoing)
            {
                quest.tracking = !quest.tracking;
                imageMark.gameObject.SetActive(quest.tracking);
                UIIngame.instance.questInfo.Refresh();
            }
        }
    }
}
