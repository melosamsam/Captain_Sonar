using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    #region attributs
    public int x;
    public int y;
    #endregion

    #region Get
    public int GetHorizontalPosition() { return x; }
    public int GetVerticalPosition() { return y; }

    #endregion

    #region Set
    public void SetHorizontalPosition(int positionh) { x = positionh; }
    public void SetVerticalPosition(int positionv) { x = positionv; }
    #endregion

    #region Constructeurs
    //c'etait initialise � 0 mais je pense que c'�tait une erreur
    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"({x}; {y})";
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
