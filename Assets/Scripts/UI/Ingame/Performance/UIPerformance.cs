using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIPerformance : MonoBehaviour
    {
        public Text textUnitCount;
        public Text textFPS;

        private void Update()
        {
            ShowPerformance();
        }

        private void ShowPerformance()
        {
            textUnitCount.text = "Unit: " + Unit.units.Count.ToString();
            textFPS.text = "FPS: " + ((int)(1 / Time.deltaTime)).ToString();
        }
    }
}

