using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Tile _tileHPrefab;

    private GameObject _Parent;
    void Start()
    {
        _Parent = GameObject.Find("TileGrid");
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for(int x= 0; x < _width; x++)
        {
            for(int y= 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3((float)(-50+117*x), (float)(-85*y+1221),0), Quaternion.identity, _Parent.transform);
                spawnedTile.name = $"Tile {x} {y}";
                var spawnedTileH = Instantiate(_tileHPrefab, new Vector3((float)(10 + 117 * x), (float)(-85 * y + 1262), 0), Quaternion.Euler(0, 0, 90), _Parent.transform);
                spawnedTileH.name = $"TileH {x} {y}";

            }
        }
    }
}
