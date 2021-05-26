using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateEiffielAttackRotate : StateEiffielAttack
    {
        /// <summary>
        /// 旋转速度
        /// </summary>
        [Header("旋转速度")]
        public float rotateSpeed;

        /// <summary>
        /// 追踪
        /// </summary>
        [Header("追踪")]
        public bool followTarget;

        internal override void StateUpdate()
        {
            base.StateUpdate();
            Rotate();
        }

        private void Rotate()
        {
            foreach (Projectile projectile in projectiles)
            {
                Vector3 difference = projectile.transform.position - transform.position;
                if(followTarget)
                {
                    Vector3 directionToTarget = (playerTarget.position - transform.position).normalized;
                    projectile.transform.position = directionToTarget * difference.magnitude + transform.position;
                    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                }
                else
                {
                    projectile.transform.position = Quaternion.Euler(Vector3.forward * -rotateSpeed * Time.deltaTime) * difference + transform.position;
                    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, difference);
                }
            }
        }
    }
}

