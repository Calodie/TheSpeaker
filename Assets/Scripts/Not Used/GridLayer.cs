using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridLayer : MonoBehaviour
{
    public Sprite spriteA;
    public Sprite spriteB;
    public BoundsInt area;

    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        Tile tile = TileFromSprite(spriteA);
        for (int index = 0; index < tileArray.Length; index++)
        {
            tileArray[index] = tile;
        }
        _tilemap.SetTilesBlock(area, tileArray);
    }

    public Tile TileFromSprite(Sprite sprite)
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        return tile;
    }
}
