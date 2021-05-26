using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class UIQuestTip : MonoBehaviour
    {
        /// <summary>
        /// 任务提示显示时长
        /// </summary>
        [Header("任务提示显示时长")]
        public float showDuration;

        /// <summary>
        /// 任务提示新增间隔
        /// </summary>
        [Header("任务提示新增间隔")]
        public float showDelta;

        private List<UIQuestTipText> showingTips = new List<UIQuestTipText>();

        private Queue<Quest> toShowQuests = new Queue<Quest>();

        private float deltaTick = 0;

        private float durationTick = 0;

        public void CreateTip(Quest quest)
        {
            toShowQuests.Enqueue(quest);
        }

        /// <summary>
        /// 清除任务提示
        /// </summary>
        public void Clear()
        {
            toShowQuests.Clear();
            durationTick = 0;
        }

        private void Update()
        {
            Tick();
            ShowQuest();
            HideTips();
        }

        private void Tick()
        {
            deltaTick = Mathf.Max(0, deltaTick - Time.deltaTime);
            durationTick = Mathf.Max(0, durationTick - Time.deltaTime);
        }

        private void ShowQuest()
        {
            if (toShowQuests.Count > 0 && deltaTick <= 0)
            {
                deltaTick = showDelta;

                durationTick = showDuration;

                Quest quest = toShowQuests.Dequeue();

                UIQuestTipText questTip =
                    Instantiate(ResourceLoader.Load("Prefabs/UI/UIQuestTipTextPrefab") as GameObject, transform).GetComponent<UIQuestTipText>();
                questTip.Initialize(quest);
                showingTips.Add(questTip);
                questTip._rectTransform.anchoredPosition = Vector2.down * 80 * (showingTips.Count - 1);
            }
        }

        private void HideTips()
        {
            if (showingTips.Count > 0 && durationTick <= 0)
            {
                foreach (UIQuestTipText showingTip in showingTips)
                {
                    Destroy(showingTip.gameObject);
                }

                showingTips = new List<UIQuestTipText>();
            }
        }
    }
}