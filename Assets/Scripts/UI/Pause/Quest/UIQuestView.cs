using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class QuestNode
    {
        public Quest quest;

        public int level;
        public QuestNode prevNode;
        public List<QuestNode> succNodes;

        public QuestNode()
        {
            level = 0;
            prevNode = null;
            succNodes = new List<QuestNode>();
        }

        /// <summary>
        /// 构建任务树
        /// </summary>
        /// <param name="existingNodes"></param>
        public void Build(List<QuestNode> existingNodes)
        {
            // 对于每一个succQuest
            foreach(Quest typeSucc in quest.succs)
            {
                // 检查这个succQuest的所有实例是否已存在于树中
                List<Quest> questSuccs = QuestManager.instance.FindQuestsOfType(typeSucc);
                foreach (Quest questSucc in questSuccs)
                {
                    QuestNode builtNode = null;

                    foreach (QuestNode existingNode in existingNodes)
                    {
                        if (existingNode.quest == questSucc)
                        {
                            builtNode = existingNode;
                            break;
                        }
                    }

                    // 不存在，创建新节点
                    if (builtNode == null)
                    {
                        QuestNode succNode = new QuestNode
                        {
                            quest = questSucc,
                            level = level + 1,
                            prevNode = this,
                        };
                        succNodes.Add(succNode);

                        // 记录这个新节点
                        existingNodes.Add(succNode);

                        // 向下继续构造
                        succNode.Build(existingNodes);
                    }
                    // 存在，连接至旧节点
                    else
                    {
                        succNodes.Add(builtNode);
                    }
                }
            }
        }

        /// <summary>
        /// 生成树
        /// </summary>
        public bool SpawnTree(List<Quest> quests, List<QuestNode> viewedNodes)
        {
            // 不走回头路
            if(viewedNodes.Contains(this))
            {
                return false;
            }
            viewedNodes.Add(this);

            QuestNode[] nodeSuccArray = succNodes.ToArray();
            for (int i = 0; i < nodeSuccArray.Length; i++)
            {
                if (!nodeSuccArray[i].SpawnTree(quests, viewedNodes))
                {
                    succNodes.Remove(nodeSuccArray[i]);
                }
            }

            // 是要保留的quest或者（quest已完成且仍有子节点存在），返回true
            return quests.Contains(quest) || succNodes.Count > 0;
        }

        /// <summary>
        /// 遍历，按层级返回所有quest
        /// </summary>
        /// <param name="levels"></param>
        public void Cover(ref List<List<Quest>> levels)
        {
            // 对于每一个nodeSucc
            foreach (QuestNode nodeSucc in succNodes)
            {
                // 创建层级
                if (levels.Count <= nodeSucc.level)
                {
                    levels.Add(new List<Quest>());
                }

                // 检查是否已存在于层级中
                if (!levels[nodeSucc.level].Contains(nodeSucc.quest))
                {
                    levels[nodeSucc.level].Add(nodeSucc.quest);

                    // 向下继续遍历
                    nodeSucc.Cover(ref levels);
                }
            }
        }
    }

    public class UIQuestView : MonoBehaviour
    {
        public RectTransform viewport;

        private List<UIQuestButton> questButtons = new List<UIQuestButton>();

        public void ShowQuest(List<Quest> quests)
        {
            foreach(UIQuestButton questButton in questButtons)
            {
                Destroy(questButton.gameObject);
            }
            questButtons = new List<UIQuestButton>();

            // 从每个顶端任务开始
            foreach (Quest topQuest in QuestManager.instance.TopQuests)
            {
                // 创建顶端任务树
                QuestNode tree = new QuestNode
                {
                    quest = topQuest,
                    level = 0,
                    prevNode = null,
                };
                tree.Build(new List<QuestNode>() { tree });

                // 生成树
                if(tree.SpawnTree(quests, new List<QuestNode>()))
                {
                    // 遍历
                    List<List<Quest>> levels = new List<List<Quest>>
                    {
                        new List<Quest>()
                    };
                    levels[0].Add(tree.quest);
                    tree.Cover(ref levels);

                    // 生成任务词条
                    foreach(List<Quest> level in levels)
                    {
                        foreach(Quest quest in level)
                        {
                            UIQuestButton questButton = 
                                Instantiate(ResourceLoader.Load("Prefabs/UI/UIQuestButtonPrefab") as GameObject, viewport).GetComponent<UIQuestButton>();
                            questButton.quest = quest;
                            questButtons.Add(questButton);
                            questButton._rectTransform.anchoredPosition = Vector2.down * 100 * (questButtons.Count - 1);
                        }
                    }

                    // 调整视窗大小
                    Vector2 viewSizeDelta = viewport.sizeDelta;
                    viewSizeDelta.y = (questButtons.Count + 1) * 100;
                    viewport.sizeDelta = viewSizeDelta;
                }
            }
        }

    }
}
