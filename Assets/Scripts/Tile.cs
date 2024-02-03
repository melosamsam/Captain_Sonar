using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool IsActive=false;
    private Renderer _renderer;
    [SerializeField] private Color _baseColor, _clickColor;

    private void Start()
    {
        _renderer = this.GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        IsActive = !IsActive;
        _renderer.material.color = IsActive ? _baseColor : _clickColor;
    }

}
