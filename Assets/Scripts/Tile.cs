using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool IsActive=false; //has the tile been clicked ?
    private Renderer _renderer;
    [SerializeField] private Color _baseColor, _clickColor; //the colors of the tile when it hasn't/has been clicked

    private void Start()
    {
        _renderer = this.GetComponent<Renderer>();
    }

    /// <summary>
    /// When tile is clicked, color changes
    /// </summary>
    private void OnMouseDown()
    {
        IsActive = !IsActive;
        _renderer.material.color = IsActive ? _baseColor : _clickColor;
    }

}
