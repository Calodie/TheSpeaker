using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardMan2D
{
    public enum DialogSpriteLocation
    {
        Left, Right
    }

    [System.Serializable]
    public struct Paragraph
    {
        /// <summary>
        /// 段落讲述者
        /// </summary>
        public string speaker;
        /// <summary>
        /// 头像位置
        /// </summary>
        public DialogSpriteLocation spriteLocation;
        /// <summary>
        /// 内容
        /// </summary>
        public string content;
        /// <summary>
        /// 触发事件
        /// </summary>
        public UnityEvent onSpeak;
        /// <summary>
        /// 不可重复触发事件
        /// </summary>
        public bool singleEvent;
    }

    [System.Serializable]
    public struct Selection
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string content;
        /// <summary>
        /// 下接段落
        /// </summary>
        public Dialog nextDialog;
        /// <summary>
        /// 不中断对话直接启动下一段落
        /// </summary>
        public bool continueDialog;
        /// <summary>
        /// 自动选择该选项（用于在最后一段落结束后的触发事件）
        /// </summary>
        public bool autoSelect;
        /// <summary>
        /// 触发事件，如果autoSelect为true则直接触发
        /// </summary>
        public UnityEvent onSelect;
    }

    // [CreateAssetMenu(menuName = "ScriptableObjects/Dialog")]
    public class Dialog : MonoBehaviour //ScriptableObject
    {
        /// <summary>
        /// 讲述人
        /// </summary>
        public string speaker;

        [Header("对话")]
        public Paragraph[] paragraphs;

        [Header("选项")]
        public Selection[] selections;
    }
}
