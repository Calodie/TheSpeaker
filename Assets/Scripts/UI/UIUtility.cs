using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardMan2D
{
    public class UIUtility : MonoBehaviour
    {
        // public GameObject[] panels;

        public UnityEvent onDisable;

        internal List<GameObject> nextPanels = new List<GameObject>();
        internal List<GameObject> nextHidePanels = new List<GameObject>();

        public void SetNextPanel(GameObject panel)
        {
            nextPanels.Add(panel);
        }

        public void SetNextHidePanel(GameObject panel)
        {
            nextHidePanels.Add(panel);
        }

        public void ShowPanel(GameObject panel)
        {
            panel.SetActive(true);
        }

        public void HidePanel(GameObject panel)
        {
            UIUtility utility = panel.GetComponent<UIUtility>();
            if (utility)
            {
                utility.onDisable.Invoke();
            }
            panel.SetActive(false);
        }

        /*
        public void SwitchPanel(GameObject panel)
        {
            panel.SetActive(true);

            foreach (GameObject otherPanel in panels)
            {
                if(otherPanel != panel)
                {
                    UIUtility utility = otherPanel.GetComponent<UIUtility>();
                    if (utility)
                    {
                        utility.onDisable.Invoke();
                    }

                    otherPanel.SetActive(false);
                }
            }

            panel.SetActive(true);
        }
        */
        public void Quit()
        {
            Application.Quit();
        }
    }
}
