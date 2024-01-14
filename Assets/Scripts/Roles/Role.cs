using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    #region Attributes

    protected bool _actionDone;

    #endregion

    #region Properties

    /// <summary>
    /// This represents whether the Role has finished their turn
    /// </summary>
    public virtual bool ActionDone {
        get { return _actionDone; } 
        protected set 
        { 
            if (_actionDone != value)
            {
                _actionDone = value;
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

    public abstract void PerformRoleAction();

    protected abstract void SetDescription();

    protected abstract void OnActionStatusChanged();

    #endregion
}
