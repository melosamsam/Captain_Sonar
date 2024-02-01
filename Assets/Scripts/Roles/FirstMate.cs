using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMate : Role
{
    #region Attributes

    [SerializeField] private List<string> _filledGauges = new();

    #endregion

    #region Unity methods

    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        SetDescription();        
    }

    // Start is called before the first frame update
    void Start()
    {
        // PerformRoleAction();
    }

    private void Update()
    {
        if (!IsTurnOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                FinishTurn();
            }
        }
    }

    #endregion

    #region To override

    protected override void OnActionStatusChanged()
    {
        ToggleUI();
    }

    public override IEnumerator PerformRoleAction()
    {
        ToggleTurn(); // setting the turn as not done when it just began
        _filledGauges.Clear(); // resetting the inventory of filled gauges during last turn

        Debug.Log("First Mate role started\n" + Description);

        yield return new WaitUntil(() => IsTurnOver);
    }

    protected override void SetDescription()
    {
        Name = "First Mate";
        Description =
            "The First Mate is responsible for filling the gauges of the submarine's systems;\n" +
            "- weapons,\n" +
            "- detection systems and ,\n" +
            "- stealth function.\r\n" +
            "He is also responsible for notifying the Captain when they are filled and therefore ready for use.\n" +
            "It is also them who trigger, in connection with the Radio Detector, the detection systems: Sonar and Drone."
            ;
    }

    protected override void ToggleUI()
    {
        // Hide UI if turn is over, show it otherwise
        GameObject.Find("Systems").transform.localScale = IsTurnOver ? new Vector3(0,0,0) : new Vector3(1,1,1);
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
        Systems currentSystem = GameObject.Find(system + " System").GetComponent<Systems>();
        if (currentSystem != null )
        {
            if (currentSystem.GaugeFull())
            {
                Debug.Log($"{system} has been activated");
            }
        }
    }

    /// <summary>
    /// Fills the gauge of a system specified by name.
    /// </summary>
    /// <param name="system">Name of the system which gauge must be filled</param>
    public void FillGauge(string system)
    {
        // Get the script of the System corresponding to the `system` string
        if (GameObject.Find(system + " System").TryGetComponent<Systems>(out var currentSystem))
        {
            int[] newGauge = currentSystem.GetQuotaJauge(); // copy the gauge

            if (_filledGauges.Count == 0)
            {
                if (!_filledGauges.Contains(system)) // if the Engineer didn't already fill that gauge during their turn
                {
                    if (!currentSystem.GaugeFull())
                    {
                        _filledGauges.Add(system); // mark the system as filled in our List
                        int toFill = Array.IndexOf(newGauge, 0); // get the 1st case that hasn't been filled yet
                        newGauge[toFill] = 1; // fill up the new gauge by 1
                        currentSystem.FillGauge(); // set the new gauge as the system's one
                        Debug.Log($"{system} has been filled by 1");
                    }
                    else if (currentSystem.GetColourSystem() == "green") // The First Mate can only activate Location systems
                    {
                        Debug.Log($"{system} is full, the gauge can't be filled anymore");
                        Debug.Log($"Do you want to activate the system {system}?");
                        if (Input.GetKeyDown(KeyCode.Y))
                            ActivateSystem(system);
                        else
                            Debug.Log($"{system} system has not been activated");
                    }
                    else
                        Debug.Log($"{system}'s gauge is full, but you can't activate this system. Ask your Captain!");
                }
            }
            else if (_filledGauges.Contains(system)) // if the gauge has already been filled
            {
                int toRemove = Array.LastIndexOf(newGauge, 1);
                newGauge[toRemove] = 0;
                currentSystem.EmptyGauge();
                _filledGauges.Remove(system);
                Debug.Log($"{system} has been decremented by 1");
            }
            else
            {
                Debug.Log($"You have already filled another gauge this turn, undo this action to fill the {system}'s gauge");
            }
        }

    }

    #endregion
}
