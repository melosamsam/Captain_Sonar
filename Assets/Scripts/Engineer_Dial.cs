using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class EngineerDial : MonoBehaviour
{
    #region Attributes
    public string nameDial;
    public EngineerDialData dialData;
    #endregion

    #region Getters
    public string GetNameDial() { return nameDial; }
    public DialObjectData[] GetSystemsInDial() { return dialData.tab; }
    #endregion

    #region Setters
    public void SetNameDial(string name) { nameDial = name; }
    public void SetSystemsInDial(DialObjectData[] systemsInDial) { dialData.tab = systemsInDial; }
    #endregion

    #region Constructors
    void Awake()
    {
        string name = nameDial;

        if (name == "west")
        {
            dialData.tab = new DialObjectData[]
            {
                new DialObjectData(0, "red", false, 1),
                new DialObjectData(1, "yellow", false, 1),
                new DialObjectData(2, "green", false, 1),
                new DialObjectData(3, "radioactivity", false, 0),
                new DialObjectData(4, "radioactivity", false, 0)
            };
        }
            else if (name == "north")
            {
                dialData.tab = new DialObjectData[]
                {
                    new DialObjectData(0, "yellow", false, 2),
                    new DialObjectData(1, "red", false, 2),
                    new DialObjectData(2, "green", false, 2),
                    new DialObjectData(3, "yellow", false, 0),
                    new DialObjectData(4, "radioactivity", false, 0)
                };
            }
            else if (name == "south")
            {
                dialData.tab = new DialObjectData[]
                {
                    new DialObjectData(0, "green", false, 3),
                    new DialObjectData(1, "yellow", false, 3),
                    new DialObjectData(2, "red", false, 3),
                    new DialObjectData(3, "red", false, 0),
                    new DialObjectData(4, "radioactivity", false, 0)
                };
            }
            else if (name == "east")
            {
                dialData.tab = new DialObjectData[]
                {
                    new DialObjectData(0, "green", false, 2),
                    new DialObjectData(1, "yellow", false, 3),
                    new DialObjectData(2, "red", false, 1),
                    new DialObjectData(3, "radioactivity", false, 0),
                    new DialObjectData(4, "green", false, 0)
                };
            }

    }
    #endregion

    #region Test Functions
    // If every dial returns true, submarine takes a damage
    public bool DialFull()
    {
        bool result = false;
        for (int i = 0; i < dialData.tab.Length && !result; i++)
        {
            if (dialData.tab[i].failureFlag) { result = true; }
        }
        return result;
    }

    // Check if  all the radioactives object of this dial have suffered from failure. Returns a boolean
    bool ThisDialRadioactive()
    {
        bool result = true;
        for (int i = 0; i < dialData.tab.Length && result; i++)
        {
            if (dialData.tab[i].color == "radioactivity" && !dialData.tab[i].failureFlag)
            {
                result = false;
            }
        }
        return result;
    }

    // From Systems class, should check the color_system and see if you can activate it (check all dials)
    bool CheckColorFailure(string colorSystem)
    {
        bool result = false;
        for (int i = 0; i < dialData.tab.Length && !result; i++)
        {
            if (dialData.tab[i].color == colorSystem && dialData.tab[i].failureFlag)
            {
                result = true;
            }
        }
        return result; // True if failure, false if okay
    }

    // Should replace id by the unity game object/button on the board
    // (Can I cross that case?) Returns true if not yet failure
    bool CrossThatFailure(int id)
    {
        return !dialData.tab[id].failureFlag;
    }
    #endregion

    void CrossFailure(int id)
    {
        if (CrossThatFailure(id))
        {
            dialData.tab[id].failureFlag = true;
            // Do something to the board
        }
        // Else send a message that tells the engineer they already crossed that case
    }

    void ClearThePath(int path)
    {
        // If all cases of the path are crossed
        for (int i = 0; i < dialData.tab.Length; i++)
        {
            if (dialData.tab[i].pathIndex == path) dialData.tab[i].failureFlag = false;
        }
    }

    void ClearDial()
    {
        // If Surface (Do this to all dial
        for (int i = 0; i < dialData.tab.Length; i++)
        {
            dialData.tab[i].failureFlag = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
