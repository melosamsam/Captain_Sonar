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
    public List<Camera> cameras;

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

        IsMicOpen = false;

        _playerRoleNames = new List<string>();
        _playerRoles = new List<Role>();

        foreach (Role role in GetComponents<Role>())
        {
            _playerRoles.Add(role);
            _playerRoleNames.Add(role.Name);
        }

        _playerInfo.text = _playerName;
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
