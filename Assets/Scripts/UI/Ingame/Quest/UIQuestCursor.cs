using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KeyboardMan2D
{
    public class UIQuestCursor : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform _rectTransform;

        [HideInInspector]
        public Animator _animator;

        public Image imageCursor;

        public Transform destination;

        private float destinationHeight;

        public Quest quest;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            destinationHeight = quest.destinationHeight;
            imageCursor.color = QuestManager.instance.GetQuestColor(quest.questType);
        }

        private void Update()
        {
            Follow();
        }

        /// <summary>
        /// 游标追踪
        /// </summary>
        private void Follow()
        {
            if(!destination)
            {
                return;
            }

            if(!quest.tracking)
            {
                imageCursor.gameObject.SetActive(false);
                return;
            }
            else
            {
                imageCursor.gameObject.SetActive(true);
            }

            Camera camera = CameraController.instance._camera;

            Vector2 vp = camera.WorldToViewportPoint(destination.position + Vector3.up * destinationHeight);
            Vector2 vp2Center = vp - new Vector2(0.5f, 0.5f);

            bool outside = false;

            if (vp2Center.x < -0.45f)
            {
                vp2Center *= -0.45f / vp2Center.x;
                outside = true;
            }
            if (vp2Center.x > 0.45f)
            {
                vp2Center *= 0.45f / vp2Center.x;
                outside = true;
            }
            if (vp2Center.y < -0.45f)
            {
                vp2Center *= -0.45f / vp2Center.y;
                outside = true;
            }
            if (vp2Center.y > 0.45f)
            {
                vp2Center *= 0.45f / vp2Center.y;
                outside = true;
            }

            vp = vp2Center + new Vector2(0.5f, 0.5f);

            _animator.SetBool("outside", outside);

            if (outside)
            {
                _rectTransform.position = camera.ViewportToScreenPoint(vp);

                _rectTransform.rotation = Quaternion.LookRotation(forward: Vector3.forward, _rectTransform.anchoredPosition);
            }
            else
            {
                _rectTransform.position = camera.WorldToScreenPoint(destination.position + Vector3.up * destinationHeight);
                _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
        }
    }
}
