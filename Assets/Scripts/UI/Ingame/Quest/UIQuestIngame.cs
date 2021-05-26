using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class UIQuestIngame : MonoBehaviour
    {
        private List<UIQuestIngameText> questIngameTexts = new List<UIQuestIngameText>();

        /// <summary>
        /// 清除缩略表
        /// </summary>
        public void Clear()
        {
            // 删除现有texts
            foreach (UIQuestIngameText questIngameText in questIngameTexts)
            {
                Destroy(questIngameText.gameObject);
            }
            questIngameTexts = new List<UIQuestIngameText>();
        }

        /// <summary>
        /// 刷新缩略表
        /// </summary>
        public void Refresh()
        {
            Clear();

            foreach (Quest quest in QuestManager.instance.OngoingQuests)
            {
                if (quest.tracking)
                {
                    UIQuestIngameText questIngameText =
                        Instantiate(ResourceLoader.Load("Prefabs/UI/UIQuestIngameTextPrefab") as GameObject, transform).GetComponent<UIQuestIngameText>();
                    questIngameTexts.Add(questIngameText);
                    questIngameText.Initialize(quest);
                    questIngameText._rectTransform.anchoredPosition = Vector2.down * 60 * (questIngameTexts.Count - 1);
                }
            }
        }
    }
}
