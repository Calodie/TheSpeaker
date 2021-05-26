using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public static class ResourceLoader
    {
        public static Dictionary<string, object> LoadedResources { get; private set; } = new Dictionary<string, object>();

        public static object Load(string path)
        {
            if(LoadedResources.ContainsKey(path))
            {
                return LoadedResources[path];
            }
            else
            {
                object res = Resources.Load(path);
                LoadedResources.Add(path, res);

                return res;
            }
        }
    }
}
