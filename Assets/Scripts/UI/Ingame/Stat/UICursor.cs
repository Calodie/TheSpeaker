using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UICursor : MonoBehaviour
    {
        /// <summary>
        /// 提示器显示组
        /// </summary>
        public Text[] texts;

        /// <summary>
        /// 当前所指unit
        /// </summary>
        private Unit unit;
        /// <summary>
        /// 所指是否可交互
        /// </summary>
        private Interactable interactable;

        private void FixedUpdate()
        {
            HideMouse();
        }

        private void Update()
        {
            FollowCursor();
            ShowInteractable();
        }

        private void HideMouse()
        {
            Cursor.visible = false;
        }

        /// <summary>
        /// 跟随鼠标
        /// </summary>
        private void FollowCursor()
        {
            transform.position = Input.mousePosition;
        }

        /// <summary>
        /// 设置鼠标提示器
        /// </summary>
        /// <param name="interactable"></param>
        /// <param name="canInteract"></param>
        public void SetPointingUnit(Unit unit, Interactable interactable = null)
        {
            this.unit = unit;
            this.interactable = interactable;
        }

        /// <summary>
        /// 提示器显示
        /// </summary>
        private void ShowInteractable()
        {
            if(!unit || unit.Killed)
            {
                texts[0].text = "";
                texts[1].text = "";
                texts[2].text = "";
                return;
            }

            texts[0].text = unit.unitName;

            if (!unit._unitStatsManager.immortal)
            {
                texts[1].text = (int)unit._unitStatsManager.hp + "/" + (int)unit._unitStatsManager.maxHp;

                if(interactable)
                {
                    texts[2].text = GetInteractableText(interactable.interactableType);
                }
                else
                {
                    texts[2].text = "";
                }

            }
            else
            {
                if (interactable)
                {
                    texts[1].text = GetInteractableText(interactable.interactableType);
                }
                else
                {
                    texts[1].text = "";
                }

                texts[2].text = "";
            }
        }

        private string GetInteractableText(InteractableTypes interactableType)
        {
            string text = "按\"E\"";
            switch (interactableType)
            {
                case InteractableTypes.none:
                    text += "交互";
                    break;
                case InteractableTypes.use:
                    text += "使用";
                    break;
                case InteractableTypes.speak:
                    text += "交流";
                    break;
                case InteractableTypes.enter:
                    text += "进入";
                    break;
                case InteractableTypes.leave:
                    text += "离开";
                    break;
                case InteractableTypes.open:
                    text += "打开";
                    break;
                case InteractableTypes.close:
                    text += "关闭";
                    break;
                case InteractableTypes.check:
                    text += "查看";
                    break;
            }

            return text;
        }
    }
}
