using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FOVClient : MonoBehaviour
    {
        [HideInInspector]
        public CircleCollider2D _collider;

        public float maxFOV;

        public uint priority;

        public bool gradient;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            SetFOV();
        }

        private void SetFOV()
        {
            Unit player = GameManager.instance.player;
            if (!player)
            {
                return;
            }

            Vector3 center = (Vector3)_collider.offset + transform.position;
            float distance = Vector3.Distance(center, player.transform.position);
            if (distance < _collider.radius)
            {
                if(gradient)
                {
                    CameraController.instance.SetFOV(
                        maxFOV - (distance / _collider.radius) * (maxFOV - CameraController.instance.fovNormal),
                        priority
                        ); 
                }
                else
                {
                    CameraController.instance.SetFOV(maxFOV, priority);
                }
            }
        }
    }
}

