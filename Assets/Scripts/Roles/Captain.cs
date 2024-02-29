using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction { North, East, South, West, None }

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
    /// 
    /// </summary>
    public bool IsInitialPositionChosen { get => _isInitialPositionChosen; }

    /// <summary>
    /// 
    /// </summary>
    public Direction ChosenCourse { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public TMP_Dropdown SystemDropdown { get => _systemDropdown; set => _systemDropdown = value; }

    /// <summary>
    /// GameObject that displays when the Captain has to choose the initial position of their Submarine
    /// </summary>
    public GameObject Notification { get => _notification; set => _notification = value; }
  
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
        if (!IsTurnOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                FinishTurn();
            }
        }
    }

    #endregion

    #region Overridden methods
    protected override void OnActionStatusChanged()
    {
        ToggleUI();
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
        // closes the overlay if it is still opened
        if (_isOverlayOpen) ToggleOverlay();

        // then finishes the turn
        ToggleTurn();
    }

    protected override void ToggleUI()
    {
        Transform ui = _board.Find("Actions");

        ui.localScale = !IsTurnOver ? Vector3.one: Vector3.zero;
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
    /// 
    /// </summary>
    public void OpenPositionNotification()
    {
        _notification.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClosePositionNotification()
    {
        _notification.transform.localScale = Vector3.zero;
    }

    public void ChooseInitialPosition()
    {
        // To do once the grid is functional
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

        ChosenCourse = course;

        if (_submarine.Move(ChosenCourse))
        {
            Debug.Log($"Chosen course: {ChosenCourse}");
            FinishTurn();
        }
        else Debug.Log($"The submarine cannot go there.");
    }

    /// <summary>
    /// Calling the submarine to surface before doing anything else
    /// </summary>
    public void OrderSurface()
    {
        Debug.Log("Surface ordered");

        // call the MakeSurface() method in the Submarine class
        _submarine.MakeSurface();

        FinishTurn();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator SelectSubmarinePosition()
    {
        _isInitialPositionChosen = false;

        // OpenPositionNotification();
        yield return new WaitForSeconds(3f);
        _submarine.SetPosition(new Position(5, 5));
        // yield return new WaitUntil(() => _isInitialPositionChosen); // when UI is done
        // ClosePositionNotification();

        _isInitialPositionChosen = true;
    }

    /// <summary>
    /// Opens/closes the overlay displaying more of the Captain's actions
    /// </summary>
    public void ToggleOverlay()
    {
        GameObject overlay = null;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestCaptain"))
             overlay = GameObject.Find("Overlay");
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestGame"))
            overlay = _board.GetChild(3).GetChild(2).gameObject;

        //switch the status of the overlay
        _isOverlayOpen = !_isOverlayOpen;
        overlay.transform.localScale = _isOverlayOpen ? Vector3.one : Vector3.zero;
    }

    #endregion

}
