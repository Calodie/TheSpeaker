using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : UnitMovable
    {
        /// <summary>
        /// 生存时长
        /// </summary>
        [Header("生存时长")]
        public float lifeTime;

        /// <summary>
        /// 伤害
        /// </summary>
        [Header("伤害")]
        public float damage;

        /// <summary>
        /// 沿前进方向
        /// </summary>
        [Header("沿前进方向")]
        public bool directed;

        internal override void OnStart()
        {
            base.OnStart();
            IgnoreCollisionWithOwner();
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();
            LifeCountDown();
            DirectRotation();
        }

        private void IgnoreCollisionWithOwner()
        {
            if (!owner)
            {
                return;
            }
            foreach (Collider2D colliderThis in GetComponentsInChildren<Collider2D>())
            {
                foreach (Collider2D colliderProjectile in owner.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(colliderThis, colliderProjectile);
                }
            }
        }

        private void LifeCountDown()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Die();
            }
        }

        private void DirectRotation()
        {
            if (directed)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, _rigidbody.velocity);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Unit unit = collision.gameObject.GetComponentInParent<Unit>();

            if (unit)
            {
                unit._unitStatsManager.Hurt(damage, transform.position);

                Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
                if (rb)
                {
                    rb.AddForce(-collision.relativeVelocity.normalized * (damage / unit._unitStatsManager.maxHp) * rb.mass * rb.drag * 10, ForceMode2D.Impulse);
                }
            }

            Die();
        }
    }
}
