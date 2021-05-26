using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(UnitMovable))]
    [RequireComponent(typeof(Seeker))]
    public class UnitController : MonoBehaviour
    {
        [HideInInspector]
        public Seeker _seeker;

        [HideInInspector]
        public UnitMovable _unitMovable;

        public float trackPerS = 3;

        /// <summary>
        /// 正在追踪
        /// </summary>
        private bool tracking = false;

        /// <summary>
        /// 寻路运算间隔
        /// </summary>
        private float trackDelta = 0.1f;

        /// <summary>
        /// 目的地
        /// </summary>
        private Vector3 targetPos;

        /// <summary>
        /// 当前路径点序号
        /// </summary>
        private int currentWayPoint = 1;

        private Path path = null;

        private void Awake()
        {
            _seeker = GetComponent<Seeker>();
            _unitMovable = GetComponent<UnitMovable>();
        }

        private void Start()
        {
            Invoke("Seek", trackDelta);
        }

        /// <summary>
        /// 追踪一个位置。未到达位置返回false，已到达返回true
        /// </summary>
        public bool Track(Vector3 targetPos, float moveSpeed)
        {
            this.targetPos = targetPos;

            tracking = Vector3.Distance(targetPos, transform.position) > 0.5f;

            trackDelta = 1 / trackPerS;

            return !tracking;
        }

        /// <summary>
        /// 停止追踪
        /// </summary>
        public void Stop()
        {
            tracking = false;
        }

        /// <summary>
        /// 追踪
        /// </summary>
        private void Seek()
        {
            // 正在追踪 寻路
            if (tracking)
            {
                _seeker.StartPath(transform.position, targetPos, OnPathFound);
            }
            // 未追踪 跳过循环
            else
            {
                Invoke("Seek", trackDelta);
            }
        }

        /// <summary>
        /// 追踪回调
        /// </summary>
        /// <param name="newPath"></param>
        private void OnPathFound(Path newPath)
        {
            if (!newPath.error)
            {
                path = newPath;
                currentWayPoint = 1;
            }

            if (!_unitMovable.Killed)
            {
                Invoke("Seek", trackDelta);
            }
        }

        private void Update()
        {
            MoveViaPath();
        }

        private void MoveViaPath()
        {
            if (tracking && path != null && currentWayPoint < path.vectorPath.Count)
            {
                Vector3 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
                _unitMovable.MoveForce(direction);
                if (Vector2.Distance(path.vectorPath[currentWayPoint], transform.position) < 0.5f)
                {
                    currentWayPoint++;
                }
                return;
            }
        }
    }
}
