using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum representing cardinal directions and none.
/// </summary>
public enum Direction 
{ 
    North, 
    East, 
    South, 
    West, 
    None 
}

/// <summary>
/// Represents the role of Captain in the game.
/// </summary>
public class Captain : Role
{
    #region Attributes

    bool _isInitialPositionChosen;
    bool _isOverlayOpen;

    TMP_Dropdown _systemDropdown;
    GameObject _notification;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the chosen course for the submarine's movement.
    /// </summary>
    public Direction ChosenCourse { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the initial position has been chosen by the Captain.
    /// </summary>
    public bool IsInitialPositionChosen { get => _isInitialPositionChosen; }

    /// <summary>
    /// Gets or sets the dropdown UI element for selecting submarine systems.
    /// </summary>
    public TMP_Dropdown SystemDropdown { get => _systemDropdown; set => _systemDropdown = value; }
  
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
        _submarine = GetComponentInParent<Submarine>();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestCaptain"))
            _systemDropdown = GameObject.Find("Systems dropdown").GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    #endregion

    #region Overridden methods

    /// <summary>
    /// Finishes the Captain's turn, closing the overlay if open.
    /// </summary>
    public override void FinishTurn()
    {
        // Closes the overlay if it is still opened
        if (_isOverlayOpen) ToggleOverlay();

        // Then finishes the turn
        ToggleTurn();
    }

    /// <summary>
    /// Handles the change in action status by toggling UI elements.
    /// </summary>
    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    /// <summary>
    /// Sets the description and name of the Captain role.
    /// </summary>
    protected override void SetDescription()
    {
        Name = "Captain";
        Description =
            "The captain is the central element of the entire crew.\n" +
            "In addition to being responsible for the trajectory taken by the submarine, they must be the link between all other posts."
            ;
    }

    /// <summary>
    /// Toggles the visibility of UI elements based on the Captain's turn status.
    /// </summary>
    protected override void ToggleUI()
    {
        Transform ui = _board.Find("Actions");

        ui.localScale = !IsTurnOver ? Vector3.one: Vector3.zero;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Activates the selected system if it is ready.
    /// </summary>
    public void ActivateSystem()
    {
        string system = "";
        string[] systems = { "...", "Mine", "Torpedo", "Sonar", "Drone" } ;

        if (_systemDropdown.value != 0)
            system = systems[_systemDropdown.value];

        // Switch statement for each case of the chosen system
        // Verification made in Systems.cs
        Debug.Log($"{system} activated.");
        FinishTurn();
    }

    /// <summary>
    /// Chooses the initial position of the submarine.
    /// </summary>
    public void ChooseInitialPosition()
    {
        // To do once the grid is functional
    }

    /// <summary>
    /// Updates the Captain's given course to direct the whereabouts of the submarine.
    /// </summary>
    /// <param name="courseChar">The letter indicating which direction to go.</param>
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

        ChosenCourse = course;

        if (_submarine.Move(ChosenCourse))
        {
            Debug.Log($"Chosen course: {ChosenCourse}");
            FinishTurn();
        }
        else Debug.Log($"The submarine cannot go there.");
    }

    /// <summary>
    /// Orders the submarine to surface before performing any other action.
    /// </summary>
    public void OrderSurface()
    {
        Debug.Log("Surface ordered");

        // Call the MakeSurface() method in the Submarine class
        _submarine.MakeSurface();

        FinishTurn();
    }

    /// <summary>
    /// Selects the submarine's initial position with a delay;
    /// </summary>
    /// <returns>An IEnumerator for coroutine use.</returns>
    public IEnumerator SelectSubmarinePosition()
    {
        _isInitialPositionChosen = false;

        yield return new WaitForSeconds(3f);
        _submarine.SetPosition(new Position(5, 5));
        // yield return new WaitUntil(() => _isInitialPositionChosen); // when UI is done

        _isInitialPositionChosen = true;
    }

    /// <summary>
    /// Toggles the overlay displaying more of the Captain's actions.
    /// </summary>
    public void ToggleOverlay()
    {
        GameObject overlay = null;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestCaptain"))
             overlay = GameObject.Find("Overlay");
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestGame"))
            overlay = _board.Find("Actions").Find("Overlay").gameObject;

        // Switch the status of the overlay
        _isOverlayOpen = !_isOverlayOpen;
        overlay.transform.localScale = _isOverlayOpen ? Vector3.one : Vector3.zero;
    }

    #endregion

    #region Private methods

    void HandleInput()
    {
        if (!IsTurnOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                FinishTurn();
            }
        }
    }

    #endregion
}
