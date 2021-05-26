using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(Attacker))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovableSpriteManager))]
    public class UnitMovable : Unit
    {
        [HideInInspector]
        public Rigidbody2D _rigidbody;

        [HideInInspector]
        public MovableSpriteManager _spriteManager;

        /// <summary>
        /// 对应攻击模块
        /// </summary>
        [HideInInspector]
        public Attacker _attacker;

        /// <summary>
        /// 当前移动方向
        /// </summary>
        private Vector3 movingDirection;

        private Vector2 velocity_old;
        private bool just_paused;

        internal override void Update()
        {
            base.Update();

            // 存储暂停时的速度
            if(GameManager.instance.Paused)
            {
                if(!just_paused)
                {
                    just_paused = true;
                    velocity_old = _rigidbody.velocity;
                    foreach(ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
                    {
                        particle.Pause();
                    }
                }

                _rigidbody.velocity = Vector2.zero;
                _rigidbody.simulated = false;
            }
            else
            {
                if (just_paused)
                {
                    _rigidbody.velocity = velocity_old;
                    _rigidbody.simulated = true;
                    foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
                    {
                        particle.Play();
                    }
                }

                just_paused = false;
            }
        }

        internal override void OnAwake()
        {
            base.OnAwake();
            _rigidbody = GetComponent<Rigidbody2D>();
            _attacker = GetComponent<Attacker>();
            _spriteManager = GetComponent<MovableSpriteManager>();
        }

        public void MoveForce(Vector3 direction) => movingDirection = direction;

        public void MoveVelocity(Vector3 direction)
        {
            if (!Killed && !_unitStatsManager.Stunned)
            {
                _rigidbody.velocity = direction * _unitStatsManager.moveSpeed;
            }
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            MoveWithForce();
        }

        private void MoveWithForce()
        {
            if (!Killed && !_unitStatsManager.Stunned)
            {
                if (movingDirection.magnitude > 0.1f)
                {
                    _rigidbody.AddForce(movingDirection * _unitStatsManager.moveSpeed * _rigidbody.mass * _rigidbody.drag);

                    if (Mathf.Abs(movingDirection.x) > 0.1f)
                    {
                        _spriteManager.left = movingDirection.x < 0;
                    }
                }

                if (_animator)
                {
                    _animator.SetBool("Running", movingDirection.magnitude > 0);
                }
            }

            if (Killed)
            {
                _rigidbody.velocity = Vector2.zero;
            }

            movingDirection = Vector3.zero;
        }

        internal override void Die()
        {
            if (Killed)
            {
                return;
            }

            base.Die();
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
        }
    }
}
