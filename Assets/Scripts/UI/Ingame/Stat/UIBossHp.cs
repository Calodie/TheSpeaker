using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIBossHp : MonoBehaviour
    {
        [HideInInspector]
        public RectTransform _rectTransform;

        [SerializeField]
        private UIBar barHp;

        [SerializeField]
        private Image avator;

        [SerializeField]
        private Text textName;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(string bossName)
        {
            avator.sprite = SpriteDictionary.instance.FindSpriteWithName(bossName + "_128");
            textName.text = bossName;
        }

        public void SetHp(float hp, float maxHp)
        {
            barHp.SetValue(hp, maxHp);
        }
    }
}

