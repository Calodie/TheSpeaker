using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UITextHit : MonoBehaviour
    {
        [HideInInspector]
        public float damage;

        [HideInInspector]
        public Vector3 worldPos;

        public Text text;

        public float lifeTime;

        public Color colorHurt, colorHeal;

        private void Start()
        {
            text.text = ((int)damage).ToString();
        }

        private void Update()
        {
            transform.position = CameraController.instance._camera.WorldToScreenPoint(worldPos);
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

