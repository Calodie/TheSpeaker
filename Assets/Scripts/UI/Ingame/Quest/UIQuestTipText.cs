using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIQuestTipText : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform _rectTransform;

        public Text textTip;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(Quest quest)
        {
            switch (quest.stage)
            {
                case QuestStage.ongoing:
                    textTip.text = $"新任务：\"{quest.questName}\"";
                    break;
                case QuestStage.finished:
                    textTip.text = $"任务 \"{quest.questName}\" 已完成！";
                    break;
                case QuestStage.failed:
                    textTip.text = $"任务 \"{quest.questName}\" 已失败！";
                    break;
            }

            textTip.color = QuestManager.instance.GetStageColor(quest.stage);
        }
    }
}

