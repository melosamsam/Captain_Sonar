using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    /// <summary>
    /// This property represents the description of the specific Role
    /// </summary>
    public string Description { get; protected set; }

    /// <summary>
    /// This represents whether the Role has finished their turn
    /// </summary>
    public bool ActionDone { get; protected set; }


    public abstract void PerformRoleAction();

    public abstract string SetDescription();
}
