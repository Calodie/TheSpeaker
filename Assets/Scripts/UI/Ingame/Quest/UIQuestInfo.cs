using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    /// <summary>
    /// 任务缩略
    /// </summary>
    public class UIQuestInfo : MonoBehaviour
    {
        /// <summary>
        /// 任务缩略显示表
        /// </summary>
        [SerializeField]
        private UIQuestIngame questIngame;
        /// <summary>
        /// 任务切换提示器
        /// </summary>
        [SerializeField]
        private UIQuestTip questTip;

        /// <summary>
        /// 刷新任务缩略
        /// </summary>
        public void Refresh()
        {
            questIngame.Refresh();
        }

        public void AddQuest(Quest quest)
        {
            Refresh();
            questTip.CreateTip(quest);
        }

        public void FinishQuest(Quest quest)
        {
            Refresh();
            questTip.CreateTip(quest);
        }

        public void FailQuest(Quest quest)
        {
            Refresh();
            questTip.CreateTip(quest);
        }

        /// <summary>
        /// 清除任务缩略
        /// </summary>
        public void Clear()
        {
            questIngame.Clear();
            questTip.Clear();
        }
    }
}

