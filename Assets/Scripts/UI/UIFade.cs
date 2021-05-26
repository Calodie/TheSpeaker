using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIFade : MonoBehaviour
    {
        public float fadeTime;

        private MaskableGraphic[] graphics;

        private float fadeTick = 0;

        private void Awake()
        {
            graphics = GetComponentsInChildren<MaskableGraphic>();
        }

        public void FadeIn()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeInCoro());
        }

        private IEnumerator FadeInCoro()
        {
            gameObject.SetActive(true);

            fadeTick = 0;

            while (fadeTick < fadeTime)
            {
                fadeTick += Time.deltaTime;

                foreach (MaskableGraphic graphic in graphics)
                {
                    Color color = graphic.color;
                    color.a = Mathf.Clamp(fadeTick / fadeTime, 0, 1);
                    graphic.color = color;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void FadeOut()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeOutCoro());
        }

        private IEnumerator FadeOutCoro()
        {
            fadeTick = fadeTime;

            while (fadeTick > 0)
            {
                fadeTick -= Time.deltaTime;

                foreach (MaskableGraphic graphic in graphics)
                {
                    Color color = graphic.color;
                    color.a = Mathf.Clamp(fadeTick / fadeTime, 0, 1);
                    graphic.color = color;
                }

                yield return new WaitForEndOfFrame();
            }

            gameObject.SetActive(false);
        }
    }
}
