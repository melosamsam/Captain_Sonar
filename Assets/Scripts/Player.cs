using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    #region Attributes

    private GameObject _submarine; // to replace with "Submarine" when implemented
    private Role _playerRole; // change to a List when the lobby is implemented

    [SerializeField] private string _playerName; // serialized for testing mostly
    [SerializeField] private string _playerRoleName; // serialized for testing mostly
    [SerializeField] private TMP_Text _playerInfo;

    public bool IsMicOpen;

    #endregion

    #region Constructor

    public Player(string playerName)
    {
        _playerName = playerName.Trim(' ');

        IsMicOpen = false;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The Role assigned to the player ; Captain, First Mate, Engineer or Radio Operator
    /// </summary>
    public Role Role { get { return _playerRole; } }

    /// <summary>
    /// The name used to refer to the player throughout the game
    /// </summary>
    public string Name { get { return _playerName; } }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        AssignRole(_playerRoleName.ToLower());

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
            case "captain":
                _playerRole = gameObject.AddComponent<Captain>();
                break;

            case "first mate":
                _playerRole = gameObject.AddComponent<FirstMate>();
                break;

            case "engineer":
                _playerRole = gameObject.AddComponent<Engineer>();
                break;
        }
    }

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

    /// <summary>
    /// Removes an assigned role to the player, used mostly in the lobby, while the team members are still debating on which role they want
    /// </summary>
    public void RemoveRole() 
    { 
        _playerRole = null;
        Destroy(gameObject.GetComponent<Role>());
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
