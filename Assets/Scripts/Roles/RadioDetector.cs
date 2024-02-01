using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RadioDetector : Role
{

    #region Attributes

    private bool _isSeeThroughOpen;
    private bool _isGridOpen;
    private GameObject _seeThrough;
    private GameObject _Grid;
    public enum Direction { North, East, South, West, None }
    private Position currentpos = new Position(9,9);

    #endregion

    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public GameObject SeeThrough { get; set; }   

    #endregion

    #region Overridden methods

    /// <summary>
    /// Method called when the turn of the Captain finishes or ends.
    /// </summary>
    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    protected override void SetDescription()
    {
        Name = "Radio Detector";
        Description =
            "The captain is the central element of the entire crew.\n" +
            "In addition to being responsible for the trajectory taken by the submarine, they must be the link between all other posts."
            ;
    }

    /// <summary>
    /// Method enabling/disabling the UI elements used to perform role actions according to whether the turn is done or not.
    /// </summary>
    protected override void ToggleUI()
    {
        GameObject canvas = GameObject.Find("Canvas");
        canvas.transform.localScale = !IsTurnOver ? Vector3.one : Vector3.zero;
    }


    #endregion

    #region Unity methods

    private void Awake()
    {
        SetDescription();
    }

    // Start is called before the first frame update
    void Start()
    {
        _seeThrough = GameObject.Find("See through");
        _Grid = GameObject.Find("Grid");
        _isSeeThroughOpen = false;
        _isGridOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSeeThroughOpen)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ToggleSeeThrough();
            }
        }
    }

    #endregion

    #region Methods

    public void ToggleSeeThrough()
    {
        _isSeeThroughOpen = !_isSeeThroughOpen;
        Debug.Log($"See through visible: {_isSeeThroughOpen}");
        _seeThrough.transform.localScale = _isSeeThroughOpen ? Vector3.one : Vector3.zero;
        //_isGridOpen = !_isGridOpen;
        //_Grid.transform.localScale = _isGridOpen ? new Vector3(1, 90, 5) : Vector3.zero;
    }

    #endregion

    public void OrderSubmarineCourse(string courseChar)
    {
        Position retour = new Position(0, 0);
        switch (courseChar)
        {
            case "N":
                retour.x = currentpos.x;
                retour.y = currentpos.y - 1;
                break;
            case "S":
                retour.x = currentpos.x;
                retour.y = currentpos.y + 1;
                break;
            case "E":
                retour.x = currentpos.x + 1;
                retour.y = currentpos.y;
                break;
            case "W":
                retour.x = currentpos.x - 1;
                retour.y = currentpos.y;
                break;
        }
        //on a la position a aller 
        //on a la position où on est.
        //dans les 4 cas de direction on fait un Find Gameobject pour récupérer l'id du truc.
        //s'il est apparent on le fait disparaitre et inversement.
        //s'il va vers le Nord et finalement nan, s'il va vers le sud ensuite, le tracé change d'etat (apparait si pas apparent et disparait si apparant)

        
    }

    public void Trace(Position pos, string direction)
    {
        Position retour=new Position(0,0);
        if (direction == "N")
        {
            retour.x = pos.x;
            retour.y = pos.y - 1;
        }
        else
        {
            if(direction == "S") {
                retour.x = pos.x;
                retour.y = pos.y + 1;
            }
            else
            {
                if(direction=="E")
                {
                    retour.x = pos.x + 1;
                    retour.y = pos.y;
                }
                else
                {
                    retour.x = pos.x - 1;
                    retour.y = pos.y;
                }
            }
        }
        //display tracé du trait correspondant avec coordo position en param et new position.
    }
}
