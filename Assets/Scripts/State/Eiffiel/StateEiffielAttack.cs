using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public enum SpawnAngleStartPlans
    {
        /// <summary>
        /// 以给定方向为零点生成
        /// </summary>
        Single,
        /// <summary>
        /// 以随机方向为零点生成
        /// </summary>
        Random,
        /// <summary>
        /// 以瞄准方向为零点生成
        /// </summary>
        Aim,
    }

    public enum ThrowPlans
    {

    }

    public class StateEiffielAttack : StateWithCd
    {
        /// <summary>
        /// 起始等待时间
        /// </summary>
        [Header("起始等待时间")]
        public float sleepDelta;

        /// <summary>
        /// 弹丸实例
        /// </summary>
        [Header("弹丸实例")]
        public Projectile projectilePrefab;

        /// <summary>
        /// 伤害
        /// </summary>
        [Header("伤害")]
        public float damage;

        /// <summary>
        /// 弹速
        /// </summary>
        [Header("弹速")]
        public float shootSpeed;

        /// <summary>
        /// 波数
        /// </summary>
        [Header("波数")]
        public int roundCount;

        /// <summary>
        /// 波次间隔时间
        /// </summary>
        [Header("波次间隔时间")]
        public float roundDelta;

        /// <summary>
        /// 每波生成弹丸数
        /// </summary>
        [Header("每波生成弹丸数")]
        public int spawnCount;

        /// <summary>
        /// 弹丸生成间隔时间
        /// </summary>
        [Header("弹丸生成间隔时间")]
        public float spawnDelta;

        /// <summary>
        /// 弹丸发射等待时间
        /// </summary>
        [Header("弹丸发射等待时间")]
        public float throwDelta;

        /// <summary>
        /// 弹丸生成距离
        /// </summary>
        [Header("弹丸生成距离")]
        public float spawnDistance;

        /// <summary>
        /// 弹丸生成起始角度类型
        /// </summary>
        [Header("弹丸生成起始角度类型")]
        public SpawnAngleStartPlans spawnAngleStartPlan;

        /// <summary>
        /// 弹丸生成起始角度（顺时针）
        /// </summary>
        [Header("弹丸生成起始角度（顺时针）")]
        public float spawnAngleStart;

        /// <summary>
        /// 弹丸生成角度差（顺时针）
        /// </summary>
        [Header("弹丸生成角度差（顺时针）")]
        public float spawnAngleDifference;

        /// <summary>
        /// 瞄准目标投出
        /// </summary>
        [Header("瞄准目标投出")]
        public bool aim;

        /// <summary>
        /// 误差
        /// </summary>
        [Header("误差")]
        public float error;

        /// <summary>
        /// 追踪目标
        /// </summary>
        [Header("追踪目标")]
        public bool track;

        /// <summary>
        /// 下一state
        /// </summary>
        [Header("下一state")]
        public StateBase nextState;

        /// <summary>
        /// 本次弹丸记录
        /// </summary>
        internal List<Projectile> projectiles;

        /// <summary>
        /// 调整后的弹丸生成起始角度（顺时针）
        /// </summary>
        private float adjustedSpawnAngleStart;

        /// <summary>
        /// 波次指针
        /// </summary>
        private int roundPtr = 0;

        /// <summary>
        /// 弹丸生成指针
        /// </summary>
        private int spawnPtr = 0;

        /// <summary>
        /// 起始等待计时器
        /// </summary>
        private float sleepTick = 0;

        /// <summary>
        /// 波次计时器
        /// </summary>
        private float roundTick = 0;

        /// <summary>
        /// 弹丸生成计时器
        /// </summary>
        private float spawnTick = 0;

        /// <summary>
        /// 弹丸发射计时器
        /// </summary>
        private List<float> throwTicks;

        internal override void StateStart()
        {
            base.StateStart();

            spawnDistance = Mathf.Max(spawnDistance, 0.01f);

            projectiles = new List<Projectile>();

            roundPtr = 0;
            spawnPtr = 0;

            sleepTick = sleepDelta;
            roundTick = 0;
            spawnTick = 0;
            throwTicks = new List<float>();
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();

            Tick();
            if (sleepTick <= 0)
            {
                StartRound();
                SpawnProjectile();
                ThrowProjectile();
            }
        }

        private void Tick()
        {
            sleepTick -= Time.deltaTime;
            roundTick -= Time.deltaTime;
            spawnTick -= Time.deltaTime;

            for (int i = 0; i < throwTicks.Count; i++)
            {
                throwTicks[i] -= Time.deltaTime;
            }
        }

        private void StartRound()
        {
            if (roundTick <= 0 && roundPtr < roundCount)
            {
                spawnPtr = 0;

                switch (spawnAngleStartPlan)
                {
                    case SpawnAngleStartPlans.Single:
                        adjustedSpawnAngleStart = spawnAngleStart;
                        break;
                    case SpawnAngleStartPlans.Random:
                        adjustedSpawnAngleStart = Random.Range(0, 360);
                        break;
                    case SpawnAngleStartPlans.Aim:
                        adjustedSpawnAngleStart =
                            spawnAngleStart - Quaternion.LookRotation(Vector3.forward, playerTarget.position - transform.position).eulerAngles.z;
                        break;
                }

                roundTick = roundDelta;
                roundPtr++;
            }
        }

        private void SpawnProjectile()
        {
            if (spawnTick <= 0 && spawnPtr < spawnCount)
            {
                Vector3 direction = Quaternion.Euler(Vector3.forward * (-spawnAngleDifference * spawnPtr + -adjustedSpawnAngleStart)) * transform.up;

                Vector3 ballPos = direction * spawnDistance + transform.position;

                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

                Projectile projectile = Instantiate(projectilePrefab, ballPos, rotation).GetComponent<Projectile>();
                projectile.owner = _unitMovable.owner;
                projectile.transform.SetParent(transform);
                projectile._rigidbody.simulated = false;
                Projectile thisProjectile = GetComponent<Projectile>();
                if (thisProjectile)
                {
                    projectile.damage = thisProjectile.damage;
                }
                else
                {
                    projectile.damage = damage;
                }

                projectiles.Add(projectile);
                throwTicks.Add(throwDelta);

                spawnTick = spawnDelta;
                spawnPtr++;
            }
        }

        private void ThrowProjectile()
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (throwTicks[i] <= 0)
                {
                    if (projectiles[i])
                    {
                        Vector3 direction = (projectiles[i].transform.position - transform.position).normalized;

                        if (aim)
                        {
                            Vector3 targetPos = playerTarget.position + (Vector3)Random.insideUnitCircle * error;
                            direction = (targetPos - projectiles[i].transform.position).normalized;
                        }

                        if (track)
                        {
                            ProjectileTracking projectileTracking = projectiles[i] as ProjectileTracking;
                            projectileTracking.trackSpeed = shootSpeed;
                            projectileTracking.targetTrans = playerTarget;
                        }

                        projectiles[i].transform.SetParent(null);
                        projectiles[i]._rigidbody.simulated = true;
                        projectiles[i]._rigidbody.velocity = direction * shootSpeed;

                    }
                    projectiles.RemoveAt(i);
                    throwTicks.RemoveAt(i);
                    i--;

                    if (projectiles.Count <= 0 && spawnPtr >= spawnCount && roundPtr >= roundCount)
                    {
                        CloseCD(new StateBase[] { nextState });
                    }
                }
            }
        }
    }
}

