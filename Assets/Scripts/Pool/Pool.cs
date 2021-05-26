using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class Pool : MonoBehaviour
    {
        public static Pool instance;

        public GameObject[] prefabs;

        /// <summary>
        /// 对象池大小
        /// </summary>
        public int poolSize;

        /// <summary>
        /// 对象队列
        /// </summary>
        private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);
            }
            else
            {
                instance = this;

            }
        }

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        private void Initialize()
        {
            foreach (GameObject prefab in prefabs)
            {
                GameObject[] prefabArray = new GameObject[poolSize];
                for (int i = 0; i < poolSize; i++)
                {
                    prefabArray[i] = Object.Instantiate(prefab, transform);
                    prefabArray[i].SetActive(false);
                }
                pools[prefab] = new Queue<GameObject>(prefabArray);
            }
        }

        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject Instantiate(GameObject go)
        {
            GameObject prefab = pools[go].Dequeue();
            prefab.transform.SetParent(null);
            prefab.transform.position = Vector3.zero;
            prefab.transform.rotation = Quaternion.identity;
            prefab.SetActive(true);
            return prefab;
        }

        /// <summary>
        /// 生成对象
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject go, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = pools[go].Dequeue();
            prefab.transform.SetParent(null);
            prefab.transform.position = position;
            prefab.transform.rotation = rotation;
            prefab.SetActive(true);
            return prefab;
        }

        /// <summary>
        /// 生成对象
        /// </summary>
        public GameObject Instantiate(GameObject go, Transform parent)
        {
            GameObject prefab = pools[go].Dequeue();
            prefab.transform.SetParent(parent);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = Quaternion.identity;
            prefab.SetActive(true);
            return prefab;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        public void Destroy(GameObject go)
        {
            foreach(GameObject prefab in pools.Keys)
            {
                if(prefab.GetType().ToString() == go.GetType().ToString())
                {
                    pools[prefab].Enqueue(go);
                    go.transform.SetParent(transform);
                    go.SetActive(false);
                    break;
                }
            }
        }
    }
}

