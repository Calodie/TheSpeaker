using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIDialogSelections : MonoBehaviour
    {
        public UIDialogButtonSelection[] buttons;

        public Transform anchor;

        public float buttonHeight;

        public void SetSelections(Dialog dialog)
        {
            foreach(UIDialogButtonSelection button in buttons)
            {
                Destroy(button.gameObject);
            }

            buttons = new UIDialogButtonSelection[dialog.selections.Length];

            GameObject buttonPrefab = ResourceLoader.Load("Prefabs/UI/UIDialogButtonSelectionPrefab") as GameObject;
            for (int i = 0; i < dialog.selections.Length; i++)
            {
                UIDialogButtonSelection button = Instantiate(buttonPrefab, anchor).GetComponent<UIDialogButtonSelection>();
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, (buttonHeight * dialog.selections.Length / 2) - (0.5f + i) * buttonHeight);
                rectTransform.localScale = Vector3.one;

                button.Initialize(dialog, i);

                buttons[i] = button;
            }
        }
    }
}
