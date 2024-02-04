using System.Collections;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    #region Attributes

    private bool _isTurnOver = true;

    protected Submarine _submarine;

    protected Transform _board;

    #endregion

    #region Properties

    /// <summary>
    /// This represents whether the Role has finished their turn
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
    /// This property represents the description of the specific Role
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// The name of the Role
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Reference to the Board used by the Role
    /// </summary>
    public Transform Board { get => _board; set { _board = value; } }

    #endregion

    #region Methods

    public virtual void FinishTurn()
    {
        ToggleTurn();
    }

    public virtual IEnumerator PerformRoleAction()
    {
        ToggleTurn();
        Debug.Log($"{Name} role started\n" + Description);
        Debug.Log($"Turn has started?: {!IsTurnOver}");

        yield return new WaitUntil(() => IsTurnOver);
    }

    protected abstract void SetDescription();

    protected virtual void OnActionStatusChanged()
    {
        ToggleUI();
    }

    protected abstract void ToggleUI();

    public void ToggleTurn()
    {
        IsTurnOver = !IsTurnOver;
        if (IsTurnOver)
            Debug.Log(Name + "'s turn is over");
    }

    #endregion
}
