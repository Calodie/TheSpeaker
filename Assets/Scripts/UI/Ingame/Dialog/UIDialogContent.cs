using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIDialogContent : MonoBehaviour
    {
        public Text text;

        public float letterPerS;

        private List<char> text2Show;

        public void SetText(string text)
        {
            text2Show = new List<char>(text.ToCharArray());
            StopAllCoroutines();
            StartCoroutine(ShowTextCoro());
        }

        private IEnumerator ShowTextCoro()
        {
            text.text = "";
            while (text2Show.Count > 0)
            {
                text.text += text2Show[0];
                text2Show.RemoveAt(0);
                yield return new WaitForSeconds(1 / letterPerS);
            }
        }
    }
}
