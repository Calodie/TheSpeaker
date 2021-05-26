using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardMan2D
{
    public class UITopUtility : UIUtility
    {
        public static UITopUtility instance;

        public GameObject panelFade;

        public float switchTime;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoro(sceneName));
        }

        private IEnumerator LoadSceneCoro(string sceneName)
        {
            UIFade uIFade = panelFade.GetComponent<UIFade>();

            uIFade.fadeTime = Mathf.Min(0.5f, switchTime / 2);
            uIFade.FadeIn();
            yield return new WaitForSeconds(switchTime);
            GameManager.instance.LoadScene(sceneName);
            foreach (GameObject panel in nextPanels)
            {
                ShowPanel(panel);
            }
            nextPanels = new List<GameObject>();
            foreach (GameObject panel in nextHidePanels)
            {
                HidePanel(panel);
            }
            nextHidePanels = new List<GameObject>();
            uIFade.FadeOut();
        }
    }
}
