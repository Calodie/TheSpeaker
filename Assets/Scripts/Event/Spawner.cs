using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawns;

    public Transform parent;

    public void Spawn()
    {
        foreach (GameObject go in spawns)
        {
            if (parent)
            {
                GameObject spawned = Instantiate(go, parent.position, Quaternion.identity);
                spawned.transform.SetParent(parent);
            }
            else
            {
                Instantiate(go, transform.position, Quaternion.identity);
            }
        }
    }
}
