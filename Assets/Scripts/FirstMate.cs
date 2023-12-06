using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMate : Role
{
    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        SetDescription();        
    }

    // Start is called before the first frame update
    void Start()
    {
        PerformRoleAction();
    }

    #endregion

    #region To override

    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    public override void PerformRoleAction()
    {
        // setting the turn as not done when it just began
        ActionDone = false;
        Debug.Log("First Mate role started\n" + Description);
    }

    protected override void SetDescription()
    {
        Description =
            "The First Mate is responsible for filling the gauges of the submarine's systems;\n" +
            "- weapons,\n" +
            "- detection systems and ,\n" +
            "- stealth function.\r\n" +
            "He is also responsible for notifying the Captain when they are filled and therefore ready for use.\n" +
            "It is also them who trigger, in connection with the Radio Detector, the detection systems: Sonar and Drone."
            ;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Activates the selected system if it is ready
    /// </summary>
    /// <param name="system">Name of the system to activate</param>
    public void ActivateSystem(string system)
    {
        // if (System.IsReady)
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="system">Name of the system which gauge must be filled</param>
    public void FillGauge(string system)
    {
        // if (!System.IsReady)

    }

    #endregion

    #region Private methods

    void ToggleUI()
    {

    }

    #endregion
}
