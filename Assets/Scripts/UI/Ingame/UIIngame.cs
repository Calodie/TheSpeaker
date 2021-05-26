using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class UIIngame : MonoBehaviour
    {
        public static UIIngame instance;

        /// <summary>
        /// 游戏内光标指示器
        /// </summary>
        public UICursor cursor;

        /// <summary>
        /// 游戏内任务缩略
        /// </summary>
        public UIQuestInfo questInfo;

        /// <summary>
        /// 玩家血条
        /// </summary>
        public UIBar playerBarHp;

        /// <summary>
        /// 玩家能量条
        /// </summary>
        public UIBar playerBarMp;

        /// <summary>
        /// Boss血条栏
        /// </summary>
        public UIBossHpContainer bossHpContainer;

        /// <summary>
        /// 生成物容器
        /// </summary>
        public Transform instantTrans;

        /// <summary>
        /// 死亡界面
        /// </summary>
        public GameObject panelDead;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Update()
        {
            if (!GameManager.instance.player)
            {
                panelDead.SetActive(true);
            }
        }
    }
}
