using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineerDialData", menuName = "CaptainSonar/EngineerDialData")]
public class EngineerDialData : ScriptableObject
{
    public string name;
    public DialObjectData[] tab = new DialObjectData [5];
    
    #region Getters
    public string GetNameDial() { return name; }
    public DialObjectData[] GetSystemsInDial() { return tab; }
    #endregion

    #region Setters
    public void SetNameDial(string dialName) { name = dialName; }
    public void SetSystemsInDial(DialObjectData[] dialData) { tab = dialData; }
    #endregion

    #region Initialization
    private void OnEnable()
    {
        for (int i = 0; i < tab.Length; i++)
        {
            if (tab[i].failureFlag == true) tab[i].failureFlag = false;
        }
    }
    #endregion

    #region Test Functions
    // If every dial returns true, submarine takes a damage
    public bool DialFull()
    {
        bool result = true;
        for (int i = 0; i < tab.Length && result; i++)
        {
            if (!tab[i].failureFlag) { result = false; }
        }
        return result;
    }

    // Check if  all the radioactives object of this dial have suffered from failure. Returns a boolean
    public bool ThisDialRadioactive()
    {
        bool result = true;
        for (int i = 0; i < tab.Length && result; i++)
        {
            if (tab[i].color == "radioactivity" && !tab[i].failureFlag)
            {
                result = false;
            }
        }
        return result;
    }

    // (Can I cross that case?) Returns true if not yet failure
    public bool CrossThatFailure(int id)
    {
        return !tab[id].failureFlag;
    }
    #endregion

}

[System.Serializable]
public class DialObjectData
{
    public int index;
    public string color;
    public bool failureFlag;
    public int pathIndex;

    public DialObjectData(int index, string color, bool failureFlag, int pathIndex)
    {
        this.index = index;
        this.color = color;
        this.failureFlag = failureFlag;
        this.pathIndex = pathIndex;
    }
}