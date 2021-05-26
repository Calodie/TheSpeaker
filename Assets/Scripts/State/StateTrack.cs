using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    [RequireComponent(typeof(UnitController))]
    public class StateTrack : StateWithCd
    {
        /// <summary>
        /// 对应控制器
        /// </summary>
        [HideInInspector]
        public UnitController _controller;

        /// <summary>
        /// 下一state
        /// </summary>
        [Header("下一state")]
        public StateBase[] nextStates;

        /// <summary>
        /// 追踪距离范围
        /// </summary>
        public Vector2 distanceRange;

        /// <summary>
        /// 追踪时长
        /// </summary>
        public float trackDuration;

        /// <summary>
        /// 追踪计时器
        /// </summary>
        private float tickTrack;

        internal override void Awake()
        {
            base.Awake();

            _controller = GetComponent<UnitController>();
        }

        internal override void StateStart()
        {
            base.StateStart();

            tickTrack = 0;
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();

            Tick();
            Track();
        }

        internal override void StateEnd()
        {
            base.StateEnd();

            _controller.Stop();
        }

        private void Tick()
        {
            tickTrack += Time.deltaTime;
        }

        private void Track()
        {
            // 保持在范围内 旋转
            Vector3 targetPos = transform.position;

            float distance = Vector3.Distance(playerTarget.position, transform.position);
            // 太近，远离
            if (distance < distanceRange.x)
            {
                Vector3 direction = (transform.position - playerTarget.position).normalized;
                targetPos = direction * 1000 + transform.position;
            }
            //太远，靠近
            else if (distance > distanceRange.y)
            {
                targetPos = playerTarget.position;
            }
            else
            {
                CloseCD(new StateBase[] { nextStates[Random.Range(0, nextStates.Length)] });
            }
            // 旋转目标位置
            // targetPos = Quaternion.Euler(0, 0, 10) * (targetPos - player.transform.position) + player.transform.position;

            // 追踪
            _controller.Track(targetPos, _unitMovable._unitStatsManager.moveSpeed);
        }
    }
}

