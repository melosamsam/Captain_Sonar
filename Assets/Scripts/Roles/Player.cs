using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

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

    public List<Camera> Cameras { get { return _cameras; } }

    /// <summary>
    /// The Role assigned to the player ; Captain, First Mate, Engineer and/or Radio Operator
    /// </summary>
    public List<Role> Role { get { return _playerRoles; } }

    /// <summary>
    /// The name used to refer to the player throughout the game
    /// </summary>
    public string Name { get { return _playerName; } set { _playerName = value; }  }
    /// <summary>
    /// The assigned submarine (or team) assigned to the player
    /// </summary>
    public Submarine AssignedSubmarine { get { return _submarine; } }

    /// <summary>
    /// 
    /// </summary>
    public int Display { get { return _display; } set { _display = value; } }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        // Assign a default value if _playerName is null
        _playerName = _playerName ?? "DefaultPlayerName";
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

    // Start is called before the first frame update
    void Start()
    {
        _submarine = GetComponentInParent<Submarine>();
    }

    private void Update()
    {
        if (_cameras.Count > 1) 
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                SwitchCamera(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SwitchCamera(-1);        
        }
        _submarine = null;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gives the player a role
    /// </summary>
    /// <param name="roleName">The name of the role to assign to the player</param>
    public void AssignRole(string roleName)
    {
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

    public void EnableCamera()
    {
        // enable the camera of the first role we have
        _currentCamera = _cameras[0];
        _currentCamera.enabled = true;
    }

    /// <summary>
    /// Removes an assigned role to the player, used mostly in the lobby, while the team members are still debating on which role they want
    /// </summary>
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

    public void SwitchCamera(int direction)
    {
        int index = _cameras.IndexOf(_currentCamera);
        int newIndex = index + direction;

        _currentCamera.enabled = false; // deactivate current camera
        _currentCamera.targetDisplay = _display + 1;


        if (newIndex < 0) _currentCamera = _cameras[^1];
        else if (!(newIndex < _cameras.Count)) _currentCamera = _cameras[0];
        else _currentCamera = _cameras[newIndex];


        _currentCamera.targetDisplay = _display;
        _currentCamera.enabled = true; // activate new camera
    }

    /// <summary>
    /// Assigns the chosen submarine to the player, used in team choice at the beginning or in the event of a swap.
    /// </summary>
    /// <param name="submarine"></param>
    public void AssignSubmarine(Submarine submarine)
    {
        _submarine = submarine;
        submarine.Players.Add(this);
        Debug.Log($"{_playerName} has been assigned to the {submarine.Name} submarine.");
    }

    /// <summary>
    /// Removes the assigned submarine in case of need, for example a team swap.
    /// </summary>
    public void RemoveSubmarine()
    {
        _submarine = null;
    }

    /// <summary>
    /// Disables/enables the usage of the microphone, to communicate with its team
    /// </summary>

    #endregion
}
