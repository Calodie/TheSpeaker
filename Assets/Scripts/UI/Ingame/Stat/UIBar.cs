using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIBar : MonoBehaviour
    {
        /// <summary>
        /// 对应rect
        /// </summary>
        [HideInInspector]
        public RectTransform _rectTransform;

        /// <summary>
        /// 条数字显示器
        /// </summary>
        public Text text;

        /// <summary>
        /// 条内圈
        /// </summary>
        public Image innerBar;
        /// <summary>
        /// 条边宽
        /// </summary>
        public float edgeWidth;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 条赋值
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="maxValue"></param>
        public void SetValue(float currentValue, float maxValue)
        {
            // 设置显示数字
            if (text)
            {
                text.text = ((int)currentValue).ToString() + "/" + ((int)maxValue).ToString();
            }

            // 设置条内圈长度
            if (innerBar)
            {
                innerBar.rectTransform.anchoredPosition = Vector2.right * edgeWidth;

                float innerBarWidth = _rectTransform.sizeDelta.x - edgeWidth * 2;
                maxValue = Mathf.Max(maxValue, 1);
                float rate = Mathf.Clamp(currentValue / maxValue, 0, 1);
                innerBar.rectTransform.sizeDelta = new Vector2(innerBarWidth * rate, -edgeWidth * 2);
            }
        }
    }
}
