using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonBind : MonoBehaviour
{
    /// <summary>
    /// 绑定按键
    /// </summary>
    [Header("绑定按键")]
    public KeyCode bindedKey;

    [HideInInspector]
    public Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(bindedKey))
        {
            _button.onClick.Invoke();
        }
    }
}
