using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaptainGridManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Tile _tileHPrefab;

    private GameObject _Parent;

    #endregion

    #region Unity methods

    void Start()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TestGame"))
        {
            _Parent = transform.parent.Find("TileGrid").gameObject;
            GenerateGrid(_Parent);
        }
    }

    #endregion

    #region Methods

    public void GenerateGrid(GameObject parent)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //var spawnedTile = Instantiate(_tilePrefab, new Vector3((float)(-50+117*x), (float)(-85*y+1221),0), Quaternion.identity, parent.transform);
                var spawnedTile = Instantiate(_tilePrefab);
                spawnedTile.transform.parent = parent.transform;
                spawnedTile.transform.rotation = Quaternion.identity;
                spawnedTile.transform.localPosition = new Vector3((float)(-750 + 117 * x), (float)(-85 * y + 285), (float)(0.0001102686));
                spawnedTile.transform.localScale = new Vector3(10, 100, 10);
                spawnedTile.name = $"Tile {x} {y}";
                //var spawnedTileH = Instantiate(_tileHPrefab, new Vector3((float)(10 + 117 * x), (float)(-85 * y + 1262), 0), Quaternion.Euler(0, 0, 90), parent.transform);
                var spawnedTileH = Instantiate(_tileHPrefab);
                spawnedTileH.transform.parent = parent.transform;
                spawnedTileH.transform.rotation = Quaternion.Euler(0, 0, 90);
                spawnedTileH.transform.localPosition = new Vector3((float)(-690 + 117 * x), (float)(-85 * y + 326), (float)(0.0001102686));
                spawnedTileH.transform.localScale = new Vector3(10, 100, 10);
                spawnedTileH.name = $"TileH {x} {y}";

            }
        }

    }

    #endregion
}
