using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class SpriteDictionary : MonoBehaviour
    {
        public static SpriteDictionary instance;

        [SerializeField]
        private Sprite[] sprites;
        [SerializeField]
        private Sprite unknownSprite;

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

            sprites = Resources.LoadAll<Sprite>("Textures");
        }

        public Sprite FindSpriteWithName(string spriteName)
        {
            foreach (Sprite sprite in sprites)
            {
                if (sprite.name == spriteName)
                {
                    return sprite;
                }
            }
            return unknownSprite;
        }
    }
}

