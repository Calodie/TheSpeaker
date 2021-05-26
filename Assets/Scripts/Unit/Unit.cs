using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KeyboardMan2D.UnitState;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(UnitStatsManager))]
    public class Unit : MonoBehaviour
    {
        public static List<Unit> units = new List<Unit>();

        public static Unit FindUnitWithName(string unitName)
        {
            foreach (Unit unit in units)
            {
                if (unit.unitName == unitName)
                {
                    return unit;
                }
            }
            return null;
        }

        public static List<Unit> FindUnitsWithName(string unitName)
        {
            List<Unit> foundUnits = new List<Unit>();
            foreach (Unit unit in units)
            {
                if (unit.unitName == unitName)
                {
                    foundUnits.Add(unit);
                }
            }
            return foundUnits;
        }

        public static List<Unit> FindUnitsWithTag(string tag)
        {
            List<Unit> foundUnits = new List<Unit>();
            foreach (Unit unit in units)
            {
                if (unit.CompareTag(tag))
                {
                    foundUnits.Add(unit);
                }
            }
            return foundUnits;
        }

        /// <summary>
        /// 对应动画机
        /// </summary>
        [HideInInspector]
        public Animator _animator;

        /// <summary>
        /// 对应声源
        /// </summary>
        [HideInInspector]
        public AudioSource _audioSource;

        /// <summary>
        /// 对应属性管理器
        /// </summary>
        [HideInInspector]
        public UnitStatsManager _unitStatsManager;

        /// <summary>
        /// 是否已死亡
        /// </summary>
        public bool Killed { get; private set; } = false;

        /// <summary>
        /// 名称
        /// </summary>
        [Header("名称")]
        public string unitName;

        /// <summary>
        /// 鼠标提示器最大距离
        /// </summary>
        [Header("鼠标提示器最大距离")]
        public float maxCursorDistance;

        /// <summary>
        /// 尸体保留时长
        /// </summary>
        [Header("尸体保留时长")]
        public float remainTime;

        /// <summary>
        /// 当前State
        /// </summary>
        [Header("当前State")]
        public List<StateBase> currentStates;

        /// <summary>
        /// 生存时obj
        /// </summary>
        [Header("生存时obj")]
        public GameObject body;

        /// <summary>
        /// 残骸obj
        /// </summary>
        [Header("残骸obj")]
        public GameObject remains;

        /// <summary>
        /// 生成时音效
        /// </summary>
        [Header("生成时音效")]
        public string spawnSFX;

        /// <summary>
        /// 死亡时音效
        /// </summary>
        [Header("死亡时音效")]
        public string dieSFX;

        public UnityEvent onDeath;

        [HideInInspector]
        public Unit owner;

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        internal virtual void Update()
        {
            if (!GameManager.instance.Paused)
            {
                OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            if(!GameManager.instance.Paused)
            {
                OnFixedUpdate();
            }
        }

        internal virtual void OnAwake()
        {
            units.Add(this);
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _unitStatsManager = GetComponent<UnitStatsManager>();
        }

        internal virtual void OnStart()
        {
            if (!owner)
            {
                owner = this;
            }
            // 初始化 stats
            _unitStatsManager.Initialize();
            // state 启动
            foreach (StateBase state in currentStates)
            {
                if (state)
                {
                    state.StateStart();
                }
            }

            AudioClip clip = AudioDictionary.instance.FindAudioClipWithName(spawnSFX);
            if(clip)
            {
                _audioSource.Stop();
                _audioSource.clip = clip;
                _audioSource.pitch = Random.Range(0.8f, 1.2f);
                _audioSource.Play();
            }
        }

        internal virtual void OnUpdate()
        {
            RunStates();
        }

        private void RunStates()
        {
            if (GameManager.instance.player)
            {
                StateBase[] currentStatesArray = currentStates.ToArray();
                foreach (StateBase state in currentStatesArray)
                {
                    if (state && !state.Closing)
                    {
                        state.StateUpdate();
                    }
                }
            }

        }

        internal virtual void OnFixedUpdate()
        {

        }

        internal virtual void Die()
        {
            if (Killed)
            {
                return;
            }

            AudioClip clip = AudioDictionary.instance.FindAudioClipWithName(dieSFX);
            if (clip)
            {
                _audioSource.Stop();
                _audioSource.clip = clip;
                _audioSource.pitch = Random.Range(0.8f, 1.2f);
                _audioSource.Play();
            }

            // 移除unit记录
            units.Remove(this);
            // 标记死亡
            Killed = true;
            // 改为默认tag
            tag = "Untagged";

            // 一定时间后完全消失
            Invoke("Eliminate", remainTime);
            // 取消碰撞体
            foreach (Collider2D colliderThis in GetComponentsInChildren<Collider2D>())
            {
                colliderThis.enabled = false;
            }

            // 本体消失
            if (body)
            {
                body.SetActive(false);
            }
            // 显示残骸
            if (remains)
            {
                remains.SetActive(true);
            }

            // 死亡触发事件
            onDeath.Invoke();
        }

        private void Eliminate()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if(units.Contains(this))
            {
                units.Remove(this);
            }
        }
    }
}
