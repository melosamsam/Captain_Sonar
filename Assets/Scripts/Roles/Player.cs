using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents a player in the game.
/// </summary>
public class Player : MonoBehaviour
{
    #region Attributes

    private Submarine _submarine;
    [SerializeField] private List<Role> _playerRoles;

    [SerializeField] private string _playerName; // serialized for testing mostly
    [SerializeField] private List<string> _playerRoleNames; // serialized for testing mostly

    [SerializeField] private List<Camera> _cameras;
    private Camera _currentCamera;
    private int _display;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the display index.
    /// </summary>
    public int Display { get { return _display; } set { _display = value; } }

    /// <summary>
    /// Gets the list of cameras associated with the player.
    /// </summary>
    public List<Camera> Cameras { get { return _cameras; } }

    /// <summary>
    /// Gets or sets the name used to refer to the player throughout the game.
    /// </summary>
    public string Name { get { return _playerName; } set { _playerName = value; }  }

    /// <summary>
    /// Gets the list of roles assigned to the player (Captain, First Mate, etc.).
    /// </summary>
    public List<Role> Role { get { return _playerRoles; } }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        InitializePlayerRoles();
    }

    // Start is called before the first frame update
    void Start()
    {
        _submarine = GetComponentInParent<Submarine>();
    }

    private void Update()
    {
        HandleCameraSwitchInput();
        _submarine = null;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Assigns a role to the player based on the provided role name.
    /// </summary>
    /// <param name="roleName">The name of the role to assign to the player.</param>
    public void AssignRole(string roleName)
    {
        // Switch statement to instantiate the appropriate role based on the provided name
        switch (roleName)
        {
            case "Captain":
                _playerRoles.Add(gameObject.AddComponent<Captain>());
                break;

            case "First Mate":
                _playerRoles.Add(gameObject.AddComponent<FirstMate>());
                break;

            case "Engineer":
                _playerRoles.Add(gameObject.AddComponent<Engineer>());
                break;

            case "Radio Detector":
                _playerRoles.Add(gameObject.AddComponent<RadioDetector>());
                break;
        }

        _playerRoleNames.Add(roleName);
    }

    /// <summary>
    /// Assigns the chosen submarine to the player.
    /// </summary>
    /// <param name="submarine">The chosen submarine to be assigned.</param>
    public void AssignSubmarine(Submarine submarine)
    {
        _submarine = submarine;
        submarine.Players.Add(this);
        Debug.Log($"{_playerName} has been assigned to the {submarine.Name} submarine.");
    }


    /// <summary>
    /// Displays the player's username on associated cameras.
    /// </summary>
    public void DisplayUsername()
    {
        foreach (Camera cam in _cameras)
        {
            GameObject board = cam.transform.GetChild(0).gameObject;
            GameObject playerInfo = board.transform.Find("Player Info").gameObject;
            TMP_Text playerInfoText = playerInfo.GetComponent<TMP_Text>();

            playerInfoText.text = _playerName;
        }
    }

    /// <summary>
    /// Enables the first camera and initializes the current camera.
    /// </summary>
    public void EnableCamera()
    {
        // enable the camera of the first role we have
        _currentCamera = _cameras[0];
        _currentCamera.enabled = true;
    }

    /// <summary>
    /// Removes an assigned role from the player.
    /// </summary>
    /// <param name="role">The name of the role to be removed.</param>
    public void RemoveRole(string role) 
    {
        int toRemove = 0;
        foreach (Role assignedRole in GetComponents<Role>())
        {
            if (role.Equals(assignedRole.Name))
            {
                Destroy(assignedRole);
                break;
            }
            toRemove++;
        }

        _playerRoleNames.RemoveAt(toRemove);
        _playerRoles.RemoveAt(toRemove);
    }

    /// <summary>
    /// Removes the assigned submarine from the player.
    /// </summary>
    public void RemoveSubmarine()
    {
        _submarine = null;
    }

    /// <summary>
    /// Switches the player's active camera based on the input direction.
    /// </summary>
    /// <param name="direction">The direction of the camera switch (-1 for left, 1 for right).</param>
    public void SwitchCamera(int direction)
    {
        int index = _cameras.IndexOf(_currentCamera);
        int newIndex = index + direction;

        // Deactivate current camera
        _currentCamera.enabled = false;
        _currentCamera.targetDisplay = _display + 1;

        // Set the new index within bounds or loop back to the first/last camera
        if (newIndex < 0) _currentCamera = _cameras[^1];
        else if (!(newIndex < _cameras.Count)) _currentCamera = _cameras[0];
        else _currentCamera = _cameras[newIndex];

        // Activate new camera
        _currentCamera.targetDisplay = _display;
        _currentCamera.enabled = true;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Handles input for switching cameras.
    /// </summary>
    void HandleCameraSwitchInput()
    {
        if (_cameras.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                SwitchCamera(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SwitchCamera(-1);
        }
    }

    /// <summary>
    /// Initializes player roles based on components attached to the player object.
    /// </summary>
    void InitializePlayerRoles()
    {
        // Assign a default value if _playerName is null
        _playerName ??= "DefaultPlayerName";
        _playerName = _playerName.Trim(' ');

        _playerRoleNames = new List<string>();
        _playerRoles = new List<Role>();
        _cameras = new List<Camera>();

        // Ensure there is at least one Role component attached
        Role[] roles = GetComponents<Role>();
        if (roles.Length > 0)
        {
            foreach (Role role in roles)
            {
                if (role != null)
                {
                    _playerRoles.Add(role);
                    _playerRoleNames.Add(role.Name);
                }
            }
        }
        else
        {
            // Log a warning or handle the case where no Role components are found
            Debug.LogWarning("No Role components found on the player object.");
        }
    }

    #endregion
}
