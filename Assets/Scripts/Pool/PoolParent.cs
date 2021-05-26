using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class PoolParent : MonoBehaviour
    {
        private void OnDestroy()
        {
            foreach (PoolObject pObj in GetComponentsInChildren<PoolObject>())
            {
                Pool.instance.Destroy(pObj.gameObject);
            }
        }
    }
}

