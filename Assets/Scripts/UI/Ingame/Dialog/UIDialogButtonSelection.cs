using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIDialogButtonSelection : MonoBehaviour
    {
        public Text text;

        public Dialog dialog;
        public Selection selection;

        public void Initialize(Dialog dialog, int selectionId)
        {
            this.dialog = dialog;
            selection = dialog.selections[selectionId];
            text.text = selection.content;
        }

        private void LateUpdate()
        {
            if (selection.autoSelect)
            {
                Select();
            }
        }

        public void Select()
        {
            selection.onSelect.Invoke();

            UIDialog.instance.EndDialog();

            UIDialog.instance.SetDialog(selection.nextDialog);

            if(selection.continueDialog)
            {
                UIDialog.instance.StartDialog();
            }
        }
    }
}
