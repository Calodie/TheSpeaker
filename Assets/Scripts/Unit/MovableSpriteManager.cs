using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class MovableSpriteManager : MonoBehaviour
    {
        [HideInInspector]
        public UnitMovable _unitMovable;

        public GameObject[] obj2Inverse;
        public GameObject[] objLeft;
        public GameObject[] objRight;

        [HideInInspector]
        public bool left;
        [HideInInspector]
        public bool followAttacker;

        private void Awake()
        {
            _unitMovable = GetComponent<UnitMovable>();
        }

        private void Update()
        {
            FollowAttacker();
            SetDirection();
        }

        private void FollowAttacker()
        {
            if(followAttacker)
            {
                left = _unitMovable._attacker.direction.x < 0;
            }
        }

        private void SetDirection()
        {
            foreach (GameObject obj in obj2Inverse)
            {
                if (left && obj.transform.localScale != new Vector3(1, 1, 1))
                {
                    obj.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (!left && obj.transform.localScale != new Vector3(-1, 1, 1))
                {
                    obj.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            foreach (GameObject obj in objLeft)
            {
                if (left)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }

            foreach (GameObject obj in objRight)
            {
                if (left)
                {
                    obj.SetActive(false);
                }
                else
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}
