using System.Collections;
using UnityEngine;

/// <summary>
/// Abstract base class representing a role in the game.
/// </summary>
public abstract class Role : MonoBehaviour
{
    #region Attributes

    bool _isTurnOver = false;

    protected Submarine _submarine;

    protected Transform _board;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the reference to the Board used by the Role.
    /// </summary>
    public Transform Board { get => _board; set { _board = value; } }

    /// <summary>
    /// Gets or sets the description of the specific Role.
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// Indicates whether the Role has finished their turn.
    /// </summary>
    public virtual bool IsTurnOver {
        get { return _isTurnOver; } 
        protected set 
        { 
            if (_isTurnOver != value)
            {
                _isTurnOver = value;
                OnActionStatusChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the name of the Role.
    /// </summary>
    public string Name { get; protected set; }

    #endregion

    #region Methods

    /// <summary>
    /// Finishes the turn of the Role.
    /// </summary>
    public virtual void FinishTurn()
    {
        ToggleTurn();
    }

    /// <summary>
    /// Called when the action status (turn state) changes. 
    /// </summary>
    protected virtual void OnActionStatusChanged()
    {
        ToggleUI();
    }

    /// <summary>
    /// Performs the action associated with the Role asynchronously.
    /// </summary>
    /// <returns>An enumerator for asynchronous execution.</returns>
    public virtual IEnumerator PerformRoleAction()
    {
        ToggleTurn();
        Debug.Log($"{Name} role started\n" + Description);
        Debug.Log($"Turn has started?: {!IsTurnOver}");

        yield return new WaitUntil(() => IsTurnOver);
    }

    /// <summary>
    /// Sets the description for the specific Role.
    /// </summary>
    protected abstract void SetDescription();

    /// <summary>
    /// Toggles the turn status of the Role.
    /// </summary>
    public void ToggleTurn()
    {
        IsTurnOver = !IsTurnOver;
        if (IsTurnOver)
            Debug.Log(Name + "'s turn is over");
    }

    /// <summary>
    /// Toggles the UI elements used to perform role actions based on the turn status.
    /// </summary>
    protected abstract void ToggleUI();

    #endregion
}
