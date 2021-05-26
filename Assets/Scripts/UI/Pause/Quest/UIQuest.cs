using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public enum QuestStage
    {
        ongoing, finished, failed
    }

    public class UIQuest : MonoBehaviour
    {
        public Text textTitle;

        public UIQuestView questView;

        private QuestStage page = QuestStage.ongoing;

        public void SetPage(int page)
        {
            this.page = (QuestStage)page;
        }

        public void ShowQuests()
        {
            switch(page)
            {
                case QuestStage.ongoing:
                    textTitle.text = "当前任务";
                    questView.ShowQuest(QuestManager.instance.OngoingQuests);
                    break;
                case QuestStage.finished:
                    textTitle.text = "已完成的任务";
                    questView.ShowQuest(QuestManager.instance.FinishedQuests);
                    break;
                case QuestStage.failed:
                    textTitle.text = "已失败的任务";
                    questView.ShowQuest(QuestManager.instance.FailedQuests);
                    break;
            }
        }
    }
}
