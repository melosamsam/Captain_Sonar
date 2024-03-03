using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
/// Add Team Manager methods, variables and sync -> sub1 and sub2 in _submarines, no current player and add role management.
/// Only one player per scene can join submarines
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Attributes

    static GameManager _instance;

    [SerializeField] List<Submarine> _submarines;
    [SerializeField] Submarine _currentSubmarine;
    Submarine _losingSubmarine = null;

    [SerializeField] Player _currentPlayer;
    [SerializeField] Role _currentRole;

    List<Camera> _cameras;

    // Game settings
    bool _isTurnBased;
    float _timePerTeam;

    bool _isNormalMode;
    bool _isGameOver;

    [SerializeField] int _mapNumber;
    [SerializeField] Map _mainMap;

    // Constants
    readonly string[] ROLE_ORDER = { "Captain", "First Mate", "Engineer" };

    public GameObject submarinePrefab;
    public GameObject playerPrefab;
    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance of GameManager.
    /// </summary>
    static public GameManager Instance { get => _instance; }

    /// <summary>
    /// The current player in the game.
    /// </summary>
    public Player CurrentPlayer { get => _currentPlayer; }
    
    /// <summary>
    /// The current role in the game.
    /// </summary>
    public Role CurrentRole { get => _currentRole; }

    /// <summary>
    /// Indicates whether the game is over.
    /// </summary>
    public bool IsGameOver {  get => _isGameOver; }

    /// <summary>
    /// Indicates whether the game is in normal mode.
    /// </summary>
    public bool IsNormalMode {  get => _isNormalMode; }

    /// <summary>
    /// Indicates whether the game is turn-based.
    /// </summary>
    public bool IsTurnBased {  get { return _isTurnBased; } }

    /// <summary>
    /// The main map used in the game.
    /// </summary>
    public Map MainMap { get => _mainMap; }

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    void Awake()
    {
        InitializeSingleton();
        AwakeGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        UpdateGameLogic();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Starts the game and initializes necessary components.
    /// </summary>
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

        _mainMap = new Map(_mapNumber, !_isTurnBased);
    }

    /// <summary>
    /// Ends the game and determines the losing team.
    /// </summary>
    public void EndGame()
    {
        _isGameOver = true;
        _losingSubmarine = _currentSubmarine;
        Debug.Log($"The {_losingSubmarine.Color} team lost the game!");
        // lead to a recap scene maybe and offer to go back to lobby 
    }

    #endregion

    #region Private methods

    void AwakeGame()
    {
        _submarines = FindObjectsOfType<Submarine>().ToList();
        _currentSubmarine = null;
        _currentPlayer = null;
        _currentRole = null;
        _isTurnBased = true;
        _isNormalMode = true;
        _isGameOver = true;
        _timePerTeam = 45;

        _mainMap = new(_mapNumber, !_isTurnBased);
        _cameras = FindObjectsOfType<Camera>().ToList();

        foreach (Submarine sub in _submarines)
            sub.GameMap = _mainMap;
    }

    IEnumerator ChooseInitialPositions()
    {
        foreach (Submarine submarine in _submarines)
        {
            Captain captain = submarine.GetComponentInChildren<Captain>();
            _currentRole = captain;
            GetPlayerFromRole();
            if (captain != null && !captain.IsInitialPositionChosen)
            {
                Debug.Log($"{submarine.Color} Captain, player {_currentPlayer.Name}, is choosing submarine position...");
                yield return captain.SelectSubmarinePosition();
                Debug.Log($"{submarine.Color} Captain, player {_currentPlayer.Name}, has chosen the submarine position!");
            }
        }
        _currentPlayer = null;
        _currentRole = null;

        if (_isTurnBased) StartCoroutine(ProcessTurnByTurn());
        //else ProcessRealTime();
    }

    void DisableCameras()
    {
        // disables all cameras available in the scene
        foreach (Camera camera in _cameras)
            camera.enabled = false;

        Debug.Log("All cameras disabled");

        foreach (Submarine sub in _submarines)
            foreach (Player player in sub.Players)
            {
                player.Cameras[0].enabled = true;
                player.Display = player.Cameras[0].targetDisplay;
            }
    }

    void DistributeCameras()
    {
        foreach (Submarine submarine in _submarines)
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

                    Transform board = role.Board;
                    submarine.Timer.TimerText.Add(board.Find("Timer").GetComponentInChildren<TMP_Text>());
                    submarine.Timer.SetStartingTime(_timePerTeam * 60);
                }
            }
        }
    }

    void InitializeGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateSubmarines();
            InstantiatePlayer();
        }

        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("TestGame"))) 
            InitializeGameLogic();

    }

    void InitializeGameLogic()
    {
        StartGame();
        _currentSubmarine = _submarines[UnityEngine.Random.Range(0, 2)];

        InitializeBoards();

        // start the game
        _isGameOver = false;

        // Captains choose their initial position before the game
        StartCoroutine(ChooseInitialPositions());
    }

    void InitializeSingleton()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TestGame"))
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
        }
    }

    void InstantiatePlayer()
    {
        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        Player playerScript = playerObject.GetComponent<Player>();
        playerScript.Name = "SomePlayerName";
    }

    void InstantiateSubmarines()
    {
        // Instantiate submarines and add them to the _submarines list
        GameObject submarine1Object = PhotonNetwork.Instantiate(submarinePrefab.name,
            Vector3.zero, Quaternion.identity);
        submarine1Object.name = "RedTeam";
        submarine1Object.GetComponent<Submarine>().Name = "RedTeam";
        _submarines.Add(submarine1Object.GetComponent<Submarine>());

        GameObject submarine2Object = PhotonNetwork.Instantiate(submarinePrefab.name, Vector3.zero, Quaternion.identity);
        submarine2Object.name = "BlueTeam";
        submarine2Object.GetComponent<Submarine>().Name = "BlueTeam";
        _submarines.Add(submarine2Object.GetComponent<Submarine>());
    }

    IEnumerator ProcessTurnByTurn()
    {
        // all roles' turn is over (disabled) right before the game starts
        foreach (Submarine sub in _submarines)
        {
            foreach (Player player in sub.Players)
                foreach (Role role in player.Role) role.ToggleTurn();
        }


        while (!_isGameOver)
        {
            _currentSubmarine.Timer.StartCountdown();
            SwitchToNextRole();
            Debug.Log($"{_currentSubmarine.Color} team's {_currentRole.Name}, player {_currentPlayer.Name}, is playing.");
            yield return _currentRole.PerformRoleAction();
        }

        Debug.Log("Game is over!");
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
                // increment index to get the next Role
                index++;
                string nextRole = ROLE_ORDER[index];

                // Look for the next Role in the Submarine's children components
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

    void SwitchToNextTeam()
    {
        _currentSubmarine.Timer.StopCountdown(); // stop the countdown before switching teams
        int index = _submarines.IndexOf(_currentSubmarine);
        _currentSubmarine = index == 0 ? _submarines[1] : _submarines[0];

        _currentSubmarine.Timer.StartCountdown(); // start the new team's countdown
        SwitchToNextRole();
    }

    void UpdateGameLogic()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("TestGame")))
        {
            if (!_currentSubmarine.IsSubmerged)
            {
                _currentSubmarine.TurnsSurfaced++;
                _currentPlayer = null;
                SwitchToNextTeam();
            }
        }
    }

    #endregion
}
