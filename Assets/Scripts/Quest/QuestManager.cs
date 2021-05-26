using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [System.Serializable]
    public struct QuestStageColor
    {
        public QuestStage questStage;
        public Color color;
    }

    [System.Serializable]
    public struct QuestColor
    {
        public QuestTypes questType;
        public Color color;
    }

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        public QuestColor[] questColors;

        public QuestStageColor[] stageColors;

        /// <summary>
        /// 当前任务
        /// </summary>
        public List<Quest> OngoingQuests { get; private set; } = new List<Quest>();
        /// <summary>
        /// 已完成的任务
        /// </summary>
        public List<Quest> FinishedQuests { get; private set; } = new List<Quest>();
        /// <summary>
        /// 已失败的任务
        /// </summary>
        public List<Quest> FailedQuests { get; private set; } = new List<Quest>();

        /// <summary>
        /// 顶端任务
        /// </summary>
        public List<Quest> TopQuests { get; private set; } = new List<Quest>();

        /// <summary>
        /// 任务目标计数器对应表
        /// </summary>
        public Dictionary<Quest, int> GoalDictionary { get; private set; } = new Dictionary<Quest, int>();

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        public Color GetQuestColor(QuestTypes questType)
        {
            foreach (QuestColor questColor in questColors)
            {
                if (questColor.questType == questType)
                {
                    return questColor.color;
                }
            }

            return Color.white;
        }

        public Color GetStageColor(QuestStage questStage)
        {
            foreach (QuestStageColor stageColor in stageColors)
            {
                if (stageColor.questStage == questStage)
                {
                    return stageColor.color;
                }
            }

            return Color.white;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            OngoingQuests = new List<Quest>();
            FinishedQuests = new List<Quest>();
            FailedQuests = new List<Quest>();
            TopQuests = new List<Quest>();
            GoalDictionary = new Dictionary<Quest, int>();

            foreach(Quest quest in GetComponentsInChildren<Quest>())
            {
                Destroy(quest.gameObject);
            }
        }

        /// <summary>
        /// 添加新任务，成功添加返回true
        /// </summary>
        /// <param name="typeToAdd"></param>
        /// <returns></returns>
        public bool AddQuest(Quest typeToAdd)
        {
            // 该任务是现有任务，不添加
            if (FindFirstQuestOfType(typeToAdd, QuestStage.ongoing))
            {
                return false;
            }

            // 该任务不可重复完成且（是现有任务、已完成或已失败），不添加
            if (!typeToAdd.reusable && FindFirstQuestOfType(typeToAdd))
            {
                return false;
            }

            // 该任务有前置任务尚未完成，不添加
            foreach (QuestLogic questLogic in typeToAdd.prevs)
            {
                if (!questLogic.Conduct())
                {
                    return false;
                }
            }

            // 该任务与现有任务冲突，不添加
            foreach (Quest quest in OngoingQuests)
            {
                if (quest.contraQuests.Contains(quest))
                {
                    return false;
                }
            }

            // 创建实例
            Quest toAdd = Instantiate(typeToAdd, transform);

            // 添加任务
            OngoingQuests.Add(toAdd);

            // 触发事件
            toAdd.onAdd.Invoke();

            // 任务状态
            toAdd.stage = QuestStage.ongoing;

            // 追踪任务
            toAdd.tracking = true;

            // 更新任务缩略
            UIIngame.instance.questInfo.AddQuest(toAdd);

            // 任务目标计数器
            if (toAdd.questType == QuestTypes.toFinishAll)
            {
                GoalDictionary.Add(toAdd, 0);
            }

            // 顶端任务
            if (toAdd.prevs.Length <= 0)
            {
                TopQuests.Add(toAdd);
            }

            // 添加并列选择性任务
            if (toAdd.selectaQuests.Count > 0)
            {
                foreach (Quest typeToAddSelecta in toAdd.selectaQuests)
                {
                    AddQuest(typeToAddSelecta);
                }
            }

            return true;
        }

        /// <summary>
        /// 完成一个任务，成功完成返回true
        /// </summary>
        /// <param name="typeToFinish"></param>
        /// <returns></returns>
        public bool FinishQuest(Quest typeToFinish)
        {
            // 从现有任务中找到第一个对应的实例
            Quest toFinish = FindFirstQuestOfType(typeToFinish, QuestStage.ongoing);

            // 非现有任务
            if(!toFinish)
            {
                return false;
            }

            // 该任务为目标数类型
            if(toFinish.questType== QuestTypes.toFinishAll)
            {
                GoalDictionary[toFinish]++;

                if(GoalDictionary[toFinish] < toFinish.goalCount)
                {
                    return false;
                }
            }

            // 与该任务并列的选择性任务全部失败
            foreach (Quest quest in toFinish.selectaQuests)
            {
                FailQuest(quest);
            }

            // 移除任务
            OngoingQuests.Remove(toFinish);

            // 触发事件
            toFinish.onFinish.Invoke();

            // 取消追踪任务
            toFinish.tracking = false; 

            // 移除任务目标计数器
            if(GoalDictionary.ContainsKey(toFinish))
            {
                GoalDictionary.Remove(toFinish);
            }

            // 添加到已完成任务
            FinishedQuests.Add(toFinish);

            // 任务状态
            toFinish.stage = QuestStage.finished;

            // 更新任务缩略
            UIIngame.instance.questInfo.FinishQuest(toFinish);

            // 添加该任务的后置任务
            if (toFinish.succs.Length > 0)
            {
                foreach (Quest typeToAdd in toFinish.succs)
                {
                    AddQuest(typeToAdd);
                }
            }

            return true;
        }

        /// <summary>
        /// 任务失败，成功导致了这个任务的失败返回true
        /// </summary>
        /// <param name="typeToFail"></param>
        public bool FailQuest(Quest typeToFail)
        {
            // 从现有任务中找到第一个对应的实例
            Quest toFail = FindFirstQuestOfType(typeToFail, QuestStage.ongoing);

            // 非现有任务
            if (!toFail)
            {
                return false;
            }

            // 移除任务
            OngoingQuests.Remove(toFail);

            // 触发事件
            toFail.onFail.Invoke();

            // 取消追踪任务
            toFail.tracking = false;

            // 移除任务目标计数器
            if (GoalDictionary.ContainsKey(toFail))
            {
                GoalDictionary.Remove(toFail);
            }

            // 添加到已失败任务
            FailedQuests.Add(toFail);

            // 任务状态
            toFail.stage = QuestStage.failed;

            // 更新任务缩略
            UIIngame.instance.questInfo.FailQuest(toFail);

            return true;
        }

        /// <summary>
        /// 从quests中找到第一个typeToCheck类型的任务
        /// </summary>
        /// <param name="quests"></param>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public Quest FindFirstQuestOfType(Quest typeToCheck, QuestStage stage)
        {
            Quest[] questArray;
            switch(stage)
            {
                case QuestStage.finished:
                    questArray = FinishedQuests.ToArray();
                    break;
                case QuestStage.failed:
                    questArray = FailedQuests.ToArray();
                    break;
                default:
                    questArray = OngoingQuests.ToArray();
                    break;
            }
            for (int i = 0; i < questArray.Length; i++)
            {
                if (questArray[i].name.Split('(')[0] == typeToCheck.name.Split('(')[0])
                {
                    return questArray[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 从所有任务（当前、已完成、已失败）中找到第一个typeToCheck类型的任务
        /// </summary>
        /// <param name="quests"></param>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public Quest FindFirstQuestOfType(Quest typeToCheck)
        {
            Quest quest = FindFirstQuestOfType(typeToCheck,QuestStage.ongoing);
            if (quest)
            {
                return quest;
            }
            quest = FindFirstQuestOfType(typeToCheck, QuestStage.finished);
            if (quest)
            {
                return quest;
            }
            quest = FindFirstQuestOfType(typeToCheck, QuestStage.failed);
            if (quest)
            {
                return quest;
            }
            return null;
        }

        /// <summary>
        /// 从quests中找到所有typeToCheck类型的任务
        /// </summary>
        /// <param name="quests"></param>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public List<Quest> FindQuestsOfType(Quest typeToCheck, QuestStage stage)
        {
            Quest[] questArray;
            List<Quest> quests = new List<Quest>();
            switch (stage)
            {
                case QuestStage.finished:
                    questArray = FinishedQuests.ToArray();
                    break;
                case QuestStage.failed:
                    questArray = FailedQuests.ToArray();
                    break;
                default:
                    questArray = OngoingQuests.ToArray();
                    break;
            }
            for (int i = 0; i < questArray.Length; i++)
            {
                if (questArray[i].name.Split('(')[0] == typeToCheck.name.Split('(')[0])
                {
                    quests.Add(questArray[i]);
                }
            }
            return quests;
        }

        /// <summary>
        /// 从所有任务（当前、已完成、已失败）中找到所有typeToCheck类型的任务
        /// </summary>
        /// <param name="quests"></param>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public List<Quest> FindQuestsOfType(Quest typeToCheck)
        {
            List<Quest> quests = FindQuestsOfType(typeToCheck, QuestStage.ongoing);
            quests.AddRange(FindQuestsOfType(typeToCheck, QuestStage.finished));
            quests.AddRange(FindQuestsOfType(typeToCheck, QuestStage.failed));
            return quests;
        }

    }
}
