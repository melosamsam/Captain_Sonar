using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Submarine : MonoBehaviour
{
    #region Attributes

    // state of submarine
    private int _health;
    private bool _isSubmerged;
    [SerializeField] private int _maxHealth;
    private int _nbOfTurnsSurfaced;

    // state of the submarine's crew
    private string _name;
    private string _color;
    [SerializeField] private List<Player> _players;

    // positions of the submarine
    private Direction _currentCourse;
    private Position _currentPosition;
    private int[,] _trail;

    // shared variables
    private Map _gameMap; // taken from the GameManager or another GameObject

    #endregion

    #region Constants

    // health
    const int NORMAL_MODE_HEALTH    = 1;
    const int HUNT_MODE_HEALTH      = 4;

    // general
    const int TURNS_SURFACED        = 3;

    // map
    const int UNEXPLORED_POSITION   = 0;
    const int PAST_POSITION         = 1;

    #endregion

    #region Properties

    /// <summary>
    /// The color used to represents the Submarine as a team
    /// </summary>
    public string Color { get => _color; }

    /// <summary>
    /// The Direction the Captain has ordered the Submarine to go towards
    /// </summary>
    public Direction CurrentCourse { get { return _currentCourse; } set { _currentCourse = value; } }

    /// <summary>
    /// Current position of the Submarine on the selected map
    /// </summary>
    public Position CurrentPosition { get => _currentPosition; } // readonly, moving the Submarine should only be executed within the class

    /// <summary>
    /// The amount of health the submarine currently has
    /// </summary>
    public int Health { get { return _health; } }

    /// <summary>
    /// Whether the submarine is currently under water or not
    /// </summary>
    public bool IsSubmerged { get { return _isSubmerged; } }

    /// <summary>
    /// The amount of health the submarine starts the game with
    /// </summary>
    public int MaxHealth { get { return _maxHealth; } }

    /// <summary>
    /// The name the players chose to refer to their crew
    /// </summary>
    public string Name { get => _name; set => _name = value; }

    /// <summary>
    /// The players constituting the Submarin's crew
    /// </summary>
    public List<Player> Players { get => _players; } // readonly, there shouldn't be any reason to change the formation of the team mid-game

    /// <summary>
    /// Matrix representing where the submarine has already gone within the game's Map 
    /// </summary>
    public int[,] Trail { get => _trail; }

    /// <summary>
    /// Number of turns during which the submarine has been on the surface
    /// </summary>
    public int TurnsSurfaced { get { return _nbOfTurnsSurfaced; } set { _nbOfTurnsSurfaced = value; } }

    #endregion

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Team_Role_Setup"))
        {
            _maxHealth          = GameManager.Instance.IsNormalMode ? NORMAL_MODE_HEALTH : HUNT_MODE_HEALTH;
            _health             = _maxHealth; // we start the game at full health
            _isSubmerged        = true;
            _currentCourse      = Direction.None;

            _color              = gameObject.name.Split(' ')[0];
            _players            = GetComponentsInChildren<Player>().ToList(); // take the players chosen from the lobby available right before
            _nbOfTurnsSurfaced  = 0;
            _gameMap            = GameManager.Instance.MainMap;

            if (_gameMap != null)
                _trail = new int[_gameMap.GetMap().GetLength(0), _gameMap.GetMap().GetLength(1)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Team_Role_Setup"))
        {
            if (!(_nbOfTurnsSurfaced <= TURNS_SURFACED))
            {
                _nbOfTurnsSurfaced = 0;
                ToggleSubmersion();
            }

            if (_health <= 0) Die();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Method called when the Captain decides to make their team's submarine surface, as a way to reset the submarine's state (failures, etc.)
    /// </summary>
    public void MakeSurface()
    {
        ToggleSubmersion();
        UnityEngine.Debug.Log($"The submarine has made surface at position ({_currentPosition.x}, {_currentPosition.y}).\n" +
            $"The members of {_name} are unable to act during {TURNS_SURFACED - _nbOfTurnsSurfaced} turns");
    }

    /// <summary>
    /// Updates the position of the submarine of the game's Map according to the Captain's decision
    /// </summary>
    /// <param name="course">The course/direction chosen by the Captain</param>
    public bool Move(Direction course)
    {
        Position newPosition = _currentPosition;

        switch (course)
        {
            case Direction.North:
                newPosition.x--;
                break;

            case Direction.South:
                newPosition.x++;
                break;

            case Direction.East:
                newPosition.y++;
                break;

            case Direction.West:
                newPosition.y--;
                break;
        }

        bool canSubmarineMove = IsPathClear(newPosition);

        if (canSubmarineMove)
        {
            _trail[_currentPosition.x, _currentPosition.y] = PAST_POSITION; // mark position as past position in history of past positions
            _currentPosition = newPosition; // then update the submarine's position
        }

        return canSubmarineMove;
    }

    /// <summary>
    /// Inflicts damage to the Submarine
    /// </summary>
    /// <param name="damage">Amount of damage to hit the Submarine, 1 by default</param>
    public void TakeDamage(int damage=1)
    {
        if (_health > 0) _health -= damage;
        UnityEngine.Debug.Log($"The submarine has just taken {damage} damage(s). It can now only take {_health} more hits.");
    }


    #endregion

    #region Private methods

    void Die()
    {
        UnityEngine.Debug.Log("The submarine has been killed");
        GameManager.Instance.EndGame();
    }
   
    bool IsPathClear(Position positionToCheck)
    {
        return !((_gameMap.GetMap()[positionToCheck.x, positionToCheck.y] != UNEXPLORED_POSITION) || 
                    (_trail[positionToCheck.x, positionToCheck.y] == PAST_POSITION));
    }

    void ToggleSubmersion()
    {
        _isSubmerged = !_isSubmerged;
    }

    #endregion
}
