using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Add Team Manager methods, variables and sync -> sub1 and sub2 in _submarines, no current player and add role management.
    /// Only one player per scene can join submarines
    /// </summary>
    #region Attributes

    static GameManager _instance;

    [SerializeField] List<Submarine> _submarines;
    [SerializeField] Submarine _currentSubmarine;

    [SerializeField] Player _currentPlayer;
    [SerializeField] Role _currentRole;

    List<Camera> _cameras;

    // game settings
    bool _isTurnBased;
    bool _isNormalMode;
    bool _isGameOver;

    [SerializeField] int _mapNumber;
    Map _mainMap;

    // constants
    readonly string[] ROLE_ORDER = { "Captain", "First Mate", "Engineer" };

    #endregion

    #region Properties

    /// <summary>
    /// Singleton
    /// </summary>
    static public GameManager Instance { get { return _instance; } }

    /// <summary>
    /// 
    /// </summary>
    public Player CurrentPlayer { get { return _currentPlayer; } }

    /// <summary>
    /// 
    /// </summary>
    public Role CurrentRole { get { return _currentRole; } }

    /// <summary>
    /// 
    /// </summary>
    public bool IsGameOver {  get { return _isGameOver; } }

    /// <summary>
    /// 
    /// </summary>
    public bool IsNormalMode {  get { return _isNormalMode; } }

    /// <summary>
    /// 
    /// </summary>
    public bool IsTurnBased {  get { return _isTurnBased; } }

    /// <summary>
    /// 
    /// </summary>
    public Map MainMap { get { return _mainMap; } }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.GetComponent<GameManager>().enabled = true;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than one GameManager in the scene");
            return;
        }
        InitializeGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameras = FindObjectsOfType<Camera>().ToList();
        StartGame();
    }

    #endregion

    #region Public methods

    public void StartGame()
    {
        DistributeCameras();
        DisableCameras();

        foreach (Submarine submarine in _submarines)
        {
            foreach (Player player in submarine.Players)
            {
                player.DisplayUsername();
                player.EnableCamera();
            }
        }

        InitializeBoards();

        // randomly chooses which team starts first
        _currentSubmarine = _submarines[UnityEngine.Random.Range(0, 2)];

        // start the game
        _isGameOver = false;
        if (_isTurnBased) ProcessTurnByTurn();
        //else ProcessRealTime();
    }

    public void EndGame()
    {
        _isGameOver = true;
        // lead to a recap scene maybe and offer to go back to lobby 
    }

    #endregion

    #region Private methods

    void DisableCameras()
    {
        // disables all cameras available in the scene
        foreach (Camera camera in _cameras)
            camera.enabled = false;

        Debug.Log("All cameras disabled");
    }

    void DistributeCameras()
    {
        foreach(Submarine submarine in _submarines)
        {
            foreach (Player player in submarine.Players)
            {
                // get cameras corresponding to the roles
                foreach (Role role in player.gameObject.GetComponents<Role>())
                    player.Cameras.Add(GameObject.Find($"{submarine.Color} {role.Name}").GetComponent<Camera>());
            }
        }
    }

    void GetPlayerFromRole()
    {
        _currentPlayer = _currentRole != null ? _currentRole.gameObject.GetComponent<Player>() : null;
    }

    void InitializeBoards()
    {
        foreach (Submarine submarine in _submarines)
        {
            foreach (Player player in submarine.Players)
            {
                foreach (Role role in player.Role)
                {
                    Board.Initialize(_mainMap, submarine, role);
                }
            }
        }
    }

    void InitializeGame()
    {
        _submarines = FindObjectsOfType<Submarine>().ToList();
        _currentSubmarine = null;
        _currentPlayer = null;
        _currentRole = null;
        _isTurnBased = true;
        _isNormalMode = true;
        _isGameOver = true;
        _mainMap = new(_mapNumber, !_isTurnBased);
    }

    void ProcessTurnByTurn()
    {
        StartCoroutine(TurnByTurnCoroutine());
    }

    void SwitchToNextTeam()
    {
        int index = _submarines.IndexOf(_currentSubmarine);
        _currentSubmarine = index == 0 ? _submarines[index++] : _submarines[0];
        SwitchToNextRole();
    }

    void SwitchToNextRole()
    {
        if (_currentRole == null)
        {
            _currentRole = _currentSubmarine.gameObject.GetComponentInChildren<Captain>();
        }
        else
        {
            int index = Array.IndexOf(ROLE_ORDER, _currentRole.Name);
            if (index >= 0 && index < ROLE_ORDER.Length - 1)
            {
                // Incr�mentez l'index pour passer au prochain r�le
                index++;
                string nextRole = ROLE_ORDER[index];

                // Cherchez le prochain r�le dans les composants enfants du sous-marin
                Role[] roles = _currentSubmarine.gameObject.GetComponentsInChildren<Role>();
                _currentRole = Array.Find(roles, role => role.Name == nextRole);
            }
            else
            {
                _currentRole = null;
                SwitchToNextTeam();
            }
        }

        GetPlayerFromRole();
    }

    IEnumerator TurnByTurnCoroutine()
    {
        while (!_isGameOver)
        {
            SwitchToNextRole();
            Debug.Log($"{_currentSubmarine.Color} team's {_currentRole.Name}, player {_currentPlayer.Name}, is playing.");
            yield return StartCoroutine(_currentRole.PerformRoleAction());
            //StopCoroutine(_currentRole.PerformRoleAction());
        }

        Debug.Log("Game is over!");
    }

    #endregion


}
