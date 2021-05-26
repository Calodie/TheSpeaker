using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardMan2D
{
    public enum LogicTypes
    {
        And, Or
    }

    [System.Serializable]
    public struct QuestLogic
    {
        public Quest quest;

        /// <summary>
        /// 逻辑类型
        /// </summary>
        [Header("逻辑类型")]
        public LogicTypes logicType;

        /// <summary>
        /// 子逻辑
        /// </summary>
        [Header("子逻辑")]
        public QuestLogic[] subLogics;

        /// <summary>
        /// 逻辑演算
        /// </summary>
        /// <returns></returns>
        public bool Conduct()
        {
            /*string methodName = logic.triggerEvent.GetPersistentMethodName(0);
            var target = logic.triggerEvent.GetPersistentTarget(0);
            MethodInfo method = target.GetType().GetMethod(methodName, new Type[] { });
            object[] parameters = null;*/
            bool open = QuestManager.instance.FindFirstQuestOfType(quest, QuestStage.finished);

            switch (logicType)
            {
                case LogicTypes.And:
                    foreach(QuestLogic subLogic in subLogics)
                    {
                        open = open && subLogic.Conduct();
                    }
                    break;
                case LogicTypes.Or:
                    foreach (QuestLogic subLogic in subLogics)
                    {
                        open = open || subLogic.Conduct();
                    }
                    break;
            }

            return open;
        }
    }

    public enum QuestTypes
    {
        toInteract, toKill, toFinishAll
    }

    /// <summary>
    /// 任务。在被实例化之前不会执行任何逻辑，仅用作存储数据
    /// </summary>
    public class Quest : MonoBehaviour
    {

        /// <summary>
        /// 任务名
        /// </summary>
        [Header("任务名")]
        public string questName;

        /// <summary>
        /// 任务类型
        /// </summary>
        [Header("任务类型")]
        public QuestTypes questType;

        /// <summary>
        /// 任务描述
        /// </summary>
        [Header("任务描述")]
        public string content;

        /// <summary>
        /// 任务所属场景
        /// </summary>
        [Header("任务所属场景")]
        public string scene;

        /// <summary>
        /// 任务目标数
        /// </summary>
        [Header("任务目标数")]
        public int goalCount;

        /// <summary>
        /// 任务可重复完成
        /// </summary>
        [Header("任务可重复完成")]
        public bool reusable;

        /// <summary>
        /// 显示任务目的地
        /// </summary>
        [Header("显示任务目的地")]
        public bool showDestination;

        /// <summary>
        /// 任务目的地
        /// </summary>
        [Header("任务目的地")]
        public string destination;

        /// <summary>
        /// 任务目的地高度
        /// </summary>
        [Header("任务目的地高度")]
        public float destinationHeight;

        /// <summary>
        /// 冲突任务
        /// </summary>
        [Header("冲突任务")]
        public List<Quest> contraQuests;

        /// <summary>
        /// 选择其一完成任务
        /// </summary>
        [Header("选择其一完成任务")]
        public List<Quest> selectaQuests;

        /// <summary>
        /// 前置任务相关逻辑
        /// </summary>
        [Header("前置任务相关逻辑")]
        public LogicTypes logicType;

        /// <summary>
        /// 前置任务
        /// </summary>
        [Header("前置任务")]
        public QuestLogic[] prevs;

        /// <summary>
        /// 后置任务
        /// </summary>
        [Header("后置任务")]
        public Quest[] succs;

        public UnityEvent onAdd;
        public UnityEvent onFinish;
        public UnityEvent onFail;

        /// <summary>
        /// 正在追踪
        /// </summary>
        [HideInInspector]
        public bool tracking = false;

        /// <summary>
        /// 任务状态
        /// </summary>
        public QuestStage stage = QuestStage.ongoing;

        private List<Transform> cursorTargets = new List<Transform>();
        private List<UIQuestCursor> cursors = new List<UIQuestCursor>();

        private void Start()
        {
            
        }

        private void Update()
        {
            LocateCursors();
            ShowCursors();
            Finish();
        }

        private void LocateCursors()
        {
            cursorTargets = new List<Transform>();

            if (stage != QuestStage.ongoing)
            {
                return;
            }

            // 目的地在同一场景下 找到具体位置
            if (SceneManager.GetActiveScene().name == scene)
            {
                foreach (Unit unit in Unit.FindUnitsWithName(destination))
                {
                    cursorTargets.Add(unit.transform);
                }
            }
            // 目的地不在同一场景下 找到到达场景的传送点
            else
            {
                List<FigureNode> route = SceneLocalizer.instance.FindRouteToScene(scene);
                if (route != null)
                {
                    string succSceneName = route[1].nodeName;
                    Portal portal = Portal.FindPortalToScene(succSceneName);
                    if (portal)
                    {
                        cursorTargets.Add(portal.transform);
                    }
                }
            }
        }

        private void ShowCursors()
        {
            // 去除多余cursor
            UIQuestCursor[] cursorArray = cursors.ToArray();
            for (int i = cursorTargets.Count; i < cursorArray.Length; i++)
            {
                Destroy(cursorArray[i].gameObject);
                cursors.Remove(cursorArray[i]);
            }

            // 生成新cursor
            for (int i = cursors.Count; i < cursorTargets.Count; i++)
            {
                UIQuestCursor newCursor =
                    Instantiate(ResourceLoader.Load("Prefabs/UI/UIQuestCursorPrefab") as GameObject, UIIngame.instance.instantTrans).
                    GetComponent<UIQuestCursor>();
                newCursor.quest = this;

                cursors.Add(newCursor);
            }

            // 追踪
            for (int i = 0; i < cursorTargets.Count; i++)
            {
                cursors[i].destination = cursorTargets[i];
            }
        }

        private void Finish()
        {
            if (stage == QuestStage.ongoing)
            {
                if (cursorTargets.Count <= 0)
                {
                    QuestManager.instance.FinishQuest(this);
                }
            }
        }

        private void OnDestroy()
        {
            // 删除cursor
            UIQuestCursor[] cursorArray = cursors.ToArray();
            for (int i = 0; i < cursorArray.Length; i++)
            {
                if(cursorArray[i])
                {
                    Destroy(cursorArray[i].gameObject);
                }
            }
        }
    }
}
