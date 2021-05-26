using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class Dialog2_2_Portal : DialogEvents
    {
        public void SpawnPortal()
        {
            GameObject portalPrefab = ResourceLoader.Load("Prefabs/Portal") as GameObject;

            Instantiate(portalPrefab, new Vector3(-1, 6.5f, 0), Quaternion.identity);
        }
    }
}
