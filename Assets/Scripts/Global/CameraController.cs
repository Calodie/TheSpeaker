using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        [HideInInspector]
        public Camera _camera;

        /// <summary>
        /// 普通FOV
        /// </summary>
        [Header("普通FOV")]
        public float fovNormal;
        /// <summary>
        /// 遇敌FOV
        /// </summary>
        [Header("遇敌FOV")]
        public float fovEnemy;

        /// <summary>
        /// 普通摄像机移速倍率
        /// </summary>
        [Header("普通摄像机移速倍率")]
        public float trackMultiNormal;
        /// <summary>
        /// 遇敌摄像机移速倍率
        /// </summary>
        [Header("遇敌摄像机移速倍率")]
        public float trackMultiEnemy;

        private UnitMovable player;

        private Vector3 cameraVelocity;

        private float cameraFOVSpeed;

        /// <summary>
        /// 当前的目标fov
        /// </summary>
        private float targetFOV;

        /// <summary>
        /// 当前的fov优先级
        /// </summary>
        public uint fovPriority;

        private float trackMultiCurrent;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            _camera = GetComponent<Camera>();

            targetFOV = 0;
        }

        /// <summary>
        /// 外部设置FOV
        /// </summary>
        public void SetFOV(float fov, uint priority)
        {
            if (priority > fovPriority)
            {
                targetFOV = fov;
                fovPriority = priority;
            }
        }

        private void Update()
        {
            player = GameManager.instance.player;
            DetactEnemy();
            Track();
            TrackFOV();
            fovPriority = 0;
        }

        private void LateUpdate()
        {
            LateTrack();
        }

        /// <summary>
        /// 敌人存在感知并调整属性
        /// </summary>
        private void DetactEnemy()
        {
            // 无player或fov被托管时不感知
            if (!player)
            {
                return;
            }

            if (Unit.FindUnitsWithTag("Enemy").Count > 0)
            {
                targetFOV = fovEnemy;
                trackMultiCurrent = trackMultiEnemy;
                player._unitStatsManager.moveSpeed = player._unitStatsManager._unitInitStats.moveSpeed * 1.5f;
            }
            else
            {
                if (fovPriority == 0)
                {
                    targetFOV = fovNormal;
                }

                trackMultiCurrent = trackMultiNormal;
                player._unitStatsManager.moveSpeed = player._unitStatsManager._unitInitStats.moveSpeed * 1.5f;
            }
        }

        private void Track()
        {
            if (!player)
            {
                return;
            }

            Vector3 targetPos = player.transform.position + Vector3.back * 10;
            if (Input.GetKey(KeyCode.F))
            {
                Vector3 mouseDifference = Vector3.ClampMagnitude(_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position, _camera.fieldOfView);
                targetPos += mouseDifference * 0.33f;
            }
            Vector3 difference = targetPos - transform.position;
            float speed = Mathf.Max(0.1f, difference.magnitude * trackMultiCurrent);
            cameraVelocity = speed * difference.normalized;
        }

        private void TrackFOV()
        {
            if (!player)
            {
                return;
            }

            cameraFOVSpeed = (targetFOV - _camera.orthographicSize) * 1f;
        }

        private void LateTrack()
        {
            if(!player)
            {
                return;
            }

            transform.position += cameraVelocity * Time.deltaTime;
            _camera.orthographicSize += cameraFOVSpeed * Time.deltaTime;
        }
    }
}
