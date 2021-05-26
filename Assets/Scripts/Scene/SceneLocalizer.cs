using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardMan2D
{
    public class SceneLocalizer : MonoBehaviour
    {
        public static SceneLocalizer instance;

        private FigureNodeMono[] figureNodeMonos;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            // 加载所有节点
            GameObject[] objs = Resources.LoadAll<GameObject>("Prefabs/Scenes");
            figureNodeMonos = new FigureNodeMono[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                figureNodeMonos[i] = objs[i].GetComponent<FigureNodeMono>();
            }
        }

        /// <summary>
        /// 寻找通向一场景的最短路径
        /// </summary>
        /// <param name="targetScene"></param>
        /// <returns></returns>
        public List<FigureNode> FindRouteToScene(string targetScene)
        {
            // 找到起点
            FigureNodeMono currentNode = null;
            foreach (FigureNodeMono figureNodeMono in figureNodeMonos)
            {
                if (figureNodeMono.name == SceneManager.GetActiveScene().name)
                {
                    currentNode = figureNodeMono;
                    break;
                }
            }
            if(!currentNode)
            {
                return null;
            }

            return currentNode.FindRouteToNode(targetScene);
        }
    }
}
