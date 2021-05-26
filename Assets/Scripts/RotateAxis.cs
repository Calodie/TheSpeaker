using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class RotateAxis : MonoBehaviour
    {
        public Rigidbody2D _rigidbody;

        public float rotateSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_rigidbody.velocity.x > 0)
            {
                _rigidbody.angularVelocity = -rotateSpeed;
            }
            else
            {
                _rigidbody.angularVelocity = rotateSpeed;
            }
        }
    }
}

