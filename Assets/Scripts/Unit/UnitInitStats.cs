using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class UnitInitStats : MonoBehaviour
    {
        /// <summary>
        /// 是否无敌
        /// </summary>
        [Header("是否无敌")]
        public bool immortal;

        /// <summary>
        ///  最大生命值
        /// </summary>
        [Header("最大生命值")]
        public float maxHp;
        /// <summary>
        ///  每秒生命回复
        /// </summary>
        [Header("每秒生命回复")]
        public float hpRege;
        /// <summary>
        ///  生命回复受伤等待时长
        /// </summary>
        [Header("生命回复受伤等待时长")]
        public float hpRegeCd = 10;

        /// <summary>
        ///  最大能量
        /// </summary>
        [Header("最大能量")]
        public float maxMp;
        /// <summary>
        ///  每秒能量回复
        /// </summary>
        [Header("每秒能量回复")]
        public float mpRege;
        /// <summary>
        ///  能量回复消耗等待时长
        /// </summary>
        [Header("能量回复消耗等待时长")]
        public float mpRegeCd = 2;

        /// <summary>
        ///  移动速度
        /// </summary>
        [Header("移动速度")]
        public float moveSpeed;

        /// <summary>
        /// 受伤硬直时长
        /// </summary>
        [Header("受伤硬直时长")]
        public float hurtStunTime;

        /// <summary>
        /// 碰撞伤害
        /// </summary>
        [Header("碰撞伤害")]
        public float collideDamage;
        /// <summary>
        /// 每秒造成碰撞伤害次数
        /// </summary>
        [Header("每秒造成碰撞伤害次数")]
        public float collideDamagePerS;
    }
}
