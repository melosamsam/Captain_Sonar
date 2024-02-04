using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
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
        if (parent.transform.localScale != Vector3.one)
            parent.transform.localScale = Vector3.one;

        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3((float)(-50+117*x), (float)(-85*y+1221),0), Quaternion.identity, parent.transform);
                spawnedTile.name = $"Tile {x} {y}";
                var spawnedTileH = Instantiate(_tileHPrefab, new Vector3((float)(10 + 117 * x), (float)(-85 * y + 1262), 0), Quaternion.Euler(0, 0, 90), parent.transform);
                spawnedTileH.name = $"TileH {x} {y}";

            }
        }

        parent.transform.localScale = Vector3.zero;
    }

    #endregion

}
