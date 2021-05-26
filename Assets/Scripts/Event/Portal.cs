using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class Portal : MonoBehaviour
    {
        public static List<Portal> portals = new List<Portal>();

        public static Portal FindPortalToScene(string sceneName)
        {
            foreach (Portal portal in portals)
            {
                if (portal.sceneName == sceneName)
                {
                    return portal;
                }
            }
            return null;
        }

        /// <summary>
        /// 传送目的场景
        /// </summary>
        public string sceneName;

        private void Awake()
        {
            portals.Add(this);
        }

        private void OnDestroy()
        {
            portals.Remove(this);
        }

        public void EnterPortal()
        {
            UITopUtility.instance.LoadScene(sceneName);
        }
    }
}
