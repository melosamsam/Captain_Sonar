using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Captain : Role
{

    #region Attributes

    public enum Direction { North, East, South, West, None }

    private bool _isOverlayOpen;
    private TMP_Dropdown _systemDropdown;
    private List<LogItem> _logItems;

    #endregion

    #region Properties

    public Direction ChosenCourse { get; private set; }
  
    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {


        SetDescription();
    }

    // Start is called before the first frame update
    void Start()
    {
        _systemDropdown = GameObject.Find("Systems dropdown").GetComponent<TMP_Dropdown>();

        PerformRoleAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Overridden methods

    /// <summary>
    /// Method called when the turn of the Captain finishes or ends.
    /// </summary>
    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    public override void PerformRoleAction()
    {
        // setting the turn as not done when it just began
        //ToggleTurn();
        Debug.Log("Captain role started\n" + Description);
    }

    protected override void SetDescription()
    {
        Name = "Captain";
        Description =
            "The captain is the central element of the entire crew.\n" +
            "In addition to being responsible for the trajectory taken by the submarine, they must be the link between all other posts."
            ;
    }

    public override void FinishTurn()
    {
        // close the overlay if it is still opened
        if (_isOverlayOpen) ToggleOverlay();

        // then finished the turn
        ToggleTurn();
    }

    /// <summary>
    /// Method enabling/disabling the UI elements used to perform role actions according to whether the turn is done or not.
    /// </summary>
    protected override void ToggleUI()
    {
        GameObject ui = GameObject.Find("Actions");

        ui.transform.localScale = !IsTurnOver? Vector3.one: Vector3.zero;
    }


    #endregion

    #region Public methods

    /// <summary>
    /// Activates the selected system if it is ready
    /// </summary>
    /// <param name="system">Name of the system to activate</param>
    public void ActivateSystem()
    {
        string system = "";
        string[] systems = { "...", "Mine", "Torpedo", "Sonar", "Drone" } ;

        if (_systemDropdown.value != 0)
            system = systems[_systemDropdown.value];

        // switch() for each case of chosen system
        // verification made in Systems.cs
        Debug.Log($"{system} activated.");
        FinishTurn();
    }


    /// <summary>
    /// Updates the Captain's given course to direct the whereabouts of the submarine.
    /// </summary>
    /// <param name="courseChar">The letter indicating which direction to go</param>
    public void OrderSubmarineCourse(string courseChar)
    {
        Direction course = Direction.None;
        switch (courseChar)
        {
            case "N":
                course = Direction.North;
                break;
            case "S":
                course = Direction.South;
                break;
            case "E":
                course = Direction.East;
                break;

            case "W":
                course = Direction.West;
                break;
        }

        // if course in Submarine.GetPossibleCourses
        ChosenCourse = course;
        // else, show error message and allow another input

        Debug.Log($"Chosen course: {ChosenCourse}");
        FinishTurn();
    }

    /// <summary>
    /// Calling the submarine to surface before doing anything else
    /// </summary>
    public void OrderSurface()
    {
        Debug.Log("Surface ordered");
        // call the Surface() method in the Submarine class


        FinishTurn();
    }

    /// <summary>
    /// Opens/closes the overlay displaying more of the Captain's actions
    /// </summary>
    public void ToggleOverlay()
    {
        GameObject overlay = GameObject.Find("Overlay");

        //switch the status of the overlay
        _isOverlayOpen = !_isOverlayOpen;
        overlay.transform.localScale = _isOverlayOpen ? Vector3.one : Vector3.zero;
    }

    #endregion

}
