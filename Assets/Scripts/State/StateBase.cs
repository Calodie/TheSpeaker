using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    [RequireComponent(typeof(UnitMovable))]
    public class StateBase : MonoBehaviour
    {
        /// <summary>
        /// 对应单位
        /// </summary>
        [HideInInspector]
        public UnitMovable _unitMovable;

        /// <summary>
        /// 音效
        /// </summary>
        [Header("音效")]
        public string sfx;

        /// <summary>
        /// 玩家
        /// </summary>
        internal UnitMovable player;

        internal Transform playerTarget;

        /// <summary>
        /// 正在结束state
        /// </summary>
        public bool Closing { get; private set; } = false;

        internal virtual void Awake()
        {
            _unitMovable = GetComponent<UnitMovable>();
        }

        /// <summary>
        /// 开启state
        /// </summary>
        public void Open()
        {
            player = GameManager.instance.player;

            if (!player || Closing)
            {
                return;
            }

            playerTarget = player.GetComponentInChildren<UnitAimTrans>().transform;

            if (!_unitMovable.currentStates.Contains(this))
            {
                StateStart();
                _unitMovable.currentStates.Add(this);
            }
        }

        /// <summary>
        /// 结束并切换至下一state
        /// </summary>
        /// <param name="nextStates"></param>
        public void Close(IEnumerable<StateBase> nextStates)
        {
            if (Closing)
            {
                return;
            }
            Closing = true;

            CancelInvoke();

            if (_unitMovable.currentStates.Contains(this))
            {
                StateEnd();
                _unitMovable.currentStates.Remove(this);

                // 间隔一帧开启下一state避免死循环
                StartCoroutine(CloseCoro(nextStates));
            }
            else
            {
                Closing = false;
            }
        }

        private IEnumerator CloseCoro(IEnumerable<StateBase> nextStates)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Closing = false;

            if (nextStates != null)
            {
                foreach (StateBase state in nextStates)
                {
                    if (state)
                    {
                        state.Open();
                    }
                }
            }
        }

        /// <summary>
        /// state开端逻辑
        /// </summary>
        internal virtual void StateStart()
        {
            player = GameManager.instance.player;
            playerTarget = player.GetComponentInChildren<UnitAimTrans>().transform;

            AudioClip clip = AudioDictionary.instance.FindAudioClipWithName(sfx);
            if (clip)
            {
                _unitMovable._audioSource.Stop();
                _unitMovable._audioSource.clip = clip;
                _unitMovable._audioSource.pitch = Random.Range(0.8f, 1.2f);
                _unitMovable._audioSource.Play();
            }
        }

        /// <summary>
        /// state逻辑
        /// </summary>
        internal virtual void StateUpdate()
        {
            if (!GameManager.instance.player|| _unitMovable.Killed)
            {
                Close(null);
            }
        }

        /// <summary>
        /// state尾声逻辑（不允许在此处开启state）
        /// </summary>
        internal virtual void StateEnd()
        {

        }
    }
}

