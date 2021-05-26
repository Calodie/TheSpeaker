using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(UnitInitStats))]
    [RequireComponent(typeof(UIStats))]
    public class UnitStatsManager : MonoBehaviour
    {
        /// <summary>
        /// 对应单位
        /// </summary>
        [HideInInspector]
        public Unit _unit;

        /// <summary>
        /// 对应默认属性表
        /// </summary>
        [HideInInspector]
        public UnitInitStats _unitInitStats;

        /// <summary>
        /// 对应ui控制器
        /// </summary>
        [HideInInspector]
        public UIStats _uIStats;

        /// <summary>
        /// 对应碰撞体
        /// </summary>
        [HideInInspector]
        public CircleCollider2D _colliderSelf;

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
        ///  当前生命值
        /// </summary>
        [Header("当前生命值")]
        public float hp;
        /// <summary>
        ///  每秒生命回复
        /// </summary>
        [Header("每秒生命回复")]
        public float hpRege;
        /// <summary>
        ///  生命回复受伤等待时长
        /// </summary>
        [Header("生命回复受伤等待时长")]
        public float hpRegeCd;

        /// <summary>
        ///  最大能量
        /// </summary>
        [Header("最大能量")]
        public float maxMp;
        /// <summary>
        ///  当前能量
        /// </summary>
        [Header("当前能量")]
        public float mp;
        /// <summary>
        ///  每秒能量回复
        /// </summary>
        [Header("每秒能量回复")]
        public float mpRege;
        /// <summary>
        ///  能量回复消耗等待时长
        /// </summary>
        [Header("能量回复消耗等待时长")]
        public float mpRegeCd;

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

        /// <summary>
        /// 受伤音效
        /// </summary>
        [Header("受伤音效")]
        public string hurtSFX;

        /// <summary>
        /// 生命受伤回复倒计时
        /// </summary>
        private float hpRegeTick = 0;

        /// <summary>
        /// 能量消耗回复倒计时
        /// </summary>
        private float mpRegeTick = 0;

        /// <summary>
        /// 受伤硬直倒计时
        /// </summary>
        private float hurtStunTick = 0;

        /// <summary>
        /// 当前是否硬直
        /// </summary>
        public bool Stunned => hurtStunTick > 0;

        private Dictionary<Unit, float> hurtDictionary = new Dictionary<Unit, float>();

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _uIStats = GetComponent<UIStats>();
            _unitInitStats = GetComponent<UnitInitStats>();
            _colliderSelf = GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            if (immortal || GameManager.instance.Paused)
            {
                return;
            }

            Rege();
            Tick();
        }

        private void FixedUpdate()
        {
            if (immortal || GameManager.instance.Paused)
            {
                return;
            }

            CollideDamage();
        }

        /// <summary>
        /// 每秒回复
        /// </summary>
        private void Rege()
        {
            if (!_unit.Killed && !immortal)
            {
                if (hpRegeTick <= 0)
                {
                    hp = Mathf.Min(hp + hpRege * Time.deltaTime, maxHp);
                }
                _uIStats.SetHp(hp, maxHp, false);
                if (mpRegeTick <= 0)
                {
                    mp = Mathf.Min(mp + mpRege * Time.deltaTime, maxMp);
                }
                _uIStats.SetMp(mp, maxMp);
            }
        }

        private void Tick()
        {
            if (!_unit.Killed)
            {
                // 生命回复倒计时
                hpRegeTick = Mathf.Max(hpRegeTick - Time.deltaTime, 0);
                // 能量回复倒计时
                mpRegeTick = Mathf.Max(mpRegeTick - Time.deltaTime, 0);
                // 硬直倒计时
                hurtStunTick = Mathf.Max(hurtStunTick - Time.deltaTime, 0);

                // 碰撞伤害倒计时
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
        }

        public void Initialize()
        {
            immortal = _unitInitStats.immortal;
            maxHp = Mathf.Max(_unitInitStats.maxHp, 1);
            hp = maxHp;
            hpRege = _unitInitStats.hpRege;
            hpRegeCd = _unitInitStats.hpRegeCd;

            maxMp = Mathf.Max(_unitInitStats.maxMp, 0);
            mp = maxMp;
            mpRege = _unitInitStats.mpRege;
            mpRegeCd = _unitInitStats.mpRegeCd;

            moveSpeed = Mathf.Max(_unitInitStats.moveSpeed, 0f);

            hurtStunTime = Mathf.Max(_unitInitStats.hurtStunTime, 0);

            collideDamage = Mathf.Max(_unitInitStats.collideDamage, 0);
            collideDamagePerS = Mathf.Max(_unitInitStats.collideDamagePerS, 0);
        }

        /// <summary>
        /// 改变最大生命值
        /// </summary>
        /// <param name="change"></param>
        public void UpdateMaxUp(float change)
        {
            maxHp = Mathf.Max(1, maxHp + change);

            if (change > 0)
            {
                Heal(change);
            }
        }

        /// <summary>
        /// 受伤 致死返回false
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool Hurt(float damage, Vector3 hitPos)
        {
            // 伤害为负 回复生命
            if (damage < 0)
            {
                Heal(-damage);
                return true;
            }

            // 无敌或已死 忽略
            if (immortal || _unit.Killed)
            {
                return true;
            }

            damage *= Random.Range(0.8f, 1.2f);

            // 伤害小于1 忽略
            if (damage < 1)
            {
                return true;
            }

            // 造成伤害
            hp -= damage;

            // 更新UI
            _uIStats.SetHp(hp, maxHp, true);

            // 显示伤害数字
            UITextHit textHit =
                Instantiate(ResourceLoader.Load("Prefabs/UI/UITextHitPrefab") as GameObject, UIIngame.instance.instantTrans).GetComponent<UITextHit>();
            if (hitPos == Vector3.zero)
            {
                textHit.worldPos = transform.position;
            }
            else
            {
                textHit.worldPos = hitPos;
            }
            textHit.worldPos += (Vector3)Random.insideUnitCircle * 0.5f;
            textHit.damage = damage;
            textHit.text.color = textHit.colorHurt;

            // 回复等待
            hpRegeTick = hpRegeCd;

            // 受伤硬直
            hurtStunTick = hurtStunTime;

            // 音效
            AudioClip clip = AudioDictionary.instance.FindAudioClipWithName(hurtSFX);
            if (clip)
            {
                _unit._audioSource.Stop();
                _unit._audioSource.clip = clip;
                _unit._audioSource.pitch = Random.Range(0.8f, 1.2f);
                _unit._audioSource.Play();
            }

            // 致死伤害
            if (hp <= 0)
            {
                // 死亡
                _unit.Die();
                // 隐藏UI
                _uIStats.Remove();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 治疗
        /// </summary>
        /// <param name="healPoint"></param>
        public void Heal(float healPoint)
        {
            if (healPoint < 0)
            {
                Hurt(-healPoint, Vector3.zero);
                return;
            }

            // 无敌或已死 忽略
            if (immortal || _unit.Killed)
            {
                return;
            }

            healPoint *= Random.Range(0.8f, 1.2f);

            if (hp + healPoint <= maxHp)
            {
                // 显示治疗数字
                UITextHit textHit =
                    Instantiate(ResourceLoader.Load("Prefabs/UI/UITextHitPrefab") as GameObject, UIIngame.instance.instantTrans).GetComponent<UITextHit>();
                textHit.worldPos = transform.position;
                textHit.worldPos += (Vector3)Random.insideUnitCircle * 0.5f;
                textHit.damage = healPoint;
                textHit.text.color = textHit.colorHeal;
            }

            hp = Mathf.Min(hp + healPoint, maxHp);

            _uIStats.SetHp(hp, maxHp, false);
        }

        /// <summary>
        /// 改变最大能量
        /// </summary>
        /// <param name="change"></param>
        public void UpdateMaxMp(float change)
        {
            maxMp = Mathf.Max(1, maxMp + change);

            if (change > 0)
            {
                RestoreMp(change);
            }
        }

        /// <summary>
        /// 使用能量 不足返回false
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CostMp(float cost)
        {
            // 消耗为负 回复能量
            if (cost < 0)
            {
                RestoreMp(-cost);
                return true;
            }

            // 无敌或已死 忽略
            if (immortal || _unit.Killed)
            {
                return true;
            }

            // 消耗小于1 忽略
            if (cost < 1)
            {
                return true;
            }

            // 回复等待（无论能量是否足够消耗）
            mpRegeTick = mpRegeCd;

            // 能量不足
            if (mp - cost < 0)
            {
                return false;
            }
            else
            {
                // 消耗
                mp -= cost;

                // 更新UI
                _uIStats.SetMp(mp, maxMp);
                return true;
            }
        }

        /// <summary>
        /// 回复能量
        /// </summary>
        /// <param name="restorePoint"></param>
        public void RestoreMp(float restorePoint)
        {
            if (restorePoint < 0)
            {
                CostMp(-restorePoint);
                return;
            }

            // 无敌或已死 忽略
            if (immortal || _unit.Killed)
            {
                return;
            }

            mp = Mathf.Min(mp + restorePoint, maxMp);

            _uIStats.SetMp(mp, maxMp);
        }

        /// <summary>
        /// 碰撞伤害
        /// </summary>
        /// <param name="collision"></param>
        private void CollideDamage()
        {
            if (!_unit.Killed && _colliderSelf)
            {
                // 找到所有接触的Unit，是可伤害的类型、无重复且不处于碰撞伤害cd
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_colliderSelf.offset + (Vector2)transform.position, _colliderSelf.radius);

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
                    hurtDictionary.Add(toHurt, 1 / collideDamagePerS);

                    toHurt._unitStatsManager.Hurt(collideDamage, Vector3.zero);

                    Rigidbody2D rb = toHurt.GetComponent<Rigidbody2D>();
                    if (rb)
                    {
                        Vector3 direction = (toHurt.transform.position - transform.position).normalized;
                        rb.AddForce(direction * collideDamage, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}