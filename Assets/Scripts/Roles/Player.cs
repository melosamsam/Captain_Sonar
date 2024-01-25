using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    #region Attributes

    private Submarine _submarine; // to replace with "Submarine" when implemented
    [SerializeField] private List<Role> _playerRoles;

    [SerializeField] private string _playerName; // serialized for testing mostly
    [SerializeField] private List<string> _playerRoleNames; // serialized for testing mostly
    [SerializeField] private TMP_Text _playerInfo;

    [SerializeField] private List<Camera> _cameras;
    private Camera _currentCamera;

    #endregion

    #region Properties

    /// <summary>
    /// The Role assigned to the player ; Captain, First Mate, Engineer and/or Radio Operator
    /// </summary>
    public List<Role> Role { get { return _playerRoles; } }

    /// <summary>
    /// The name used to refer to the player throughout the game
    /// </summary>
    public string Name { get { return _playerName; } }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        _playerName = _playerName.Trim(' ');

        _playerRoleNames = new List<string>();
        _playerRoles = new List<Role>();
        _cameras = new List<Camera>();

        foreach (Role role in GetComponents<Role>())
        {
            _playerRoles.Add(role);
            _playerRoleNames.Add(role.Name);
        }

        _playerInfo.text = _playerName;
    }

    // Start is called before the first frame update
    void Start()
    {
        _submarine = GetComponentInParent<Submarine>();

        // get cameras corresponding to the roles
        foreach (Role role in _playerRoles)
            _cameras.Add(GameObject.Find($"{_submarine.Color} {role.Name}").GetComponent<Camera>());

        // disables all cameras available in the scene
        foreach (Camera camera in GameObject.FindObjectsOfType<Camera>())
            camera.enabled = false;

        Debug.Log("All cameras disabled");

        // enable the camera of the first role we have
        _currentCamera = _cameras[0];
        _currentCamera.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            SwitchCamera(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SwitchCamera(-1);
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

        if (newIndex < 0) _currentCamera = _cameras[^1];
        else if (!(newIndex < _cameras.Count)) _currentCamera = _cameras[0];
        else _currentCamera = _cameras[newIndex];

        _currentCamera.enabled = true; // activate new camera
    }

    #endregion
}
