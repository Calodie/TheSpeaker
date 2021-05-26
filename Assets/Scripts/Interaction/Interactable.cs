using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardMan2D
{
    public enum InteractableTypes
    {
        none, speak, use, enter, leave, open, close, check
    }

    [RequireComponent(typeof(Unit))]
    public class Interactable : MonoBehaviour
    {
        public static List<Interactable> interactables = new List<Interactable>();

        /// <summary>
        /// 对应unit
        /// </summary>
        [HideInInspector]
        public Unit _unit;

        /// <summary>
        ///  交互类型
        /// </summary>
        [Header("交互类型")]
        public InteractableTypes interactableType;

        /// <summary>
        /// 交互触发事件
        /// </summary>
        public UnityEvent onInteract;

        /// <summary>
        /// 最大交互距离
        /// </summary>
        [Header("最大交互距离")]
        public float maxDistance;

        private void Awake()
        {
            interactables.Add(this);
            _unit = GetComponent<Unit>();
        }

        private void OnDestroy()
        {
            interactables.Remove(this);
        }

        public void Interact()
        {
            onInteract.Invoke();
        }
    }
}
