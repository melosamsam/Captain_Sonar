using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Engineer_Dial : MonoBehaviour
{
    #region attributs
    public string nameDial;
    public object [,] systems; 
    //Format is [5,3] with a string, a bool and an int
    //string is either a system color or radioactivity, bool is whether there is failure, and int for the path on which the object is (if any) to cancel the failures
    #endregion

    #region Get
    public string GetNameDial() { return nameDial; }
    public object[,] GetSystemsInDial() { return systems; }
    #endregion

    #region Set
    public void SetNameDial(string name) { nameDial = name; }
    public void SetSystemsInDial(object[,] systemsInDial) { systems = systemsInDial; }
    #endregion

    #region Constructeurs
    void Awake()
    {
        string name = nameDial;

        if (name == "west")
        {
            systems = new object[5,3]
            {
                { "red", false, 1 },
                { "yellow", false, 1 },
                { "green", false, 1 },
                { "radioactivity", false, 0 },
                { "radioactivity", false, 0 }
            };
        }
        if (name == "north")
        {
            systems = new object[,]
            {
                { "yellow", false, 2 },
                { "red", false, 2 },
                { "green", false, 2 },
                { "yellow", false, 0 },
                { "radioactivity", false, 0 }
            };
        }
        if (name == "south")
        {
            systems = new object[,]
            {
                { "green", false, 3 },
                { "yellow", false, 3 },
                { "red", false, 3 },
                { "red", false, 0 },
                { "radioactivity", false, 0 }
            };
        }
        if (name == "east")
        {
            systems = new object[,]
            {
                { "green", false, 2},
                { "yellow", false, 3 },
                { "red", false, 1 },
                { "radioactivity", false, 0 },
                { "green", false, 0 }
            };
        }
    }
    #endregion


    #region Fonctions test
    public bool DialFull()
    {
        bool result = false;
        for (int i = 0;i < systems.GetLength(0) && !result; i++) 
        { 
            if ((bool)systems[i,1]==true) { result = true;}
        }
        return result;
    }
    
    /// Check if  all the radioactives object of this dial have suffered from failure. Returns a boolea
    bool ThisDialRadioactive()
    {
        bool result = true;
        for (int i=0; i<systems.GetLength(0) && result; i++)
        {
            if ((string)systems[i,0]=="radioactivity" && (bool)systems[i,1]==false)
            {
                result = false;
            }
        }
        return result;
    }

    bool CheckColorFailure(string color)
    {
        bool result = false;
        for (int i = 0; i < systems.GetLength(0) && !result; i++)
        {
            if ((string)systems[i, 0] == color && (bool)systems[i, 1] == true)
            {
                result = true;
            }
        }
        return result; //True if failure, false if okay
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
