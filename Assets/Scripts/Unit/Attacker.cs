using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KeyboardMan2D.UnitState;

namespace KeyboardMan2D
{
    public class Attacker : MonoBehaviour
    {
        /// <summary>
        /// 对应Unit
        /// </summary>
        [HideInInspector]
        public Unit _unit;

        /// <summary>
        /// 当前正在攻击
        /// </summary>
        [HideInInspector]
        public bool isAttacking;

        public GameObject projectilePrefab;

        /// <summary>
        /// 能量消耗
        /// </summary>
        [Header("能量消耗")]
        public float mpCost;

        /// <summary>
        /// 弹速
        /// </summary>
        [Header("弹速")]
        public float shootSpeed;

        /// <summary>
        /// 攻速
        /// </summary>
        [Header("攻速")]
        public float attackPerS;

        /// <summary>
        /// 是ai
        /// </summary>
        [Header("是ai")]
        public bool ai;

        /// <summary>
        /// 攻击启动state
        /// </summary>
        [Header("攻击启动state")]
        public StateBase attackState;

        /// <summary>
        /// 攻击方向
        /// </summary>
        [HideInInspector]
        public Vector3 direction;

        /// <summary>
        /// 攻击动画启动cd
        /// </summary>
        private float tickAnimAttack = 0;

        /// <summary>
        /// 实际伤害cd
        /// </summary>
        private float tickAttack = 0;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            Tick();
            SetAnim();
            Attack();

            AIFollow();
        }

        private void AIFollow()
        {
            if (ai)
            {
                (_unit as UnitMovable)._spriteManager.followAttacker = true;
                if(GameManager.instance.player)
                {
                    direction = GameManager.instance.player.transform.position - transform.position;
                }
            }
        }

        private void Tick()
        {
            tickAnimAttack = Mathf.Max(tickAnimAttack - Time.deltaTime, 0);
            tickAttack = Mathf.Max(tickAttack - Time.deltaTime, 0);
        }

        private void SetAnim()
        {
            if (!_unit._animator)
            {
                return;
            }

            _unit._animator.SetFloat("AttackPerS", attackPerS);
        }

        /// <summary>
        /// 发起攻击
        /// </summary>
        public void StartAttack(Vector3 direction)
        {
            if (tickAnimAttack <= 0 && _unit._unitStatsManager.CostMp(mpCost))
            {
                this.direction = direction;
                tickAnimAttack = 1 / attackPerS;
                _unit._animator.SetBool("IsAttacking", true);
            }
        }

        /// <summary>
        /// 真实攻击
        /// </summary>
        private void Attack()
        {
            if (isAttacking && tickAttack <= 0)
            {
                tickAttack = tickAnimAttack;
                _unit._animator.SetBool("IsAttacking", false);

                if (ai)
                {
                    attackState.Open();
                }
                else
                {
                    Projectile projectile =
                        Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, direction)).GetComponent<Projectile>();
                    projectile.owner = _unit.owner;
                    projectile._rigidbody.velocity = shootSpeed * direction;
                    projectile.damage = 100;
                }
            }
        }
    }
}

