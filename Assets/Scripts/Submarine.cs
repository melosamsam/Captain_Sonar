using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    #region Attributes

    // state of submarine
    private int _health;
    private bool _isSubmerged;
    private int _maxHealth;
    private int _nbOfTurnsSurfaced;

    // state of the submarine's crew
    private string _name;
    private string _color;
    private List<Player> _players;

    // positions of the submarine
    private Captain.Direction _currentCourse;
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
    /// 
    /// </summary>
    public string Color { get => _color; }

    /// <summary>
    /// The Direction the Captain has ordered the Submarine to go towards
    /// </summary>
    public Captain.Direction CurrentCourse { get { return _currentCourse; } set { _currentCourse = value; } }

    /// <summary>
    /// Current position of the Submarine on the selected map
    /// </summary>
    public Position CurrentPosition { get => _currentPosition; } // readonly, moving the Submarine should only be executed within the class

    /// <summary>
    /// The amount of health the submarine currently has
    /// </summary>
    public int Health { get { return _health; } set { _health = value; } }

    /// <summary>
    /// Whether the submarine is currently under water or not
    /// </summary>
    public bool IsSubmerged { get { return _isSubmerged; } set { _isSubmerged = value; } }

    /// <summary>
    /// The amount of health the submarine starts the game with
    /// </summary>
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    /// <summary>
    /// The name the players chose to refer to their crew
    /// </summary>
    public string Name { get => _name; } // readonly, no reason to change the team's name mid-game

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
    public int TurnsSurfaced { get { return _nbOfTurnsSurfaced; } set { _nbOfTurnsSurfaced = (value >= 0 && value <= 3) ? value: 0; } }

    #endregion

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        _maxHealth          = GameManager.Instance.IsNormalMode ? NORMAL_MODE_HEALTH : HUNT_MODE_HEALTH;
        _health             = _maxHealth; // we start the game at full health
        _isSubmerged        = true;
        _currentCourse      = Captain.Direction.None;

        _color              = gameObject.name.Split(' ')[0];
        _players            = GetComponentsInChildren<Player>().ToList(); // take the players chosen from the lobby available right before

        _nbOfTurnsSurfaced  = 0;

        _gameMap            = GameManager.Instance.MainMap;

        _trail              = new int[_gameMap.GetMap().GetLength(0), _gameMap.GetMap().GetLength(1)];

    }

    // Update is called once per frame
    void Update()
    {
        if (!(_nbOfTurnsSurfaced < TURNS_SURFACED)) ToggleSubmersion();

        if (_health <= 0) Die();
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
    public void Move(Captain.Direction course)
    {
        Position newPosition = _currentPosition;

        switch (course)
        {
            case Captain.Direction.North:
                newPosition.x--;
                break;
            case Captain.Direction.South:
                newPosition.x++;
                break;
            case Captain.Direction.East:
                newPosition.y++;
                break;
            case Captain.Direction.West:
                newPosition.y--;
                break;
        }

        if (IsPathClear(newPosition))
        {
            _trail[_currentPosition.x, _currentPosition.y] = PAST_POSITION; // mark position as past position in history of past positions
            _currentPosition = newPosition; // then update the submarine's position
        }
    }

    /// <summary>
    /// Inflicts damage to the Submarine
    /// </summary>
    /// <param name="damage">Amount of damage to hit the Submarine</param>
    public void TakeDamage(int damage)
    {
        if (_health > 0) _health -= damage;
        UnityEngine.Debug.Log($"The submarine has just taken {damage} damage(s). It can now only take {_health} more hits.");
    }


    #endregion

    #region Private methods

    private void Die()
    {
        UnityEngine.Debug.Log("The submarine has been killed");
        GameManager.Instance.EndGame();
    }
   
    private bool IsPathClear(Position positionToCheck)
    {
        return !((_gameMap.GetMap()[positionToCheck.x, positionToCheck.y] != UNEXPLORED_POSITION) || (_trail[positionToCheck.x, positionToCheck.y] == PAST_POSITION));
    }

    private void ToggleSubmersion()
    {
        _isSubmerged = !_isSubmerged;
    }

    #endregion
}
