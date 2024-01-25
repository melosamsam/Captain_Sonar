using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    #region Attributes

    private bool _isTurnOver = true;

    protected Submarine _submarine;

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

    #endregion

    #region Methods

    public virtual void FinishTurn()
    {
        ToggleTurn();
    }

    public abstract void PerformRoleAction();

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
