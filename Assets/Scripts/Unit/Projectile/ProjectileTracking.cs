using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class ProjectileTracking : Projectile
    {
        public Transform targetTrans;

        public float trackSpeed;

        public float acceleration;

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Track();
        }

        private void Track()
        {
            if (targetTrans && !Killed)
            {
                Vector3 difference = targetTrans.position - transform.position;
                Vector3 project = Vector3.ProjectOnPlane(difference, _rigidbody.velocity);
                _rigidbody.AddForce(project.normalized * acceleration * _rigidbody.mass);
            }
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, trackSpeed);
        }
    }
}

