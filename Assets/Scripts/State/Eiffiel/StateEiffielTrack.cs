using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateEiffielTrack : StateWithCd
    {
        /// <summary>
        /// 下一state
        /// </summary>
        [Header("下一state")]
        public StateBase[] nextStates;

        /// <summary>
        /// 传送速度
        /// </summary>
        public float teleportSpeed;

        /// <summary>
        /// 传送距离范围
        /// </summary>
        public Vector2 teleportDistanceRange;

        /// <summary>
        /// 传送时隐藏的身体
        /// </summary>
        public GameObject body;

        private Vector3 teleportTargetDifference;

        private bool teleporting = false;

        internal override void Awake()
        {
            base.Awake();
            _stateStay = GetComponent<StateStay>();
        }

        internal override void StateStart()
        {
            base.StateStart();

            teleporting = true;
            StartTeleport();
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();
            Teleport();
        }

        internal override void StateEnd()
        {
            base.StateEnd();

            teleporting = false;
        }

        /// <summary>
        /// 开启传送
        /// </summary>
        private void StartTeleport()
        {
            Vector3 difference = transform.position - playerTarget.position;
            Vector3 rotatedDifference = Quaternion.Euler(Vector3.forward * Random.Range(120, 240)) * difference;
            teleportTargetDifference = rotatedDifference.normalized * Random.Range(teleportDistanceRange.x, teleportDistanceRange.y);
        }

        /// <summary>
        /// 传送过程
        /// </summary>
        private void Teleport()
        {
            if(teleporting)
            {
                Vector3 difference = (teleportTargetDifference + playerTarget.position) - transform.position;
                if(difference.magnitude > teleportSpeed * Time.deltaTime)
                {
                    _unitMovable._rigidbody.velocity = difference.normalized * teleportSpeed;

                    // 取消碰撞体
                    foreach (Collider2D colliderThis in GetComponentsInChildren<Collider2D>())
                    {
                        colliderThis.enabled = false;
                    }

                    body.SetActive(false);
                }
                else
                {
                    transform.position = teleportTargetDifference + playerTarget.position;

                    // 恢复碰撞体
                    foreach (Collider2D colliderThis in GetComponentsInChildren<Collider2D>())
                    {
                        colliderThis.enabled = true;
                    }

                    body.SetActive(true);

                    CloseCD(new StateBase[] { nextStates[Random.Range(0, nextStates.Length)] });
                }
            }
        }
    }
}

