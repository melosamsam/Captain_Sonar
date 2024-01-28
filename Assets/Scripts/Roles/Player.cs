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

    public bool IsMicOpen;

    #endregion

    #region Properties

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


    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        // Assign a default value if _playerName is null
        _playerName = _playerName ?? "DefaultPlayerName";
        _playerName = _playerName.Trim(' ');

        IsMicOpen = false;

        _playerRoleNames = new List<string>();
        _playerRoles = new List<Role>();

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

        if (_playerInfo != null)
        {
            _playerInfo.text = _playerName;
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

    /*
    /// <summary>
    /// Allows the player to perform their designated actions for their Role
    /// </summary>
    public void PerformRoleAction()
    {
        string displayedName = _playerName[^1] == 's' ? _playerName + '\'' : _playerName + "\'s";
        Debug.Log($"It is {displayedName} turn.");
        _playerRole.PerformRoleAction();
        Debug.Log($"{_playerName} has finished their turn.");
    }
    */

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
    public void ToggleMic()
    {
        if (IsMicOpen) 
        {
            IsMicOpen = false;
            Debug.Log("The microphone is now turned off.");
            /* do things to close the mic and disable communication */ 
        }
        else 
        {
            IsMicOpen = true;
            Debug.Log("The microphone is now turned on.");
            /* do things to open the mic and enable communication */ 
        }
    }

    #endregion
}
