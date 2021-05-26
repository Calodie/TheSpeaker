using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class ProjectileLaser : Projectile
    {
        /// <summary>
        /// 激光主干
        /// </summary>
        [Header("激光主干")]
        public Transform laserTrans;

        /// <summary>
        /// 激光终端特效
        /// </summary>
        [Header("激光终端特效")]
        public ParticleSystem endEffect;

        /// <summary>
        /// 每秒伤害次数
        /// </summary>
        [Header("每秒伤害次数")]
        public float hurtPerS;

        /// <summary>
        /// 激光宽度
        /// </summary>
        [Header("激光宽度")]
        public float width;

        /// <summary>
        /// 激光闪烁宽度差
        /// </summary>
        [Header("激光闪烁宽度差")]
        public float widthFlashRange;

        /// <summary>
        /// 激光最大长度
        /// </summary>
        [Header("激光最大长度")]
        public float maxLength;

        private float length;

        private Dictionary<Unit, float> hurtDictionary = new Dictionary<Unit, float>();

        internal override void OnUpdate()
        {
            base.OnUpdate();

            Tick();
            HitLength();
            DealDamage();
        }

        private void Tick()
        {
            List<Unit> toRemoves = new List<Unit>();

            Unit[] keys = new Unit[hurtDictionary.Count];
            hurtDictionary.Keys.CopyTo(keys, 0);

            foreach (Unit unit in keys)
            {
                hurtDictionary[unit] -= Time.deltaTime;
                if (hurtDictionary[unit] <= 0)
                {
                    toRemoves.Add(unit);
                }
            }

            foreach (Unit toRemove in toRemoves)
            {
                hurtDictionary.Remove(toRemove);
            }
        }

        private void HitLength()
        {
            LayerMask mask = ~(1 << LayerMask.NameToLayer("Projectile"));
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, width / 2, transform.up, maxLength, mask);
            if (hit.collider)
            {
                length = Vector2.Distance(transform.position, hit.point) + 0.1f;
                endEffect.transform.position = hit.point;
                if(endEffect.isStopped)
                {
                    endEffect.Play();
                }
            }
            else
            {
                length = maxLength;
                if (endEffect.isPlaying)
                {
                    endEffect.Stop();
                }
            }

            laserTrans.localScale = new Vector3(width + UnityEngine.Random.Range(-widthFlashRange, widthFlashRange), length, 1);
        }

        private void DealDamage()
        {
            // 找到所有接触的Unit，是可伤害的类型、无重复且不处于伤害cd
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + transform.up * length / 2, new Vector2(width, length), transform.eulerAngles.z);

            List<Unit> toHurts = new List<Unit>();

            foreach (Collider2D collider in colliders)
            {
                Unit unit = collider.GetComponentInParent<Unit>();
                if (unit && unit.CompareTag("Player") && !toHurts.Contains(unit) && !hurtDictionary.ContainsKey(unit))
                {
                    toHurts.Add(unit);
                }
            }

            foreach (Unit toHurt in toHurts)
            {
                hurtDictionary.Add(toHurt, 1 / hurtPerS);

                toHurt._unitStatsManager.Hurt(damage, Vector3.zero);
            }
        }
    }
}

