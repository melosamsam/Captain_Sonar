using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Attributes

    static GameManager _instance;

    List<Submarine> _submarines;
    Submarine _currentSubmarine;

    Player _currentPlayer;
    Role _currentRole;

    bool _isTurnBased;
    bool _isNormalMode;
    bool _isGameOver;

    Map _mainMap;

    // constants
    readonly string[] ROLE_ORDER = { "Captain", "FirstMate", "Engineer" };

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
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameOver)
        {
            if (IsTurnBased)
            {
                StartCoroutine(ProcessTurnByTurn());
            }
        }
    }

    #endregion

    #region Public methods

    public void EndGame()
    {
        _isGameOver = true;
        // lead to a recap scene maybe and offer to go back to lobby 
    }

    #endregion

    #region Private methods

    void InitiliazeBoards()
    {

    }

    void InitializeGame()
    {
        _submarines = new List<Submarine>();
        _currentSubmarine = null;
        _currentPlayer = null;
        _currentRole = null;
        _isTurnBased = true;
        _isNormalMode = true;
        _isGameOver = false;
    }

    IEnumerator ProcessTurnByTurn()
    {
        SwitchToNextRole();
        _currentRole.PerformRoleAction();

        // Attendre la fin du tour du rôle
        yield return new WaitUntil(() => !_currentRole.IsTurnOver);
    }

    void SwitchToNextTeam()
    {
        int index = _submarines.IndexOf(_currentSubmarine);
        if (index >= 0)
        {
            if (index < _submarines.Count) _currentSubmarine = _submarines[index++];
            else _currentSubmarine = _submarines[0];
        }
    }

    void SwitchToNextRole()
    {
        if (_currentRole == null)
        {
            _currentRole = _currentSubmarine.gameObject.GetComponentInChildren<Captain>() ;
        }
        else
        {
            int index = ROLE_ORDER.ToList().IndexOf(_currentRole.Name);
            if (index < ROLE_ORDER.Length)
            {
                string nextRole = ROLE_ORDER[index++];
                foreach (Role role in _currentSubmarine.gameObject.GetComponentsInChildren<Role>())
                {
                    if (role.Name == nextRole)
                    {
                        _currentRole = role;
                        break;
                    }
                }
            }
            else
            {
                _currentRole = null;
                SwitchToNextTeam();
            }
        }
    }

    #endregion


}
